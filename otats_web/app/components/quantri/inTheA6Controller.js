var list = [];
angular.module('app').controller('inTheA6Controller', function ($location, $timeout, $rootScope, $scope, Notification, ComboboxDataService, quanTriDataService) {
    $scope.list = $rootScope.listInA6;
    list = $scope.list;
    setTimeout(makeQR, 5000);
})

function makeQR() {
    $(".loadding-gif").hide();
    var QR_CODEX = [];
    $.each(list, function (index, item) {
        var QR_CODEX = new QRCode("qrcode" + index, {
            width: 230,
            height: 230,
            colorDark: "#000000",
            colorLight: "#ffffff",
            correctLevel: QRCode.CorrectLevel.H,
        });
        QR_CODEX.makeCode("https://tourshopping.vn?ref=" + item.iD_QuanLy);

    })
}