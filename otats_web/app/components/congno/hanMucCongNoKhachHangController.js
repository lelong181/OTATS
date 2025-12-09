angular.module('app').controller('hanMucCongNoKhachHangController', function ($rootScope, $scope, $state, Notification, ComboboxDataService, hanMucDataService) {
    CreateSiteMap();

    function init() {
        initCombobox();
        loadgrid(0, 0, -1);
    }
    function initCombobox() {

        ComboboxDataService.getTinhThanh().then(function (result) {
            $scope.tinhthanhData = result.data;
        });

        ComboboxDataService.getQuanHuyen(0).then(function (result) {
            $scope.quanhuyenData = result.data;
        });

        ComboboxDataService.getLoaiKhachHang().then(function (result) {
            $scope.loaikhachhangData = result.data;
        });
    }

    function listColumnsgrid() {
        var dataList = [];

        dataList.push({
            field: "chiTiet", title: $.i18n("header_tacvu"),
            template: '<button ng-click="openformsetnguong()" class="btn btn-link btn-menubar" title ="' + $.i18n("label_datnguongcongno") + '" ><i class="fas fa-plus-circle fas-sm color-infor"></i></button> ',
            attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });

        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({
            field: "tenKhachHang", title: $.i18n("header_tenkhachhang"),
            template: function (dataItem) {
                return "<a href='' class='color-primary' ng-click='openDetailFromGrid(" + kendo.htmlEncode(dataItem.iD_KhachHang) + ")'>" + kendo.htmlEncode(dataItem.tenKhachHang) + "</a>";
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px"
        });
        dataList.push({ field: "maKH", title: $.i18n("header_makhachhang"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px" });
        dataList.push({ field: "diaChi", title: $.i18n("header_diachi"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "350px" });
        dataList.push({
            field: "toaDo1", title: $.i18n("header_toado"),
            template: function (dataItem) {
                if (dataItem.toaDo == '' || dataItem.toaDo == '0.0000000000000, 0.0000000000000')
                    return '<button ng-click="xemvitrichuacotoado()" class="btn btn-link btn-menubar" title ="' + $.i18n("menu_vitrikhachhang") + '" ><i class="fas fa-map-marker-alt fas-sm color-gray"></i></button>';
                else
                    return '<button ng-click="xemvitri(' + kendo.htmlEncode(dataItem.viDo) + ',' + kendo.htmlEncode(dataItem.kinhDo) + ')" class="btn btn-link btn-menubar" title ="' + $.i18n("menu_vitrikhachhang") + '" ><i class="fas fa-map-marker-alt fas-sm color-infor"></i></button>';
            },
            attributes: { class: "text-center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: defaultFilterableGrid, width: "80px"
        });
        dataList.push({
            field: "congNoChoPhep", title: $.i18n("header_congnochophep"),
            template: function (dataItem) {
                if (dataItem.congNoChoPhep == null) {
                    return kendo.htmlEncode('0');
                } else {
                    return kendo.toString(dataItem.congNoChoPhep, $rootScope.UserInfo.dinhDangSo);
                }
            },
            attributes: { style: "text-align: right" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({ field: "tenTinh", title: $.i18n("header_tinhtp"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px" });
        dataList.push({ field: "tenQuan", title: $.i18n("header_quanhuyen"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({ field: "tenPhuong", title: $.i18n("header_phuongxa"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({ field: "soDienThoai", title: $.i18n("header_dienthoai"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px" });
        dataList.push({ field: "tenNhanVien", title: $.i18n("header_nhanvientao"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({ field: "tenLoaiKhachHang", title: $.i18n("header_loaikhachhang"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });

        return dataList;
    }
    function loadgrid(idtinh, idquan, idloai) {
        kendo.ui.progress($("#grid"), true);
        $scope.gridOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".content-filter").height());
                return heightGrid - 40;
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
        hanMucDataService.getdatakhachhang(idtinh, idquan, idloai).then(function (result) {
            $scope.gridData = {
                data: result.data,
                schema: {
                    model: {
                        id: 'iD_KhachHang',
                        fields: {
                            iD_KhachHang: {
                                type: "number"
                            },
                            maKH: {
                                type: "string"
                            },
                            ngayTao: {
                                type: "date"
                            }
                        }
                    }
                },
                pageSize: 20
            };
            kendo.ui.progress($("#grid"), false);

            if (!result.flag) {
                Notification({ title: $.i18n("label_thongbao"), message: $.i18n("label_coloixayra_khongtheloaddanhsachkhachhang") }, 'warning');
            }
        });
    }
    function openFormDetail(id) {
        $state.go('editkhachhang', { idkhachhang: id });
    }
    //event
    $scope.tinhthanhOnChange = function () {
        $scope.tinhthanhselect = this.tinhthanhselect;

        let idTinh = 0;
        if ($scope.tinhthanhselect != undefined)
            idTinh = ($scope.tinhthanhselect.iD_Tinh < 0) ? 0 : $scope.tinhthanhselect.iD_Tinh;

        ComboboxDataService.getQuanHuyen(idTinh).then(function (result) {
            $scope.quanhuyenData = result.data;
        });
    }
    $scope.quanhuyenOnChange = function () {
        $scope.quanhuyenselect = this.quanhuyenselect;
    }
    $scope.loaikhachhangOnChange = function () {
        $scope.loaikhachhangselect = this.loaikhachhangselect;
    }

    $scope.xemBaoCao = function () {
        var idtinh = 0;
        var idquan = 0;
        var idloaikhachhang = -1;
        if ($scope.tinhthanhselect != undefined)
            idtinh = ($scope.tinhthanhselect.iD_Tinh < 0) ? 0 : $scope.tinhthanhselect.iD_Tinh;
        if ($scope.quanhuyenselect != undefined)
            idquan = ($scope.quanhuyenselect.iD_Quan < 0) ? 0 : $scope.quanhuyenselect.iD_Quan;
        if ($scope.loaikhachhangselect != undefined)
            idloaikhachhang = ($scope.loaikhachhangselect.iD_LoaiKhachHang < 0) ? -1 : $scope.loaikhachhangselect.iD_LoaiKhachHang;

        loadgrid(idtinh, idquan, idloaikhachhang);
    }
    $scope.openformsetnguong = function (e) {
        let myGrid = $('#grid').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;

        $scope.idkhachhang = dataItem.iD_KhachHang;
        $scope.khachhang = dataItem.tenKhachHang == undefined ? '' : dataItem.tenKhachHang;
        $scope.nguong = dataItem.congNoChoPhep == null ? 0 : dataItem.congNoChoPhep;

        $scope.formsetnguong.center().open();
    }
    $scope.setnguong = function () {
        let flag = true;

        if ($scope.nguong <= 0) {
            flag = false;
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_nguongcongnokhongthenhohonhoacbangkhong') }, 'warning');
        }

        if (flag) {
            hanMucDataService.setnguongkhachhang($scope.idkhachhang, $scope.nguong).then(function (result) {
                if (result.flag) {
                    $scope.formsetnguong.close();
                    $scope.xemBaoCao();
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                }
                else
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
            });
        }
    }
    $scope.huysetnguong = function () {
        $scope.formsetnguong.close();
    }
    $scope.openDetailFromGrid = function (iD_KhachHang) {
        openFormDetail(iD_KhachHang);
    }

    $scope.xemvitrichuacotoado = function () {
        Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_khachhangchuacothongtinvitri') }, 'warning');
    }
    $scope.xemvitri = function (vido, kinhdo) {
        let url = 'https://www.google.com/maps/dir/' + vido.toString() + ',' + kinhdo.toString();
        window.open(url, '_blank');
    }

    init();

})