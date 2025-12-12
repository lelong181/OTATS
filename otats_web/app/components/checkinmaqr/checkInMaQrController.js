angular.module('app').controller('checkInMaQrController', checkInMaQrController);
function checkInMaQrController($rootScope, $scope, $state, Notification, checkInMaQrDataService, ComboboxDataService, $timeout) {
    CreateSiteMap();
    hideLoadingPage();

    $scope.manualCode = "";
    $scope.isCameraOpen = false;
    $scope.ticketResult = null;
    $scope.errorMessage = null;
    $scope.checkInMode = 'SCAN'; // Mặc định là chế độ Scan vé
    let html5QrCode;

    function init() {
        setTimeout(function () {
            let input = document.getElementById("manualInput");
            if (input) input.focus();
        }, 500);
    }

    $scope.changeMode = function () {
        $scope.ticketResult = null;
        $scope.ticketHistory = [];
        $scope.errorMessage = null;
        $scope.scanMessage = null;
        $scope.scanStatus = null;
        $scope.showScanResult = false;
        $scope.manualCode = "";
        let input = document.getElementById("manualInput");
        if (input) input.focus();
    };

    $scope.handleManualInput = function (event) {
        if (event.which === 13) {
            let code = $scope.manualCode;
            if (code) {
                if ($scope.checkInMode === 'SCAN') {
                    // Nếu đang ở chế độ Scan vé -> Gọi hàm sử dụng vé luôn
                    $scope.usingTicket(code);
                } else {
                    checkTicket(code);
                }
                $scope.manualCode = "";
                let input = document.getElementById("manualInput");
                if (input) input.blur();
            }
        }
    };

    $scope.toggleCamera = function () {
        if ($scope.isCameraOpen) {
            stopCamera();
        } else {
            startCamera();
        }
    };

    $scope.usingTicket = function (code) {
        if (code) {
            checkInMaQrDataService.usingTicket(code, 'Mobile_SonTQ').then(function (result) {
                if (result.flag && result.data && result.data.data && result.data.data.status === 'SUCCESS') {
                    $scope.scanMessage = $scope.serverProcessMessage || "Sử dụng vé thành công!";
                    $scope.scanStatus = 'SUCCESS';
                    $scope.showScanResult = true;
                    $timeout(function () {
                        $scope.closeScanResult();
                    }, 1000);
                } else {
                    $scope.scanMessage = (result.data && result.data.data && result.data.data.message) ? result.data.data.message : ($scope.serverProcessMessage || "Lỗi khi sử dụng vé.");
                    $scope.scanStatus = 'ERROR';
                    $scope.showScanResult = true;
                }
                checkTicket(code);
            });
        }
    };

    $scope.closeScanResult = function () {
        $scope.showScanResult = false;
        $scope.scanMessage = null;
        $scope.scanStatus = null;
        $scope.scanStatus = null;
        // let input = document.getElementById("manualInput");
        // if (input) input.focus();
    };

    function startCamera() {
        $scope.isCameraOpen = true;

        html5QrCode = new Html5Qrcode("reader");
        let config = { fps: 10, qrbox: { width: 250, height: 250 } };

        html5QrCode.start({ facingMode: "environment" }, config, onScanSuccess, onScanFailure)
            .catch(err => {
                console.error("Error starting camera", err);
                $scope.errorMessage = "Không thể mở camera: " + err;
                $scope.isCameraOpen = false;
                $scope.$apply();
            });
    }

    function stopCamera() {
        if (html5QrCode) {
            html5QrCode.stop().then((ignore) => {
                html5QrCode.clear();
                $scope.isCameraOpen = false;
                $scope.$apply();
            }).catch((err) => {
                console.error("Failed to stop camera", err);
            });
        }
    }

    function onScanSuccess(decodedText, decodedResult) {
        console.log(`Code matched = ${decodedText}`, decodedResult);
        if ($scope.checkInMode === 'SCAN') {
            $scope.usingTicket(decodedText);
        } else {
            checkTicket(decodedText);
        }
        stopCamera();
    }

    function onScanFailure(error) {
    }

    function checkTicket(code) {
        $scope.ticketResult = null;
        $scope.ticketHistory = [];
        $scope.errorMessage = null;

        var payload = {
            code: code,
            type: 'TICKET',
            langCode: 'vi'
        };
        checkInMaQrDataService.checkTicket(payload).then(function (result) {
            if (result.flag) {
                if (result.data && result.data.value && result.data.value.ticket && result.data.value.ticket.length > 0) {
                    var ticket = result.data.value.ticket[0];
                    $scope.ticketResult = {
                        statusStr: ticket.statusStr,
                        status: ticket.status,
                        code: ticket.serviceCode,
                        bookingCode: ticket.bookingCode,
                        invoiceCode: ticket.invoiceCode,
                        ticketName: ticket.serviceRateName,
                        expirationDate: ticket.expirationDate,
                        lastUsingTime: ticket.lastUsingTime,
                        lastUsingACM: ticket.lastUsingACM
                    };

                    $scope.ticketHistory = [];
                    if (result.data.value.tcp && result.data.value.tcp.length > 0) {
                        $scope.ticketHistory = result.data.value.tcp.filter(function (item) {
                            return item.server_CommandResultID === 'SERVER_PROCESS_SUCCESS';
                        });
                        console.log($scope.ticketHistory);
                    }
                } else {
                    $scope.errorMessage = "Không tìm thấy thông tin vé.";
                    Notification.error($scope.errorMessage);
                }
            } else {
                $scope.errorMessage = result.message || "Không tìm thấy vé hoặc có lỗi xảy ra.";
                Notification.error($scope.errorMessage);
            }
        });
    }

    $scope.$on('$destroy', function () {
        if ($scope.isCameraOpen) {
            stopCamera();
        }
    });

    init();
}
