(function () {
    'use strict';

    angular
        .module('app')
        .factory('danhMucDataService', danhMucDataService);

    danhMucDataService.$inject = ['$http', '$cookies', '$rootScope', '$timeout'];
    function danhMucDataService($http, $cookies, $rootScope, $timeout) {
        var service = {};

        service.getLoaiKhachHang = getLoaiKhachHang;
        service.getbyidloaiKhachHang = getbyidloaiKhachHang;
        service.deleteLoaiKhachHang = deleteLoaiKhachHang;
        service.saveLoaiKhachHang = saveLoaiKhachHang;

        service.getTrangThaiDonHang = getTrangThaiDonHang;
        service.getbyidTrangThaiDonHang = getbyidTrangThaiDonHang;
        service.deleteTrangThaiDonHang = deleteTrangThaiDonHang;
        service.saveTrangThaiDonHang = saveTrangThaiDonHang;

        service.getLoaiHaoHut = getLoaiHaoHut;
        service.getbyidLoaiHaoHut = getbyidLoaiHaoHut;
        service.deleteLoaiHaoHut = deleteLoaiHaoHut;
        service.saveLoaiHaoHut = saveLoaiHaoHut;

        service.getKhoHang = getKhoHang;
        service.getbyidKhoHang = getbyidKhoHang;
        service.deleteKhoHang = deleteKhoHang;
        service.saveKhoHang = saveKhoHang;

        service.getPhanHoiKhachHang = getPhanHoiKhachHang;
        service.getbyidPhanHoi = getbyidPhanHoi;
        service.deletePhanHoi = deletePhanHoi;
        service.savePhanHoi = savePhanHoi;

        service.getKenhBanHang = getKenhBanHang;
        service.getbyidKenhBanHang = getbyidKenhBanHang;
        service.deleteKenhBanHang = deleteKenhBanHang;
        service.saveKenhBanHang = saveKenhBanHang;
        service.comboDataKenhBanHang = comboDataKenhBanHang;


        service.getCheckList = getCheckList;
        service.getbyidCheckList = getbyidCheckList;
        service.deleteCheckList = deleteCheckList;
        service.saveCheckList = saveCheckList;

        service.getDonViTinh = getDonViTinh;
        service.getbyidDonViTinh = getbyidDonViTinh;
        service.deleteDonViTinh = deleteDonViTinh;
        service.saveDonViTinh = saveDonViTinh;

        service.getListNganhHang = getListNganhHang;
        service.deleteNganhHang = deleteNganhHang;
        service.saveNganhHang = saveNganhHang;

        service.getNhanHieu = getNhanHieu;
        service.deleteNhanHieu = deleteNhanHieu;
        service.saveNhanHieu = saveNhanHieu;

        service.getNhaCungCap = getNhaCungCap;
        service.deleteNhaCungCap = deleteNhaCungCap;
        service.saveNhaCungCap = saveNhaCungCap;

        service.xuLyNapVi = xuLyNapVi;
        service.getLichSuNapVi = getLichSuNapVi;

        service.getHinhThucThanhToan = getHinhThucThanhToan;
        service.luuHinhThucThanhToan = luuHinhThucThanhToan;
        service.deleteHinhThucThanhToan = deleteHinhThucThanhToan;

        return service;

        function getNhaCungCap() {
            return $http.get(urlApi + '/api/danhmuc/getlistnhacungcap').then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function deleteNhaCungCap(id) {
            return $http.post(urlApi + '/api/danhmuc/xoanhacungcap?ID=' + id).then(function (response) {
                if (response.data.success)
                    return { flag: true, message: response.data.msg }
                else
                    return { flag: false, message: response.data.msg }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_xoanhacungcapkhongthanhcongvuilonglienhequantri') }
            });

        }
        function saveNhaCungCap(data) {
            return $http.post(urlApi + '/api/danhmuc/themsuanhacungcap', data).then(function (response) {
                if (response.data.success)
                    return { flag: true, message: response.data.msg }
                else
                    return { flag: false, message: response.data.msg }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_luunhacungcapkhongthanhcongvuilonglienhequantri') }
            });

        }

        function getNhanHieu() {
            return $http.get(urlApi + '/api/danhmuc/getlistnhanhieu').then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function deleteNhanHieu(id) {
            return $http.post(urlApi + '/api/danhmuc/xoanhanhieu?ID=' + id).then(function (response) {
                if (response.data.success)
                    return { flag: true, message: response.data.msg }
                else
                    return { flag: false, message: response.data.msg }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_xoanhanhieukhongthanhcongvuilonglienhequantri') }
            });

        }
        function saveNhanHieu(data) {
            return $http.post(urlApi + '/api/danhmuc/themsuanhanhieu', data).then(function (response) {
                if (response.data.success)
                    return { flag: true, message: response.data.msg }
                else
                    return { flag: false, message: response.data.msg }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_luunhanhieukhongthanhcongvuilonglienhequantri') }
            });

        }

        function getLoaiKhachHang() {
            return $http.get(urlApi + '/api/loaikhachhang/getall').then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getbyidloaiKhachHang(idloaikhachhang) {
            return $http.get(urlApi + '/api/loaikhachhang/getbyid?ID=' + idloaikhachhang).then(function (response) {
                return {
                    flag: true, data: response.data,}
            }, function (response) {
                return { flag: false, data: [] }
                });
            
        }
        function deleteLoaiKhachHang(idloaikhachhang) {
            return $http.post(urlApi + '/api/loaikhachhang/delete?ID=' + idloaikhachhang).then(function (response) {
                if (response.data.success)
                    return { flag: true, message: response.data.msg }
                else
                    return { flag: false, message: response.data.msg }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_xoaloaikhachhangkhongthanhcongvuilonglienhequantri') }
            });

        }
        function saveLoaiKhachHang(data) {
            return $http.post(urlApi + '/api/loaikhachhang/themmoi', data).then(function (response) {
                if (response.data.success)
                    return { flag: true, message: response.data.msg }
                else
                    return { flag: false, message: response.data.msg }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_luuloaikhachhangkhongthanhcongvuilonglienhequantri') }
            });

        }

        function getTrangThaiDonHang() {
            return $http.get(urlApi + '/api/danhmuc/getall_trangthaidonhang').then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getbyidTrangThaiDonHang(idtrangthai) {
            return $http.post(urlApi + '/api/danhmuc/trangthaidonhang_theoid?ID=' + idtrangthai).then(function (response) {
                return {
                    flag: true, data: response.data,
                }
            }, function (response) {
                return { flag: false, data: [] }
            });

        }
        function deleteTrangThaiDonHang(idtrangthai) {
            return $http.post(urlApi + '/api/danhmuc/xoa_trangthaidonhang?ID=' + idtrangthai).then(function (response) {
                if (response.data.success)
                    return { flag: true, message: response.data.msg }
                else
                    return { flag: false, message: response.data.msg }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_xoatrangthaidonkhongthanhcongvuilonglienhequantri') }
            });

        }
        function saveTrangThaiDonHang(data) {
            return $http.post(urlApi + '/api/danhmuc/themsuatrangthaidonhang', data).then(function (response) {
                if (response.data.success)
                    return { flag: true, message: response.data.msg }
                else
                    return { flag: false, message: response.data.msg }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_luutrangthaidonhangkhongthanhcongvuilonglienhequantri')}
            });

        }

        function getLoaiHaoHut() {
            return $http.get(urlApi + '/api/danhmuc/getall_haohut').then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getbyidLoaiHaoHut(idhaohut) {
            return $http.post(urlApi + '/api/danhmuc/haohut_theoid?ID=' + idhaohut).then(function (response) {
                return {
                    flag: true, data: response.data,
                }
            }, function (response) {
                return { flag: false, data: [] }
            });

        }
        function deleteLoaiHaoHut(idhaohut) {
            return $http.post(urlApi + '/api/danhmuc/xoa_haohut?ID=' + idhaohut).then(function (response) {
                if (response.data.success)
                    return { flag: true, message: response.data.msg }
                else
                    return { flag: false, message: response.data.msg }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_xoaloaihaohutkhongthanhcongvuilonglienhequantri') }
            });

        }
        function saveLoaiHaoHut(data) {
            return $http.post(urlApi + '/api/danhmuc/themsuahaohut', data).then(function (response) {
                if (response.data.success)
                    return { flag: true, message: response.data.msg }
                else
                    return { flag: false, message: response.data.msg }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_luuloaihaohutkhongthanhcongvuilonglienhequantri') }
            });

        }

        function getKhoHang() {
            return $http.get(urlApi + '/api/danhmuc/getall_khohang').then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getbyidKhoHang(idkhohang) {
            return $http.post(urlApi + '/api/danhmuc/khohang_theoid?ID=' + idkhohang).then(function (response) {
                return {
                    flag: true, data: response.data,
                }
            }, function (response) {
                return { flag: false, data: [] }
            });

        }
        function deleteKhoHang(idkhohang) {
            return $http.post(urlApi + '/api/danhmuc/xoa_khohang?ID=' + idkhohang).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });

        }
        function saveKhoHang(data) {
            var url = "";
            if (data.ID_Kho == 0) {
                url = urlApi + '/api/danhmuc/them_khohang'
            } else {
                url = urlApi + '/api/danhmuc/sua_khohang'
            }
            return $http.post(url, data).then(function (response) {
                return {
                    flag: true, data: response.data,
                }
            }, function (response) {
                return { flag: false, data: [] }
            });

        }

        function getPhanHoiKhachHang() {
            return $http.get(urlApi + '/api/danhmuc/getall_phanhoi').then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getbyidPhanHoi(idphanhoi) {
            return $http.post(urlApi + '/api/danhmuc/phanhoi_theoid?ID=' + idphanhoi).then(function (response) {
                return {
                    flag: true, data: response.data,
                }
            }, function (response) {
                return { flag: false, data: [] }
            });

        }
        function deletePhanHoi(idphanhoi) {
            return $http.post(urlApi + '/api/danhmuc/xoa_phanhoi?ID=' + idphanhoi).then(function (response) {
                if (response.data.success)
                    return { flag: true, message: response.data.msg }
                else
                    return { flag: false, message: response.data.msg }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_xoaphanhoikhongthanhcongvuilonglienhequantri') }
            });

        }
        function savePhanHoi(data) {
            return $http.post(urlApi + '/api/danhmuc/themsuaphanhoi', data).then(function (response) {
                if (response.data.success)
                    return { flag: true, message: response.data.msg }
                else
                    return { flag: false, message: response.data.msg }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_themphanhoikhongthanhcongvuilonglienhequantri') }
            });

        }

        function getKenhBanHang() {
            return $http.get(urlApi + '/api/danhmuc/getlistkenhbanhang').then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getbyidKenhBanHang(idkenhbanhang) {
            return $http.post(urlApi + '/api/danhmuc/kenhbanhang_theoid?ID=' + idkenhbanhang).then(function (response) {
                return {
                    flag: true, data: response.data,
                }
            }, function (response) {
                return { flag: false, data: [] }
            });

        }
        function deleteKenhBanHang(idkenhbanhang) {
            return $http.post(urlApi + '/api/danhmuc/xoa_kenhbanhang?ID=' + idkenhbanhang).then(function (response) {
                if (response.data.success)
                    return { flag: true, message: response.data.msg }
                else
                    return { flag: false, message: response.data.msg }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_xoakenhbanhangkhongthanhcongvuilonglienhequantri') }
            });

        }
        function saveKenhBanHang(data) {
            return $http.post(urlApi + '/api/danhmuc/themsuakenhbanhang', data).then(function (response) {
                if (response.data.success)
                    return { flag: true, message: response.data.msg }
                else
                    return { flag: false, message: response.data.msg }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_luukenhbanhangkhongthanhcongvuilonglienhequantri') }
            });

        }
        function comboDataKenhBanHang() {
            return $http.get(urlApi + '/api/danhmuc/combodata_kenhbanhang').then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getCheckList() {
            return $http.get(urlApi + '/api/danhmuc/getall_checklist').then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getbyidCheckList(idchecklist) {
            return $http.post(urlApi + '/api/danhmuc/checklist_theoid?ID=' + idchecklist).then(function (response) {
                return {
                    flag: true, data: response.data,
                }
            }, function (response) {
                return { flag: false, data: [] }
            });

        }
        function deleteCheckList(idchecklist) {
            return $http.post(urlApi + '/api/danhmuc/xoa_checklist?ID=' + idchecklist).then(function (response) {
                if (response.data.success)
                    return { flag: true, message: response.data.msg }
                else
                    return { flag: false, message: response.data.msg }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_xoachecklistkhongthanhcongvuilonglienhequantri') }
            });

        }
        function saveCheckList(data) {
            return $http.post(urlApi + '/api/danhmuc/themsuachecklist', data).then(function (response) {
                if (response.data.success)
                    return { flag: true, message: response.data.msg }
                else
                    return { flag: false, message: response.data.msg }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_luuchecklistkhongthanhcongvuilonglienhequantri') }
            });

        }

        function getDonViTinh() {
            return $http.get(urlApi + '/api/danhmuc/getall_donvi').then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getbyidDonViTinh(idDvtinh) {
            return $http.post(urlApi + '/api/danhmuc/donvi_theoid?ID=' + idDvtinh).then(function (response) {
                return {
                    flag: true, data: response.data,
                }
            }, function (response) {
                return { flag: false, data: [] }
            });

        }
        function deleteDonViTinh(idDvtinh) {
            return $http.post(urlApi + '/api/danhmuc/xoa_donvi?ID=' + idDvtinh).then(function (response) {
                if (response.data.success)
                    return { flag: true, message: response.data.msg }
                else
                    return { flag: false, message: response.data.msg }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_xoadonvitinhkhongthanhcongvuilonglienhequantri') }
            });
        }
        function saveDonViTinh(data) {
            return $http.post(urlApi + '/api/danhmuc/themsuadonvi', data).then(function (response) {
                if (response.data.success)
                    return { flag: true, message: response.data.msg }
                else
                    return { flag: false, message: response.data.msg }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_luudonvitinhkhongthanhcongvuilonglienhequantri') }
            });

        }

        function getListNganhHang() {
            return $http.get(urlApi + '/api/danhmuc/getlistnganhhang').then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function deleteNganhHang(id) {
            return $http.post(urlApi + '/api/danhmuc/xoanganhhang?ID=' + id).then(function (response) {
                if (response.data.success)
                    return { flag: true, message: response.data.msg }
                else
                    return { flag: false, message: response.data.msg }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_xoanganhhangkhongthanhcongvuilonglienhequantri') }
            });

        }
        function saveNganhHang(data) {
            return $http.post(urlApi + '/api/danhmuc/themsuanganhhang', data).then(function (response) {
                if (response.data.success)
                    return { flag: true, message: response.data.msg }
                else
                    return { flag: false, message: response.data.msg }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_luunganhhangkhongthanhcongvuilonglienhequantri') }
            });

        }

        function getLichSuNapVi(id) {
            return $http.get(urlApi + '/api/danhmuc/getlsnapvi_nhacungcap?ID=' + id).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function xuLyNapVi(data) {
            return $http.post(urlApi + '/api/danhmuc/xuLyNapVi', data).then(function (response) {
                if (response.data.success)
                    return { flag: true, message: response.data.msg }
                else
                    return { flag: false, message: response.data.msg }
            }, function (response) {
                return { flag: false, message: $.i18n('label_luunhacungcapkhongthanhcongvuilonglienhequantri') }
            });

        }

        function getHinhThucThanhToan() {
            return $http.get(urlApi + '/api/danhmuc/gethinhthucthanhtoan').then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function luuHinhThucThanhToan(data) {
            return $http.post(urlApi + '/api/danhmuc/luuhinhthucthanhtoan', data).then(function (response) {
                if (response.data.success)
                    return { flag: true, message: response.data.msg }
                else
                    return { flag: false, message: response.data.msg }
            }, function (response) {
                return { flag: false, message: $.i18n('label_luunhacungcapkhongthanhcongvuilonglienhequantri') }
            });

        }

        function deleteHinhThucThanhToan(id) {
            return $http.post(urlApi + '/api/danhmuc/xoahinhthucthanhtoan?ID=' + id).then(function (response) {
                if (response.data.success)
                    return { flag: true, message: response.data.msg }
                else
                    return { flag: false, message: response.data.msg }
            }, function (response) {
                return { flag: false, message: $.i18n('label_luunhacungcapkhongthanhcongvuilonglienhequantri') }
            });

        }
    }

})();