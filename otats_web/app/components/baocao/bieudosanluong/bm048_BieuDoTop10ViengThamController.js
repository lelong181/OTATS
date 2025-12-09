angular.module('app').controller('bm048_BieuDoTop10ViengThamController', function ($scope, $rootScope, $timeout, Notification, baoCaoSanLuongDataService) {
    CreateSiteMap();
    let activelist = 0;
    function init() {
        $scope.colors = ['#007bff', '#007bff', '#007bff', '#007bff', '#007bff', '#007bff', '#007bff', '#007bff', '#007bff', '#007bff',];
        $scope.options = {
            title: {
                display: false,
                position: 'bottom',
                fontColor: 'rgb(255, 99, 132)',
                text: $.i18n('menu_bieudotop10viengtham')
            },
            tooltips: {
                callbacks: {
                    label: function (tooltipItem, data) {
                        let label = data.datasets[tooltipItem.datasetIndex].label || '';

                        if (label) {
                            label += ': ';
                        }
                        //label += Intl.NumberFormat().format(tooltipItem.yLabel);
                        label += kendo.toString(tooltipItem.yLabel, $rootScope.UserInfo.dinhDangSo);
                        label += $.i18n('label_lan_bieudo');
                        return label;
                    }
                }
            },
            legend: {
                display: false,
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
                            //stepSize: 1000000,
                            //suggestedMax: 5000000,
                            min: 0,
                            callback: function (label, index, labels) {
                                //return Intl.NumberFormat().format(label);
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
                xAxes: [{
                    barThickness: 50,  // number (pixels) or 'flex'
                    maxBarThickness: 100 // number (pixels)
                }]
            }
        };
        initdate();
        load_chart();
    }
    function initdate() {
        var dateNow = new Date();
        $scope.obj_TuNgay = new Date(dateNow.setHours(0, 0, 0));
        $scope.obj_DenNgay = new Date(dateNow.setHours(23, 59, 59));
        $scope.maxDate = $scope.obj_DenNgay;
        $scope.minDate = $scope.obj_TuNgay;
    }

    function load_chart() {
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);

        baoCaoSanLuongDataService.getBieuDoTop10NhanVienViengTham(fromdate, todate).then(function (result) {
            let totaldata = result.data;
            if (totaldata.categories.length > 0) {
                $scope.showchart = true;

                $scope.labels_nhanvien = totaldata.categories;
                $scope.data_nhanvien = totaldata.data;
                
            } else {
                $scope.showchart = false;
            }
            if (!result.flag)
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayravuilongloadlaitrang') }, 'warning');
        });
    }
    function load_chart_khachhang() {

        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);

        baoCaoSanLuongDataService.getBieuDoTop10KhachHangViengTham(fromdate, todate).then(function (result) {
            let totaldata = result.data;
            if (totaldata.categories.length > 0) {
                $scope.showchart = true;

                $scope.labels_khachhang = totaldata.categories;
                $scope.data_khachhang = totaldata.data;

            } else {
                $scope.showchart = false;
            }
            if (!result.flag)
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayravuilongloadlaitrang') }, 'warning');
        });
    }

    //event
    $scope.xemBaoCao = function () {
        load_chart();
        if (activelist == 2) {
            load_chart_khachhang();
        }
    }
    $scope.fromDateChanged = function () {
        $scope.minDate = $scope.obj_TuNgay;
    }
    $scope.toDateChanged = function () {
        $scope.maxDate = $scope.obj_DenNgay;
    }
    $scope.clicktabkhachhang = function () {
        activelist = 2;
        $timeout(load_chart_khachhang, 200);
    }
    $scope.clicktabnhanvien = function () {
        activelist = 1;
        $timeout(load_chart, 200);
    }

    init();

})