angular.module('app').controller('phieuDieuChinhController', function ($location, $rootScope, $scope, $timeout, Notification, khoHangDataService, ComboboxDataService) {
    CreateSiteMap();

    let height = 350;

    $scope.object = { PhieuDieuChinh: {}, ChiTietDieuChinh: [] };
    $scope.iD_PhieuDieuChinhNhapKho = 0;
    $scope.iD_PhieuNhap = 0;

    function init() {
        loadgrid();
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
    function loadphieunhap() {
        khoHangDataService.getlistphieunhapcombo().then(function (response) {
            $scope.phieunhapData = response.data;
        });
    }

    function listColumnsgrid() {
        let dataList = [];

        dataList.push({ headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, selectable: true, width: "40px" });
        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({ field: "tenPhieuNhap", title: $.i18n("header_sophieunhap"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px" });
        dataList.push({
            field: "createdDate", title: $.i18n("header_ngaydieuchinh"),
            template: function (dataItem) {
                return kendo.htmlEncode(kendo.toString(dataItem.createdDate, formatDateTime));
            },
            attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({ field: "tenNhanVien", title: $.i18n("header_nhanvienlap"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({ field: "dienGiai", title: $.i18n("header_diengiai"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "350px" });

        return dataList;
    }
    function loadgrid() {
        kendo.ui.progress($("#grid"), true);
        $scope.gridOptions = {
            sortable: true,
            height: function () {
                let heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height());
                return heightGrid - 80;
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

        khoHangDataService.getlistphieudieuchinh().then(function (response) {
            $scope.gridData = {
                data: response.data,
                schema: {
                    model: {
                        fields: {
                            createdDate: {
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

    function listColumnsgridphieunhap() {
        let dataList = [];

        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({ field: "maHang", title: $.i18n("header_mahang"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px" });
        dataList.push({ field: "tenHang", title: $.i18n("header_tenhang"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({
            field: "soLuong", title: $.i18n("header_soluong"),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            attributes: { style: "text-align: center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px"
        });

        return dataList;
    }
    function loadgridphieunhap() {
        kendo.ui.progress($("#gridphieunhap"), true);
        $scope.gridphieunhapOptions = {
            sortable: true,
            persistSelection: true,
            autoFitColumn: true,
            height: function () {
                return height - 80;
            },
            dataBinding: function () {
                RecordNumber = (this.dataSource.page() - 1) * this.dataSource.pageSize();
            },
            dataBound: function (e) {
                onDataBound(e);
            },
            resizable: true,
            editable: false,
            filterable: {
                mode: "row"
            },
            pageable: $scope.lang == 'vi-vn' ? pageableShort_vi : pageableShort_en,
            columns: listColumnsgridphieunhap()
        };

        khoHangDataService.getchitietphieunhap($scope.iD_PhieuNhap).then(function (response) {
            $scope.gridphieunhapData = {
                data: response.data,
                schema: {
                    model: {
                        fields: {
                            soLuong: {
                                type: "number"
                            }
                        }
                    }
                },
                pageSize: 20
            };

            kendo.ui.progress($("#gridphieunhap"), false);
        });
    }

    function onDataBound(e) {
        var grid = e.sender;
        var rows = grid.tbody.find("[role='row']");

        rows.unbind("dblclick");
        var myGrid = $('#gridphieunhap').data("kendoGrid");
        rows.on("dblclick", function (e) {
            var rowClick = myGrid.dataItem($(e.target).closest("tr"));
            e.preventDefault();
            pushDataToAdd(rowClick);
        });
    };
    function pushDataToAdd(rowClick) {
        let myGrid = $("#gridphieunhap").data("kendoGrid");
        let myGridAdd = $("#gridphieudieuchinh").data("kendoGrid");

        let obj = {
            iD_PhieuDieuChinhNhapChiTiet: 0,
            iD_ChiTietPhieuNhap: rowClick.iD_ChiTietPhieuNhap,
            iD_HangHoa: rowClick.iD_HangHoa,
            maHang: rowClick.maHang,
            tenHang: rowClick.tenHang,
            soLuongPhieuNhap: rowClick.soLuong,
            soLuong: rowClick.soLuong,
            soLuongDieuChinh: 0,
            loaiDieuChinh: 1,
            tenLoaiDieuChinh: $.i18n("label_dieuchinhtang")
        }
        myGridAdd.dataSource.add(obj);

        myGrid.dataSource.remove(rowClick);
    };

    function listColumnsgridphieudieuchinh() {
        let dataList = [];

        dataList.push({
            field: "chiTiet", title: $.i18n("button_xoa"),
            template: '<button ng-click="deleterow()" class="btn btn-link btn-menubar" title ="' + $.i18n("button_xoa") + '" ><i class="fas fa-trash fas-sm color-danger"></i></button>',
            attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "55px"
        });
        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({ field: "maHang", title: $.i18n("header_mahang"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px" });
        dataList.push({ field: "tenHang", title: $.i18n("header_tenhang"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({
            field: "soLuongPhieuNhap", title: $.i18n("header_soluongphieunhap"),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            attributes: { style: "text-align: center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        });
        dataList.push({
            field: "soLuong", title: $.i18n("header_soluongsaudieuchinh"),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            attributes: { style: "text-align: center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        });
        dataList.push({
            field: "soLuongDieuChinh", title: $.i18n("header_soluongdieuchinh"),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            attributes: { style: "text-align: center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        });
        dataList.push({ field: "tenLoaiDieuChinh", title: $.i18n("header_loai"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });

        return dataList;
    }
    function loadgridphieudieuchinh() {
        kendo.ui.progress($("#gridphieudieuchinh"), true);
        $scope.gridphieudieuchinhOptions = {
            sortable: true,
            height: function () {
                return height - 80;
            },
            dataBinding: function () {
                RecordNumber = (this.dataSource.page() - 1) * this.dataSource.pageSize();
            },
            save: function (e) {
                savegrid(e);
            },
            resizable: true,
            editable: true,
            filterable: {
                mode: "row"
            },
            pageable: $scope.lang == 'vi-vn' ? pageableShort_vi : pageableShort_en,
            columns: listColumnsgridphieudieuchinh()
        };

        khoHangDataService.getchitietdieuchinh($scope.iD_PhieuDieuChinhNhapKho).then(function (response) {
            $scope.gridphieudieuchinhData = {
                data: response.data,
                schema: {
                    model: {
                        fields: {
                            iD_PhieuDieuChinhNhapChiTiet: {
                                type: "number",
                                editable: false,
                                nullable: true
                            },
                            iD_ChiTietPhieuNhap: {
                                type: "number",
                                editable: false,
                                nullable: true
                            },
                            iD_HangHoa: {
                                type: "number",
                                editable: false,
                                nullable: true
                            },
                            maHang: {
                                type: "string",
                                editable: false,
                                nullable: true
                            },
                            tenHang: {
                                type: "string",
                                editable: false,
                                nullable: true
                            },
                            tenDonVi: {
                                type: "string",
                                editable: false,
                                nullable: true
                            },
                            soLuongPhieuNhap: {
                                type: "number",
                                editable: false,
                                nullable: false
                            },
                            soLuong: {
                                type: "number",
                                editable: true,
                                nullable: false
                            },
                            soLuongDieuChinh: {
                                type: "number",
                                editable: false,
                                nullable: false
                            },
                            loaiDieuChinh: {
                                type: "number",
                                editable: false,
                                nullable: false
                            },
                            tenLoaiDieuChinh: {
                                type: "string",
                                editable: false,
                                nullable: false
                            },
                        }
                    },
                    parse: function (response) {
                        for (var i = 0; i < response.length; i++) {
                            if (response[i].loaiDieuChinh == 1) {
                                response[i].tenLoaiDieuChinh = $.i18n('label_dieuchinhtang');
                            } else if (response[i].loaiDieuChinh == 2) {
                                response[i].tenLoaiDieuChinh = $.i18n('label_dieuchinhgiam');
                            }
                        }
                        return response;
                    }
                },
                pageSize: 20
            };

            kendo.ui.progress($("#gridphieudieuchinh"), false);
        });
    }
    function savegrid(e) {
        var grid = e.sender;
        var id = grid.element[0].id;
        var model = e.model;
        e.preventDefault();

        //Thay đổi số lượng
        if (e.values.soLuong == null) {
            model.soLuong = 0;
            model.soLuongDieuChinh = model.soLuongPhieuNhap - model.soLuong;
            model.loaiDieuChinh = 2;
            model.tenLoaiDieuChinh = $.i18n("label_dieuchinhgiam"),
            grid.refresh();
        }
        else {
            model.soLuong = e.values.soLuong;
            if (model.soLuong >= model.soLuongPhieuNhap) {
                model.soLuongDieuChinh = model.soLuong - model.soLuongPhieuNhap;
                model.loaiDieuChinh = 1;
                model.tenLoaiDieuChinh = $.i18n("label_dieuchinhtang");
            } else {
                model.soLuongDieuChinh = model.soLuongPhieuNhap - model.soLuong;
                model.loaiDieuChinh = 2;
                model.tenLoaiDieuChinh = $.i18n("label_dieuchinhgiam");
            }

            grid.refresh();
        }
    }

    function openedit(id) {
        $scope.formdetail.center().maximize().open();

        height = $(window).height() - ($(".toolbarmenu").height() + $(".body-detail").height());

        var grid = $("#grid").data("kendoGrid");
        dataPhieu = grid.dataItem(grid.select());

        $scope.iD_PhieuNhap = dataPhieu.iD_PhieuNhap;
        $scope.iD_PhieuDieuChinhNhapKho = dataPhieu.iD_PhieuDieuChinhNhapKho;

        $scope.ghichu = dataPhieu.dienGiai;
        $scope.nhanvien = $rootScope.UserInfo.tenAdmin;
        $scope.obj_ngaydieuchinh = dataPhieu.createdDate;
        $scope.ngaydieuchinh.readonly(true);

        $scope.phieunhapData = [{ iD_PhieuNhap: dataPhieu.iD_PhieuNhap, tenPhieuNhap: dataPhieu.tenPhieuNhap }];
        $scope.phieunhapselect = [];

        let phieunhap = $("#phieunhap").data("kendoComboBox");
        $timeout(function () {
            phieunhap.value(dataPhieu.iD_PhieuNhap);
            phieunhap.readonly(true);
        }, 200);
        
        loadgridphieunhap();
        loadgridphieudieuchinh();
    }
    function openadd() {
        $scope.formdetail.center().maximize().open();

        height = $(window).height() - ($(".toolbarmenu").height() + $(".body-detail").height());

        $scope.iD_PhieuDieuChinhNhapKho = 0;
        $scope.iD_PhieuNhap = 0;

        $scope.ghichu = "";
        $scope.nhanvien = $rootScope.UserInfo.tenAdmin;
        $scope.obj_ngaydieuchinh = new Date();
        $scope.ngaydieuchinh.readonly(true);

        let phieunhap = $("#phieunhap").data("kendoComboBox");
        phieunhap.value("");
        phieunhap.readonly(false);

        loadphieunhap();

        loadgridphieunhap();
        loadgridphieudieuchinh();
    }
    function openFormDetail(idphieudieuchinh) {
        $scope.object = { PhieuDieuChinh: {}, ChiTietDieuChinh: [] };
        $scope.iD_PhieuDieuChinhNhapKho = 0;
        $scope.iD_PhieuNhap = 0;
        $scope.idphieu = idphieudieuchinh;

        if (idphieudieuchinh > 0)
            openedit(idphieudieuchinh);
        else
            openadd();
    }
    function validationOpenDetail(listRowsSelected) {
        let flag = true;
        let msg = "";
        if (listRowsSelected.length > 1) {
            msg = $.i18n("label_canchonmotdongdethuchien");
            flag = false;
        } else if (listRowsSelected.length == 0) {
            flag = false;
            msg = $.i18n("label_chuachondongdethuchien");
        }
        if (!flag) {
            Notification({ title: $.i18n("label_thongbao"), message: msg }, "error");
        }
        return flag;
    }

    function validate() {
        let flag = true;
        let msg = '';

        let myGridAdd = $("#gridphieudieuchinh").data("kendoGrid");
        let dataSource = myGridAdd.dataSource.data();
        let DataPhieu = {
            ID_PhieuDieuChinhNhapKho: $scope.iD_PhieuDieuChinhNhapKho,
            ID_PhieuNhap: $scope.iD_PhieuNhap,
            DienGiai: $scope.ghichu,
        };
        let DataChiTiet = [];

        for (i = 0; i < dataSource.length; i++) {
            var obj = {
                ID_PhieuDieuChinhNhapChiTiet: dataSource[i].iD_PhieuDieuChinhNhapChiTiet,
                ID_PhieuDieuChinhNhap: $scope.iD_PhieuDieuChinhNhapKho,
                ID_ChiTietPhieuNhap: dataSource[i].iD_ChiTietPhieuNhap,
                ID_HangHoa: dataSource[i].iD_HangHoa,
                SoLuong: dataSource[i].soLuong,
                SoLuongDieuChinh: dataSource[i].soLuongDieuChinh,
                LoaiDieuChinh: dataSource[i].loaiDieuChinh
            }
            DataChiTiet.push(obj);
        }

        $scope.object = { PhieuDieuChinh: DataPhieu, ChiTietDieuChinh: DataChiTiet };

        if ($scope.iD_PhieuNhap <= 0 && flag) {
            flag = false;
            msg = $.i18n("label_chuachonphieunhap");
        }

        if (dataSource.length <= 0 && flag) {
            flag = false;
            msg = $.i18n("label_chuanhapthongtinmathangdieuchinh");
        }

        if (!flag)
            Notification({ title: $.i18n('label_thongbao'), message: msg }, 'warning');

        return flag;
    }

    //event
    $scope.phieunhapOnChange = function () {
        $scope.phieunhapselect = this.phieunhapselect;

        if ($scope.phieunhapselect != undefined)
            $scope.iD_PhieuNhap = ($scope.phieunhapselect.iD_PhieuNhap < 0) ? 0 : $scope.phieunhapselect.iD_PhieuNhap;

        loadgridphieunhap();
        loadgridphieudieuchinh();
    }
    $scope.deleterow = function (e) {
        let myGrid = $('#gridphieudieuchinh').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;

        let myGridPhieuNhap = $("#gridphieunhap").data("kendoGrid");
        let item = {
            iD_ChiTietPhieuNhap: dataItem.iD_ChiTietPhieuNhap,
            iD_HangHoa: dataItem.iD_HangHoa,
            maHang: dataItem.maHang,
            tenHang: dataItem.tenHang,
            soLuong: dataItem.soLuongPhieuNhap
        }
        myGridPhieuNhap.dataSource.add(item);

        myGrid.dataSource.remove(dataItem);

    }

    $scope.themphieudieuchinh = function () {
        openFormDetail(0);
    }
    $scope.suaphieudieuchinh = function () {
        let listRowsSelected = commonGetRowSelected("#grid");
        if (validationOpenDetail(listRowsSelected)) {
            openFormDetail(listRowsSelected[0].iD_PhieuDieuChinhNhapKho);
        }
    }

    $scope.luuphieudieuchinh = function () {
        if (validate()) {
            commonOpenLoadingText("#btn_luuphieudieuchinh");
            khoHangDataService.themsuaphieudieuchinh($scope.object).then(function (result) {
                if (result.flag) {
                    $scope.formdetail.center().close();
                    $scope.object = { PhieuDieuChinh: {}, ChiTietDieuChinh: [] };
                    $scope.iD_PhieuDieuChinhNhapKho = 0;
                    $scope.iD_PhieuNhap = 0;
                    loadgrid();

                    Notification({ title: $.i18n('label_thongbao'), message: result.message }, 'success');
                }
                else
                    Notification({ title: $.i18n('label_thongbao'), message: result.message }, 'warning');

                commonCloseLoadingText("#btn_luuphieudieuchinh")
            })
        }
    }
    $scope.huyluuphieudieuchinh = function () {
        $scope.formdetail.center().close();
    }

    init();

    $("#grid").on("dblclick", "tr[role='row']", function () {
        let grid = $("#grid").data("kendoGrid");
        let row = grid.table.find("[data-uid=" + $(this).attr("data-uid") + "]");

        grid.clearSelection();
        grid.select(row);

        let selectedItem = grid.dataItem(grid.select());

        openFormDetail(selectedItem.iD_PhieuDieuChinhNhapKho);
    })

})