angular.module('app').controller('bm023_BaoCaoLichSuMatTinHieuController', function ($state, $rootScope, $scope, Notification, ComboboxDataService, baoCaoNhanVienDataService) {
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
            attributes: { class: "text-center" } });
        dataList.push({
            field: "tenNhanVien", title: $.i18n('header_tennhanvien'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "200px"
        });
        dataList.push({
            field: "thoiGianBatDauExport", title: $.i18n('header_thoigianbatdau'), attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px"
        });
        dataList.push({
            field: "thoiGianKetThucExport", title: $.i18n('header_thoigianketthuc'), attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px"
        });
        dataList.push({
            field: "tyle", title: $.i18n('header_vitribatdau'), template: function (dataItem) {
                if (dataItem.viDo_BatDau == '' || dataItem.kinhDo_BatDau == '')
                    return '<button ng-click="xemvitrichuacotoado()" class="btn btn-link btn-menubar" ><i class="fas fa-map-marker-alt fas-sm color-gray"></i></button>';
                else
                    return '<button ng-click="xemvitri(' + kendo.htmlEncode(dataItem.viDo_BatDau) + ',' + kendo.htmlEncode(dataItem.kinhDo_BatDau) + ')" class="btn btn-link btn-menubar" ><i class="fas fa-map-marker-alt fas-sm color-infor"></i></button>';
            },
            attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "200px"
        });
        dataList.push({
            field: "tyle", title: $.i18n('header_vitriketthuc'), template: function (dataItem) {
                if (dataItem.viDo_KetThuc == '' || dataItem.kinhDo_KetThuc == '')
                    return '<button ng-click="xemvitrichuacotoado()" class="btn btn-link btn-menubar" ><i class="fas fa-map-marker-alt fas-sm color-gray"></i></button>';
                else
                    return '<button ng-click="xemvitri2(' + kendo.htmlEncode(dataItem.viDo_KetThuc) + ',' + kendo.htmlEncode(dataItem.kinhDo_KetThuc) + ')" class="btn btn-link btn-menubar" ><i class="fas fa-map-marker-alt fas-sm color-infor"></i></button>';
            }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "200px"
        });
        dataList.push({
            title: $.i18n('header_chitiet'), template: function (dataItem) {
                return '<button class="btn btn-link btn-menubar" ng-click="tacVu()" ><i class="fas fa-clipboard-list fas-md color-primary"></i> </button>';
            }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "200px"
        });

        return dataList;
    }
    function loadgrid(idnhom, idnhanvien) {
        kendo.ui.progress($("#grid"), true);
        commonOpenLoadingText("#btn_bm023xembaocao");
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

        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        baoCaoNhanVienDataService.getBaoCaoLichSuMatTinHieu(idnhom, idnhanvien, fromdate, todate).then(function (result) {
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
            commonCloseLoadingText("#btn_bm023xembaocao")
        });
    }
    //event
    $scope.tacVu = function () {
        $state.go('lotrinhdichuyen', { });
    }
    $scope.xemvitrichuacotoado = function () {
        Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_chuacothongtinvitri') }, 'warning');
    }
    $scope.xemvitri = function (viDo_BatDau, kinhDo_BatDau) {
        let url = 'https://www.google.com/maps/dir/' + viDo_BatDau.toString() + ',' + kinhDo_BatDau.toString();
        window.open(url, '_blank');
    }
    $scope.xemvitri2 = function (viDo_KetThuc, kinhDo_KetThuc) {
        let url = 'https://www.google.com/maps/dir/' + viDo_KetThuc.toString() + ',' + kinhDo_KetThuc.toString();
        window.open(url, '_blank');
    }
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
        commonOpenLoadingText("#btn_bm023xuatexcel");
        var idnhanvien = 0;
        var idnhom = 0;
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        if ($scope.nhomnhanvienselect != undefined)
            idnhom = ($scope.nhomnhanvienselect.iD_Nhom < 0) ? 0 : $scope.nhomnhanvienselect.iD_Nhom;
        baoCaoNhanVienDataService.getExcelBaoCaoLichSuMatTinHieu(idnhom, idnhanvien, fromdate, todate).then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayravuilongloadlaitrang') }, 'warning');

            commonCloseLoadingText("#btn_bm023xuatexcel")
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
    $scope.fromDateChanged = function () {
        $scope.minDate = $scope.obj_TuNgay;
    }
    $scope.toDateChanged = function () {
        $scope.maxDate = $scope.obj_DenNgay;
    }
    init();

})
