angular.module('app').controller('phanQuyenKhachHangTheoNhomNhanVienController', function (ComboboxDataService, $location,$scope, Notification, phanQuyenDataService) {
    CreateSiteMap();

    let __idkhachhang = -1;
    let __idkhachhangsaochep = 0;

    function init() {
        getquyen();
        $scope.khachhangsaochep = '-';
        loadgridkhachhang();
        initgridphanquyen();
        
        inittreephongban();
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
    function listColumnsgridkhachhang() {
        let dataList = [];

        dataList.push({ headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { class: "text-center" }, selectable: true, width: "40px" });
        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({
            field: "ten", title: $.i18n('header_tenkhachhang'),
            //template: function (dataItem) {
            //    return "<a href='' class='color-primary' ng-click='openDetailFromGrid(" + kendo.htmlEncode(dataItem.iD_KhachHang) + ")'>" + kendo.htmlEncode(dataItem.ten) + "</a>";
            //},
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px"
        });
        dataList.push({
            field: "maKH", title: $.i18n('header_makhachhang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px" });
        dataList.push({
            field: "diaChi", title: $.i18n('header_diachi'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "440px" });

        return dataList;
    }
    function loadgridkhachhang() {
        kendo.ui.progress($("#gridKhachhang"), true);
        $scope.gridKhachHangOptions = {
            sortable: true,
            height: function () {
                let heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".group-btn").height());
                return heightGrid - 35;
            },
            dataBinding: function () {
                RecordNumber = (this.dataSource.page() - 1) * this.dataSource.pageSize();
            },
            dataBound: function (e) {
                e.sender.select("tr:eq(0)");
            },
            resizable: true,
            editable: false,
            filterable: {
                mode: "row"
            },
            selectable: "multiple, row",
            change: onChange,
            pageable: $scope.lang == 'vi-vn' ? pageableShort_vi : pageableShort_en,
            columns: listColumnsgridkhachhang()
        };

        phanQuyenDataService.getlistkhachhang().then(function (result) {
            $scope.gridKhachHangData = {
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
                            }
                        }
                    }
                },
                pageSize: 20
            };



            kendo.ui.progress($("#gridKhachhang"), false);

            if (!result.flag) {
                Notification({ message: $.i18n('label_khongtheloaddanhsachkhachhangvuilongtailaitrang') }, 'warning');
            }
        });
    }
    function onChange(arg) {
        let grid = $("#gridKhachhang").data("kendoGrid");
        let listid = grid.selectedKeyNames();
        if (listid.length == 1)
            __idkhachhang = listid[0];
        else
            __idkhachhang = -1;

        loadgridphanquyen();
    }

    function listColumnsgridphanquyen() {
        let dataList = [];

        dataList.push({ headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { class: "text-center" }, selectable: true, width: "40px" });
        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({
            field: "tenNhom", title: $.i18n('header_tennhom'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "175px" });
        dataList.push({
            field: "iD_Quyen", title: $.i18n('header_xemdaily'),
            template: function (e) {
                if (e.iD_Quyen != null && e.iD_Quyen.includes(";1;", 0)) {
                    return '<input type="checkbox" ng-disabled="(permission.sua <= 0)" id="xemdaily' + e.iD_Nhom + '" ng-click="XoaQuyen(' + e.iD_Nhom + ',\'' + e.iD_Quyen + '\',1)" checked="checked" class="k-checkbox">'
                        + '<label class="k-checkbox-label" for="xemdaily' + e.iD_Nhom + '"></label>';
                } else {
                    return '<input type="checkbox" ng-disabled="(permission.sua <= 0)" id="xemdaily' + e.iD_Nhom + '" ng-click="ThemQuyen(' + e.iD_Nhom + ',\'' + e.iD_Quyen + '\',1)"  class="k-checkbox">'
                        + '<label class="k-checkbox-label" for="xemdaily' + e.iD_Nhom + '"></label>';
                }
            },
            attributes: {
                style: "text-align: center;",
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: false,
            width: "100px"
        });
        dataList.push({
            field: "iD_Quyen", title: $.i18n('header_vaodiem'),
            template: function (e) {
                if (e.iD_Quyen != null && e.iD_Quyen.includes(";2;", 0)) {
                    return '<input type="checkbox" ng-disabled="(permission.sua <= 0)" id="vaodiem' + e.iD_Nhom + '" ng-click="XoaQuyen(' + e.iD_Nhom + ',\'' + e.iD_Quyen + '\',2)" checked="checked" class="k-checkbox">'
                        + '<label class="k-checkbox-label" for="vaodiem' + e.iD_Nhom + '"></label>';
                } else {
                    return '<input type="checkbox" ng-disabled="(permission.sua <= 0)" id="vaodiem' + e.iD_Nhom + '" ng-click="ThemQuyen(' + e.iD_Nhom + ',\'' + e.iD_Quyen + '\',2)"  class="k-checkbox">'
                        + '<label class="k-checkbox-label" for="vaodiem' + e.iD_Nhom + '"></label>';
                }
            },
            attributes: {
                style: "text-align: center;",
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: false,
            width: "100px"
        });
        dataList.push({
            field: "iD_Quyen", title: $.i18n('header_giaohang'),
            template: function (e) {
                if (e.iD_Quyen != null && e.iD_Quyen.includes(";3;", 0)) {
                    return '<input type="checkbox" ng-disabled="(permission.sua <= 0)" id="giaohang' + e.iD_Nhom + '" ng-click="XoaQuyen(' + e.iD_Nhom + ',\'' + e.iD_Quyen + '\',3)" checked="checked" class="k-checkbox">'
                        + '<label class="k-checkbox-label" for="giaohang' + e.iD_Nhom + '"></label>';
                } else {
                    return '<input type="checkbox" ng-disabled="(permission.sua <= 0)" id="giaohang' + e.iD_Nhom + '" ng-click="ThemQuyen(' + e.iD_Nhom + ',\'' + e.iD_Quyen + '\',3)"  class="k-checkbox">'
                        + '<label class="k-checkbox-label" for="giaohang' + e.iD_Nhom + '"></label>';
                }
            },
            attributes: {
                style: "text-align: center;",
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: false,
            width: "100px"
        });
        dataList.push({
            field: "iD_Quyen", title: $.i18n('header_thanhtoan'),
            template: function (e) {
                if (e.iD_Quyen != null && e.iD_Quyen.includes(";4;", 0)) {
                    return '<input type="checkbox" ng-disabled="(permission.sua <= 0)" id="thanhtoan' + e.iD_Nhom + '" ng-click="XoaQuyen(' + e.iD_Nhom + ',\'' + e.iD_Quyen + '\',4)" checked="checked" class="k-checkbox">'
                        + '<label class="k-checkbox-label" for="thanhtoan' + e.iD_Nhom + '"></label>';
                } else {
                    return '<input type="checkbox" ng-disabled="(permission.sua <= 0)" id="thanhtoan' + e.iD_Nhom + '" ng-click="ThemQuyen(' + e.iD_Nhom + ',\'' + e.iD_Quyen + '\',4)"  class="k-checkbox">'
                        + '<label class="k-checkbox-label" for="thanhtoan' + e.iD_Nhom + '"></label>';
                }
            },
            attributes: {
                style: "text-align: center;",
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: false,
            width: "100px"
        });
        return dataList;
    }
    function initgridphanquyen() {
        kendo.ui.progress($("#gridPhanQuyen"), true);
        $scope.gridPhanQuyenOptions = {
            sortable: true,
            height: function () {
                let heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".group-btn").height());
                return heightGrid - 35;
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
            columns: listColumnsgridphanquyen()
        };

        phanQuyenDataService.getlistNhomNhanVienPhanQuyen(__idkhachhang).then(function (result) {
            $scope.gridPhanQuyenData = {
                data: result.data,
                schema: {
                    model: {
                        id: 'iD_Nhom',
                        fields: {
                            iD_Nhom: {
                                type: "number"
                            }
                        }
                    }
                },
                pageSize: 20
            };
            kendo.ui.progress($("#gridPhanQuyen"), false);
        });
    }
    function loadgridphanquyen() {
        kendo.ui.progress($("#gridPhanQuyen"), true);

        phanQuyenDataService.getlistNhomNhanVienPhanQuyen(__idkhachhang).then(function (result) {
            $scope.gridPhanQuyenData = {
                data: result.data,
                schema: {
                    model: {
                        id: 'iD_Nhom',
                        fields: {
                            iD_Nhom: {
                                type: "number"
                            }
                        }
                    }
                },
                pageSize: 20
            };
            kendo.ui.progress($("#gridPhanQuyen"), false);
        });
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
                checkChildren: true,
                template: "<input id='checkbox_#:item.uid#' ng-model='dataItem.isChecked' type='checkbox' class='k-checkbox'/><label for='checkbox_#:item.uid#' class='k-checkbox-label'></label>"
            },
            template: "#: item.tenNhom #"
        };

        loadTreePhongBan();
    }
    function loadTreePhongBan() {
        phanQuyenDataService.getListNhomNhanVien().then(function (result) {
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

            if (nodes[i].checked != undefined) {
                if (nodes[i].checked) {
                    checkedNodes.push(nodes[i].iD_Nhom);
                }
            }

            if (nodes[i].hasChildren) {
                getcheckedNodeIds(nodes[i].children.view(), checkedNodes, nodes[i].iD_Nhom);
            }
        }
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

    //event
    init();

    $scope.saochepquyen = function () {
        if (__idkhachhang > 0) {
            __idkhachhangsaochep = __idkhachhang;

            let grid = $("#gridKhachhang").data("kendoGrid");
            let listRowsSelected = commonGetRowSelected("#gridKhachhang");
            $scope.khachhangsaochep = listRowsSelected[0].ten;

            Notification({ title: $.i18n('label_thongbao'), message: $.i18n("label_dasaochepthanhcongdanhsachnhomnhanvienganquyenvaobonhotam") }, 'success');
        } else {
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_canchonmotkhachhangdethuchien') }, 'error');
        }

    }
    $scope.danquyen = function () {
        let grid = $('#gridKhachhang').data("kendoGrid");
        let listid = grid.selectedKeyNames();

        if (__idkhachhangsaochep > 0) {
            if (listid.length == 1) {
                openConfirm($.i18n("label_bancochacchanmuondanquyenkhong"), 'apDungDanQuyen', null, listid);
            } else if (listid.length > 1) {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_chicothedanquyenchotungkhachhangmot') }, 'error');
            }
            else {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_chuachonkhachhangdethuchiendanquyen') }, 'error');
            }
        } else
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_khongcodulieukhachhangsaochepdethuchien') }, 'error');

    }
    $scope.apDungDanQuyen = function (listid) {
        commonOpenLoadingText("#btn_danquyen");
        phanQuyenDataService.copyPhanQuyenByNhom(__idkhachhangsaochep, __idkhachhang).then(function (result) {
            if (result.flag) {
                if (result.data.success) {
                    loadgridphanquyen();
                    Notification({ title: $.i18n('label_thongbao'), message: result.data.msg }, 'success');
                } else {
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.data.msg) }, 'error');
                }
            } else {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.data.msg) }, 'error');
            }

            commonCloseLoadingText("#btn_danquyen");
        });
    }

    $scope.openformaddnhanvien = function () {
        if (__idkhachhang > 0) {
            $scope.xemdaily = true;
            $scope.vaodiem = true;
            $scope.giaohang = true;
            $scope.thanhtoan = true;

            loadTreePhongBan();

            $scope.formdetailphanquyen.center().open();
        } else
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_canchonmotkhachhangdethuchien') }, 'error');
    }
    $scope.xoaallquyen = function () {
        let grid = $('#gridPhanQuyen').data("kendoGrid");
        let listid = grid.selectedKeyNames();

        let flag = true;
        if (__idkhachhang <= 0) {
            flag = false;
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_canchonmotkhachhangdethuchien') }, 'error');
        }
            

        if (flag && listid.length <= 0) {
            flag = false;
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_chonnhomnhanvien') }, 'error');
        }
            
        if (flag) {
            openConfirm($.i18n("label_bancochacchanmuonxoakhong"), 'apDungXoaAllQuyen', null, listid);
        }
    }
    $scope.apDungXoaAllQuyen = function (_listid) {
        phanQuyenDataService.xoaphanquyennhomnhanvienkhachhang(_listid, __idkhachhang).then(function (result) {
            if (result.flag) {
                loadgridphanquyen();
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
            } else {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'error');
            }
        });
    }

    $scope.ThemQuyen = function (_idnhom, _curquyen, _idquyen) {
        if (_curquyen == "null") {
            _curquyen = ";" + _idquyen + ";";
        } else {
            _curquyen += _idquyen + ";";
        }
        this.dataItem.iD_Quyen = _curquyen;
        $scope.gridPhanQuyen.refresh();
        let data = {
            ID_Nhom: _idnhom,
            ID_KhachHang: __idkhachhang,
            ID_Quyen: _curquyen
        }
        phanQuyenDataService.updatePhanQuyenByNhom(data).then(function (result) {
            if (result.flag) {
                if (result.data.success) {
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_phanquyenkhachhangchonhomnhanvienthanhcong') }, 'success');
                } else {
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_phanquyenkhachhangchonhomnhanvienthatbaivuilongthuchienlai') }, 'error');
                }
            } else {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_phanquyenkhachhangchonhomnhanvienthatbaivuilongthuchienlai') }, 'error');
            }
        })
    }
    $scope.XoaQuyen = function (_idnhom, _curquyen, _idquyen) {
        if (_curquyen != "null")
            _curquyen = _curquyen.replace(_idquyen + ';', '');

        this.dataItem.iD_Quyen = _curquyen;
        $scope.gridPhanQuyen.refresh();
        let data = {
            ID_Nhom: _idnhom,
            ID_KhachHang: __idkhachhang,
            ID_Quyen: _curquyen
        }
        phanQuyenDataService.updatePhanQuyenByNhom(data).then(function (result) {
            if (result.flag) {
                if (result.data.success) {
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_phanquyenkhachhangchonhomnhanvienthanhcong') }, 'success');
                } else {
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_phanquyenkhachhangchonhomnhanvienthatbaivuilongthuchienlai') }, 'error');
                }
            } else {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_phanquyenkhachhangchonhomnhanvienthatbaivuilongthuchienlai') }, 'error');
            }
        })
    }

    $scope.luuphanquyen = function () {
        let nodes = $scope.tree.dataSource.view();
        let checkednodes = [];
        getcheckedNodeIds(nodes, checkednodes);

        if (checkednodes.length > 0) {
            let idquyen = ';';
            if ($scope.xemdaily)
                idquyen += '1;';
            if ($scope.vaodiem)
                idquyen += '2;';
            if ($scope.giaohang)
                idquyen += '3;';
            if ($scope.thanhtoan)
                idquyen += '4;';
            if (idquyen == ';') {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_chuachonquyenapdungdethuchien') }, 'error');
            } else {
                commonOpenLoadingText("#btn_luuphanquyen");
                phanQuyenDataService.phanquyennhieunhomnhanvienkhachhang(checkednodes, __idkhachhang, idquyen).then(function (result) {
                    if (result.flag) {
                        $scope.formdetailphanquyen.center().close();
                        loadgridphanquyen();
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                    } else {
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'error');
                    }

                    commonCloseLoadingText("#btn_luuphanquyen");
                });
            }
        }
        else
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_chuacodulieunhomnhanviendethuchien') }, 'error');
    }
    $scope.huyluuphanquyen = function () {
        $scope.formdetailphanquyen.center().close();
    }

})