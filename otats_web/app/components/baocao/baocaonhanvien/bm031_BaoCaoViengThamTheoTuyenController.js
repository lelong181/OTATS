angular.module('app').controller('bm031_BaoCaoViengThamTheoTuyenController', function ($state,$rootScope, $scope, Notification, ComboboxDataService, baoCaoNhanVienDataService) {
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
                Notification({ message: $.i18n('label_khongtheloaddanhsachnhanvienvuilongtailaitrang') }, 'warning');
            }
        });
        ComboboxDataService.getTuyen().then(function (result) {
            $scope.tuyenData = result.data;
            if (!result.flag) {
                Notification({ message: $.i18n('label_khongtheloaddanhsachtuyenvuilongtailaitrang') }, 'warning');
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
            field: "ngayExport", title: $.i18n('header_ngay'), attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "120px"
        });
        dataList.push({
            field: "tenNhanVien", title: $.i18n('header_nhanvien'), attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "tenTuyen", title: $.i18n('header_tuyen'), attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "250px"
        });
        dataList.push({
            field: "tyle", title: $.i18n('header_sokhachdacheckin'),
            template: function (dataItem) {
                return "<a href='' class='color-primary' ng-click='openformdetail()'>" + kendo.htmlEncode(dataItem.tyle) + "</a>";
            }, attributes: { style: "text-align: center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "200px"
        });
        
        return dataList;
    }
    function loadgrid(idtuyen, idnhanvien) {
        kendo.ui.progress($("#grid"), true);
        commonOpenLoadingText("#btn_bm031xembaocao");

        $scope.gridOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".content-filter").height());
                return heightGrid < 100 ? 500 : heightGrid;
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
        baoCaoNhanVienDataService.getBaoCaoViengThamTheoTuyen(idtuyen, idnhanvien, fromdate, todate).then(function (result) {
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
            commonCloseLoadingText("#btn_bm031xembaocao")
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
            field: "checkInTime", title: $.i18n('header_ngay'),
            template: function (dataItem) { return kendo.htmlEncode(kendo.toString(dataItem.checkInTime, formatDateTime)); },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "tenNhanVien", title: $.i18n('header_nhanvien'), footerTemplate: $.i18n('label_total'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px"
        });

        dataList.push({
            field: "tenKhachHang", title: $.i18n('header_khachhang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px"
        });
        dataList.push({
            field: "dienThoai", title: $.i18n('header_dienthoai'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px"
        });

        return dataList;
    }
    function loadgriddetail(_idtuyen, _idnhanvien, _day) {
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

        baoCaoNhanVienDataService.baoCaoViengThamKhachHangTheoTuyenChiTiet_detail(_idtuyen, _idnhanvien, _day).then(function (result) {
            $scope.griddetailData = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                            checkInTime: {
                                type: "date"
                            },
                        }
                    }
                },
                pageSize: 20,
            };

            kendo.ui.progress($("#griddetail"), false);
        });

    }

    //event
    $scope.openformdetail = function () {
        let myGrid = $('#grid').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;

        let idtuyen = dataItem.iD_Tuyen;
        let idnhanvien = dataItem.iD_NhanVien;
        let day = kendo.toString(dataItem.ngay, formatDateTimeFilter);

        $scope.formdetail.center().maximize().open();
        loadgriddetail(idtuyen, idnhanvien, day);
    }

    $scope.xemBaoCao = function () {
        var idnhanvien = 0;
        var idtuyen = 0;
        
        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        if ($scope.tuyenselect != undefined)
            idtuyen = ($scope.tuyenselect.iD_KhachHang < 0) ? 0 : $scope.tuyenselect.iD_KhachHang;
        
        loadgrid(idtuyen, idnhanvien);
    }
    $scope.XuatExcel = function () {
        commonOpenLoadingText("#btn_bm031xuatexcel");
        var idnhanvien = 0;
        var idtuyen = 0;
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);

        baoCaoNhanVienDataService.getExcelBaoCaoViengThamTheoTuyen(idtuyen, idnhanvien, fromdate, todate).then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayravuilongloadlaitrang') }, 'warning');

            commonCloseLoadingText("#btn_bm031xuatexcel")
        });

    }

    $scope.nhanvienOnChange = function () {
        $scope.nhanvienselect = this.nhanvienselect;
    }
    $scope.tuyenOnChange = function () {
        $scope.tuyenselect = this.tuyenselect;
    }
    $scope.fromDateChanged = function () {
        $scope.minDate = $scope.obj_TuNgay;
    }
    $scope.toDateChanged = function () {
        $scope.maxDate = $scope.obj_DenNgay;
    }
    init();

})
