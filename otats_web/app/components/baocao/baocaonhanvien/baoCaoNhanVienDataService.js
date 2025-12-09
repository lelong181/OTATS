(function () {
    'use strict';

    angular
        .module('app')
        .factory('baoCaoNhanVienDataService', baoCaoNhanVienDataService);

    baoCaoNhanVienDataService.$inject = ['$http', '$cookies', '$rootScope', '$timeout'];
    function baoCaoNhanVienDataService($http, $cookies, $rootScope, $timeout) {
        var service = {};
        service.getBaoCaoKPITheoNhanVien = getBaoCaoKPITheoNhanVien;
        service.updateTrangThaiTheDo = updateTrangThaiTheDo;

        service.getBieuDoKPICacChiTieu = getBieuDoKPICacChiTieu;
        service.getBaoCaoDungDo = getBaoCaoDungDo;
        service.getBaoCaoChuyenDo = getBaoCaoChuyenDo;
        service.getExcelBaoCaoDungDo = getExcelBaoCaoDungDo;

        service.getBaoCaoLichSuGiaoHang = getBaoCaoLichSuGiaoHang;
        service.getExcelBaoCaoLichSuGiaoHang = getExcelBaoCaoLichSuGiaoHang;

        service.getBaoCaoLichSuSuaChua = getBaoCaoLichSuSuaChua;
        service.getExcelBaoCaoLichSuSuaChua = getExcelBaoCaoLichSuSuaChua;

        service.getBaoCaoDonHangTheoDiem = getBaoCaoDonHangTheoDiem;
        service.getChiTietDonHangTheoDiem = getChiTietDonHangTheoDiem;
        service.getExcelBaoCaoDonHangTheoDiem = getExcelBaoCaoDonHangTheoDiem;

        service.getBaoCaoKPINhanVien = getBaoCaoKPINhanVien;
        service.getExcelBaoCaoKPINhanVien = getExcelBaoCaoKPINhanVien;

        service.getBaoCaoDonHangCoBanKPINhanVien = getBaoCaoDonHangCoBanKPINhanVien;
        service.getExcelBaoCaoDonHangCoBanKPINhanVien = getExcelBaoCaoDonHangCoBanKPINhanVien;

        service.getBaoCaoTongHopMatHangTheoNhanVien = getBaoCaoTongHopMatHangTheoNhanVien;
        service.getBaoCaoTongHopMatHangTheoNhanVien_ChiTiet = getBaoCaoTongHopMatHangTheoNhanVien_ChiTiet;
        service.getExcelBaoCaoTongHopMatHangTheoNhanVien = getExcelBaoCaoTongHopMatHangTheoNhanVien;

        service.getBaoCaoKMDiChuyen = getBaoCaoKMDiChuyen;
        service.getExcelBaoCaoKMDiChuyen = getExcelBaoCaoKMDiChuyen;

        service.getBaoCaoTinhTrangGPS = getBaoCaoTinhTrangGPS;
        service.getExcelBaoCaoTinhTrangGPS = getExcelBaoCaoTinhTrangGPS;

        service.getBaoCaoTinhTrangFakeGPS = getBaoCaoTinhTrangFakeGPS;
        service.getExcelBaoCaoTinhTrangFakeGPS = getExcelBaoCaoTinhTrangFakeGPS;

        service.getBaoCaoViengThamTheoTuyen = getBaoCaoViengThamTheoTuyen;
        service.getExcelBaoCaoViengThamTheoTuyen = getExcelBaoCaoViengThamTheoTuyen;

        service.getBaoCaoLichSuMatTinHieu = getBaoCaoLichSuMatTinHieu;
        service.getExcelBaoCaoLichSuMatTinHieu = getExcelBaoCaoLichSuMatTinHieu;

        service.getBaoCaoTongHopRaVaoDiem = getBaoCaoTongHopRaVaoDiem;
        service.getExcelBaoCaoTongHopRaVaoDiem = getExcelBaoCaoTongHopRaVaoDiem;

        service.getBaoCaoLichHen = getBaoCaoLichHen;
        service.getExcelBaoCaoLichHen = getExcelBaoCaoLichHen;
        
        service.getBaoCaoViengThamKhachHangTheoTuyen = getBaoCaoViengThamKhachHangTheoTuyen;
        service.getBaoCaoSoLuongViengThamKhachHangTheoTuyen = getBaoCaoSoLuongViengThamKhachHangTheoTuyen;
        service.baoCaoViengThamKhachHangTheoTuyenChiTiet_detail = baoCaoViengThamKhachHangTheoTuyenChiTiet_detail;
        service.getExcelBaoCaoViengThamKhachHangTheoTuyen = getExcelBaoCaoViengThamKhachHangTheoTuyen;
        service.getExcelBaoCaoSoLuongViengThamKhachHangTheoTuyen = getExcelBaoCaoSoLuongViengThamKhachHangTheoTuyen;

        service.getBaoCaoTongHopCongViecNhanVien = getBaoCaoTongHopCongViecNhanVien;
        service.getExcelBaoCaoTongHopCongViecNhanVien = getExcelBaoCaoTongHopCongViecNhanVien;

        service.getAllBienDong = getAllBienDong;
        service.getTongSoDu = getTongSoDu;

        return service;

        function getBaoCaoKPITheoNhanVien(idnhom, fromdate, todate) {
            return $http.get(urlApi + '/api/BaoCaoAPI/BieuDoChiTieuKPICongViec?id_Nhom=' + idnhom + '&fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getBieuDoKPITheoNhanVien(idnhom, fromdate, todate) {
            return $http.get(urlApi + '/api/BaoCaoAPI/BieuDoKPICongViec?id_Nhom=' + idnhom + '&fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getBieuDoKPICacChiTieu(date_string) {
            //date_string: nếu là năm (00/yyyy), nếu quý: [01-04]/yyyy
            return $http.get(urlApi + '/api/BaoCaoAPI/BieuDoKPI?_date=' + date_string).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        //bm0018
        function getBaoCaoDungDo(idnhanvien, dungDo, fromdate, todate) {
            return $http.get(urlApi + '/api/BaoCaoAPI/BaoCaoDungDo?id_NhanVien=' + idnhanvien + '&dungDo=' + dungDo + '&fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelBaoCaoDungDo(idnhanvien, dungDo, fromdate, todate) {
            return $http.get(urlApi + '/api/ExportReport/ExcelBaoCaoDungDo?id_NhanVien=' + idnhanvien + '&dungDo=' + dungDo + '&fromdate=' + fromdate + '&todate=' + todate, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }
        //bm022
        function getBaoCaoLichSuGiaoHang(idkhachhang, idnhanvien, fromdate, todate) {
            return $http.get(urlApi + '/api/BaoCaoAPI/BaoCaoLichSuGiaoHang?id_KhachHang=' + idkhachhang + '&id_NhanVien=' + idnhanvien + '&fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelBaoCaoLichSuGiaoHang(idkhachhang, idnhanvien, fromdate, todate) {
            return $http.get(urlApi + '/api/ExportReport/ExcelBaoCaoLichSuGiaoHang?id_KhachHang=' + idkhachhang + '&id_NhanVien=' + idnhanvien + '&fromdate=' + fromdate + '&todate=' + todate, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }
        //bm024
        function getBaoCaoLichSuSuaChua(idkhachhang, idnhanvien, fromdate, todate) {
            return $http.get(urlApi + '/api/BaoCaoAPI/BaoCaoLichSuBaoDuongSuaChua?id_KhachHang=' + idkhachhang + '&id_NhanVien=' + idnhanvien + '&fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelBaoCaoLichSuSuaChua(idkhachhang, idnhanvien, fromdate, todate) {
            return $http.get(urlApi + '/api/ExportReport/ExcelBaoCaoLichSuBaoDuongSuaChua?id_KhachHang=0' + '&id_NhanVien=0' + '&fromdate=' + fromdate + '&todate=' + todate, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }
        //bm017
        function getBaoCaoDonHangTheoDiem(idnhanvien, fromdate, todate) {
            return $http.get(urlApi + '/api/BaoCaoAPI/BaoCaoDonHangTheoDiem?id_Nhom=0' + '&id_NhanVien=' + idnhanvien + '&fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getChiTietDonHangTheoDiem(idnhanvien, fromdate, todate) {
            return $http.get(urlApi + '/api/BaoCaoAPI/BaoCaoDonHangTheoDiem_ChiTiet?id_NhanVien=' + idnhanvien + '&fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelBaoCaoDonHangTheoDiem(idnhanvien, fromdate, todate) {
            return $http.get(urlApi + '/api/ExportReport/ExcelBaoCaoDonHangTheoDiem?id_Nhom=0' + '&id_NhanVien=' + idnhanvien + '&fromdate=' + fromdate + '&todate=' + todate, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }
        //bm020
        function getBaoCaoKPINhanVien(idnhom, idnhanvien, fromdate, todate) {
            return $http.get(urlApi + '/api/BaoCaoAPI/BaoCaoKPINhanVien?&id_Nhom=' + idnhom + '&id_NhanVien=' + idnhanvien +  '&fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelBaoCaoKPINhanVien(idnhom, idnhanvien, fromdate, todate) {
            return $http.get(urlApi + '/api/ExportReport/ExcelBaoCaoKPINhanVien?&id_Nhom=' + idnhom + '&id_NhanVien=' + idnhanvien +  '&fromdate=' + fromdate + '&todate=' + todate, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }
        //bm016
        function getBaoCaoDonHangCoBanKPINhanVien(idkhachhang, idnhanvien, idmathang, fromdate, todate) {
            return $http.get(urlApi + '/api/BaoCaoAPI/baoCaoHoanThanhDonHangCoBanThucTe?id_KhachHang=' + idkhachhang + '&id_NhanVien=' + idnhanvien + '&id_MatHang=' + idmathang + '&fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelBaoCaoDonHangCoBanKPINhanVien(idkhachhang, idnhanvien, idmathang, fromdate, todate) {
            return $http.get(urlApi + '/api/ExportReport/ExcelbaoCaoHoanThanhDonHangCoBanThucTe?id_KhachHang=' + idkhachhang + '&id_NhanVien=' + idnhanvien + '&id_MatHang=' + idmathang + '&fromdate=' + fromdate + '&todate=' + todate, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }
        //bm028
        function getBaoCaoTongHopMatHangTheoNhanVien(idnhanvien, idmathang, iD_Nhom, fromdate, todate) {
            return $http.get(urlApi + '/api/BaoCaoAPI/BaoCaoMatHang_NV?id_KhachHang=0' + '&id_NhanVien=' + idnhanvien + '&id_MatHang=' + idmathang + '&id_NganhHang=' + iD_Nhom + '&fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getBaoCaoTongHopMatHangTheoNhanVien_ChiTiet(idnhanvien, idmathang, fromdate, todate) {
            return $http.get(urlApi + '/api/BaoCaoAPI/BaoCaoMatHang_NV_ChiTiet?id_KhachHang=0' + '&id_NhanVien=' + idnhanvien + '&id_MatHang=' + idmathang + '&fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelBaoCaoTongHopMatHangTheoNhanVien(idnhanvien, idmathang, iD_Nhom, fromdate, todate) {
            return $http.get(urlApi + '/api/ExportReport/ExcelBaoCaoMatHang_NV?id_KhachHang=0' + '&id_NhanVien=' + idnhanvien + '&id_MatHang=' + idmathang + '&id_NganhHang=' + iD_Nhom + '&fromdate=' + fromdate + '&todate=' + todate, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }
        //bm019
        function getBaoCaoKMDiChuyen(idnhanvien, fromdate, todate) {
            return $http.get(urlApi + '/api/BaoCaoAPI/BaoCaoQuangDuongDiChuyen?id_NhanVien=' + idnhanvien + '&fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelBaoCaoKMDiChuyen(idnhanvien, fromdate, todate) {
            return $http.get(urlApi + '/api/ExportReport/ExcelBaoCaoQuangDuongDiChuyen?id_NhanVien=' + idnhanvien + '&fromdate=' + fromdate + '&todate=' + todate, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }   
        //bm026
        function getBaoCaoTinhTrangGPS(idnhanvien, fromdate, todate) {
            return $http.get(urlApi + '/api/BaoCaoAPI/BaoCaoBatTatGPS?id_NhanVien=' + idnhanvien + '&fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelBaoCaoTinhTrangGPS(idnhanvien, fromdate, todate) {
            return $http.get(urlApi + '/api/ExportReport/ExcelBaoCaoBatTatGPS?id_NhanVien=' + idnhanvien + '&fromdate=' + fromdate + '&todate=' + todate, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }
        //bm025
        function getBaoCaoTinhTrangFakeGPS(idnhanvien, fromdate, todate) {
            return $http.get(urlApi + '/api/BaoCaoAPI/BaoCaoBatTatFakeGPS?id_NhanVien=' + idnhanvien + '&fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelBaoCaoTinhTrangFakeGPS(idnhanvien, fromdate, todate) {
            return $http.get(urlApi + '/api/ExportReport/ExcelBaoCaoBatTatFakeGPS?id_NhanVien=' + idnhanvien + '&fromdate=' + fromdate + '&todate=' + todate, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }
        //bm031
        function getBaoCaoViengThamTheoTuyen(idtuyen, idnhanvien, fromdate, todate) {
            return $http.get(urlApi + '/api/BaoCaoAPI/BaoCaoCheckInTuyen?id_Tuyen=' + idtuyen + '&id_NhanVien=' + idnhanvien + '&fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelBaoCaoViengThamTheoTuyen(idtuyen, idnhanvien, fromdate, todate) {
            return $http.get(urlApi + '/api/ExportReport/ExcelBaoCaoCheckInTuyen?id_Tuyen=' + idtuyen + '&id_NhanVien=' + idnhanvien + '&fromdate=' + fromdate + '&todate=' + todate, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }
        //bm023
        function getBaoCaoLichSuMatTinHieu(idnhom, idnhanvien, fromdate, todate) {
            return $http.get(urlApi + '/api/BaoCaoAPI/BaoCaoLichSuMatTinHieu?&id_Nhom=' + idnhom + '&id_NhanVien=' + idnhanvien + '&fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelBaoCaoLichSuMatTinHieu(idnhom, idnhanvien, fromdate, todate) {
            return $http.get(urlApi + '/api/ExportReport/ExcelBaoCaoLichSuMatTinHieu?&id_Nhom=' + idnhom + '&id_NhanVien=' + idnhanvien + '&fromdate=' + fromdate + '&todate=' + todate, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }
        //bm029
        function getBaoCaoTongHopRaVaoDiem(idnhom, idnhanvien, fromdate, todate) {
            return $http.get(urlApi + '/api/BaoCaoAPI/baocaotonghopravaodiem?&id_Nhom=' + idnhom + '&id_NhanVien=' + idnhanvien + '&from=' + fromdate + '&to=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelBaoCaoTongHopRaVaoDiem(idnhom, idnhanvien, fromdate, todate) {
            return $http.get(urlApi + '/api/ExportReport/Excelbaocaotonghopravaodiem?&id_Nhom=' + idnhom + '&id_NhanVien=' + idnhanvien + '&from=' + fromdate + '&to=' + todate, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }
        //bm021
        function getBaoCaoLichHen(idkhachhang, idnhanvien, fromdate, todate) {
            return $http.get(urlApi + '/api/BaoCaoAPI/BaoCaoLichHen?id_KhachHang=' + idkhachhang + '&id_NhanVien=' + idnhanvien + '&fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelBaoCaoLichHen(idkhachhang, idnhanvien, fromdate, todate) {
            return $http.get(urlApi + '/api/ExportReport/ExcelBaoCaoLichHen?id_KhachHang=0' + '&id_NhanVien=0' + '&fromdate=' + fromdate + '&todate=' + todate, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }
        //bm030
        function getBaoCaoViengThamKhachHangTheoTuyen(idnhanvien, fromdate, todate) {
            return $http.get(urlApi + '/api/BaoCaoAPI/baoCaoViengThamKhachHangTheoTuyenChiTiet?id_NhanVien=' + idnhanvien + '&fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getBaoCaoSoLuongViengThamKhachHangTheoTuyen(idnhanvien, fromdate, todate) {
            return $http.get(urlApi + '/api/BaoCaoAPI/baoCaoViengThamKhachHangTheoTuyenSoLuong?id_NhanVien=' + idnhanvien + '&fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function baoCaoViengThamKhachHangTheoTuyenChiTiet_detail(idtuyen, idnhanvien, day) {
            return $http.get(urlApi + '/api/BaoCaoAPI/baoCaoViengThamKhachHangTheoTuyenChiTiet_detail?idtuyen=' + idtuyen + '&idnhanvien=' + idnhanvien + '&day=' + day).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelBaoCaoViengThamKhachHangTheoTuyen(idnhanvien, fromdate, todate) {
            return $http.get(urlApi + '/api/ExportReport/ExcelbaoCaoViengThamKhachHangTheoTuyenChiTiet?id_NhanVien=' + idnhanvien + '&fromdate=' + fromdate + '&todate=' + todate, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }
        function getExcelBaoCaoSoLuongViengThamKhachHangTheoTuyen(idnhanvien, fromdate, todate) {
            return $http.get(urlApi + '/api/ExportReport/ExcelbaoCaoViengThamKhachHangTheoTuyenSoLuong?id_NhanVien=' + idnhanvien + '&fromdate=' + fromdate + '&todate=' + todate, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }
        //bm027
        function getBaoCaoTongHopCongViecNhanVien(idnhom, idnhanvien, fromdate, todate) {
            return $http.get(urlApi + '/api/BaoCaoAPI/BaoCaoCongViecNhanVien?&id_Nhom=' + idnhom + '&id_NhanVien=' + idnhanvien + '&fromdate=' + fromdate + '&todate=' + todate).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getExcelBaoCaoTongHopCongViecNhanVien(idnhom, idnhanvien, fromdate, todate) {
            return $http.get(urlApi + '/api/ExportReport/BaoCaoCongViecNhanVien?&id_Nhom=' + idnhom + '&id_NhanVien=' + idnhanvien + '&fromdate=' + fromdate + '&todate=' + todate, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }

        //bm0018
        function getBaoCaoChuyenDo(fromdate, todate, trangthai) {
            return $http.get(urlApi + '/api/BaoCaoAPI/BaoCaoChuyenDo?fromdate=' + fromdate + '&todate=' + todate + '&trangthai=' + trangthai).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function updateTrangThaiTheDo(id, trangthai) {
            return $http.get(urlApi + '/api/donhang/updateTrangThaiTheDo?ID_ChiTiet_MatHang_DonHang=' + id + '&TrangThai=' + trangthai).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getAllBienDong(id_nhom, from, to) {
            return $http.post(urlApi + '/api/donhang/GetAllBienDong?id_nhom=' + id_nhom +'&from=' + from + '&to=' + to).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getTongSoDu(id_nhom) {
            return $http.post(urlApi + '/api/donhang/GetTongSoDu?id_nhom=' + id_nhom).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
    }
})(); 
