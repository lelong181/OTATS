angular.module('app').controller('guiBanMatHangController', function ($scope, $http, $location, Notification, ComboboxDataService, hangHoaDataService) {
    CreateSiteMap();

    let idkhachhang = 0;

    function init() {
        getquyen();

        loadgrid();
    }

    function getquyen() {
        let path = $location.path();
        let url = path.replace('/', '')
        ComboboxDataService.getquyen(url).then(function (result) {
            $scope.permission = result.data;
            if ($scope.permission.iD_ChucNang <= 0) {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_khongcoquyentruycapchucnang') }, "error");
                $location.path('/home')
            }
        });
    }

    function listColumnsgrid() {
        let dataList = [];

        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({ field: "ten", title: $.i18n('header_tenkhachhang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({
            field: "diaChi", title: $.i18n('header_diachi'),
            template: function (dataItem) {
                if (dataItem.toaDo == '' || dataItem.toaDo == '0.0000000000000, 0.0000000000000')
                    return '<button ng-click="xemvitrichuacotoado()" class="btn btn-link btn-menubar" title ="' + $.i18n('menu_vitrikhachhang') + '" >' + kendo.htmlEncode(dataItem.diaChi) + '</button>';
                else
                    return '<button ng-click="xemvitri(' + kendo.htmlEncode(dataItem.viDo) + ',' + kendo.htmlEncode(dataItem.kinhDo) + ')" class="btn btn-link btn-menubar" title ="' + $.i18n('menu_vitrikhachhang') + '" >' + kendo.htmlEncode(dataItem.diaChi) + '</button>';
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: defaultFilterableGrid, width: "300px"
        });
        dataList.push({ field: "dienThoaiMacDinh", title: $.i18n('header_sodienthoai'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({ field: "nguoiLienHe", title: $.i18n('header_nguoilienhe'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({
            title: $.i18n('header_mathang'),
            template: function (dataItem) {
                return '<button class="btn btn-link btn-menubar" ng-click="getMatHangGuiBan()" title="' + $.i18n('label_danhsachmathang') + '"><i class="fas fa-list fas-md color-primary"></i> </button>';
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { class: "text-center" }, filterable: false, width: "60px"
        });
        return dataList;
    }
    function loadgrid() {
        kendo.ui.progress($("#grid"), true);
        $scope.gridOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height());
                return heightGrid - 40;
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

        hangHoaDataService.guiBanMatHang().then(function (result) {
            $scope.gridData = {
                data: result.data,
                schema: {
                    model: {
                        id: 'iD_KhachHang',
                        fields: {
                            iD_KhachHang: {
                                type: "number"
                            }
                        }
                    }
                },
                pageSize: 20
            };
            kendo.ui.progress($("#grid"), false);
        });
    }

    function listColumnsgridChuaGuiBan() {
        var dataList = [];

        dataList.push({
            title: "",
            template: '<button ng-click="addMatHangChuaGuiBan()" class="btn btn-link btn-menubar" ><i class="fas fa-plus-circle fas-sm color-success"></i></button> ',
            attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "70px"
        });
        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({ field: "maHang", title: $.i18n('header_mahang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({ field: "tenHang", title: $.i18n('header_tenhang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "300px" });
        dataList.push({ field: "tenDonVi", title: $.i18n('header_donvi'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px" });
        //dataList.push({
        //    field: "ngayBatDau", template: function (e) {
        //        return kendo.toString(e.ngayBatDau, formatDate)
        //    },
        //    editor: function (container, options) {
        //        var ngayBatDau_editor = $('<input name="' + options.field + '"/>').appendTo(container).kendoDateTimePicker({
        //            value: options.model.ngayBatDau,
        //            format: "{0:dd/MM/yyyy HH:mm}",
        //            dateInput: false,
        //            change: function (e) {
        //                if (options.model.ngayKetThuc >= e.sender.value()) {
        //                    options.model.ngayBatDau = e.sender.value();
        //                } else {
        //                    Notification({ title: $.i18n('label_thongbao'), message: 'Ngày kết thúc không thể nhỏ hơn ngày bắt đầu' }, 'warning');
        //                    options.model.ngayBatDau = options.model.ngayKetThuc
        //                }
        //            }
        //        });
        //        //ngayBatDau_editor.data("kendoDateTimePicker").max(options.model.ngayBatDau);
        //    },
        //    title: "Ngày bắt đầu", headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        //});
        //dataList.push({
        //    field: "ngayKetThuc", template: function (e) {
        //        return kendo.toString(e.ngayKetThuc, formatDate)
        //    },
        //    editor: function (container, options) {
        //        var ngayKetThuc_editor = $('<input name="' + options.field + '"/>').appendTo(container).kendoDateTimePicker({
        //            value: options.model.ngayKetThuc,
        //            format: "{0:dd/MM/yyyy HH:mm}",
        //            dateInput: false,
        //            change: function (e) {
        //                if (e.sender.value() >= options.model.ngayBatDau) {
        //                    options.model.ngayKetThuc = e.sender.value();
        //                } else {
        //                    Notification({ title: $.i18n('label_thongbao'), message: 'Ngày kết thúc không thể nhỏ hơn ngày bắt đầu' }, 'warning');
        //                    options.model.ngayKetThuc = options.model.ngayBatDau
        //                }
        //            }
        //        });
        //        //ngayKetThuc_editor.data("kendoDateTimePicker").min(options.model.ngayKetThuc);
        //    },
        //    title: "Ngày kết thúc", headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        //});
        //dataList.push({ field: "ghiChuGia", title: $.i18n('header_ghichu'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });

        return dataList;
    }
    function loadHangChuaGuiBan() {
        kendo.ui.progress($("#gridChuaGui"), true);
        $scope.gridOptionsChuaGui = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height());
                return heightGrid - 100;
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
            columns: listColumnsgridChuaGuiBan()
        };
        hangHoaDataService.matHangChuaGuiBan(idkhachhang).then(function (result) {
            $.each(result.data, function (index, item) {
                item.ngayBatDau = new Date();
                item.ngayKetThuc = new Date();
            });

            $scope.gridDataChuaGui = {
                data: result.data,
                schema: {
                    model: {
                        id: 'idMatHang',
                        fields: {
                            iD_Hang: {
                                type: "number", editable: false
                            },
                            maHang: {
                                editable: false
                            },
                            tenDonVi: {
                                editable: false
                            },
                            tenHang: {
                                editable: false
                            },
                            ghiChuGia: {
                                editable: true
                            },
                            ngayBatDau: {
                                type: "date", editable: true
                            },
                            ngayKetThuc: {
                                type: "date", editable: true
                            }
                        }
                    }
                },
                pageSize: 20
            };
            kendo.ui.progress($("#gridChuaGui"), false);
        });
    }

    function listColumnsgridDaGuiBan() {
        var dataList = [];

        dataList.push({
            title: "",
            template: '<button ng-click="deleteRow()" class="btn btn-link btn-menubar" ><i class="fas fa-trash fas-sm color-danger"></i></button> ',
            attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "70px"
        });
        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({ field: "maHang", title: $.i18n('header_mahang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({ field: "tenMatHang", title: $.i18n('header_tenhang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "300px" });
        dataList.push({ field: "tenDonVi", title: $.i18n('header_donvi'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px" });
        dataList.push({
            field: "ngayBatDau",
            template: function (dataItem) {
                return kendo.toString(dataItem.ngayBatDau, formatDate)
            },
            //editor: function (container, options) {
            //    var ngayBatDau_editor = $('<input name="' + options.field + '"/>').appendTo(container).kendoDateTimePicker({
            //        value: options.model.ngayBatDau,
            //        format: "{0:dd/MM/yyyy HH:mm}",
            //        dateInput: false,
            //        change: function (e) {
            //            options.model.ngayBatDau = e.sender.value();
            //        }
            //    });
            //    ngayBatDau_editor.data("kendoDateTimePicker").max(options.model.ngayBatDau);
            //},
            title: $.i18n('header_ngaybatdau'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "ngayKetThuc",
            template: function (dataItem) {
                return kendo.toString(dataItem.ngayKetThuc, formatDate)
            },
            title: $.i18n('header_ngayketthuc'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({ field: "dienGiai", title: $.i18n('header_ghichu'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });

        return dataList;
    }
    function loadHangDaGuiBan() {
        kendo.ui.progress($("#gridDaGui"), true);
        $scope.gridOptionsDaGui = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height());
                return heightGrid - 100;
            },
            dataBinding: function () {
                RecordNumber = (this.dataSource.page() - 1) * this.dataSource.pageSize();
            },
            resizable: true,
            editable: true,
            filterable: {
                mode: "row"
            },
            pageable: $scope.lang == 'vi-vn' ? pageableShort_vi : pageableShort_en,
            columns: listColumnsgridDaGuiBan()
        };
        hangHoaDataService.matHangDaGuiBan(idkhachhang).then(function (result) {
            $scope.gridDataDaGui = {
                data: result.data,
                schema: {
                    model: {
                        id: 'idMatHang',
                        fields: {
                            iD_Hang: {
                                type: "number", editable: false
                            },
                            maHang: {
                                editable: false
                            },
                            tenDonVi: {
                                editable: false
                            },
                            tenHang: {
                                editable: false
                            },
                            ghiChuGia: {
                                editable: true
                            },
                            ngayBatDau: {
                                type: "date", editable: true
                            },
                            ngayKetThuc: {
                                type: "date", editable: true
                            }
                        }

                    }
                },
                pageSize: 20
            };
            kendo.ui.progress($("#gridDaGui"), false);
        });
    }

    function initpopup() {
        let dateNow = new Date();
        let dateweek = new Date();
        dateweek.setDate(dateweek.getDate() + 7);

        $scope.obj_TuNgay = new Date(dateNow.setHours(0, 0, 0));
        $scope.obj_DenNgay = new Date(dateweek.setHours(23, 59, 59));
        $scope.maxDate = $scope.obj_DenNgay;
        $scope.minDate = $scope.obj_TuNgay;

        $scope.ghichu = '';

        loadHangChuaGuiBan();
        loadHangDaGuiBan();
    }
    function openMatHangKyGui() {
        $scope.formdetail.center().maximize().open();

        initpopup();
    }
    function validate() {
        let flag = true;
        let msg = '';

        let myGridAdd = $("#gridDaGui").data("kendoGrid");
        let dataSource = myGridAdd.dataSource.data();
        let DataChiTiet = [];

        if (dataSource.length <= 0) {
            flag = false;
            msg = $.i18n('label_chuachonmathangguiban');
        }

        if (flag) {
            for (i = 0; i < dataSource.length; i++) {
                if (dataSource[i].ngayBatDau == undefined || dataSource[i].ngayBatDau == null) {
                    flag = false;
                    msg = $.i18n('label_chuachonngaybatdautaidongso') + (i + 1).toString();
                    break;
                }

                if (dataSource[i].ngayKetThuc == undefined || dataSource[i].ngayKetThuc == null) {
                    flag = false;
                    msg = $.i18n('label_chuachonngayketthuctaidongso') + (i + 1).toString();
                    break;
                }

                if (dataSource[i].ngayKetThuc < dataSource[i].ngayBatDau) {
                    flag = false;
                    msg = $.i18n('label_ngayketthuckhongthebehonngaybatdautaidongso') + (i + 1).toString();
                    break;
                }

                var obj = {
                    ID: dataSource[i].id,
                    ID_KhachHang: dataSource[i].iD_KhachHang,
                    ID_MatHang: dataSource[i].iD_MatHang,
                    DienGiai: dataSource[i].dienGiai,
                    NgayBatDau: kendo.toString(dataSource[i].ngayBatDau, formatDateTimeFilter),
                    NgayKetThuc: kendo.toString(dataSource[i].ngayKetThuc, formatDateTimeFilter),
                }
                DataChiTiet.push(obj);
            }
        }

        if (!flag)
            Notification({ title: $.i18n('label_thongbao'), message: msg }, 'warning');

        let result = {
            flag: flag,
            data: DataChiTiet
        }

        return result;

    }

    //event
    $scope.fromDateChanged = function () {
        $scope.minDate = $scope.obj_TuNgay;
    }
    $scope.toDateChanged = function () {
        $scope.maxDate = $scope.obj_DenNgay;
    }

    $scope.xemvitrichuacotoado = function () {
        Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_khachhangchuacothongtinvitri') }, 'warning');
    }
    $scope.xemvitri = function (vido, kinhdo) {
        let url = 'https://www.google.com/maps/dir/' + vido.toString() + ',' + kinhdo.toString();
        window.open(url, '_blank');
    }

    $scope.getMatHangGuiBan = function () {
        if ($scope.permission.them > 0) {
            let myGrid = $('#grid').data("kendoGrid");
            let row = $(this).closest("tr");
            let dataItem = row.prevObject[0].dataItem;

            idkhachhang = dataItem.iD_KhachHang;
            openMatHangKyGui();
        } else
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_bankhongcoquyenthuchienthaotac') }, 'warning');
    }

    $scope.addMatHangChuaGuiBan = function () {
        let myGrid = $('#gridChuaGui').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;

        let myGridHangDaGui = $("#gridDaGui").data("kendoGrid");
        let item = {
            id: 0,
            iD_MatHang: dataItem.idMatHang,
            iD_KhachHang: idkhachhang,
            dienGiai: $scope.ghichu,
            ngayBatDau: $scope.obj_TuNgay,
            ngayKetThuc: $scope.obj_DenNgay,
            maHang: dataItem.maHang,
            tenMatHang: dataItem.tenHang,
            tenDonVi: dataItem.tenDonVi,
        }
        myGridHangDaGui.dataSource.add(item);
        myGrid.dataSource.remove(dataItem);


    }
    $scope.deleteRow = function () {
        let myGrid = $('#gridDaGui').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;

        let myGridHangChuaGui = $("#gridChuaGui").data("kendoGrid");
        let item = {
            iD_MatHang: dataItem.iD_MatHang,
            ghiChuGia: dataItem.ghiChuGia,
            maHang: dataItem.maHang,
            tenHang: dataItem.tenMatHang,
            tenDonVi: dataItem.tenDonVi,
        }
        myGridHangChuaGui.dataSource.add(item);
        myGrid.dataSource.remove(dataItem);

    }

    $scope.luuthaydoi = function () {
        let result = validate();
        if (result.flag) {
            hangHoaDataService.luuThayDoi(result.data).then(function (result) {
                if (result.flag) {
                    $scope.formdetail.center().close();
                    loadgrid();
                    idkhachhang = 0;
                    Notification({ title: $.i18n('label_thongbao'), message: result.message }, 'success');
                }
                else
                    Notification({ title: $.i18n('label_thongbao'), message: result.message }, 'warning');

                commonCloseLoadingText("#luuthaydoi")
            })
        }
    }

    init();
})