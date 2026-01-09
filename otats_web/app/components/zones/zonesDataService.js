(function () {
    'use strict';

    angular
        .module('app')
        .factory('zonesDataService', zonesDataService);

    zonesDataService.$inject = ['$http', '$cookies', '$rootScope', '$timeout'];
    function zonesDataService($http, $cookies, $rootScope, $timeout) {
        var service = {};

        service.getByHotel = getByHotel;
        service.insert = insert;
        service.update = update;
        service.deleteZone = deleteZone;

        return service;

        function getByHotel(hotelId) {
            return $http.get(urlApi + '/api/zones/getbyhotel?hotelId=' + hotelId).then(function (response) {
                if (response.status === 200) {
                    return { flag: true, data: response.data };
                } else {
                    return { flag: false, message: response.statusText };
                }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') };
            });
        }

        function insert(zoneModel) {
            return $http.post(urlApi + '/api/zones/insert', zoneModel).then(function (response) {
                if (response.status === 200) {
                    return { flag: true, data: response.data, message: $.i18n('label_themmoithanhcong') };
                } else {
                    return { flag: false, message: $.i18n('label_themmoithatbaivuilongkiemtralaitruongdulieu') };
                }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') };
            });
        }

        function update(zoneModel) {
            return $http.post(urlApi + '/api/zones/update', zoneModel).then(function (response) {
                if (response.status === 200 && response.data === true) {
                    return { flag: true, message: $.i18n('label_tacvuthuchienthanhcong') };
                } else {
                    return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') };
                }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') };
            });
        }

        function deleteZone(zoneId) {
            return $http.post(urlApi + '/api/zones/delete?id=' + zoneId).then(function (response) {
                if (response.status === 200 && response.data === true) {
                    return { flag: true, message: $.i18n('label_tacvuthuchienthanhcong') };
                } else {
                    return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') };
                }
            }, function (response) {
                return { flag: false, message: response.data && response.data.Message ? response.data.Message : $.i18n('label_coloixayravuilongloadlaitrang') };
            });
        }
    }
})();
