(function () {
    'use strict';

    angular
        .module('app')
        .factory('lspayDataService', lspayDataService);

    lspayDataService.$inject = ['$http', '$cookies', '$rootScope', '$timeout'];
    function lspayDataService($http, $cookies, $rootScope, $timeout) {
        var service = {};
      
        service.getAllBienDong = getAllBienDong;
        service.getLichSuNap = getLichSuNap;
        service.getBaoCaoSuDungVi = getBaoCaoSuDungVi;
        service.getBaoCaoDoiSoatOnepay = getBaoCaoDoiSoatOnepay;
        service.getTongSoDu = getTongSoDu;
        service.getSoDuDauKy = getSoDuDauKy;
        service.checkQueryOnepay = checkQueryOnepay;
        service.checkQueryOnepayB2C = checkQueryOnepayB2C;
        service.getQRDynamic = getQRDynamic;
        service.xuLyNapVi = xuLyNapVi;
        service.xacNhanNapVi = xacNhanNapVi;
        return service;
        function getBaoCaoSuDungVi(id_nhom, from, to) {
            return $http.get(urlApi + '/api/baocao/getBaoCaoSuDungVi?id_nhom=' + id_nhom + '&from=' + from + '&to=' + to).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getBaoCaoDoiSoatOnepay( from, to) {
            return $http.get(urlApi + '/api/baocao/getBaoCaoDoiSoatOnepay?from=' + from + '&to=' + to).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function checkQueryOnepay(data) {
            return $http.get(urlApi + '/api/donhang/checkquyerydronepay?vpc_MerchTxnRef=' + data).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function checkQueryOnepayB2C(data) {
            return $http.get(urlApi + '/api/donhang/checkquyerydronepayb2c?vpc_MerchTxnRef=' + data).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getAllBienDong(id_nhom, from, to) {
            return $http.post(urlApi + '/api/donhang/GetAllBienDong?id_nhom=' + id_nhom + '&from=' + from + '&to=' + to).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getLichSuNap(id_nhom) {
            return $http.post(urlApi + '/api/donhang/GetLichSuNap?ID_Nhom=' + id_nhom).then(function (response) {
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
        function getSoDuDauKy(id_nhom,to) {
            return $http.post(urlApi + '/api/donhang/GetSoDuDauKy?id_nhom=' + id_nhom + '&to=' + to).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function getQRDynamic(id_nhom, sotien) {
            return $http.get(urlApi + '/api/ota/PayCollect_CreatePaymentLsPay?ID_NhomTaiKhoan=' + id_nhom + '&SoTien=' + sotien).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function xuLyNapVi(data) {
            return $http.post(urlApi + '/api/donhang/xulynapvi', data).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }

        function xacNhanNapVi(id) {
            return $http.get(urlApi + '/api/donhang/xacnhannapvithanhcong?ID=' + id).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
    }



})();