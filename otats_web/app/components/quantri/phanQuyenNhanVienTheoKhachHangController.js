angular.module('app').controller('phanQuyenNhanVienTheoKhachHangController', function ($scope, $http, $location, Notification, phanQuyenDataService, ComboboxDataService) {
    CreateSiteMap();

    let __idkhachhang = -1;
    let __idkhachhangsaochep = 0;
    let __idnhom = 0;

    let __arrnhanviendaphan = [];
    let __arridnhanvienchon = [];
    let __arridnhanvienthemquyen = [];

    function init() {
        $scope.khachhangsaochep = '-';
        loadgridkhachhang();
        initgridphanquyen();
        getquyen();
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
    function listColumnsgridkhachhang() {
        let dataList = [];

        dataList.push({ headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, selectable: true, width: "40px" });
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
            field: "maKH", title: $.i18n('header_makhachhang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px"
        });
        dataList.push({
            field: "diaChi", title: $.i18n('header_diachi'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "440px"
        });

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
            field: "tenDangNhap", title: $.i18n('header_tendangnhap'),
            //template: function (dataItem) {
            //    return "<a href='' class='color-primary' ng-click='openDetailFromGrid(" + kendo.htmlEncode(dataItem.idnv) + ")'>" + kendo.htmlEncode(dataItem.tenDangNhap) + "</a>";
            //},
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px"
        });
        dataList.push({
            field: "tenDayDu", title: $.i18n('header_tendaydu'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "175px"
        });
        dataList.push({
            field: "iD_Quyen", title: $.i18n('header_xemdaily'),
            template: function (e) {
                if (e.iD_Quyen != null && e.iD_Quyen.includes(";1;", 0)) {
                    return '<input type="checkbox" ng-disabled="(permission.sua <= 0)" id="xemdaily' + e.iD_NhanVien + '" ng-click="XoaQuyen(' + e.iD_NhanVien + ',\'' + e.iD_Quyen + '\',1)" checked="checked" class="k-checkbox">'
                        + '<label class="k-checkbox-label" for="xemdaily' + e.iD_NhanVien + '"></label>';
                } else {
                    return '<input type="checkbox" ng-disabled="(permission.sua <= 0)" id="xemdaily' + e.iD_NhanVien + '" ng-click="ThemQuyen(' + e.iD_NhanVien + ',\'' + e.iD_Quyen + '\',1)"  class="k-checkbox">'
                        + '<label class="k-checkbox-label" for="xemdaily' + e.iD_NhanVien + '"></label>';
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
                    return '<input type="checkbox" ng-disabled="(permission.sua <= 0)" id="giaohang' + e.iD_NhanVien + '" ng-click="XoaQuyen(' + e.iD_NhanVien + ',\'' + e.iD_Quyen + '\',2)" checked="checked" class="k-checkbox">'
                        + '<label class="k-checkbox-label" for="giaohang' + e.iD_NhanVien + '"></label>';
                } else {
                    return '<input type="checkbox" ng-disabled="(permission.sua <= 0)" id="giaohang' + e.iD_NhanVien + '" ng-click="ThemQuyen(' + e.iD_NhanVien + ',\'' + e.iD_Quyen + '\',2)"  class="k-checkbox">'
                        + '<label class="k-checkbox-label" for="giaohang' + e.iD_NhanVien + '"></label>';
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
                    return '<input type="checkbox" ng-disabled="(permission.sua <= 0)" id="vaodiem' + e.iD_NhanVien + '" ng-click="XoaQuyen(' + e.iD_NhanVien + ',\'' + e.iD_Quyen + '\',3)" checked="checked" class="k-checkbox">'
                        + '<label class="k-checkbox-label" for="vaodiem' + e.iD_NhanVien + '"></label>';
                } else {
                    return '<input type="checkbox" ng-disabled="(permission.sua <= 0)" id="vaodiem' + e.iD_NhanVien + '" ng-click="ThemQuyen(' + e.iD_NhanVien + ',\'' + e.iD_Quyen + '\',3)"  class="k-checkbox">'
                        + '<label class="k-checkbox-label" for="vaodiem' + e.iD_NhanVien + '"></label>';
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
                    return '<input type="checkbox" ng-disabled="(permission.sua <= 0)" id="thanhtoan' + e.iD_NhanVien + '" ng-click="XoaQuyen(' + e.iD_NhanVien + ',\'' + e.iD_Quyen + '\',4)" checked="checked" class="k-checkbox">'
                        + '<label class="k-checkbox-label" for="thanhtoan' + e.iD_NhanVien + '"></label>';
                } else {
                    return '<input type="checkbox" ng-disabled="(permission.sua <= 0)" id="thanhtoan' + e.iD_NhanVien + '" ng-click="ThemQuyen(' + e.iD_NhanVien + ',\'' + e.iD_Quyen + '\',4)"  class="k-checkbox">'
                        + '<label class="k-checkbox-label" for="thanhtoan' + e.iD_NhanVien + '"></label>';
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

        phanQuyenDataService.getlistnhanviendaphanquyenkhachhang(__idkhachhang).then(function (result) {
            __arrnhanviendaphan = [];
            __arridnhanvienchon = __arrnhanviendaphan.map((ar, index, arr) => {
                return ar.iD_NhanVien
            })
            $scope.gridPhanQuyenData = {
                data: __arrnhanviendaphan,
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
            kendo.ui.progress($("#gridPhanQuyen"), false);
        });
    }
    function loadgridphanquyen() {
        kendo.ui.progress($("#gridPhanQuyen"), true);

        phanQuyenDataService.getlistnhanviendaphanquyenkhachhang(__idkhachhang).then(function (result) {
            __arrnhanviendaphan = result.data;
            __arridnhanvienchon = __arrnhanviendaphan.map((ar, index, arr) => {
                return ar.iD_NhanVien
            })
            $scope.gridPhanQuyenData = {
                data: __arrnhanviendaphan,
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
            kendo.ui.progress($("#gridPhanQuyen"), false);
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
        phanQuyenDataService.getListNhomNhanVien().then(function (result) {
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
        let dataList = [];

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
        dataList.push({
            field: "tenDayDu", title: $.i18n('header_tendaydu'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "175px"
        });
        dataList.push({
            field: "dienThoai", title: $.i18n('header_sodienthoai'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px"
        });
        dataList.push({
            field: "tenNhom", title: $.i18n('header_tennhom'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px"
        });

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

        phanQuyenDataService.getlistNhanVienPhanQuyen(__idnhom, __idkhachhang).then(function (result) {
            let arr_nhanvienall = result.data;
            let arr_nhanvienchuachon = arr_nhanvienall.filter((item) => {
                return (__arridnhanvienchon.indexOf(item.iD_NhanVien) == -1)
            })

            $scope.gridnhanvienData = {
                data: arr_nhanvienchuachon,
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
            kendo.ui.progress($("#gridnhanvien"), false);
        });
    }

    function listColumnsgridchon() {
        let dataList = [];

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
            //template: function (dataItem) {
            //    return "<a href='' class='color-primary' ng-click='openDetailFromGrid(" + kendo.htmlEncode(dataItem.idnv) + ")'>" + kendo.htmlEncode(dataItem.tenDangNhap) + "</a>";
            //},
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px"
        });
        dataList.push({
            field: "tenDayDu", title: $.i18n('header_tendaydu'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "175px"
        });
        dataList.push({
            field: "dienThoai", title: $.i18n('header_sodienthoai'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px"
        });
        //dataList.push({
        //    field: "iD_Quyen", title: "Xem đại lý",
        //    template: function (e) {
        //        if (e.iD_Quyen != null && e.iD_Quyen.includes(";1;", 0)) {
        //            return '<input type="checkbox" id="xemdaily' + e.iD_NhanVien + '" ng-click="XoaQuyen(' + e.iD_NhanVien + ',\'' + e.iD_Quyen + '\',1)" checked="checked" class="k-checkbox">'
        //                + '<label class="k-checkbox-label" for="xemdaily' + e.iD_NhanVien + '"></label>';
        //        } else {
        //            return '<input type="checkbox" id="xemdaily' + e.iD_NhanVien + '" ng-click="ThemQuyen(' + e.iD_NhanVien + ',\'' + e.iD_Quyen + '\',1)"  class="k-checkbox">'
        //                + '<label class="k-checkbox-label" for="xemdaily' + e.iD_NhanVien + '"></label>';
        //        }
        //    },
        //    attributes: {
        //        style: "text-align: center;",
        //    },
        //    headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
        //    filterable: defaultFilterableGrid,
        //    width: "100px"
        //});
        //dataList.push({
        //    field: "iD_Quyen", title: "Giao hàng",
        //    template: function (e) {
        //        if (e.iD_Quyen != null && e.iD_Quyen.includes(";2;", 0)) {
        //            return '<input type="checkbox" id="giaohang' + e.iD_NhanVien + '" ng-click="XoaQuyen(' + e.iD_NhanVien + ',\'' + e.iD_Quyen + '\',2)" checked="checked" class="k-checkbox">'
        //                + '<label class="k-checkbox-label" for="giaohang' + e.iD_NhanVien + '"></label>';
        //        } else {
        //            return '<input type="checkbox" id="giaohang' + e.iD_NhanVien + '" ng-click="ThemQuyen(' + e.iD_NhanVien + ',\'' + e.iD_Quyen + '\',2)"  class="k-checkbox">'
        //                + '<label class="k-checkbox-label" for="giaohang' + e.iD_NhanVien + '"></label>';
        //        }
        //    },
        //    attributes: {
        //        style: "text-align: center;",
        //    },
        //    headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
        //    filterable: defaultFilterableGrid,
        //    width: "100px"
        //});
        //dataList.push({
        //    field: "iD_Quyen", title: "Vào điểm",
        //    template: function (e) {
        //        if (e.iD_Quyen != null && e.iD_Quyen.includes(";3;", 0)) {
        //            return '<input type="checkbox" id="vaodiem' + e.iD_NhanVien + '" ng-click="XoaQuyen(' + e.iD_NhanVien + ',\'' + e.iD_Quyen + '\',3)" checked="checked" class="k-checkbox">'
        //                + '<label class="k-checkbox-label" for="vaodiem' + e.iD_NhanVien + '"></label>';
        //        } else {
        //            return '<input type="checkbox" id="vaodiem' + e.iD_NhanVien + '" ng-click="ThemQuyen(' + e.iD_NhanVien + ',\'' + e.iD_Quyen + '\',3)"  class="k-checkbox">'
        //                + '<label class="k-checkbox-label" for="vaodiem' + e.iD_NhanVien + '"></label>';
        //        }
        //    },
        //    attributes: {
        //        style: "text-align: center;",
        //    },
        //    headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
        //    filterable: defaultFilterableGrid,
        //    width: "100px"
        //});
        //dataList.push({
        //    field: "iD_Quyen", title: "Thanh toán",
        //    template: function (e) {
        //        if (e.iD_Quyen != null && e.iD_Quyen.includes(";4;", 0)) {
        //            return '<input type="checkbox" id="thanhtoan' + e.iD_NhanVien + '" ng-click="XoaQuyen(' + e.iD_NhanVien + ',\'' + e.iD_Quyen + '\',4)" checked="checked" class="k-checkbox">'
        //                + '<label class="k-checkbox-label" for="thanhtoan' + e.iD_NhanVien + '"></label>';
        //        } else {
        //            return '<input type="checkbox" id="thanhtoan' + e.iD_NhanVien + '" ng-click="ThemQuyen(' + e.iD_NhanVien + ',\'' + e.iD_Quyen + '\',4)"  class="k-checkbox">'
        //                + '<label class="k-checkbox-label" for="thanhtoan' + e.iD_NhanVien + '"></label>';
        //        }
        //    },
        //    attributes: {
        //        style: "text-align: center;",
        //    },
        //    headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
        //    filterable: defaultFilterableGrid,
        //    width: "100px"
        //});
        return dataList;
    }
    function loadgridchon() {
        __arridnhanvienchon = __arrnhanviendaphan.map((ar, index, arr) => {
            return ar.iD_NhanVien
        })

        kendo.ui.progress($("#gridchon"), true);
        $scope.gridchonOptions = {
            sortable: true,
            height: function () {
                return 220;
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
            columns: listColumnsgridchon()
        };

        $scope.gridchonData = {
            data: [],
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
        kendo.ui.progress($("#gridchon"), false);
    }

    let fileUpload = '';
    function onUploadExcelSuccess(e) {
        var data = new FormData();
        data.append('file', e.files[0].rawFile);
        var files = e.files[0];
        $.ajax({
            url: urlApi + '/api/uploadfile/savefileExcel',
            processData: false,
            contentType: false,
            data: data,
            type: 'POST'
        }).done(function (result) {
            fileUpload = result;
        }).fail(function (a, b, c) {
            fileUpload = '';
        });
    }
    $("#filesExcel").kendoUpload({
        multiple: false,
        select: onUploadExcelSuccess,
        validation: {
            allowedExtensions: [".xls", ".xlsx"]
        },
        showFileList: true
    });
    $("#filesExcel").closest(".k-upload").find("span").text($.i18n('label_taifiledulieulen'));
    $("#files").closest(".k-upload").find("span").text($.i18n('label_chontep'));

    //event
    init();

    $scope.taiFileMau = function () {
        commonOpenLoadingText("#btn_taifilemau");

        phanQuyenDataService.taifilemauphanquyen().then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayravuilongloadlaitrang') }, 'warning');

            commonCloseLoadingText("#btn_taifilemau")
        });
    }
    $scope.importExcel = function () {
        $scope.formimport.center().open();
    }
    $scope.xuatExcel = function () {
        commonOpenLoadingText("#btn_xuatexcel");

        phanQuyenDataService.exportExcel().then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayravuilongloadlaitrang') }, 'warning');

            commonCloseLoadingText("#btn_xuatexcel");
        });
    }

    $scope.capNhatTuExcel = function () {
        commonOpenLoadingText("#btn_capnhatimport");

        phanQuyenDataService.importphanquyen(fileUpload).then(function (result) {
            if (result.flag)
                if (result.data.status == 200) {
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_tacvuthuchienthanhcong') }, 'success');
                    $scope.formimport.center().close();
                    loadgridphanquyen();
                }
                else if (result.data.status == 201)
                    commonDownFile(result.data);
                else if (result.data.status == 204)
                    Notification({
                        title: $.i18n('label_thongbao'), message: $.i18n('label_filemaukhongdungdinhdanghoackhongtontaibanghi')
                    }, 'warning');
                else if (result.data.status == 411)
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_soluongbanghikhongvuotqua2000dong') }, 'warning');
                else
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloisayravuilongkiemtralaifilecapnhat') }, 'warning');
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayra_loadlaitrang') }, 'warning');

            commonCloseLoadingText("#btn_capnhatimport")
        });
    }
    $scope.huyCapNhatExcel = function () {
        $scope.formimport.center().close();
    }

    $scope.saochepquyen = function () {
        if (__idkhachhang > 0) {
            __idkhachhangsaochep = __idkhachhang;

            let grid = $("#gridKhachhang").data("kendoGrid");
            let listRowsSelected = commonGetRowSelected("#gridKhachhang");
            $scope.khachhangsaochep = listRowsSelected[0].ten;

            Notification({ title: $.i18n('label_thongbao'), message: $.i18n("label_dasaochepthanhcongdanhsachnhanvienganquyenvaobonhotam") }, 'success');
        } else {
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_canchonmotkhachhangdethuchien') }, 'error');
        }

    }
    $scope.danquyen = function () {
        let grid = $('#gridKhachhang').data("kendoGrid");
        let listid = grid.selectedKeyNames();

        if (__idkhachhangsaochep > 0) {
            //if (listid.indexOf(__idkhachhangsaochep) > -1)
            if (listid.length > 0) {
                openConfirm($.i18n("label_bancochacchanmuondanquyenkhong"), 'apDungDanQuyen', null, listid);
            } else {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_chuachonkhachhangdethuchiendanquyen') }, 'error');
            }
        } else
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_khongcodulieukhachhangsaochepdethuchien') }, 'error');

    }
    $scope.apDungDanQuyen = function (listid) {
        commonOpenLoadingText("#btn_danquyen");
        phanQuyenDataService.copyPhanQuyen(__idkhachhangsaochep, listid).then(function (result) {
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

            __arridnhanvienthemquyen = [];

            loadgridchon();

            $scope.formdetailphanquyen.center().maximize().open();
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
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_canchonnhanviendethuchien') }, 'error');
        }

        if (flag) {
            openConfirm($.i18n("label_bancochacchanmuonxoacacnhanviendaphanchokhachhangkhong"), 'apDungXoaAllQuyen', null, listid);
        }
    }
    $scope.apDungXoaAllQuyen = function (_listid) {
        phanQuyenDataService.themPhanQuyenNhanVienKhachHang(_listid, __idkhachhang, ';').then(function (result) {
            if (result.flag) {
                loadgridphanquyen();
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_xoaphanquyenkhachhangchonhanvienthanhcong') }, 'success');
            } else {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_xoaphanquyenkhachhangchonhanvienthatbaivuilongthuchienlai') }, 'error');
            }
        });
    }

    $scope.ThemQuyen = function (_idnv, _curquyen, _idquyen) {
        if (_curquyen == "null") {
            _curquyen = ";" + _idquyen + ";";
        } else {
            _curquyen += _idquyen + ";";
        }
        this.dataItem.iD_Quyen = _curquyen;
        $scope.gridPhanQuyen.refresh();
        let data = {
            ID_NhanVien: _idnv,
            ID_KhachHang: __idkhachhang,
            ID_Quyen: _curquyen
        }
        phanQuyenDataService.updatePhanQuyen(data).then(function (result) {
            if (result.flag) {
                if (result.data.success) {
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.data.msg) }, 'success');
                } else {
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.data.msg) }, 'error');
                }
            } else {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.data.msg) }, 'error');
            }
        })
    }
    $scope.XoaQuyen = function (_idnv, _curquyen, _idquyen) {
        if (_curquyen != "null")
            _curquyen = _curquyen.replace(_idquyen + ';', '');

        this.dataItem.iD_Quyen = _curquyen;
        $scope.gridPhanQuyen.refresh();
        let data = {
            ID_NhanVien: _idnv,
            ID_KhachHang: __idkhachhang,
            ID_Quyen: _curquyen
        }
        phanQuyenDataService.updatePhanQuyen(data).then(function (result) {
            if (result.flag) {
                if (result.data.success) {
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.data.msg) }, 'success');
                } else {
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.data.msg) }, 'error');
                }
            } else {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.data.msg) }, 'error');
            }
        })
    }

    $scope.themnhanvienquyen = function (e) {
        let myGrid = $('#gridnhanvien').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;

        let myGridAdd = $("#gridchon").data("kendoGrid");
        myGridAdd.dataSource.add(dataItem);

        myGrid.dataSource.remove(dataItem);

        __arridnhanvienchon.push(dataItem.iD_NhanVien);
        __arridnhanvienthemquyen.push(dataItem.iD_NhanVien);
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

        __arridnhanvienchon = __arridnhanvienchon.filter(item => item != dataItem.iD_NhanVien);
        __arridnhanvienthemquyen = __arridnhanvienthemquyen.filter(item => item != dataItem.iD_NhanVien);
    }

    $scope.luuphanquyen = function () {
        if (__arridnhanvienthemquyen.length > 0) {
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
                Notification({ title: $.i18n('label_thongbao'), message: 'label_chuachonquyenapdungdethuchien' }, 'error');
            } else {
                commonOpenLoadingText("#btn_luuphanquyen");
                phanQuyenDataService.themPhanQuyenNhanVienKhachHang(__arridnhanvienthemquyen, __idkhachhang, idquyen).then(function (result) {
                    if (result.flag) {
                        $scope.formdetailphanquyen.center().maximize().close();
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
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_chuacodulieunhanviendethuchien') }, 'error');
    }
    $scope.huyluuphanquyen = function () {
        $scope.formdetailphanquyen.center().maximize().close();
    }

})