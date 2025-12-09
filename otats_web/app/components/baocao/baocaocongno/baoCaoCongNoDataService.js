(function () {
    'use strict';

    angular
        .module('app')
        .factory('baoCaoCongNoDataService', baoCaoCongNoDataService);

    baoCaoCongNoDataService.$inject = ['$http', '$cookies', '$rootScope', '$timeout'];
    function baoCaoCongNoDataService($http, $cookies, $rootScope, $timeout) {
        var service = {};

        service.getBaoCaoTheoDoiCongNo = getBaoCaoTheoDoiCongNo;
        service.getExcelBaoCaoTheoDoiCongNo = getExcelBaoCaoTheoDoiCongNo;

        service.getBaoCaoThuHoiCongNo = getBaoCaoThuHoiCongNo;
        service.getChiTietBaoCaoThuHoiCongNo = getChiTietBaoCaoThuHoiCongNo;
        service.getExcelBaoCaoThuHoiCongNo = getExcelBaoCaoThuHoiCongNo;

        return service;
        //bm001
        function getBaoCaoTheoDoiCongNo(idkhachhang, idnhanvien, fromdate, todate) {
            return $http.get(urlApi + '/api/BaoCaoAPI/BaoCaoCongNo?ID_KhachHang=' + idkhachhang + '&id_NhanVien=' + idnhanvien + '&fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelBaoCaoTheoDoiCongNo(idkhachhang, idnhanvien, fromdate, todate) {
            return $http.get(urlApi + '/api/ExportReport/ExcelBaoCaoCongNo?ID_KhachHang=' + idkhachhang + '&id_NhanVien=' + idnhanvien + '&fromdate=' + fromdate + '&todate=' + todate, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }
        //bm002
        function getBaoCaoThuHoiCongNo(idkhachhang, idnhanvien, fromdate, todate) {
            return $http.get(urlApi + '/api/BaoCaoAPI/BaoCaoThuHoiCongNo?ID_KhachHang=' + idkhachhang + '&id_NhanVien=' + idnhanvien + '&fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getChiTietBaoCaoThuHoiCongNo(idkhachhang, idnhanvien, fromdate, todate) {
            return $http.get(urlApi + '/api/BaoCaoAPI/BaoCaoThuHoiCongNo_ChiTiet?ID_KhachHang=' + idkhachhang + '&id_NhanVien=' + idnhanvien + '&fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelBaoCaoThuHoiCongNo(idkhachhang, idnhanvien, fromdate, todate) {
            return $http.get(urlApi + '/api/ExportReport/ExcelBaoCaoThuHoiCongNo?ID_KhachHang=' + idkhachhang + '&id_NhanVien=' + idnhanvien + '&fromdate=' + fromdate + '&todate=' + todate, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }
        
    }

})();
