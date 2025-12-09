angular.module('app').controller('tongHopDonHangController', function ($rootScope, $scope, Notification, baoCaoDonHangDataService) {
    CreateSiteMap();

    function init() {
        initdate();
        loadgrid();
    }
    function initdate() {
        var dateNow = new Date();
        $scope.obj_TuNgay = new Date(dateNow.setHours(0, 0, 0));
        $scope.obj_DenNgay = new Date(dateNow.setHours(23, 59, 59));
        $scope.maxDate = $scope.obj_DenNgay;
        $scope.minDate = $scope.obj_TuNgay;
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
            field: "createDate", title: $.i18n("header_ngay"),
            template: function (dataItem) {
                return kendo.htmlEncode(kendo.toString(dataItem.createDate, formatDate));
            },
            attributes: {
                "class": "table-cell",
                style: "text-align: center"
            }, footerTemplate: $.i18n("label_tong"),
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "tongSoDon", title: $.i18n("header_tongsodonhang"),
            attributes: {
                "class": "table-cell",
                style: "text-align: center"
            },
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('tongSoDon.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "chuaHoanTat", title: $.i18n("header_chuahoantat"),
            attributes: {
                "class": "table-cell",
                style: "text-align: center"
            }, format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('chuaHoanTat.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "daHoanTat", title: $.i18n("header_dahoantat"),
            attributes: {
                "class": "table-cell",
                style: "text-align: center"
            }, format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('daHoanTat.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "huy", title: $.i18n("header_huy"),
            attributes: {
                "class": "table-cell",
                style: "text-align: center"
            }, format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('huy.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "chuaThanhToan", title: $.i18n("header_chuathanhtoan"),
            attributes: {
                "class": "table-cell",
                style: "text-align: center"
            }, format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('chuaThanhToan.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "thanhToan1Phan", title: $.i18n("header_thanhtoanmotphan"),
            attributes: {
                "class": "table-cell",
                style: "text-align: center"
            }, format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('thanhToan1Phan.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "daThanhToan", title: $.i18n("header_dathanhtoan"),
            attributes: {
                "class": "table-cell",
                style: "text-align: center"
            }, format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('daThanhToan.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "chuaGiaoHang", title: $.i18n("header_chuagiaohang"),
            attributes: {
                "class": "table-cell",
                style: "text-align: center"
            }, format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('chuaGiaoHang.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "giaoHang1Phan", title: $.i18n("header_giaohangmotphan"),
            attributes: {
                "class": "table-cell",
                style: "text-align: center"
            }, format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('giaoHang1Phan.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "daGiaoHang", title: $.i18n("header_dagiaohang"),
            attributes: {
                "class": "table-cell",
                style: "text-align: center"
            }, format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('daGiaoHang.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "tongTien", title: $.i18n("header_tongtien"),
            attributes: {
                "class": "table-cell",
                style: "text-align: right"
            }, format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('tongTien.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: right" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "tienDaThanhToan", title: $.i18n("header_tiendathanhtoan"),
            attributes: {
                "class": "table-cell",
                style: "text-align: right"
            }, format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('tienDaThanhToan.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: right" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "conLai", title: $.i18n("header_conlai"),
            attributes: {
                "class": "table-cell",
                style: "text-align: right"
            }, format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('conLai.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: right" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        return dataList;
    }
    function loadgrid() {
        kendo.ui.progress($("#grid"), true);
        commonOpenLoadingText("#btn_xemBaoCao");
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
        baoCaoDonHangDataService.getTongHopDonHang(fromdate, todate).then(function (result) {
            $scope.gridData = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                            createDate: {
                                type: "date"
                            },
                            
                        }
                    }
                },
                pageSize: 20,
                aggregate: [
               { field: "tongSoDon", aggregate: "sum" },
               { field: "daHoanTat", aggregate: "sum" },
               { field: "chuaHoanTat", aggregate: "sum" },
               { field: "huy", aggregate: "sum" },
               { field: "chuaThanhToan", aggregate: "sum" },
               { field: "thanhToan1Phan", aggregate: "sum" },
               { field: "daThanhToan", aggregate: "sum" },
               { field: "chuaGiaoHang", aggregate: "sum" },
               { field: "giaoHang1Phan", aggregate: "sum" },
               { field: "daGiaoHang", aggregate: "sum" },
               { field: "tongTien", aggregate: "sum" },
               { field: "tienDaThanhToan", aggregate: "sum" },
               { field: "conLai", aggregate: "sum" }
                ]
            };
            kendo.ui.progress($("#grid"), false);
            commonCloseLoadingText("#btn_xemBaoCao")
        });
    }
    //event
    $scope.xemBaoCao = function () {
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        loadgrid();
    }
    $scope.XuatExcel = function () {
        commonOpenLoadingText("#btn_XuatExcel");
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        baoCaoDonHangDataService.getExcelTongHopDonHang(fromdate, todate).then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n("label_thongbao"), message: $.i18n("label_coloixayra_loadlaitrang") }, 'warning');

            commonCloseLoadingText("#btn_XuatExcel")
        });
    }
    $scope.fromDateChanged = function () {
        $scope.minDate = $scope.obj_TuNgay;
    }
    $scope.toDateChanged = function () {
        $scope.maxDate = $scope.obj_DenNgay;
    }
    init();

})
