(function () {
    'use strict';
    angular
        .module('app')
        .factory('baoCaoSanLuongDataService', baoCaoSanLuongDataService);

    baoCaoSanLuongDataService.$inject = ['$http', '$cookies', '$rootScope', '$timeout'];
    function baoCaoSanLuongDataService($http, $cookies, $rootScope, $timeout) {
        var service = {};
        service.getBieuDoKhachHangTheoNganhHang = getBieuDoKhachHangTheoNganhHang;
        service.getBaoCaoKhachHangTheoNganhHang = getBaoCaoKhachHangTheoNganhHang;

        service.getBieuDoKhachHangTheoKhuVuc = getBieuDoKhachHangTheoKhuVuc;
        service.getBaoCaoKhachHangTheoKhuVuc = getBaoCaoKhachHangTheoKhuVuc;

        service.getBieuDoDonHangNhomNhanVien = getBieuDoDonHangNhomNhanVien;

        service.getBieuDoDonHangTheoKhuVuc = getBieuDoDonHangTheoKhuVuc;
        service.getBaoCaoDonHangTheoKhuVuc = getBaoCaoDonHangTheoKhuVuc;
        service.getExcelDonHangTheoKhuVuc = getExcelDonHangTheoKhuVuc;

        service.getBieuDoTop10DonHangTheoNhanVien = getBieuDoTop10DonHangTheoNhanVien;
        service.getBaoCaoTop10DonHangTheoNhanVien = getBaoCaoTop10DonHangTheoNhanVien;
        service.getExcelTop10DonHangTheoNhanVien = getExcelTop10DonHangTheoNhanVien;
        
        service.getBieuDoTop10SanPhamBanThap = getBieuDoTop10SanPhamBanThap;
        service.getBaoCaoTop10SanPhamBanThap = getBaoCaoTop10SanPhamBanThap;
        service.getExcelTop10SanPhamBanThap = getExcelTop10SanPhamBanThap;

        service.getBieuDoTop10DonHangTheoKhachHang = getBieuDoTop10DonHangTheoKhachHang;
        service.getBaoCaoTop10DonHangTheoKhachHang = getBaoCaoTop10DonHangTheoKhachHang;
        service.getExcelTop10DonHangTheoKhachHang = getExcelTop10DonHangTheoKhachHang;

        service.getBieuDoTop10NhanVienDiChuyen = getBieuDoTop10NhanVienDiChuyen;

        service.getBieuDoTop10NhanVienViengTham = getBieuDoTop10NhanVienViengTham;
        service.getBieuDoTop10KhachHangViengTham = getBieuDoTop10KhachHangViengTham;

        service.getBieuDoTop10SanPhamBanChay = getBieuDoTop10SanPhamBanChay;
        service.getBaoCaoTop10SanPhamBanChay = getBaoCaoTop10SanPhamBanChay;
        service.getExcelTop10SanPhamBanChay = getExcelTop10SanPhamBanChay;
        
        return service;
        function getBieuDoKhachHangTheoNganhHang() {
            return $http.get(urlApi + '/api/BieuDoSanLuong/BieuDoPhanLoaiKhachHangNganhHang').then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getBaoCaoKhachHangTheoNganhHang() {
            return $http.get(urlApi + '/api/BieuDoSanLuong/BaoCaoPhanLoaiKhachHangNganhHang').then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getBieuDoKhachHangTheoKhuVuc() {
            return $http.get(urlApi + '/api/BieuDoSanLuong/BieuDoPhanLoaiKhachHangTheoKhuVuc').then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getBaoCaoKhachHangTheoKhuVuc() {
            return $http.get(urlApi + '/api/BieuDoSanLuong/BaoCaoPhanLoaiKhachHangTheoKhuVuc').then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getBieuDoDonHangNhomNhanVien(fromdate, todate) {
            return $http.get(urlApi + '/api/BieuDoSanLuong/BieuDoDonHangTheoNhanVien?fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getBieuDoDonHangTheoKhuVuc(fromdate, todate) {
            return $http.get(urlApi + '/api/BieuDoSanLuong/BieuDoDonHangTheoKhuVuc?fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getBaoCaoDonHangTheoKhuVuc(fromdate, todate) {
            return $http.get(urlApi + '/api/BieuDoSanLuong/BaoCaoDonHangTheoKhuVuc?fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelDonHangTheoKhuVuc(fromdate, todate) {
            return $http.get(urlApi + '/api/ExportReport/ExcelBieuDoDonHangTheoKhuVuc?fromdate=' + fromdate + '&todate=' + todate + '&orderby=0', { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }

        function getBieuDoTop10DonHangTheoNhanVien(fromdate, todate) {
            return $http.get(urlApi + '/api/BieuDoSanLuong/BieuDoTopTenDonHangTheoNhanVien?fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getBaoCaoTop10DonHangTheoNhanVien(fromdate, todate) {
            return $http.get(urlApi + '/api/BieuDoSanLuong/BaoCaoTopTenDonHangTheoNhanVien?fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelTop10DonHangTheoNhanVien(fromdate, todate) {
            return $http.get(urlApi + '/api/ExportReport/ExcelBieuDoTopTenDonHangTheoNhanVien?fromdate=' + fromdate + '&todate=' + todate + '&orderby=0', { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }
        
        function getBieuDoTop10SanPhamBanThap(fromdate, todate) {
            return $http.get(urlApi + '/api/BieuDoSanLuong/BieuDoTopTenSanPhamBanThapTheoKhachHang?fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getBaoCaoTop10SanPhamBanThap(fromdate, todate) {
            return $http.get(urlApi + '/api/BieuDoSanLuong/BaoCaoTopTenSanPhamBanThapTheoKhachHang?fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelTop10SanPhamBanThap(fromdate, todate) {
            return $http.get(urlApi + '/api/ExportReport/ExcelBieuDoTopTenSanPhamBanThapTheoKhachHang?fromdate=' + fromdate + '&todate=' + todate, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }

        function getBieuDoTop10DonHangTheoKhachHang(loai, fromdate, todate) {
            return $http.get(urlApi + '/api/BieuDoSanLuong/BieuDoTopTenDonHangKhachHang?phanloai=' + loai + '&fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getBaoCaoTop10DonHangTheoKhachHang(loai, fromdate, todate) {
            return $http.get(urlApi + '/api/BieuDoSanLuong/BaoCaoTopTenDonHangKhachHang?phanloai=' + loai + '&fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelTop10DonHangTheoKhachHang(fromdate, todate) {
            return $http.get(urlApi + '/api/ExportReport/ExcelBieuDoTopTenDonHangKhachHang?fromdate=' + fromdate + '&todate=' + todate + '&orderby=0', { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }

        function getBieuDoTop10NhanVienDiChuyen(fromdate, todate) {
            return $http.get(urlApi + '/api/BieuDoSanLuong/BieuDoDiChuyen?fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getBieuDoTop10NhanVienViengTham(fromdate, todate) {
            return $http.get(urlApi + '/api/BieuDoSanLuong/BaoCaoViengThamNhanVien?fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getBieuDoTop10KhachHangViengTham(fromdate, todate) {
            return $http.get(urlApi + '/api/BieuDoSanLuong/BieuDoViengThamKhachHang?fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getBieuDoTop10SanPhamBanChay(fromdate, todate) {
            return $http.get(urlApi + '/api/BieuDoSanLuong/BieuDoTopTenSanPhamTheoKhachHang?fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getBaoCaoTop10SanPhamBanChay(fromdate, todate) {
            return $http.get(urlApi + '/api/BieuDoSanLuong/BaoCaoTopTenSanPhamTheoKhachHang?fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelTop10SanPhamBanChay(fromdate, todate) {
            return $http.get(urlApi + '/api/ExportReport/ExcelBieuDoTopTenSanPhamTheoKhachHang?fromdate=' + fromdate + '&todate=' + todate, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }
    }

})();

