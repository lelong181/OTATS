(function () {
    'use strict';

    angular
        .module('app')
        .factory('albumDataService', albumDataService);

    albumDataService.$inject = ['$http', '$cookies', '$rootScope', '$timeout'];
    function albumDataService($http, $cookies, $rootScope, $timeout) {
        var service = {};

        service.getalbum = getalbum;
        service.xoayanh = xoayanh;
        
        return service;

        function getalbum(id_album, id_image, id_checkin, id_baoduong) {
            return $http.get(urlApi + '/api/album/getalbum?id_album=' + id_album + '&id_image=' + id_image
                + '&id_checkin=' + id_checkin + '&id_baoduong=' + id_baoduong).then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: {} }
            });
        }

        function xoayanh(_url) {
            return $.ajax({
                type: "GET",
                url: _url,
                dataType: 'jsonp',
                crossDomain: true
            }).done(function (data) {
                return { flag: true, data: data }
            }).fail(function (jqXHR, textStatus) {
                return { flag: false, data: [] }
            });
        }
    }

})();