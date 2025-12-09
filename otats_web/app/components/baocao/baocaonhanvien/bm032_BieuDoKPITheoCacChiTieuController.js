angular.module('app').controller('bm032_BieuDoKPITheoCacChiTieuController', function ($scope, $rootScope, $timeout, Notification, baoCaoNhanVienDataService) {
    CreateSiteMap();

    function init() {
        $scope.colors = ['#007bff', '#e74a3b']
        $scope.series = [$.i18n('header_kehoach'), $.i18n('header_thucte')];
        initdata();
    }

    function initdata() {
        $scope.loaikhoangthoigianData = [
            { id: 1, name: $.i18n('button_nam') },
            { id: 2, name: $.i18n('label_quy') }
        ];

        $timeout(function () {
            $("#loaikhoangthoigian").data("kendoDropDownList").value(1);
            loaddatakhoangthoigian(1);
        }, 100);
    }
    function loaddatakhoangthoigian(loai) {
        let data = [];

        if (loai == 1) {
            data = getlistyear();
        } else {
            data = getlistquater();
        }

        $scope.khoangthoigianData = data;

        $timeout(function () {
            $("#khoangthoigian").data("kendoDropDownList").value(1);

            loaddata();
        }, 100);
    }
    function getlistyear() {
        let data = [];
        let d = new Date();
        let y = d.getFullYear();
        for (i = 0; i < 5; i++) {
            let obj = { id: i + 1, name: y, value: '00/' + y };
            data.push(obj);
            y -= 1;

            if (i == 0) 
                $scope.khoangthoigianselect = obj;
        }

        return data;
    }
    function getlistquater() {
        let data = [];

        let d = new Date();
        let y = d.getFullYear();
        let m = d.getMonth();

        let q = 4;
        if (m <= 2)
            q = 1;
        else if (m > 2 && m <= 5)
            q = 2;
        else if (m > 5 && m <= 8)
            q = 3;
        else
            q = 4;

        for (i = 0; i < 5; i++) {
            let obj = { id: i + 1, name: 'Quý ' + q + ' năm ' + y, value: q + '/' + y };
            data.push(obj);

            if (q > 1)
                q -= 1;
            else {
                q = 4;
                y -= 1;
            }

            if (i == 0)
                $scope.khoangthoigianselect = obj;
        }

        return data;
    }

    function loaddata() {
        let date_string = '';
        if ($scope.khoangthoigianselect != undefined)
            date_string = $scope.khoangthoigianselect.value;
        else {
            let d = new Date();
            date_string = '00/' + d.getFullYear();
        }

        baoCaoNhanVienDataService.getBieuDoKPICacChiTieu(date_string).then(function (response) {
            let totaldata = response.data;
            $scope.labels = totaldata[0];

            let data_viengtham = [];
            data_viengtham.push(totaldata[1]);
            data_viengtham.push(totaldata[2]);
            $scope.data_viengtham = data_viengtham;
            $scope.options_viengtham = {
                title: {
                    display: false,
                    position: 'bottom',
                    fontColor: 'rgb(255, 99, 132)',
                },
                tooltips: {
                    callbacks: {
                        label: function (tooltipItem, data) {
                            let label = data.datasets[tooltipItem.datasetIndex].label || '';
                            if (label) {
                                label += ': ';
                            }
                            label += kendo.toString(tooltipItem.yLabel, $rootScope.UserInfo.dinhDangSo);
                            label += $.i18n('label_lan_bieudo');
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
                scales: {
                    yAxes: [
                        {
                            ticks: {
                                beginAtZero: true,
                                min: 0,
                                callback: function (label, index, labels) {
                                    return kendo.toString(label, $rootScope.UserInfo.dinhDangSo);
                                }
                            },
                            scaleLabel: {
                                display: true,
                                fontColor: '#007bff',
                                labelString: $.i18n('label_viengtham')
                            }
                        },
                    ],
                }
            };

            let data_doanhso = [];
            data_doanhso.push(totaldata[4]);
            data_doanhso.push(totaldata[5]);
            $scope.data_doanhso = data_doanhso;
            $scope.options_doanhso = {
                title: {
                    display: false,
                    position: 'bottom',
                    fontColor: 'rgb(255, 99, 132)',
                },
                tooltips: {
                    callbacks: {
                        label: function (tooltipItem, data) {
                            let label = data.datasets[tooltipItem.datasetIndex].label || '';
                            if (label) {
                                label += ': ';
                            }
                            label += kendo.toString(tooltipItem.yLabel, $rootScope.UserInfo.dinhDangSo);
                            label += ' (VNĐ)';
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
                scales: {
                    yAxes: [
                        {
                            ticks: {
                                beginAtZero: true,
                                min: 0,
                                callback: function (label, index, labels) {
                                    return kendo.toString(label, $rootScope.UserInfo.dinhDangSo);
                                }
                            },
                            scaleLabel: {
                                display: true,
                                fontColor: '#007bff',
                                labelString: $.i18n('label_doanhthu')
                            }
                        },
                    ],
                }
            };

            let data_donhang = [];
            data_donhang.push(totaldata[7]);
            data_donhang.push(totaldata[8]);
            $scope.data_donhang = data_donhang;
            $scope.options_donhang = {
                title: {
                    display: false,
                    position: 'bottom',
                    fontColor: 'rgb(255, 99, 132)',
                },
                tooltips: {
                    callbacks: {
                        label: function (tooltipItem, data) {
                            let label = data.datasets[tooltipItem.datasetIndex].label || '';
                            if (label) {
                                label += ': ';
                            }
                            label += kendo.toString(tooltipItem.yLabel, $rootScope.UserInfo.dinhDangSo);
                            label += $.i18n('label_don_bieudo');
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
                scales: {
                    yAxes: [
                        {
                            ticks: {
                                beginAtZero: true,
                                min: 0,
                                callback: function (label, index, labels) {
                                    return kendo.toString(label, $rootScope.UserInfo.dinhDangSo);
                                }
                            },
                            scaleLabel: {
                                display: true,
                                fontColor: '#007bff',
                                labelString: $.i18n('label_donhang')
                            }
                        },
                    ],
                }
            };

            let data_ngaycong = [];
            data_ngaycong.push(totaldata[10]);
            data_ngaycong.push(totaldata[11]);
            $scope.data_ngaycong = data_ngaycong;
            $scope.options_ngaycong = {
                title: {
                    display: false,
                    position: 'bottom',
                    fontColor: 'rgb(255, 99, 132)',
                },
                tooltips: {
                    callbacks: {
                        label: function (tooltipItem, data) {
                            let label = data.datasets[tooltipItem.datasetIndex].label || '';
                            if (label) {
                                label += ': ';
                            }
                            label += kendo.toString(tooltipItem.yLabel, $rootScope.UserInfo.dinhDangSo);
                            label += $.i18n('label_ngay_bieudo');
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
                scales: {
                    yAxes: [
                        {
                            ticks: {
                                beginAtZero: true,
                                min: 0,
                                callback: function (label, index, labels) {
                                    return kendo.toString(label, $rootScope.UserInfo.dinhDangSo);
                                }
                            },
                            scaleLabel: {
                                display: true,
                                fontColor: '#007bff',
                                labelString: $.i18n('label_ngaycong')
                            }
                        },
                    ],
                }
            };
        });
    }

    //event
    $scope.xemBaoCao = function () {
        loaddata();
    }

    $scope.loaikhoangthoigianOnChange = function () {
        $scope.loaikhoangthoigianselect = this.loaikhoangthoigianselect;

        loaddatakhoangthoigian($scope.loaikhoangthoigianselect.id);
    }
    $scope.khoangthoigianOnChange = function () {
        $scope.khoangthoigianselect = this.khoangthoigianselect;
    }

    init();

})