angular.module('app').controller('bm018_BaoCaoChuyenDoController', function ($state, $rootScope, $scope, Notification, ComboboxDataService, baoCaoNhanVienDataService) {
    CreateSiteMap();
   

    function init() {
        initdate();
        inittrangthai();
        loadgrid();

    }
    function initdate() {
        var dateNow = new Date();
        $scope.obj_TuNgay = new Date(dateNow.setHours(0, 0, 0));
        $scope.obj_DenNgay = new Date(dateNow.setHours(23, 59, 59));
        $scope.maxDate = $scope.obj_DenNgay;
        $scope.minDate = $scope.obj_TuNgay;
    }

    function inittrangthai() {
        $scope.listrangthai = new kendo.data.DataSource({
            data: [
                { Id: -1, trangthai: "Tất cả" },
                { Id: 0, trangthai: "Thẻ chưa thu" },
                { Id: 1, trangthai: "Thẻ đã thu" },

            ]
        });
        $scope.trangthaiSelect = { Id: -1, trangthai: "Tất cả" }
    }

    function listColumnsgrid() {
        var dataList = [];

        dataList.push({
            field: "maVeBoSung", title: "Mã thẻ đò", headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            template: function (dataItem) {
                return "<a href='' class='color-primary' ng-click='openThuTheDo(" + kendo.htmlEncode(dataItem.maVeBoSung) + ")'>" + kendo.htmlEncode(dataItem.maVeBoSung) + "</a>";
            },
            footerTemplate: function (e) {
                return "<b>Tổng số thẻ đò: " + kendo.toString(e.maVeBoSung.count, "N0") + "</b>"
            },
            footerAttributes: { style: "text-align: right" },
            filterable: defaultFilterableGrid, width: "140px"
        });
        dataList.push({
            field: "maThamChieu", title: "Mã OTA Booking",
            template: function (dataItem) {
                return "<a href='' class='color-primary' ng-click='openDetailFromGrid(" + kendo.htmlEncode(dataItem.iD_DonHang) + ")'>" + kendo.htmlEncode(dataItem.maThamChieu) + "</a>";
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "140px",
        });
        dataList.push({
            field: "maBookingDichVu", title: "Mã Booking", headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "140px"
        });
        dataList.push({
            field: "giaBanLe", title: "Giá tiền", headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "140px",
            format: "{0:N0}",
            attributes: { style: "text-align: right" },
            footerTemplate: function (e) {
                return "<b>Tổng tiền: " + kendo.toString(e.giaBanLe.sum, "N0") + "</b>"
            },
            footerAttributes: { style: "text-align: right" }
        });
        dataList.push({
            field: "tenTrangThai", title: "Trạng thái", headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "140px",
            template: function (e) {
                if (e.trangThai == 1) {
                    return "<b style='color:green'>" + e.tenTrangThai + "</b>"
                } else {
                    return "<b style='color:red'>" + e.tenTrangThai + "</b>"

                }
            }
        });
        dataList.push({
            field: "ngayTao", title: "Ngày bán thẻ", template: function (dataItem) {
                return kendo.htmlEncode(kendo.toString(dataItem.ngayTao, formatDateTime));
            }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "140px"
        });
        dataList.push({
            field: "tenNhanVien", title: "Người bán", headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "140px"
        });
        dataList.push({
            field: "ngayThuThe", title: "Ngày thu thẻ", template: function (dataItem) {
                if (dataItem.ngayThuThe)
                    return kendo.htmlEncode(kendo.toString(dataItem.ngayThuThe, formatDateTime));
                else
                    return "";
            }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "140px"
        });
        dataList.push({
            field: "tenNhanVienThuThe",
            template: function (dataItem) {
                if (dataItem.tenNhanVienThuThe)
                    return dataItem.tenNhanVienThuThe;
                else
                    return "";
            },
            title: "Người thu", headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "140px"
        });
        dataList.push({
            field: "donGia", title: "Đơn giá", headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "140px",
            format: "{0:N0}",
            footerTemplate: function (e) {
                return "<b>Tổng tiền: " + kendo.toString(e.donGia.sum, "N0") + "</b>"
            },
            attributes: { style: "text-align: right" },
            footerAttributes: { style: "text-align: right" }
        });

        return dataList;
    }
    function loadgrid() {
        kendo.ui.progress($("#grid"), true);
        commonOpenLoadingText("#btn_bm018xembaocao");
        $scope.gridOptions = {
            dataSource: {
                data: [],
                schema: {
                    model: {
                        id: 'maVeBoSung',
                        fields: {
                            ngayTao: {
                                type: "date"
                            },
                            ngayThuThe: {
                                type: "date"
                            },
                            giaBanLe: {
                                type: "number"
                            },
                            donGia: {
                                type: "number"
                            }
                        }
                    }
                },
                pageSize: 20,
                aggregate: [
                    { field: "maVeBoSung", aggregate: "count" },
                    { field: "giaBanLe", aggregate: "sum" },
                    { field: "donGia", aggregate: "sum" },
                ]

            },
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".content-filter").height());
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
            columns: listColumnsgrid()
        };

        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);

        baoCaoNhanVienDataService.getBaoCaoChuyenDo(fromdate, todate, $scope.trangthaiSelect.Id).then(function (result) {
            $scope.gridData = {
                data: result.data,
                schema: {
                    model: {
                        id: 'maVeBoSung',
                        fields: {
                            ngayTao: {
                                type: "date"
                            },
                            ngayThuThe: {
                                type: "date"
                            },
                            giaBanLe: {
                                type: "number"
                            },
                            donGia: {
                                type: "number"
                            }
                        }
                    }
                },
                pageSize: 20,
                aggregate: [
                    { field: "maVeBoSung", aggregate: "count" },
                    { field: "giaBanLe", aggregate: "sum" },
                    { field: "donGia", aggregate: "sum" },
                ]

            };

            kendo.ui.progress($("#grid"), false);
            commonCloseLoadingText("#btn_bm018xembaocao")
        });
    }

    function openFormDetail(_id) {

        let url = $state.href('editdonhang', { iddonhang: _id });
        window.open(url, '_blank');

        //$state.go('editdonhang', { iddonhang: _id });
    }

    function loadgridchitiet() {
        $scope.gridchitietOptions = {
            sortable: true,
            height: function () {
                return 350;
            },
            dataBinding: function () {
                RecordNumber = (this.dataSource.page() - 1) * this.dataSource.pageSize();
            },
            resizable: true,
            editable: true,
            filterable: {
                mode: "row"
            },
            pageable: $scope.lang == 'vi-vn' ? pageableShort_vi : pageableShort_en,
            update: function (e) {
                e.success();
            },
            columns: listColumnsgridchitiet()
        };

        $timeout(function () {
            $scope.gridchitietData = {
                data: [],
                schema: {
                    model: {
                        ID: 'id',
                        fields: {
                            xoa: {
                                type: "string",
                                editable: false,
                            },
                            mota: {
                                type: "string",
                            }
                        }
                    }
                },
                pageSize: 20,
                aggregate: [
                    { field: "tong", aggregate: "sum" }
                ]
            };
            $("#mathedo").focus();
        }, 500);

    }

    function listColumnsgridchitiet() {
        let dataList = [];

        dataList.push({
            field: "xoa", title: $.i18n('button_xoa'),
            template: '<button ng-click="deleterowgridchitiet()" class="btn btn-link btn-menubar" title ="' + $.i18n('button_xoa') + '" ><i class="fas fa-trash fas-sm color-danger"></i></button> ',
            attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "60px"
        });

        dataList.push({
            field: "mota", title: "Mã thẻ đò",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });

        return dataList;
    }

    $scope.trangthaiOnChange = function () {
        $scope.trangthaiSelect = this.trangthaiSelect;
        loadgrid();
    }

    $scope.openformchitiet = function () {

        $scope.formthuthe.center().open();
        //$("#formthuthe").data("kendoWindow").wrapper.find(".k-window-action").css("visibility", "hidden");
        loadgridchitiet();

    }

    $scope.themdongchitiet = function () {
        let myGridAdd = $("#gridchitiet").data("kendoGrid");
        let data = myGridAdd.dataSource.data();

        let obj = {
            id: 0,
            mota: ""
        }

        myGridAdd.dataSource.add(obj);
    }
    $scope.deleterowgridchitiet = function () {
        let myGrid = $('#gridchitiet').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;
        try {
            myGrid.dataSource.remove(dataItem);
        } catch (ex) { }
    }

    $scope.keyupMaTheDo = function (input) {
        if (event.key === 'Enter') {
            let dt = $("#grid").data("kendoGrid").dataSource.data();
            let myGridAdd = $("#gridchitiet").data("kendoGrid");
            let data = myGridAdd.dataSource.data();
            var existed = data.some(function (st) { return st.mota == $scope.mathedo });
            if (existed) {
                Notification({ title: $.i18n('label_thongbao'), message: "Thẻ này đã được nhập" }, 'warning');
            } else {
                let obj = {
                    id: 0,
                    mota: $scope.mathedo
                }
                myGridAdd.dataSource.add(obj);
            }
            $scope.mathedo = "";

        }
    }

    $scope.luuchitiet = function () {
        let grid = $("#grid").data("kendoGrid");
        $.each($("#gridchitiet").data("kendoGrid").dataSource.data(), function (index, item) {
            let thedo = grid.dataSource.get(item.mota);
            console.log(thedo);
            baoCaoNhanVienDataService.updateTrangThaiTheDo(thedo.iD_ChiTiet, 1).then(function (result) {
            })
        })
        $("#gridchitiet").data("kendoGrid").dataSource.data([]);
    }


    $scope.updateTrangThaiTheDo = function (trangthai) {
        baoCaoNhanVienDataService.updateTrangThaiTheDo($scope.thedo.id, trangthai).then(function (result) {
            $scope.xemBaoCao();
            $scope.formdetail.close();
        })
    }

    $scope.thuthedo = function () {
        openConfirm("Bạn có chắc chắn muốn thu lại thẻ đò?", 'updateTrangThaiTheDo', null, 1);
    }

    $scope.openThuTheDo = function (id) {
        let grid = $("#grid").data("kendoGrid");
        let item = grid.dataSource.get(id);
        $scope.thedo = {
            id: item.iD_ChiTiet,
            mathe: id,
            loaithe: "Thẻ đò",
            trangthai: item.trangThai,
            tenTrangThai: item.tenTrangThai,
            sotien: kendo.toString(item.giaBanLe, 'n0'),
            ngaymua: kendo.toString(item.ngayTao, 'dd/MM/yyyy HH:mm'),
            tongsothe: 1,
            tongtien: kendo.toString(item.giaBanLe, 'n0')
        };
        $scope.formdetail.center().open();
    }

    $scope.openDetailFromGrid = function (iD_DonHang) {
        openFormDetail(iD_DonHang);
    }

    $scope.xemBaoCao = function () {
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        loadgrid(fromdate, todate);
    }
    $scope.XuatExcel = function () {
        $("#grid").data("kendoGrid").saveAsExcel();
    }

    $scope.fromDateChanged = function () {
        $scope.minDate = $scope.obj_TuNgay;
    }
    $scope.toDateChanged = function () {
        $scope.maxDate = $scope.obj_DenNgay;
    }
    init();
})