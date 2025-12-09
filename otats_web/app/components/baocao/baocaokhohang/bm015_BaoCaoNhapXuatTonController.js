angular.module('app').controller('bm015_BaoCaoNhapXuatTonController', function ($state,$rootScope, $scope, Notification, ComboboxDataService, baoCaoKhoHangDataService) {
    CreateSiteMap();

    function init() {
        initdate();
        initcombo();
        loadgrid(0, 0, 0);
    }
    function initdate() {
        var dateNow = new Date();
        $scope.obj_TuNgay = new Date(dateNow.setHours(0, 0, 0));
        $scope.obj_DenNgay = new Date(dateNow.setHours(23, 59, 59));
        $scope.maxDate = $scope.obj_DenNgay;
        $scope.minDate = $scope.obj_TuNgay;
    }
    function initcombo() {
        ComboboxDataService.getDataKhoHang().then(function (result) {
            $scope.khohangData = result.data;
            if (!result.flag) {
                Notification({ title: $.i18n("label_thongbao"), message: $.i18n("label_coloixayra_khongtheloaddanhsachkhohang") }, 'warning');
            }
        });
        ComboboxDataService.getDataMatHang().then(function (result) {
            $scope.mathangData = result.data;
            if (!result.flag) {
                Notification({ title: $.i18n("label_thongbao"), message: $.i18n("label_coloixayra_khongtheloaddanhsachmathang")  }, 'warning');
            }
        });
        let arr = [
            { value: $.i18n("label_xemnhapxuatban"), id: 1 },
            { value: $.i18n("label_xemdieuchuyennoibo"), id: 2 },
        ]
        $scope.loaiData = arr;
    }
    function listColumnsgriddetail() {
        var dataList = [];

        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({
            field: "ngay", title: $.i18n("header_ngay"), template: function (dataItem) {
                d = new Date(dataItem.ngay);
                if (dataItem.ngay == null)
                    return '';
                else
                    return kendo.htmlEncode(kendo.toString(d, formatDateTime));
            }, footerTemplate: $.i18n('label_total'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "soHieu", title: $.i18n("header_sophieu"), template: function (dataItem) {
                return "<a href='' class='color-primary' ng-click='openFormDsPhieuNhap()'>" + kendo.toString(dataItem.soHieu) + "</a>";
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "70px"
        });
        dataList.push({
            field: "dienGiai", title: $.i18n("header_diengiai"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "maHang", title: $.i18n("header_mahang"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "tenDonVi", title: $.i18n("header_dvt"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            title: $.i18n("header_nhaphang"),
            columns: [{
                field: "soLuongNhap",
                title: $.i18n("header_soluong"), filterable: false, format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
                footerTemplate: formatNumberInFooterGrid('soLuongNhap.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: right" },
                width: 70, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { style: "text-align: right" },
            }, {
                    field: "giaTriNhap",
                title: $.i18n("header_giatri"), filterable: false, format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
                    footerTemplate: formatNumberInFooterGrid('giaTriNhap.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: right" },
                width: 100, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { style: "text-align: right" },
            }], headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            title: $.i18n("header_xuathang"),
            columns: [{
                field: "soLuongXuat",
                title: $.i18n("header_soluong"), filterable: false, format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
                footerTemplate: formatNumberInFooterGrid('soLuongXuat.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: right" },
                width: 70, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { style: "text-align: right" },
            }, {
                    field: "giaTriXuat",
                title: $.i18n("header_giatri"), filterable: false, format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
                    footerTemplate: formatNumberInFooterGrid('giaTriXuat.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: right" },
                width: 100, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { style: "text-align: right" },
            }], headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        return dataList;
    }
    function listColumnsgrid() {
        let dataList = [];

        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center;padding-bottom: 26px;" },
            attributes: { class: "text-center" },
        });
        dataList.push({
            field: "maHang", title: $.i18n("header_mahang"),
            footerTemplate: $.i18n("label_tong"), headerAttributes: { "class": "table-header-cell", style: "text-align: center;padding-bottom: 26px;" }, filterable: defaultFilterableGrid, width: "80px"
        });
        dataList.push({
            field: "tenHang", title: $.i18n("header_mathang"), template: function (dataItem) {
                return "<a href='' class='color-primary' ng-click='openFormDetail(" + kendo.htmlEncode(dataItem.iD_Hang) + ")'>" + kendo.htmlEncode(dataItem.tenHang) + "</a>";
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center;padding-bottom: 26px;" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "tenDonVi", title: $.i18n("header_dvt"),
            attributes: {style: "text-align: center"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center ;padding-bottom: 26px;" }, filterable: false, width: "70px"
        });
        dataList.push({
            title: $.i18n("header_tondauky"),
            columns: [{
                field: "tonSoLuongDauKy",
                title: $.i18n("header_soluong"), filterable: false, format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
                footerTemplate: formatNumberInFooterGrid('tonSoLuongDauKy.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: right" },
                width: 70, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { style: "text-align: right" },
            }, {
                field: "tonGiaTriDauKy",
                title: $.i18n("header_giatri"), filterable: false, format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
                footerTemplate: formatNumberInFooterGrid('tonGiaTriDauKy.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: right" },
                width: 100, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { style: "text-align: right" },
            }], headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            title: $.i18n("header_nhaptrongky"),
            columns: [{
                field: "nhapSoLuongTrongKy",
                title: $.i18n("header_soluong"), filterable: false, format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
                footerTemplate: formatNumberInFooterGrid('nhapSoLuongTrongKy.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: right" },
                width: 70, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { style: "text-align: right" },
            }, {
                field: "nhapGiaTriTrongKy",
                title: $.i18n("header_giatri"), filterable: false, format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
                footerTemplate: formatNumberInFooterGrid('nhapGiaTriTrongKy.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: right" },
                width: 100, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { style: "text-align: right" },
            }], headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            title: $.i18n("header_xuattrongky"),
            columns: [{
                field: "xuatSoLuongTrongKy",
                title: $.i18n("header_soluong"), filterable: false, format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
                footerTemplate: formatNumberInFooterGrid('xuatSoLuongTrongKy.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: right" },
                width: 70, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { style: "text-align: right" },
            }, {
                field: "xuatGiaTriTrongKy",
                title: $.i18n("header_giatri"), filterable: false, format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
                footerTemplate: formatNumberInFooterGrid('xuatGiaTriTrongKy.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: right" },
                width: 100, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { style: "text-align: right" },
            }], headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            title: $.i18n("header_toncuoiky"),
            columns: [{
                field: "tonSoLuongCuoiKy",
                title: $.i18n("header_soluong"), filterable: false, format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
                footerTemplate: formatNumberInFooterGrid('tonSoLuongCuoiKy.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: right" },
                width: 70, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { style: "text-align: right" },
            }, {
                field: "tonGiaTriCuoiKy",
                title: $.i18n("header_giatri"), filterable: false, format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
                footerTemplate: formatNumberInFooterGrid('tonGiaTriCuoiKy.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: right" },
                width: 100, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { style: "text-align: right" },
            }], headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        return dataList;
    }
    function loadgrid(idkhohang, idmathang, idloai) {
        kendo.ui.progress($("#grid"), true);
        commonOpenLoadingText("#btn_xembaocao");

        $scope.gridOptions = {
            sortable: true,
            //persistSelection: true,
            //autoFitColumn: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".content-filter").height() + $(".toolbarmenu").height());
                return heightGrid - 30;
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
        baoCaoKhoHangDataService.getBaoCaoTon(idkhohang, idmathang, fromdate, todate, idloai).then(function (result) {
            $scope.gridData = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                            soLuong: {
                                type: "number"
                            },
                            maKH: {
                                type: "string"
                            },
                            thoiGian: {
                                type: "date"
                            },
                        }
                    }
                },
                pageSize: 20,
                aggregate: [
                  { field: "xuatSoLuongTrongKy", aggregate: "sum" },
                  { field: "xuatGiaTriTrongKy", aggregate: "sum" },
                  { field: "tonSoLuongDauKy", aggregate: "sum" },
                  { field: "tonSoLuongCuoiKy", aggregate: "sum" },
                  { field: "tonGiaTriDauKy", aggregate: "sum" },
                  { field: "tonGiaTriCuoiKy", aggregate: "sum" },
                  { field: "nhapSoLuongTrongKy", aggregate: "sum" },
                  { field: "nhapGiaTriTrongKy", aggregate: "sum" }
                ]

            };

            kendo.ui.progress($("#grid"), false);
            commonCloseLoadingText("#btn_xembaocao")
        });

    }
    //event
    $scope.openFormDsPhieuNhap = function () {
        $state.go('phieunhapkho', { });
    }
    $scope.openFormDetail = function () {
        $scope.formchitiet.center().maximize().open();
        $scope.griddetailOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height());
                return heightGrid - 20;
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
            columns: listColumnsgriddetail()
        };
        let myGrid = $('#grid').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;
        let idmathang = dataItem.iD_Hang;
        $scope.idmathangSelect = idmathang;
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        let idkhohang = 0;
        let idloai = 0;

        if ($scope.khohangselect != undefined)
            idkhohang = ($scope.khohangselect.iD_Kho < 0) ? 0 : $scope.khohangselect.iD_Kho;
        if ($scope.loaiselect != undefined)
            idloai = ($scope.loaiselect.id < 0) ? 0 : $scope.loaiselect.id;
        baoCaoKhoHangDataService.getChiTietBaoCaoTon(idkhohang, idmathang, fromdate, todate, idloai).then(function (response) {
            $scope.griddetailData = {
                data: response.data,
                schema: {
                    model: {
                        fields: {
                            ngay: {
                                type: "date"
                            },
                            soLuongXuat: {
                                type: "number"
                            },
                            soLuongNhap: {
                                type: "number"
                            },
                            giaTriXuat: {
                                type: "number"
                            },
                            giaTriNhap: {
                                type: "number"
                            },
                        }
                    }
                },
                pageSize: 20,
                aggregate: [
                    { field: "giaTriNhap", aggregate: "sum" },
                    { field: "giaTriXuat", aggregate: "sum" },
                    { field: "soLuongNhap", aggregate: "sum" },
                    { field: "soLuongXuat", aggregate: "sum" }
                ]
            };
        });
    }
    $scope.xemBaoCao = function () {
        let idkhohang = 0;
        let idmathang = 0;
        let idloai = 0;

        if ($scope.khohangselect != undefined)
            idkhohang = ($scope.khohangselect.iD_Kho < 0) ? 0 : $scope.khohangselect.iD_Kho;
        if ($scope.mathangselect != undefined)
            idmathang = ($scope.mathangselect.id < 0) ? 0 : $scope.mathangselect.id;
        if ($scope.loaiselect != undefined)
            idloai = ($scope.loaiselect.id < 0) ? 0 : $scope.loaiselect.id;

        loadgrid(idkhohang, idmathang, idloai);
    }
    $scope.XuatExcel = function () {
        commonOpenLoadingText("#btn_xuatexcel");
        let idkhohang = 0;
        let idmathang = 0;
        let idloai = 0;

        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        if ($scope.khohangselect != undefined)
            idkhohang = ($scope.khohangselect.iD_Kho < 0) ? 0 : $scope.khohangselect.iD_Kho;
        if ($scope.mathangselect != undefined)
            idmathang = ($scope.mathangselect.id < 0) ? 0 : $scope.mathangselect.id;
        if ($scope.loaiselect != undefined)
            idloai = ($scope.loaiselect.id < 0) ? 0 : $scope.loaiselect.id;
        baoCaoKhoHangDataService.getExcelBaoCaoTon(idkhohang, idmathang, fromdate, todate, idloai).then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n("label_thongbao"), message: $.i18n("label_coloixayra_loadlaitrang") }, 'warning');

            commonCloseLoadingText("#btn_xuatexcel")
        });
    }

    $scope.XuatExcelChiTiet = function () {
        commonOpenLoadingText("#btn_xuatexcel");
        let idmathang = $scope.idmathangSelect;
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        let idkhohang = 0;
        let idloai = 0;

        if ($scope.khohangselect != undefined)
            idkhohang = ($scope.khohangselect.iD_Kho < 0) ? 0 : $scope.khohangselect.iD_Kho;
        if ($scope.loaiselect != undefined)
            idloai = ($scope.loaiselect.id < 0) ? 0 : $scope.loaiselect.id;
        baoCaoKhoHangDataService.getExcelBaoCaoTonChiTiet(idkhohang, idmathang, fromdate, todate, idloai).then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n("label_thongbao"), message: $.i18n("label_coloixayra_loadlaitrang") }, 'warning');

            commonCloseLoadingText("#btn_xuatexcel")
        });
    }
    $scope.khohangOnChange = function () {
        $scope.khohangselect = this.khohangselect;
    }
    $scope.mathangOnChange = function () {
        $scope.mathangselect = this.mathangselect;
    }
    $scope.loaiOnChange = function () {
        $scope.loaiselect = this.loaiselect;
    }
    $scope.fromDateChanged = function () {
        $scope.minDate = $scope.obj_TuNgay;
    }
    $scope.toDateChanged = function () {
        $scope.maxDate = $scope.obj_DenNgay;
    }
    init();

})
