angular.module('app').controller('BaoCaoCongSoatVeController', function ($rootScope, $scope, $state, Notification, baoCaoKhachHangDataService, ComboboxDataService) {
    CreateSiteMap();

    //config
    function init() {
        initdate();   
        ComboboxDataService.getSite().then(function (result) {
            $scope.sitedata = result.data;
            $scope.siteselect = $scope.sitedata[0];
            initializeGrid(); 
        });          
    }

    function initdate() {
        var dateNow = new Date();
        $scope.obj_TuNgay = new Date(dateNow.setHours(0, 0, 0));
        $scope.obj_DenNgay = new Date(dateNow.setHours(23, 59, 59));
        $scope.maxDate = $scope.obj_DenNgay;
        $scope.minDate = $scope.obj_TuNgay;
    }
    let previousColumnCount = 0; 
    async function listColumnsgrid() {
        var dataList = [];
        dataList.push({
            field: "ngayTao",
            title: "Ngày Bán Vé",
            attributes: {
                style: "text-align: center"
            },
            groupFooterAttributes: {
                style: "text-align: right"
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: defaultFilterableGrid, width: "80px"
        });

        dataList.push({
            field: "tenKhachHang",
            title: "Nguồn Khách",
            attributes: {
                style: "text-align: center"
            },
            groupFooterAttributes: {
                style: "text-align: right"
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: defaultFilterableGrid, width: "80px"
        });

        dataList.push({
            field: "diemBanVe",
            title: "Điểm Bán Vé",
            attributes: {
                style: "text-align: center"
            },
            groupFooterAttributes: {
                style: "text-align: right"
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: defaultFilterableGrid, width: "50px"
        });

        dataList.push({
            field: "tenNhanVien",
            title: "Nhân Viên Bán Vé",
            attributes: {
                style: "text-align: center"
            },
            groupFooterAttributes: {
                style: "text-align: right"
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: defaultFilterableGrid, width: "60px"
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
            filterable: defaultFilterableGrid, width: "50px"
        });

        dataList.push({
            field: "maVeDichVu",
            title: "Mã vé dịch vụ",
            attributes: {
                style: "text-align: center"
            },
            groupFooterAttributes: {
                style: "text-align: right"
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: defaultFilterableGrid, width: "50px"
        });

        dataList.push({
            field: "tenHienThi",
            title: "Tên Vé",
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
            field: "hanSuDung",
            title: "Ngày Hết Hạn",
            attributes: {
                style: "text-align: center"
            },
            groupFooterAttributes: {
                style: "text-align: right"
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: defaultFilterableGrid, width: "80px"
        });

        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        let sitecode = $scope.siteselect == undefined ? '' : $scope.siteselect.siteCode;
        try {
            const result = await baoCaoKhachHangDataService.getNameACM(fromdate, todate, sitecode);
            var list = result.data;
            for (let i = 0; i < list.length; i++) {
                var item = list[i];
                dataList.push({
                    field: item.device_ID,
                    title: item.deviceName,
                    attributes: {
                        style: "text-align: center"
                    },
                    groupFooterAttributes: {
                        style: "text-align: right"
                    },
                    headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
                    filterable: defaultFilterableGrid, width: "80px"
                });
            }
            return dataList
        } catch (error) {
            console.error(error);
            return [];
        }
       
        return dataList;
    }
    async function initializeGrid() {
        try {
            const columns = await listColumnsgrid();
            console.log(columns);
            commonOpenLoadingText("#btn_xembaocao");
            loadgrid(columns);
            commonCloseLoadingText("#btn_xembaocao");
        } catch (error) {
            console.error(error);
        }
    }
    function loadgrid(columns) {

        $scope.datenow = kendo.toString(new Date(), "dd/MM/yyyy HH:mm");
        kendo.ui.progress($("#grid"), true);
        commonOpenLoadingText("#btn_xembaocao");
        $("#grid").kendoGrid ({
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
            columns: columns
        });
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        let sitecode = $scope.siteselect == undefined ? '' : $scope.siteselect.siteCode;
        baoCaoKhachHangDataService.getBaoCaoCongSoatVe(fromdate, todate, sitecode).then(function (result) {
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
    //event
    $scope.xemBaoCao = function () {
        destroyGrid();
        setTimeout(function () {
            initializeGrid();
        }, 500);
    }

    function destroyGrid() {
        const gridElement = $("#grid");
        const grid = gridElement.data("kendoGrid");
        if (grid) {
            grid.destroy();
            // Xóa grid khỏi DOM
            gridElement.empty();
        }
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

