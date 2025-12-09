angular.module('app').controller('bm036_BieuDoDoanhThuTheoNhanVienController', function ($scope, $rootScope, $timeout, Notification, ComboboxDataService, bieuDoDoanhThuDataService) {
    CreateSiteMap();
    let activelist = 0;
    function init() {
        initdate();
        initcombo();
        load_chart(0,0);
    }
    function initdate() {
        var dateNow = new Date();
        $scope.obj_TuNgay = new Date(dateNow.setHours(0, 0, 0));
        $scope.obj_DenNgay = new Date(dateNow.setHours(23, 59, 59));
        $scope.maxDate = $scope.obj_DenNgay;
        $scope.minDate = $scope.obj_TuNgay;
    }
    function initcombo() {
        ComboboxDataService.getDataNhomNhanVien().then(function (result) {
            $scope.nhomnhanvienData = result.data;
        });
        ComboboxDataService.getlistnhanvienbymultiidnhom({ IDNhom: -2 }).then(function (result) {
            if (result.flag) {
                $scope.nhanvienData = result.data;
            }
            else
                $scope.nhanvienData = [];
        });

    }
    function listColumnsgrid() {
        var dataList = [];
        dataList.push({
            field: "tenNhanVien", title: $.i18n('header_tennhanvien'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "donHang", title: $.i18n('header_donhang'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('donHang.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: center" }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "tongTien", title: $.i18n('header_doanhthu'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('tongTien.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: right" }, attributes: { style: "text-align: right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        return dataList;
    }

    function load_chart(idnhom, idnhanvien) {
        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        if ($scope.nhomnhanvienselect != undefined)
            idnhom = ($scope.nhomnhanvienselect.iD_Nhom < 0) ? 0 : $scope.nhomnhanvienselect.iD_Nhom;
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);

        bieuDoDoanhThuDataService.getBieuDoDoanhThuTheoNhanVien(fromdate, todate, idnhom, idnhanvien).then(function (result) {
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
                        text: $.i18n('menu_bieudodoanhthutheonhanvien')
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
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayra_loadlaitrang') }, 'warning');
        });
    }
    function loadgrid(idnhom, idnhanvien) {
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
         idnhom = 0;
         idnhanvien = 0;
        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        if ($scope.nhomnhanvienselect != undefined)
            idnhom = ($scope.nhomnhanvienselect.iD_Nhom < 0) ? 0 : $scope.nhomnhanvienselect.iD_Nhom;
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        bieuDoDoanhThuDataService.getBaoCaoDoanhThuTheoNhanVien(fromdate, todate, idnhom, idnhanvien).then(function (response) {
            $scope.gridData = {
                data: response.data,
                schema: {
                    model: {
                        fields: {
                            tongTien: {
                                type: "number"
                            },
                            donHang: {
                                type: "number"
                            },

                        }
                    }
                },
                pageSize: 20,
                aggregate: [
              { field: "tongTien", aggregate: "sum" },
              { field: "donHang", aggregate: "sum" },

                ]
            };
            kendo.ui.progress($("#grid"), false);
            commonCloseLoadingText("#btn_xembaocao")

        });
    }

    //event
    $scope.xemBaoCao = function () {
        let idnhom = 0;
        let idnhanvien = 0;
        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        if ($scope.nhomnhanvienselect != undefined)
            idnhom = ($scope.nhomnhanvienselect.iD_Nhom < 0) ? 0 : $scope.nhomnhanvienselect.iD_Nhom;
        load_chart(idnhom, idnhanvien);
        if (activelist == 1) {
            loadgrid(idnhom, idnhanvien);
        }
        //let idnhom = 0;
        //let idnhanvien = 0;
        //if ($scope.nhanvienselect != undefined)
        //    idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        //if ($scope.nhomnhanvienselect != undefined)
        //    idnhom = ($scope.nhomnhanvienselect.iD_Nhom < 0) ? 0 : $scope.nhomnhanvienselect.iD_Nhom;
        //load_chart(idnhom, idnhanvien);
        //loadgrid(idnhom, idnhanvien);
    }
    $scope.xuatExcel = function () {
        commonOpenLoadingText("#btn_xuatExcel");
        var idnhom = 0;
        var idnhanvien = 0;
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        if ($scope.nhomnhanvienselect != undefined)
            idnhom = ($scope.nhomnhanvienselect.iD_Nhom < 0) ? 0 : $scope.nhomnhanvienselect.iD_Nhom;
        bieuDoDoanhThuDataService.getExcelDoanhThuTheoNhanVien(fromdate, todate, idnhom, idnhanvien).then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayravuilongloadlaitrang') }, 'warning');

            commonCloseLoadingText("#btn_xuatExcel")
        });
    }
    $scope.nhomnhanvienOnChange = function () {
        $scope.nhomnhanvienselect = this.nhomnhanvienselect;
        idnhom = $scope.nhomnhanvienselect.iD_Nhom;
        ComboboxDataService.getlistnhanvienbymultiidnhom({ IDNhom: idnhom }).then(function (result) {
            if (result.flag) {
                $scope.nhanvienData = result.data;
            }
            else
                $scope.nhanvienData = [];
        });
    };
    $scope.nhanvienOnChange = function () {
        $scope.nhanvienselect = this.nhanvienselect;
    }
    $scope.fromDateChanged = function () {
        $scope.minDate = $scope.obj_TuNgay;
    }
    $scope.toDateChanged = function () {
        $scope.maxDate = $scope.obj_DenNgay;
    }
    $scope.clicktabdanhsach = function () {
        activelist = 1;
        $timeout(loadgrid, 200);
    }
    $scope.clicktabbieudo = function () {
        activelist = 2;
        $timeout(load_chart, 200);
    }
    init();

}) 