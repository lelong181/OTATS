angular.module('app').controller('tonghopdichvutheobookingController', function ($rootScope, $scope, $state, Notification, bieuDoDoanhThuDataService, ComboboxDataService) {
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
            field: "maThamChieu", title: "OTA Booking",
            //groupHeaderTemplate: '<b>Nhóm dịch vụ: #= value #</b>',
            groupHeaderTemplate: function (e) {
                console.log(e);
                dataItem = e.items[0];
                return "<a href='' class='color-primary' ng-click='openDetailFromGrid(" + kendo.htmlEncode(dataItem.iD_DonHang) + ")'>"
                    + kendo.htmlEncode(dataItem.maThamChieu)
                    + " - Khách hàng: <b style='color:#111'>" + kendo.htmlEncode(dataItem.tenKhachHang) + "</b>"
                    + " - Nhân viên: <b style='color:#111'>" + kendo.htmlEncode(dataItem.tenNhanVien) + "</b>"
                    + " - Trạng thái: <b style='color:" + dataItem.mauTrangThai + "'>" + kendo.htmlEncode(dataItem.tenTrangThai) + "</b>"
                    + " - Thời gian tạo: <b style='color:#111'>" + kendo.toString(dataItem.createDate, 'dd/MM/yyyy HH:mm') + "</b>"
                    + "</a>";
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
            field: "siteNCC", title: "PKD",
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
            field: "siteCode", title: "Site",
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
            field: "tenDichVu", title: "Dịch vụ",
            attributes: {
                style: "text-align: center"
            },
            groupFooterAttributes: {
                style: "text-align: right"
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: defaultFilterableGrid
        });
        dataList.push({
            field: "giaBan", title: "Giá",
            //aggregates: ["sum"],
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            //footerTemplate: function (e) {
            //    if (e.s_9950) {
            //        return kendo.toString(e.s_9950.sum, $rootScope.UserInfo.dinhDangSo);
            //    }
            //},
            //footerAttributes: { style: "text-align: right; font-weight:700" },

            attributes: {
                style: "text-align: right"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "soLuong", title: "Số lượng (Đã dùng/Đã mua)",
            //aggregates: ["sum"],
            template: function (e) {
                if (e.daSuDung > 0) {
                    return "<p style='color:blue'>" + e.daSuDung + "/" + e.soLuong + "</p>";

                } else {
                    return e.daSuDung + "/" + e.soLuong;

                }
            },
            //format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            //footerTemplate: function (e) {
            //    if (e.s_9950) {
            //        return kendo.toString(e.s_9950.sum, $rootScope.UserInfo.dinhDangSo);
            //    }
            //},
            //footerAttributes: { style: "text-align: right; font-weight:700" },
            attributes: {
                style: "text-align: right"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "tongTienDichVu", title: "Thành tiền",
            aggregates: ["sum"],
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: function (e) {
                if (e.tongTienDichVu) {
                    return kendo.toString(e.tongTienDichVu.sum, $rootScope.UserInfo.dinhDangSo);
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
                title: "BÁO CÁO TỔNG HỢP BOOKING",
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
                                bookingCode: { type: "string" },
                                siteCode: { type: "string" },
                                siteNCC: { type: "string" },
                                createDate: { type: "date" },
                                daSuDung: { type: "number" },
                                giaBan: { type: "number" },
                                iD_DonHang: { type: "number" },
                                invoiceCode: { type: "string" },
                                maThamChieu: { type: "string" },
                                soLuong: { type: "number" },
                                tenDichVu: { type: "string" },
                                tenKhachHang: { type: "string" },
                                tenNhanVien: { type: "string" },
                                tenTrangThai: { type: "string" },
                                tienDaThanhToan: { type: "number" },
                                tongTien: { type: "number" },
                                tongTienDichVu: { type: "number" },
                            }
                        }
                    },
                    pageSize: 200,
                    group: {
                        field: "maThamChieu",
                        aggregates: [
                            { field: "tongTien", aggregate: "sum" },
                            { field: "tongTienDichVu", aggregate: "sum" },
                            { field: "tienDaThanhToan", aggregate: "sum" }

                        ]
                    },
                    aggregate: [
                        { field: "tongTien", aggregate: "sum" },
                        { field: "tongTienDichVu", aggregate: "sum" },
                        { field: "tienDaThanhToan", aggregate: "sum" }
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
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        let sitecode = $scope.siteselect == undefined ? '' : $scope.siteselect.siteCode
        bieuDoDoanhThuDataService.gettonghopdichvutheobooking(sitecode, fromdate, todate).then(function (result) {
            $scope.gridData = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                            bookingCode: { type: "string" },
                            siteCode: { type: "string" },
                            siteNCC: { type: "string" },
                            createDate: { type: "date" },
                            daSuDung: { type: "number" },
                            giaBan: { type: "number" },
                            iD_DonHang: { type: "number" },
                            invoiceCode: { type: "string" },
                            maThamChieu: { type: "string" },
                            soLuong: { type: "number" },
                            tenDichVu: { type: "string" },
                            tenKhachHang: { type: "string" },
                            tenNhanVien: { type: "string" },
                            tenTrangThai: { type: "string" },
                            tienDaThanhToan: { type: "number" },
                            tongTienDichVu: { type: "number" },
                            tongTien: { type: "number" }
                        }
                    }
                },
                pageSize: 200,
                group: {
                    field: "maThamChieu",
                    aggregates: [
                        { field: "tongTien", aggregate: "sum" },
                        { field: "tongTienDichVu", aggregate: "sum" },
                        { field: "tienDaThanhToan", aggregate: "sum" }

                    ]
                },
                aggregate: [
                    { field: "tongTien", aggregate: "sum" },
                    { field: "tongTienDichVu", aggregate: "sum" },
                    { field: "tienDaThanhToan", aggregate: "sum" }
                ]
            };

            kendo.ui.progress($("#grid"), false);
            commonCloseLoadingText("#btn_bm009xembaocao")
        });
    }

    function openFormDetail(_id) {

        let url = $state.href('editdonhangpkd', { iddonhang: _id });
        window.open(url, '_blank');

        //$state.go('editdonhang', { iddonhang: _id });
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
                            fileName: "NCC_" + $scope.siteselect.siteCode + "_" + kendo.toString(new Date(), "ddmmyyyy") + ".pdf"
                        });
                        progress.resolve();
                    });
            })
        //$("#grid").data("kendoGrid").saveAsPDF();
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
    $scope.openDetailFromGrid = function (iD_DonHang) {
        openFormDetail(iD_DonHang);
    }
    init();

})

