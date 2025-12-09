angular.module('app').controller('lichSuVaoRaDiemController', function ($rootScope, $scope, $location, $timeout, $state, $stateParams, Notification, ComboboxDataService, NhanVienDetail) {
    CreateSiteMap();

    let param_idcheckin = 0;
    let param_idkhachhang = 0;
    let param_idnhanvien = 0;
    let param_from = '';
    let param_to = '';

    let idkhachhang = 0;
    let idcheckin = 0;

    function init() {
        getquyen();
        initparam();
        initdate();
        initcombo();
        loadgrid(0, param_idnhanvien, -1);
    }
    function getquyen() {
        let path = $location.path();
        let url = path.replace('/', '')
        ComboboxDataService.getquyen(url).then(function (result) {
            $scope.permission = result.data;
            if ($scope.permission.iD_ChucNang <= 0) {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_khongcoquyentruycapchucnang') }, "error");
                $location.path('/home')
            }
        });
    }
    function initparam() {
        param_idcheckin = ($stateParams.idcheckin == undefined) ? 0 : $stateParams.idcheckin;
        param_idkhachhang = ($stateParams.idkhachhang == undefined) ? 0 : $stateParams.idkhachhang;
        param_idnhanvien = ($stateParams.idnhanvien == undefined) ? 0 : $stateParams.idnhanvien;
        param_from = ($stateParams.from == undefined) ? '' : $stateParams.from;
        param_to = ($stateParams.to == undefined) ? '' : $stateParams.to;
    }
    function initdate() {
        var dateNow = new Date();

        if (param_from != '') {
            $scope.obj_TuNgay = new Date(param_from);
        } else
            $scope.obj_TuNgay = new Date(dateNow.setHours(0, 0, 0));

        if (param_to != '') {
            $scope.obj_DenNgay = new Date(param_to);
        } else
            $scope.obj_DenNgay = new Date(dateNow.setHours(23, 59, 59));

        $scope.maxDate = $scope.obj_DenNgay;
        $scope.minDate = $scope.obj_TuNgay;
    }
    function initcombo() {
        ComboboxDataService.getDataNhomNhanVien().then(function (result) {
            $scope.nhomnhanvienData = result.data;
        });
        ComboboxDataService.getLoaiKhachHang().then(function (result) {
            $scope.loaikhachhangData = result.data;
        });
        ComboboxDataService.getlistnhanvienbymultiidnhom({ IDNhom: -2 }).then(function (result) {
            if (result.flag) {
                $scope.nhanvienData = result.data;
                if (param_idnhanvien > 0) {
                    $timeout(function () { $("#nhanvien").data("kendoComboBox").value(param_idnhanvien); }, 100);
                }
            }
            else
                $scope.nhanvienData = [];
        });
    }

    function listColumnsgrid() {
        let dataList = [];

        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({ field: "tenNhanVien", title: $.i18n('header_tennhanvien'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({ field: "tenKhachHang", title: $.i18n('header_tenkhachhang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({ field: "dienThoai", title: $.i18n('header_sodienthoai'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px" });
        dataList.push({ field: "tenTuyen", title: $.i18n('header_tuyen'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({
            field: "diaChiKhachHang", title: $.i18n('header_diachikhachhang'),
            template: function (dataItem) {
                if (dataItem.kinhDo == 0 || dataItem.kinhDo == null || dataItem.kinhDo == '')
                    return '<button ng-click="xemvitrichuacotoado()" class="btn btn-link btn-menubar" title ="' + $.i18n('label_vitrikhachhang')+'" >' + kendo.htmlEncode(dataItem.diaChi) + '</button>';
                else
                    return '<button ng-click="xemvitri(' + kendo.htmlEncode(dataItem.viDo) + ',' + kendo.htmlEncode(dataItem.kinhDo) + ')" class="btn btn-link btn-menubar" title ="' + $.i18n('label_vitrikhachhang') +'" >' + kendo.htmlEncode(dataItem.diaChiKhachHang) + '</button>';
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "300px"
        });
        dataList.push({
            field: "diaChiVaoDiem", title: $.i18n('header_diachivaodiem'),
            template: function (dataItem) {
                if (dataItem.kinhDo == 0 || dataItem.kinhDo == null || dataItem.kinhDo == '')
                    return '<button ng-click="xemvitrichuacotoado()" class="btn btn-link btn-menubar" title ="' + $.i18n('label_vitrikhachhang') +'" >' + kendo.htmlEncode(dataItem.diaChiVaoDiem) + '</button>';
                else
                    return '<button ng-click="xemvitri(' + kendo.htmlEncode(dataItem.viDo) + ',' + kendo.htmlEncode(dataItem.kinhDo) + ')" class="btn btn-link btn-menubar" title ="' + $.i18n('label_vitrikhachhang') +'" >' + kendo.htmlEncode(dataItem.diaChiVaoDiem) + '</button>';
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "400px"
        });
        dataList.push({
            field: "checkInDay", title: $.i18n('header_ngayvaodiem'),
            template: function (dataItem) {
                return (dataItem.checkInDay == null) ? '' : dataItem.checkInDay;
            },
            attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px"
        });
        dataList.push({
            field: "checkInTime", title: $.i18n('header_giovaodiem'),
            template: function (dataItem) {
                return (dataItem.checkInTime == null) ? '' : dataItem.checkInTime;
            },
            attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px"
        });
        dataList.push({
            field: "diaChiRaDiem", title: $.i18n('header_diachiradiem'),
            template: function (dataItem) {
                if (dataItem.kinhDoRaDiem == 0 || dataItem.kinhDoRaDiem == null || dataItem.kinhDoRaDiem == '')
                    return '<button ng-click="xemvitrichuacotoado()" class="btn btn-link btn-menubar" title ="' + $.i18n('label_vitrikhachhang') +'" >' + kendo.htmlEncode(dataItem.diaChiRaDiem) + '</button>';
                else
                    return '<button ng-click="xemvitri(' + kendo.htmlEncode(dataItem.viDoRaDiem) + ',' + kendo.htmlEncode(dataItem.kinhDoRaDiem) + ')" class="btn btn-link btn-menubar" title ="' + $.i18n('label_vitrikhachhang') +'" >' + kendo.htmlEncode(dataItem.diaChiRaDiem) + '</button>';
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "400px"
        });
        dataList.push({
            field: "checkOutDay", title: $.i18n('header_ngayradiem'),
            template: function (dataItem) {
                return (dataItem.checkOutDay == null) ? '' : dataItem.checkOutDay;
            },
            attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px"
        });
        dataList.push({
            field: "checkOutTime", title: $.i18n('header_gioradiem'),
            template: function (dataItem) {
                return (dataItem.checkOutTime == null) ? '' : dataItem.checkOutTime;
            },
            attributes: { style: "text-align: center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px"
        });
        dataList.push({ field: "khoangCach", title: $.i18n('header_khoangcach'), attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px" });
        dataList.push({
            field: "thoiGianTaiDiem", title: $.i18n('header_thoigiantaidiem'),
            template: function (dataItem) {
                return (dataItem.thoiGianTaiDiem == null) ? '' : dataItem.thoiGianTaiDiem;
            },
            attributes: { style: "text-align: center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px"
        });
        dataList.push({ field: "soDonHang", title: $.i18n('header_sodontaidiem'), attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px" });
        dataList.push({
            field: "checkList", title: $.i18n('header_congviecdalam'),
            template: function (dataItem) {
                return "<a href='' class='color-primary' ng-click='openDetailCheckList()'>" + kendo.htmlEncode(dataItem.checkList) + "</a>";
            }, attributes: { style: "text-align: center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px"
        });
        dataList.push({
            field: "soAnhChup", title: $.i18n('header_soluonganh'),
            template: function (dataItem) {
                return "<a href='' class='color-primary' ng-click='openAlbumImage()'>" + kendo.htmlEncode(dataItem.soAnhChup) + "</a>";
            }, attributes: { style: "text-align: center" },
            attributes: { "class": "table-cell", style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px"
        });
        dataList.push({ field: "ghiChu", title: $.i18n('header_ghichu'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "210px" });

        return dataList;
    }
    function loadgrid(idnhom, idnhanvien, id_LoaiKhachHang) {
        kendo.ui.progress($("#grid"), true);
        commonOpenLoadingText("#dataLSRVD");
        $scope.gridOptions = {
            sortable: true,
            height: function () {
                let heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".content-filter").height() + $(".toolbarmenu").height());
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
            columns: listColumnsgrid(),
        };

        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        NhanVienDetail.getLichSuVaoRaDiem(idnhom, idnhanvien, id_LoaiKhachHang, fromdate, todate, param_idkhachhang, param_idcheckin).then(function (result) {
            $scope.gridData = {
                data: result.data.data,
                schema: {
                    model: {
                        fields: {
                            iD_CheckIn: {
                                type: "number"
                            }
                        }
                    }
                },
                pageSize: 20,
            };
            $scope.tongvaodiem = result.data.vaoDiem;
            $scope.tongradiem = result.data.raDiem;

            kendo.ui.progress($("#grid"), false);
            commonCloseLoadingText("#dataLSRVD")
        });

    }

    function listColumnsgrid_CheckList() {
        let dataList = [];

        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({ field: "tenCheckList", title: $.i18n('header_tencongviec'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px" });
        dataList.push({
            field: "daCheck", title: $.i18n('header_dathuchien'),
            template: function (dataItem) {
                if (dataItem.daCheck == 0)
                    return '<button class="btn btn-link btn-menubar" ><i class="far fa-square fas-sm color-primary"></i></button> ';
                else
                    return '<button class="btn btn-link btn-menubar" ><i class="far fa-check-square fas-sm color-primary"></i></button> ';
            },
            attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "80px"
        });
        dataList.push({
            field: "trangThai", title: $.i18n('header_danhgia'),
            template: function (dataItem) {
                if (dataItem.trangThai == 0)
                    return '<button class="btn btn-link btn-menubar" ><i class="fas fa-frown fas-sm color-warning"></i></button> ';
                else
                    return '<button class="btn btn-link btn-menubar" ><i class="fas fa-smile fas-sm color-success"></i></button> ';
            },
            attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "80px"
        });
        dataList.push({ field: "chiTiet", title: $.i18n('header_noidung'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px" });

        return dataList;
    }
    function loadgrid_CheckList(idkhachhang, idcheckin) {
        $scope.gridOptions_CheckList = {
            sortable: true,
            height: function () {
                return 400;
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
            columns: listColumnsgrid_CheckList(),
        };
        NhanVienDetail.getCheckList(idkhachhang, idcheckin).then(function (result) {
            $scope.gridDataCheckList = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                        }
                    }
                },
                pageSize: 20,
            };
        });
    }

    //event
    $scope.xemBaoCao = function () {
        var idnhanvien = 0;
        var idnhom = 0;
        var idloaikhachhang = -1;
        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        if ($scope.nhomnhanvienselect != undefined)
            idnhom = ($scope.nhomnhanvienselect.iD_Nhom < 0) ? 0 : $scope.nhomnhanvienselect.iD_Nhom;
        if ($scope.loaikhachhangselect != undefined)
            idloaikhachhang = ($scope.loaikhachhangselect.iD_LoaiKhachHang <= 0) ? -1 : $scope.loaikhachhangselect.iD_LoaiKhachHang;

        loadgrid(idnhom, idnhanvien, idloaikhachhang);
    }
    $scope.XuatExcel = function () {
        commonOpenLoadingText("#dataLSRVD_taifile");
        var idnhanvien = 0;
        var idnhom = 0;
        var idloaikhachhang = -1;
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        if ($scope.nhomnhanvienselect != undefined)
            idnhom = ($scope.nhomnhanvienselect.iD_Nhom < 0) ? 0 : $scope.nhomnhanvienselect.iD_Nhom;
        if ($scope.loaikhachhangselect != undefined)
            idloaikhachhang = ($scope.loaikhachhangselect.iD_LoaiKhachHang <= 0) ? -1 : $scope.loaikhachhangselect.iD_LoaiKhachHang

        NhanVienDetail.getExcelLichSuVaoRaDiem(idnhom, idnhanvien, idloaikhachhang, fromdate, todate, param_idkhachhang, param_idcheckin).then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayravuilongloadlaitrang') }, 'warning');

            commonCloseLoadingText("#dataLSRVD_taifile")
        });
    }
    $scope.openDetailCheckList = function () {
        let myGrid = $('#grid').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;

        idkhachhang = dataItem.iD_KhachHang;
        idcheckin = dataItem.iD_CheckIn;
        $scope.formdetail.center().open();
        loadgrid_CheckList(idkhachhang, idcheckin);
    }
    $scope.openAlbumImage = function () {
        let myGrid = $('#grid').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;

        idcheckin = dataItem.iD_CheckIn;

        var url = $state.href('album', { idcheckin: idcheckin });
        window.open(url, '_blank');
    }

    $scope.xemvitrichuacotoado = function () {
        Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_khachhangchuacothongtinvitri') }, 'warning');
    }
    $scope.xemvitri = function (vido, kinhdo) {
        let url = 'https://www.google.com/maps/dir/' + vido.toString() + ',' + kinhdo.toString();
        window.open(url, '_blank');
    }

    $scope.nhanvienOnChange = function () {
        $scope.nhanvienselect = this.nhanvienselect;
    }
    $scope.nhomnhanvienOnChange = function () {
        $scope.nhomnhanvienselect = this.nhomnhanvienselect;
        idnhom = $scope.nhomnhanvienselect.iD_Nhom;
        ComboboxDataService.getlistnhanvienbymultiidnhom({ IDNhom: idnhom }).then(function (result) {
            $("#nhanvien").data("kendoComboBox").value('');
            if (result.flag) {
                $scope.nhanvienData = result.data;
            }
            else
                $scope.nhanvienData = [];
        });
    };
    $scope.loaikhachhangOnChange = function () {
        $scope.loaikhachhangselect = this.loaikhachhangselect;
    }
    $scope.fromDateChanged = function () {
        $scope.minDate = $scope.obj_TuNgay;
    }
    $scope.toDateChanged = function () {
        $scope.maxDate = $scope.obj_DenNgay;
    }

    init();

})
