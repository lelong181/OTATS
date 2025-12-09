angular.module('app').controller('bm002_BaoCaoThuHoiCongNoController', function ($state, $rootScope, $scope, Notification, ComboboxDataService, baoCaoCongNoDataService) {
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
            field: "tenNhanVien", title: $.i18n("header_nhanvien"), footerTemplate: $.i18n("label_tong"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "tenKhachHang", title: $.i18n("header_khachhang"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "dienThoai", title: $.i18n("header_dienthoai"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px"
        });
        dataList.push({
            field: "soTien", title: $.i18n("header_sotien"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('soTien.sum', $rootScope.UserInfo.dinhDangSo),
            footerAttributes: { style: "text-align: right" }, attributes: { style: "text-align: right" }, filterable: false, width: "140px"
        });
        dataList.push({
            title: $.i18n("header_chitiet"),  template: function (dataItem) {
                return '<button class="btn btn-link btn-menubar" ng-click="tacVu()" ><i class="fas fa-clipboard-list fas-md color-primary"></i> </button>';
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { style: "text-align: center" }, filterable: false, width: "60px"
        });

        return dataList;
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
            field: "tenNhanVien", title: $.i18n("header_nhanvien"), footerTemplate: $.i18n("label_tong"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px"
        });
        dataList.push({
            field: "tenKhachHang", title: $.i18n("header_khachhang"),headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px"
        });
        dataList.push({
            field: "ngayThanhToan", title: $.i18n("header_ngaythanhtoan"), template: function (dataItem) {
                d = new Date(dataItem.ngayThanhToan);
                if (dataItem.ngayThanhToan == null)
                    return '';
                else
                    return kendo.htmlEncode(kendo.toString(d, formatDate));
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px"
        });
        dataList.push({
            field: "ghiChu", title: $.i18n("header_ghichu"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px"
        });
        dataList.push({
            field: "soTien", title: $.i18n("header_sotien"), template: function (dataItem) {
                return kendo.toString(dataItem.soTien, $rootScope.UserInfo.dinhDangSo);
            },
            footerTemplate: formatNumberInFooterGrid('soTien.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: right" }, attributes: { style: "text-align: right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
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

        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);

        baoCaoCongNoDataService.getBaoCaoThuHoiCongNo(idkhachhang, idnhanvien, fromdate, todate).then(function (result) {
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
                  { field: "soTien", aggregate: "sum" },
                ]
            };

            kendo.ui.progress($("#grid"), false);
            commonCloseLoadingText("#btn_xemBaoCao")
        });

    }
    //event
    $scope.tacVu = function () {
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
        let idkhachhang = dataItem.iD_KhachHang;
        let idnhanvien = dataItem.iD_NhanVien;
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        baoCaoCongNoDataService.getChiTietBaoCaoThuHoiCongNo(idkhachhang, idnhanvien, fromdate, todate).then(function (response) {
            $scope.griddetailData = {
                data: response.data,
                schema: {
                    model: {
                        fields: {
                            ngayThanhToan: {
                                type: "date"
                            },
                            soTien: {
                                type: "number"
                            },
                        }
                    }
                },
                pageSize: 20,
                aggregate: [
                    { field: "soTien", aggregate: "sum" },]
            };
        });
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
        var idnhanvien = 0;
        var idkhachhang = 0;
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        baoCaoCongNoDataService.getExcelBaoCaoThuHoiCongNo(idkhachhang, idnhanvien, fromdate, todate).then(function (result) {
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
