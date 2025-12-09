/// <reference path="../libs/angularjs/angular.min1.7.2.js" />

'use strict';

angular
    .module('app', [])
    .controller('dangkyhdvController', dangkyhdvController);

dangkyhdvController.$inject = ['$rootScope', '$scope', '$http', '$location', 'Notification', 'hdvDataService'];
function dangkyhdvController($rootScope, $scope, $http, $location, Notification, hdvDataService) {


    (function initController() {
        hideLoadingPage();
        $scope.flag = false;

    })();

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
}