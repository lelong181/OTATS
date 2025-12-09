angular.module('app').controller('tonghopdoanhthutheocaticketotaController', function ($rootScope, $scope, Notification, bieuDoDoanhThuDataService, ComboboxDataService) {
    CreateSiteMap();

    //config
    function init() {
        initdate();
        loadgrid();
        ComboboxDataService.getSite().then(function (result) {
            $scope.sitedata = result.data;
        });

    }

    function initdate() {
        var dateNow = new Date();
        $scope.obj_TuNgay = new Date(dateNow.setHours(0, 0, 0));
    }
    function listColumnsgrid() {
        var dataList = [];
        dataList.push({
            field: "id", title: "Số ca",
            groupHeaderTemplate: '<b>#= value # Tổng số lượng: #= serviceRateName.count # Tổng tiền: #= kendo.toString(totalMoney.sum,"n0") #</b>',

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
            field: "serviceRateGroupName", title: "Nhóm",
            groupHeaderTemplate: '<b>Nhóm dịch vụ: #= value #</b>',

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
            field: "serviceRateName", title: "Dịch vụ",
            aggregates: ["count"],
            attributes: { style: "text-align: center" },
            groupFooterTemplate: function (e) {
                console.log(e);
                return kendo.toString(e.serviceRateName.count, $rootScope.UserInfo.dinhDangSo);
            },
            footerAttributes: { style: "text-align: right; font-weight:700" },
            headerAttributes: {
                "class": "table-header-cell",
                style: "text-align: center"
            },
            filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "price", title: "Đơn giá", attributes: {
                style: "text-align: center"
            },
            attributes: {

                style: "text-align: right"
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "80px"
        });
        dataList.push({
            field: "quantity", title: $.i18n('header_soluong'),
            aggregates: ["sum"],
            attributes: {
                style: "text-align: right"
            },
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerAttributes: { style: "text-align: right; font-weight:700" },
            footerTemplate: function (e) {
                return kendo.toString(e.quantity.sum, $rootScope.UserInfo.dinhDangSo);
            },
            groupFooterTemplate: function (e) {
                return kendo.toString(e.quantity.sum, $rootScope.UserInfo.dinhDangSo);
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "80px"
        });
        dataList.push({
            field: "totalMoney", title: $.i18n('header_tongtien'),
            aggregates: ["sum"],
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: function (e) {
                return "Tổng: " + kendo.toString(e.totalMoney.sum, $rootScope.UserInfo.dinhDangSo);
            },
            footerAttributes: { style: "text-align: right; font-weight:700" },
            groupFooterTemplate: function (e) {
                return kendo.toString(e.totalMoney.sum, $rootScope.UserInfo.dinhDangSo);
            },
            attributes: {
                style: "text-align: right"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "status", title: "Trạng thái", attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        return dataList;
    }

    function loadgrid() {
        $scope.datenow = kendo.toString(new Date(), "dd/MM/yyyy HH:mm");
        kendo.ui.progress($("#grid"), true);
        commonOpenLoadingText("#btn_bm009xembaocao");
        $scope.gridOptions = {
            pdf: {
                allPages: true,
                avoidLinks: true,
                paperSize: "A4",
                margin: { top: "1.5cm", /*left: "1cm", right: "1cm", bottom: "1cm"*/ },
                landscape: true,
                repeatHeaders: false,
                forcePageBreak: ".page-break",
                template: $("#page-template").html(),
                scale: 0.8
            },
            dataSource: new kendo.data.DataSource(
                {
                    data: [],
                    schema: {
                        model: {
                            fields: {
                                totalMoney: { type: "number" },
                                quantity: { type: "number" },
                            }
                        }
                    },
                    pageSize: 200,
                    group: {
                        field: "id",
                        aggregates: [
                            { field: "totalMoney", aggregate: "sum" },
                            { field: "quantity", aggregate: "sum" },
                            { field: "serviceRateName", aggregate: "count" }

                        ]
                    },
                    aggregate: [
                        { field: "totalMoney", aggregate: "sum" },
                        { field: "quantity", aggregate: "sum" },
                        { field: "serviceRateName", aggregate: "count" }
                    ]
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
            groupable: true,
            filterable: {
                mode: "row"
            },
            pageable: $scope.lang == 'vi-vn' ? pageableShort_vi : pageableShort_en,
            columns: listColumnsgrid()
        };
        let date = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let sitecode = $scope.siteselect == undefined ? '' : $scope.siteselect.siteCode
        bieuDoDoanhThuDataService.gettonghopdoanhthutheocaticketota(sitecode, date).then(function (result) {
            $scope.gridData = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                            totalMoney: { type: "number" },
                            quantity: { type: "number" },
                        }
                    }
                },
                pageSize: 200,
                group: {
                    field: "id",
                    aggregates: [
                        { field: "totalMoney", aggregate: "sum" },
                        { field: "quantity", aggregate: "sum" },
                        { field: "serviceRateName", aggregate: "count" }

                    ]
                },
                aggregate: [
                    { field: "totalMoney", aggregate: "sum" },
                    { field: "quantity", aggregate: "sum" },
                    { field: "serviceRateName", aggregate: "count" }
                ]
            };

            kendo.ui.progress($("#grid"), false);
            commonCloseLoadingText("#btn_bm009xembaocao")
        });
    }

    //event
    $scope.xemBaoCao = function () {


        loadgrid();
    }
    $scope.XuatExcel = function () {
        $("#grid").data("kendoGrid").saveAsExcel();
    }

    $scope.XuatPDF = function () {
        //$("#grid").data("kendoGrid").saveAsPDF();
        $scope.datenow = kendo.toString(new Date(), "dd/MM/yyyy HH:mm");
        var progress = $.Deferred();
        kendo.drawing.drawDOM(".content")
            .done(function (header) {
                $("#grid").data("kendoGrid")._drawPDF(progress)
                    .then(function (root) {
                        root.children.push(header);
                        console.log(root);
                        return kendo.drawing.exportPDF(root, {
                            paperSize: "A4",
                            margin: { top: "1.5cm", /*left: "1cm", right: "1cm", bottom: "1cm"*/ },
                            landscape: true,
                            scale: 0.8
                        });
                    })
                    .done(function (dataURI) {
                        kendo.saveAs({
                            dataURI: dataURI,
                            fileName: "TongHopDoanhThuTheoCa_" + $scope.siteselect.siteCode + "_" + kendo.toString(new Date(), '_dd-MM-yyyy') + ".pdf"
                        });
                        progress.resolve();
                    });
            })
    }

    $scope.siteOnChange = function () {
        $scope.siteselect = this.siteselect;
    }
    init();

})

