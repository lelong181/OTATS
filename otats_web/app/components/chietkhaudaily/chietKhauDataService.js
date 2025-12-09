(function () {
    'use strict';

    angular
        .module('app')
        .factory('chietKhauDataService', chietKhauDataService);

    chietKhauDataService.$inject = ['$http', '$rootScope', '$timeout'];
    function chietKhauDataService($http, $rootScope, $timeout) {
        var service = {};

        service.getdatanhomnhanvien = getdatanhomnhanvien;
        service.sethoahongnhomnhanvien = sethoahongnhomnhanvien;

        
        return service;


        function getdatanhomnhanvien() {
            return $http.get(urlApi + '/api/nhomnhanvien/getlistchietkhau').then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function sethoahongnhomnhanvien(idnhom, hoahong) {
            return $http.post(urlApi + '/api/nhomnhanvien/setchietkhau?idnhom=' + idnhom + '&hoahong=' + hoahong).then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                return { flag: false, message: $.i18n('label_thietlapnguongkhongthanhcongvuilonglienhequantri') }
            });
        }
        
    }

})();