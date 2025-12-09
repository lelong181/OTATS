(function () {
    'use strict';

    angular
        .module('app')
        .factory('hanMucDataService', hanMucDataService);

    hanMucDataService.$inject = ['$http', '$rootScope', '$timeout'];
    function hanMucDataService($http, $rootScope, $timeout) {
        var service = {};

        service.getdatakhachhang = getdatakhachhang;
        service.setnguongkhachhang = setnguongkhachhang;
        service.getListNhomNhanVien = getListNhomNhanVien;
        service.getdatanhanvien = getdatanhanvien;
        service.setnguongnhanvien = setnguongnhanvien;
        service.getdataloaikhachhang = getdataloaikhachhang;
        service.setnguongloaikhachhang = setnguongloaikhachhang;
        service.getdatanhomnhanvien = getdatanhomnhanvien;
        service.setnguongnhomnhanvien = setnguongnhomnhanvien;
        
        return service;

        function getdatakhachhang(idtinh, idquan, idloai) {
            return $http.get(URL_CONGNO_GETLISTKHACHHANG + '?idtinh=' + idtinh + '&idquan=' + idquan + '&idloai=' + idloai).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function setnguongkhachhang(idkhachhang, nguong) {
            return $http.post(URL_CONGNO_SETNGUONGCONGNOKHACHHANG + '?idkhachhang=' + idkhachhang + '&nguong=' + nguong).then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_thietlapnguongkhongthanhcongvuilonglienhequantri') }
            });
        }

        function getListNhomNhanVien() {
            return $http.get(URL_COMBOBOX_GETTREENHOMNHANVIEN).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getdatanhanvien(idnhom) {
            return $http.get(URL_CONGNO_GETLISTNHANVIENBYIDNHOM + '?idnhom=' + idnhom).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function setnguongnhanvien(idnhanvien, nguong) {
            return $http.post(URL_CONGNO_SETNGUONGCONGNONHANVIEN + '?idnhanvien=' + idnhanvien + '&nguong=' + nguong).then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                return { flag: false, message: $.i18n('label_thietlapnguongkhongthanhcongvuilonglienhequantri') }
            });
        }

        function getdataloaikhachhang() {
            return $http.get(URL_CONGNO_GETLISTLOAIKHACHHANG).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function setnguongloaikhachhang(idloaikhachhang, nguong) {
            return $http.post(URL_CONGNO_SETNGUONGCONGNOLOAIKHACHHANG + '?idloaikhachhang=' + idloaikhachhang + '&nguong=' + nguong).then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                return { flag: false, message: $.i18n('label_thietlapnguongkhongthanhcongvuilonglienhequantri') }
            });
        }

        function getdatanhomnhanvien() {
            return $http.get(URL_CONGNO_GETLISTNHOMNHANVIEN).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function setnguongnhomnhanvien(idnhom, nguong) {
            return $http.post(URL_CONGNO_SETNGUONGCONGNONHOMNHANVIEN + '?idnhom=' + idnhom + '&nguong=' + nguong).then(function (response) {
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