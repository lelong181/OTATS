angular.module('app').controller('bm041_BieuDoDonHangTheoKhuVucController', function ($scope, $rootScope, $timeout, Notification, ComboboxDataService, baoCaoSanLuongDataService) {
    CreateSiteMap();

    function init() {
        $scope.colors = ['#ff6384', '#4bc0c0', '#ffcd56'];
        $scope.options = {
            tooltips: {
                callbacks: {
                    label: function (tooltipItem, data) {
                        let dataset = data.datasets[tooltipItem.datasetIndex];
                        let meta = dataset._meta[Object.keys(dataset._meta)[0]];
                        let total = meta.total;
                        let currentValue = dataset.data[tooltipItem.index];
                        currentValue2 = kendo.toString(currentValue, $rootScope.UserInfo.dinhDangSo);
                        currentValue2 += $.i18n('label_don');
                        let percentage = parseFloat((currentValue / total * 100).toFixed(2));
                        return currentValue2 + ' (' + percentage + '%)';
                    },
                    title: function (tooltipItem, data) {
                        return data.labels[tooltipItem[0].index];
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
            plugins: {
                datalabels: {
                    formatter: (value, ctx) => {
                        let sum = 0;
                        let dataArr = ctx.chart.data.datasets[0].data;
                        dataArr.map(data => {
                            sum += data;
                        });
                        let percentage = (value * 100 / sum).toFixed(2) + "%";
                        return percentage;
                    },
                    color: '#fff',
                }
            }
        };

        initdate();
        load_chart();
        loadgrid();
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
            field: "ten", title: $.i18n("header_tenkhuvuc"), footerTemplate: $.i18n("label_tong"), attributes: { style: "text-align: center" }, footerAttributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "soDonHang", title: $.i18n("header_sodonhang"), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('soDonHang.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: center" }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "tongTien", title: $.i18n("header_doanhthu"), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('tongTien.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: right" }, attributes: { style: "text-align: right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        return dataList;
    }

    function load_chart() {
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        baoCaoSanLuongDataService.getBieuDoDonHangTheoKhuVuc(fromdate, todate).then(function (response) {
            let totaldata = response.data;
            $scope.labels = totaldata[0];
            $scope.data = totaldata[1];

        });
    }
    function loadgrid() {
        kendo.ui.progress($("#grid"), true);
        commonOpenLoadingText("#btn_xembaocao");
        $scope.gridOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".content-filter").height() );
                return heightGrid - 170;
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
        baoCaoSanLuongDataService.getBaoCaoDonHangTheoKhuVuc(fromdate, todate).then(function (response) {
            $scope.gridData = {
                data: response.data,
                schema: {
                    model: {
                        fields: {
                            tongTien: {
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
              { field: "tongTien", aggregate: "sum" },
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
        loadgrid();
    }
    $scope.xuatExcel = function () {
        commonOpenLoadingText("#btn_xuatExcel");
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        baoCaoSanLuongDataService.getExcelDonHangTheoKhuVuc(fromdate, todate).then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n("label_thongbao"), message: $.i18n("label_coloixayra_loadlaitrang") }, 'warning');

            commonCloseLoadingText("#btn_xuatExcel")
        });
    }
    $scope.fromDateChanged = function () {
        $scope.minDate = $scope.obj_TuNgay;
    }
    $scope.toDateChanged = function () {
        $scope.maxDate = $scope.obj_DenNgay;
    }
    init();

})