angular.module('app').controller('homeController', function ($rootScope, $scope, $state, Notification, homeDataService, ComboboxDataService) {
    CreateSiteMap();
    hideLoadingPage();

    if (!$rootScope.isAdmin == 1) {
        console.log($rootScope.UserInfo.username);
        console.log($rootScope.UserInfo.danhSachNhom);
        if ($rootScope.UserInfo.username.indexOf("pchl") >= 0 || $rootScope.UserInfo.username.indexOf("ta") >= 0) {
            $state.go('danhsachdonhangpkd');
        }
        else if ($rootScope.UserInfo.isHDV) {
            $state.go('danhsachdonhanghdv');
        }
        else {
            $state.go('danhsachdonhang');
        }
    }

    function init() {
        getdatachart();
        getdatabox();
    }

    function fomartnumber(data) {
        return kendo.toString(data, $rootScope.UserInfo.dinhDangSo);
        //return Intl.NumberFormat().format(data)
    }

    function getdatabox() {
        homeDataService.getdatabox().then(function (result) {
            $scope.objbox = result.data[0];
            $scope.objbox.doanhthungay = fomartnumber($scope.objbox.doanhthungay);
            $scope.objbox.congnokhachhang = fomartnumber($scope.objbox.congnokhachhang);
        });
    }
    function getdatachart() {
        homeDataService.getdatachart().then(function (result) {
            $scope.datasetOverride = [{ yAxisID: 'y-axis-doanhthu' }, { yAxisID: 'y-axis-donhang' }];
            $scope.colors = ['#007bff', '#e74a3b'];
            $scope.series = [$.i18n('header_doanhthu'), $.i18n('header_donhang')];
            $scope.labels = result.data.labels;
            $scope.data = result.data.data;
            $scope.options = {
                tooltips: {
                    callbacks: {
                        label: function (tooltipItem, data) {
                            var label = data.datasets[tooltipItem.datasetIndex].label || '';

                            if (label) {
                                label += ': ';
                            }
                            label += fomartnumber(tooltipItem.yLabel);
                            return label;
                        }
                    }
                },
                legend: {
                    display: true,
                    position: 'bottom',
                    labels: {
                        fontColor: 'rgb(255, 99, 132)'
                    }
                },
                elements: {
                    line: {
                        tension: 0,
                        fill: false
                    }
                },
                scales: {
                    yAxes: [
                        {
                            id: 'y-axis-doanhthu',
                            display: true,
                            labels: $.i18n('header_doanhthu'),
                            position: 'left',
                            ticks: {
                                beginAtZero: true,
                                stepSize: result.data.stepsize_doanhthu,
                                suggestedMax: result.data.suggestedMax_doanhthu,
                                min: 0,
                                callback: function (label, index, labels) {
                                    return fomartnumber(label);
                                }
                            },
                            scaleLabel: {
                                display: true,
                                fontColor: '#007bff',
                                labelString: $.i18n('label_doanhthu') + " (VNĐ)"
                            }
                        },
                        {
                            id: 'y-axis-donhang',
                            display: true,
                            labels: $.i18n('header_sodonhang'),
                            position: 'right',
                            ticks: {
                                beginAtZero: true,
                                stepSize: result.data.stepsize_donhang,
                                suggestedMax: result.data.suggestedMax_donhang,
                                min: 0
                            },
                            scaleLabel: {
                                display: true,
                                fontColor: '#e74a3b',
                                labelString: $.i18n('header_sodonhang')
                            }
                        }
                    ]
                }
            };

            if (!result.flag)
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloisayrakhiketnoihethongvuilongdangxuatvadangnhaplai') }, 'warning');
        });
    }

    //event
    $scope.opennhanvientructuyen = function () {
        $state.go('nhanvien', { tructuyen: 1 });
    }
    $scope.openviengthamngay = function () {
        $state.go('lichsuvaoradiem');
    }
    $scope.opendoanhthungay = function () {
        $state.go('baocaodoanhthu');
    }
    $scope.opencongnokhackhang = function () {
        $state.go('baocaotheodoicongno');
    }

    init();

})