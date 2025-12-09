
(function () {
    'use strict';
    angular
        .module('app')
        .factory('congViecDataService', congViecDataService);

    congViecDataService.$inject = ['$http', '$rootScope', '$timeout'];
    function congViecDataService($http, $rootScope, $timeout) {
        var service = {};
        service.getListCongViec = getListCongViec;
        service.addCongViec = addCongViec;
        service.getChiTietCongViec = getChiTietCongViec;
        service.getChiTietTrangThaiCongViec = getChiTietTrangThaiCongViec;

        service.getListNhomNhanVien = getListNhomNhanVien;
        service.getlistapibyidnhom = getlistapibyidnhom;
        service.getlistnhanvienbyidnhom = getlistnhanvienbyidnhom;
        service.addapi = addapi;
        service.addapinhom = addapinhom;
        service.editapi = editapi;
        service.deleteapi = deleteapi;
        service.uploadmultifile = uploadmultifile;
        service.getlistnhanvienbymultiidnhom = getlistnhanvienbymultiidnhom;
        
        return service;

        function addCongViec(obj) {
            return $http.post(urlApi + '/api/congviec/create', obj).then(function (response) {
                if (response.data.success)
                    return { flag: true, message: response.data.msg }
                else
                    return { flag: false, message: response.data.msg }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_themcongvieckhongthanhcongvuilonglienhequantri') }
            });
        }
        function getListCongViec() {
            return $http.get(urlApi + '/api/congviec/getlist_grid').then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getChiTietCongViec(idcongviec) {
            return $http.get(urlApi + '/api/congviec/getchitietcongviec?idcongviec=' + idcongviec).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getChiTietTrangThaiCongViec(idcongviec) {
            return $http.get(urlApi + '/api/congviec/getchitiettrangthaicongviec?idcongviec=' + idcongviec).then(function (response) {
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

        function getlistapibyidnhom(idnhom) {
            return $http.get(urlApi + '/api/kpi/getbyidnhom?idnhom=' + idnhom).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getlistnhanvienbymultiidnhom(data) {
            return $http.post(urlApi + '/api/nhanvienapp/getallbynhom', data).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getlistnhanvienbyidnhom(idnhom) {
            return $http.get(urlApi + '/api/phienlamviec/getlistnhanvien?idNhom=' + idnhom).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function addapi(obj) {
            return $http.post(urlApi + '/api/kpi/add', obj).then(function (response) {
                return { flag: true, message: $.i18n('label_luukpithanhcong') }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_luukpikhongthanhcongvuilongthulai') }
            });
        }

        function addapinhom(obj) {
            return $http.post(urlApi + '/api/kpi/addbynhom', obj).then(function (response) {
                return { flag: true, message: response.data.msg }
            }, function (response) {
                return { flag: false, message: $.i18n('label_luukpikhongthanhcongvuilongthulai') }
            });
        }

        function editapi(obj) {
            return $http.post(urlApi + '/api/kpi/update', obj).then(function (response) {
                return { flag: true, message: $.i18n('label_luukpithanhcong') }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_luukpikhongthanhcongvuilongthulai') }
            });
        }

        function deleteapi(listid) {
            return $http.post(urlApi + '/api/kpi/deletemulti?id=' + listid).then(function (response) {
                return { flag: true, message: $.i18n('label_xoakpithanhcong') }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_xoakpikhongthanhcongvuilongthulai') }
            });
        }

        function uploadmultifile(data) {
            return $http.post(urlApi + '/api/uploadfile/savemultifile', data, {
                headers: { 'Content-Type': undefined }
            }).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
    }

})();