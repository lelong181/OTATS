angular.module('app').controller('bm016_BaoCaoDonHangCoBanKPINhanVienController', function ($rootScope, $scope, Notification, ComboboxDataService, baoCaoNhanVienDataService) {
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
        //load list nhân viên
        ComboboxDataService.getDataNhanVien().then(function (result) {
            $scope.nhanvienData = result.data;
            if (!result.flag) {
                Notification({ message: $.i18n('label_khongtheloaddanhsachnhanvienvuilongtailaitrang')}, 'warning');
            }
        });
        //load list khách hàng
        ComboboxDataService.getDataKhachHang().then(function (result) {
            $scope.khachhangData = result.data;
            if (!result.flag) {
                Notification({ message: $.i18n('label_khongtheloaddanhsachkhachhangvuilongtailaitrang') }, 'warning');
            }
        });
        //load list mặt hàng
        ComboboxDataService.getDataMatHang().then(function (result) {
            console.log(result);
            $scope.mathangData = result.data;
            if (!result.flag) {
                Notification({ message: $.i18n('label_khongtheloaddanhsachmathangvuilongtailaitrang') }, 'warning');
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
            field: "tenHang", title: $.i18n('header_mathang'), footerTemplate: $.i18n('label_total'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px"
        });
        dataList.push({
            field: "slDonHangCoBan", title: $.i18n('header_soluongcoban'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('slDonHangCoBan.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: right" }, attributes: { style: "text-align: right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        });
        dataList.push({
            field: "slDonHangThucTe", title: $.i18n('header_soluongthucte'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('slDonHangThucTe.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: right" }, attributes: { style: "text-align: right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        });
        dataList.push({
            field: "tyLeHoanThanh", title: $.i18n('header_tilehoanthanh'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), attributes: { style: "text-align: right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        });
        dataList.push({
            field: "phatSinh", title: $.i18n('header_phatsinh'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('phatSinh.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: right" }, attributes: { style: "text-align: right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        });
        dataList.push({
            field: "tongSoLuongDonHang", title: $.i18n('header_tongsoluong'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('tongSoLuongDonHang.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: right" }, attributes: { style: "text-align: right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        });
        dataList.push({
            field: "slTieuThu", title: $.i18n('header_soluongtieuthu'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('slTieuThu.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: right" }, attributes: { style: "text-align: right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        });
        dataList.push({
            field: "tyLeTieuThu", title: $.i18n('header_tiletieuthu'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), attributes: { style: "text-align: right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        });

        return dataList;
    }
    function loadgrid(idkhachhang, idnhanvien, idmathang) {
        kendo.ui.progress($("#grid"), true);
        commonOpenLoadingText("#btn_bm016xembaocao");

        $scope.gridOptions = {
            sortable: true,
            //persistSelection: true,
            //autoFitColumn: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".content-filter").height() + $(".toolbarmenu").height());
                return heightGrid - 35;
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
        baoCaoNhanVienDataService.getBaoCaoDonHangCoBanKPINhanVien(idkhachhang, idnhanvien, idmathang, fromdate, todate).then(function (result) {
            $scope.gridData = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                            tyLeTieuThu: {
                                type: "number"
                            },
                            slTieuThu: {
                                type: "number"
                            },
                            tongSoLuongDonHang: {
                                type: "number"
                            },
                            phatSinh: {
                                type: "number"
                            },
                            tyLeHoanThanh: {
                                type: "number"
                            },
                            slDonHangThucTe: {
                                type: "number"
                            },
                            slDonHangCoBan: {
                                type: "number"
                            },
                            
                        }
                    }
                },
                pageSize: 20,
                aggregate: [
                  { field: "slDonHangCoBan", aggregate: "sum" },
                  { field: "slDonHangThucTe", aggregate: "sum" },
                  { field: "tyLeHoanThanh", aggregate: "sum" },
                  { field: "phatSinh", aggregate: "sum" },
                  { field: "tongSoLuongDonHang", aggregate: "sum" },
                  { field: "slTieuThu", aggregate: "sum" },
                  { field: "tyLeTieuThu", aggregate: "sum" },
                ]

            };

            kendo.ui.progress($("#grid"), false);
            commonCloseLoadingText("#btn_bm016xembaocao")
        });

    }
    //event
    $scope.xemBaoCao = function () {

        var idnhanvien = 0;
        var idkhachhang = 0;
        var idmathang = 0;

        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        if ($scope.khachhangselect != undefined)
            idkhachhang = ($scope.khachhangselect.iD_KhachHang < 0) ? 0 : $scope.khachhangselect.iD_KhachHang;
        if ($scope.mathangselect != undefined)
            idmathang = ($scope.mathangselect.id < 0) ? 0 : $scope.mathangselect.id;

        loadgrid(idkhachhang, idnhanvien, idmathang);
    }
    $scope.XuatExcel = function () {
        commonOpenLoadingText("#btn_bm016xuatexcel");

        var idnhanvien = 0;
        var idkhachhang = 0;
        var idmathang = 0;
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        if ($scope.khachhangselect != undefined)
            idkhachhang = ($scope.khachhangselect.iD_KhachHang < 0) ? 0 : $scope.khachhangselect.iD_KhachHang;
        if ($scope.mathangselect != undefined)
            idmathang = ($scope.mathangselect.id < 0) ? 0 : $scope.mathangselect.id;
        baoCaoNhanVienDataService.getExcelBaoCaoDonHangCoBanKPINhanVien(idkhachhang, idnhanvien, idmathang, fromdate, todate).then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayravuilongloadlaitrang') }, 'warning');

            commonCloseLoadingText("#btn_bm016xuatexcel")
        });
    }

    $scope.nhanvienOnChange = function () {
        $scope.nhanvienselect = this.nhanvienselect;
    }
    $scope.khachhangOnChange = function () {
        $scope.khachhangselect = this.khachhangselect;
    }
    $scope.mathangOnChange = function () {
        $scope.mathangselect = this.mathangselect;
    }
    $scope.fromDateChanged = function () {
        $scope.minDate = $scope.obj_TuNgay;
    }
    $scope.toDateChanged = function () {
        $scope.maxDate = $scope.obj_DenNgay;
    }
    init();

})
