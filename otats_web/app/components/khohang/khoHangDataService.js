(function () {
    'use strict';

    angular
        .module('app')
        .factory('khoHangDataService', khoHangDataService);

    khoHangDataService.$inject = ['$http', '$cookies', '$rootScope', '$timeout'];
    function khoHangDataService($http, $cookies, $rootScope, $timeout) {
        var service = {};

        service.getlistphieunhap = getlistphieunhap;
        service.getlistphieunhapdetail = getlistphieunhapdetail;
        service.themmoiphieunhap = themmoiphieunhap;
        service.excellistphieunhap = excellistphieunhap;
        service.taifilemauphieunhap = taifilemauphieunhap;
        service.importphieunhap = importphieunhap;

        service.getlistphieudieuchuyen = getlistphieudieuchuyen;
        service.getlistphieudieuchuyendetail = getlistphieudieuchuyendetail;
        service.getlistmathangtheokho = getlistmathangtheokho;
        service.themmoiphieudieuchuyen = themmoiphieudieuchuyen;
        service.excellistphieudieuchuyen = excellistphieudieuchuyen;

        service.baocaodieuchinh = baocaodieuchinh;
        service.getlistphieudieuchinh = getlistphieudieuchinh;
        service.excelbaocaodieuchinh = excelbaocaodieuchinh;

        service.getphieutondau = getphieutondau;
        service.themphieutondau = themphieutondau;
        service.taifilemauphieutondau = taifilemauphieutondau;
        service.importphieutondau = importphieutondau;

        service.getlistphieunhapcombo = getlistphieunhapcombo;
        service.getchitietphieunhap = getchitietphieunhap;
        service.getchitietdieuchinh = getchitietdieuchinh;
        service.themsuaphieudieuchinh = themsuaphieudieuchinh;

        service.getlistkhohang = getlistkhohang;
        service.getlistnhanvienbyidkho = getlistnhanvienbyidkho;
        service.getlistmathangbyidkho = getlistmathangbyidkho;
        service.deletekho = deletekho;
        service.themsuakho = themsuakho;
        service.phanquyennhanvienkho = phanquyennhanvienkho;

        return service;

        function getlistphieunhap(from, to) {
            return $http.get(urlApi + '/api/phieunhap/getlist?from=' + from + '&to=' + to).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getlistphieunhapdetail(idphieunhap) {
            return $http.get(urlApi + '/api/phieunhap/getlistdetail?idphieunhap=' + idphieunhap).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function themmoiphieunhap(obj) {
            return $http.post(urlApi + '/api/phieunhap/themphieunhap', obj).then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_luuphieunhapkhongthanhcongvuilonglienhequantri') }
            });
        }
        function excellistphieunhap(from, to) {
            return $http.get(urlApi + '/api/phieunhap/excellistphieunhap?from=' + from + '&to=' + to, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }
        function taifilemauphieunhap() {
            return $http.get(urlApi + '/api/phieunhap/gettemplatephieunhap', { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }
        function importphieunhap(fileUpload) {
            return $http.post(urlApi + '/api/phieunhap/importphieunhap', { filename: fileUpload }, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }

        function getlistphieudieuchuyen(from, to) {
            return $http.get(urlApi + '/api/phieudieuchuyen/getlist?from=' + from + '&to=' + to).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getlistphieudieuchuyendetail(idphieudieuchuyen) {
            return $http.get(urlApi + '/api/phieudieuchuyen/getlistdetail?idphieudieuchuyen=' + idphieudieuchuyen).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getlistmathangtheokho(idkho, idmathang) {
            return $http.get(urlApi + '/api/phieudieuchuyen/getlistmathangtheokho?idkho=' + idkho + '&idmathang=' + idmathang).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function themmoiphieudieuchuyen(obj) {
            return $http.post(urlApi + '/api/phieudieuchuyen/themphieudieuchuyen', obj).then(function (response) {
                if (response.data.success) {
                    return { flag: true, message: response.data.msg }
                } else {
                    return { flag: false, message: response.data.msg }
                }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_luuphieudieuchuyenkhongthanhcongvuilonglienhequantri') }
            });
        }
        function excellistphieudieuchuyen(from, to) {
            return $http.get(urlApi + '/api/phieudieuchuyen/excellistphieudieuchuyen?from=' + from + '&to=' + to, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }

        function baocaodieuchinh(idKho, from, to) {
            return $http.get(urlApi + '/api/dieuchinh/baocaodieuchinh?ID_Kho=' + idKho + '&from=' + from + '&to=' + to).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function excelbaocaodieuchinh(idKho, from, to) {
            return $http.get(urlApi + '/api/dieuchinh/excelbaocaodieuchinh?ID_Kho=' + idKho + '&from=' + from + '&to=' + to, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }
        function getlistphieudieuchinh() {
            return $http.get(urlApi + '/api/dieuchinh/getlistphieudieuchinh').then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getphieutondau(idkho) {
            return $http.get(urlApi + '/api/tonkhodau/getphieutondau?idKho=' + idkho).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function themphieutondau(obj) {
            return $http.post(urlApi + '/api/tonkhodau/add', obj).then(function (response) {
                return { flag: true, data: response.data, message: $.i18n('label_luuphieutondauthanhcong') }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_luuphieutonkhongthanhcongvuilonglienhequantri') }
            });
        }
        function taifilemauphieutondau() {
            return $http.get(urlApi + '/api/tonkhodau/gettemplatephieutondau', { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }
        function importphieutondau(fileUpload) {
            return $http.post(urlApi + '/api/tonkhodau/importphieutondau', { filename: fileUpload }, { responseType: "arraybuffer" }).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }

        function getlistphieunhapcombo() {
            return $http.get(urlApi + '/api/dieuchinh/getlistphieunhap').then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getchitietphieunhap(idphieunhap) {
            return $http.get(urlApi + '/api/dieuchinh/getchitietphieunhap?ID_PhieuNhap=' + idphieunhap).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getchitietdieuchinh(idphieudieuchinh) {
            return $http.get(urlApi + '/api/dieuchinh/getchitietdieuchinh?ID_PhieuDieuChinhNhapKho=' + idphieudieuchinh).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function themsuaphieudieuchinh(obj) {
            return $http.post(urlApi + '/api/dieuchinh/add', obj).then(function (response) {
                return { flag: true, message: $.i18n('label_luuphieudieuchinhthanhcong') }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_luuphieudieuchinhkhongthanhcongvuilonglienhequantri') }
            });
        }

        function getlistkhohang() {
            return $http.get(urlApi + '/api/khohang/getall').then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getlistnhanvienbyidkho(idkho) {
            return $http.get(urlApi + '/api/khohang/getlistnhanvienbyidkho?idkho=' + idkho).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function getlistmathangbyidkho(idkho) {
            return $http.get(urlApi + '/api/khohang/getlistmathangbyidkho?idkho=' + idkho).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
        function deletekho(idkho) {
            return $http.post(urlApi + '/api/khohang/deletekho?idkho=' + idkho).then(function (response) {
                if (response.data.success)
                    return { flag: true, message: response.data.msg }
                else
                    return { flag: false, message: response.data.msg }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_xoakhohangkhongthanhcongvuilonglienhequantri') }
            });

        }
        function themsuakho(data) {
            return $http.post(urlApi + '/api/khohang/themsuakho', data).then(function (response) {
                if (response.data.success)
                    return { flag: true, message: response.data.msg }
                else
                    return { flag: false, message: response.data.msg }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_luukhohangkhongthanhcongvuilonglienhequantri') }
            });

        }
        function phanquyennhanvienkho(data, idkho) {
            return $http.post(urlApi + '/api/khohang/phanquyennhanvienkho?idkho=' + idkho, data).then(function (response) {
                if (response.data.success)
                    return { flag: true, message: response.data.msg }
                else
                    return { flag: false, message: response.data.msg }
            }, function (response) {
                    return { flag: false, message: $.i18n('label_phanquyennhanvienkhohangkhongthanhcongvuilonglienhequantri') }
            });

        }
    }

})();