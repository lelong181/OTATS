angular.module('app').controller('bm034_BieuDoDoanhThuThangController', function ($rootScope, $scope, $timeout, Notification, ComboboxDataService, bieuDoDoanhThuDataService) {
    CreateSiteMap();

    function init() {
        $scope.colors = ['#007bff', '#e74a3b']
        $scope.series = ['Tổng công việc', 'Số hoàn thành'];
        initdate();
        initcombo();
        loaddata(0,0,0);
        //loadgrid(0,0,0);
    }

    function initdate() {
        let dateNow = new Date();
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
        ComboboxDataService.getlistnhanvienbymultiidnhom({ IDNhom: -2 }).then(function (result) {
            if (result.flag) {
                $scope.nhanvienData = result.data;
            }
            else
                $scope.nhanvienData = [];
        });
        ComboboxDataService.getDataKhachHang().then(function (result) {
            $scope.khachhangData = result.data;
            if (!result.flag) {
                Notification({ title: $.i18n("label_thongbao"), message: $.i18n("label_khongtheloaddanhsachkhachhangvuilongtailaitrang") }, 'warning');
            }
        });

    }
    function listColumnsgrid() {
        let dataList = [];

        dataList.push({
            field: "ngayHienThi", title: $.i18n('header_thoigian'), attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "donHang", title: $.i18n('header_sodonhang'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('donHang.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: center" }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "tongTien", title: $.i18n('header_doanhthu'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('tongTien.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: right" }, attributes: { style: "text-align: right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        return dataList;
    }
    function loaddata(idnhom, idnhanvien, idkhachhang) {

        let date = $scope.obj_Thang;
        let firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
        let lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);
        lastDay = new Date(lastDay.setHours(23, 59, 59));

        let fromdate = kendo.toString(firstDay, formatDateTimeFilter);
        let todate = kendo.toString(lastDay, formatDateTimeFilter);

        bieuDoDoanhThuDataService.getBieuDoDoanhThuThang(fromdate, todate, idnhom, idnhanvien, idkhachhang).then(function (response) {
            kendo.ui.progress($("#grid"), true);
            commonOpenLoadingText("#btn_xembaocao");

            let totaldata = response.data;
            let data = [];
            let labels = [];
            data.push(totaldata[1]);
            data.push(totaldata[2]);
            labels = totaldata[0];
            setchart(data, labels);

            kendo.ui.progress($("#grid"), false);
            commonCloseLoadingText("#btn_xembaocao")

        });
    }

    function loadgrid(idnhom, idnhanvien, idkhachhang) {
        kendo.ui.progress($("#grid"), true);
        commonOpenLoadingText("#btn_xembaocao");
        $scope.gridOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".content-filter").height() + $(".nav-pills").height());
                return heightGrid - 50;
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

        let date = $scope.obj_Thang;
        let firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
        let lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);
        lastDay = new Date(lastDay.setHours(23, 59, 59));

        let fromdate = kendo.toString(firstDay, formatDateTimeFilter);
        let todate = kendo.toString(lastDay, formatDateTimeFilter);
        bieuDoDoanhThuDataService.getBaoCaoDoanhThuThang(fromdate, todate, idnhom, idnhanvien, idkhachhang).then(function (response) {
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
    function setchart(data, labels) {
        $scope.datasetOverride = [{ yAxisID: 'y-axis-1' }, { yAxisID: 'y-axis-2' }];
        $scope.options = {
            scales: {
                yAxes: [
                  {
                      id: 'y-axis-1',
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
                            labelString: $.i18n('label_doanhthu')
                        }
                  },
                  {
                      id: 'y-axis-2',
                      display: true,
                      labels: $.i18n('label_sodonhang'),
                      position: 'right',
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
                          labelString: $.i18n('label_sodonhang')
                      }
                  }
                ]
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
        };

        $scope.colors = ['#007bff', '#e74a3b'];
        $scope.series = [$.i18n('label_doanhthu'), $.i18n('label_sodonhang')];
        $scope.data = data;
        $scope.labels = labels;
    }
    //event
    $scope.clicktabdanhsach = function () {
        $timeout(function () {
            $scope.xemBaoCao();
        }, 200);
    }
    $scope.clicktabbieudo = function () {
        $timeout(function () {
            $scope.xemBaoCao();
        }, 200);
    }

    $scope.xemBaoCao = function () {
        let idnhom = 0;
        let idnhanvien = 0;
        let idkhachhang = 0;
        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        if ($scope.khachhangselect != undefined)
            idkhachhang = ($scope.khachhangselect.idkh < 0) ? 0 : $scope.khachhangselect.idkh;
        if ($scope.nhomnhanvienselect != undefined)
            idnhom = ($scope.nhomnhanvienselect.iD_Nhom < 0) ? 0 : $scope.nhomnhanvienselect.iD_Nhom;
        loaddata(idnhom, idnhanvien, idkhachhang);
        loadgrid(idnhom, idnhanvien, idkhachhang);
    }
    $scope.xuatExcel = function () {
        commonOpenLoadingText("#btn_xuatexcel");
        let idnhanvien = 0;
        let idkhachhang = 0;
        let idnhom = 0;
        let date = $scope.obj_Thang;
        let firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
        let lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);
        lastDay = new Date(lastDay.setHours(23, 59, 59));
        let fromdate = kendo.toString(firstDay, formatDateTimeFilter);
        let todate = kendo.toString(lastDay, formatDateTimeFilter);
        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        if ($scope.khachhangselect != undefined)
            idkhachhang = ($scope.khachhangselect.idkh < 0) ? 0 : $scope.khachhangselect.idkh;
        if ($scope.nhomnhanvienselect != undefined)
            idnhom = ($scope.nhomnhanvienselect.iD_Nhom < 0) ? 0 : $scope.nhomnhanvienselect.iD_Nhom;
        bieuDoDoanhThuDataService.getExcelDoanhThuThang(fromdate, todate, idnhom, idnhanvien, idkhachhang).then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayravuilongloadlaitrang') }, 'warning');

            commonCloseLoadingText("#btn_xuatexcel")
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
    $scope.khachhangOnChange = function () {
        $scope.khachhangselect = this.khachhangselect;
    }

    init();

})