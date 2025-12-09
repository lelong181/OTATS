angular.module('app').controller('tonghopdoanhthuticketotaController', function ($rootScope, $scope, Notification, bieuDoDoanhThuDataService, ComboboxDataService) {
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
    }
    function listColumnsgrid() {
        var dataList = [];

        dataList.push({
            field: "serviceSubGroupName", title: "Nhóm",
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
            field: "serviceRateName", title: "Dịch vụ", attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "quantity", title: $.i18n('header_soluong'),
            aggregates: ["sum"],
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: function (e) {
                //console.log(e);
                return "Tổng: " + kendo.toString(e.quantity.sum, $rootScope.UserInfo.dinhDangSo);
            },
            footerAttributes: { style: "text-align: right; font-weight:700" },
            groupFooterTemplate: function (e) {
                //console.log(e);
                return kendo.toString(e.quantity.sum, $rootScope.UserInfo.dinhDangSo);
            },
            attributes: {
                style: "text-align: right"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            title: "Đơn giá",
            aggregates: ["sum"],
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            template: function (e) {
                return kendo.toString(e.totalMoney / e.quantity, "N0");
            },
            attributes: {
                style: "text-align: right"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "totalMoney", title: $.i18n('header_tongtien'),
            aggregates: ["sum"],
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: function (e) {
                //console.log(e);
                return "Tổng: " + kendo.toString(e.totalMoney.sum, $rootScope.UserInfo.dinhDangSo);
            },
            footerAttributes: { style: "text-align: right; font-weight:700" },
            groupFooterTemplate: function (e) {
                //console.log(e);
                return kendo.toString(e.totalMoney.sum, $rootScope.UserInfo.dinhDangSo);
            },
            attributes: {
                style: "text-align: right"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
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
                landscape: false,
                fileName: "BaoCaoTongHop_" + kendo.toString(new Date(), '_dd-MM-yyyy') + ".pdf",
                repeatHeaders: true,
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
                    pageSize: 20,
                    group: {
                        field: "serviceSubGroupName",
                        aggregates: [
                            { field: "totalMoney", aggregate: "sum" },
                            { field: "quantity", aggregate: "sum" }

                        ]
                    },
                    aggregate: [
                        { field: "totalMoney", aggregate: "sum" },
                        { field: "quantity", aggregate: "sum" },
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
        bieuDoDoanhThuDataService.gettonghopdoanhthuticketota(sitecode, date).then(function (result) {
            $scope.gridData = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                            totalMoney: { type: "number" },
                            quantity: { type: "number" }
                        }
                    }
                },
                pageSize: 20,
                group: {
                    field: "serviceSubGroupName",
                    aggregates: [
                        { field: "totalMoney", aggregate: "sum" },
                        { field: "quantity", aggregate: "sum" }

                    ]
                },
                aggregate: [
                    { field: "totalMoney", aggregate: "sum" },
                    { field: "quantity", aggregate: "sum" },
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
        $scope.datenow = kendo.toString(new Date(), "dd/MM/yyyy HH:mm");
        //var progress = $.Deferred();
        //kendo.drawing.drawDOM(".content")
        //    .done(function (header) {
        //        $("#grid").data("kendoGrid")._drawPDF(progress)
        //            .then(function (root) {
        //                //root.children.push(header);
        //                console.log(root);
        //                return kendo.drawing.exportPDF(root, {
        //                    paperSize: "A4",
        //                    margin: { top: "1.5cm", bottom: "3cm" /*left: "1cm", right: "1cm", */ },
        //                    landscape: false,
        //                    scale: 0.8
        //                });
        //            })
        //            .done(function (dataURI) {
        //                kendo.saveAs({
        //                    dataURI: dataURI,
        //                    fileName: "BaoCaoTongHop_" + $scope.siteselect.siteCode + "_" + kendo.toString(new Date(), '_dd-MM-yyyy') + ".pdf"
        //                });
        //                progress.resolve();
        //            });
        //    })
        $("#grid").data("kendoGrid").saveAsPDF();
    }

    $scope.siteOnChange = function () {
        $scope.siteselect = this.siteselect;
    }
    init();

})

