angular.module('app').controller('BaoCaoSoatVeKhachHangController', function ($rootScope, $scope, $state, Notification, baoCaoKhachHangDataService, ComboboxDataService) {
    CreateSiteMap();

    //config
    function init() {
        initdate();
        
        ComboboxDataService.getSite().then(function (result) {
            $scope.sitedata = result.data;
            $scope.siteselect = $scope.sitedata[0];
            loadgrid();
        });
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
            field: "ngayTao",
            title: "Thời Gian Tạo",
            attributes: {
                style: "text-align: center"
            },
            groupFooterAttributes: {
                style: "text-align: right"
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: defaultFilterableGrid, width: "100px"
        });

        dataList.push({
            field: "maThamChieu",
            title: "Mã OTA Booking",
            attributes: {
                style: "text-align: center"
            },
            groupFooterAttributes: {
                style: "text-align: right"
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: defaultFilterableGrid, width: "70px"
        });

        dataList.push({
            field: "tenKhachHang",
            title: "Khách Hàng",
            attributes: {
                style: "text-align: center"
            },
            groupFooterAttributes: {
                style: "text-align: right"
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: defaultFilterableGrid, width: "150px"
        });

        dataList.push({
            field: "tenHienThi",
            title: "Mặt Hàng",
            attributes: {
                style: "text-align: center"
            },
            groupFooterAttributes: {
                style: "text-align: right"
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: defaultFilterableGrid, width: "150px"
        });

        dataList.push({
            field: "maVeDichVu",
            title: "Mã Vé",
            attributes: {
                style: "text-align: center"
            },
            groupFooterAttributes: {
                style: "text-align: right"
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: defaultFilterableGrid, width: "70px"
        });

        dataList.push({
            field: "hanSuDung",
            title: "Hạn Sử Dụng",
            attributes: {
                style: "text-align: center"
            },
            groupFooterAttributes: {
                style: "text-align: right"
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: defaultFilterableGrid, width: "100px"
        });

        dataList.push({
            field: "thoiGianSuDung",
            title: "Thời Gian Sử Dụng",
            attributes: {
                style: "text-align: center"
            },
            groupFooterAttributes: {
                style: "text-align: right"
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: defaultFilterableGrid, width: "100px"
        });

        dataList.push({
            field: "acm",
            title: "Cổng Soát Vé",
            attributes: {
                style: "text-align: center"
            },
            groupFooterAttributes: {
                style: "text-align: right"
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: defaultFilterableGrid, width: "70px"
        });
        return dataList;
    }

    function loadgrid() {
        $scope.datenow = kendo.toString(new Date(), "dd/MM/yyyy HH:mm");
        kendo.ui.progress($("#grid"), true);
        commonOpenLoadingText("#btn_xembaocao");
        $scope.gridOptions = {
            dataSource: new kendo.data.DataSource(
                {
                    data: [],
                    schema: {
                        model: {                           
                        }
                    },
                    pageSize: 200,
                }
            ),
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".content-filter").height()) - 70;
                return heightGrid < 100 ? 500 : heightGrid;
            },
            excel: {
                allPages: true
            },
            excelExport: function (e) {

            },
            resizable: true,
            editable: false,
            groupable: false,
            filterable: {
                mode: "row"
            },
            pageable: $scope.lang == 'vi-vn' ? pageableShort_vi : pageableShort_en,
            columns: listColumnsgrid()
        };
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);       

        let sitecode = $scope.siteselect == undefined ? '' : $scope.siteselect.siteCode
        
        baoCaoKhachHangDataService.getBaoCaoSoatVeKhachHang(fromdate, todate, sitecode).then(function (result) {
            $scope.gridData = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                           
                        }
                    }
                },
                pageSize: 200,
            };

            kendo.ui.progress($("#grid"), false);
            commonCloseLoadingText("#btn_xembaocao")
        });
    }
   

    //event
    $scope.xemBaoCao = function () {
        loadgrid();
    }
    $scope.XuatExcel = function () {
        $("#grid").data("kendoGrid").saveAsExcel();
    }
    $scope.siteOnChange = function () {
        $scope.siteselect = this.siteselect;
    }
    $scope.fromDateChanged = function () {
        $scope.minDate = $scope.obj_TuNgay;
    }
    $scope.toDateChanged = function () {
        $scope.maxDate = $scope.obj_DenNgay;
    }

    init();
})

