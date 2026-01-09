(function () {
    'use strict';

    angular
        .module('app')
        .factory('hotelsDataService', hotelsDataService);

    hotelsDataService.$inject = ['$http', '$cookies', '$rootScope', '$timeout'];
    function hotelsDataService($http, $cookies, $rootScope, $timeout) {
        var service = {};

        service.getAll = getAll;
        service.getById = getById;
        service.insert = insert;
        service.update = update;
        service.deleteHotel = deleteHotel;

        return service;

        function getAll() {
            return $http.get(urlApi + '/api/hotels/getall').then(function (response) {
                if (response.status === 200) {
                    return { flag: true, data: response.data };
                } else {
                    return { flag: false, message: response.statusText };
                }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') };
            });
        }

        function getById(id) {
            return $http.get(urlApi + '/api/hotels/getbyid?id=' + id).then(function (response) {
                if (response.status === 200) {
                    return { flag: true, data: response.data };
                } else {
                    return { flag: false, message: response.statusText };
                }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') };
            });
        }

        function insert(model) {
            return $http.post(urlApi + '/api/hotels/insert', model).then(function (response) {
                // API returns new ID (int) on success, checking status 200
                if (response.status === 200) {
                    return { flag: true, data: response.data, message: $.i18n('label_themmoithanhcong') };
                } else {
                    return { flag: false, message: $.i18n('label_themmoithatbaivuilongkiemtralaitruongdulieu') };
                }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') };
            });
        }

        function update(model) {
            return $http.post(urlApi + '/api/hotels/update', model).then(function (response) {
                if (response.status === 200 && response.data === true) {
                    return { flag: true, message: $.i18n('label_tacvuthuchienthanhcong') };
                } else {
                    return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') };
                }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') };
            });
        }

        function deleteHotel(id) {
            return $http.post(urlApi + '/api/hotels/delete?id=' + id).then(function (response) {
                // Assuming delete also returns bool true on success primarily
                if (response.status === 200) {
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
