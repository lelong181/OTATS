angular.module('app').controller('bm005_BaoCaoKhachHangMatHangDonHangController', function ($rootScope, $scope, Notification, ComboboxDataService, baoCaoKhachHangDataService) {
    CreateSiteMap();
    hideLoadingPage();

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
                Notification({ message: 'Không thể load danh sách nhân viên, vui lòng tải lại trang' }, 'warning');
            }
        });
        //load list khách hàng
        ComboboxDataService.getDataKhachHang().then(function (result) {
            $scope.khachhangData = result.data;
            if (!result.flag) {
                Notification({ message: 'Không thể load danh sách khách hàng, vui lòng tải lại trang' }, 'warning');
            }
        });
        //load list mặt hàng
        ComboboxDataService.getDataMatHang().then(function (result) {
            console.log(result);
            $scope.mathangData = result.data;
            if (!result.flag) {
                Notification({ message: 'Không thể load danh sách mặt hàng, vui lòng tải lại trang' }, 'warning');
            }
        });
    }

    function listColumnsgrid() {
        var dataList = [];

        dataList.push({
            field: "thoiGian", title: $.i18n('header_thoigian'),
            template: function (dataItem) {
                return kendo.htmlEncode(kendo.toString(dataItem.thoiGian, formatDate));
            }, footerTemplate: $.i18n('label_tong') + ":",
            attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "maThamChieu", title: $.i18n('header_donhang'),
            attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "tenNhanVien", title: $.i18n('header_nhanvienlap'), attributes: {

                style: "text-align: center"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "tenKhachHang", title: $.i18n('header_khachhang'), attributes: {
                style: "text-align: center"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        //dataList.push({
        //    field: "dienThoai", title: $.i18n('header_sodienthoai'), attributes: {style: "text-align: center"
        //    }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        //});
        dataList.push({
            field: "maHang", title: $.i18n('header_mahang'), attributes: {
                style: "text-align: center"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "tenHang", title: $.i18n('header_mathang'), attributes: {
                style: "text-align: center"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "tenDonVi", title: $.i18n('header_donvi'), attributes: {
                style: "text-align: center"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "soLuong", title: $.i18n('header_soluong'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('soLuong.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: center" }, attributes: {
                style: "text-align: center"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "ngaySuDung", title: "Ngày dịch vụ",
            format: "{0:dd/MM/yyyy}",
            attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: true, width: "150px"
        });
        //dataList.push({
        //    field: "daGiao", title: $.i18n('header_dagiao'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
        //    footerTemplate: formatNumberInFooterGrid('daGiao.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: center" }, attributes: {style: "text-align: center"
        //    }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px"
        //});
        //dataList.push({
        //    field: "tenTrangThai", title: $.i18n('header_trangthai'), attributes: { style: "text-align: center"}, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px"
        //});
        //dataList.push({
        //    field: "tongTienChietKhau", title: $.i18n('header_tongtienchietkhau'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
        //    footerTemplate: formatNumberInFooterGrid('tongTienChietKhau.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: right" }, attributes: { style: "text-align: right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px"
        //});
        dataList.push({
            field: "tongTien", title: $.i18n('header_thanhtien'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('tongTien.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: right" }, attributes: { style: "text-align: right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px"
        });

        return dataList;
    }
    function loadgrid(idnhanvien, idkhachhang, idmathang) {
        kendo.ui.progress($("#grid"), true);
        commonOpenLoadingText("#btn_bm005xembaocao");

        $scope.gridOptions = {
            sortable: true,
            //persistSelection: true,
            //autoFitColumn: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".content-filter").height() + $(".toolbarmenu").height());
                return heightGrid - 30;
            },
            excel: {
                allPages: true
            },
            excelExport: function (e) {
                //excelExport(e);
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
        baoCaoKhachHangDataService.getBaoCaoMatHang_KH_DH(idnhanvien, idmathang, idkhachhang, fromdate, todate).then(function (result) {
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
                            ngaySuDung: {
                                type: "date"
                            },
                        }
                    }
                },
                pageSize: 20,
                aggregate: [

                    { field: "daGiao", aggregate: "sum" },
                    { field: "soLuong", aggregate: "sum" },
                    { field: "tongTienChietKhau", aggregate: "sum" },
                    { field: "tongTien", aggregate: "sum" }
                ]

            };

            kendo.ui.progress($("#grid"), false);
            commonCloseLoadingText("#btn_bm005xembaocao")
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

        loadgrid(idnhanvien, idkhachhang, idmathang);
    }
    $scope.XuatExcel = function () {
        //commonOpenLoadingText("#btn_bm005xuatexcel");

        //var idnhanvien = 0;
        //var idkhachhang = 0;
        //var idmathang = 0;
        //let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        //let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        //if ($scope.nhanvienselect != undefined)
        //    idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        //if ($scope.khachhangselect != undefined)
        //    idkhachhang = ($scope.khachhangselect.iD_KhachHang < 0) ? 0 : $scope.khachhangselect.iD_KhachHang;
        //if ($scope.mathangselect != undefined)
        //    idmathang = ($scope.mathangselect.id < 0) ? 0 : $scope.mathangselect.id;
        //baoCaoKhachHangDataService.getExcelBaoCaoMatHang_KH_DH(idnhanvien, idmathang, idkhachhang, fromdate, todate).then(function (result) {
        //    if (result.flag)
        //        commonDownFile(result.data);
        //    else
        //        Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayravuilongloadlaitrang') }, 'warning');

        //    commonCloseLoadingText("#btn_bm005xuatexcel")
        //});
        $("#grid").data("kendoGrid").saveAsExcel();
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
    };
    $scope.toDateChanged = function () {
        $scope.maxDate = $scope.obj_DenNgay;
    };
    init();

})
