angular.module('app').controller('tonghophtttController', function ($rootScope, $scope, Notification, bieuDoDoanhThuDataService, ComboboxDataService) {
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
            field: "fullName", title: "Tên nhân viên",
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
            field: "userName", title: "Tài khoản",
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
            field: "s_9950", title: "Thẻ Visa",
            aggregates: ["sum"],
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: function (e) {
                if (e.s_9950) {
                    return kendo.toString(e.s_9950.sum, $rootScope.UserInfo.dinhDangSo);
                }
            },
            footerAttributes: { style: "text-align: right; font-weight:700" },
            attributes: {
                style: "text-align: right"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "s_9951", title: "Thẻ Master",
            aggregates: ["sum"],
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: function (e) {
                if (e.s_9951) {
                    return kendo.toString(e.s_9951.sum, $rootScope.UserInfo.dinhDangSo);
                }
            },
            footerAttributes: { style: "text-align: right; font-weight:700" },
            attributes: {
                style: "text-align: right"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "s_9953", title: "Thẻ JBC",
            aggregates: ["sum"],
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: function (e) {
                if (e.s_9953) {
                    return kendo.toString(e.s_9953.sum, $rootScope.UserInfo.dinhDangSo);
                }
            },
            footerAttributes: { style: "text-align: right; font-weight:700" },
            attributes: {
                style: "text-align: right"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "s_9954", title: "Thẻ ATM nội địa",
            aggregates: ["sum"],
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: function (e) {
                if (e.s_9954) {
                    return  kendo.toString(e.s_9954.sum, $rootScope.UserInfo.dinhDangSo);
                }
            },
            footerAttributes: { style: "text-align: right; font-weight:700" },
            attributes: {
                style: "text-align: right"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "s_9965", title:"Tiền mặt",
            aggregates: ["sum"],
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: function (e) {
                if (e.s_9965) {
                    return kendo.toString(e.s_9965.sum, $rootScope.UserInfo.dinhDangSo);
                }
            },
            footerAttributes: { style: "text-align: right; font-weight:700" },
            attributes: {
                style: "text-align: right"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "s_9975", title: "Chuyển khoản",
            aggregates: ["sum"],
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: function (e) {
                if (e.s_9975) {
                    return kendo.toString(e.s_9975.sum, $rootScope.UserInfo.dinhDangSo);
                }
            },
            footerAttributes: { style: "text-align: right; font-weight:700" },
            attributes: {
                style: "text-align: right"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "s_9980", title: "Công nợ",
            aggregates: ["sum"],
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: function (e) {
                if (e.s_9980) {
                    return kendo.toString(e.s_9980.sum, $rootScope.UserInfo.dinhDangSo);
                }
            },
            footerAttributes: { style: "text-align: right; font-weight:700" },
            attributes: {
                style: "text-align: right"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "lspay", title: "Ví LsPay",
            aggregates: ["sum"],
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: function (e) {
                if (e.lspay) {
                    return kendo.toString(e.lspay.sum, $rootScope.UserInfo.dinhDangSo);
                }
            },
            footerAttributes: { style: "text-align: right; font-weight:700" },
            attributes: {
                style: "text-align: right"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "total", title: "Tổng",
            aggregates: ["sum"],
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: function (e) {
                if (e.total) {
                    return "Tổng: " + kendo.toString(e.total.sum, $rootScope.UserInfo.dinhDangSo);
                }
            },
            footerAttributes: { style: "text-align: right; font-weight:700" },
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
                title: "BÁO CÁO HÌNH THỨC THANH TOÁN THEO NHÂN VIÊN",
                allPages: true,
                avoidLinks: true,
                paperSize: "A4",
                margin: { top: "1.5cm", bottom: "5cm" /*left: "1cm", right: "1cm", bottom: "1cm"*/ },
                landscape: true,
                repeatHeaders: false,
                template: $("#page-template").html(),
                scale: 0.8
            },
            dataSource: new kendo.data.DataSource(
                {
                    data: [],
                    schema: {
                        model: {
                            fields: {
                                s_9950: { type: "number" },
                                s_9951: { type: "number" },
                                s_9953: { type: "number" },
                                s_9954: { type: "number" },
                                s_9965: { type: "number" },
                                s_9975: { type: "number" },
                                s_9980: { type: "number" },
                                total: { type: "number" }
                            }
                        }
                    },
                    pageSize: 200,
                    //group: {
                    //    field: "paymentType",
                    //    aggregates: [
                    //        { field: "totalMoney", aggregate: "sum" },

                    //    ]
                    //},
                    aggregate: [
                        { field: "s_9950", aggregate: "sum" },
                        { field: "s_9951", aggregate: "sum" },
                        { field: "s_9953", aggregate: "sum" },
                        { field: "s_9954", aggregate: "sum" },
                        { field: "s_9965", aggregate: "sum" },
                        { field: "s_9975", aggregate: "sum" },
                        { field: "s_9980", aggregate: "sum" },
                        { field: "total", aggregate: "sum" }
                    ]
                }
            ),
            //dataBound: function () {
            //    kendo.drawing.drawDOM("#grid", {
            //        paperSize: "A4",
            //        margin: "3cm",
            //        template: $("#page-template").html()
            //    }).then(function (group) {
            //        kendo.drawing.pdf.saveAs(group, "filename.pdf");
            //    });
            //},
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
        let date = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let sitecode = $scope.siteselect == undefined ? '' : $scope.siteselect.siteCode
        bieuDoDoanhThuDataService.gettonghophttt(sitecode, date).then(function (result) {
            $scope.gridData = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                            s_9950: { type: "number" },
                            s_9951: { type: "number" },
                            s_9953: { type: "number" },
                            s_9954: { type: "number" },
                            s_9965: { type: "number" },
                            s_9975: { type: "number" },
                            s_9980: { type: "number" },
                            lspay: { type: "number" },
                            total: { type: "number" }
                        }
                    }
                },
                pageSize: 200,
                //group: {
                //    field: "paymentType",
                //    aggregates: [
                //        { field: "totalMoney", aggregate: "sum" },

                //    ]
                //},
                aggregate: [
                    { field: "s_9950", aggregate: "sum" },
                    { field: "s_9951", aggregate: "sum" },
                    { field: "s_9953", aggregate: "sum" },
                    { field: "s_9954", aggregate: "sum" },
                    { field: "s_9965", aggregate: "sum" },
                    { field: "s_9975", aggregate: "sum" },
                    { field: "s_9980", aggregate: "sum" },
                    { field: "lspay", aggregate: "sum" },
                    { field: "total", aggregate: "sum" }
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
                            fileName: "test.pdf"
                        });
                        progress.resolve();
                    });
            })
        //$("#grid").data("kendoGrid").saveAsPDF();
    }

    $scope.siteOnChange = function () {
        $scope.siteselect = this.siteselect;
    }
    init();

})

