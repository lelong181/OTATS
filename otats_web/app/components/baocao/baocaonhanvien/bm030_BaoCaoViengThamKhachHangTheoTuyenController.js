angular.module('app').controller('bm030_BaoCaoViengThamKhachHangTheoTuyenController', function ($rootScope, $scope, $timeout, Notification, ComboboxDataService, baoCaoNhanVienDataService) {
    CreateSiteMap();
    let idnhanvien = 0;
    let activelist = 1;
    function init() {
        initdate();
        initcombo();
        loadgrid_chitiet();
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
    }
    function listColumnsgrid_chitiet() {
        var dataList = [];

        dataList.push({
            title: "#", template: "#= ++RecordNumber #", width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({
            field: "ngay", title: $.i18n('header_ngay'), template: function (dataItem) {
                d = new Date(dataItem.ngay);
                if (dataItem.ngay == null)
                    return '';
                else
                    return kendo.htmlEncode(kendo.toString(d, formatDate));
            }, footerTemplate: $.i18n('header_tong') + ":", attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "200px"
        });
        dataList.push({
            field: "tenNhanVien", title: $.i18n('header_nhanvien'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px"
        });
        dataList.push({
            field: "tenTuyen", title: $.i18n('header_tuyen'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px"
        });
        dataList.push({
            field: "tongSo", title: $.i18n('header_tongkhachhangtrongtuyen'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('tongSo.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: center" }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "200px"
        });
        dataList.push({
            field: "daViengTham", title: $.i18n('header_sokhachdaviengtham'), attributes: { style: "text-align: center" }, format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('daViengTham.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "200px"
        });
        dataList.push({
            field: "chuaViengTham", title: $.i18n('header_sokhachchuaviengtham'), attributes: { style: "text-align: center" }, format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('chuaViengTham.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "200px"
        });
        dataList.push({
            field: "soDonHang", title: $.i18n('header_sodonhang'), attributes: { style: "text-align: center" }, format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('soDonHang.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "200px"
        });
        dataList.push({
            field: "tongTien", title: $.i18n('header_doanhthu'), attributes: { style: "text-align: right" }, format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('tongTien.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "200px"
        });

        return dataList;
    }
    function loadgrid_chitiet() {
        kendo.ui.progress($("#grid_chitiet"), true);
        commonOpenLoadingText("#btn_bm030xembaocao");
        $scope.gridOptions_chitiet = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".content-filter").height() + $(".nav-pills").height());
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
            columns: listColumnsgrid_chitiet(),
        };
        idnhanvien = 0;
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);

        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        baoCaoNhanVienDataService.getBaoCaoViengThamKhachHangTheoTuyen(idnhanvien, fromdate, todate).then(function (result) {
            $scope.gridData_chitiet = {
                data: result.data,
                schema: {
                    model: {
                        ngay: {
                            type: "date"
                        },
                        tongTien: {
                            type: "number"
                        },
                        soDonHang: {
                            type: "number"
                        },
                        tongSo: {
                            type: "number"
                        },
                        daViengTham: {
                            type: "number"
                        },
                        chuaViengTham: {
                            type: "number"
                        },
                    }
                },
                pageSize: 20,
                aggregate: [
                    { field: "chuaViengTham", aggregate: "sum" },
                    { field: "daViengTham", aggregate: "sum" },
                    { field: "tongSo", aggregate: "sum" },
                    { field: "soDonHang", aggregate: "sum" },
                    { field: "tongTien", aggregate: "sum" },
                ]
            };
            kendo.ui.progress($("#grid_chitiet"), false);
            commonCloseLoadingText("#btn_bm030xembaocao")
        });

    }
    function listColumnsgrid_soluong() {
        var dataList = [];

        dataList.push({
            title: "#", template: "#= ++RecordNumber #", width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({
            field: "ngay", title: $.i18n('header_ngay'), template: function (dataItem) {
                d = new Date(dataItem.ngay);
                if (dataItem.ngay == null)
                    return '';
                else
                    return kendo.htmlEncode(kendo.toString(d, formatDate));
            }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px"
        });
        dataList.push({
            field: "tenTuyen", title: $.i18n('header_tuyen'), attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px"
        });
        dataList.push({
            field: "tenKhachHang", title: $.i18n('header_tenkhachhang'), attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px"
        });
        dataList.push({
            field: "dienThoai", title: $.i18n('header_sodienthoai'), attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px"
        });
        dataList.push({
            field: "trangThai", title: $.i18n('header_trangthai'), attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "200px"
        });

        return dataList;
    }
    function loadgrid_soluong() {
        kendo.ui.progress($("#grid_soluong"), true);
        commonOpenLoadingText("#btn_bm030xembaocao");
        $scope.gridOptions_soluong = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".content-filter").height() + $(".nav-pills").height());
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
            columns: listColumnsgrid_soluong(),
        };

        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        idnhanvien = 0;
        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        baoCaoNhanVienDataService.getBaoCaoSoLuongViengThamKhachHangTheoTuyen(idnhanvien, fromdate, todate).then(function (result) {
            $scope.gridData_soluong = {
                data: result.data,
                schema: {
                    model: {
                        ngay: {
                            type: "date"
                        }
                    }
                },
                pageSize: 20,
            };
            kendo.ui.progress($("#grid_soluong"), false);
            commonCloseLoadingText("#btn_bm030xembaocao")
        });

    }
    //event

    $scope.xemBaoCao = function () {
        loadgrid_chitiet();
        if (activelist == 2) {
            loadgrid_soluong();
        }
        if (activelist == 1) {
            loadgrid_chitiet();
        }
    }
    $scope.XuatExcel = function () {
        commonOpenLoadingText("#btn_bm030xuatexcel");
        idnhanvien = 0;
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        if (activelist == 2) {
            baoCaoNhanVienDataService.getExcelBaoCaoSoLuongViengThamKhachHangTheoTuyen(idnhanvien, fromdate, todate).then(function (result) {
                if (result.flag)
                    commonDownFile(result.data);
                else
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayravuilongloadlaitrang') }, 'warning');

                commonCloseLoadingText("#btn_bm030xuatexcel")
            });
        }
        if (activelist == 1) {
            baoCaoNhanVienDataService.getExcelBaoCaoViengThamKhachHangTheoTuyen(idnhanvien, fromdate, todate).then(function (result) {
                if (result.flag)
                    commonDownFile(result.data);
                else
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayravuilongloadlaitrang') }, 'warning');

                commonCloseLoadingText("#btn_bm030xuatexcel")
            });
        }


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
    $scope.clicktabchitiet = function () {
        activelist = 1;
        $timeout(loadgrid_chitiet, 200);
    }
    $scope.clicktabsoluong = function () {
        activelist = 2;
        $timeout(loadgrid_soluong, 200);
    }
    init();

})
