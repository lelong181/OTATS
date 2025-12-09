angular.module('app').controller('bm004_BaoCaoDoanhThuTheoKhachHangController', function ($rootScope, $scope, Notification, ComboboxDataService, baoCaoKhachHangDataService) {
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
        ComboboxDataService.getDataKhachHang().then(function (result) {
            $scope.khachhangData = result.data;
            if (!result.flag) {
                Notification({ message: 'Không thể load danh sách khách hàng, vui lòng tải lại trang' }, 'warning');
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
            field: "tenKhachHang", title: $.i18n('header_tenkhachhang'), footerTemplate: $.i18n('label_total'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "dienThoai", title: $.i18n('header_dienthoai'), attributes: {
                "class": "table-cell",
                style: "text-align: center"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "tongDonHang", title: $.i18n('header_sodonhang'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('tongDonHang.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: center" }, attributes: {
                "class": "table-cell",
                style: "text-align: center"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "donThanhCong", title: $.i18n('header_sodonhangdahoantat'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('donThanhCong.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: center" }, attributes: {
                "class": "table-cell",
                style: "text-align: center"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "tongTien", title: $.i18n("header_tongtien"), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('tongTien.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: right" }, attributes: {
                "class": "table-cell",
                style: "text-align: right"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "daThanhToan", title: $.i18n('header_tongtiendathanhtoan'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('daThanhToan.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: right" }, attributes: {
                "class": "table-cell",
                style: "text-align: right"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });

        return dataList;
    }
    function loadgrid(idkhachhang) {
        kendo.ui.progress($("#grid"), true);
        commonOpenLoadingText("#btn_xembaocao");
        $scope.gridOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".content-filter").height());
                return heightGrid -20;
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
        baoCaoKhachHangDataService.getBaoCaoDoanhThuTheoKhachHang(fromdate, todate, idkhachhang).then(function (result) {
            $scope.gridData = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                            tongDonHang: {
                                type: "number"
                            },
                            tongTien: {
                                type: "number"
                            },
                            donThanhCong: {
                                type: "number"
                            },
                            daThanhToan: {
                                type: "number"
                            },
                        }
                    }
                },
                pageSize: 20,
                aggregate: [
                    { field: "tongDonHang", aggregate: "sum" },
                    { field: "donThanhCong", aggregate: "sum" },
                    { field: "tongTien", aggregate: "sum" },
                    { field: "daThanhToan", aggregate: "sum" },
                ]

            };
            kendo.ui.progress($("#grid"), false);
            commonCloseLoadingText("#btn_xembaocao")
        });
    }
    //event
    $scope.xemBaoCao = function () {
        var idkhachhang = 0;
        if ($scope.khachhangselect != undefined)
            idkhachhang = ($scope.khachhangselect.iD_KhachHang < 0) ? 0 : $scope.khachhangselect.iD_KhachHang;

        loadgrid(idkhachhang);
    }
    $scope.XuatExcel = function () {
        commonOpenLoadingText("#btn_xuatExcel");

        var idkhachhang = 0;
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        if ($scope.khachhangselect != undefined)
            idkhachhang = ($scope.khachhangselect.iD_KhachHang < 0) ? 0 : $scope.khachhangselect.iD_KhachHang;


        baoCaoKhachHangDataService.getExcelBaoCaoDoanhThuTheoKhachHang(fromdate, todate, idkhachhang).then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayravuilongloadlaitrang') }, 'warning');

            commonCloseLoadingText("#btn_xuatExcel")
        });
    }
    $scope.khachhangOnChange = function () {
        $scope.khachhangselect = this.khachhangselect;
    }
    $scope.fromDateChanged = function () {
        $scope.minDate = $scope.obj_TuNgay;
    }
    $scope.toDateChanged = function () {
        $scope.maxDate = $scope.obj_DenNgay;
    }

    init();

})
