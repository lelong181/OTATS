angular.module('app').controller('tonghophinhthucthanhtoanController', function ($rootScope, $scope, Notification, bieuDoDoanhThuDataService, ComboboxDataService) {
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
            groupHeaderTemplate: function (e) {
                if (e.totalMoney) {
                    return '<b>' + e.value + ' Tổng tiền: ' + kendo.toString(e.totalMoney.sum, "n0") + '</b>';
                } else if (e.aggregates.totalMoney) {
                    return e.value + ' Tổng tiền: ' + kendo.toString(e.aggregates.totalMoney.sum, "n0");
                }
            },
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
            field: "serialNumber", title: "STT",
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
            field: "paymentType", title: "HTTT",
            groupHeaderTemplate: function (e) {
                console.log(e);
                if (e.totalMoney) {
                    return '<b>' + e.value + ' : ' + kendo.toString(e.totalMoney.sum, "n0") + '</b>'
                } else if (e.aggregates.totalMoney) {
                    ;
                    return e.value + ' : ' + kendo.toString(e.aggregates.totalMoney.sum, "n0");
                }
            },
            attributes: {
                style: "text-align: center"
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "80px"
        });

        dataList.push({
            field: "totalMoney", title: $.i18n('header_tongtien'),
            aggregates: ["sum"],
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: function (e) {
                if (e.totalMoney) {
                    return "Tổng: " + kendo.toString(e.totalMoney.sum, $rootScope.UserInfo.dinhDangSo);
                }
            },
            footerAttributes: { style: "text-align: right; font-weight:700" },
            groupFooterTemplate: function (e) {
                if (e.totalMoney) {
                    return kendo.toString(e.totalMoney.sum, $rootScope.UserInfo.dinhDangSo);
                }
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
                            }
                        }
                    },
                    pageSize: 200,
                    group: {
                        field: "paymentType",
                        aggregates: [
                            { field: "totalMoney", aggregate: "sum" },

                        ]
                    },
                    aggregate: [
                        { field: "totalMoney", aggregate: "sum" },
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
        bieuDoDoanhThuDataService.gettonghopthanhtoanthungan(sitecode, date).then(function (result) {
            $scope.gridData = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                            totalMoney: { type: "number" },
                        }
                    }
                },
                pageSize: 200,
                group: {
                    field: "paymentType",
                    aggregates: [
                        { field: "totalMoney", aggregate: "sum" },

                    ]
                },
                aggregate: [
                    { field: "totalMoney", aggregate: "sum" },
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
                            fileName: "TongHopHinhThucThanhToan_" + $scope.siteselect.siteCode + "_" + kendo.toString(new Date(), "dd-mm-yyyy") + ".pdf"
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

