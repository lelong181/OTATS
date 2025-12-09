(function () {
    'use strict';

    angular
        .module('app')
        .factory('checkInMaQrDataService', checkInMaQrDataService);

    checkInMaQrDataService.$inject = ['$http', '$cookies', '$rootScope', '$timeout'];
    function checkInMaQrDataService($http, $cookies, $rootScope, $timeout) {
        var service = {};
        service.checkTicket = checkTicket;

        return service;

        function checkTicket(obj) {
            var apiUrl = 'http://42.96.58.57:8888/mobile/check-ticket';
            return $http.post(apiUrl, obj).then(function (response) {
                if (response.data.status === 'SUCCESS')
                    return { flag: true, data: response.data }
                else
                    return { flag: false, message: response.data.errors }
            }, function (response) {
                return { flag: false, message: $.i18n('label_coloixayravuilongloadlaitrang') }
            });
        }

    }

})();
