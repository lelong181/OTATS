angular.module('app').controller('bm042_BieuDoDonHangTheoNhanVienController', function ($scope, $rootScope, Notification, ComboboxDataService, baoCaoSanLuongDataService) {
    CreateSiteMap();
    function init() {
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

        baoCaoSanLuongDataService.getBieuDoDonHangNhomNhanVien(fromdate, todate).then(function (result) {
            commonOpenLoadingText("#btn_xembaocao");
            let totaldata = result.data;
            if (totaldata.categories.length > 0) {
                $scope.showchart = true;

                $scope.labels = totaldata.categories;
                $scope.data = totaldata.data;
                $scope.colors = ['#007bff', '#007bff', '#007bff', '#007bff', '#007bff', '#007bff', '#007bff', '#007bff', '#007bff', '#007bff', '#007bff', '#007bff', '#007bff', '#007bff', '#007bff', '#007bff', '#007bff', '#007bff', '#007bff',];
                $scope.options = {
                    title: {
                        display: false,
                        position: 'bottom',
                        fontColor: 'rgb(255, 99, 132)',
                        text: $.i18n('label_bieudodonhangtheonhanvien')
                    },
                    tooltips: {
                        callbacks: {
                            label: function (tooltipItem, data) {
                                let label = data.datasets[tooltipItem.datasetIndex].label || '';
                                if (label) {
                                    label += ': ';
                                }
                                label += kendo.toString(tooltipItem.xLabel, $rootScope.UserInfo.dinhDangSo);
                                label += $.i18n("label_don_bieudo");
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
                                    min: 0,
                                    callback: function (label, index, labels) {
                                        return kendo.toString(label, $rootScope.UserInfo.dinhDangSo);
                                    }
                                },
                                //scaleLabel: {
                                //    display: true,
                                //    fontColor: '#007bff',
                                //    labelString: 'Nhóm nhân viên'
                                //}
                            },
                        ],
                        xAxes: [{
                            barThickness: 20,  // number (pixels) or 'flex'
                            maxBarThickness: 100, // number (pixels)
                            scaleLabel: {
                                display: true,
                                fontColor: '#007bff',
                                labelString: $.i18n("label_sodonhang(don)"),
                            }
                        }]
                    }
                };
            } else {
                $scope.showchart = false;
            }
            if (!result.flag)
                Notification({ title: $.i18n("label_thongbao"), message: $.i18n("label_coloixayra_loadlaitrang") }, 'warning');

            commonCloseLoadingText("#btn_xembaocao")

        });
    }
    //event
    $scope.xemBaoCao = function () {
        load_chart();
    }
    $scope.fromDateChanged = function () {
        $scope.minDate = $scope.obj_TuNgay;
    }
    $scope.toDateChanged = function () {
        $scope.maxDate = $scope.obj_DenNgay;
    }
    init();

})