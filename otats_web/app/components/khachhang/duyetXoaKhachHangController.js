angular.module('app').controller('duyetXoaKhachHangController', function ($scope, $location, Notification, ComboboxDataService, khachHangDataService) {
    CreateSiteMap();
    $scope.idNhom = 0;
    $scope.idKhachHang = 0;

    function init() {
        getquyen();
        loadgrid();
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
            dataTextField: "tenHienThi_NhanVien",
            dataValueField: "iD_Nhom",
            select: onSelectNhom,
        });

        loadtreeView();
    }
    function loadtreeView() {
        khachHangDataService.getListNhomNhanVien().then(function (result) {
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
        loadgrid_nhanvien();
    }
    function onSelectNhom(e) {
        $scope.idNhom = $("#treeview").getKendoTreeView().dataItem(e.node).iD_Nhom;
        loadgrid_nhanvien();
    }

    let columnThaoTac = function () {
        var template = '<button ng-click="duyetxoa(#=idKhachHang#,#=trangThai#)" class="btn btn-link btn-menubar" title ="' + $.i18n('button_duyet') + '" ><i class="fas fa-check-circle fas-sm color-infor"></i></button> '
            + '<button ng-click="huyxoa(#=idKhachHang#,#=trangThai#)" class="btn btn-link btn-menubar" title ="' + $.i18n('button_huy') + '" ><i class="fas fa-trash fas-sm color-danger"></i></button>';
        var obj = {
            template: template,
            title: $.i18n('label_thaotac'), width: "110px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
        }
        return obj;
    }
    function listColumnsgrid() {
        let dataList = [];

        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push(columnThaoTac());
        dataList.push({ field: "maKhachHang", title: $.i18n('header_makhachhang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px" });
        dataList.push({ field: "tenKhachHang", title: $.i18n('header_tenkhachhang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "250px" });
        dataList.push({ field: "diaChi", title: $.i18n('header_diachi'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "300px" });
        dataList.push({ field: "nhanVienQuanLy", title: $.i18n('header_nhanvienquanly'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "160px" });
        dataList.push({ field: "yeuCau", title: $.i18n('header_yeucau'), attributes: { "class": "table-cell", style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "130px" });
        dataList.push({ field: "ghiChu", title: $.i18n('header_ghichu'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid });

        return dataList;
    }
    function listColumnsgrid_nhanvien() {
        let dataList = [];

        dataList.push({ headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, selectable: true, width: "40px" });
        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({ field: "tenDangNhap", title: $.i18n('header_tennhanvien'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px" });
        dataList.push({ field: "tenDayDu", title: $.i18n('header_tendaydu'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "175px" });
        dataList.push({ field: "dienThoai", title: $.i18n('header_sodienthoai'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px" });
        dataList.push({ field: "trangThaiTrucTuyen", title: $.i18n('header_trangthaitructuyen'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({ field: "tenNhom", title: $.i18n('header_tennhom'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px" });
        //dataList.push({ field: "anhDaiDien", title: $.i18n('header_anhdaidien'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });

        return dataList;
    }
    function loadgrid() {
        kendo.ui.progress($("#grid"), true);
        $scope.gridOptions = {
            sortable: true,
            height: function () {
                let heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height());
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
        khachHangDataService.getlistduyetxoa().then(function (result) {
            $scope.gridData = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                            iD_KhachHang: { type: 'number' }
                        }
                    }
                },
                pageSize: 20
            };

            kendo.ui.progress($("#grid"), false);
        });
    }
    function loadgrid_nhanvien() {
        kendo.ui.progress($("#grid_chiTiet"), true);
        $scope.gridOptions_chitiet = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".toolbarmenu").height());
                return heightGrid - 90;
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
            columns: listColumnsgrid_nhanvien(),
        };
        khachHangDataService.getlist_nhanVien($scope.idNhom).then(function (result) {
            $scope.gridData_chiTiet = {
                data: result.data,
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
            kendo.ui.progress($("#grid_chiTiet"), false);
        });
    }

    function openConfirmDuyet(message, acceptAction, cancelAction, idKhachHang, trangThai) {
        var scope = angular.element("#mainContentId").scope();
        $(" <div id='confirmDelete'></div>").appendTo("body").kendoDialog({
            width: "450px",
            closable: true,
            modal: true,
            title: $scope.lang == 'vi-vn' ? "Xác nhận!" : "Confirm!",
            content: message,
            actions: [
                {
                    text: $scope.lang == 'vi-vn' ? 'Chuyển quyền' : "Authorization Transfer", primary: false, action: function () {
                        if (cancelAction != null) {
                            scope[cancelAction](idKhachHang, trangThai);
                        }
                    }
                },
                {
                    text: $scope.lang == 'vi-vn' ? 'Xóa' : 'Delete', primary: true, action: function () {
                        scope[acceptAction](idKhachHang, trangThai);
                    }
                }
            ],
        })
    }
    function openConfirmHuy(message, acceptAction, cancelAction, idKhachHang, trangThai) {
        var scope = angular.element("#mainContentId").scope();
        $(" <div id='confirmDelete'></div>").appendTo("body").kendoDialog({
            width: "450px",
            closable: true,
            modal: true,
            title: $scope.lang == 'vi-vn' ? "Xác nhận!" : "Confirm!",
            content: message,
            actions: [
                {
                    text: $scope.lang == 'vi-vn' ? 'Hủy' : 'Cancel', primary: false, action: function () {
                        if (cancelAction != null) {
                            scope[cancelAction](idKhachHang, trangThai);
                        }
                    }
                },
                {
                    text: $scope.lang == 'vi-vn' ? 'Đồng ý' : 'Accept', primary: true, action: function () {
                        scope[acceptAction](idKhachHang, trangThai);
                    }
                }
            ],
        })
    }
    function openFormDetail(idKhachHang) {
        $scope.idKhachHang = idKhachHang;
        $scope.form_chitiet.center().maximize().open();
        loadtreeView();
        loadgrid_nhanvien();
    }

    function getRowSelected() {
        $scope.listRowsSelected = [];
        var grid = $("#grid_chiTiet").data("kendoGrid");
        grid.select().each(function () {
            var dataItem = grid.dataItem(this);
            if ($scope.listRowsSelected.indexOf(dataItem) == -1) {
                $scope.listRowsSelected.push(dataItem);
            }
        })
    }
    function validateChuyenQuyen() {
        var flag = true;
        getRowSelected();

        var message = "";
        if ($scope.listRowsSelected.length == 0) {
            flag = false;
            message = $.i18n('label_chuachonnhanviendethuchienchuyenquyen');
        }
        if (!flag) {
            Notification({ title: $.i18n('label_thongbao'), message: message }, 'warning');
        }
        return flag;
    }
    function thucHienChuyenQuyen() {
        if (validateChuyenQuyen()) {
            var listID = '';
            var count = 0;
            var length = $scope.listRowsSelected.length;
            angular.forEach($scope.listRowsSelected, function (obj) {
                if (count != (length - 1)) {
                    listID += (obj.idnv + ",");
                } else {
                    listID += obj.idnv;
                }
                count++;
            });

            khachHangDataService.chuyenquyen($scope.idKhachHang, listID).then(function (result) {
                if (result.flag) {
                    Notification({ title: $.i18n('label_thongbao'), message: result.message }, 'success');
                    $scope.form_chitiet.center().close();
                    loadgrid();
                } else {
                    Notification({ title: $.i18n('label_thongbao'), message: result.message }, 'warning');
                }
            });
        }
    }

    //event
    $scope.duyetxoa = function (idKhachHang, trangThai) {
        if ($scope.permission.sua > 0)
            openConfirmDuyet($.i18n("label_bancomuonchuyenquyenquanlychonhanvienkhackhongneukhongkhachhangsebixoakhoihethong")
                , 'apDungDuyetXoa', 'chuyenQuyen', idKhachHang, trangThai);

        else
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_bankhongcoquyenthuchienthaotac') }, 'warning');
    };
    $scope.apDungDuyetXoa = function (idKhachHang, trangThai) {
        khachHangDataService.duyetxoa(idKhachHang, trangThai).then(function (result) {
            if (result.flag) {
                Notification({ title: $.i18n('label_thongbao'), message: result.message }, 'success');
                loadgrid();
            } else {
                Notification({ title: $.i18n('label_thongbao'), message: result.message }, 'warning');
            }
        });
    }
    $scope.chuyenQuyen = function (idKhachHang, trangThai) {
        openFormDetail(idKhachHang);
    }

    $scope.huyxoa = function (idKhachHang, trangThai) {
        if ($scope.permission.sua > 0)
            openConfirmHuy($.i18n('label_khachhangsequaytrolaidanhsachkhachhangbancochacchanhuyyeucaunay')
                , 'apDungHuyXoa', null, idKhachHang, trangThai);
        else
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_bankhongcoquyenthuchienthaotac') }, 'warning');
    };
    $scope.apDungHuyXoa = function (idKhachHang, trangThai) {
        khachHangDataService.huyxoa(idKhachHang, trangThai).then(function (result) {
            if (result.flag) {
                Notification({ title: $.i18n('label_thongbao'), message: result.message }, 'success');
                loadgrid();
            } else {
                Notification({ title: $.i18n('label_thongbao'), message: result.message }, 'warning');
            }
        });
    }

    $scope.capNhatChuyenQuyen = function (listID) {
        thucHienChuyenQuyen();
    }

    init();

})