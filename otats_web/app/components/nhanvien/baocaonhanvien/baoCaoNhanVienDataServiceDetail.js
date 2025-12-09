(function () {
    'use strict';

    angular
        .module('app')
        .factory('NhanVienDetail', baoCaoNhanVienDataServiceDetail);

    baoCaoNhanVienDataServiceDetail.$inject = ['$http', '$cookies', '$rootScope', '$timeout'];
    function baoCaoNhanVienDataServiceDetail($http, $cookies, $rootScope, $timeout) {
        var service = {};
        service.getCheckList = getCheckList;

        service.getPhienLamViec = getPhienLamViec;
        service.getExcelPhienLamViec = getExcelPhienLamViec;
        service.getKMDiChuyen = getKMDiChuyen;

        service.getBaoCaoKhongKetNoi = getBaoCaoKhongKetNoi;
        service.getExcelBaoCaoKhongKetNoi = getExcelBaoCaoKhongKetNoi;

        service.getLichSuVaoRaDiem = getLichSuVaoRaDiem;
        service.getExcelLichSuVaoRaDiem = getExcelLichSuVaoRaDiem;

        service.getLichSuThaoTac = getLichSuThaoTac;
        service.getExcelLichSuThaoTac = getExcelLichSuThaoTac;

        return service;
        function getCheckList(idkhachhang, idcheckin) {
            return $http.get(urlApi + '/api/baocao/getchecklistbykhachhang?idkhachhang=' + idkhachhang + '&idcheckin=' + idcheckin).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getPhienLamViec(id_Nhom, idnhanvien, fromdate, todate) {
            return $http.get(urlApi + '/api/phienlamviec/loaddata?idNhom=' + id_Nhom + '&idnhanvien=' + idnhanvien + '&from=' + fromdate + '&to=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelPhienLamViec(id_Nhom, idnhanvien, fromdate, todate) {
            return $http.get(urlApi + '/api/phienlamviec/ExportExcelPhienLamViec?idNhom=' + id_Nhom + '&idnhanvien=' + idnhanvien + '&from=' + fromdate + '&to=' + todate, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }
        function getKMDiChuyen(idnhanvien, fromdate, todate) {
            return $http.get(urlApi + '/api/phienlamviec/getkmdichuyen?idnhanvien=' + idnhanvien + '&thoigiandangnhap=' + fromdate + '&thoigiandangxuatphien=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getBaoCaoKhongKetNoi(id_Nhom, idnhanvien, fromdate, todate) {
            return $http.get(urlApi + '/api/phienlamviec/baocaomatketnoi?idNhom=' + id_Nhom + '&idnhanvien=' + idnhanvien + '&from=' + fromdate + '&to=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelBaoCaoKhongKetNoi(id_Nhom, idnhanvien, fromdate, todate) {
            return $http.get(urlApi + '/api/phienlamviec/Excelbaocaomatketnoi?idNhom=' + id_Nhom + '&idnhanvien=' + idnhanvien + '&from=' + fromdate + '&to=' + todate, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }

        function getLichSuVaoRaDiem(id_Nhom, idnhanvien, id_LoaiKhachHang, fromdate, todate, idkhachhang, idcheckin) {
            return $http.get(urlApi + '/api/baocao/lichsuvaoradiem?idnhom=' + id_Nhom + '&idnhanvien=' + idnhanvien + '&loaikhachhang=' + id_LoaiKhachHang + '&from=' + fromdate + '&to=' + todate + '&id=' + idcheckin + '&type=0' + '&idkhachhang=' + idkhachhang + '&vaodiemtheokhachhang=0').then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelLichSuVaoRaDiem(id_Nhom, idnhanvien, id_LoaiKhachHang, fromdate, todate, idkhachhang, idcheckin) {
            return $http.get(urlApi + '/api/baocao/ExportExcelLichSuRavaoDiem?idnhom=' + id_Nhom + '&idnhanvien=' + idnhanvien + '&loaikhachhang=' + id_LoaiKhachHang + '&from=' + fromdate + '&to=' + todate + '&id=' + idcheckin + '&type=0' + '&idkhachhang=' + idkhachhang + '&vaodiemtheokhachhang=0', { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }

        function getLichSuThaoTac(idKhachHang, idnhanvien, thaoTac, fromdate, todate) {
            return $http.get(urlApi + '/api/thaotacnguoidung/get?idKhachHang=' + idKhachHang + '&idnhanvien=' + idnhanvien + '&thaoTac=' + thaoTac
                + '&from=' + fromdate + '&to=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelLichSuThaoTac(idKhachHang, idnhanvien, thaoTac, fromdate, todate) {
            return $http.get(urlApi + '/api/thaotacnguoidung/ExportExcellichSuThaoTac?idKhachHang=' + idKhachHang + '&idnhanvien=' + idKhachHang + '&idnhanvien=' + idnhanvien + '&thaoTac=' + thaoTac
                + '&from=' + fromdate + '&to=' + todate, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }

    }

})();