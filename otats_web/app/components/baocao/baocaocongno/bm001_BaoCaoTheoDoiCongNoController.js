angular.module('app').controller('bm001_BaoCaoTheoDoiCongNoController', function ($state, $rootScope, $location, $scope, Notification, ComboboxDataService, baoCaoCongNoDataService) {
    CreateSiteMap();

    function init() {
        getquyen();
        initdate();
        initcombo();
        loadgrid(0, 0);
    }
    function getquyen() {
        let path = $location.path();
        let url = path.replace('/', '')
        ComboboxDataService.getquyen(url).then(function (result) {
            $scope.permission = result.data;
            if ($scope.permission.iD_ChucNang <= 0) {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_khongcoquyentruycapchucnang') }, "error");
                $location.path('/home')
            }
        });
    }
    function initdate() {
        let dateNow = new Date();
        let d = new Date(new Date().getFullYear(), 0, 1);
        $scope.obj_TuNgay = new Date(d.setHours(0, 0, 0));
        $scope.obj_DenNgay = new Date(dateNow.setHours(23, 59, 59));
        $scope.maxDate = $scope.obj_DenNgay;
        $scope.minDate = $scope.obj_TuNgay;
    }
    function initcombo() {
        ComboboxDataService.getDataNhanVien().then(function (result) {
            $scope.nhanvienData = result.data;
            if (!result.flag) {
                Notification({ title: $.i18n("label_thongbao"), message: $.i18n("label_khongtheloaddanhsachnhanvienvuilongtailaitrang") }, 'warning');
            }
        });
        ComboboxDataService.getDataKhachHang().then(function (result) {
            $scope.khachhangData = result.data;
            if (!result.flag) {
                Notification({ title: $.i18n("label_thongbao"), message: $.i18n("label_khongtheloaddanhsachkhachhangvuilongtailaitrang") }, 'warning');
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
            field: "maKH", title: $.i18n("header_makhachhang"), footerTemplate: $.i18n("label_tong"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px"
        });
        dataList.push({
            field: "tenKhachHang", title: $.i18n("header_khachhang"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "dienThoai", title: $.i18n("header_dienthoai"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px"
        });
        dataList.push({
            field: "diaChi", title: $.i18n("header_diachi"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "220px"
        });
        dataList.push({
            field: "tienConNo", title: $.i18n("header_sotien"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('tienConNo.sum', $rootScope.UserInfo.dinhDangSo),
            footerAttributes: { style: "text-align: right" }, attributes: { style: "text-align: right" }, filterable: false, width: "140px"
        });
        dataList.push({
            title: $.i18n("header_chitiet"), template: function () {
                return '<button class="btn btn-link btn-menubar" ng-click="tacVu()" ><i class="fas fa-clipboard-list fas-md color-primary"></i> </button>';
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { style: "text-align: center" }, filterable: false, width: "60px"
        });

        return dataList;
    }
    function loadgrid(idkhachhang, idnhanvien) {
        kendo.ui.progress($("#grid"), true);
        commonOpenLoadingText("#btn_xemBaoCao");

        $scope.gridOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".content-filter").height());
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
            columns: listColumnsgrid()
        };

        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);

        baoCaoCongNoDataService.getBaoCaoTheoDoiCongNo(idkhachhang, idnhanvien, fromdate, todate).then(function (result) {
            $scope.gridData = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                        }
                    }
                },
                pageSize: 20,
                aggregate: [
                  { field: "tienConNo", aggregate: "sum" },
                ]
            };

            kendo.ui.progress($("#grid"), false);
            commonCloseLoadingText("#btn_xemBaoCao")
        });

    }
    //event
    $scope.tacVu = function () {
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);

        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;
        let idkhachhang = dataItem.iD_KhachHang;
        $state.go('danhsachdonhang', { idkh: idkhachhang, from: fromdate, to: todate });
    }
    $scope.xemBaoCao = function () {

        let idnhanvien = 0;
        let idkhachhang = 0;

        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        if ($scope.khachhangselect != undefined)
            idkhachhang = ($scope.khachhangselect.iD_KhachHang < 0) ? 0 : $scope.khachhangselect.iD_KhachHang;

        loadgrid(idkhachhang, idnhanvien);
    }
    $scope.XuatExcel = function () {
        commonOpenLoadingText("#btn_XuatExcel");
        let idnhanvien = 0;
        let idkhachhang = 0;
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        if ($scope.khachhangselect != undefined)
            idkhachhang = ($scope.khachhangselect.iD_KhachHang < 0) ? 0 : $scope.khachhangselect.iD_KhachHang;
        baoCaoCongNoDataService.getExcelBaoCaoTheoDoiCongNo(idkhachhang, idnhanvien, fromdate, todate).then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n("label_thongbao"), message: $.i18n("label_coloixayra_loadlaitrang") }, 'warning');

            commonCloseLoadingText("#btn_XuatExcel")
        });

    }
    $scope.nhanvienOnChange = function () {
        $scope.nhanvienselect = this.nhanvienselect;
    }
    $scope.khachhangOnChange = function () {
        $scope.khachhangselect = this.khachhangselect;
    }
    $scope.fromDateChanged = function () {
        $scope.minDate = $scope.obj_TuNgay;
    };
    $scope.toDateChanged = function () {
        $scope.maxDate = $scope.obj_DenNgay;
    };
    init();

})
