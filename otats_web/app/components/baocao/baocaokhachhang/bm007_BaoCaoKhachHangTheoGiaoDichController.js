angular.module('app').controller('bm007_BaoCaoKhachHangTheoGiaoDichController', function ($rootScope, $scope, $timeout, Notification, ComboboxDataService, baoCaoKhachHangDataService) {
    CreateSiteMap();


    function init() {
        initcombo();
        loadgrid(0, 0, 0, 0);
    }

    function initcombo() {
        ComboboxDataService.getDataNhanVien().then(function (result) {
            $scope.nhanvienData = result.data;
        });
        ComboboxDataService.getDataKhachHang().then(function (result) {
            $scope.khachhangData = result.data;
        });

        $scope.soNgay = 0;
        let arr = [
            { value: "Không phát sinh doanh thu", id: 0 },
                { value: "Không viếng thăm", id: 1 },
        ]
        if ($rootScope.lang.substring(0, 2) == 'en')
            arr = [
            { value: "No revenue generated", id: 0 },
                { value: "no visited", id: 1 },
            ]

        $scope.loaiData = arr;

        $timeout(function () { $("#loai").data("kendoComboBox").value(0); }, 100);

    }

    function listColumnsgrid() {
        var dataList = [];

        dataList.push({
            field: "tenKhachHang", title: $.i18n('header_tenkhachhang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "130px"
        });
        dataList.push({
            field: "dienThoai", title: $.i18n('header_sodienthoai'), attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "130px"
        });
        dataList.push({
            field: "diaChi", title: $.i18n('header_diachikhachhang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "300px"
        });
        dataList.push({
            field: "tenNhanVien", title: $.i18n('header_tennhanvien'), attributes: {

                style: "text-align: center"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "140px"
        });
        dataList.push({
            field: "insertedtime", title: $.i18n('header_thoigiantao'), template: function (dataItem) {
                return kendo.htmlEncode(kendo.toString(dataItem.insertedtime, formatDateTime));
            }, attributes: {

                style: "text-align: center"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "soDonHang", title: $.i18n('header_sodonhang'), attributes: {

                style: "text-align: center"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "80px"
        });
        dataList.push({
            field: "giaTriDonHang", title: $.i18n('header_doanhthu'), attributes: { style: "text-align: center" }, template: function (dataItem) {
                if (dataItem.giaTriDonHang == null)
                    return '';
                else
                    return kendo.htmlEncode(kendo.toString(dataItem.giaTriDonHang, $rootScope.UserInfo.dinhDangSo));
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px"
        });

        return dataList;
    }
    function loadgrid(idkhachhang, idnhanvien, soNgay, loai) {
        kendo.ui.progress($("#grid"), true);
        commonOpenLoadingText("#btn_bm007xembaocao");

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
            columns: listColumnsgrid()
        };
        idnhanvien = 0;
        idkhachhang = 0;
        loai = 0;

        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        if ($scope.khachhangselect != undefined)
            idkhachhang = ($scope.khachhangselect.iD_KhachHang < 0) ? 0 : $scope.khachhangselect.iD_KhachHang;
        if ($scope.loaiselect != undefined)
            loai = ($scope.loaiselect.id < 0) ? 0 : $scope.loaiselect.id;

        baoCaoKhachHangDataService.getBaoCaoKhachHangTheoGiaoDich(idkhachhang, idnhanvien, soNgay, loai).then(function (result) {
            $scope.gridData = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                            iD_KhachHang: {
                                type: "number"
                            },
                            maKH: {
                                type: "string"
                            },
                            insertedtime: {
                                type: "date"
                            },
                            giaTriDonHang: {
                                type: "number"
                            },
                        }
                    }
                },
                pageSize: 20,
            };

            kendo.ui.progress($("#grid"), false);
            commonCloseLoadingText("#btn_bm007xembaocao")
        });
    }
    //event
    $scope.xemBaoCao = function () {

        let idnhanvien = 0;
        let idkhachhang = 0;
        let loai = 0;

        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        if ($scope.khachhangselect != undefined)
            idkhachhang = ($scope.khachhangselect.iD_KhachHang < 0) ? 0 : $scope.khachhangselect.iD_KhachHang;
        if ($scope.loaiselect != undefined)
            loai = ($scope.loaiselect.id < 0) ? 0 : $scope.loaiselect.id;

        loadgrid(idkhachhang, idnhanvien, $scope.soNgay, loai);
    }
    $scope.XuatExcel = function () {
        commonOpenLoadingText("#btn_bm007xuatexcel");

        let idnhanvien = 0;
        let idkhachhang = 0;
        let loai = 0;

        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        if ($scope.khachhangselect != undefined)
            idkhachhang = ($scope.khachhangselect.iD_KhachHang < 0) ? 0 : $scope.khachhangselect.iD_KhachHang;
        if ($scope.loaiselect != undefined)
            loai = ($scope.loaiselect.id < 0) ? 0 : $scope.loaiselect.id;


        baoCaoKhachHangDataService.getExcelBaoCaoKhachHangTheoGiaoDich(idkhachhang, idnhanvien, $scope.soNgay, loai).then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayravuilongloadlaitrang') }, 'warning');

            commonCloseLoadingText("#btn_bm007xuatexcel")
        });

    }

    $scope.nhanvienOnChange = function () {
        $scope.nhanvienselect = this.nhanvienselect;
    }
    $scope.khachhangOnChange = function () {
        $scope.khachhangselect = this.khachhangselect;
    }
    $scope.loaiOnChange = function () {
        $scope.loaiselect = this.loaiselect;
    }

    init();

})
