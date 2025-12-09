angular.module('app').controller('khoHangController', function ($rootScope, $scope, $location, Notification, ComboboxDataService, khoHangDataService, nhanVienDataService) {
    CreateSiteMap();

    let __idkho = 0;
    let __idkhoquyen = 0;
    let __idnhom = 0;
    let __arrnhanvienchon = [];
    let __arridnhanvienchon = [];

    function init() {
        getquyen();
        loadgrid();
        loadgriddetailmathang(-1);
        loadgriddetailnhanvien(-1);

        inittreeview();
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

    function listColumnsgrid() {
        let dataList = [];

        //dataList.push({ headerAttributes: { "class": "table-header-cell" }, selectable: true, width: "40px" });
        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({ field: "maKho", title: $.i18n('header_makho'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px" });
        dataList.push({ field: "tenKho", title: $.i18n('header_tenkho'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px" });
        dataList.push({ field: "diaChi", title: $.i18n('header_diachi'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid });
        dataList.push({
            field: "ngayTao", title: $.i18n('header_ngaytao'),
            template: function (dataItem) {
                //let d = new Date(dataItem.ngayTao);
                return kendo.htmlEncode(kendo.toString(dataItem.ngayTao, formatDateTime));
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "chiTiet", title: $.i18n('header_tacvu'),
            template: '<button ng-click="openphanquyenkhonhanvien()" class="btn btn-link btn-menubar" title ="' + $.i18n('label_phanquyenkhonhanvien') + '" ><i class="fas fa-user-edit fas-sm color-infor"></i></button> ',
            attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        });

        return dataList;
    }
    function loadgrid() {
        kendo.ui.progress($("#grid"), true);
        $scope.gridOptions = {
            sortable: true,
            height: function () {
                return 350;
            },
            dataBinding: function () {
                RecordNumber = (this.dataSource.page() - 1) * this.dataSource.pageSize();
            },
            resizable: true,
            editable: false,
            filterable: {
                mode: "row"
            },
            selectable: "row",
            pageable: $scope.lang == 'vi-vn' ? pageableShort_vi : pageableShort_en,
            change: onChange,
            columns: listColumnsgrid()
        };

        khoHangDataService.getlistkhohang().then(function (response) {
            $scope.gridData = {
                data: response.data,
                schema: {
                    model: {
                        id: 'iD_Kho',
                        fields: {
                            iD_Kho: {
                                type: "number"
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
        });
    }
    function onChange(arg) {
        let listid = []

        let grid = $("#grid").data("kendoGrid");
        grid.select().each(function () {
            let dataItem = grid.dataItem(this);
            listid.push({ iD_Kho: dataItem.iD_Kho });
        });

        if (listid.length == 1) {
            let iD_Kho = listid[0].iD_Kho;
            loadgriddetailmathang(iD_Kho);
            loadgriddetailnhanvien(iD_Kho);
        } else {
            loadgriddetailmathang(-1);
            loadgriddetailnhanvien(-1);
        }
    }

    function listColumnsgriddetailmathang() {
        let dataList = [];

        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({ field: "maHang", title: $.i18n('header_mahang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({ field: "tenHang", title: $.i18n('header_tenhang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({
            field: "tenDonVi", title: $.i18n('header_donvi'),
            attributes: { style: "text-align: center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "soLuongTon", title: $.i18n('header_tonkho'),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            attributes: { style: "text-align: center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });

        return dataList;
    }
    function loadgriddetailmathang(_idkho) {
        kendo.ui.progress($("#gridDetailMatHang"), false);
        $scope.gridDetailMatHangOptions = {
            sortable: true,
            height: function () {
                return 300;
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
            columns: listColumnsgriddetailmathang()
        };

        khoHangDataService.getlistmathangbyidkho(_idkho).then(function (response) {
            $scope.gridDetailMatHangData = {
                data: response.data,
                schema: {
                    model: {
                        id: 'idMatHang',
                        fields: {
                            idMatHang: {
                                type: "number"
                            }
                        }
                    }
                },
                pageSize: 20
            };

            kendo.ui.progress($("#gridDetailMatHang"), false);
        });
    }

    function listColumnsgriddetailnhanvien() {
        let dataList = [];

        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({ field: "tenDangNhap", title: $.i18n('header_tendangnhap'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({ field: "tenDayDu", title: $.i18n('header_tendaydu'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({
            field: "dienThoai", title: $.i18n('header_dienthoai'),
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "tenNhom", title: $.i18n('header_nhom'),
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });

        return dataList;
    }
    function loadgriddetailnhanvien(_idkho) {
        kendo.ui.progress($("#gridDetailNhanVien"), false);
        $scope.gridDetailNhanVienOptions = {
            sortable: true,
            height: function () {
                return 300;
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
            columns: listColumnsgriddetailnhanvien()
        };

        khoHangDataService.getlistnhanvienbyidkho(_idkho).then(function (response) {
            $scope.gridDetailNhanVienData = {
                data: response.data,
                schema: {
                    model: {
                        id: 'iD_NhanVien',
                        fields: {
                            iD_NhanVien: {
                                type: "number"
                            }
                        }
                    }
                },
                pageSize: 20
            };

            kendo.ui.progress($("#gridDetailNhanVien"), false);
        });
    }

    function validationOpenDetail(listRowsSelected) {
        let flag = true;
        let msg = "";
        if (listRowsSelected.length > 1) {
            msg = $.i18n('label_chuachonkhodethuchien');
            flag = false;
        } else if (listRowsSelected.length == 0) {
            flag = false;
            msg = $.i18n('label_chuachonkhodethuchien');
        }
        if (!flag) {
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n(msg) }, "error");
        }
        return flag;
    }
    function openFormDetail(_objkho) {
        $scope.idkho = __idkho;
        if (__idkho > 0)
            loadeditkhohangform(_objkho);
        else
            loadaddkhohangform();

        $scope.formdetail.center().open();
    }
    function loadeditkhohangform(_objkho) {
        $scope.maKho = _objkho.maKho;
        $scope.tenKho = _objkho.tenKho;
        $scope.diaChi = _objkho.diaChi;
        $scope.hoatdong = (_objkho.trangThai == 1) ? true : false;
    }
    function loadaddkhohangform() {
        $scope.maKho = '';
        $scope.tenKho = '';
        $scope.diaChi = '';
        $scope.hoatdong = true;
    }
    function validatethemsua() {
        let flag = true;
        let msg = '';

        if ($scope.maKho == '' || $scope.maKho == undefined) {
            flag = false;
            msg = $.i18n('label_makhokhongduocdetrong');
            $("#maKho").focus();
        }

        if (flag && ($scope.tenKho == '' || $scope.tenKho == undefined)) {
            flag = false;
            msg = $.i18n('label_tenkhokhongduocdetrong');
            $("#tenKho").focus();
        }

        if (!flag)
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n(msg) }, 'warning');

        return flag;
    }
    function openConfirm(message, acceptAction, cancelAction, data) {
        var scope = angular.element("#mainContentId").scope();
        $(" <div id='confirmDelete'></div>").appendTo("body").kendoDialog({
            width: "450px",
            closable: true,
            modal: true,
            title: $.i18n('label_xacnhan'),
            content: message,
            actions: [
                {
                    text: $.i18n('button_huy'), primary: false, action: function () {
                        if (cancelAction != null) {
                            scope[cancelAction](data);
                        }
                    }
                },
                {
                    text: $.i18n('button_dongy'), primary: true, action: function () {
                        scope[acceptAction](data);
                    }
                }
            ],
        })
    }

    function inittreeview() {
        let dataSource = new kendo.data.HierarchicalDataSource({
            data: [],
            schema: {
                model: {
                    children: "childs"
                }
            }
        })

        $("#treeview").kendoTreeView({
            dataSource: dataSource,
            dataTextField: "tenNhom",
            dataValueField: "iD_Nhom",
            select: onSelectNhom,
        });
    }
    function loadtreeView() {
        nhanVienDataService.getListNhomNhanVien().then(function (result) {
            setDataTreeview(result.data);
        });
    }
    function setDataTreeview(data) {
        let dataSource = new kendo.data.HierarchicalDataSource({
            data: data,
            schema: {
                model: {
                    children: "childs"
                }
            }
        })

        let tree = $("#treeview").data("kendoTreeView");
        tree.setDataSource(dataSource);
        tree.expand(".k-item");
        tree.select(".k-first");

        let selectedNode = tree.select();
        __idnhom = tree.dataItem(selectedNode).iD_Nhom;
        loadgridnhanvien();
    }
    function onSelectNhom(e) {
        __idnhom = $("#treeview").getKendoTreeView().dataItem(e.node).iD_Nhom;
        loadgridnhanvien();
    }

    function listColumnsgridnhanvien() {
        var dataList = [];

        dataList.push({
            field: "tacvu", title: $.i18n('header_tacvu'),
            template: '<button ng-click="themnhanvienquyen()" class="btn btn-link btn-menubar" title ="' + $.i18n('button_themnhanvienquyen') + '" ><i class="fas fa-plus-circle fas-sm color-infor"></i></button> ',
            attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "70px"
        });
        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({
            field: "tenDangNhap", title: $.i18n('header_tendangnhap'),
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px"
        });
        dataList.push({ field: "tenDayDu", title: $.i18n('header_tendaydu'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "175px" });
        dataList.push({ field: "dienThoai", title: $.i18n('header_sodienthoai'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px" });
        dataList.push({ field: "phienBan", title: $.i18n('header_phienban'), attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px" });
        dataList.push({ field: "tenNhom", title: $.i18n('header_tennhom'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px" });

        return dataList;
    }
    function loadgridnhanvien() {
        kendo.ui.progress($("#gridnhanvien"), true);
        $scope.gridnhanvienOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height();
                return heightGrid - 350;
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
            columns: listColumnsgridnhanvien()
        };

        nhanVienDataService.getlist(__idnhom).then(function (result) {
            let arr_nhanvienall = result.data;
            let arr_nhanvienchuachon = arr_nhanvienall.filter((item) => {
                return (__arridnhanvienchon.indexOf(item.idnv) == -1)
            })

            $scope.gridnhanvienData = {
                data: arr_nhanvienchuachon,
                schema: {
                    model: {
                        id: 'idnv',
                        fields: {
                            idnv: {
                                type: "number"
                            },
                            thoiGianCapNhat: {
                                type: "date"
                            }
                        }
                    }
                },
                pageSize: 20
            };
            kendo.ui.progress($("#gridnhanvien"), false);
        });
    }

    function listColumnsgridchon() {
        var dataList = [];

        dataList.push({
            field: "tacvu", title: $.i18n('header_tacvu'),
            template: '<button ng-click="xoanhanvienquyen()" class="btn btn-link btn-menubar" title ="' + $.i18n('button_xoa') + '" ><i class="fas fa-trash fas-sm color-danger"></i></button> ',
            attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "70px"
        });
        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({
            field: "tenDangNhap", title: $.i18n('header_tendangnhap'),
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px"
        });
        dataList.push({ field: "tenDayDu", title: $.i18n('header_tendaydu'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "175px" });
        dataList.push({ field: "dienThoai", title: $.i18n('header_sodienthoai'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px" });
        dataList.push({ field: "phienBan", title: $.i18n('header_phienban'), attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px" });
        dataList.push({ field: "tenNhom", title: $.i18n('header_tennhom'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px" });

        return dataList;
    }
    function loadgridchon() {
        kendo.ui.progress($("#gridchon"), true);
        $scope.gridchonOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height();
                return 210;
            },
            dataBinding: function () {
                RecordNumber = (this.dataSource.page() - 1) * this.dataSource.pageSize();
            },
            resizable: true,
            editable: false,
            filterable: false,
            pageable: $scope.lang == 'vi-vn' ? pageableShort_vi : pageableShort_en,
            columns: listColumnsgridchon()
        };

        khoHangDataService.getlistnhanvienbyidkho(__idkhoquyen).then(function (result) {
            __arrnhanvienchon = result.data;
            __arridnhanvienchon = __arrnhanvienchon.map((ar, index, arr) => {
                return ar.idnv
            })

            $scope.gridchonData = {
                data: __arrnhanvienchon,
                schema: {
                    model: {
                        id: 'idnv',
                        fields: {
                            idnv: {
                                type: "number"
                            }
                        }
                    }
                },
                pageSize: 20
            };
            kendo.ui.progress($("#gridchon"), false);

            loadtreeView();
        });
    }

    //event
    $scope.addkho = function () {
        __idkho = 0;
        openFormDetail(__idkho);
    }
    $scope.editkho = function () {
        let listRowsSelected = commonGetRowSelected("#grid");
        if (validationOpenDetail(listRowsSelected)) {
            __idkho = listRowsSelected[0].iD_Kho;
            openFormDetail(listRowsSelected[0]);
        }
    }
    $scope.deletekho = function () {
        let listRowsSelected = commonGetRowSelected("#grid");
        if (validationOpenDetail(listRowsSelected)) {
            openConfirm($.i18n('label_bancochacchanmuonxoakhong'), 'apDungXoaHang', null, listRowsSelected[0].iD_Kho);
        }
    }
    $scope.apDungXoaHang = function (data) {
        khoHangDataService.deletekho(data).then(function (result) {
            if (result.flag) {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                loadgrid();
            }
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
        });
    }

    $scope.luukho = function () {
        if (validatethemsua()) {
            commonOpenLoadingText("#btn_luukho");

            let data = {
                ID_QLLH: 0,
                ID_Kho: __idkho,
                MaKho: $scope.maKho,
                TenKho: $scope.tenKho,
                DiaChi: $scope.diaChi,
                TrangThai: ($scope.hoatdong) ? 1 : 0,
            }
            khoHangDataService.themsuakho(data).then(function (result) {
                if (result.flag) {
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                    loadgrid();
                    $scope.formdetail.center().close();
                }
                else
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');

                commonCloseLoadingText("#btn_luukho");
            });
        }
    }
    $scope.huyluukho = function () {
        $scope.formdetail.center().close();
    }

    $scope.openphanquyenkhonhanvien = function () {
        let myGrid = $('#grid').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;

        __idkhoquyen = dataItem.iD_Kho;
        __idnhom = 0;
        __arrnhanvienchon = [];
        __arridnhanvienchon = [];

        loadgridchon();

        $scope.formdetailphanquyen.center().maximize().open();
    }
    $scope.themnhanvienquyen = function (e) {
        let myGrid = $('#gridnhanvien').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;

        let myGridAdd = $("#gridchon").data("kendoGrid");
        myGridAdd.dataSource.add(dataItem);

        myGrid.dataSource.remove(dataItem);

        __arrnhanvienchon.push(dataItem);
        __arridnhanvienchon.push(dataItem.idnv);
    }
    $scope.xoanhanvienquyen = function (e) {
        let myGrid = $('#gridchon').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;

        if (dataItem.iD_Nhom == __idnhom || __idnhom <= 0) {
            let myGridAdd = $("#gridnhanvien").data("kendoGrid");
            myGridAdd.dataSource.add(dataItem);
        }

        myGrid.dataSource.remove(dataItem);

        __arrnhanvienchon = __arrnhanvienchon.filter(item => item.idnv != dataItem.idnv);
        __arridnhanvienchon = __arridnhanvienchon.filter(item => item != dataItem.idnv);
    }

    $scope.luuphanquyen = function () {
        commonOpenLoadingText("#btn_luuphanquyen");

        khoHangDataService.phanquyennhanvienkho(__arridnhanvienchon, __idkhoquyen).then(function (result) {
            if (result.flag) {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                loadgriddetailnhanvien(__idkhoquyen);
                $scope.formdetailphanquyen.center().close();
            }
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');

            commonCloseLoadingText("#btn_luuphanquyen");
        });
    }
    $scope.huyluuphanquyen = function () {
        $scope.formdetailphanquyen.center().close();
    }

    init();

})