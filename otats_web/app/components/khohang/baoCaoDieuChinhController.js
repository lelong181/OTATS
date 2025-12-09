angular.module('app').controller('baoCaoDieuChinhController', function ($rootScope, $scope, Notification, ComboboxDataService, khoHangDataService) {
    CreateSiteMap();

    function init() {
        initdate();
        initcombobox();

        loadgrid(0);
    }

    function initdate() {
        var dateNow = new Date();
        $scope.obj_TuNgay = new Date(dateNow.setHours(0, 0, 0));
        $scope.obj_DenNgay = new Date(dateNow.setHours(23, 59, 59));
        $scope.maxDate = $scope.obj_DenNgay;
        $scope.minDate = $scope.obj_TuNgay;
    }
    function initcombobox() {
        ComboboxDataService.getDataKhoHang().then(function (result) {
            $scope.khohangData = result.data;
        });
    }
    
    function listColumnsgrid() {
        let dataList = [];

        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({ field: "maHang", title: $.i18n("header_mahang"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px" });
        dataList.push({ field: "tenHang", title: $.i18n("header_tenhang"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "175px" });
        dataList.push({ field: "tenKho", title: $.i18n("header_tenkho"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({
            field: "ngayDieuChinh", title: $.i18n("header_ngaydieuchinh"),
            template: function (dataItem) {
                return kendo.htmlEncode(kendo.toString(dataItem.ngayDieuChinh, formatDateTime));
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({ field: "tenPhieuNhap", title: $.i18n("header_sophieunhap"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "300px" });
        dataList.push({ field: "tenNhanVien", title: $.i18n("header_nhanvienlap"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({
            field: "soLuongPhieuNhap", title: $.i18n("header_soluongphieunhap"),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            attributes: { style: "text-align: center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "soLuongSauDieuChinh", title: $.i18n("header_soluongsaudieuchinh"),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            attributes: { style: "text-align: center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "soLuongDieuChinh", title: $.i18n("header_soluongdieuchinh"),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            attributes: { style: "text-align: center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({ field: "tenLoaiDieuChinh", title: $.i18n("header_loaidieuchinh"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        
        return dataList;
    }
    function loadgrid(idkho) {
        commonOpenLoadingText("#btn_xembaocao");
        kendo.ui.progress($("#grid"), true);
        $scope.gridOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".content-filter").height());
                return heightGrid < 100 ? 500 : heightGrid;
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
        khoHangDataService.baocaodieuchinh(idkho, fromdate, todate).then(function (response) {
            $scope.gridData = {
                data: response.data,
                schema: {
                    model: {
                        fields: {
                            ngayDieuChinh: {
                                type: "date"
                            }
                        }
                    }
                },
                pageSize: 20
            };

            kendo.ui.progress($("#grid"), false);
            commonCloseLoadingText("#btn_xembaocao")
        });
    }

    //event
    $scope.khohangOnChange = function () {
        $scope.khohangselect = this.khohangselect;
    }
    $scope.fromDateChanged = function () {
        $scope.minDate = $scope.obj_TuNgay;
    }
    $scope.toDateChanged = function () {
        $scope.maxDate = $scope.obj_DenNgay;
    }

    $scope.xemBaoCao = function () {
        let idkho = 0;
        if ($scope.khohangselect != undefined)
            idkho = ($scope.khohangselect.iD_Kho < 0) ? 0 : $scope.khohangselect.iD_Kho;

        loadgrid(idkho);
    }
    $scope.xuatExcel = function () {
        commonOpenLoadingText("#btn_xuatExcel");
        let idkho = 0;
        if ($scope.khohangselect != undefined)
            idkho = ($scope.khohangselect.iD_Kho < 0) ? 0 : $scope.khohangselect.iD_Kho;

        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        khoHangDataService.excelbaocaodieuchinh(idkho, fromdate, todate).then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n("label_thongbao"), message: $.i18n("label_coloixayra_loadlaitrang"), }, 'warning');

            commonCloseLoadingText("#btn_xuatExcel")
        });
    }
    
    init();

})