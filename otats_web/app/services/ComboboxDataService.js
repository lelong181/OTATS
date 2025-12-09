(function () {
    'use strict';

    angular
        .module('app')
        .factory('ComboboxDataService', ComboboxDataService);

    ComboboxDataService.$inject = ['$http', '$cookies', '$rootScope', '$timeout'];
    function ComboboxDataService($http, $cookies, $rootScope, $timeout) {
        var service = {};

        service.getcauhinhchung = getcauhinhchung;
        service.getquyen = getquyen;

        service.getDataNhanVien = getDataNhanVien;
        service.getDataNhanVienByIdNhom = getDataNhanVienByIdNhom;
        service.getDataNhanVienDangNhapPhanMem = getDataNhanVienDangNhapPhanMem;
        service.getDataNhomNhanVien = getDataNhomNhanVien;
        service.getDataTreeNhomNhanVien = getDataTreeNhomNhanVien;
        service.getDataKhachHang = getDataKhachHang;
        service.getDataKhachHang_ServerPaging = getDataKhachHang_ServerPaging;
        service.getDataKhachHangByNhanVien = getDataKhachHangByNhanVien;
        service.getlistnhanvienbymultiidnhom = getlistnhanvienbymultiidnhom;

        service.getDataKhuVuc = getDataKhuVuc;
        service.getTinhThanh = getTinhThanh;
        service.getQuanHuyen = getQuanHuyen;
        service.getXaPhuong = getXaPhuong;
        service.getTuyen = getTuyen;
        service.getSite = getSite;

        service.getLoaiKhachHang = getLoaiKhachHang;
        service.getKenhBanHang = getKenhBanHang;
        service.getKenhBanHangCapTren = getKenhBanHangCapTren;

        service.getDataMatHang = getDataMatHang;
        service.getDataMatHangByIDNhom = getDataMatHangByIDNhom;
        service.getDataNhomMatHang = getDataNhomMatHang;
        service.getDataKhoHang = getDataKhoHang;
        service.getDataKhoHangByIDHang = getDataKhoHangByIDHang;
        service.getDataTreeNhomMatHang = getDataTreeNhomMatHang;
        service.getDataDonViTinh = getDataDonViTinh;
        service.getDataNhanHieu = getDataNhanHieu;
        service.getDataNhaCungCap = getDataNhaCungCap;

        service.getDataTrangThaiDonHang = getDataTrangThaiDonHang;
        service.getDataTrangThaiXemDonHang = getDataTrangThaiXemDonHang;
        service.getDataTrangThaiGiaoHangDonHang = getDataTrangThaiGiaoHangDonHang;
        service.getDataTrangThaiThanhToanDonHang = getDataTrangThaiThanhToanDonHang;
        service.getDataTrangThaiHoanTatDonHang = getDataTrangThaiHoanTatDonHang;
        service.getListKhuyenMai = getListKhuyenMai;

        
        return service;

        function getquyen(url) {
            return $http.get(URL_COMBOBOX_GETQUYEN + '?url=' + url).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: { iD_ChucNang: 0, them: 0, sua: 0, xoa: 0 } }
            });
        }

        function getcauhinhchung() {
            return $http.get(URL_COMBOBOX_GETCAUHINHCHUNG).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: {} }
            });
        }

        function getlistnhanvienbymultiidnhom(data) {
            return $http.post(URL_COMBOBOX_GETLISTNHANVIENBYMULTIIDNHOM, data).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getDataNhanVien() {
            return $http.get(URL_COMBOBOX_GETALLNHANVIEN).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getDataNhanVienByIdNhom(idnhom) {
            return $http.get(URL_COMBOBOX_GETNHANVIENBYIDNHOM + '?idNhom=' + idnhom).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getDataNhanVienDangNhapPhanMem(idnhom) {
            return $http.get(URL_COMBOBOX_GETLISTNHANVIENDANGNHAPPHANMEM + '?idnhom=' + idnhom).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getDataKhachHang() {
            return $http.get(URL_COMBOBOX_GETALLKHACHHANG).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getDataKhachHang_ServerPaging(take, skip) {
            return $http.get(URL_COMBOBOX_GETALLKHACHHANG_SERVERPAGING + '?take=' + take + '&skip=' + skip).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getDataKhachHangByNhanVien(iD_NhanVien) {
            return $http.get(URL_COMBOBOX_GETKHACHHANGBYIDNHANVIEN + '?ID_NhanVien=' + iD_NhanVien).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getDataNhomNhanVien() {
            return $http.get(URL_COMBOBOX_GETALLNHOMNHANVIEN).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getDataTreeNhomNhanVien() {
            return $http.get(URL_COMBOBOX_GETTREENHOMNHANVIEN).then(function (response) {
                let array = response.data;
                let d = array.filter(function (value, index, arr) {
                    return value.iD_Nhom > 0;
                });
                return { flag: true, data: d }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getDataKhuVuc() {
            return $http.get(URL_COMBOBOX_GETALLKHUVUC).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getTinhThanh() {
            return $http.get(URL_COMBOBOX_GETALLTINHTHANH).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getQuanHuyen(idtinh) {
            return $http.get(URL_COMBOBOX_GETQUANHUYENBYIDTINH + '?IdTinh=' + idtinh).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getXaPhuong(idquan) {
            return $http.get(URL_COMBOBOX_GETXAPHUONGBYIDQUAN + '?IdQuan=' + idquan).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getSite() {
            return $http.get(urlApi + '/api/danhmuc/getdssite').then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getTuyen() {
            return $http.get(URL_COMBOBOX_GETALLTUYENKHACHHANG).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getLoaiKhachHang() {
            return $http.get(URL_COMBOBOX_GETALLLOAIKHACHHANG).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getKenhBanHang() {
            return $http.get(URL_COMBOBOX_GETALLKENHBANHANG).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getKenhBanHangCapTren(idkenh) {
            return $http.get(URL_COMBOBOX_GETKENHBANHANGCAPTREN + '?idkenh=' + idkenh).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getDataMatHang() {
            return $http.get(URL_COMBOBOX_GETALLMATHANG).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getDataMatHangByIDNhom(idnhom) {
            return $http.get(URL_COMBOBOX_GETALLMATHANGBYIDNHOM + '?ID_DanhMuc=' + idnhom).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getDataNhomMatHang() {
            return $http.get(URL_COMBOBOX_GETALLNHOMMATHANG).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getDataKhoHang() {
            return $http.get(URL_COMBOBOX_GETALLKHOHANG).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getDataKhoHangByIDHang(idmathang) {
            return $http.get(URL_COMBOBOX_GETALLKHOHANGBYIDMATHANG + '?idmathang=' + idmathang).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getDataTreeNhomMatHang() {
            return $http.get(URL_COMBOBOX_GETTREENHOMMATHANG).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getDataDonViTinh() {
            return $http.get(URL_COMBOBOX_GETALLDONVITINH).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getDataNhanHieu() {
            return $http.get(URL_COMBOBOX_GETALLNHANHIEU).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getDataNhaCungCap() {
            return $http.get(URL_COMBOBOX_GETALLNHACUNGCAP).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getDataTrangThaiDonHang() {
            return $http.get(URL_COMBOBOX_GETALLTRANGTHAIDONHANG).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getDataTrangThaiXemDonHang(lang) {
            return $http.get(URL_COMBOBOX_GETALLTRANGTHAIXEMDONHANG + '?lang=' + lang).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getDataTrangThaiGiaoHangDonHang(lang) {
            return $http.get(URL_COMBOBOX_GETALLTRANGTHAIGIAOHANGDONHANG + '?lang=' + lang).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getDataTrangThaiThanhToanDonHang(lang) {
            return $http.get(URL_COMBOBOX_GETALLTRANGTHAITHANHTOANDONHANG + '?lang=' + lang).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getDataTrangThaiHoanTatDonHang(lang) {
            return $http.get(URL_COMBOBOX_GETALLTRANGTHAIHOANTATDONHANG + '?lang=' + lang).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getListKhuyenMai() {
            return $http.get(URL_COMBOBOX_GETLISTKHUYENMAI).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

    }

})();