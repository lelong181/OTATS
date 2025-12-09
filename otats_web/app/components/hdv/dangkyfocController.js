angular.module('app').controller('dangkyfocController', function ($scope, $location, $state, $stateParams, $timeout, Notification, ComboboxDataService, hdvDataService) {
    CreateSiteMap();

    hideLoadingPage();



    $scope.flag = false;

    $scope.checkAccountCode = function (text) {
        if (text.length == 10) {
            Notification({ title: $.i18n('label_thongbao'), message: "Mã vé hợp lệ" }, "success");
        } else {
            Notification({ title: $.i18n('label_thongbao'), message: "Mã vé không hợp lệ" }, "error");
        }
    }

    $scope.register = function () {
        $scope.dataLoading = true;
        let model = {
            ID: 0,
            TenDayDu: $scope.tendaydu,
            Email: $scope.email,
            DienThoai: $scope.dienthoai,
            MaTheHDV: $scope.sothehdv,
            TenDangNhap: $scope.sothehdv,
            CCCD: $scope.cccd,
        }
        hdvDataService.save(model).then(function (result) {
            console.log(result);
            if (result !== null && result) {
                if (result.flag) {
                    $scope.message = "Thông tin của bạn đã được gửi lên hệ thống. Tài khoản của bạn sẽ được xác nhận và gửi về địa chỉ email đã đăng ký " + $scope.email + ". Cảm ơn!";
                    $scope.flag = true;
                    $scope.messageClass = 'alert-success';
                } else {
                    $scope.message = result.message;
                    $scope.flag = true;
                    $scope.messageClass = 'alert-danger';
                }

            } else {
                $scope.message = "Có lỗi trong quá trình gửi dữ liệu, vui lòng thử lại sau!";
                $scope.flag = true;
                $scope.messageClass = 'alert-danger';
            }
            $scope.dataLoading = false;

        });
    }
})