angular.module('app').controller('bm033_BieuDoKPITheoNhanVienController', function ($rootScope, $scope, $timeout, Notification, ComboboxDataService, baoCaoNhanVienDataService) {
    CreateSiteMap();
    let idnhom = 0;
    let activelist = 0;
    function init() {
        $scope.colors = ['#007bff', '#e74a3b']
        $scope.series = [$.i18n('label_tongcongviec'), $.i18n('label_sohoanthanh')];
        initdate();
        initcombo();
        loaddata();
    }

    function initdate() {
        var dateNow = new Date();
        $scope.obj_Thang = new Date(dateNow.setHours(23, 59, 59));
        $scope.monthSelectorOptions = {
            start: "year",
            depth: "year"
        };
    }
    function initcombo() {
        ComboboxDataService.getDataNhomNhanVien().then(function (result) {
            $scope.nhomnhanvienData = result.data;
        });
    }
    function listColumnsgrid() {
        var dataList = [];

        dataList.push({
            field: "tenNhanVien", title: $.i18n('header_tennhanvien'), footerTemplate: $.i18n('label_total'), attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "tong", title: $.i18n('header_tongcongviecduocgiao'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('tong.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: center" }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "hoanThanh", title: $.i18n('header_tonghoanthanh'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('hoanThanh.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: center" }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        return dataList;
    }
    function loaddata() {
        let date = $scope.obj_Thang;
        let firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
        let lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);
        lastDay = new Date(lastDay.setHours(23, 59, 59));

        let fromdate = kendo.toString(firstDay, formatDateTimeFilter);
        let todate = kendo.toString(lastDay, formatDateTimeFilter);
        idnhom = 0;
        if ($scope.nhomnhanvienselect != undefined)
            idnhom = ($scope.nhomnhanvienselect.iD_Nhom < 0) ? 0 : $scope.nhomnhanvienselect.iD_Nhom;
        baoCaoNhanVienDataService.getBieuDoKPITheoNhanVien(idnhom, fromdate, todate).then(function (response) {
            kendo.ui.progress($("#grid"), true);
            let totaldata = response.data;
            if (totaldata[0].length > 0) {
                $scope.showchart = true;

                $scope.labels = totaldata[0];
                let data_congviec = [];
                data_congviec.push(totaldata[1]);
                data_congviec.push(totaldata[2]);
                $scope.data = data_congviec;
                $scope.options = {
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
                                label += $.i18n('label_congviec_bieudo');
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
                                    labelString: $.i18n('header_kpicongviec')
                                }
                            },
                        ],
                    }
                };
                kendo.ui.progress($("#grid"), false);
            }
            else {
                $scope.showchart = false;
            }
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

        let date = $scope.obj_Thang;
        let firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
        let lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);
        lastDay = new Date(lastDay.setHours(23, 59, 59));
        idnhom = 0;
        if ($scope.nhomnhanvienselect != undefined)
            idnhom = ($scope.nhomnhanvienselect.iD_Nhom < 0) ? 0 : $scope.nhomnhanvienselect.iD_Nhom;
        let fromdate = kendo.toString(firstDay, formatDateTimeFilter);
        let todate = kendo.toString(lastDay, formatDateTimeFilter);
        baoCaoNhanVienDataService.getBaoCaoKPITheoNhanVien(idnhom, fromdate, todate).then(function (response) {
            $scope.gridData = {
                data: response.data,
                schema: {
                    model: {
                        fields: {
                            tong: {
                                type: "number"
                            },
                            hoanThanh: {
                                type: "number"
                            },

                        }
                    }
                },
                pageSize: 20,
                aggregate: [
                    { field: "tong", aggregate: "sum" },
                    { field: "hoanThanh", aggregate: "sum" },

                ]
            };
            kendo.ui.progress($("#grid"), false);
            commonCloseLoadingText("#btn_xembaocao")

        });
    }
    //event
    $scope.xemBaoCao = function () {
        loaddata();
        if (activelist == 2) {
            loadgrid();
        }
    }
    $scope.clicktabdanhsach = function () {
        activelist = 2;
        $timeout(loadgrid, 200);
    }
    $scope.clicktabbieudo = function () {
        activelist = 1;
        $timeout(loaddata, 200);
    }
    $scope.nhomnhanvienOnChange = function () {
        $scope.nhomnhanvienselect = this.nhomnhanvienselect;
    }

    init();


})