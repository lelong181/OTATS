angular.module('app').controller('bm003_BaoCaoAnhChupController', function ($rootScope, $state, $scope, Notification, ComboboxDataService, baoCaoKhachHangDataService) {
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
            field: "tenKhachHang", title: $.i18n('header_khachhang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "170px"
        });
        dataList.push({
            field: "dienThoai", title: $.i18n('header_sodienthoai'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "diaChiKhachHang", title: $.i18n('header_diachikhachhang'),
            template: function (dataItem) {
                if (dataItem.kinhDoKhachHang == 0 || dataItem.kinhDoKhachHang == null || dataItem.kinhDoKhachHang == '')
                    return '<a href="" ng-click="xemvitrichuacotoado()" title ="' + $.i18n('menu_vitrikhachhang') + '" >' + kendo.htmlEncode(dataItem.diaChiKhachHang) + '</a>';
                else
                    return '<a href="" ng-click="xemvitri(' + kendo.htmlEncode(dataItem.viDoKhachHang) + ',' + kendo.htmlEncode(dataItem.kinhDoKhachHang) + ')" title ="' + $.i18n('menu_vitrikhachhang') + '" >' + kendo.htmlEncode(dataItem.diaChiKhachHang) + '</a>';
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "380px"
        });
        dataList.push({
            field: "diaChi", title: $.i18n('header_diachichupanh'),
            template: function (dataItem) {
                if (dataItem.kinhdo == 0 || dataItem.kinhdo == null || dataItem.kinhdo == '')
                    return '<a href="" ng-click="xemvitrichuacotoado()" title ="' + $.i18n('header_diachichupanh') + '" >' + kendo.htmlEncode(dataItem.diaChi) + '</a>';
                else
                    return '<a href="" ng-click="xemvitri(' + kendo.htmlEncode(dataItem.vido) + ',' + kendo.htmlEncode(dataItem.kinhdo) + ')" title ="' + $.i18n('header_diachichupanh') + '" >' + kendo.htmlEncode(dataItem.diaChi) + '</a>';
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "380px"
        });
        dataList.push({
            field: "tenNhanVien", title: $.i18n('header_tennhanvien'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "170px"
        });
        dataList.push({
            field: "insertedtime", title: $.i18n('header_thoigianchup'), template: function (dataItem) {
                return kendo.htmlEncode(kendo.toString(dataItem.insertedtime, formatDateTime));
            }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "ghichu", title: $.i18n('header_ghichu'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "130px"
        });
        dataList.push({
            field: "soLuongAnh", title: $.i18n('header_soluonganh'), attributes: {
                style: "text-align: center"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "130px"
        });
        dataList.push({
            field: "hinhdaidien", title: $.i18n('header_anh'),
            template: function (dataItem) {
                if (dataItem.hinhdaidien == null || dataItem.hinhdaidien == '')
                    return ''
                else {
                    let src = SERVERIMAGE + dataItem.hinhdaidien;
                    return '<img src="' + src + '" alt="" class="img-avatar rounded-circle">';
                }
            },
            attributes: {style: "text-align: center"}, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "120px"
        });
        dataList.push({
            title: $.i18n('header_tacvu'),
            template: function (dataItem) {
                return '<button class="btn btn-link btn-menubar" ng-click="btn_ChiTietAlbum()" title="' + $.i18n('header_xem') + '"><i class="fas fa-file-alt fas-md color-primary"></i> </button>';
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { class: "text-center" }, filterable: false, width: "100px"
        });
        
        return dataList;
    }
    function loadgrid(idnhanvien, idkhachhang) {
        kendo.ui.progress($("#grid"), true);
        commonOpenLoadingText("#btn_bm003xembaocao");

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

        baoCaoKhachHangDataService.getBaoCaoAnhChup(idnhanvien, idkhachhang, fromdate, todate).then(function (result) {
            $scope.gridData = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                            insertedtime: {
                                type: "date"
                            },
                        }
                    }
                },
                pageSize: 20,
            };
            kendo.ui.progress($("#grid"), false);
            commonCloseLoadingText("#btn_bm003xembaocao")
        });
    }
    
    $scope.xemBaoCao = function () {
        let idnhanvien = 0;
        let idkhachhang = 0;
        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        if ($scope.khachhangselect != undefined)
            idkhachhang = ($scope.khachhangselect.iD_KhachHang < 0) ? 0 : $scope.khachhangselect.iD_KhachHang;

        loadgrid(idnhanvien, idkhachhang);
        
    }
    $scope.XuatExcel = function () {
        commonOpenLoadingText("#btn_bm003xuatexcel");
        let idnhanvien = 0;
        let idkhachhang = 0;
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        if ($scope.khachhangselect != undefined)
            idkhachhang = ($scope.khachhangselect.iD_KhachHang < 0) ? 0 : $scope.khachhangselect.iD_KhachHang;

        baoCaoKhachHangDataService.getExcelBaoCaoAnhChup(idnhanvien, idkhachhang,  fromdate, todate).then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayravuilongloadlaitrang') }, 'warning');

            commonCloseLoadingText("#btn_bm003xuatexcel")
        });

    }

    $scope.btn_ChiTietAlbum = function () { 
        let myGrid = $('#grid').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;

        var url = $state.href('album', { idalbum: dataItem.id_album });
        window.open(url, '_blank');
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
