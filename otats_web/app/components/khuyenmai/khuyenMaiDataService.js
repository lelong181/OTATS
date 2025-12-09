(function () {
    'use strict';

    angular
        .module('app')
        .factory('khuyenMaiDataService', khuyenMaiDataService);

    khuyenMaiDataService.$inject = ['$http', '$rootScope', '$timeout'];
    function khuyenMaiDataService($http, $rootScope, $timeout) {
        var service = {};

        service.getbyid = getbyid;
        service.getlist = getlist;
        service.getdonhangkhuyenmai = getdonhangkhuyenmai;
        service.getlistLoaiKhuyenMai = getlistLoaiKhuyenMai;
        service.getListNhomMatHang = getListNhomMatHang;
        service.getlisthangchon = getlisthangchon;
        service.getlistmathangctkm = getlistmathangctkm;
        service.GetChiTietHangTang = GetChiTietHangTang;
        service.Getlisthangtangbyidmathang = Getlisthangtangbyidmathang;
        service.getlisthangtangchon = getlisthangtangchon;
        service.suakhuyenmai = suakhuyenmai;
        service.themkhuyenmai = themkhuyenmai;
        service.ngungsudung = ngungsudung;
        service.xoachuongtrinhkhuyenmai = xoachuongtrinhkhuyenmai;
        service.uploadAnhDaiDien = uploadAnhDaiDien;
        service.exportExcel = exportExcel;


        return service;

        function getdonhangkhuyenmai(idctkm) {
            return $http.get(urlApi + '/api/ChuongTrinhKhuyenMai/GetDonHangKhuyenMai?ID_CTKM=' + idctkm).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: { donhang: [], chitiet: [] } }
            });
        }

        function getbyid(idctkm) {
            return $http.get(urlApi + '/api/ChuongTrinhKhuyenMai/GetKhuyenMaiByID?ID_CTKM=' + idctkm).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                let d = new Date();
                return { flag: false, data: { ngayApDung: d, ngayKetThuc: d, loai: 1, chiTietCTKM: []} }
            });
        }

        function getlist(iddonhang) {
            return $http.get(urlApi + '/api/ChuongTrinhKhuyenMai/GetAll').then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getlistLoaiKhuyenMai() {
            return $http.get(urlApi + '/api/ChuongTrinhKhuyenMai/getallhinhthuc').then(function (response) {
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

        function getlisthangchon(idnhom) {
            return $http.get(urlApi + '/api/mathang/getallmathangNew?ID_DanhMuc=' + idnhom).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getlistmathangctkm(idctkm, idnhom) {
            return $http.get(urlApi + '/api/ChuongTrinhKhuyenMai/getmathangctkm?ID_CTKM=' + idctkm + '&ID_DANHMUC=' + idnhom).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function GetChiTietHangTang(idctkm) {
            return $http.get(urlApi + '/api/ChuongTrinhKhuyenMai/GetChiTietHangTang?ID_CTKM=' + idctkm).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function Getlisthangtangbyidmathang(idctkm, idmathang) {
            return $http.get(urlApi + '/api/ChuongTrinhKhuyenMai/GetListHangTangByMatHang?ID_CTKM=' + idctkm + '&idmathang=' + idmathang).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getlisthangtangchon(idnhom) {
            return $http.get(urlApi + '/api/mathang/getallmathangNew?ID_DanhMuc=' + idnhom).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function suakhuyenmai(obj) {
            return $http.post(urlApi + '/api/ChuongTrinhKhuyenMai/SuaKhuyenMai_v2', obj).then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_luuchuongtrinhkhuyenmaikhongthanhcongvuilonglienhequantri') }
            });
        }

        function themkhuyenmai(obj) {
            return $http.post(urlApi + '/api/ChuongTrinhKhuyenMai/ThemKhuyenMai', obj).then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_luuchuongtrinhkhuyenmaikhongthanhcongvuilonglienhequantri') }
            });
        }

        function ngungsudung(idctkm) {
            return $http.post(urlApi + '/api/ChuongTrinhKhuyenMai/ngungsudung?ID_CTKM=' + idctkm).then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_ngungchuongtrinhkhuyenmaikhongthanhcongvuilonglienhequantri') }
            });
        }

        function xoachuongtrinhkhuyenmai(idctkm) {
            return $http.post(urlApi + '/api/ChuongTrinhKhuyenMai/xoachuongtrinhkhuyenmai?ID_CTKM=' + idctkm).then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_xoachuongtrinhkhuyenmaikhongthanhcongvuilonglienhequantri') }
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

        function exportExcel() {
            return $http.get(urlApi + '/api/ChuongTrinhKhuyenMai/ExportExcelKhuyenMai', { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }
    }

})();