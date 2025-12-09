
angular.module('app').controller('bm018_BaoCaoDungDoController', function ($state, $rootScope, $scope, Notification, ComboboxDataService, baoCaoNhanVienDataService) {
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
                Notification({ title: $.i18n("label_thongbao"), message: $.i18n("label_coloixayra_khongtheloaddanhsachnhanvien") }, 'warning');
            }
        });
        $scope.dungDo = 0;
    }

    function listColumnsgrid() {
        var dataList = [];

        dataList.push({
            field: "tenNhanVien", title: $.i18n("header_tennhanvien"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "140px"
        });
        dataList.push({
            field: "thoiGianBatDau", title: $.i18n("header_thoigianbatdaudung"), template: function (dataItem) {
                return kendo.htmlEncode(kendo.toString(dataItem.thoiGianBatDau, formatDateTime));
            }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "140px"
        });
        dataList.push({
            field: "thoiGianKetThuc", title: $.i18n("header_thoigianketthuc"), template: function (dataItem) {
                return kendo.htmlEncode(kendo.toString(dataItem.thoiGianKetThuc, formatDateTime));
            }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "140px"
        });
        dataList.push({
            field: "thoiGianDung", title: $.i18n("header_thoigiandung"), template: function (dataItem) {
                return kendo.htmlEncode(kendo.toString(dataItem.thoiGianDung, formatTime));
            },
            attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "120px"
        });
        dataList.push({
            field: "diaChi", title: $.i18n("header_vitri"),
            template: function (dataItem) {
                if (dataItem.kinhDo == 0 && dataItem.viDo == 0)
                    return '<button ng-click="xemvitrichuacotoado()" class="btn btn-link btn-menubar" title ="Vị trí" >' + kendo.htmlEncode(dataItem.diaChi) + '</button>';
                else
                    return '<button ng-click="xemvitri(' + kendo.htmlEncode(dataItem.viDo) + ',' + kendo.htmlEncode(dataItem.kinhDo) + ')" class="btn btn-link btn-menubar" title ="Vị trí" > ' + kendo.htmlEncode(dataItem.diaChi) + '</button>';
            },
            attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "200px"
        });
        dataList.push({
            field: "khGanNhat", title: $.i18n("header_diachikhachhanggannhat"),
            template: function (dataItem) {
                return "<a href='' class='color-primary' ng-click='openFormViTri()'>" + kendo.htmlEncode(dataItem.khGanNhat) + "</a>";
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "200px"
        });

        return dataList;
    }
    function loadgrid(idnhanvien, dungDo) {
        kendo.ui.progress($("#grid"), true);
        commonOpenLoadingText("#btn_bm018xembaocao");
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
        baoCaoNhanVienDataService.getBaoCaoDungDo(idnhanvien, $scope.dungDo, fromdate, todate).then(function (result) {
            $scope.gridData = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                            thoiGianDung: $scope.dungDo,
                            iD_KhachHang: {
                                type: "number"
                            },
                            maKH: {
                                type: "string"
                            },
                            dungDo: {
                                type: "date"
                            },
                            thoiGianBatDau: {
                                type: "date"
                            },
                            thoiGianKetThuc: {
                                type: "date"
                            },
                            thoiGianDung: {
                                type: "date"
                            },

                        }
                    }
                },
                pageSize: 20,


            };

            kendo.ui.progress($("#grid"), false);
            commonCloseLoadingText("#btn_bm018xembaocao")
        });
    }

    //event
    $scope.openFormViTri = function () {

        let myGrid = $('#grid').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;

        let vido = dataItem.viDo;
        let kinhdo = dataItem.kinhDo;

        $state.go('vitrikhachhang', { kinhdo: kinhdo, vido: vido });
    }
    $scope.xemvitrichuacotoado = function () {
        Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_chuacothongtinvitri') }, 'warning');
    }
    $scope.xemvitri = function (vido, kinhdo) {
        let url = 'https://www.google.com/maps/dir/' + vido.toString() + ',' + kinhdo.toString();
        window.open(url, '_blank');
    }
    $scope.xemBaoCao = function () {
        let idnhanvien = 0;
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;

        loadgrid(idnhanvien, $scope.dungDo, fromdate, todate);
        console.log($scope.dungDo)
    }
    $scope.XuatExcel = function () {
        commonOpenLoadingText("#btn_bm018xuatexcel");
        let idnhanvien = 0;
        $scope.dungDo = 0;
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;

        baoCaoNhanVienDataService.getExcelBaoCaoDungDo(idnhanvien, $scope.dungDo, fromdate, todate).then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n("label_thongbao"), message: $.i18n("label_coloixayra_loadlaitrang") }, 'warning');

            commonCloseLoadingText("#btn_bm018xuatexcel")
        });

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
    init();

})

