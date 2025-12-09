angular.module('app').controller('bm039_BieuDoTop10DoanhThuTheoNhanVienController', function ($state, $scope, $rootScope, $timeout, Notification, ComboboxDataService, bieuDoDoanhThuDataService) {
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
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { class: "text-center" },
        });
        dataList.push({
            field: "tenDangNhap", title: $.i18n('header_tendangnhap'), footerTemplate: $.i18n('label_total'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "70px"
        });
        dataList.push({
            field: "tenNhanVien", title: $.i18n('header_tennhanvien'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px"
        });
        dataList.push({
            field: "dienThoai", title: $.i18n('header_sodienthoai'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px"
        });
        dataList.push({
            field: "diaChi", title: $.i18n('header_diachi'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "200px"
        });
        dataList.push({
            field: "soDonHang", title: $.i18n('header_donhang'), template: function (dataItem) {
                return "<a href='' class='color-primary' ng-click='openFormDsDonHang(" + kendo.htmlEncode(dataItem.iD_NhanVien) + ")'>" + kendo.toString(dataItem.soDonHang, $rootScope.UserInfo.dinhDangSo) + "</a>";
            },
            footerTemplate: formatNumberInFooterGrid('soDonHang.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: center" }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "70px"
        });
        dataList.push({
            field: "doanhThu", title: $.i18n('header_doanhthu'), template: function (dataItem) {
                return "<a href='' class='color-primary' ng-click='openFormDsDonHang(" + kendo.htmlEncode(dataItem.iD_NhanVien) + ")'>" + kendo.toString(dataItem.doanhThu, $rootScope.UserInfo.dinhDangSo) + "</a>";
            },
            footerTemplate: formatNumberInFooterGrid('doanhThu.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: right" }, attributes: { style: "text-align: right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        });
        return dataList;
    }

    function load_chart() {
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);

        bieuDoDoanhThuDataService.getBieuDoTop10DoanhThuTheoNhanVien(fromdate, todate).then(function (result) {
            let totaldata = result.data;
            if (totaldata[0].length > 0) {
                $scope.showchart = true;

                $scope.labels = totaldata[0];
                $scope.data = totaldata[1];
                $scope.colors = ['#007bff', '#007bff', '#007bff', '#007bff', '#007bff', '#007bff', '#007bff', '#007bff', '#007bff', '#007bff',];
                $scope.options = {
                    title: {
                        display: false,
                        position: 'bottom',
                        fontColor: 'rgb(255, 99, 132)',
                        text: $.i18n('menu_bieudotop10doanhthutheonhanvien')
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
                                label += ' (VNĐ)';
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
                                    labelString: $.i18n('label_doanhthu')
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
            });
    }
    function loadgrid() {
        kendo.ui.progress($("#grid"), true);
        commonOpenLoadingText("#btn_xembaocao");
        $scope.gridOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".nav-pills").height());
                return heightGrid - 110;
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
        bieuDoDoanhThuDataService.getBaoCaoTop10DoanhThuTheoNhanVien(fromdate, todate).then(function (response) {
            $scope.gridData = {
                data: response.data,
                schema: {
                    model: {
                        fields: {
                            doanhThu: {
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
              { field: "doanhThu", aggregate: "sum" },
              { field: "soDonHang", aggregate: "sum" },

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
        bieuDoDoanhThuDataService.getExcelTop10DoanhThuTheoNhanVien(fromdate, todate).then(function (result) {
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
    $scope.openFormDsDonHang = function (idnhanvien) {
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);

        $state.go('danhsachdonhang', { idnv: idnhanvien, from: fromdate, to: todate });
    }
    init();

})