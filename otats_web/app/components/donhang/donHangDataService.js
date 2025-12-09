(function () {
    'use strict';

    angular
        .module('app')
        .factory('donHangDataService', donHangDataService);

    donHangDataService.$inject = ['$http', '$cookies', '$rootScope', '$timeout'];
    function donHangDataService($http, $cookies, $rootScope, $timeout) {
        var service = {};

        service.getById = getById;
        service.getsite = getsite;
        service.getallsite = getallsite;
        service.getChiTietHangHoa = getChiTietHangHoa;
        service.getChiTietDichVu = getChiTietDichVu;
        service.getLichSuThanhToan = getLichSuThanhToan;
        service.getLichSuGiaoHang = getLichSuGiaoHang;
        service.getNhanVienPhanQuyen = getNhanVienPhanQuyen;
        service.getLichSuTraHang = getLichSuTraHang;
        service.getChiTietLichSuTraHang = getChiTietLichSuTraHang;

        service.huydonhang = huydonhang;
        service.updateMaVeKhac = updateMaVeKhac;
        service.updateGroupLink = updateGroupLink;
        service.giaHanVe = giaHanVe;
        service.updateTrangThaiTheDo = updateTrangThaiTheDo;
        service.giaohang = giaohang;
        service.phanquyen = phanquyen;
        service.thanhtoan = thanhtoan;
        service.thanhtoanvi = thanhtoanvi;
        service.xuLyDonHang = xuLyDonHang;
        service.capnhatdsve = capnhatdsve;
        service.trahang = trahang;
        service.save = save;
        service.savedv = savedv;
        service.checkTheDo = checkTheDo;

        service.getlist = getlist;
        service.getlistbyhdv = getlistbyhdv;

        service.exportExcel = exportExcel;
        service.exportExcelDetail = exportExcelDetail;

        service.getcombonhanvienlap = getcombonhanvienlap;
        service.getcombokhuyenmai = getcombokhuyenmai;
        service.getcombohaohut = getcombohaohut;
        service.getdanhsachmathangbyidkhachhangidnhom = getdanhsachmathangbyidkhachhangidnhom;
        service.getdanhsachmathangbyidkhachhangmultiidnhom = getdanhsachmathangbyidkhachhangmultiidnhom;
        service.getnhommathangtheophanquyen = getnhommathangtheophanquyen;
        service.getlistkhachhangbyidnhanvien = getlistkhachhangbyidnhanvien;
        service.themdonhang = themdonhang;
        service.themdonhangdichvu = themdonhangdichvu;
        service.themvathanhtoandonhang = themvathanhtoandonhang;
        service.themkhachhangtudonhang = themkhachhangtudonhang;
        service.timkiemkhachhang_accountcode = timkiemkhachhang_accountcode;
        service.uploadAnhThanhToan = uploadAnhThanhToan;
        service.checkAnhThanhToan = checkAnhThanhToan;
        service.getTongSoDu = getTongSoDu;
        service.getQRDynamic = getQRDynamic;
        service.gethttt = gethttt;
        return service;

        function themkhachhangtudonhang(obj) {
            return $http.post(urlApi + '/api/khachhang/themmoi', obj).then(function (response) {
                if (response.data.success) {
                    return { flag: true, data: response.data.data, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                return { flag: false, message: $.i18n('label_luukhachhangkhongthanhcongvuilonglienhequantri') }
            });
        }

        function themdonhang(obj) {
            return $http.post(urlApi + '/api/donhang/taoMoiDonHang', obj).then(function (response) {
                if (response.data) {
                    return { flag: true, message: $.i18n('label_taodonhangthanhcong') }
                } else {
                    return { flag: false, message: $.i18n('label_taodonhangkhongthanhcong') }
                }
            }, function (response) {
                return { flag: false, message: $.i18n('label_luudonhangkhongthanhcongvuilonglienhequantri') }
            });
        }


        function themdonhangdichvu(obj) {
            return $http.post(urlApi + '/api/donhang/taoMoiDonHangDichVu', obj).then(function (response) {
                if (response.data) {
                    return { flag: true, data: response.data, message: $.i18n('label_taodonhangthanhcong') }
                } else {
                    return { flag: false, message: $.i18n('label_taodonhangkhongthanhcong') }
                }
            }, function (response) {
                return { flag: false, message: $.i18n('label_luudonhangkhongthanhcongvuilonglienhequantri') }
            });
        }

        function themvathanhtoandonhang(obj) {
            return $http.post(urlApi + '/api/donhang/taoMoiVaThanhToanDonHang', obj).then(function (response) {
                console.log(response);
                if (response.data) {
                    return { flag: true, message: $.i18n('label_taodonhangthanhcong'), data: response.data }
                } else {
                    return { flag: false, message: $.i18n('label_taodonhangkhongthanhcong') }
                }
            }, function (response) {
                return { flag: false, message: $.i18n('label_luudonhangkhongthanhcongvuilonglienhequantri') }
            });
        }

        function getlistkhachhangbyidnhanvien(idnhanvien) {
            return $http.get(urlApi + '/api/khachhang/getDanhSachKhachHangTheoIdNhanVien?idNhanVien=' + idnhanvien).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function timkiemkhachhang_accountcode(accountCode) {
            return $http.get(urlApi + '/api/donhang/timkiemkhachhang_accountcode?accountCode=' + accountCode).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getnhommathangtheophanquyen() {
            return $http.get(urlApi + '/api/mathang/getnhommathangtheophanquyen').then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getcombohaohut() {
            return $http.get(urlApi + '/api/donhang/danhSachHaoHut').then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getcombokhuyenmai(loai) {
            return $http.get(urlApi + '/api/donhang/getListKhuyenMai?loai=' + loai).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getcombonhanvienlap() {
            return $http.get(urlApi + '/api/nhanvienapp/getnhanvienbodauxoa').then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function gethttt(idnhanvien) {
            return $http.get(urlApi + '/api/nhanvienapp/GetNV_ListHTTT?IDNV=' + idnhanvien).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getdanhsachmathangbyidkhachhangidnhom(idkhachhang, idnhom) {
            return $http.get(urlApi + '/api/donhang/gethanghoabyidkhachhangidnhom?idKhachHang=' + idkhachhang + "&ID_DANHMUC=" + idnhom).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getdanhsachmathangbyidkhachhangmultiidnhom(idkhachhang, idnhom) {
            return $http.get(urlApi + '/api/donhang/gethanghoabyidkhachhangmultiidnhom?idKhachHang=' + idkhachhang + "&ID_DANHMUC=" + idnhom).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getById(iddonhang) {
            return $http.get(urlApi + '/api/donhang/chiTietDonHang?idDonHang=' + iddonhang).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getChiTietHangHoa(iddonhang) {
            return $http.get(urlApi + '/api/donhang/danhSachChiTietDonHang?idDonHang=' + iddonhang).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getChiTietDichVu(iddonhang) {
            return $http.get(urlApi + '/api/donhang/danhSachDichVuDonHang?idDonHang=' + iddonhang).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getLichSuThanhToan(iddonhang) {
            return $http.get(urlApi + '/api/donhang/getDanhSachThanhToan?idDonHang=' + iddonhang).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getLichSuGiaoHang(iddonhang) {
            return $http.get(urlApi + '/api/donhang/lichSuGiaoHang?idDonHang=' + iddonhang).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getNhanVienPhanQuyen(iddonhang) {
            return $http.get(urlApi + '/api/donhang/getlistnhanvienphanquyen?idDonHang=' + iddonhang).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getLichSuTraHang(iddonhang) {
            return $http.get(urlApi + '/api/donhang/danhSachLichSuTraHang?idDonHang=' + iddonhang).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getChiTietLichSuTraHang(idHangTra) {
            return $http.get(urlApi + '/api/donhang/GetChiTietHangTraById?idHangTra=' + idHangTra).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getlist(data) {
            return $http.post(urlApi + '/api/donhang/getlistdonghang', data).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getlistbyhdv(data) {
            return $http.post(urlApi + '/api/donhang/getlistdonhangbyhdv', data).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getsite(data) {
            return $http.get(urlApi + '/api/donhang/getsite?sitecode=' + data).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getallsite(data) {
            return $http.get(urlApi + '/api/donhang/getallsite').then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function huydonhang(id_donhang, lydo) {
            return $http.get(urlApi + '/api/donhang/huydonhang?id_donhang=' + id_donhang + '&lydo=' + lydo).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function updateMaVeKhac(id, mave) {
            return $http.get(urlApi + '/api/donhang/updateMaVeKhac?ID_ChiTiet_MatHang_DonHang=' + id + '&MaVeKhac=' + mave).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function updateGroupLink(obj) {
            return $http.post(urlApi + '/api/donhang/updateGroupLink', obj).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function giaHanVe(id, ngay) {
            return $http.get(urlApi + '/api/donhang/giaHanVe?id_donhang=' + id + '&ngay=' + ngay).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function checkTheDo(id) {
            return $http.get(urlApi + '/api/donhang/checkTrangThaiTheDo?ID_TheDo=' + id).then(function (response) {
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

        function giaohang(obj) {
            return $http.post(urlApi + '/api/donhang/giaoHang', obj).then(function (response) {
                let data = JSON.parse(response.data);
                if (data.status) {
                    return { flag: true, message: $.i18n("label_giaohangthanhcong") }
                } else {
                    return { flag: false, message: $.i18n("label_soluongmathanggiaovuotquasoluongcuakhodachon") }
                }
            }, function (response) {
                return { flag: false, message: $.i18n('label_giaohangkhongthanhcongvuilonglienhequantri') }
            });
        }

        function phanquyen(obj) {
            return $http.post(urlApi + '/api/donhang/updateQuyenChoNhanVienTheoDonHang', obj).then(function (response) {
                return { flag: true, message: "label_phanquyenthanhcong" }
            }, function (response) {
                return { flag: false, message: $.i18n('label_phanquyenkhongthanhcongvuilonglienhequantri') }
            });
        }

        function thanhtoan(obj) {
            return $http.post(urlApi + '/api/donhang/thanhToanDonHang', obj).then(function (response) {
                let data = JSON.parse(response.data);
                if (data.status) {
                    return { flag: true, message: data.msg }
                } else {
                    return { flag: false, message: data.msg }
                }
            }, function (response) {
                return { flag: false, message: $.i18n('label_thanhtoankhongthanhcongvuilonglienhequantri') }
            });
        }

        function thanhtoanvi(obj) {
            return $http.post(urlApi + '/api/donhang/thanhToanDonHangLspay', obj).then(function (response) {
                let data = JSON.parse(response.data);
                if (data.status) {
                    return { flag: true, message: data.msg }
                } else {
                    return { flag: false, message: data.msg }
                }
            }, function (response) {
                return { flag: false, message: $.i18n('label_thanhtoankhongthanhcongvuilonglienhequantri') }
            });
        }

        function xuLyDonHang(obj) {
            return $http.get(urlApi + '/api/donhang/xuLyDonHang?ID_DonHang=' + obj).then(function (response) {
                let data = JSON.parse(response.data);
                if (data.status) {
                    return { flag: true, message: data.msg }
                } else {
                    return { flag: false, message: data.msg }
                }
            }, function (response) {
                return { flag: false, message: $.i18n('label_thanhtoankhongthanhcongvuilonglienhequantri') }
            });
        }

        function capnhatdsve(obj) {
            return $http.get(urlApi + '/api/donhang/capNhatDsVe?ID_DonHang=' + obj).then(function (response) {
                let data = JSON.parse(response.data);
                if (data.status) {
                    return { flag: true, message: data.msg }
                } else {
                    return { flag: false, message: data.msg }
                }
            }, function (response) {
                return { flag: false, message: $.i18n('label_thanhtoankhongthanhcongvuilonglienhequantri') }
            });
        }

        function trahang(obj) {
            return $http.post(urlApi + '/api/donhang/thucHienTraHang', obj).then(function (response) {
                return { flag: true, message: $.i18n("label_trahangthanhcong") }
            }, function (response) {
                return { flag: false, message: $.i18n("label_trahangkhongthanhcong") }
            });
        }

        //cần sửa lại lấy message từ server
        function save(obj) {
            return $http.post(urlApi + '/api/donhang/suaDonHang', obj).then(function (response) {
                if (response.data) {
                    return { flag: true, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }

        function savedv(obj) {
            return $http.post(urlApi + '/api/donhang/suaDonHangDV', obj).then(function (response) {
                if (response.data) {
                    return { flag: true, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }

        function exportExcel(data) {
            return $http.post(urlApi + '/api/donhang/BaoCaoDonHangDanhSach', data, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }

        function exportExcelDetail(data) {
            return $http.post(urlApi + '/api/donhang/BaocaoDonHangChiTiet', data, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }

        function checkAnhThanhToan(data) {
            return $http.post(urlApi + '/api/uploadfile/checkAnhThanhToan', data).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }
        function uploadAnhThanhToan(data) {
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
        function getTongSoDu(id_nhom) {
            return $http.post(urlApi + '/api/donhang/GetTongSoDu?id_nhom=' + id_nhom).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getQRDynamic(code, iddonhang, sotien) {
            return $http.get(urlApi + '/api/ota/paycollect_createpayment_trangan?Code=' + code + '&ID_DonHang=' + iddonhang + '&SoTien=' + sotien).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
    }

})();