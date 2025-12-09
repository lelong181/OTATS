angular.module('app').controller('khuyenMaiController', function ($rootScope, $scope, $state, $location, ComboboxDataService, Notification, khuyenMaiDataService) {
    CreateSiteMap();

    function init() {
        getquyen();

        loadgrid();
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

        dataList.push({ headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { class: "text-center" }, selectable: true, width: "40px" });
        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({
            field: "tenCTKM", title: $.i18n("header_tenctkm"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "250px"
        });
        dataList.push({
            field: "ngayApDung",
            title: $.i18n("header_ngayapdung"),
            template: function (dataItem) {
                if (dataItem.ngayApDung != null && dataItem.ngayApDung.getFullYear() > 1900)
                    return kendo.htmlEncode(kendo.toString(dataItem.ngayApDung, formatDate));
                else
                    return '';
            },
            attributes: { style: "text-align: center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        });
        dataList.push({
            field: "ngayKetThuc", title: $.i18n("header_ngayketthuc"),
            template: function (dataItem) {
                if (dataItem.ngayKetThuc != null && dataItem.ngayKetThuc.getFullYear() > 1900)
                    return kendo.htmlEncode(kendo.toString(dataItem.ngayKetThuc, formatDate));
                else
                    return '';
            },
            attributes: { style: "text-align: center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        });
        dataList.push({ field: "tenHinhThucKM", title: $.i18n("header_loai"),headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px" });
        dataList.push({ field: "ghiChu", title: $.i18n("label_ghichu"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({
            field: "ngayTao", title: $.i18n("header_ngaytao"), 
            template: function (dataItem) {
                if (dataItem.ngayTao != null && dataItem.ngayTao.getFullYear() > 1900)
                    return kendo.htmlEncode(kendo.toString(dataItem.ngayTao, formatDateTime));
                else
                    return '';
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "trangThai_Text", title: $.i18n("header_trangthai"),  
            template: function (dataItem) {
                if (dataItem.trangThai > 0)
                    return '<strong class="color-success">' + kendo.htmlEncode(dataItem.trangThai_Text) + '</strong>';
                else
                    return '<strong class="color-danger">' + kendo.htmlEncode(dataItem.trangThai_Text) + '</strong>';
            },
            attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px"
        });

        return dataList;
    }
    function loadgrid() {
        $scope.gridOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".toolbarmenu").height());
                return heightGrid - 40;
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
            selectable: "row, multi",
            change: onChange,
            pageable: $scope.lang == 'vi-vn' ? pageableShort_vi : pageableShort_en,
            columns: listColumnsgrid()
        };

        khuyenMaiDataService.getlist().then(function (response) {
            $scope.gridData = {
                data: response.data,
                schema: {
                    model: {
                        id: 'iD_CTKM',
                        fields: {
                            iD_CTKM: {
                                type: "number"
                            },
                            ngayApDung: {
                                type: "date"
                            },
                            ngayKetThuc: {
                                type: "date"
                            },
                            ngayTao: {
                                type: "date"
                            }
                        }
                    }
                },
                pageSize: 20
            };
        })
    }
    function onChange(arg) {
        let listid = []

        let grid = $("#grid").data("kendoGrid");
        grid.select().each(function () {
            let dataItem = grid.dataItem(this);
            listid.push({ iD_CTKM: dataItem.iD_CTKM });
        });

        if (listid.length == 1) {
            let iD_CTKM = listid[0].iD_CTKM;
            loadchitiet(iD_CTKM);
        } else {
            loadchitiet(-1);
        }
    }

    function listColumnsgriddonhang() {
        let dataList = [];

        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({ field: "maThamChieu", title: $.i18n('header_madonhang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({ field: "tenKhachHang", title: $.i18n('header_khachhang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({ field: "tenNhanVien", title: $.i18n('header_nhanvienlap'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({
            field: "chietKhauPhanTram", title: $.i18n('label_chietkhauphantram'),
            format: formatNumberInGrid('n2'),
            attributes: { style: "text-align: center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "chietKhauTien", title: $.i18n('label_chietkhautien'),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            attributes: { style: "text-align: center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "tongTienChietKhau", title: $.i18n('label_tongchietkhau'),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            attributes: { style: "text-align: center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "tongTien", title: $.i18n('label_tongtien'),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            attributes: { style: "text-align: center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });

        return dataList;
    }
    function listColumnsgridchitiet() {
        let dataList = [];

        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({ field: "maHang", title: $.i18n('header_mahang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({ field: "tenMatHang", title: $.i18n('header_tenhang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({
            field: "chietKhauPhanTram_BanLe", title: $.i18n('header_chietkhauphantrambanle'),
            format: formatNumberInGrid('n2'),
            attributes: { style: "text-align: center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "chietKhauTien_BanLe", title: $.i18n('header_chietkhautienbanle'),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            attributes: { style: "text-align: center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "chietKhauPhanTram_BanBuon", title: $.i18n('header_chietkhauphantrambanbuon'),
            format: formatNumberInGrid('n2'),
            attributes: { style: "text-align: center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "chietKhauTien_BanBuon", title: $.i18n('header_chietkhautienbanbuon'),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            attributes: { style: "text-align: center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });

        return dataList;
    }
    function loadchitiet(_idctkm) {
        kendo.ui.progress($("#gridchitiet"), false);
        kendo.ui.progress($("#griddonhang"), false);
        $scope.gridchitietOptions = {
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
            columns: listColumnsgridchitiet()
        };
        $scope.griddonhangOptions = {
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
            columns: listColumnsgriddonhang()
        };

        khuyenMaiDataService.getdonhangkhuyenmai(_idctkm).then(function (response) {
            $scope.gridchitietData = {
                data: response.data.chitiet,
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
            $scope.griddonhangData = {
                data: response.data.donhang,
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

            kendo.ui.progress($("#gridchitiet"), false);
            kendo.ui.progress($("#griddonhang"), false);
        });
    }

    function openFormDetail(id) {
        $state.go('editkhuyenmai', { idkhuyenmai: id });
    }
    function validationOpenDetail(listRowsSelected) {
        let flag = true;
        let msg = "";
        if (listRowsSelected.length > 1) {
            msg = $.i18n('label_canchonmotdongdethuchien');
            flag = false;
        } else if (listRowsSelected.length == 0) {
            flag = false;
            msg = $.i18n('label_chuachondongdethuchien');
        }
        if (!flag) {
            Notification({ title: $.i18n('label_thongbao'), message: msg }, "error");
        }
        return flag;
    }

    function openConfirm(message, acceptAction, cancelAction, data) {
        var scope = angular.element("#mainContentId").scope();
        $(" <div id='confirmDelete'></div>").appendTo("body").kendoDialog({
            width: "450px",
            closable: true,
            modal: true,
            title: $.i18n("label_xacnhan"),
            content: message,
            actions: [
                {
                    text: $.i18n("button_dong"), primary: false, action: function () {
                        if (cancelAction != null) {
                            scope[cancelAction](data);
                        }
                    }
                },
                {
                    text: $.i18n("button_dongy"), primary: true, action: function () {
                        scope[acceptAction](data);
                    }
                }
            ],
        })
    }

    //event
    $scope.themkhuyenmai = function () {
        openFormDetail(0);
    }
    $scope.suakhuyenmai = function () {
        let listRowsSelected = commonGetRowSelected("#grid");
        if (validationOpenDetail(listRowsSelected)) {
            openFormDetail(listRowsSelected[0].iD_CTKM);
        }
    }
    $scope.tamdungkhuyenmai = function () {
        let listRowsSelected = commonGetRowSelected("#grid");
        if (validationOpenDetail(listRowsSelected)) {
            if (listRowsSelected[0].trangThai > 0)
                openConfirm($.i18n('label_bancochacchanmuontamdungchuongtrinhkhuyenmaikhong'), 'apdungtamdungkhuyenmai', null, listRowsSelected[0].iD_CTKM);
            else
                openConfirm($.i18n('label_bancochacchanmuonmolaichuongtrinhkhuyenmaikhong'), 'apdungtamdungkhuyenmai', null, listRowsSelected[0].iD_CTKM);
        }
    }
    $scope.apdungtamdungkhuyenmai = function (_idctkm) {
        khuyenMaiDataService.ngungsudung(_idctkm).then(function (result) {
            if (result.flag) {
                loadgrid();
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
            }
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
        })
    }

    $scope.deletekhuyenmai = function () {
        let listRowsSelected = commonGetRowSelected("#grid");
        if (validationOpenDetail(listRowsSelected)) {
            openConfirm($.i18n('label_bancochacchanxoa'), 'apdungdeletekhuyenmai', null, listRowsSelected[0].iD_CTKM);
        }
    }
    $scope.apdungdeletekhuyenmai = function (_idctkm) {
        khuyenMaiDataService.xoachuongtrinhkhuyenmai(_idctkm).then(function (result) {
            if (result.flag) {
                loadgrid();
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
            }
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
        })
    }

    $scope.xuatexcelkhuyenmai = function () {
        commonOpenLoadingText("#btn_xuatexcel");

        khuyenMaiDataService.exportExcel().then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayra_loadlaitrang') }, 'warning');

            commonCloseLoadingText("#btn_xuatexcel");
        });
    }

    init();

    $("#grid").on("dblclick", "tr[role='row']", function () {
        let grid = $("#grid").data("kendoGrid");
        let row = grid.table.find("[data-uid=" + $(this).attr("data-uid") + "]");

        grid.clearSelection();
        grid.select(row);

        let selectedItem = grid.dataItem(grid.select());

        openFormDetail(selectedItem.iD_CTKM);
    })

})