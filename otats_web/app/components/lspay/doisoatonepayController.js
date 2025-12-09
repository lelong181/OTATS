angular.module('app').controller('doisoatonepayController', function ($rootScope, $scope, Notification, ComboboxDataService, lspayDataService) {
    CreateSiteMap();

    function init() {
        initdate();
        initcombo();
        loadgrid()
    }
    function initdate() {
        var dateNow = new Date();
        $scope.obj_TuNgay = new Date(dateNow.setHours(0, 0, 0));
        $scope.obj_DenNgay = new Date(dateNow.setHours(23, 59, 59));
        $scope.maxDate = $scope.obj_DenNgay;
        $scope.minDate = $scope.obj_TuNgay;
    }
    function initcombo() {

        //ComboboxDataService.getDataNhomNhanVien().then(function (result) {
        //    $scope.nhomnhanvienData = result.data;
        //});
    }
    function listColumnsgrid() {
        var dataList = [];

        dataList.push({
            field: "ngayTao", format: "{0:HH:mm:ss dd/MM/yyyy}", title: $.i18n('header_thoigian'), attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "180px"
        });
        dataList.push({
            field: "ten", title: "Nội dung", headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid,
            template: function (e) {
                if (e.loai == 2) {
                    return "<b>Thanh toán " + e.ten + "</b>"
                } else {
                    return "<b>Nạp ví " + e.ten + "</b>"
                }
            },
        });
        dataList.push({
            field: "loai", title: $.i18n('header_loaibiendong'), attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: defaultFilterableGrid, width: "150px",
            template: function (e) {
                if (e.loai == 2) {
                    return "<b>Thanh toán đơn hàng</b>"
                } else {
                    return "<b>Nạp ví LsPay</b>"
                }
            },
        });
        dataList.push({
            field: "soTien", format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            title: $.i18n('header_sotien'), attributes: { style: "text-align: right" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px",
            footerTemplate: function (e) {
                if (e.soTien) {
                    return kendo.toString(e.soTien.sum, $rootScope.UserInfo.dinhDangSo);
                }
            },
            footerAttributes: { style: "text-align: right; font-weight:700" },
        });
        dataList.push({
            field: "tacvu",
            title: "Kiểm tra",
            template: function (e) {
                if (e.congThanhToan == 'ONEPAY-B2C') {
                    let d = JSON.parse(e.duLieuThanhToan);
                    return '<button ng-click="openCheckOnepayB2C(\'' + e.duLieuThanhToan + '\')" class="btn btn-link btn-menubar"><i class="fas fa-money-bill fas-sm color-success"></i></button> '
                } else {
                    return '<button ng-click="openCheckOnepay(\'' + e.duLieuThanhToan + '\')" class="btn btn-link btn-menubar"><i class="fas fa-money-bill fas-sm color-success"></i></button> '
                }
            },
            attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });

        return dataList;
    }
    function loadgrid() {
        kendo.ui.progress($("#grid"), true);
        commonOpenLoadingText("#btn_bm022xembaocao");

        $scope.gridOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".content-filter").height());
                return heightGrid < 100 ? 500 : heightGrid;
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

        lspayDataService.getBaoCaoDoiSoatOnepay(fromdate, todate).then(function (result) {
            $scope.gridData = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                            ngayTao: {
                                type: "date"
                            },
                            soTien: {
                                type: "number"
                            },
                        }
                    }
                },
                pageSize: 20,
                aggregate: [
                    { field: "soTien", aggregate: "sum" }
                ]
            };

            kendo.ui.progress($("#grid"), false);
            commonCloseLoadingText("#btn_bm022xembaocao")
        });

    }
    //event
    $scope.openCheckOnepay = function (data) {
        let params = data.split("/");

        $scope.formCheckOnepay.center().open();
        lspayDataService.checkQueryOnepay(params[3]).then(function (result) {
            
            $scope.giaodich = JSON.parse(result.data);
            $scope.giaodich.vpc_Amount = parseFloat($scope.giaodich.vpc_Amount) / 100;
            console.log($scope.giaodich);
        });
    }

    $scope.openCheckOnepayB2C = function (data) {
        let params = data.split("/");

        $scope.formCheckOnepay.center().open();
        lspayDataService.checkQueryOnepayB2C(params[3]).then(function (result) {

            $scope.giaodich = JSON.parse(result.data);
            $scope.giaodich.vpc_Amount = parseFloat($scope.giaodich.vpc_Amount) / 100;
            console.log($scope.giaodich);
        });
    }

    $scope.xemBaoCao = function () {

        var idnhanvien = 0;
        var idnhom = 0;
        if ($scope.nhomnhanvienselect != undefined)
            idnhom = ($scope.nhomnhanvienselect.iD_Nhom < 0) ? 0 : $scope.nhomnhanvienselect.iD_Nhom;
        loadgrid(idnhom);

    }
    $scope.XuatExcel = function () {

    }
    //$scope.nhomnhanvienOnChange = function () {
    //    $scope.nhomnhanvienselect = this.nhomnhanvienselect;
    //    idnhom = $scope.nhomnhanvienselect.iD_Nhom;
    //    lspayDataService.getTongSoDu(idnhom).then(function (result) {
    //        $scope.sodukhadung = result.data;
    //    });
    //};
    $scope.fromDateChanged = function () {
        $scope.minDate = $scope.obj_TuNgay;
    }
    $scope.toDateChanged = function () {
        $scope.maxDate = $scope.obj_DenNgay;
    }
    init();

})
