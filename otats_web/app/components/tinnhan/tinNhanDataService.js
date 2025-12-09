(function () {
    'use strict';
    angular
        .module('app')
        .factory('tinNhanDataService', tinNhanDataService);

    tinNhanDataService.$inject = ['$http', '$rootScope', '$timeout'];
    function tinNhanDataService($http, $rootScope, $timeout) {
        var service = {};

        service.baocaotinnhannhanvien = baocaotinnhannhanvien;
        service.excelbaocaotinnhannhanvien = excelbaocaotinnhannhanvien;
        service.baocaotinnhan = baocaotinnhan;
        service.excelbaocaotinnhan = excelbaocaotinnhan;
        service.getlist = getlist;
        service.getListNhomNhanVien = getListNhomNhanVien;
        service.guiTinNhan = guiTinNhan;

        service.baocaochitiettinnhan = baocaochitiettinnhan;

        return service;

        function baocaochitiettinnhan(idtinnhan) {
            return $http.get(urlApi + '/api/baocao/baocaochitiettinnhan?idtinnhan=' + idtinnhan).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function baocaotinnhannhanvien(idnhanvien, from, to) {
            return $http.get(urlApi + '/api/baocao/baocaotinnhannhanvien?idnhanvien=' + idnhanvien + '&from=' + from + '&to=' + to).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function excelbaocaotinnhannhanvien(idnhanvien, from, to) {
            return $http.get(urlApi + '/api/baocao/ExcelBaoCaoTinNhanNhanVien?idnhanvien=' + idnhanvien + '&from=' + from + '&to=' + to, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }
        function baocaotinnhan(idnhanvien, chuadoc, from, to) {
            return $http.get(urlApi + '/api/baocao/baocaotinnhan?idnhanvien=' + idnhanvien + '&chuadoc=' + chuadoc + '&from=' + from + '&to=' + to).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function excelbaocaotinnhan(idnhanvien, chuadoc, from, to) {
            return $http.get(urlApi + '/api/baocao/ExcelBaoCaoTinNhan?idnhanvien=' + idnhanvien + '&chuadoc=' + chuadoc + '&from=' + from + '&to=' + to, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }


        function getlist(kinhdo, vido, bankinh, trangthai, idnhom) {
            return $http.get(urlApi + '/api/guitinnhan/getlistnhanvien?KinhDo='
                + kinhdo + '&ViDo=' + vido + '&BanKinh=' + bankinh + '&TrangThai=' + trangthai + '&idNhom=' + idnhom).then(function (response) {
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

        function guiTinNhan(data) {
            return $http.post(urlApi + '/api/guitinnhan/guitinnhan', data).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

    }

})();