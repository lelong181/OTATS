(function () {
    'use strict';

    angular
        .module('app')
        .factory('quanLyXeDataService', quanLyXeDataService);

    quanLyXeDataService.$inject = ['$http', '$cookies', '$rootScope', '$timeout'];
    function quanLyXeDataService($http, $cookies, $rootScope, $timeout) {
        var service = {}; 

        
        service.getlist = getlist;
        service.getbyid = getbyid;
        service.exportExcel = exportExcel;

        service.themsuaxe = themsuaxe;
        service.xoaxe = xoaxe;

        service.lichsubaoduong = lichsubaoduong;
        service.lichsusudung = lichsusudung;
        
        return service;

        function getbyid(id) {
            return $http.get(urlApi + '/api/xe/getbyid?ID=' + id).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getlist() {
            return $http.get(urlApi + '/api/xe/getalldata').then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function exportExcel() {
            return $http.get(urlApi + '/api/xe/ExportExcelXe', { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }
        function themsuaxe(obj) {
            if (obj.ID_Xe > 0) {
                return $http.post(urlApi + '/api/xe/update', obj).then(function (response) {
                    if (response.data.success) {
                        return { flag: true, message: response.data.msg }
                    } else {
                        return { flag: false, message: response.data.msg }
                    }
                }, function (response) {
                        return { flag: false, message: $.i18n('label_luuthongtinxekhongthanhcongvuilonglienhequantri') }
                });
            } else {
                return $http.post(urlApi + '/api/xe/create', obj).then(function (response) {
                    if (response.data.success) {
                        return { flag: true, message: response.data.msg }
                    } else {
                        return { flag: false, message: response.data.msg }
                    }
                }, function (response) {
                        return { flag: false, message: $.i18n('label_luuthongtinxekhongthanhcongvuilonglienhequantri') }
                });
            }
            
        }
        function xoaxe(obj) {
            return $http.post(urlApi + '/api/xe/delete', obj).then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_xoaxethatbaivuilonglienhequantri') }
            });
        }
        function lichsubaoduong(id) {
            return $http.get(urlApi + '/api/xe/getlichbaoduong?ID='+id).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function lichsusudung(id) {
            return $http.get(urlApi + '/api/xe/getlichsudung?ID=' + id).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
       
    }

})();