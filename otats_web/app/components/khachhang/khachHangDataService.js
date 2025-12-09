(function () {
    'use strict';

    angular
        .module('app')
        .factory('khachHangDataService', khachHangDataService);

    khachHangDataService.$inject = ['$http', '$cookies', '$rootScope', '$timeout'];
    function khachHangDataService($http, $cookies, $rootScope, $timeout) {
        var service = {};
        service.getListNhomNhanVien = getListNhomNhanVien;
        service.getlist_nhanVien = getlist_nhanVien;

        service.getById = getById;
        service.save = save;
        service.del = del;
        service.delall = delall;
        service.getlist = getlist;
        service.exportExcel = exportExcel;
        service.taiFileMau = taiFileMau;
        service.importkhachhang = importkhachhang;
        service.uploadAnhDaiDien = uploadAnhDaiDien;

        service.getlistduyetxoa = getlistduyetxoa;
        service.huyxoa = huyxoa;
        service.duyetxoa = duyetxoa;
        service.chuyenquyen = chuyenquyen;
        service.savemulti = savemulti;

        return service;
        function chuyenquyen(idKhachHang, listID) {
            return $http.get(urlApi + '/api/duyetkhachhang/chuyenquyen?idKhachHang=' + idKhachHang + '&listid=' + listID).then(function (response) {
                return { flag: true, data: response.data, message: $.i18n('label_chuyenquyenthanhcong') }
            }, function (response) {
                    return { flag: false, data: [], message: $.i18n('label_chuyenquyenkhongthanhcongvuilongthulai') }
            });
        }

        function getlist_nhanVien(idnhom) {
            return $http.get(urlApi + '/api/nhanvienapp/getall?IdNhom=' + idnhom).then(function (response) {
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

        function getById(idkhachhang) {
            return $http.get(urlApi + '/api/khachhang/getbyid?ID=' + idkhachhang).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: {} }
            });
        }

        function getlist() {
            return $http.get(urlApi + '/api/khachhang/getall').then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function exportExcel(data) {
            return $http.post(urlApi + '/api/KhachHang/ExportExcelKhachHang', data, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }

        function taiFileMau() {
            return $http.get(urlApi + '/api/khachhang/ExportTeamplateKH', { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }

        function importkhachhang(fileUpload) {
            return $http.post(urlApi + '/api/khachhang/importkhachhang', { filename: fileUpload }, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }

        function getlistduyetxoa() {
            return $http.get(urlApi + '/api/duyetkhachhang/getdanhsach').then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function huyxoa(idKhachHang, trangThai) {
            return $http.get(urlApi + '/api/duyetkhachhang/huyyeucau?idKhachHang=' + idKhachHang + '&trangThai=' + trangThai).then(function (response) {
                return { flag: true, data: response.data, message: $.i18n('label_huyxoathanhcong') }
            }, function (response) {
                    return { flag: false, data: [], message: $.i18n('label_duyetxoakhongthanhcongvuilongthulai') }
            });
        }

        function duyetxoa(idKhachHang, trangThai) {
            return $http.get(urlApi + '/api/duyetkhachhang/duyetyeucau?idKhachHang=' + idKhachHang + '&trangThai=' + trangThai).then(function (response) {
                return { flag: true, data: response.data, message: $.i18n('label_duyetxoathanhcong') }
            }, function (response) {
                    return { flag: false, data: [], message: $.i18n('label_duyetxoakhongthanhcongvuilongthulai') }
            });
        }

        function save(obj) {
            return $http.post(urlApi + '/api/khachhang/themmoi', obj).then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_luukhachhangkhongthanhcongvuilonglienhequantri') }
            });
        }

        function savemulti(obj) {
            return $http.post(urlApi + '/api/khachhang/savemultidata', obj).then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                return { flag: false, message: $.i18n('label_luukhachhangkhongthanhcongvuilonglienhequantri') }
            });
        }

        function del(obj) {
            return $http.post(urlApi + '/api/khachhang/deleteKhachHang', obj).then(function (response) {
                return { flag: true, message: $.i18n('label_xoakhachhangthanhcong') }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_xoathatbaixinvuilongthulai') }
            });
        }

        function delall() {
            return $http.post(urlApi + '/api/khachhang/deleteall').then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                return { flag: false, message: $.i18n('label_xoathatbaixinvuilongthulai') }
            });
        }

        function uploadAnhDaiDien(data) {
            return $http.post(urlApi + '/api/uploadfile/savefile', data,
                {
                    transformRequest: angular.identity,
                    headers: { 'Content-Type': undefined }
                }).then(function (response) {
                    if (response != null) {
                        return { flag: true, url: response.data, message: $.i18n('label_taianhthanhcong') }
                    } else {
                        return { flag: false, url: '', message: $.i18n('label_taianhthatbaixinvuilongthulai') }
                    }
                }, function (response) {
                        return { flag: false, url: '', message: $.i18n('label_taianhthatbaixinvuilongthulai') }
                });
        }

    }

})();