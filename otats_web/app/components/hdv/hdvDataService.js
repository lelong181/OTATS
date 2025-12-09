(function () {
    'use strict';

    angular
        .module('app')
        .factory('hdvDataService', hdvDataService);

    hdvDataService.$inject = ['$http', '$cookies', '$rootScope', '$timeout'];
    function hdvDataService($http, $cookies, $rootScope, $timeout) {
        var service = {};

        service.getbymathe = getbymathe;
        service.saveedit = saveedit;
        service.save = save;
        service.activeHDV = activeHDV;
        service.inactiveHDV = inactiveHDV;
        service.del = del;
        service.resetpass = resetpass;
        service.getlist = getlist;

        return service;

        function getbymathe(mathe) {
            return $http.get(urlApi + '/api/huongdanvien/getbymatthe?mathe=' + mathe).then(function (response) {
                if (response.data)
                    return { flag: true, data: response.data }
                else
                    return { flag: false, data: {} }
            }, function (response) {
                return { flag: false, data: {} }
            });
        }

        function getlist(idnhom) {
            return $http.get(urlApi + '/api/huongdanvien/getall').then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function resetpass(idnhanvien, newpass) {
            let obj = {
                id: idnhanvien,
                newpass: newpass,
            }
            return $http.post(urlApi + '/api/userinfo/resetPassword', obj).then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                return { flag: false, message: $.i18n('label_khongthanhcongvuilonglienhequantri') }
            });
        }

        function saveedit(obj) {
            return $http.post(urlApi + '/api/huongdanvien/suanhanvien', obj).then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                return { flag: false, message: $.i18n('label_luunhanvienkhongthanhcongvuilonglienhequantri') }
            });
        }

        function save(obj) {
            return $http.post(urlApi + '/api/huongdanvien/themmoi', obj).then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                return { flag: false, message: $.i18n('label_luunhanvienkhongthanhcongvuilonglienhequantri') }
            });
        }

        function del(lisdid) {
            return $http.post(urlApi + '/api/huongdanvien/xoa?ID=' + lisdid).then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                return { flag: false, message: $.i18n('label_khongthanhcongvuilonglienhequantri') }
            });
        }
        function activeHDV(lisdid) {
            return $http.post(urlApi + '/api/huongdanvien/kichhoat_hdv?ID=' + lisdid).then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                return { flag: false, message: $.i18n('label_khongthanhcongvuilonglienhequantri') }
            });
        }
        function inactiveHDV(lisdid) {
            return $http.post(urlApi + '/api/huongdanvien/khoa_hdv?ID=' + lisdid).then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                return { flag: false, message: $.i18n('label_khongthanhcongvuilonglienhequantri') }
            });
        }
    }

})();