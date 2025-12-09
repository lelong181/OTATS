(function () {
    'use strict';

    angular
        .module('app')
        .factory('baoCaoKeHoachDataService', baoCaoKeHoachDataService);

    baoCaoKeHoachDataService.$inject = ['$http', '$cookies', '$rootScope', '$timeout'];
    function baoCaoKeHoachDataService($http, $cookies, $rootScope, $timeout) {
        var service = {};

        service.getBaoCaoKeHoachNhanVien = getBaoCaoKeHoachNhanVien;
        service.xoaBaoCaoKeHoach = xoaBaoCaoKeHoach;
        service.luuBaoCaoKeHoach = luuBaoCaoKeHoach;
        service.suaBaoCaoKeHoach = suaBaoCaoKeHoach;

        return service;
        function getBaoCaoKeHoachNhanVien(data) {
            return $http.post(urlApi + '/api/baocaokehoachnhanvien/getdatabaocao',data).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function xoaBaoCaoKeHoach(data) {
            return $http.post(urlApi + '/api/baocaokehoachnhanvien/xoakehoach_v1', data).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });

        }
        function luuBaoCaoKeHoach(obj) {
            return $http.get(urlApi + '/api/kehoachnhanvien/create?' + obj).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });

        }

        function suaBaoCaoKeHoach(obj) {
            return $http.get(urlApi + '/api/kehoachnhanvien/update?' + obj).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });

        }

    }

})(); 
