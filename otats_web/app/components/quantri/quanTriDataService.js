(function () {
    'use strict';

    angular
        .module('app')
        .factory('quanTriDataService', quanTriDataService);

    quanTriDataService.$inject = ['$http', '$cookies', '$rootScope', '$timeout'];
    function quanTriDataService($http, $cookies, $rootScope, $timeout) {
        var service = {};

        service.getbyid = getbyid;
        service.getlist = getlist;
        service.getListNhomNhanVien = getListNhomNhanVien;
        service.getListNhomNhanVienPhanQuyen = getListNhomNhanVienPhanQuyen;
        service.themquanly = themquanly;
        service.suaquanly = suaquanly;
        service.saveeditnhomnhanvien = saveeditnhomnhanvien;
        service.saveinsertnhomnhanvien = saveinsertnhomnhanvien;
        service.delnhomnhanvien = delnhomnhanvien;
        service.xoaquanly = xoaquanly;
        service.resetpass = resetpass;
        
        return service;

        function resetpass(_username, _newpass) {
            let obj = {
                username: _username,
                newpass: _newpass,
            }
            return $http.post(urlApi + '/api/nhanvienweb/resetPassword', obj).then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                    return { flag: false, message: 'label_khongthanhcongvuilonglienhequantri' }
            });
        }

        function saveinsertnhomnhanvien(obj) {
            return $http.post(urlApi + '/api/nhomnhanvien/insertNhom', obj).then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                    return { flag: false, message: 'label_khongthanhcongvuilonglienhequantri' }
            });
        }

        function saveeditnhomnhanvien(obj) {
            return $http.post(urlApi + '/api/nhomnhanvien/editNhom', obj).then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                    return { flag: false, message: 'label_khongthanhcongvuilonglienhequantri' }
            });
        }

        function delnhomnhanvien(id) {
            return $http.post(urlApi + '/api/nhomnhanvien/deleteNhom?ID_Nhom=' + id).then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                    return { flag: false, message: 'label_khongthanhcongvuilonglienhequantri' }
            });
        }

        function getbyid(id) {
            return $http.get(urlApi + '/api/nhanvienweb/getbyid?idtaikhoan=' + id).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: {} }
            });
        }

        function getlist(idnhom) {
            return $http.get(urlApi + '/api/nhanvienweb/getlist?idnhom=' + idnhom).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getListNhomNhanVien() {
            return $http.get(urlApi + '/api/nhomnhanvien/treenhom').then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getListNhomNhanVienPhanQuyen(idtaikhoan) {
            return $http.get(urlApi + '/api/nhanvienweb/treenhom?idtaikhoan=' + idtaikhoan).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function themquanly(data) {
            return $http.post(urlApi + '/api/nhanvienweb/themquanly', data).then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                    return { flag: false, message: 'label_khongthanhcongvuilonglienhequantri' }
            });
        }

        function suaquanly(data) {
            return $http.post(urlApi + '/api/nhanvienweb/suaquanly', data).then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                    return { flag: false, message: 'label_khongthanhcongvuilonglienhequantri' }
            });
        }

        function xoaquanly(data) {
            return $http.post(urlApi + '/api/nhanvienweb/xoaquanly', data).then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                    return { flag: false, message: 'label_khongthanhcongvuilonglienhequantri' }
            });
        }
    }

})();