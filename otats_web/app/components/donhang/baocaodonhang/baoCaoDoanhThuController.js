angular.module('app').controller('baoCaoDoanhThuController', function ($rootScope, $scope, $location, Notification, baoCaoDonHangDataService, ComboboxDataService) {
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
                Notification({ message: $.i18n('label_khongtheloaddanhsachnhanvienvuilongtailaitrang') }, 'warning');
            }
        });
        ComboboxDataService.getDataKhachHang().then(function (result) {
            $scope.khachhangData = result.data;
            if (!result.flag) {
                Notification({ message: $.i18n('label_khongtheloaddanhsachkhachhangvuilongtailaitrang') }, 'warning');
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
            field: "createDate", title: $.i18n('header_ngay'),
            template: function (dataItem) {
                return kendo.htmlEncode(kendo.toString(dataItem.createDate, formatDate));
            }, footerTemplate: $.i18n('label_total'),
            attributes: {
                "class": "table-cell",
                style: "text-align: center"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "180px"
        });
        dataList.push({
            field: "soDonHang", title: $.i18n('header_sodonhang'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('soDonHang.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: center" }, attributes: {
                "class": "table-cell",
                style: "text-align: center"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "180px"
        });
        //dataList.push({
        //    field: "daHoanTat", title: $.i18n('header_sodonhangdahoantat'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
        //    footerTemplate: formatNumberInFooterGrid('daHoanTat.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: center" }, attributes: {
        //        "class": "table-cell",
        //        style: "text-align: center"
        //    }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "180px"
        //});
        //dataList.push({
        //    field: "chuaHoanTat", title: $.i18n('header_sodonhangchuahoantat'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
        //    footerTemplate: formatNumberInFooterGrid('chuaHoanTat.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: center" }, attributes: {
        //        "class": "table-cell",
        //        style: "text-align: center"
        //    }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "180px"
        //});
        dataList.push({
            field: "slHuy", title: $.i18n('header_sodonhangdahuy'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('slHuy.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: center" }, attributes: {
                "class": "table-cell",
                style: "text-align: center"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "180px"
        });
        //dataList.push({
        //    field: "tongTienChuaChietKhau", title: $.i18n('header_tongtienchuachietkhau'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
        //    footerTemplate: formatNumberInFooterGrid('tongTienChuaChietKhau.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: right" }, attributes: {
        //        "class": "table-cell",
        //        style: "text-align: right"
        //    }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "180px"
        //});
        //dataList.push({
        //    field: "tongTienChietKhau", title: $.i18n('header_tongtienchietkhau'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
        //    footerTemplate: formatNumberInFooterGrid('tongTienChietKhau.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: right" }, attributes: {
        //        "class": "table-cell",
        //        style: "text-align: right"
        //    }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "180px"
        //});
        dataList.push({
            field: "tongTien", title: $.i18n('header_tongtien'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('tongTien.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: right" }, attributes: {
                "class": "table-cell",
                style: "text-align: right"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "180px"
        });
        dataList.push({
            field: "tienDaThanhToan", title: $.i18n('header_tongtiendathanhtoan'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('tienDaThanhToan.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: right" }, attributes: {
                "class": "table-cell",
                style: "text-align: right"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "180px"
        });


        return dataList;
    }
    function loadgrid(idnhanvien, idkhachhang) {
        kendo.ui.progress($("#grid"), true);
        commonOpenLoadingText("#btn_xemBaoCao");
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
            columns: listColumnsgrid(),
        };

        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        baoCaoDonHangDataService.getBaoCaoDoanhThu(idnhanvien, idkhachhang, fromdate, todate).then(function (result) {
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
                  { field: "soDonHang", aggregate: "sum" },
                  { field: "daHoanTat", aggregate: "sum" },
                  { field: "chuaHoanTat", aggregate: "sum" },
                  { field: "slHuy", aggregate: "sum" },
                  { field: "tongTienChuaChietKhau", aggregate: "sum" },
                  { field: "tongTienChietKhau", aggregate: "sum" },
                  { field: "tongTien", aggregate: "sum" },
                  { field: "tienDaThanhToan", aggregate: "sum" }
                ]
            };
            kendo.ui.progress($("#grid"), false);
            commonCloseLoadingText("#btn_xemBaoCao")
        });
    }
    //event
    $scope.xemBaoCao = function () {
        var idnhanvien = 0;
        var idkhachhang = 0;
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        if ($scope.khachhangselect != undefined)
            idkhachhang = ($scope.khachhangselect.iD_KhachHang < 0) ? 0 : $scope.khachhangselect.iD_KhachHang;
        loadgrid(idnhanvien, idkhachhang);
    }
    $scope.XuatExcel = function () {
        commonOpenLoadingText("#btn_XuatExcel");
        var idnhanvien = 0;
        var idkhachhang = 0;
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        if ($scope.khachhangselect != undefined)
            idkhachhang = ($scope.khachhangselect.iD_KhachHang < 0) ? 0 : $scope.khachhangselect.iD_KhachHang;

        baoCaoDonHangDataService.getExcelBaoCaoDoanhThu(idnhanvien, idkhachhang, fromdate, todate).then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayravuilongloadlaitrang') }, 'warning');

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
    }
    $scope.toDateChanged = function () {
        $scope.maxDate = $scope.obj_DenNgay;
    }
    init();

})
