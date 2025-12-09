angular.module('app').controller('bm017_BaoCaoDonHangTheoDiemController', function ($state,$rootScope, $scope, Notification, ComboboxDataService, baoCaoNhanVienDataService) {
    CreateSiteMap();

    function init() {
        initdate();
        initcombo();
        loadgrid(0, 0, 0);
    }
    function initdate() {
        var dateNow = new Date();
        $scope.obj_TuNgay = new Date(dateNow.setHours(0, 0, 0));
        $scope.obj_DenNgay = new Date(dateNow.setHours(23, 59, 59));
        $scope.maxDate = $scope.obj_DenNgay;
        $scope.minDate = $scope.obj_TuNgay;
    }
    function initcombo() {
        ComboboxDataService.getDataNhanVien().then(function (result) {
            $scope.nhanvienData = result.data;
            if (!result.flag) {
                Notification({ title: $.i18n("label_thongbao"), message: $.i18n("label_khongtheloaddanhsachnhanvienvuilongtailaitrang") }, 'warning');
            }
        });
    }
    function listColumnsgrid() {
        var dataList = [];

        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({
            field: "tenNhanVien", title: $.i18n("header_tennhanvien"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px"
        });
        dataList.push({
            title: $.i18n("header_luotvaora"), attributes: { style: "text-align: center" },
            template: function (dataItem) {
                return "<a href='' class='color-primary' ng-click='openDetailFromGrid(" + kendo.htmlEncode(dataItem.maNhanVien) + ',' + kendo.htmlEncode(dataItem.maKhachHang) +")'>" + (kendo.htmlEncode(dataItem.luotVao) + '/' + kendo.htmlEncode(dataItem.luotRa)) + "</a>"
                 ;
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "tongKhachHang", title: $.i18n("header_tongkhachhangviengtham"), template: function (dataItem) {
                return "<a href='' class='color-primary' ng-click='openDetailFromGrid(" + kendo.htmlEncode(dataItem.maNhanVien) + ',' + kendo.htmlEncode(dataItem.maKhachHang) + ")'>" + kendo.htmlEncode(dataItem.tongKhachHang) + "</a>";
            }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "", title: $.i18n("header_donhangdalap"), template: function (dataItem) {
                return "<a href='' class='color-primary' ng-click='openFormDsDonHang(" + kendo.htmlEncode(dataItem.maNhanVien) + ")'>" + kendo.htmlEncode(dataItem.soDonVao + dataItem.soDonKhongVao) + "</a>";
            }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "soDonVao", title: $.i18n("header_tongdonhangcuakhachhangdaden"),  template: function (dataItem) {
                return "<a href='' class='color-primary' ng-click='openFormDsDonHang(" + kendo.htmlEncode(dataItem.maNhanVien) + ")'>" + kendo.htmlEncode(dataItem.soDonVao) + "</a>";
            },
            attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "soDonKhongVao", title: $.i18n("header_tongdonhangcuakhachhangchuaden"),template: function (dataItem) {
                return "<a href='' class='color-primary' ng-click='openFormDsDonHang(" + kendo.htmlEncode(dataItem.maNhanVien) + ")'>" + kendo.htmlEncode(dataItem.soDonKhongVao) + "</a>";
            }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            title: $.i18n("header_chitiet"), template: function (dataItem) {
            return "<a class='btn btn-link btn-menubar' ng-click='tacVu(" + kendo.htmlEncode(dataItem.maNhanVien) + ")' >"+ "<i class='fas fa-clipboard-list fas-md color-primary'></i>" +"</a>";
            }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "70px"
        });

        return dataList;
    }
    function listColumnsgriddetail() {
        var dataList = [];

        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({
            field: "ngay", title: $.i18n("header_ngay"), template: function (dataItem) {
                d = new Date(dataItem.ngay);
                if (dataItem.ngay == null)
                    return '';
                else
                    return kendo.htmlEncode(kendo.toString(d, formatDate));
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px"
        });
        dataList.push({
            title: $.i18n("header_luotvaora"),attributes: { style: "text-align: center" },
            template: function (dataItem) {
                return "<a href='' class='color-primary' ng-click='openDetailFromGrid(" + kendo.htmlEncode(dataItem.maNhanVien) + ',' + kendo.htmlEncode(dataItem.maKhachHang) + ")'>" + (kendo.htmlEncode(dataItem.luotVao) + '/' + kendo.htmlEncode(dataItem.luotRa)) + "</a>"
                    ;
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "tongKhachHang", title: $.i18n("header_tongkhachhangviengtham"), template: function (dataItem) {
                return "<a href='' class='color-primary' ng-click='openDetailFromGrid(" + kendo.htmlEncode(dataItem.maNhanVien) + ',' + kendo.htmlEncode(dataItem.maKhachHang) + ")'>" + kendo.htmlEncode(dataItem.tongKhachHang) + "</a>";
            }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "", title: $.i18n("header_donhangdalap"), template: function (dataItem) {
                return "<a href='' class='color-primary' ng-click='openFormDsDonHang(" + kendo.htmlEncode(dataItem.maNhanVien) + ")'>" + kendo.htmlEncode(dataItem.soDonVao + dataItem.soDonKhongVao) + "</a>";
            }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "soDonVao", title: $.i18n("header_tongdonhangcuakhachhangdaden"), template: function (dataItem) {
                return "<a href='' class='color-primary' ng-click='openFormDsDonHang(" + kendo.htmlEncode(dataItem.maNhanVien) + ")'>" + kendo.htmlEncode(dataItem.soDonVao) + "</a>";
            },
            attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "soDonKhongVao", title: $.i18n("header_tongdonhangcuakhachhangchuaden"), template: function (dataItem) {
                return "<a href='' class='color-primary' ng-click='openFormDsDonHang(" + kendo.htmlEncode(dataItem.maNhanVien) + ")'>" + kendo.htmlEncode(dataItem.soDonKhongVao) + "</a>";
            }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            title: $.i18n("header_chitiet"), template: function (dataItem) {
                return '<button class="btn btn-link btn-menubar" ng-click="openFormDsDonHang()" ><i class="fas fa-clipboard-list fas-md color-primary"></i> </button>';
            }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "70px"
        });

        return dataList;
    }
    function loadgrid(idnhanvien) {
        kendo.ui.progress($("#grid"), true);
        commonOpenLoadingText("#btn_bm017xembaocao");
        $scope.gridOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".content-filter").height());
                return heightGrid - 20;
            },
            excelExport: function (e) {
                excelExport(e);
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

        baoCaoNhanVienDataService.getBaoCaoDonHangTheoDiem(idnhanvien, fromdate, todate).then(function (result) {
            $scope.gridData = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                            luotVao: {
                                type: "number"
                            },
                            luotRa: {
                                type: "number"
                            },
                            tongKhachHang: {
                                type: "number"
                            },
                            soLanViengTham: {
                                type: "number"
                            },
                            soDonVao: {
                                type: "number"
                            },
                            soDonKhongVao: {
                                type: "number"
                            },
                            ngay: {
                                type: "date"
                            },
                        }
                    }
                },
                pageSize: 20,
            };
            kendo.ui.progress($("#grid"), false);
            commonCloseLoadingText("#btn_bm017xembaocao")
        });

    }
    $scope.openDetailFromGrid = function (maNhanVien, maKhachHang) {
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        $state.go('lichsuvaoradiem', { idnhanvien: maNhanVien, idkhachhang: maKhachHang, from: fromdate, to: todate });
    }
    $scope.openFormDsDonHang = function (maNhanVien) {
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        $state.go('danhsachdonhang', { idnhanvien: maNhanVien, from: fromdate, to: todate });
    }
    $scope.tacVu = function () {
        $scope.formchitiet.center().maximize().open();
        $scope.griddetailOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height());
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
            columns: listColumnsgriddetail()
        };
        let myGrid = $('#grid').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;
        let idnhanvien = dataItem.maNhanVien;
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        baoCaoNhanVienDataService.getChiTietDonHangTheoDiem(idnhanvien, fromdate, todate).then(function (response) {
            $scope.griddetailData = {
                data: response.data,
                schema: {
                    model: {
                        fields: {
                            ngayHoanThanh: {
                                type: "date"
                            }
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
        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        
        loadgrid(idnhanvien);
    }
    $scope.XuatExcel = function () {
        commonOpenLoadingText("#btn_bm017xuatexcel");
        var idnhanvien = 0;
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        
        baoCaoNhanVienDataService.getExcelBaoCaoDonHangTheoDiem(idnhanvien, fromdate, todate).then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n("label_thongbao"), message: $.i18n("label_coloixayra_loadlaitrang") }, 'warning');

            commonCloseLoadingText("#btn_bm017xuatexcel")
        });
    }

    $scope.nhanvienOnChange = function () {
        $scope.nhanvienselect = this.nhanvienselect;
    }
    $scope.fromDateChanged = function () {
        $scope.minDate = $scope.obj_TuNgay;
    }
    $scope.toDateChanged = function () {
        $scope.maxDate = $scope.obj_DenNgay;
    }
    init();

})
