(function () {
    'use strict';

    angular
        .module('app')
        .factory('baoCaoDonHangDataService', baoCaoDonHangDataService);

    baoCaoDonHangDataService.$inject = ['$http', '$cookies', '$rootScope', '$timeout'];
    function baoCaoDonHangDataService($http, $cookies, $rootScope, $timeout) {
        var service = {};

        service.getTongHopDonHang = getTongHopDonHang;
        service.getExcelTongHopDonHang = getExcelTongHopDonHang;

        service.getDonHangTheoNhanVien = getDonHangTheoNhanVien;
        service.getExcelDonHangTheoNhanVien = getExcelDonHangTheoNhanVien;

        service.getBaoCaoDoanhThu = getBaoCaoDoanhThu;
        service.getExcelBaoCaoDoanhThu = getExcelBaoCaoDoanhThu;

        return service;
        
        function getTongHopDonHang(fromdate, todate) {
            return $http.get(urlApi + '/api/baocao/BaoCaoTongHopDonHang?fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelTongHopDonHang(fromdate, todate) {
            return $http.get(urlApi + '/api/baocao/BaoCaoTongHopDonHangReport?fromdate=' + fromdate + '&todate=' + todate, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }

        function getDonHangTheoNhanVien(idnhanvien, fromdate, todate) {
            return $http.get(urlApi + '/api/baocao/BaoCaoDonHangTheoNhanVienNew?idnv=' + idnhanvien+ '&fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelDonHangTheoNhanVien(idnhanvien, fromdate, todate) {
            return $http.get(urlApi + '/api/baocao/BaoCaoDonHangTheoNhanVienReport?idnv=' + idnhanvien + '&fromdate=' + fromdate + '&todate=' + todate, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }

        function getBaoCaoDoanhThu(idnhanvien,idkhachhang, fromdate, todate) {
            return $http.get(urlApi + '/api/baocao/BaocaoDoanhThuNew?idnv=' + idnhanvien + '&idkh=' + idkhachhang + '&fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelBaoCaoDoanhThu(idnhanvien,idkhachhang, fromdate, todate) {
            return $http.get(urlApi + '/api/baocao/BaocaoDoanhThuReport?idnv=' + idnhanvien + '&idkh=' + idkhachhang + '&fromdate=' + fromdate + '&todate=' + todate, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }
    }

})();
