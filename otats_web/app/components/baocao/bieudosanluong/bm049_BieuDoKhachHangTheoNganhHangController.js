angular.module('app').controller('bm049_BieuDoKhachHangTheoNganhHangController', function ($scope, $rootScope, $timeout, Notification, ComboboxDataService, baoCaoSanLuongDataService) {
    CreateSiteMap();

    function init() {
        $scope.colors = ['#ffc107', '#dc3545', '#28a745', '#007bff', '#dc3545', '#28a745', '#fd7e14', '#6610f2','#fd7e14'];
        $scope.options = {
            tooltips: {
                callbacks: {
                    label: function(tooltipItem, data) {
                        let dataset = data.datasets[tooltipItem.datasetIndex];
                        let meta = dataset._meta[Object.keys(dataset._meta)[0]];
                        let total = meta.total;
                        let currentValue = dataset.data[tooltipItem.index];
                        currentValue2 = kendo.toString(currentValue, $rootScope.UserInfo.dinhDangSo);
                        let percentage = parseFloat((currentValue / total * 100).toFixed(2));
                        return currentValue2 + ' (' + percentage + '%)';
                    },
                    title: function(tooltipItem, data) {
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
        loadgrid();
        load_chart();
    }
    function listColumnsgrid() {
        let dataList = [];
        dataList.push({
            field: "tenDanhMuc", title: $.i18n("header_tennganhhang"), attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "soKhachHang", title: $.i18n("header_sokhachhang"), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('soKhachHang.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: center" }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        return dataList;
    }

    function load_chart() {
        baoCaoSanLuongDataService.getBieuDoKhachHangTheoNganhHang().then(function (response) {
            let totaldata = response.data;
            $scope.labels = totaldata[0];
            $scope.data = totaldata[1];
        });
    }
    function loadgrid() {
        kendo.ui.progress($("#grid"), true);
        $scope.gridOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height());
                return heightGrid - 230;
            },
            dataBinding: function () {
                RecordNumber = (this.dataSource.page() - 1) * this.dataSource.pageSize();
            },
            resizable: true,
            editable: false,
            filterable: {
                mode: "row"
            },
            pageable: $scope.lang == 'vi-vn' ? pageableShort_vi : pageableShort_en_nodisplay,
            columns: listColumnsgrid()
        };
        baoCaoSanLuongDataService.getBaoCaoKhachHangTheoNganhHang().then(function (response) {
            $scope.gridData = {
                data: response.data,
                schema: {
                    model: {
                        fields: {
                            soKhachHang: {
                                type: "number"
                            },

                        }
                    }
                },
                pageSize: 20,
                aggregate: [
              { field: "soKhachHang", aggregate: "sum" },

                ]
            };
            kendo.ui.progress($("#grid"), false);

        });
    }

    init();

})