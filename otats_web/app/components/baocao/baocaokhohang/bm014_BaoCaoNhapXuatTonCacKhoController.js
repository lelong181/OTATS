angular.module('app').controller('bm014_BaoCaoNhapXuatTonCacKhoController', function ($rootScope, $scope, Notification, ComboboxDataService, baoCaoKhoHangDataService) {
    CreateSiteMap();

    let d = new Date();
    let ext = kendo.toString(d, 'ddMMMyyyy') + '.xlsx'
    let filename = ($rootScope.lang == 'vi-vn') ? ('BM075_BaoCaoXuatNhapTonCacKho_' + ext) : ('BM075_InventoryReportByWareHouse_' + ext)

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
                Notification({ title: $.i18n("label_thongbao"), message: $.i18n("label_coloixayra_khongtheloaddanhsachmathang") }, 'warning');
            }
        });
        let arr = [
            { value: $.i18n("label_xemnhapxuatban"), id: 1 },
            { value: $.i18n("label_xemdieuchuyennoibo"), id: 2 },
        ]
        $scope.loaiData = arr;
    }

    function listColumnsgrid() {
        var dataList = [];

        dataList.push({
            field: "iD_Kho", title: "#",
            groupHeaderTemplate: " #= kendo.toString(items[0].tenKho)#",
            template: "#= stt #",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center;padding-bottom: 26px;" }, filterable: false, width: "50px"
        });

        dataList.push({
            field: "maHang", title: $.i18n("header_mahang"),
            footerTemplate: $.i18n("label_tong"), headerAttributes: { "class": "table-header-cell", style: "text-align: center;padding-bottom: 26px;" }, filterable: defaultFilterableGrid, width: "80px"
        });
        dataList.push({
            field: "tenHang", title: $.i18n("header_mathang"), headerAttributes: { "class": "table-header-cell", style: "text-align: center;padding-bottom: 26px;" }, filterable: defaultFilterableGrid, width: "150px"
        })
        dataList.push({
            field: "tenDonVi", title: $.i18n("header_dvt"),
            attributes: { style: "text-align: center" }, template: function (dataItem) {
                if (dataItem.tenDonVi == null)
                    return '';
                else
                    return kendo.htmlEncode(kendo.toString(dataItem.tenDonVi));
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center;padding-bottom: 26px;" }, filterable: false, width: "70px"
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
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".content-filter").height() + $(".toolbarmenu").height());
                return heightGrid - 30;
            },            
            excel: {
                proxyURL: "//demos.telerik.com/kendo-ui/service/export",
                filterable: true,
                allPages: true,
                fileName: filename,
            },
            excelExport: function (e) {
                var columns = e.workbook.sheets[0].columns;
                var sheet = e.workbook.sheets[0];
                sheet.title = $.i18n("label_nhapxuatcackho");
                //"từ" + kendo.toString($("#TuNgay").data("kendoDatePicker").value(), "dd/MM/yyyy") + " đến " + kendo.toString($("#DenNgay").data("kendoDatePicker").value(), "dd/MM/yyyy");
                for (var rowIndex = 0; rowIndex < sheet.rows.length; rowIndex++) {
                    var row = sheet.rows[rowIndex];
                    var flag = false;
                    if (rowIndex == 0) {
                        flag = true;
                    }
                    for (var cellIndex = 0; cellIndex < row.cells.length; cellIndex++) {

                        if (flag) {
                            row.cells[cellIndex].textAlign = 'center';
                            row.cells[cellIndex].bold = true;
                        }
                        row.cells[cellIndex].borderBottom = { color: "#000", size: 1 };
                        row.cells[cellIndex].borderTop = { color: "#000", size: 1 };
                        row.cells[cellIndex].borderRight = { color: "#000", size: 1 };
                        row.cells[cellIndex].borderleft = { color: "#000", size: 1 };
                    }
                }
            },
            dataBinding: function () {
                RecordNumber = (this.dataSource.page() - 1) * this.dataSource.pageSize();
            },
            resizable: true,
            editable: false,
            filterable: {
                mode: "row"
            },
            pageable: $scope.lang == 'vi-vn' ? pageableShort_vi_nodisplay : pageableShort_en_nodisplay,
            columns: listColumnsgrid()
        };
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        baoCaoKhoHangDataService.getBaoCaoTonKho(idkhohang, idmathang, fromdate, todate, idloai).then(function (result) {
            $scope.gridData = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                            iD_Kho: {
                                type: "number"
                            },
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
                group: {
                    field: "iD_Kho"
                },
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
    $scope.xemBaoCao = function () {
        var idkhohang = 0;
        var idmathang = 0;
        var idloai = 0;

        if ($scope.khohangselect != undefined)
            idkhohang = ($scope.khohangselect.iD_Kho < 0) ? 0 : $scope.khohangselect.iD_Kho;
        if ($scope.mathangselect != undefined)
            idmathang = ($scope.mathangselect.id < 0) ? 0 : $scope.mathangselect.id;
        if ($scope.loaiselect != undefined)
            idloai = ($scope.loaiselect.id < 0) ? 0 : $scope.loaiselect.id;

        loadgrid(idkhohang, idmathang, idloai);
    }
    $scope.XuatExcel = function () {
        //commonOpenLoadingText("#btn_xuatexcel");
        //var idkhohang = 0;
        //var idmathang = 0;
        //var idloai = 0;

        //let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        //let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        //if ($scope.khohangselect != undefined)
        //    idkhohang = ($scope.khohangselect.iD_Kho < 0) ? 0 : $scope.khohangselect.iD_Kho;
        //if ($scope.mathangselect != undefined)
        //    idmathang = ($scope.mathangselect.id < 0) ? 0 : $scope.mathangselect.id;
        //if ($scope.loaiselect != undefined)
        //    idloai = ($scope.loaiselect.id < 0) ? 0 : $scope.loaiselect.id;
        //baoCaoKhoHangDataService.getExcelBaoCaoTonKho(idkhohang, idmathang, fromdate, todate, idloai).then(function (result) {
        //    if (result.flag)
        //        commonDownFile(result.data);
        //    else
        //        Notification({ title: $.i18n("label_thongbao"), message: $.i18n("label_coloixayra_loadlaitrang")}, 'warning');

        //    commonCloseLoadingText("#btn_xuatexcel")
        //});
        $("#grid").data("kendoGrid").saveAsExcel();
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
