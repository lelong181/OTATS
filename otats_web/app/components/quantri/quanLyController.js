angular.module('app').controller('quanLyController', function ($location, $timeout, $rootScope, $scope, $state, Notification, ComboboxDataService, quanTriDataService) {
    CreateSiteMap();

    let __idtaikhoan = 0;
    $scope.idNhom = 0;

    function init() {
        inittreeview();
        initcombobox();
        loadgrid();
        inittreephongban();
        getquyen();

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
    function initcombobox() {
        quanTriDataService.getListNhomNhanVien(0).then(function (result) {
            $scope.nhomchaData = result.data;
        });
    }
    function loadcombobox() {
        ComboboxDataService.getDataNhomNhanVien().then(function (result) {
            let arr = result.data;
            let small_arr = arr.filter((item) => {
                return (item.iD_Nhom > 0)
            })

            $scope.nhomchaData = small_arr;

            $timeout(function () {
                if ($scope.objectNhom.iD_PARENT > 0)
                    $("#nhomcha").data("kendoComboBox").value($scope.objectNhom.iD_PARENT);
                else
                    $("#nhomcha").data("kendoComboBox").value("");
            }, 10);
        });
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

        loadtreeView();
    }
    function loadtreeView() {
        quanTriDataService.getListNhomNhanVien(0).then(function (result) {
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
        $scope.idNhom = tree.dataItem(selectedNode).iD_Nhom;
        loadgrid();
    }
    function onSelectNhom(e) {
        $scope.idNhom = $("#treeview").getKendoTreeView().dataItem(e.node).iD_Nhom;
        loadgrid();
    }

    function inittreephongban() {
        var treeData = new kendo.data.HierarchicalDataSource({
            data: [],
            schema: {
                model: {
                    id: "id",
                    hasChildren: "childs",
                    children: "childs"
                }
            }
        })

        $scope.options = {
            dataSource: treeData,
            dragAndDrop: true,
            loadOnDemand: false,
            checkboxes: {
                checkChildren: false,
                template: "<input id='checkbox_#:item.uid#' ng-model='dataItem.isChecked' type='checkbox' class='k-checkbox'/><label for='checkbox_#:item.uid#' class='k-checkbox-label'></label>"
            },
            template: "#: item.tenNhom #"
        };

        loadTreePhongBan();
    }
    function loadTreePhongBan() {
        quanTriDataService.getListNhomNhanVienPhanQuyen(__idtaikhoan).then(function (result) {
            let ar = result.data;
            let data = ar.filter(obj => obj.iD_Nhom > 0);

            setDataTreePhongBan(data);
        });
    }
    function setDataTreePhongBan(data) {
        var dataSource = new kendo.data.HierarchicalDataSource({
            data: data,
            schema: {
                model: {
                    id: "iD_Nhom",
                    hasChildren: "childs",
                    children: "childs"
                }
            }
        })

        var treeview = $("#tree").data("kendoTreeView");
        treeview.enable(false);
        treeview.setDataSource(dataSource);
        treeview.expand(".k-item:first");
    }

    function getcheckedNodeIds(nodes, checkedNodes, parentid) {
        for (let i = 0; i < nodes.length; i++) {

            if (nodes[i].isChecked != undefined) {
                if (nodes[i].isChecked) {
                    checkedNodes.push(nodes[i].iD_Nhom);
                }
            }

            if (nodes[i].hasChildren) {
                getcheckedNodeIds(nodes[i].children.view(), checkedNodes, nodes[i].iD_Nhom);
            }
        }
    }

    function listColumnsgrid() {
        let dataList = [];

        dataList.push({ headerAttributes: { "class": "table-header-cell" }, selectable: true, width: "40px" });
        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({ field: "username", title: $.i18n('header_tendangnhap'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({ field: "tenAdmin", title: $.i18n('header_hoten'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px" });
        dataList.push({
            field: "danhSachNhom", title: $.i18n('header_danhsachnhom'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "500px"
        });

        return dataList;
    }
    function loadgrid() {
        kendo.ui.progress($("#grid"), true);
        $scope.gridOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".toolbarmenu").height());
                return heightGrid - 50;
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
        quanTriDataService.getlist($scope.idNhom).then(function (result) {
            $scope.gridData = {
                data: result.data,
                schema: {
                    model: {
                        id: 'idnhom',
                        fields: {
                            iD_Hang: {
                                type: "number"
                            }
                        }
                    }
                },
                pageSize: 20
            };
            kendo.ui.progress($("#grid"), false);
        });
    }

    function openFormNhom(idparent, id, code, name) {
        if (id < 0)
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_khongthechinhsuanhommacdinh') }, 'warning');
        else {
            $scope.formnhom.center().open();
            $scope.objectNhom = {
                iD_Nhom: id,
                iD_PARENT: idparent,
                maNhom: code,
                tenNhom: name
            }

            loadcombobox();
        }

    }

    function validatethemsuanhom() {
        let flag = true;
        let msg = '';

        if ($scope.nhomchaselect != undefined)
            $scope.objectNhom.iD_PARENT = ($scope.nhomchaselect.iD_Nhom < 0) ? 0 : $scope.nhomchaselect.iD_Nhom;

        if ($scope.objectNhom.maNhom == '' || $scope.objectNhom.maNhom == undefined) {
            flag = false;
            msg = 'label_manhomkhongduocdetrong';
            $("#maNhom").focus();
        }

        if (flag && ($scope.objectNhom.tenNhom == '' || $scope.objectNhom.tenNhom == undefined)) {
            flag = false;
            msg = 'label_tennhomkhongduocdetrong';
            $("#tenNhom").focus();
        }

        if (flag && $scope.objectNhom.iD_Nhom > 0 && ($scope.objectNhom.iD_PARENT == $scope.objectNhom.iD_Nhom)) {
            flag = false;
            msg = 'label_khongthechonnhomchalachinhno';
        }

        if (!flag)
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n(msg) }, 'warning');

        return flag;
    }

    function validationOpenDetail(listRowsSelected) {
        let flag = true;
        let msg = "";
        if (listRowsSelected.length > 1) {
            msg = "label_canchonmottaikhoandethuchien";
            flag = false;
        } else if (listRowsSelected.length == 0) {
            flag = false;
            msg = "label_chuachontaikhoandethuchien";
        }
        if (!flag) {
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n(msg) }, "error");
        }
        return flag;
    }
    function openFormDetail(id) {
        __idtaikhoan = id;
        $scope.id = id;
        $scope.formdetail.center().open();
        if (id > 0)
            loadedittaikhoanquantri(id);
        else
            loadaddtaikhoanquantri();
    }
    function loadedittaikhoanquantri(id) {
        $scope.isEdit = true;
        quanTriDataService.getbyid(id).then(function (result) {
            $scope.objectTaiKhoan = {
                ID_QuanLy: id,
                ID_Cha: result.data.iD_Cha,
                Level: result.data.level,
                TenDangNhap: result.data.username,
                TenDayDu: result.data.tenAdmin,
                Email: result.data.email,
                Phone: result.data.phone,
                MatKhau: "",
                DanhSachNhom: result.data.danhSachNhom,
                refLink: "https://tourshopping.vn/#!/home/" + result.data.username
            };
            $("#qrcode").html("");
            var QR_CODEX = new QRCode("qrcode", {
                width: 160,
                height: 160,
                colorDark: "#000000",
                colorLight: "#ffffff",
                correctLevel: QRCode.CorrectLevel.H,
            });
            QR_CODEX.makeCode($scope.objectTaiKhoan.refLink);

            if (result.data.danhSachNhom == '' || result.data.danhSachNhom == '0') {
                $scope.isActivation = true;
            }
        });
        loadTreePhongBan();
    }
    function loadaddtaikhoanquantri() {
        $scope.isEdit = false;
        $scope.isActivation = true;
        $scope.objectTaiKhoan = {
            ID_QuanLy: 0,
            ID_Cha: 0,
            Level: 0,
            TenDangNhap: "",
            TenDayDu: "",
            MatKhau: "",
            Email: "",
            Phone: "",
            DanhSachNhom: [],
        }

        loadTreePhongBan();

    }

    function validatethemsua() {
        let flag = true;
        let msg = '';

        if ($scope.objectTaiKhoan.TenDayDu == '' || $scope.objectTaiKhoan.TenDayDu == undefined) {
            flag = false;
            msg = 'label_tendaydukhongduocdetrong';
            $("#TenDayDu").focus();
        }


        if (flag && ($scope.objectTaiKhoan.TenDangNhap == '' || $scope.objectTaiKhoan.TenDangNhap == undefined)) {
            flag = false;
            msg = 'label_tendangnhapkhongduocdetrong';
            $("#TenDangNhap").focus();
        }
        if (!$scope.isEdit) {
            if (flag && ($scope.objectTaiKhoan.MatKhau == '' || $scope.objectTaiKhoan.MatKhau == undefined)) {
                flag = false;
                msg = 'label_batbuocnhapmatkhau';
                $("#MatKhau").focus();
            }

            if (flag && ($scope.objectTaiKhoan.MK_replay == '' || $scope.objectTaiKhoan.MK_replay !== $scope.objectTaiKhoan.MatKhau)) {
                flag = false;
                msg = 'label_nhaplaimatkhaukhongdung';
                $("#MK_replay").focus();
            }

            if (flag && ($scope.objectTaiKhoan.Email == '' || $scope.objectTaiKhoan.Email == undefined)) {
                flag = false;
                msg = 'label_emailkhongduocdetrong';
                $("#Email").focus();
            }

            if (flag && ($scope.objectTaiKhoan.Phone == '' || $scope.objectTaiKhoan.Phone == undefined)) {
                flag = false;
                msg = 'label_sodienthoaikhongduocdetrong';
                $("#Phone").focus();
            }
        }
        if (flag && $scope.objectTaiKhoan.DanhSachNhom.length == 0) {
            flag = false;
            msg = 'label_chuachonnhomquanly';
        }


        if (!flag)
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n(msg) }, 'warning');

        return flag;
    }

    function openFormResetPass(_row) {
        if (_row.iD_QuanLy > 0) {
            $scope.objectpass = {
                username: _row.username,
                iD_QuanLy: _row.iD_QuanLy,
                pass: '',
                repass: ''
            }
            $scope.formresetpass.center().open();
        } else {
            Notification({ title: $.i18n('label_thongbao'), message: 'label_loinhanvienvuilongchonlai' }, "error");
        }
    }
    function validateresetpass() {
        let flag = true;
        let msg = '';

        if ($scope.objectpass.pass == '' || $scope.objectpass.pass == undefined) {
            flag = false;
            msg = 'label_matkhaukhongduocdetrong';
            $("#pass").focus();
        }

        if (flag && ($scope.objectpass.repass == '' || $scope.objectpass.repass == undefined)) {
            flag = false;
            msg = 'label_xacnhanmatkhaukhongduocdetrong';
            $("#repass").focus();
        }

        if (flag && $scope.objectpass.pass.length < 8) {
            flag = false;
            msg = 'label_matkhaucododaiitnhat8kytu';
            $("#pass").focus();
        }

        if (flag && $scope.objectpass.pass != $scope.objectpass.repass) {
            flag = false;
            msg = 'label_xacnhanmatkhaukhongkhop';
            $("#repass").focus();
        }

        if (!flag)
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n(msg) }, 'warning');

        return flag;
    }
    //event
    $scope.luuTaiKhoan = function () {
        let nodes = $scope.tree.dataSource.view();
        let checkednodes = [];
        getcheckedNodeIds(nodes, checkednodes);
        $scope.objectTaiKhoan.DanhSachNhom = checkednodes;

        if (validatethemsua()) {
            commonOpenLoadingText("#btn_luutaikhoan");
            if ($scope.objectTaiKhoan.ID_QuanLy <= 0) {
                quanTriDataService.themquanly($scope.objectTaiKhoan).then(function (result) {
                    if (result.flag) {
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                        loadgrid();
                        $scope.formdetail.center().close();
                    }
                    else
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
                    commonCloseLoadingText("#btn_luutaikhoan");
                });
            } else {
                quanTriDataService.suaquanly($scope.objectTaiKhoan).then(function (result) {
                    if (result.flag) {
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                        loadgrid();
                        $scope.formdetail.center().close();
                    }
                    else
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
                    commonCloseLoadingText("#btn_luutaikhoan");
                });
            }
        }

    }
    $scope.huyluuTaiKhoan = function () {
        $scope.formdetail.center().close();
    }

    $scope.addTaiKhoan = function () {
        __idtaikhoan = 0;
        openFormDetail(0);
    }
    $scope.editTaiKhoan = function () {
        let listRowsSelected = commonGetRowSelected("#grid");
        if (validationOpenDetail(listRowsSelected)) {
            openFormDetail(listRowsSelected[0].iD_QuanLy);
        }
    }
    $scope.deleteTaiKhoan = function () {
        let listRowsSelected = commonGetRowSelected("#grid");
        if (validationOpenDetail(listRowsSelected)) {
            openConfirm($.i18n("label_xoataikhoanquanlysedongthoixoacacquanlycapduoibancochacchanxoa"), 'apDungXoaTK', null, listRowsSelected[0].iD_QuanLy);
        }
    }
    $scope.apDungXoaTK = function (data) {
        quanTriDataService.xoaquanly(data).then(function (result) {
            if (result.flag) {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                loadgrid();
            }
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
        });
    }

    $scope.nhomchaOnChange = function () {
        $scope.nhomchaselect = this.nhomchaselect;
    }

    $scope.addnhom = function () {
        openFormNhom($scope.idNhom, 0, '', '');
    }
    $scope.editnhom = function () {
        let tree = $("#treeview").data("kendoTreeView");
        let selectedNode = tree.select();
        let dataItem = tree.dataItem(selectedNode);
        openFormNhom(dataItem.iD_PARENT, dataItem.iD_Nhom, dataItem.maNhom, dataItem.tenNhom);
    }
    $scope.deletenhom = function () {
        if ($scope.idNhom <= 0)
            Notification({ title: $.i18n('label_thongbao'), message: 'label_khongthexoanhommacdinh' }, 'warning');
        else {
            openConfirm($.i18n("label_bancochacchanxoanhomnhanvien")
                , 'apDungXoaNhom', null, $scope.idNhom);
        }
    }
    $scope.apDungXoaNhom = function (data) {
        quanTriDataService.delnhomnhanvien(data).then(function (result) {
            if (result.flag) {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                loadtreeView();
            }
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
        });
    }

    $scope.luuNhomNhanVien = function () {
        if (validatethemsuanhom()) {
            if ($scope.objectNhom.iD_Nhom > 0) {
                commonOpenLoadingText("#btn_luunhomnhanvien");
                quanTriDataService.saveeditnhomnhanvien($scope.objectNhom).then(function (result) {
                    if (result.flag) {
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                        loadtreeView();
                        $scope.formnhom.center().close();
                    }
                    else
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');

                    commonCloseLoadingText("#btn_luunhomnhanvien");
                });
            }
            else {
                commonOpenLoadingText("#btn_luunhomnhanvien");
                quanTriDataService.saveinsertnhomnhanvien($scope.objectNhom).then(function (result) {
                    if (result.flag) {
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                        loadtreeView();
                        $scope.formnhom.center().close();
                    }
                    else
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');

                    commonCloseLoadingText("#btn_luunhomnhanvien");
                });
            }
        }
    }
    $scope.huyLuuNhomNhanVien = function () {
        $scope.formnhom.center().close();
    }

    $scope.datLaiMatKhau = function () {
        let listRowsSelected = commonGetRowSelected("#grid");
        if (validationOpenDetail(listRowsSelected)) {
            openFormResetPass(listRowsSelected[0]);
        }
    }
    $scope.luuMatKhau = function () {
        if (validateresetpass()) {
            if ($scope.objectpass.iD_QuanLy > 0) {
                commonOpenLoadingText("#btn_luumatkhau");
                quanTriDataService.resetpass($scope.objectpass.username, $scope.objectpass.pass).then(function (result) {
                    if (result.flag) {
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                        $scope.formresetpass.center().close();
                    }
                    else
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');

                    commonCloseLoadingText("#btn_luumatkhau");
                });
            }
            else
                Notification({ title: $.i18n('label_thongbao'), message: 'label_loinhanvienvuilongchonlai' }, 'warning');
        }
    }
    $scope.huyLuuMatKhau = function () {
        $scope.formresetpass.center().close();
    }

    $scope.printQR = function () {
        $rootScope.listInA6 = commonGetRowSelected("#grid");
        console.log($rootScope.listInA6);
        $state.go('inthea6');
        //let url = $state.href('inthea6');
        //window.open(url, '_blank');
    }

    init();

    $("#grid").on("dblclick", "tr[role='row']", function () {
        let grid = $("#grid").data("kendoGrid");
        let row = grid.table.find("[data-uid=" + $(this).attr("data-uid") + "]");

        grid.clearSelection();
        grid.select(row);

        let selectedItem = grid.dataItem(grid.select());

        openFormDetail(selectedItem.iD_QuanLy);
    })

})