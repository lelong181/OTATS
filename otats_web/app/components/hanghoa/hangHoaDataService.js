(function () {
    'use strict';

    angular
        .module('app')
        .factory('hangHoaDataService', hangHoaDataService);

    hangHoaDataService.$inject = ['$http', '$cookies', '$rootScope', '$timeout'];
    function hangHoaDataService($http, $cookies, $rootScope, $timeout) {
        var service = {};
        service.getLoaiKhachHangByIdMatHang = getLoaiKhachHangByIdMatHang;
        service.hoantatGiaTheoLoaiKH = hoantatGiaTheoLoaiKH;
        service.getBangGia = getBangGia;
        service.getBangLoiNhuan = getBangLoiNhuan;
        service.setBangLoiNhuan = setBangLoiNhuan;
        service.setTrangThaiBangLoiNhuan = setTrangThaiBangLoiNhuan;
        service.guiBanMatHang = guiBanMatHang;
        service.matHangChuaGuiBan = matHangChuaGuiBan;
        service.matHangDaGuiBan = matHangDaGuiBan;
        service.luuThayDoi = luuThayDoi;
        service.getbyid = getbyid;
        service.getlist = getlist;
        service.getListNhomMatHang = getListNhomMatHang;
        service.taiFileMau = taiFileMau;
        service.taiFileMauBangGia = taiFileMauBangGia;
        service.importmathang = importmathang;
        service.importbanggiamathang = importbanggiamathang;
        service.themsuahang = themsuahang;
        service.xoahang = xoahang;
        service.themsuanhomhang = themsuanhomhang;
        service.xoanhomhang = xoanhomhang;
        service.uploadAnhDaiDien = uploadAnhDaiDien;
        service.exportExcel = exportExcel;
        service.dongbodichvu = dongbodichvu;
        service.getalldichvu = getalldichvu;

        return service;

        function exportExcel(ID_DanhMuc) {
            return $http.get(urlApi + '/api/mathang/ExportExcelMatHang?ID_DanhMuc=' + ID_DanhMuc, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }

        function getLoaiKhachHangByIdMatHang(id) {
            return $http.get(urlApi + '/api/banggialoaikhachhang/getloaikhachhang?idMatHang=' + id).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function hoantatGiaTheoLoaiKH(obj) {
            return $http.post(urlApi + '/api/banggialoaikhachhang/hoantat', obj).then(function (response) {
                return { flag: true, message: $.i18n('label_luuthanhcong') }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_luukhongthanhcong') }
            });
        }
        function getBangGia(id) {
            return $http.get(urlApi + '/api/banggialoaikhachhang/getbanggia?idMatHang=' + id).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function setBangLoiNhuan(obj) {
            return $http.post(urlApi + '/api/mathang/setloinhuanbymathang', obj).then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                return { flag: false, message: $.i18n('label_luumathangkhongthanhcongvuilonglienhequantri') }
            });
        }

        function setTrangThaiBangLoiNhuan(obj) {
            return $http.post(urlApi + '/api/mathang/updatetrangthailoinhuanbymathang', obj).then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                return { flag: false, message: $.i18n('label_luumathangkhongthanhcongvuilonglienhequantri') }
            });
        }

        function getBangLoiNhuan(id) {
            return $http.get(urlApi + '/api/mathang/getloinhuanbymathang?iD_MatHang=' + id).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function guiBanMatHang() {
            return $http.get(urlApi + '/api/khachhang/getall').then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function matHangChuaGuiBan(idkhachhang) {
            return $http.get(urlApi + '/api/mathang/getbykhachhang?idKhachHang='+idkhachhang).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function matHangDaGuiBan(idkhachhang) {
            return $http.get(urlApi + '/api/guibanmathang/getdsmtahangbykhach?ID_KhachHang=' + idkhachhang).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function luuThayDoi(data) {
            return $http.post(urlApi + '/api/guibanmathang/hoantat', data).then(function (response) {
                return { flag: true, message: $.i18n('label_luuthanhcong')}
            }, function (response) {
                    return { flag: false, message: $.i18n('label_luukhongthanhcong') }
            });
        }

        function getbyid(id) {
            return $http.get(urlApi + '/api/mathang/getbyid?ID_MatHang=' + id).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getlist(idnhom) {
            return $http.get(urlApi + '/api/mathang/getallmathangNew?ID_DanhMuc=' + idnhom).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getListNhomMatHang() {
            return $http.get(urlApi + '/api/mathang/getnhommathang').then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function dongbodichvu() {
            return $http.get(urlApi + '/api/mathang/dongbodichvu').then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getalldichvu() {
            return $http.get(urlApi + '/api/mathang/getalldichvu').then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function taiFileMau() {
            return $http.get(urlApi + '/api/mathang/GetTamPlateMatHang_New', { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }

        function taiFileMauBangGia(idNhom) {
            return $http.get(urlApi + '/api/mathang/Getteamplate_GiaMatHang_New?idNhom=' + idNhom, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }

        function importmathang(fileUpload) {
            return $http.post(urlApi + '/api/mathang/importmathang', { filename: fileUpload }, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }

        function importbanggiamathang(fileUpload) {
            return $http.post(urlApi + '/api/mathang/importbanggiamathang', { filename: fileUpload }, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }

        function themsuahang(obj) {
            return $http.post(urlApi + '/api/mathang/themsuamathang', obj).then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_luumathangkhongthanhcongvuilonglienhequantri') }
            });
        }

        function xoahang(obj) {
            return $http.post(urlApi + '/api/mathang/deletemathang', obj).then(function (response) {
                if (response.data) {
                    return { flag: true, message: $.i18n('label_xoamathangthanhcong') }
                } else {
                    return { flag: false, message: $.i18n('label_xoamathangthatbai') }
                }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_xoamathangthatbaivuilonglienhequantri') }
            });
        }

        function themsuanhomhang(obj) {
            return $http.post(urlApi + '/api/mathang/themsuanhom', obj).then(function (response) {
                return { flag: true, message: $.i18n('label_luunhommathangthanhcong') }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_luunhommathangkhongthanhcongvuilonglienhequantri') }
            });
        }

        function xoanhomhang(idnhom) {
            return $http.post(urlApi + '/api/mathang/xoanhom?ID_Nhom=' + idnhom).then(function (response) {
                return { flag: true, message: $.i18n('label_xoanhomthanhcong') }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_xoanhomthatbaivuilongthulai') }
            });
        }

        function uploadAnhDaiDien(data) {
            return $http.post(urlApi + '/api/uploadfile/savefile', data,
                {
                    transformRequest: angular.identity,
                    headers: { 'Content-Type': undefined }
                }).then(function (response) {
                    if (response != null) {
                        return { flag: true, url: response.data, message: $.i18n('label_taianhthanhcong') }
                    } else {
                        return { flag: false, url: '', message: $.i18n('label_taianhthatbaixinvuilongthulai') }
                    }
                }, function (response) {
                        return { flag: false, url: '', message: $.i18n('label_taianhthatbaixinvuilongthulai') }
                });
        }
    }

})();