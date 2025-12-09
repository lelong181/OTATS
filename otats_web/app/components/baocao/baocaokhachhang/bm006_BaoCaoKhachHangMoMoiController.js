angular.module('app').controller('bm006_BaoCaoKhachHangMoMoiController', function ($rootScope, $scope, Notification, ComboboxDataService, baoCaoKhachHangDataService) {
    CreateSiteMap();

    function init() {
        initdate();
        initcombo();
        loadgrid(0, 0, 0, 0);
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
                Notification({ message: 'Không thể load danh sách nhân viên, vui lòng tải lại trang' }, 'warning');
            }
        });
        ComboboxDataService.getDataKhachHang().then(function (result) {
            $scope.khachhangData = result.data;
            if (!result.flag) {
                Notification({ message: 'Không thể load danh sách khách hàng, vui lòng tải lại trang' }, 'warning');
            }
        });

        $scope.soDonHang = 0;
        $scope.giaTriDonHang = 0;
    }

    function listColumnsgrid() {
        var dataList = [];

        dataList.push({
            field: "tenKhachHang", title: $.i18n('header_khachhang'),
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "130px"
        });
        dataList.push({
            field: "dienThoai", title: $.i18n('header_sodienthoai'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px"
        });
        dataList.push({
            field: "email", title: $.i18n('header_email'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px"
        });
        dataList.push({
            field: "diaChi", title: $.i18n('header_diachikhachhang'),
            template: function (dataItem) {
                if (dataItem.kinhDo == 0 || dataItem.kinhDo == null || dataItem.kinhDo == '')
                    return '<button ng-click="xemvitrichuacotoado()" class="btn btn-link btn-menubar" title ="Vị trí khách hàng" >' + kendo.htmlEncode(dataItem.diaChi) + '</button>';
                else
                    return '<button ng-click="xemvitri(' + kendo.htmlEncode(dataItem.viDo) + ',' + kendo.htmlEncode(dataItem.kinhDo) + ')" class="btn btn-link btn-menubar" title ="Vị trí khách hàng" >' + kendo.htmlEncode(dataItem.diaChi) + '</button>';
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "300px"
        });
        //dataList.push({
        //    field: "tenNhanVien", title: $.i18n('header_tennhanvien'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "130px"
        //});
        dataList.push({
            field: "insertedtime", title: $.i18n('header_thoigiantao'), template: function (dataItem) {
                return kendo.htmlEncode(kendo.toString(dataItem.insertedtime, formatDateTime));
            },
            attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "soDonHang", title: $.i18n('header_sodonhang'),
            attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "145px"
        });
        //dataList.push({
        //    field: "giaTriDonHang", title: $.i18n('header_doanhthu'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), attributes: { style: "text-align: right" },
        //    headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        //});

        return dataList;
    }
    function loadgrid(idnhanvien, idkhachhang, soDonHang, giaTriDonHang) {
        kendo.ui.progress($("#grid"), true);
        commonOpenLoadingText("#btn_bm006xembaocao");
        $scope.gridOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".content-filter").height() + $(".toolbarmenu").height());
                return heightGrid - 30;
            },
            dataBinding: function () {
                RecordNumber = (this.dataSource.page() - 1) * this.dataSource.pageSize();
            },
            excel: {
                allPages: true
            },
            excelExport: function (e) {

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
        baoCaoKhachHangDataService.getBaoCaoKhachHangMoMoi(idnhanvien, idkhachhang, $scope.soDonHang, $scope.giaTriDonHang, fromdate, todate).then(function (result) {
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
                            soDonHang: {
                                type: "number"
                            },
                            giaTriDonHang: {
                                type: "number"
                            },
                            insertedtime: {
                                type: "date"
                            },

                        }
                    }
                },
                pageSize: 20,


            };

            kendo.ui.progress($("#grid"), false);
            commonCloseLoadingText("#btn_bm006xembaocao")
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
        loadgrid(idnhanvien, idkhachhang, $scope.soDonHang, $scope.giaTriDonHang, fromdate, todate);
    }
    $scope.XuatExcel = function () {
        //commonOpenLoadingText("#btn_bm006xuatexcel");
        //var idnhanvien = 0;
        //var idkhachhang = 0;
        //let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        //let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        //if ($scope.nhanvienselect != undefined)
        //    idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        //if ($scope.khachhangselect != undefined)
        //    idkhachhang = ($scope.khachhangselect.iD_KhachHang < 0) ? 0 : $scope.khachhangselect.iD_KhachHang;
        //baoCaoKhachHangDataService.getExcelBaoCaoKhachHangMoMoi(idnhanvien, idkhachhang, $scope.soDonHang, $scope.giaTriDonHang, fromdate, todate).then(function (result) {
        //    if (result.flag)
        //        commonDownFile(result.data);
        //    else
        //        Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayravuilongloadlaitrang') }, 'warning');

        //    commonCloseLoadingText("#btn_bm006xuatexcel")
        //});
        $("#grid").data("kendoGrid").saveAsExcel();
    }

    $scope.xemvitrichuacotoado = function () {
        Notification({ title: $.i18n('label_thongbao'), message: 'Chưa có thông tin vị trí' }, 'warning');
    }
    $scope.xemvitri = function (vido, kinhdo) {
        let url = 'https://www.google.com/maps/dir/' + vido.toString() + ',' + kinhdo.toString();
        window.open(url, '_blank');
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
