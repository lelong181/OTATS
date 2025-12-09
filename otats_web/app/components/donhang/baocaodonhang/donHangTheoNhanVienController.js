angular.module('app').controller('donHangTheoNhanVienController', function ($rootScope, $scope, Notification, baoCaoDonHangDataService, ComboboxDataService) {
    CreateSiteMap();

    function init() {
        initdate();
        initcombo();
        loadgrid(0);
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
            field: "tenNhanVien", title: $.i18n("header_tennhanvien"),
            footerTemplate: $.i18n("label_tong"),
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "tongDonHang", title: $.i18n("header_sodonhang"),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('tongDonHang.sum', $rootScope.UserInfo.dinhDangSo),
            attributes: { style: "text-align: center" }, footerAttributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "donThanhCong", title: $.i18n("header_sodonhangdahoantat"),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), footerTemplate: formatNumberInFooterGrid('donThanhCong.sum', $rootScope.UserInfo.dinhDangSo),
            attributes: { style: "text-align: center" }, footerAttributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }
            , filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "tongTienChuaChietKhau", title: $.i18n("header_tongtienchuachietkhau"),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), footerTemplate: formatNumberInFooterGrid('tongTienChuaChietKhau.sum', $rootScope.UserInfo.dinhDangSo),
            attributes: { class: "text-right" }, footerAttributes: { class: "text-right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "tongTienChietKhau", title: $.i18n("header_tongtienchietkhau"),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), footerTemplate: formatNumberInFooterGrid('tongTienChietKhau.sum', $rootScope.UserInfo.dinhDangSo),
            attributes: { class: "text-right" }, footerAttributes: { class: "text-right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "tongTien", title: $.i18n("header_tongtien"),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), footerTemplate: formatNumberInFooterGrid('tongTien.sum', $rootScope.UserInfo.dinhDangSo),
            attributes: { class: "text-right" }, footerAttributes: { class: "text-right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "daThanhToan", title: $.i18n("header_tongtienthanhtoan"),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), footerTemplate: formatNumberInFooterGrid('daThanhToan.sum', $rootScope.UserInfo.dinhDangSo),
            attributes: { class: "text-right" }, footerAttributes: { class: "text-right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: defaultFilterableGrid, width: "150px"
        });

        return dataList;
    }
    function loadgrid(idnhanvien) {
        kendo.ui.progress($("#grid"), true);
        commonOpenLoadingText("#btn_xemBaoCao");
        $scope.gridOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".content-filter").height());
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
        baoCaoDonHangDataService.getDonHangTheoNhanVien(idnhanvien, fromdate, todate).then(function (result) {
            $scope.gridData = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                            createDate: {
                                type: "date"
                            },

                        }
                    }
                },
                pageSize: 20,
                aggregate: [
                  { field: "tongDonHang", aggregate: "sum" },
                  { field: "donThanhCong", aggregate: "sum" },
                  { field: "tongTienChuaChietKhau", aggregate: "sum" },
                  { field: "tongTienChietKhau", aggregate: "sum" },
                  { field: "tongTien", aggregate: "sum" },
                  { field: "daThanhToan", aggregate: "sum" }
                ]
            };
            kendo.ui.progress($("#grid"), false);
            commonCloseLoadingText("#btn_xemBaoCao")
        });
    }
    //event
    $scope.xemBaoCao = function (idnhanvien) {
        var idnhanvien = 0;
        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        loadgrid(idnhanvien);
    }
    $scope.XuatExcel = function (idnhanvien) {
        commonOpenLoadingText("#btn_XuatExcel");
        var idnhanvien = 0;
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        baoCaoDonHangDataService.getExcelDonHangTheoNhanVien(idnhanvien, fromdate, todate).then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n("label_thongbao"), message: $.i18n("label_coloixayra_loadlaitrang")}, 'warning');

            commonCloseLoadingText("#btn_XuatExcel")
        });
    }
    $scope.nhanvienOnChange = function () {
        $scope.nhanvienselect = this.nhanvienselect;
    }
    $scope.fromDateChanged = function () {
        $scope.minDate = $scope.obj_TuNgay;
    }
    $scope.toDateChanged = function () {
        $scope.maxDate = $scope.obj_DenNgay;
    }
    init();

})
