angular.module('app').controller('bm010_BaoCaoPhanHoiController', function ($rootScope, $scope, Notification, ComboboxDataService, baoCaoKhachHangDataService) {
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
                Notification({ message: 'Không thể load danh sách nhân viên, vui lòng tải lại trang' }, 'warning');
            }
        });
        
        ComboboxDataService.getDataKhachHang().then(function (result) {
            $scope.khachhangData = result.data;
            if (!result.flag) {
                Notification({ message: 'Không thể load danh sách khách hàng, vui lòng tải lại trang' }, 'warning');
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
            field: "thoiGian", title: $.i18n('header_ngay'), template: function (dataItem) {
                return kendo.htmlEncode(kendo.toString(dataItem.thoiGian, formatDate));
            }, attributes: {style: "text-align: center"}, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px"
        });
        dataList.push({
            field: "iD_KhachHang", title: $.i18n('header_makhachhang'), attributes: {
                style: "text-align: center"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px"
        });
        dataList.push({
            field: "tenKhachHang", title: $.i18n('header_tenkhachhang'), attributes: {
                style: "text-align: center"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "170px"
        });
        dataList.push({
            field: "dienThoai", title: $.i18n('header_sodienthoai'), attributes: {
                style: "text-align: center"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "diaChi", title: $.i18n('header_diachi'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "330px"
        });
        dataList.push({
            field: "tenNhanVien", title: $.i18n('header_tennhanvien'), attributes: {
                style: "text-align: center"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "170px"
        });
        dataList.push({
            field: "tenPhanHoi", title: $.i18n('header_tenphanhoi'), attributes: {
                style: "text-align: center"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "130px"
        });
        dataList.push({
            field: "chiTiet", title: $.i18n('header_noidungphanhoi'), attributes: {
                style: "text-align: center"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "130px"
        });
        
        return dataList;
    }
    function loadgrid(idnhanvien, idkhachhang) {
        kendo.ui.progress($("#grid"), true);
        commonOpenLoadingText("#btn_bm010xembaocao");

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

        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);

        baoCaoKhachHangDataService.getBaoCaoPhanHoi(idkhachhang,idnhanvien, fromdate, todate).then(function (result) {
            $scope.gridData = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                            
                            maKH: {
                                type: "string"
                            },
                            thoiGian: {
                                type: "date"
                            },
                        }
                    }
                },
                pageSize: 20,
            };

            kendo.ui.progress($("#grid"), false);
            commonCloseLoadingText("#btn_bm010xembaocao")
        });

    }
    //event
    $scope.xemBaoCao = function () {

        var idnhanvien = 0;
        var idkhachhang = 0;

        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        if ($scope.khachhangselect != undefined)
            idkhachhang = ($scope.khachhangselect.iD_KhachHang < 0) ? 0 : $scope.khachhangselect.iD_KhachHang;

        loadgrid(idnhanvien, idkhachhang);
    }
    $scope.XuatExcel = function () {
        commonOpenLoadingText("#btn_bm010xuatexcel");

        var idnhanvien = 0;
        var idkhachhang = 0;
        
        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        if ($scope.khachhangselect != undefined)
            idkhachhang = ($scope.khachhangselect.iD_KhachHang < 0) ? 0 : $scope.khachhangselect.iD_KhachHang;
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);


        baoCaoKhachHangDataService.getExcelBaoCaoPhanHoi(idkhachhang, idnhanvien, fromdate, todate).then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayravuilongloadlaitrang') }, 'warning');

            commonCloseLoadingText("#btn_bm010xuatexcel")
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
