angular.module('app').controller('bm020_BaoCaoKPINhanVienController', function ($rootScope, $scope, Notification, ComboboxDataService, baoCaoNhanVienDataService) {
    CreateSiteMap();

    function init() {
        initdate();
        initcombo();
        loadgrid(0, 0);
    }
    function initdate() {
        var dateNow = new Date();
        $scope.obj_Thang = new Date(dateNow.setHours(23, 59, 59));

        $scope.monthSelectorOptions = {
            start: "year",
            depth: "year"
        };
    }
    function initcombo() {
        ComboboxDataService.getDataNhomNhanVien().then(function (result) {
            $scope.nhomnhanvienData = result.data;
        });
        ComboboxDataService.getlistnhanvienbymultiidnhom({ IDNhom: -2 }).then(function (result) {
            if (result.flag) {
                $scope.nhanvienData = result.data;
            }
            else
                $scope.nhanvienData = [];
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
            field: "tenNhanVien", title: $.i18n('header_tennhanvien'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px"
        });
        dataList.push({
            title: $.i18n('header_chitieudoanhso'),
            columns: [{
                field: "doanhSo",
                title: $.i18n('header_kehoach'), filterable: false, format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
                width: 100, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { style: "text-align: right" },
            },{
                field: "tong_DoanhSo",
                    title: $.i18n('header_thucte'), filterable: false, format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
                width: 100, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { style: "text-align: right" },
            },
            {
                field: "phanTram_DoanhSo",
                title: $.i18n('header_tile'), filterable: false, format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
                width: 100, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { style: "text-align: right" },
            }],
             headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            title: $.i18n('header_chitieusodonhang'),
            columns: [{
                field: "soDonHang",
                title: $.i18n('header_kehoach'), filterable: false, format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
                width: 100, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { style: "text-align: right" },
            }, {
                field: "tong_SoDonHang",
                    title: $.i18n('header_thucte'), filterable: false, format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
                width: 100, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { style: "text-align: right" },
            },
            {
                field: "phanTram_SoDonHang",
                title: $.i18n('header_tile'), filterable: false, format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
                width: 100, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { style: "text-align: right" },
            }], headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            title: $.i18n('header_chitieungaycong'),
             columns: [{
                 field: "ngayCong",
                 title: $.i18n('header_kehoach'), filterable: false, format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
                 width: 100, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { style: "text-align: right" },
             }, {
                 field: "tong_NgayCong",
                     title: $.i18n('header_thucte'), filterable: false, format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
                 width: 100, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { style: "text-align: right" },
             },
             {
                 field: "phanTram_NgayCong",
                 title: $.i18n('header_tile'), filterable: false, format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
                 width: 100, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { style: "text-align: right" },
             }], headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            title: $.i18n('header_chitieuviengtham'),
            columns: [{
                field: "luotViengTham",
                title: $.i18n('header_kehoach'), filterable: false, format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
                width: 100, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { style: "text-align: right" },
            }, {
                field: "tong_LuotViengTham",
                    title: $.i18n('header_thucte'), filterable: false, format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
                width: 100, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { style: "text-align: right" },
            },
            {
                field: "phanTram_LuotViengTham",
                title: $.i18n('header_tile'), filterable: false, format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
                width: 100, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { style: "text-align: right" },
            }],
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        
        return dataList;
    }
    function loadgrid(idnhom, idnhanvien) {
        kendo.ui.progress($("#grid"), true);
        commonOpenLoadingText("#btn_bm020xembaocao");
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
            columns: listColumnsgrid(),
        };

        let date = $scope.obj_Thang;
        let firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
        let lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);
        lastDay = new Date(lastDay.setHours(23, 59, 59));

        let fromdate = kendo.toString(firstDay, formatDateTimeFilter);
        let todate = kendo.toString(lastDay, formatDateTimeFilter);
        baoCaoNhanVienDataService.getBaoCaoKPINhanVien(idnhom, idnhanvien, fromdate, todate).then(function (result) {
            $scope.gridData = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                            khachHangQuanLy: {
                                type: "number"
                            },
                            soKhachKhongViengTham: {
                                type: "number"
                            },
                            soKhachViengTham: {
                                type: "number"
                            },
                            soLanViengTham: {
                                type: "number"
                            },
                            maKH: {
                                type: "string"
                            },
                            ngay: {
                                type: "date"
                            },
                        }
                    }
                },
                pageSize: 20,
                aggregate: [
                  { field: "khachHangQuanLy", aggregate: "sum" },
                  { field: "soKhachViengTham", aggregate: "sum" },
                  { field: "soLanViengTham", aggregate: "sum" },
                  { field: "soKhachKhongViengTham", aggregate: "sum" }

                ]

            };

            kendo.ui.progress($("#grid"), false);
            commonCloseLoadingText("#btn_bm020xembaocao")
        });
    }
    //event
    $scope.xemBaoCao = function () {

        var idnhanvien = 0;
        var idnhom = 0;

        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        if ($scope.nhomnhanvienselect != undefined)
            idnhom = ($scope.nhomnhanvienselect.iD_Nhom < 0) ? 0 : $scope.nhomnhanvienselect.iD_Nhom;

        loadgrid(idnhom, idnhanvien);
    }
    $scope.XuatExcel = function () {
        commonOpenLoadingText("#btn_bm020xuatexcel");
        var idnhanvien = 0;
        var idnhom = 0;
        let date = $scope.obj_Thang;
        var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
        var lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);

        let fromdate = kendo.toString(firstDay, formatDateTimeFilter);
        let todate = kendo.toString(lastDay, formatDateTimeFilter);
        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        if ($scope.nhomnhanvienselect != undefined)
            idnhom = ($scope.nhomnhanvienselect.iD_Nhom < 0) ? 0 : $scope.nhomnhanvienselect.iD_Nhom;
        baoCaoNhanVienDataService.getExcelBaoCaoKPINhanVien(idnhom, idnhanvien, fromdate, todate).then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayravuilongloadlaitrang') }, 'warning');

            commonCloseLoadingText("#btn_bm020xuatexcel")
        });
    }

    $scope.nhanvienOnChange = function () {
        $scope.nhanvienselect = this.nhanvienselect;
    }
    $scope.nhomnhanvienOnChange = function () {
        $scope.nhomnhanvienselect = this.nhomnhanvienselect;
        idnhom = $scope.nhomnhanvienselect.iD_Nhom;
        ComboboxDataService.getlistnhanvienbymultiidnhom({ IDNhom: idnhom }).then(function (result) {
            if (result.flag) {
                $scope.nhanvienData = result.data;
            }
            else
                $scope.nhanvienData = [];


        });
    };
    init();

})
