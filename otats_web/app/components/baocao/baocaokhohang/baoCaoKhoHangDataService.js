(function () {
    'use strict';

    angular
        .module('app')
        .factory('baoCaoKhoHangDataService', baoCaoKhoHangDataService);

    baoCaoKhoHangDataService.$inject = ['$http', '$cookies', '$rootScope', '$timeout'];
    function baoCaoKhoHangDataService($http, $cookies, $rootScope, $timeout) {
        var service = {};

        service.getBaoCaoTon = getBaoCaoTon;
        service.getChiTietBaoCaoTon = getChiTietBaoCaoTon;
        service.getExcelBaoCaoTon = getExcelBaoCaoTon;

        service.getBaoCaoTonKho = getBaoCaoTonKho;
        service.getExcelBaoCaoTonKho = getExcelBaoCaoTonKho;
        service.getExcelBaoCaoTonChiTiet = getExcelBaoCaoTonChiTiet;


        return service;
        //bm015
        function getBaoCaoTon(idkhohang, idmathang, fromdate, todate, idloai) {
            return $http.get(urlApi + '/api/BaoCaoAPI/BaoCaoTongHopNhapXuatTon?id_KhoHang=' + idkhohang + '&id_MatHang=' + idmathang + '&fromdate=' + fromdate + '&todate=' + todate + '&id_Loai=' + idloai).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getChiTietBaoCaoTon(idkhohang, idmathang, fromdate, todate, idloai) {
            return $http.get(urlApi + '/api/BaoCaoAPI/BaoCaoTongHopNhapXuatTon_ChiTiet?id_KhoHang=' + idkhohang + '&id_MatHang=' + idmathang + '&fromdate=' + fromdate + '&todate=' + todate + '&id_LoaiBienDong=0'+ '&id_Loai=' + idloai).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelBaoCaoTon(idkhohang, idmathang, fromdate, todate, idloai) {
            return $http.get(urlApi + '/api/ExportReport/ExcelBaoCaoTongHopNhapXuatTon?id_KhoHang=' + idkhohang + '&id_MatHang=' + idmathang + '&fromdate=' + fromdate + '&todate=' + todate + '&id_Loai=' + idloai, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }
        function getExcelBaoCaoTonChiTiet(idkhohang, idmathang, fromdate, todate, idloai) {
            return $http.get(urlApi + '/api/ExportReport/ExcelBaoCaoTongHopNhapXuatTonChiTiet?id_KhoHang=' + idkhohang + '&id_MatHang=' + idmathang + '&fromdate=' + fromdate + '&todate=' + todate +'&id_LoaiBienDong=0' + '&id_Loai=' + idloai, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }
        //bm014
        function getBaoCaoTonKho(idkhohang, idmathang, fromdate, todate, idloai) {
            return $http.get(urlApi + '/api/BaoCaoAPI/BaoCaoTongHopNhapXuatTonCacKho?id_KhoHang=' + idkhohang + '&id_MatHang=' + idmathang + '&fromdate=' + fromdate + '&todate=' + todate + '&id_Loai=' + idloai).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelBaoCaoTonKho(idkhohang, idmathang, fromdate, todate, idloai) {
            return $http.get(urlApi + '/api/ExportReport/ExcelBaoCaoTongHopNhapXuatTonCacKho?id_KhoHang=' + idkhohang + '&id_MatHang=' + idmathang + '&fromdate=' + fromdate + '&todate=' + todate + '&id_Loai=' + idloai, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }

    }

})();
