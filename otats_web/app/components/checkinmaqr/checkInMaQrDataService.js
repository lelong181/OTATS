(function () {
    'use strict';

    angular
        .module('app')
        .factory('checkInMaQrDataService', checkInMaQrDataService);

    checkInMaQrDataService.$inject = ['$http', '$cookies', '$rootScope', '$timeout'];
    function checkInMaQrDataService($http, $cookies, $rootScope, $timeout) {
        var service = {};
        service.checkTicket = checkTicket;
        service.usingTicket = usingTicket;

        return service;

        function checkTicket(obj) {
            var apiUrl = 'https://localapits.lscloud.vn/mobile/check-ticket';
            return $http.post(apiUrl, obj).then(function (response) {
                if (response.data.status === 'SUCCESS')
                    return { flag: true, data: response.data }
                else
                    return { flag: false, message: response.data.errors }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }

        function usingTicket(code, device) {
            return $http.post('https://localapits.lscloud.vn/mobile/using-ticket?code=' + code + "&device=" + device).then(function (response) {
                return { flag: true, data: response }
            }, function (response) {
                return { flag: false, data: [] }
            });
        }
    }

})();
