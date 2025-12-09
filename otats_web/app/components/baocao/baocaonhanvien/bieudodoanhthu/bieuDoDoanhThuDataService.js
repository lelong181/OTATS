(function () {
    'use strict';
    angular
        .module('app')
        .factory('bieuDoDoanhThuDataService', bieuDoDoanhThuDataService);
    
    bieuDoDoanhThuDataService.$inject = ['$http', '$cookies', '$rootScope', '$timeout'];
    function bieuDoDoanhThuDataService($http, $cookies, $rootScope, $timeout) {
        var service = {};
        service.getBieuDoTyTrongDoanhThuTheoKhachHang = getBieuDoTyTrongDoanhThuTheoKhachHang;
        service.getBaoCaoTyTrongDoanhThuTheoKhachHang = getBaoCaoTyTrongDoanhThuTheoKhachHang;
        service.getExcelTyTrongDoanhThuTheoKhachHang = getExcelTyTrongDoanhThuTheoKhachHang;

        service.getBieuDoDoanhThuTheoNhanVien = getBieuDoDoanhThuTheoNhanVien;
        service.getBaoCaoDoanhThuTheoNhanVien = getBaoCaoDoanhThuTheoNhanVien;
        service.getExcelDoanhThuTheoNhanVien = getExcelDoanhThuTheoNhanVien;

        service.getBieuDoDoanhThuTheoKhuVuc = getBieuDoDoanhThuTheoKhuVuc;
        service.getBaoCaoDoanhThuTheoKhuVuc = getBaoCaoDoanhThuTheoKhuVuc;
        service.getExcelDoanhThuTheoKhuVuc = getExcelDoanhThuTheoKhuVuc;

        service.getBieuDoDoanhThuTheoNhomNhanVien = getBieuDoDoanhThuTheoNhomNhanVien;
        service.getBaoCaoDoanhThuTheoNhomNhanVien = getBaoCaoDoanhThuTheoNhomNhanVien;
        service.getExcelDoanhThuTheoNhomNhanVien = getExcelDoanhThuTheoNhomNhanVien;

        service.getBieuDoDoanhThuThang = getBieuDoDoanhThuThang;
        service.gettonghopdoanhthuticketota = gettonghopdoanhthuticketota;
        service.gettonghopdoanhthutheocaticketota = gettonghopdoanhthutheocaticketota;
        service.gettonghopthanhtoanthungan = gettonghopthanhtoanthungan;
        service.gettonghophttt = gettonghophttt;
        service.gettonghopdichvutheobooking = gettonghopdichvutheobooking;
        service.getBaoCaoDoanhThuThang = getBaoCaoDoanhThuThang;
        service.getExcelDoanhThuThang = getExcelDoanhThuThang;

        service.getBieuDoTop10DoanhThuTheoNhanVien = getBieuDoTop10DoanhThuTheoNhanVien;
        service.getBaoCaoTop10DoanhThuTheoNhanVien = getBaoCaoTop10DoanhThuTheoNhanVien;
        service.getExcelTop10DoanhThuTheoNhanVien = getExcelTop10DoanhThuTheoNhanVien;

        service.getBieuDoTop10DoanhThuTheoKhachHang = getBieuDoTop10DoanhThuTheoKhachHang;
        service.getBaoCaoTop10DoanhThuTheoKhachHang = getBaoCaoTop10DoanhThuTheoKhachHang;
        service.getExcelTop10DoanhThuTheoKhachHang = getExcelTop10DoanhThuTheoKhachHang;

        return service;
        function getBieuDoTyTrongDoanhThuTheoKhachHang(fromdate, todate) {
            return $http.get(urlApi + '/api/BieuDoDoanhThu/BieuDoTyTrongKhachHang?fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getBaoCaoTyTrongDoanhThuTheoKhachHang(fromdate, todate) {
            return $http.get(urlApi + '/api/BieuDoDoanhThu/APIBieuDoTyTrongKhachHang?fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelTyTrongDoanhThuTheoKhachHang(fromdate, todate) {
            return $http.get(urlApi + '/api/ExportReport/APIBieuDoTyTrongKhachHang?fromdate=' + fromdate + '&todate=' + todate, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }

        function getBieuDoDoanhThuTheoNhanVien(fromdate, todate, idnhom, idnhanvien) {
            return $http.get(urlApi + '/api/BieuDoDoanhThu/BieuDoDoanhThuNhanVien?fromdate=' + fromdate + '&todate=' + todate + '&id_Nhom=' + idnhom + '&id_NhanVien=' + idnhanvien).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getBaoCaoDoanhThuTheoNhanVien(fromdate, todate, idnhom, idnhanvien) {
            return $http.get(urlApi + '/api/BieuDoDoanhThu/APIBieuDoDoanhThuNhanVien?fromdate=' + fromdate + '&todate=' + todate + '&id_Nhom=' + idnhom + '&id_NhanVien=' + idnhanvien).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelDoanhThuTheoNhanVien(fromdate, todate, idnhom, idnhanvien) {
            return $http.get(urlApi + '/api/ExportReport/ExcelBieuDoDoanhThuNhanVien?fromdate=' + fromdate + '&todate=' + todate + '&id_Nhom=' + idnhom + '&id_NhanVien=' + idnhanvien, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }

        function getBieuDoDoanhThuTheoKhuVuc(fromdate, todate) {
            return $http.get(urlApi + '/api/BieuDoDoanhThu/BieuDoDoanhThuTheoKhuVuc?fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getBaoCaoDoanhThuTheoKhuVuc(fromdate, todate) {
            return $http.get(urlApi + '/api/BieuDoDoanhThu/APIBieuDoDoanhThuTheoKhuVuc?fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelDoanhThuTheoKhuVuc(fromdate, todate) {
            return $http.get(urlApi + '/api/ExportReport/ExcelBieuDoDoanhThuTheoKhuVuc?fromdate=' + fromdate + '&todate=' + todate + '&orderby=0', { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }

        function getBieuDoDoanhThuTheoNhomNhanVien(fromdate, todate) {
            return $http.get(urlApi + '/api/BieuDoDoanhThu/BieuDoDoanhThuTheoNhomNhanVien?fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getBaoCaoDoanhThuTheoNhomNhanVien(fromdate, todate) {
            return $http.get(urlApi + '/api/BieuDoDoanhThu/APIBieuDoDoanhThuTheoNhomNhanVien?fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelDoanhThuTheoNhomNhanVien(fromdate, todate) {
            return $http.get(urlApi + '/api/ExportReport/ExcelBieuDoDoanhThuTheoNhomNhanVien?fromdate=' + fromdate + '&todate=' + todate, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }

        function getBieuDoDoanhThuThang(fromdate, todate, idnhom, idnhanvien, idkhachhang) {
            return $http.get(urlApi + '/api/BieuDoDoanhThu/BieuDoDoanhThuThang?fromdate=' + fromdate + '&todate=' + todate + '&id_Nhom=' + idnhom + '&id_NhanVien=' + idnhanvien + '&id_KhachHang=' + idkhachhang).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function gettonghopdoanhthuticketota(sitecode, date) {
            return $http.get(urlApi + '/api/baocao/ticket_tonghopdoanhthu?SiteCode=' + sitecode + '&ReportDate=' + date).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function gettonghopdoanhthutheocaticketota(sitecode, date) {
            return $http.get(urlApi + '/api/baocao/ticket_tonghopdoanhthutheoca?SiteCode=' + sitecode + '&ReportDate=' + date).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function gettonghopthanhtoanthungan(sitecode, date) {
            return $http.get(urlApi + '/api/baocao/ticket_tonghopthanhtoanthungan?SiteCode=' + sitecode + '&ReportDate=' + date).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function gettonghophttt(sitecode, date) {
            return $http.get(urlApi + '/api/baocao/ticket_tonghophttt?SiteCode=' + sitecode + '&ReportDate=' + date).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function gettonghopdichvutheobooking(sitecode, fromdate, todate) {
            return $http.get(urlApi + '/api/baocao/gettonghopdichvutheobooking?SiteCode=' + sitecode + '&From=' + fromdate + '&To=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getBaoCaoDoanhThuThang(fromdate, todate, idnhom, idnhanvien, idkhachhang) {
            return $http.get(urlApi + '/api/BieuDoDoanhThu/APIBieuDoDoanhThuThang?fromdate=' + fromdate + '&todate=' + todate + '&id_Nhom=' + idnhom + '&id_NhanVien=' + idnhanvien + '&id_KhachHang=' + idkhachhang).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelDoanhThuThang(fromdate, todate, idnhom, idnhanvien, idkhachhang) {
            return $http.get(urlApi + '/api/ExportReport/ExcelBieuDoDoanhThuThang?fromdate=' + fromdate + '&todate=' + todate + '&id_Nhom=' + idnhom + '&id_NhanVien=' + idnhanvien + '&id_KhachHang=' + idkhachhang, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }

        function getBieuDoTop10DoanhThuTheoNhanVien(fromdate, todate) {
            return $http.get(urlApi + '/api/BieuDoDoanhThu/BieuDoTopTenDoanhThuTheoNhanVien?fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getBaoCaoTop10DoanhThuTheoNhanVien(fromdate, todate) {
            return $http.get(urlApi + '/api/BieuDoDoanhThu/APIBieuDoTopTenDoanhThuTheoNhanVien?fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelTop10DoanhThuTheoNhanVien(fromdate, todate) {
            return $http.get(urlApi + '/api/ExportReport/ExcelBieuDoTopTenDoanhThuTheoNhanVien?fromdate=' + fromdate + '&todate=' + todate + '&orderby=0', { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }

        function getBieuDoTop10DoanhThuTheoKhachHang(loai, fromdate, todate) {
            return $http.get(urlApi + '/api/BieuDoDoanhThu/BieuDoTopTenDoanhThuKhachHang?phanloai=' + loai + '&fromdate=' + fromdate + '&todate=' + todate + '&orderby=0').then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getBaoCaoTop10DoanhThuTheoKhachHang(loai, fromdate, todate) {
            return $http.get(urlApi + '/api/BieuDoDoanhThu/APIBieuDoTopTenDoanhThuKhachHang?phanloai=' + loai + '&fromdate=' + fromdate + '&todate=' + todate + '&orderby=0').then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelTop10DoanhThuTheoKhachHang(fromdate, todate) {
            return $http.get(urlApi + '/api/ExportReport/ExcelBieuDoTopTenDoanhThuKhachHang?fromdate=' + fromdate + '&todate=' + todate + '&orderby=0', { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }

    }

})();

