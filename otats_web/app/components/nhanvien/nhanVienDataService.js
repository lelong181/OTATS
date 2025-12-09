(function () {
    'use strict';

    angular
        .module('app')
        .factory('nhanVienDataService', nhanVienDataService);

    nhanVienDataService.$inject = ['$http', '$cookies', '$rootScope', '$timeout'];
    function nhanVienDataService($http, $cookies, $rootScope, $timeout) {
        var service = {};

        service.getById = getById;
        service.saveedit = saveedit;
        service.saveinsert = saveinsert;
        service.saveinsertnhanvienkhongdangnhap = saveinsertnhanvienkhongdangnhap;
        service.del = del;
        service.resetimei = resetimei;
        service.resetpass = resetpass;
        service.getlist = getlist;
        service.getListNhomNhanVien = getListNhomNhanVien;
        service.exportExcel = exportExcel;
        service.taiFileMau = taiFileMau;
        service.importnhanvien = importnhanvien;

        service.getlistkhachhangcapquyen = getlistkhachhangcapquyen;
        service.getlistkhachhangchon = getlistkhachhangchon;
        service.addphanquyen = addphanquyen;
        service.removephanquyen = removephanquyen;

        service.saveeditnhomnhanvien = saveeditnhomnhanvien;
        service.saveinsertnhomnhanvien = saveinsertnhomnhanvien;
        service.delnhomnhanvien = delnhomnhanvien;
        service.uploadAnhDaiDien = uploadAnhDaiDien;

        service.dsnhanvien = dsnhanvien;
        service.dskhachhang = dskhachhang;
        service.dsnhanvien_capnhat = dsnhanvien_capnhat;

        service.gethinhthucthanhtoan = gethinhthucthanhtoan;
        service.luuNV_HinhThucTT = luuNV_HinhThucTT;
        service.getHinhThucThanhToanByNV = getHinhThucThanhToanByNV;

        return service;

        function dsnhanvien() {
            return $http.get(urlApi + '/api/chuyenquyen/getlistnhanvien').then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function dskhachhang(idnhanvien) {
            return $http.get(urlApi + '/api/chuyenquyen/getlistkhachhangbyidnhanvien?idnhanvien=' + idnhanvien).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function dsnhanvien_capnhat(obj) {
            return $http.post(urlApi + '/api/chuyenquyen/capnhat',obj).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getById(idnhanvien) {
            return $http.get(urlApi + '/api/nhanvienapp/getnvbyId?ID_NV=' + idnhanvien).then(function (response) {
                if (response.data)
                    return { flag: true, data: response.data }
                else
                    return { flag: false, data: {} }
            }, function (response) {
                return { flag: false, data: {} }
            });
        }

        function getlist(idnhom) {
            return $http.get(urlApi + '/api/nhanvienapp/getall?IdNhom=' + idnhom).then(function (response) {
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

        function exportExcel(data) {
            return $http.post(urlApi + '/api/nhanvienapp/ExportExcelNhanVien', data, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }

        function taiFileMau() {
            return $http.get(urlApi + '/api/nhanvienapp/ExportTeamplateNhanVien', { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }

        function importnhanvien(fileUpload) {
            return $http.post(urlApi + '/api/nhanvienapp/importnhanvien', { filename: fileUpload }, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }

        function getlistkhachhangcapquyen(idnhanvien) {
            return $http.get(urlApi + '/api/nhanvienapp/GetKhachHangDaCapQuyen?idNhanvien=' + idnhanvien).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getlistkhachhangchon() {
            return $http.get(urlApi + '/api/khachhang/getall').then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function addphanquyen(idnv, listidkh) {
            return $http.post(urlApi + '/api/nhanvienapp/addphanquyen?idnhanvien=' + idnv + '&lstidkh=' + listidkh).then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: $.i18n('label_capnhatphanquyenthanhcong') }
                } else {
                    return { flag: false, message: $.i18n('label_capnhatphanquyenkhongthanhcong') }
                }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_capnhatphanquyenkhongthanhcong') }
            });
        }

        function removephanquyen(idnv, idkhList) {
            return $http.post(urlApi + '/api/nhanvienapp/removephanquyen?idnhanvien=' + idnv + '&lstidkh=' + idkhList).then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_luukhachhangkhongthanhcongvuilonglienhequantri') }
            });
        }

        function resetimei(idnhanvien) {
            return $http.post(urlApi + '/api/nhanvienapp/resetimei?IDNV=' + idnhanvien).then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: $.i18n('label_thaotacdatlaiimeithanhcong') }
                } else {
                    return { flag: false, message: $.i18n('label_datlaiimeikhongthanhcongvuilongtailaitrangvathulai') }
                }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_khongthanhcongvuilonglienhequantri') }
            });
        }

        function resetpass(idnhanvien, newpass) {
            let obj = {
                id: idnhanvien,
                newpass: newpass,
            }
            return $http.post(urlApi + '/api/userinfo/resetPassword', obj).then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_khongthanhcongvuilonglienhequantri') }
            });
        }

        function saveinsertnhanvienkhongdangnhap(obj) {
            return $http.post(urlApi + '/api/nhanvienapp/themnhanvienkhongdangnhap', obj).then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                return { flag: false, message: $.i18n('label_luukhachhangkhongthanhcongvuilonglienhequantri') }
            });
        }

        function saveinsert(obj) {
            return $http.post(urlApi + '/api/nhanvienapp/themmoinhanvien_v1', obj).then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_luukhachhangkhongthanhcongvuilonglienhequantri') }
            });
        }

        function saveedit(obj) {
            return $http.post(urlApi + '/api/nhanvienapp/suanhanvien', obj).then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_luunhanvienkhongthanhcongvuilonglienhequantri') }
            });
        }

        function del(lisdid) {
            return $http.post(urlApi + '/api/nhanvienapp/xoanhanvien?IDNV=' + lisdid).then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_xoathatbaixinvuilongthulai') }
            });
        }

        function saveinsertnhomnhanvien(obj) {
            return $http.post(urlApi + '/api/nhomnhanvien/insertNhom', obj).then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_luunhomnhanvienkhongthanhcongvuilonglienhequantri') }
            });
        }

        function saveeditnhomnhanvien(obj) {
            return $http.post(urlApi + '/api/nhomnhanvien/editNhom', obj).then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_luunhomnhanvienkhongthanhcongvuilonglienhequantri') }
            });
        }

        function delnhomnhanvien(id) {
            return $http.post(urlApi + '/api/nhomnhanvien/deleteNhom?ID_Nhom=' + id).then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_xoathatbaixinvuilongthulai') }
            });
        }

        function uploadAnhDaiDien(data) {
            return $http.post(urlApi + '/api/uploadfile/savefilenv', data,
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

        function gethinhthucthanhtoan() {
            return $http.get(urlApi + '/api/danhmuc/gethinhthucthanhtoan').then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function luuNV_HinhThucTT(idNV, httts) {
            return $http.post(urlApi + '/api/nhanvienapp/SaveHinhThucThanhToan?IDNV=' + idNV + '&httts=' + httts).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getHinhThucThanhToanByNV(IDNV) {
            return $http.get(urlApi + '/api/nhanvienapp/GetNV_ListHTTT?IDNV=' + IDNV).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

    }

})();