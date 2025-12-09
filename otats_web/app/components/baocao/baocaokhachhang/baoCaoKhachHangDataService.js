(function () {
    'use strict';

    angular
        .module('app')
        .factory('baoCaoKhachHangDataService', baoCaoKhachHangDataService);

    baoCaoKhachHangDataService.$inject = ['$http', '$cookies', '$rootScope', '$timeout'];
    function baoCaoKhachHangDataService($http, $cookies, $rootScope, $timeout) {
        var service = {};
        service.getBaoCaoDoanhThuTheoKhachHang = getBaoCaoDoanhThuTheoKhachHang;
        service.getExcelBaoCaoDoanhThuTheoKhachHang = getExcelBaoCaoDoanhThuTheoKhachHang;
        service.getBaoCaoTongHopDonHangTheoKhachHang = getBaoCaoTongHopDonHangTheoKhachHang;
        service.getExcelBaoCaoTongHopDonHangTheoKhachHang = getExcelBaoCaoTongHopDonHangTheoKhachHang;
        service.getBaoCaoKhachHangTheoGiaoDich = getBaoCaoKhachHangTheoGiaoDich;
        service.getExcelBaoCaoKhachHangTheoGiaoDich = getExcelBaoCaoKhachHangTheoGiaoDich;
        service.getBaoCaoPhanHoi = getBaoCaoPhanHoi;
        service.getExcelBaoCaoPhanHoi = getExcelBaoCaoPhanHoi;
        service.getBaoCaoMatHang_KH_DH = getBaoCaoMatHang_KH_DH;
        service.getExcelBaoCaoMatHang_KH_DH = getExcelBaoCaoMatHang_KH_DH;
        service.getBaoCaoMatHang_KH = getBaoCaoMatHang_KH;
        service.getExcelBaoCaoMatHang_KH = getExcelBaoCaoMatHang_KH;
        service.getBaoCaoTongHopVaoDiemTheoKhachHang = getBaoCaoTongHopVaoDiemTheoKhachHang;
        service.getExcelBaoCaoTongHopVaoDiemTheoKhachHang = getExcelBaoCaoTongHopVaoDiemTheoKhachHang;
        service.getBaoCaoViengThamTheoKhachHang = getBaoCaoViengThamTheoKhachHang;
        service.getBieuDoBaoCaoViengThamTheoKhachHang = getBieuDoBaoCaoViengThamTheoKhachHang;
        service.getExcelBaoCaoViengThamTheoKhachHang = getExcelBaoCaoViengThamTheoKhachHang;
        service.getBaoCaoKhachHangMoMoi = getBaoCaoKhachHangMoMoi;
        service.getExcelBaoCaoKhachHangMoMoi = getExcelBaoCaoKhachHangMoMoi;
        service.getBaoCaoKhachHangTheoKhuVuc = getBaoCaoKhachHangTheoKhuVuc;
        service.getExcelBaoCaoKhachHangTheoKhuVuc = getExcelBaoCaoKhachHangTheoKhuVuc;
        service.getBaoCaoAnhChup = getBaoCaoAnhChup;
        service.getExcelBaoCaoAnhChup = getExcelBaoCaoAnhChup;
        service.getdoisoatdichvu = getdoisoatdichvu;

        service.getBaoCaoSoatVeKhachHang = getBaoCaoSoatVeKhachHang;
        service.getBaoCaoCongSoatVe = getBaoCaoCongSoatVe;
        service.getNameACM = getNameACM;
        return service;
        //bm003
        function getBaoCaoAnhChup(idnhanvien, idkhachhang, fromdate, todate) {
            return $http.get(urlApi + '/api/BaoCaoAPI/BaoCaoAnhChupTheoAlbum?id_NhanVien=' + idnhanvien + '&id_KhachHang=' + idkhachhang + '&fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelBaoCaoAnhChup(idnhanvien, idkhachhang, fromdate, todate) {
            return $http.get(urlApi + '/api/ExportReport/ExcelBaoCaoAnhChupTheoAlbum?id_NhanVien=' + idnhanvien + '&id_KhachHang=' + idkhachhang + '&fromdate=' + fromdate + '&todate=' + todate, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }
        //bm004
        function getBaoCaoDoanhThuTheoKhachHang(fromdate, todate, idkhachhang) {
            return $http.get(urlApi + '/api/BieuDoDoanhThu/BaoCaoBanHang?fromdate=' + fromdate + '&todate=' + todate + '&id_KhachHang=' + idkhachhang).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelBaoCaoDoanhThuTheoKhachHang(fromdate, todate, idkhachhang) {
            return $http.get(urlApi + '/api/ExportReport/ExcelBaoCaoBanHang?fromdate=' + fromdate + '&todate=' + todate + '&id_KhachHang=' + idkhachhang, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }
        //bm011
        function getBaoCaoTongHopDonHangTheoKhachHang(ID_KhachHang, fromdate, todate) {
            return $http.get(urlApi + '/api/BaoCaoAPI/BaoCaoTongHopTheoKhachHang?ID_KhachHang=' + ID_KhachHang + '&fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelBaoCaoTongHopDonHangTheoKhachHang(ID_KhachHang, fromdate, todate) {
            return $http.get(urlApi + '/api/ExportReport/ExcelBaoCaoTongHopTheoKhachHang?ID_KhachHang=' + ID_KhachHang + '&fromdate=' + fromdate + '&todate=' + todate, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }
        //bm007
        function getBaoCaoKhachHangTheoGiaoDich(idkhachhang, idnhanvien, soNgay, loai) {
            return $http.get(urlApi + '/api/BaoCaoAPI/BaoCaoKhachHangTheoGiaoDich?id_KhachHang=' + idkhachhang + '&id_NhanVien=' + idnhanvien + '&soNgay=' + soNgay + '&loai=' + loai).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelBaoCaoKhachHangTheoGiaoDich(idkhachhang, idnhanvien, soNgay, loai) {
            return $http.get(urlApi + '/api/ExportReport/ExcelBaoCaoKhachHangTheoGiaoDich?id_KhachHang=' + idkhachhang + '&id_NhanVien=' + idnhanvien + '&soNgay=' + soNgay + '&loai=' + loai, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }
        //bm010
        function getBaoCaoPhanHoi(idkhachhang, idnhanvien, fromdate, todate) {
            return $http.get(urlApi + '/api/BaoCaoAPI/BaoCaoPhanHoi?id_KhachHang=' + idkhachhang + '&id_NhanVien=' + idnhanvien + '&fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelBaoCaoPhanHoi(idkhachhang, idnhanvien, fromdate, todate) {
            return $http.get(urlApi + '/api/ExportReport/ExcelBaoCaoPhanHoi?id_KhachHang=' + idkhachhang + '&id_NhanVien=' + idnhanvien + '&fromdate=' + fromdate + '&todate=' + todate, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }
        //bm005
        function getBaoCaoMatHang_KH_DH(idnhanvien, idmathang, idkhachhang, fromdate, todate) {
            return $http.get(urlApi + '/api/BaoCaoAPI/BaoCaoMH_KH_DH_NV?id_NhanVien=' + idnhanvien + '&id_MatHang=' + idmathang + '&id_KhachHang=' + idkhachhang + '&fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelBaoCaoMatHang_KH_DH(idnhanvien, idmathang, idkhachhang, fromdate, todate) {
            return $http.get(urlApi + '/api/ExportReport/ExcelBaoCaoMH_KH_DH_NV?id_NhanVien=' + idnhanvien + '&id_MatHang=' + idmathang + '&id_KhachHang=' + idkhachhang + '&fromdate=' + fromdate + '&todate=' + todate, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }
        //bm009
        function getBaoCaoMatHang_KH(idmathang, idloaikhachhang, idkhachhang, idnhanvien, fromdate, todate) {
            return $http.get(urlApi + '/api/BaoCaoAPI/BaoCaoMatHang_KH?id_MatHang=' + idmathang + '&id_LoaiKhachHang=' + idloaikhachhang + '&id_KhachHang=' + idkhachhang + '&idnhanvien=' + idnhanvien + '&fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelBaoCaoMatHang_KH(idmathang, idkhachhang, fromdate, todate) {
            return $http.get(urlApi + '/api/ExportReport/ExcelBaoCaoMatHang_KH?id_MatHang=' + idmathang + '&id_KhachHang=' + idkhachhang + '&fromdate=' + fromdate + '&todate=' + todate, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }
        //bm012
        function getBaoCaoTongHopVaoDiemTheoKhachHang(idnhanvien, idkhachhang, idnhom, fromdate, todate) {
            return $http.get(urlApi + '/api/BaoCaoAPI/BaoCaoTongHopCheckInTheoKhachHang?id_NhanVien=' + idnhanvien + '&id_KhachHang=' + idkhachhang + '&id_Nhom=' + idnhom + '&fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelBaoCaoTongHopVaoDiemTheoKhachHang(idnhanvien, idkhachhang, idnhom, fromdate, todate) {
            return $http.get(urlApi + '/api/ExportReport/ExcelBaoCaoTongHopCheckInTheoKhachHang?id_NhanVien=' + idnhanvien + '&id_KhachHang=' + idkhachhang + '&id_Nhom=' + idnhom + '&fromdate=' + fromdate + '&todate=' + todate, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }
        //bm013
        function getBaoCaoViengThamTheoKhachHang(idnhanvien, idkhachhang, idnhom, fromdate, todate) {
            return $http.get(urlApi + '/api/BaoCaoAPI/BaoCaoViengThamTheoKhachHang?id_NhanVien=' + idnhanvien + '&id_KhachHang=' + idkhachhang + '&id_Nhom=' + idnhom + '&fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getBieuDoBaoCaoViengThamTheoKhachHang(idnhanvien, idkhachhang, idnhom, fromdate, todate) {
            return $http.get(urlApi + '/api/BaoCaoAPI/BieuDoViengThamTheoKhachHang?id_NhanVien=' + idnhanvien + '&id_KhachHang=' + idkhachhang + '&id_Nhom=' + idnhom + '&fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelBaoCaoViengThamTheoKhachHang(idnhanvien, idkhachhang, idnhom, fromdate, todate) {
            return $http.get(urlApi + '/api/ExportReport/ExcelBaoCaoViengThamTheoKhachHang?id_NhanVien=' + idnhanvien + '&id_KhachHang=' + idkhachhang + '&id_Nhom=' + idnhom + '&fromdate=' + fromdate + '&todate=' + todate, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }
        //bm006
        function getBaoCaoKhachHangMoMoi(idnhanvien, idkhachhang, soDonHang, giaTriDonHang, fromdate, todate) {
            return $http.get(urlApi + '/api/BaoCaoAPI/BaoCaoKhachHangMoMoi?id_NhanVien=' + idnhanvien + '&id_KhachHang=' + idkhachhang + '&soDonHang=' + soDonHang + '&giaTriDonHang=' + giaTriDonHang + '&fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelBaoCaoKhachHangMoMoi(idnhanvien, idkhachhang, soDonHang, soGiatridonhang, fromdate, todate) {
            return $http.get(urlApi + '/api/ExportReport/ExcelBaoCaoKhachHangMoMoi?id_NhanVien=' + idnhanvien + '&id_KhachHang=' + idkhachhang + '&soDonHang=' + soDonHang + '&giaTriDonHang=' + soGiatridonhang + '&fromdate=' + fromdate + '&todate=' + todate, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }
        //bm008
        function getBaoCaoKhachHangTheoKhuVuc(idtinhthanh, idquanhuyen) {
            return $http.get(urlApi + '/api/BaoCaoAPI/BaoCaoKhachHangTheoKhuVuc?id_Tinh=' + idtinhthanh + '&id_Quan=' + idquanhuyen).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelBaoCaoKhachHangTheoKhuVuc(idtinhthanh, idquanhuyen) {
            return $http.get(urlApi + '/api/ExportReport/ExcelBaoCaoKhachHangTheoKhuVuc?id_Tinh=' + idtinhthanh + '&id_Quan=' + idquanhuyen, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }

        function getdoisoatdichvu(sitecode, fromdate, todate) {
            return $http.get(urlApi + '/api/baocao/getdoisoatdichvu?SiteCode=' + sitecode + '&From=' + fromdate + '&To=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        //Báo cáo soát vé khách hàng
        function getBaoCaoSoatVeKhachHang(fromdate, todate, sitecode) {
            return $http.get(urlApi + '/api/baocao/BaoCaoSoatVe?fromdate=' + fromdate + '&todate=' + todate + '&sitecode=' + sitecode).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getBaoCaoCongSoatVe(fromdate, todate, sitecode) {
            return $http.get(urlApi + '/api/baocao/BaoCaoCongSoatVe?fromdate=' + fromdate + '&todate=' + todate + '&sitecode=' + sitecode).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getNameACM(fromdate, todate, sitecode) {
            return $http.get(urlApi + '/api/baocao/DanhSachCongACM?fromdate=' + fromdate + '&todate=' + todate + '&sitecode=' + sitecode).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
    }

})();

