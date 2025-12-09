angular.module('app').controller('bm027_BaoCaoTongHopCongViecNhanVienController', function ($rootScope, $scope, Notification, ComboboxDataService, baoCaoNhanVienDataService) {
    CreateSiteMap();

    function init() {
        initdate();
        initcombo();
        loadgrid(0, 0);
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
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        
        dataList.push({
            field: "tenNhanVien", title: $.i18n('header_tennhanvien'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px"
        });
        dataList.push({
            field: "soKhachHangMoi", title: $.i18n('header_sokhachhangmoimo'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('soKhachHangMoi.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: center" }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "200px"
        });
        dataList.push({
            field: "soLanViengTham", title: $.i18n('header_solanviengtham'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('soLanViengTham.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: center" }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "200px"
        });
        dataList.push({
            field: "soAnhChup", title: $.i18n('header_soanhchup'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('soAnhChup.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: center" }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "200px"
        });
        dataList.push({
            field: "soDonHang", title: $.i18n('header_sodonhang'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('soDonHang.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: center" }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "200px"
        });
        dataList.push({
            field: "doanhThu", title: $.i18n('header_tongtien'),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), footerTemplate: formatNumberInFooterGrid('doanhThu.sum', $rootScope.UserInfo.dinhDangSo),
            attributes: { style: "text-align: right" }, footerAttributes: { style: "text-align: right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "200px"
        });
        dataList.push({
            field: "soMatHang", title: $.i18n('header_somathang'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('soMatHang.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: center" }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "200px"
        });

        return dataList;
    }
    function loadgrid(idnhom, idnhanvien) {
        kendo.ui.progress($("#grid"), true);
        commonOpenLoadingText("#btn_bm027xembaocao");
        $scope.gridOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".content-filter").height() + $(".toolbarmenu").height());
                return heightGrid - 20;
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
            columns: listColumnsgrid(),
        };

        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        baoCaoNhanVienDataService.getBaoCaoTongHopCongViecNhanVien(idnhom, idnhanvien, fromdate, todate).then(function (result) {
            $scope.gridData = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                            soKhachHangMoi: {
                                type: "number"
                            },
                            soLanViengTham: {
                                type: "number"
                            },
                            soDonHang: {
                                type: "number"
                            },
                            doanhThu: {
                                type: "number"
                            },
                            soMatHang: {
                                type: "number"
                            },
                            soAnhChup: {
                                type: "number"
                            },
                            
                        }
                    }
                },
                pageSize: 20,
                aggregate: [
                  { field: "soMatHang", aggregate: "sum" },
                  { field: "doanhThu", aggregate: "sum" },
                  { field: "soDonHang", aggregate: "sum" },
                  { field: "soAnhChup", aggregate: "sum" },
                  { field: "soLanViengTham", aggregate: "sum" },
                  { field: "soKhachHangMoi", aggregate: "sum" },

                ]

            };

            kendo.ui.progress($("#grid"), false);
            commonCloseLoadingText("#btn_bm027xembaocao")
        });
    }
    //event
    $scope.xemBaoCao = function () {
        let idnhanvien = 0;
        let idnhom = 0;
        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        if ($scope.nhomnhanvienselect != undefined)
            idnhom = ($scope.nhomnhanvienselect.iD_Nhom < 0) ? 0 : $scope.nhomnhanvienselect.iD_Nhom;
        loadgrid(idnhom, idnhanvien);
    }
    $scope.XuatExcel = function () {
        commonOpenLoadingText("#btn_bm027xuatexcel");
        let idnhanvien = 0;
        let idnhom = 0;
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        if ($scope.nhomnhanvienselect != undefined)
            idnhom = ($scope.nhomnhanvienselect.iD_Nhom < 0) ? 0 : $scope.nhomnhanvienselect.iD_Nhom;
        baoCaoNhanVienDataService.getExcelBaoCaoTongHopCongViecNhanVien(idnhom, idnhanvien, fromdate, todate).then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayravuilongloadlaitrang') }, 'warning');

            commonCloseLoadingText("#btn_bm027xuatexcel")
        });
    }

    $scope.nhanvienOnChange = function () {
        $scope.nhanvienselect = this.nhanvienselect;
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
    $scope.fromDateChanged = function () {
        $scope.minDate = $scope.obj_TuNgay;
    }
    $scope.toDateChanged = function () {
        $scope.maxDate = $scope.obj_DenNgay;
    }
    init();

})
