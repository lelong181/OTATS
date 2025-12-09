(function () {
    'use strict';

    angular
        .module('app')
        .factory('homeDataService', homeDataService);

    homeDataService.$inject = ['$http', '$cookies', '$rootScope', '$timeout'];
    function homeDataService($http, $cookies, $rootScope, $timeout) {
        var service = {};
        service.getdatabox = getdatabox;
        service.getdatachart = getdatachart;
        
        return service;
        
        function getdatabox() {
            return $http.get(urlApi + '/api/dashboard/getdatabox').then(function (response) {
                return { flag: true, data: response.data }
            }, function (response) {
                return { flag: false, data: [{ tongnhanvien: 0, nhanvientructuyen: 0, donhangtrongngay: 0, doanhthungay: 0, viengthamngay: 0, congnokhachhang: 0, donhanghoantat: 0, donhangchuahoantat: 0, donhanghuy: 0 }] }
            });
        }

        function getdatachart() {
            return $http.get(urlApi + '/api/dashboard/getdatachart').then(function (response) {
                let dataset = response.data;

                let datalabel = dataset.table;
                let datapoint = dataset.table1;
                let datamaxdoanhthu = dataset.table2;
                let datamaxdonhang = dataset.table3;

                let _labels = [];
                let _data = [];
                let _data_doanhthu = [];
                let _data_donhang = [];
                let _stepsize_donhang = 10;
                let _stepsize_doanhthu = 1000000;
                let _suggestedMax_donhang = 50;
                let _suggestedMax_doanhthu = 5000000;

                let _maxdoanhthu = datamaxdoanhthu[0].maxDoanhThu;
                let _maxdonhang = datamaxdonhang[0].maxDonHang;

                if (_maxdoanhthu > 1000000) {
                    let numdoanhthu = parseInt(_maxdoanhthu);
                    _maxdoanhthu = numdoanhthu + (1000000 - (numdoanhthu % 1000000));

                    _stepsize_doanhthu = _maxdoanhthu / 5;
                    _suggestedMax_doanhthu = _maxdoanhthu;
                }

                if (_maxdonhang > 50) {
                    let numdonhang = parseInt(_maxdonhang);
                    _maxdonhang = numdonhang + (50 - (numdonhang % 50));

                    _stepsize_donhang = _maxdonhang / 5;
                    _suggestedMax_donhang = _maxdonhang;
                }

                datalabel.forEach(function (element) {
                    _labels.push(element.dd_Name);
                });

                datapoint.forEach(function (element) {
                    _data_doanhthu.push(element.doanhThu);
                    _data_donhang.push(element.donHang);
                });

                _data.push(_data_doanhthu);
                _data.push(_data_donhang);

                let chart = {
                    labels: _labels,
                    data: _data,
                    suggestedMax_donhang: _suggestedMax_donhang,
                    suggestedMax_doanhthu: _suggestedMax_doanhthu,
                    stepsize_donhang: _stepsize_donhang,
                    stepsize_doanhthu: _stepsize_doanhthu
                }

                return { flag: true, data: chart }
            }, function (response) {
                let chart = {
                    labels: ['', '', '', '', '', '', ''],
                    data: [
                      [0, 0, 0, 0, 0, 0, 0],
                      [0, 0, 0, 0, 0, 0, 0]
                    ],
                    suggestedMax_donhang: 50,
                    suggestedMax_doanhthu: 5000000,
                    stepsize_donhang: 10,
                    stepsize_doanhthu: 1000000
                }

                return { flag: false, data: chart }
            });
        }

    }

})();