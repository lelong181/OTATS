angular.module('app').controller('bm046_BieuDoTop10SanPhamBanChayController', function ($scope, $rootScope, $state, $timeout, Notification, ComboboxDataService, baoCaoSanLuongDataService) {
    CreateSiteMap();
    let activelist = 0;
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
    function listColumnsgrid() {
        var dataList = [];

        dataList.push({
            title: "#", template: "#= ++RecordNumber #", width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { class: "text-center" },
        });
        dataList.push({
            field: "maHang", title: $.i18n('label_mahang'), footerTemplate: $.i18n('label_total'), attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "70px"
        });
        dataList.push({
            field: "tenHang", title: $.i18n('label_tenhanghoa'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px"
        });
        dataList.push({
            field: "soDonHang", title: $.i18n('label_sodonhang'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('soDonHang.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: center" }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "70px"
        });
        dataList.push({
            field: "soKhachHangMua", title: $.i18n('label_sokhachmuahang'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('soKhachHangMua.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: center" }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "70px"
        });
        dataList.push({
            field: "soLuong", title: $.i18n('label_soluong'), template: function (dataItem) {
                return "<a href='' class='color-primary' ng-click='openFormDsDonHang(" + kendo.htmlEncode(dataItem.iD_HangHoa) + ")'>" + kendo.toString(dataItem.soLuong, $rootScope.UserInfo.dinhDangSo) + "</a>";
            },
            footerTemplate: formatNumberInFooterGrid('soLuong.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: center" }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        });
        dataList.push({
            field: "doanhThu", title: $.i18n('label_doanhthu'), template: function (dataItem) {
                return "<a href='' class='color-primary' ng-click='openFormDsDonHang(" + kendo.htmlEncode(dataItem.iD_HangHoa) + ")'>" + kendo.toString(dataItem.doanhThu, $rootScope.UserInfo.dinhDangSo) + "</a>";
            },
            footerTemplate: formatNumberInFooterGrid('doanhThu.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: right" }, attributes: { style: "text-align: right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "120px"
        });
        return dataList;
    }
    function load_chart() {
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);

        baoCaoSanLuongDataService.getBieuDoTop10SanPhamBanChay(fromdate, todate).then(function (result) {
            commonOpenLoadingText("#btn_xembaocao");
            $scope.datasetOverride = [{ yAxisID: 'y-axis-doanhthu' }, { yAxisID: 'y-axis-soluong' }];
            let totaldata = result.data;
            if (totaldata[0].length > 0) {
                $scope.showchart = true;
                $scope.labels = totaldata[0];
                let data_sanpham = [];
                data_sanpham.push(totaldata[1]);
                data_sanpham.push(totaldata[2]);
                $scope.data = data_sanpham;
                $scope.series = [$.i18n('header_doanhthu'), $.i18n('header_soluong')];
                $scope.colors = ['#007bff', '#e74a3b'];
                $scope.options = {
                    title: {
                        display: false,
                        position: 'bottom',
                        fontColor: 'rgb(255, 99, 132)',
                        text: $.i18n('menu_bieudotop10sanphambanchay')
                    },
                    tooltips: {
                        callbacks: {
                            label: function (tooltipItem, data) {
                                let label = data.datasets[tooltipItem.datasetIndex].label || '';

                                if (label) {
                                    label += ': ';
                                }
                                label += kendo.toString(tooltipItem.yLabel, $rootScope.UserInfo.dinhDangSo);
                                
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
                                id: 'y-axis-doanhthu',
                                display: true,
                                labels: $.i18n('label_doanhthu'),
                                position: 'left',
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
                                    labelString: $.i18n('label_doanhthu') + "(VNĐ)"
                                }
                            }, {
                                id: 'y-axis-soluong',
                                display: true,
                                labels: $.i18n('label_soluong'),
                                position: 'right',
                                ticks: {
                                    beginAtZero: true,
                                    min: 0
                                },
                                scaleLabel: {
                                    display: true,
                                    fontColor: '#e74a3b',
                                    labelString: $.i18n('label_soluong')
                                }
                            }
                        ],
                        
                    }
                };
            } else {
                $scope.showchart = false;
            }
            if (!result.flag)
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayravuilongloadlaitrang') }, 'warning');

            commonCloseLoadingText("#btn_xembaocao")

        });
    }
    function loadgrid() {
        kendo.ui.progress($("#grid"), true);
        commonOpenLoadingText("#btn_xembaocao");
        $scope.gridOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".content-filter").height() + $(".nav-pills").height());
                return heightGrid - 60;
            },
            excelExport: function (e) {
                excelExport(e);
            },
            dataBinding: function () {
                RecordNumber = (this.dataSource.page() - 1) * this.dataSource.pageSize();
            },
            resizable: true,
            editable: false,
            filterable: {
                mode: "row"
            },
            pageable: $scope.lang == 'vi-vn' ? pageableShort_vi : pageableShort_en,
            columns: listColumnsgrid()
        };

        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        baoCaoSanLuongDataService.getBaoCaoTop10SanPhamBanChay(fromdate, todate).then(function (response) {
            $scope.gridData = {
                data: response.data,
                schema: {
                    model: {
                        fields: {
                            doanhThu: {
                                type: "number"
                            },
                            soLuong: {
                                type: "number"
                            },
                            soKhachHangMua: {
                                type: "number"
                            },
                            soDonHang: {
                                type: "number"
                            },
                        }
                    }
                },
                pageSize: 20,
                aggregate: [
              { field: "soDonHang", aggregate: "sum" },
              { field: "soKhachHangMua", aggregate: "sum" },
              { field: "soLuong", aggregate: "sum" },
              { field: "doanhThu", aggregate: "sum" },
                ]
            };
            kendo.ui.progress($("#grid"), false);
            commonCloseLoadingText("#btn_xembaocao")

        });
    }

    //event
    $scope.xemBaoCao = function () {
        load_chart();
        if (activelist == 2) {
            loadgrid();
        }
    }
    $scope.xuatExcel = function () {
        commonOpenLoadingText("#btn_xuatExcel");
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        baoCaoSanLuongDataService.getExcelTop10SanPhamBanChay(fromdate, todate).then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayravuilongloadlaitrang') }, 'warning');

            commonCloseLoadingText("#btn_xuatExcel")
        });
    }
    $scope.fromDateChanged = function () {
        $scope.minDate = $scope.obj_TuNgay;
    }
    $scope.toDateChanged = function () {
        $scope.maxDate = $scope.obj_DenNgay;
    }
    $scope.clicktabdanhsach = function () {
        activelist = 2;
        $timeout(loadgrid, 200);
    }
    $scope.clicktabbieudo = function () {
        activelist = 1;
        $timeout(load_chart, 200);
    }
    $scope.openFormDsDonHang = function (iD_HangHoa) {
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        $state.go('danhsachdonhang', { idmathang: iD_HangHoa, from: fromdate, to: todate });
    }
    init();

})


