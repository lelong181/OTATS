(function () {
    'use strict';


    angular
        .module('app')
        .factory('keHoachTuyenKhachHangDataService', keHoachTuyenKhachHangDataService);

    keHoachTuyenKhachHangDataService.$inject = ['$http', '$cookies', '$rootScope', '$timeout'];
    function keHoachTuyenKhachHangDataService($http, $cookies, $rootScope, $timeout) {
        var service = {};

        service.themSuaTuyen = themSuaTuyen;
        service.getExcelTuyenKhachHang = getExcelTuyenKhachHang;

        return service;
       
        function themSuaTuyen(obj) {
            return $http.post(urlApi + '/api/tuyenkhachhang/themsua', obj).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });

        }

        function getExcelTuyenKhachHang() {
            return $http.get(urlApi + '/api/tuyenkhachhang/ExcelTuyenKhachHang', { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }

    }

})(); 
