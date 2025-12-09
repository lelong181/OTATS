angular.module('app').controller('bm028_BaoCaoTongHopMatHangTheoNhanVienController', function ($rootScope, $scope, Notification, ComboboxDataService, baoCaoNhanVienDataService) {
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
        ComboboxDataService.getDataNhanVien().then(function (result) {
            $scope.nhanvienData = result.data;
            if (!result.flag) {
                Notification({ title: $.i18n("label_thongbao"), message: $.i18n("label_coloixayra_khongtheloaddanhsachnhanvien") }, 'warning');
            }
        });
        ComboboxDataService.getDataMatHang().then(function (result) {
            console.log(result);
            $scope.mathangData = result.data;
            if (!result.flag) {
                Notification({ title: $.i18n("label_thongbao"), message: $.i18n("label_coloixayra_khongtheloaddanhsachmathang") }, 'warning');
            }
        });
        ComboboxDataService.getDataNhomMatHang().then(function (result) {
            console.log(result);
            $scope.nganhhangData = result.data;
            if (!result.flag) {
                Notification({ title: $.i18n("label_thongbao"), message: $.i18n("label_coloixayra_khongtheloaddanhsachnganhhang") }, 'warning');
            }
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
            field: "tenNhom", title: $.i18n("label_nhom"), footerTemplate: $.i18n("label_tong"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px"
        });
        dataList.push({
            field: "tenNhanVien", title: $.i18n("label_nhanvien"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px"
        });
        dataList.push({
            field: "maHang", title: $.i18n("header_mahang"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px"
        });
        dataList.push({
            field: "tenHang", title: $.i18n("header_mathang"),  headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px"
        });
        dataList.push({
            field: "tenDonVi", title: $.i18n("header_donvi"), attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        });
        dataList.push({
            field: "soLuong", title: $.i18n("header_soluong"),
            template: function (dataItem) {
                return "<a href='' class='color-primary' ng-click='openformdetail()'>" + kendo.toString(dataItem.soLuong, $rootScope.UserInfo.dinhDangSo) + "</a>";
            },
            footerTemplate: formatNumberInFooterGrid('soLuong.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: center" }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        });
        dataList.push({
            field: "tongTien", title: $.i18n("header_tongtien"),
            template: function (dataItem) {
                return "<a href='' class='color-primary' ng-click='openformdetail()'>" + kendo.toString(dataItem.tongTien, $rootScope.UserInfo.dinhDangSo) + "</a>";
            },
            footerTemplate: formatNumberInFooterGrid('tongTien.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: right" }, attributes: { style: "text-align: right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        });
        dataList.push({
            title: $.i18n("header_chitiet"),template: function (dataItem) {
                return '<button class="btn btn-link btn-menubar" ng-click="openformdetail()" ><i class="fas fa-clipboard-list fas-md color-primary"></i> </button>';
            }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "70px"
        });

        return dataList;
    }
    function loadgrid(idnhanvien, idmathang, iD_Nhom) {
        kendo.ui.progress($("#grid"), true);
        commonOpenLoadingText("#btn_bm028xembaocao");

        $scope.gridOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".content-filter").height() + $(".toolbarmenu").height());
                return heightGrid - 20;
            },
            dataBinding: function () {
                RecordNumber = (this.dataSource.page() - 1) * this.dataSource.pageSize();
            },
            resizable: true,
            editable: false,
            excel: {
                allPages: true
            },
            excelExport: function (e) {

            },
            filterable: {
                mode: "row"
            },
            pageable: $scope.lang == 'vi-vn' ? pageableShort_vi : pageableShort_en,
            columns: listColumnsgrid()
        };
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        baoCaoNhanVienDataService.getBaoCaoTongHopMatHangTheoNhanVien(idnhanvien, idmathang, iD_Nhom, fromdate, todate).then(function (result) {
            $scope.gridData = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                            soLuong: {
                                type: "number"
                            },
                            tongTien: {
                                type: "number"
                            },
                        }
                    }
                },
                pageSize: 20,
                aggregate: [

                  { field: "soLuong", aggregate: "sum" },
                  { field: "tongTien", aggregate: "sum" },
                ]

            };

            kendo.ui.progress($("#grid"), false);
            commonCloseLoadingText("#btn_bm028xembaocao")
        });

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
            field: "tenNhanVien", title: $.i18n("label_nhanvien"), footerTemplate: $.i18n("label_tong"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px"
        });
        dataList.push({
            field: "thoiGian", title: $.i18n("header_thoigian"),
            template: function (dataItem) { return kendo.htmlEncode(kendo.toString(dataItem.thoiGian, formatDate)); },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px"
        });
        dataList.push({
            field: "maHang", title: $.i18n("header_mahang"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px"
        });
        dataList.push({
            field: "tenHang", title: $.i18n("header_mathang"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px"
        });
        dataList.push({
            field: "tenDonVi", title: $.i18n("header_donvi"), attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        });
        dataList.push({
            field: "soLuong", title: $.i18n("header_soluong"),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('soLuong.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: center" }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        });
        dataList.push({
            field: "tongTien", title: $.i18n("header_tongtien"),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('tongTien.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: right" }, attributes: { style: "text-align: right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        });

        return dataList;
    }
    function loadgriddetail(idnhanvien, idmathang) {
        kendo.ui.progress($("#griddetail"), true);

        $scope.griddetailOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height();
                return heightGrid - 80;
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
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        baoCaoNhanVienDataService.getBaoCaoTongHopMatHangTheoNhanVien_ChiTiet(idnhanvien, idmathang, fromdate, todate).then(function (result) {
            $scope.griddetailData = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                            thoiGian: {
                                type: "date"
                            },
                            soLuong: {
                                type: "number"
                            },
                            tongTien: {
                                type: "number"
                            },
                        }
                    }
                },
                pageSize: 20,
                aggregate: [

                  { field: "soLuong", aggregate: "sum" },
                  { field: "tongTien", aggregate: "sum" },
                ]

            };

            kendo.ui.progress($("#griddetail"), false);
        });

    }

    //event
    $scope.xemBaoCao = function () {
        var idnhanvien = 0;
        var idmathang = 0;
        var iD_Nhom = 0;

        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        if ($scope.mathangselect != undefined)
            idmathang = ($scope.mathangselect.id < 0) ? 0 : $scope.mathangselect.id;
        if ($scope.nganhhangselect != undefined)
            iD_Nhom = ($scope.nganhhangselect.iD_DANHMUC < 0) ? 0 : $scope.nganhhangselect.iD_DANHMUC;
        loadgrid(idnhanvien, idmathang, iD_Nhom);
    }
    $scope.XuatExcel = function () {
        //commonOpenLoadingText("#btn_bm028xuatexcel");

        //var idnhanvien = 0;
        //var idmathang = 0;
        //var iD_Nhom = 0;
        //let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        //let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        //if ($scope.nhanvienselect != undefined)
        //    idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        //if ($scope.mathangselect != undefined)
        //    idmathang = ($scope.mathangselect.id < 0) ? 0 : $scope.mathangselect.id;
        //if ($scope.nganhhangselect != undefined)
        //    iD_Nhom = ($scope.nganhhangselect.iD_DANHMUC < 0) ? 0 : $scope.nganhhangselect.iD_DANHMUC;
        //baoCaoNhanVienDataService.getExcelBaoCaoTongHopMatHangTheoNhanVien(idnhanvien, idmathang, iD_Nhom, fromdate, todate).then(function (result) {
        //    if (result.flag)
        //        commonDownFile(result.data);
        //    else
        //        Notification({ title: $.i18n("label_thongbao"), message: $.i18n("label_coloixayra_loadlaitrang") }, 'warning');

        //    commonCloseLoadingText("#btn_bm028xuatexcel")
        //});
        $("#grid").data("kendoGrid").saveAsExcel();
    }

    $scope.openformdetail = function () {
        let myGrid = $('#grid').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;

        let idnhanvien = dataItem.iD_NhanVien;
        let idmathang = dataItem.iD_Hang;

        $scope.formdetail.center().maximize().open();
        loadgriddetail(idnhanvien, idmathang);
    }

    $scope.nhanvienOnChange = function () {
        $scope.nhanvienselect = this.nhanvienselect;
    }
    $scope.mathangOnChange = function () {
        $scope.mathangselect = this.mathangselect;
    }
    $scope.nganhhangOnChange = function () {
        $scope.nganhhangselect = this.nganhhangselect;
    }
    $scope.fromDateChanged = function () {
        $scope.minDate = $scope.obj_TuNgay;
    }
    $scope.toDateChanged = function () {
        $scope.maxDate = $scope.obj_DenNgay;
    }

    init();

})
