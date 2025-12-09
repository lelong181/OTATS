angular.module('app').controller('bm045_BieuDoTop10NhanVienDiChuyenController', function ($scope, $rootScope, Notification, ComboboxDataService, baoCaoSanLuongDataService) {
    CreateSiteMap();

    function init() {
        initdate();
        loaddatanhanvien();
    }

    function initdate() {
        var dateNow = new Date();
        $scope.obj_TuNgay = new Date(dateNow.setHours(0, 0, 0));
        $scope.obj_DenNgay = new Date(dateNow.setHours(23, 59, 59));
        $scope.maxDate = $scope.obj_DenNgay;
        $scope.minDate = $scope.obj_TuNgay;
    }

    function loaddatanhanvien() {
        commonOpenLoadingText("#btn_xembaocao_nhanVien");
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);

        baoCaoSanLuongDataService.getBieuDoTop10NhanVienDiChuyen(fromdate, todate).then(function (result) {
            let totaldata = result.data;
            if (totaldata.categories.length > 0) {
                $scope.showchart = true;

                $scope.labels = totaldata.categories;
                $scope.data = totaldata.data;
                $scope.colors = ['#007bff', '#007bff', '#007bff', '#007bff', '#007bff', '#007bff', '#007bff', '#007bff', '#007bff', '#007bff',];
                $scope.options = {
                    title: {
                        display: false,
                        position: 'bottom',
                        fontColor: 'rgb(255, 99, 132)',
                        text: $.i18n('menu_bieudotop10nhanviendichuyen')
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
                                label += ' (Km)';
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
                                    labelString: $.i18n('label_kmdichuyen')
                                }
                            },
                        ],
                        xAxes: [{
                            barThickness: 50,  // number (pixels) or 'flex'
                            maxBarThickness: 100 // number (pixels)
                        }]
                    }
                };
            } else {
                $scope.showchart = false;
            }

            if (!result.flag)
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayravuilongloadlaitrang') }, 'warning');
            commonCloseLoadingText("#btn_xembaocao_nhanVien")
        });
    }
    //event
    $scope.xemBaoCao_NV = function () {
        loaddatanhanvien();
    }
    $scope.fromDateChanged = function () {
        $scope.minDate = $scope.obj_TuNgay;
    }
    $scope.toDateChanged = function () {
        $scope.maxDate = $scope.obj_DenNgay;
    }
    init();

})