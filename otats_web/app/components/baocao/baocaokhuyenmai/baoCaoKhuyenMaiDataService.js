(function () {
    'use strict';

    angular
        .module('app')
        .factory('baoCaoKhuyenMaiDataService', baoCaoKhuyenMaiDataService);

    baoCaoKhuyenMaiDataService.$inject = ['$http'];
    function baoCaoKhuyenMaiDataService($http) {
        var service = {};

        service.getBaoCaoTongHopChuongTrinhKhuyenMai = getBaoCaoTongHopChuongTrinhKhuyenMai;
        service.getExcelBaoCaoTongHopChuongTrinhKhuyenMai = getExcelBaoCaoTongHopChuongTrinhKhuyenMai;

        service.getBaoCaoChiTietChuongTrinhKhuyenMai = getBaoCaoChiTietChuongTrinhKhuyenMai;
        service.getExcelBaoCaoChiTietChuongTrinhKhuyenMai = getExcelBaoCaoChiTietChuongTrinhKhuyenMai;


        return service;
        
        function getBaoCaoTongHopChuongTrinhKhuyenMai(idctkm, idkho, idnhom) {
            return $http.get(urlApi + '/api/baocao/tonghopchuongtrinhkhuyenmai?idctkm=' + idctkm + '&idkho=' + idkho + '&idnhom=' + idnhom).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelBaoCaoTongHopChuongTrinhKhuyenMai(idctkm, idkho, idnhom) {
            return $http.get(urlApi + '/api/ExportReport/ExcelBaoCaoTongHopChuongTrinhKhuyenMai?idctkm=' + idctkm + '&idkho=' + idkho + '&idnhom=' + idnhom, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }
        
        function getBaoCaoChiTietChuongTrinhKhuyenMai(idctkm, idkho, idnhanvien, idhang) {
            return $http.get(urlApi + '/api/baocao/chitietchuongtrinhkhuyenmai?idctkm=' + idctkm + '&idkho=' + idkho + '&idnhanvien=' + idnhanvien + '&idhang=' + idhang).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelBaoCaoChiTietChuongTrinhKhuyenMai(idctkm, idkho, idnhanvien, idhang) {
            return $http.get(urlApi + '/api/ExportReport/ExcelBaoCaoChiTietChuongTrinhKhuyenMai?idctkm=' + idctkm + '&idkho=' + idkho + '&idnhanvien=' + idnhanvien + '&idhang=' + idhang, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }

    }

})();
