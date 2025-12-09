(function () {
    'use strict';

    angular
        .module('app')
        .factory('layoutService', layoutService);

    layoutService.$inject = ['$http', '$cookies', '$rootScope', '$timeout'];
    function layoutService($http, $cookies, $rootScope, $timeout) {
        var service = {};

        service.LoadNotiDonHang = LoadNotiDonHang;
        service.LoadNotiHoatDong = LoadNotiHoatDong;
        service.LoadNotiTinNhan = LoadNotiTinNhan;
        service.clearnoti = clearnoti;

        return service;

        function clearnoti(type) {
            return $http.get(urlApi + '/api/noti/clearnoti?type=' + type).then(function (response) {
                let data = response.data.data;
                if (data != null && data.length > 0)
                    return { flag: true, data: data }
                else
                    return { flag: false, data: [] }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function LoadNotiDonHang() {
            return $http.get(urlApi + '/api/noti/canhbaodonhangmoi?type=donhangmoi').then(function (response) {
                let data = response.data.data;
                if (data != null && data.length > 0)
                    return { flag: true, data: data }
                else
                    return { flag: false, data: [] }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function LoadNotiHoatDong() {
            return $http.get(urlApi + '/api/noti/canhbaonewfeed?type=newfeed').then(function (response) {
                let data = response.data.data;
                if (data != null && data.length > 0)
                    return { flag: true, data: data }
                else
                    return { flag: false, data: [] }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function LoadNotiTinNhan() {
            return $http.get(urlApi + '/api/noti/canhbaotinnhanmoi?type=tinnhanmoi').then(function (response) {
                let data = response.data.data;
                if (data != null && data.length > 0)
                    return { flag: true, data: data }
                else
                    return { flag: false, data: [] }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

    }

})();