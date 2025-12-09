(function () {
    'use strict';
    angular
        .module('app')
        .factory('mapsDataService', mapsDataService);

    mapsDataService.$inject = ['$http', '$rootScope', '$timeout'];
    function mapsDataService($http, $rootScope, $timeout) {
        var service = {};

        service.getlistdaily = getlistdaily;
        service.getchitietdaily = getchitietdaily;
        service.getvitridaily = getvitridaily;
        service.getthongketructuyen = getthongketructuyen;
        service.getdanhsachnhanvientheotrangthai = getdanhsachnhanvientheotrangthai;
        service.getlistnhanvien = getlistnhanvien;
        service.getvitrihientainhanvien = getvitrihientainhanvien;
        service.getchitietnhanvien = getchitietnhanvien;
        service.getdatalotrinhnhanvien = getdatalotrinhnhanvien;
        service.getdatalotrinhnhanvienphienlamviec = getdatalotrinhnhanvienphienlamviec;
        service.chitietdungdo = chitietdungdo;

        return service;

        function getdatalotrinhnhanvienphienlamviec(idnhanvien, khongnoidiem, loaiLoTrinh, tungayF, denngayF) {
            return $http.get(urlApi + '/api/bando/getdatalotrinhnhanvienphienlamviec?idnhanvien=' + idnhanvien + '&khongnoidiem=' + khongnoidiem + '&loaiLoTrinh=' + loaiLoTrinh + '&tungayF=' + tungayF + '&denngayF=' + denngayF).then(function (response) {
                if (response.data.status)
                    return { flag: true, data: response.data }
                else
                    return { flag: false, data: response.data, message: response.data.msg }
            }, function (response) {
                    return { flag: false, data: {}, message: $.i18n('label_khongcodulieulotrinh') }
            });
        }

        function getdatalotrinhnhanvien(idnhanvien, khongnoidiem, loaiLoTrinh, tungayF, denngayF) {
            return $http.get(urlApi + '/api/bando/getdatalotrinhnhanvien?idnhanvien=' + idnhanvien + '&khongnoidiem=' + khongnoidiem + '&loaiLoTrinh=' + loaiLoTrinh + '&tungayF=' + tungayF + '&denngayF=' + denngayF).then(function (response) {
                if (response.data.status)
                    return { flag: true, data: response.data }
                else
                    return { flag: false, data: response.data, message: response.data.msg }
            }, function (response) {
                    return { flag: false, data: {}, message: $.i18n('label_khongcodulieulotrinh') }
            });
        }
        function chitietdungdo(idnhanvien, kinhdostr, vidostr, from, to) {
            return $http.get(urlApi + '/api/bando/chitietdungdo?idnhanvien=' + idnhanvien + '&kinhdostr=' + kinhdostr + '&vidostr=' + vidostr + '&from=' + from + '&to=' + to).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: '' }
            });
        }

        function getlistdaily(kinhdo, vido, idtinh, idquan, idloaikhachhang) {
            return $http.get(urlApi + '/api/bando/getlistdaily?kinhdo=' + kinhdo + '&vido=' + vido
                + '&idtinh=' + idtinh + '&idquan=' + idquan + '&idloaikhachhang=' + idloaikhachhang).then(function (response) {
                    return { flag: true, data: response.data }
                }, function (response) {
                    return { flag: false, data: [] }
                });
        }

        function getchitietdaily(idkhachhang) {
            return $http.get(urlApi + '/api/bando/getchitietdaily?idkhachhang=' + idkhachhang).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: '' }
            });
        }

        function getvitridaily(idkhachhang) {
            return $http.get(urlApi + '/api/bando/getvitridaily?idkhachhang=' + idkhachhang).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: {} }
            });
        }

        function getthongketructuyen(idnhom) {
            return $http.get(urlApi + '/api/bando/getthongketructuyen?idnhom=' + idnhom).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: {} }
            });
        }

        function getdanhsachnhanvientheotrangthai(trangthai) {
            return $http.get(urlApi + '/api/bando/getdanhsachnhanvientheotrangthai?trangthai=' + trangthai).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: {} }
            });
        }

        function getlistnhanvien(idnhom, loctrangthai) {
            return $http.get(urlApi + '/api/bando/getlistnhanvien?idnhom=' + idnhom + '&loctrangthai=' + loctrangthai).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getvitrihientainhanvien(idnhanvien) {
            return $http.get(urlApi + '/api/bando/getvitrihientainhanvien?idnhanvien=' + idnhanvien).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: {} }
            });
        }

        function getchitietnhanvien(idnhanvien) {
            return $http.get(urlApi + '/api/bando/getchitietnhanvien?idnhanvien=' + idnhanvien).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: {} }
            });
        }

    }

})();