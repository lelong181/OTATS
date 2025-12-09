angular.module('app').controller('checkInMaQrController', checkInMaQrController);
function checkInMaQrController($rootScope, $scope, $state, Notification, checkInMaQrDataService, ComboboxDataService) {
    CreateSiteMap();
    hideLoadingPage();

    $scope.manualCode = "";
    $scope.isCameraOpen = false;
    $scope.ticketResult = null;
    $scope.errorMessage = null;
    let html5QrCode;

    function init() {
        setTimeout(function () {
            let input = document.getElementById("manualInput");
            if (input) input.focus();
        }, 500);
    }

    $scope.handleManualInput = function (event) {
        if (event.which === 13) {
            let code = $scope.manualCode;
            if (code) {
                checkTicket(code, 'Desktop');
                $scope.manualCode = "";
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
        checkTicket(decodedText, 'Mobile');

    }

    function onScanFailure(error) {
    }

    function checkTicket(code, type) {
        $scope.ticketResult = null;
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
                        status: ticket.statusStr,
                        code: ticket.serviceCode,
                        bookingCode: ticket.bookingCode,
                        invoiceCode: ticket.invoiceCode
                    };
                    Notification.success("Quét vé thành công!");
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
