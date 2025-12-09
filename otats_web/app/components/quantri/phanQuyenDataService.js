(function () {
    'use strict';
    angular
        .module('app')
        .factory('phanQuyenDataService', phanQuyenDataService);

    phanQuyenDataService.$inject = ['$http', '$rootScope', '$timeout'];
    function phanQuyenDataService($http, $rootScope, $timeout) {
        var service = {};

        service.getListNhomNhanVien = getListNhomNhanVien;
        service.getListAllNhomNhanVien = getListAllNhomNhanVien;
        service.getdatanganhhangphanquyen = getdatanganhhangphanquyen;
        service.getdatanganhhang = getdatanganhhang;
        service.xoaphanquyennganhhangnhom = xoaphanquyennganhhangnhom;
        service.themnganhhangnhom = themnganhhangnhom;

        service.getchucnangweb = getchucnangweb;
        service.getchucnangapp = getchucnangapp;
        service.capnhatphanquyennhom = capnhatphanquyennhom;
        service.getlistkhachhang = getlistkhachhang;
        service.getlistNhanVienPhanQuyen = getlistNhanVienPhanQuyen;
        service.updatePhanQuyen = updatePhanQuyen;
        service.copyPhanQuyen = copyPhanQuyen;
        service.xoaPhanQuyen = xoaPhanQuyen;

        service.getlistNhomNhanVienPhanQuyen = getlistNhomNhanVienPhanQuyen;
        service.updatePhanQuyenByNhom = updatePhanQuyenByNhom;
        service.copyPhanQuyenByNhom = copyPhanQuyenByNhom;
        service.phanquyennhieunhomnhanvienkhachhang = phanquyennhieunhomnhanvienkhachhang;
        service.xoaphanquyennhomnhanvienkhachhang = xoaphanquyennhomnhanvienkhachhang;

        service.getlistnhanvien = getlistnhanvien;
        service.getdskhachhangdacapquyen = getdskhachhangdacapquyen;
        service.getdskhachhangchuacapquyen = getdskhachhangchuacapquyen;
        service.phanquyenkhachhangchonhanvien = phanquyenkhachhangchonhanvien;
        service.bophanquyenkhachhangchonhanvien = bophanquyenkhachhangchonhanvien;

        service.copyPhanQuyenKhachHang = copyPhanQuyenKhachHang;
        service.xoaPhanQuyenKhachHang = xoaPhanQuyenKhachHang;

        service.themPhanQuyenNhanVienKhachHang = themPhanQuyenNhanVienKhachHang;
        service.getlistnhanviendaphanquyenkhachhang = getlistnhanviendaphanquyenkhachhang;

        service.exportExcel = exportExcel;
        service.taifilemauphanquyen = taifilemauphanquyen;
        service.importphanquyen = importphanquyen;

        return service;

        function exportExcel() {
            return $http.get(urlApi + '/api/phanquyen/ExportExcelPhanQuyen', { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }
        function taifilemauphanquyen() {
            return $http.get(urlApi + '/api/phanquyen/GetTemplatePhanQuyen', { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }
        function importphanquyen(fileUpload) {
            return $http.post(urlApi + '/api/phanquyen/importphanquyen', { filename: fileUpload }, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }

        function themPhanQuyenNhanVienKhachHang(listnhanvien, idkhachhang, idquyen) {
            return $http.post(urlApi + '/api/phanquyen/phanquyennhieunhanvienkhachhang?idkhachhang=' + idkhachhang + '&idquyen=' + idquyen, listnhanvien).then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_phanquyenkhongthanhcongvuilonglienhequantri') }
            });
        }

        function getlistnhanviendaphanquyenkhachhang(idkhachhang) {
            return $http.get(urlApi + '/api/phanquyen/getnhanviendaphanchokhachhang?idkhachhang=' + idkhachhang).then(function (response) {
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

        function getListAllNhomNhanVien() {
            return $http.get(urlApi + '/api/nhomnhanvien/getallcombox').then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getdatanganhhangphanquyen(idnhom) {
            return $http.get(urlApi + '/api/phanquyen/getdatanganhhangphanquyen?idnhom=' + idnhom).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getdatanganhhang() {
            return $http.get(urlApi + '/api/phanquyen/getdatanganhhang').then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function xoaphanquyennganhhangnhom(idnhom, idnganhhang) {
            return $http.post(urlApi + '/api/phanquyen/xoaphanquyennganhhangnhom?idnhom=' + idnhom + '&idnganhhang=' + idnganhhang).then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_thietlapnguongkhongthanhcongvuilonglienhequantri') }
            });
        }

        function themnganhhangnhom(idnhom, idnganhhang) {
            return $http.post(urlApi + '/api/phanquyen/themnganhhangnhom?idnhom=' + idnhom + '&idnganhhang=' + idnganhhang).then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_thietlapnguongkhongthanhcongvuilonglienhequantri') }
            });
        }

        function getchucnangweb(idnhom) {
            return $http.get(urlApi + '/api/phanquyen/getchucnangweb?idnhom=' + idnhom).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getchucnangapp(idnhom) {
            return $http.get(urlApi + '/api/phanquyen/getchucnangapp?idnhom=' + idnhom).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function capnhatphanquyennhom(data) {
            return $http.post(urlApi + '/api/phanquyen/capnhatphanquyennhom', data).then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_phanquyenkhongthanhcongvuilonglienhequantri') }
            });
        }

        function getlistkhachhang() {
            return $http.get(urlApi + '/api/khachhang/getall').then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getlistNhanVienPhanQuyen(idnhom, idkhachhang) {
            return $http.get(urlApi + '/api/phanquyen/getDSPhanQuyenNhanVienByKhachHang?IdNhom=' + idnhom + "&ID_KhachHang=" + idkhachhang).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function updatePhanQuyen(data) {
            return $http.post(urlApi + '/api/phanquyen/phanquyen_nhanvienkhachhang', data).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_phanquyenkhongthanhcongvuilonglienhequantri') }
            });

        }

        function copyPhanQuyen(idkhnguon, idkhdich) {
            return $http.post(urlApi + '/api/phanquyen/saochepphanquyen_nhieunhanvienkhachhang?ID_KhachHangNguon=' + idkhnguon, idkhdich).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_ganphanquyenkhongthanhcongvuilonglienhequantri') }
            });

        }

        function copyPhanQuyenKhachHang(nvnguon, nvdich) {
            return $http.post(urlApi + '/api/phanquyen/saochepphanquyen_khachhangnhanvien?IDNVNguon=' + nvnguon + '&IDNVDich=' + nvdich).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_saochepphanquyenkhongthanhcongvuilonglienhequantri') }
            });

        }

        function xoaPhanQuyenKhachHang(idnv) {
            return $http.post(urlApi + '/api/phanquyen/removephanquyen_khachhangnhanvien?ID_NhanVien=' + idnv).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_xoaphanquyenkhongthanhcongvuilonglienhequantri') }
            });

        }

        function xoaPhanQuyen(idkh) {
            return $http.post(urlApi + '/api/phanquyen/removephanquyen_nhanvienkhachhang?ID_KhachHang=' + idkh).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_xoaphanquyenkhongthanhcongvuilonglienhequantri') }
            });

        }

        function getlistNhomNhanVienPhanQuyen(idkhachhang) {
            return $http.get(urlApi + '/api/phanquyen/getDSPhanQuyenNhomNhanVienByKhachHang?ID_KhachHang=' + idkhachhang).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function phanquyennhieunhomnhanvienkhachhang(listnhomnhanvien, idkhachhang, idquyen) {
            return $http.post(urlApi + '/api/phanquyen/phanquyennhieunhomnhanvienkhachhang?idkhachhang=' + idkhachhang + '&idquyen=' + idquyen, listnhomnhanvien).then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_phanquyenkhongthanhcongvuilonglienhequantri') }
            });
        }

        function xoaphanquyennhomnhanvienkhachhang(listnhomnhanvien, idkhachhang) {
            return $http.post(urlApi + '/api/phanquyen/phanquyennhieunhomnhanvienkhachhang?idkhachhang=' + idkhachhang + '&idquyen=;', listnhomnhanvien).then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: $.i18n('label_xoaphanquyennhomnhanvienthanhcong') }
                } else {
                    return { flag: false, message: $.i18n('label_xoaphanquyenkhachhangchonhomnhanvienkhongthanhcong') }
                }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_xoaphanquyenkhachhangchonhomnhanvienkhongthanhcong') }
            });
        }

        function updatePhanQuyenByNhom(data) {
            return $http.post(urlApi + '/api/phanquyen/phanquyen_nhomnhanvienkhachhang', data).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_phanquyenkhongthanhcongvuilonglienhequantri')}
            });

        }

        function copyPhanQuyenByNhom(idkhnguon, idkhdich) {
            return $http.post(urlApi + '/api/phanquyen/saochepphanquyen_nhomnhanvienkhachhang?ID_NhomNguon=' + idkhnguon + '&ID_NhomDich=' + idkhdich).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_saochepphanquyenkhongthanhcongvuilonglienhequantri') }
            });

        }

        function getlistnhanvien(idnhom) {
            return $http.get(urlApi + '/api/phanquyen/getlistnhanvienphanquyen').then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getdskhachhangdacapquyen(idnhanvien) {
            return $http.get(urlApi + '/api/phanquyen/getlistkhachhangdaphanquyen?idnhanvien=' + idnhanvien).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getdskhachhangchuacapquyen(idnhanvien) {
            return $http.get(urlApi + '/api/khachhang/GetKhachHangChuaCapQuyen?idNhanvien=' + idnhanvien).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function phanquyenkhachhangchonhanvien(listkhachhang, idnhanvien, idquyen) {
            return $http.post(urlApi + '/api/phanquyen/phanquyenkhachhangchonhanvien?idnhanvien=' + idnhanvien + '&idquyen=' + idquyen, listkhachhang).then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_phanquyenkhongthanhcongvuilonglienhequantri') }
            });
        }

        function bophanquyenkhachhangchonhanvien(listkhachhang, idnhanvien) {
            return $http.post(urlApi + '/api/phanquyen/bophanquyenkhachhangchonhanvien?idnhanvien=' + idnhanvien, listkhachhang).then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_boquyenkhongthanhcongvuilonglienhequantri') }
            });
        }

    }

})();