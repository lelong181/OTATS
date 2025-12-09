angular.module('app').controller('donHangHDVController', function ($rootScope, $scope, $location, $state, $stateParams, $timeout, Notification, ComboboxDataService, donHangDataService) {
    CreateSiteMap();

    let param_from = '';
    let param_to = '';

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
    function init() {
        initdate();

        let data = {
            from: kendo.toString($scope.obj_TuNgay, formatDateTimeFilter),
            to: kendo.toString($scope.obj_DenNgay, formatDateTimeFilter),
        }
        loadgrid(data);
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

        dataList.push({ headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, selectable: true, width: "40px" });
        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({ field: "ngayLap", format: "{0:dd/MM/yyyy HH:mm}", title: $.i18n("header_ngaylap"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: true, width: "150px" });
        dataList.push({
            field: "maThamChieu", title: $.i18n("header_madonhang"),
            template: function (dataItem) {
                return "<a href='' class='color-primary' ng-click='openDetailFromGrid(" + kendo.htmlEncode(dataItem.iD_DonHang) + ")'>" + kendo.htmlEncode(dataItem.maThamChieu) + "</a>";
            },
            footerTemplate: $.i18n("header_tongtien"),
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px"
        });
        dataList.push({ field: "tenKhachHang", title: $.i18n("header_tenkhachhang"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px" });
        dataList.push({
            field: "tenTrangThaiDongHang", title: $.i18n("header_trangthaidonhang"),
            template: function (e) {
                return e.tenTrangThaiDongHang;
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px"
        });
        dataList.push({ field: "tenNhanVien", title: $.i18n("header_tennhanvien"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({ field: "ghiChu", title: $.i18n("header_ghichu"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px" });
        dataList.push({
            field: "tongTien", title: $.i18n("header_tongtien"),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), footerTemplate: formatNumberInFooterGrid('tongTien.sum', $rootScope.UserInfo.dinhDangSo),
            attributes: { style: "text-align: right" }, footerAttributes: { style: "text-align: right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: false, width: "120px"
        });
        //dataList.push({
        //    field: "tienDaThanhToan", title: $.i18n("header_dathanhtoan"),
        //    format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), footerTemplate: formatNumberInFooterGrid('tienDaThanhToan.sum', $rootScope.UserInfo.dinhDangSo),
        //    attributes: { style: "text-align: right" }, footerAttributes: { style: "text-align: right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
        //    filterable: false, width: "120px"
        //});
        //dataList.push({
        //    field: "conLai", title: $.i18n("header_conlai"),
        //    format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), footerTemplate: formatNumberInFooterGrid('conLai.sum', $rootScope.UserInfo.dinhDangSo),
        //    attributes: { style: "text-align: right" }, footerAttributes: { style: "text-align: right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
        //    filterable: false, width: "120px"
        //});
        
        dataList.push({ field: "ngayXuatVe", format: "{0:dd/MM/yyyy}", title: "Ngày xuất vé", headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: true, width: "150px" });

        dataList.push({
            field: "isProcess_Name", title: $.i18n("header_trangthaihoantat"),
            template: function (dataItem) {
                return "<span> <i class='fas fa-circle fas-sm' ng-class='{warning: dataItem.isProcess == 0, primary: dataItem.isProcess == 1, danger: dataItem.isProcess == 2}'></i> " + kendo.htmlEncode(dataItem.isProcess_Name) + "</span>";
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px"
        });
        return dataList;
    }
    function loadgrid(_data) {
        kendo.ui.progress($("#grid"), true);
        commonOpenLoadingText("#btn_loaddonghang");
 
        donHangDataService.getlistbyhdv(_data).then(function (result) {
            $scope.gridData = {
                data: result.data,
                schema: {
                    model: {
                        id: 'iD_DonHang',
                        fields: {
                            ngayLap: {
                                type:"date"
                            },
                            ngayXuatVe: {
                                type: "date"
                            },
                            iD_DonHang: {
                                type: "number"
                            },
                            tongTien: {
                                type: "number"
                            },
                            tienDaThanhToan: {
                                type: "number"
                            },
                            conLai: {
                                type: "number"
                            }
                        }
                    }
                },
                pageSize: 20,
                aggregate: [
                  { field: "tongTien", aggregate: "sum" },
                  { field: "tienDaThanhToan", aggregate: "sum" },
                  { field: "conLai", aggregate: "sum" }
                ]
            };
            $scope.gridOptions = {
                sortable: true,
                height: function () {
                    var heightGrid = $(window).height() - ($(".sitemap").height() + $(".content-filter").height() + $(".toolbarmenu").height());
                    if (heightGrid > 400)
                        return heightGrid - 100;
                    else
                        return 400;
                },
                dataBinding: function () {
                    RecordNumber = (this.dataSource.page() - 1) * this.dataSource.pageSize();
                },
                excel: {
                    allPages: true
                },
                resizable: true,
                editable: false,
                filterable: {
                    mode: "row"
                },
                pageable: $scope.lang == 'vi-vn' ? pageableShort_vi : pageableShort_en,
                columns: listColumnsgrid()
            };
            kendo.ui.progress($("#grid"), false);
            commonCloseLoadingText("#btn_loaddonghang");
        });
    }

    function openFormDetail(_id) {

        let url = $state.href('editdonhanghdv', { iddonhang: _id });
        window.open(url, '_blank');

        //$state.go('editdonhang', { iddonhang: _id });
    }
    function validationOpenDetail(_listRowsSelected) {
        let flag = true;
        let msg = "";
        if (_listRowsSelected.length > 1) {
            msg = $.i18n("label_canchonmotdonhangdethuchien");
            flag = false;
        } else if (_listRowsSelected.length == 0) {
            flag = false;
            msg = $.i18n("label_chuachondonhangdethuchien");
        }
        if (!flag) {
            Notification({ title: $.i18n('label_thongbao'), message: msg }, "error");
        }
        return flag;
    }

    function getdataparam() {

        let dataSource = $("#grid").data("kendoGrid").dataSource;

        let data = {
            from: kendo.toString($scope.obj_TuNgay, formatDateTimeFilter),
            to: kendo.toString($scope.obj_DenNgay, formatDateTimeFilter),
        }

        return data;
    }

    //even
    $scope.fromDateChanged = function () {
        $scope.minDate = $scope.obj_TuNgay;
    };

    $scope.toDateChanged = function () {
        $scope.maxDate = $scope.obj_DenNgay;
    };

    $scope.loaddonhang = function () {
        let data = getdataparam();

        loadgrid(data);
    }
    
    $scope.suadonhang = function () {
        let listRowsSelected = commonGetRowSelected("#grid");
        if (validationOpenDetail(listRowsSelected)) {
            openFormDetail(listRowsSelected[0].iD_DonHang);
        }
    }
    $scope.xuatexcel = function () {
        $("#grid").data("kendoGrid").saveAsExcel();
    }
    $scope.xuatexcelchitiet = function () {
        commonOpenLoadingText("#btn_xuatexcelchitietdonhang");
        let data = getdataparam();

        donHangDataService.exportExcelDetail(data).then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayravuilongloadlaitrang') }, 'warning');
            commonCloseLoadingText("#btn_xuatexcelchitietdonhang");
        });
    }

    $scope.openDetailFromGrid = function (iD_DonHang) {
        openFormDetail(iD_DonHang);
    }

    init();

    $("#grid").on("dblclick", "tr[role='row']", function () {
        let grid = $("#grid").data("kendoGrid");
        let row = grid.table.find("[data-uid=" + $(this).attr("data-uid") + "]");

        grid.clearSelection();
        grid.select(row);

        let selectedItem = grid.dataItem(grid.select());

        openFormDetail(selectedItem.iD_DonHang);
    })

})