angular.module('app').controller('phieuDieuChuyenController', function ($rootScope, $scope, Notification, ComboboxDataService, khoHangDataService) {
    CreateSiteMap();

    $scope.object = {};

    function init() {
        initdate();
        initcombobox();
        loadgrid();
        loadgriddetail(0);
    }

    function initdate() {
        var dateNow = new Date();
        $scope.obj_TuNgay = new Date(dateNow.setHours(0, 0, 0));
        $scope.obj_DenNgay = new Date(dateNow.setHours(23, 59, 59));
        $scope.maxDate = $scope.obj_DenNgay;
        $scope.minDate = $scope.obj_TuNgay;
    }
    function initcombobox() {
        ComboboxDataService.getDataKhoHang().then(function (result) {
            $scope.khoxuatData = result.data;
            $scope.khonhapData = result.data;
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
        dataList.push({
            field: "ngayDieuChuyen", title: $.i18n('header_thoigian'),
            template: function (dataItem) {
                return kendo.htmlEncode(kendo.toString(dataItem.ngayDieuChuyen, formatDateTime));
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({ field: "tenKhoNhap", title: $.i18n('header_khonhap'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({
            field: "tenKhoXuat", title: $.i18n('header_khoxuat'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({
            field: "tenQuanLy", title: $.i18n('header_nguoithuchien'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({
            field: "dienGiai", title: $.i18n('header_diengiai'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px" });
        
        return dataList;
    }
    function loadgrid() {
        commonOpenLoadingText("#btn_load");
        kendo.ui.progress($("#grid"), true);
        $scope.gridOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".content-filter").height() + $(".toolbarmenu").height());
                return heightGrid - 60;
            },
            dataBinding: function () {
                RecordNumber = (this.dataSource.page() - 1) * this.dataSource.pageSize();
            },
            resizable: true,
            editable: false,
            filterable: {
                mode: "row"
            },
            selectable: "row",
            pageable: $scope.lang == 'vi-vn' ? pageableShort_vi : pageableShort_en,
            change: onChange,
            columns: listColumnsgrid()
        };

        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        khoHangDataService.getlistphieudieuchuyen(fromdate, todate).then(function (response) {
            $scope.gridData = {
                data: response.data,
                schema: {
                    model: {
                        id: 'iD_PhieuDieuChuyen',
                        fields: {
                            iD_PhieuDieuChuyen: {
                                type: "number"
                            },
                            ngayDieuChuyen: {
                                type: "date"
                            }
                        }
                    }
                },
                pageSize: 20
            };

            kendo.ui.progress($("#grid"), false);
            commonCloseLoadingText("#btn_load")
        });
    }
    function onChange(arg) {
        let listid = []

        let grid = $("#grid").data("kendoGrid");
        grid.select().each(function () {
            let dataItem = grid.dataItem(this);
            listid.push({ iD_PhieuDieuChuyen: dataItem.iD_PhieuDieuChuyen });
        });

        if (listid.length == 1) {
            let iD_PhieuDieuChuyen = listid[0].iD_PhieuDieuChuyen;
            loadgriddetail(iD_PhieuDieuChuyen);
        } else {
            loadgriddetail(0);
        }
    }

    function listColumnsgriddetail() {
        let dataList = [];

        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({
            field: "maHang", title: $.i18n('header_mahang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({
            field: "tenHang", title: $.i18n('header_tenhang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({
            field: "tenDonVi", title: $.i18n('header_donvi'),
            attributes: { style: "text-align: center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "soLuong", title: $.i18n('header_soluong'),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            attributes: { style: "text-align: center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });

        return dataList;
    }
    function loadgriddetail(idphieudieuchuyen) {
        $scope.gridDetailOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".content-filter").height() + $(".toolbarmenu").height());
                return heightGrid - 60;
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
            columns: listColumnsgriddetail()
        };

        khoHangDataService.getlistphieudieuchuyendetail(idphieudieuchuyen).then(function (response) {
            $scope.gridDetailData = {
                data: response.data,
                schema: {
                    model: {
                        id: 'iD_PhieuNhap',
                        fields: {
                            iD_PhieuNhap: {
                                type: "number"
                            },
                            ngayNhap: {
                                type: "date"
                            }
                        }
                    }
                },
                pageSize: 20
            };

            kendo.ui.progress($("#gridDetail"), false);
        });
    }

    function listColumnsgridhangdieuchuyen() {
        let dataList = [];

        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });

        dataList.push({
            field: "maHang", title: $.i18n('header_mahang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px" });
        dataList.push({
            field: "tenHang", title: $.i18n('header_tenhang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "175px" });
        dataList.push({
            field: "tenDonVi", title: $.i18n('header_donvi'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px" });
        dataList.push({
            field: "soLuongTon", title: $.i18n('header_soluongton'),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            attributes: { style: "text-align: center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        });
        dataList.push({
            field: "soLuongChuyen", title: $.i18n('header_soluongchuyen'),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            attributes: { style: "text-align: center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        });

        return dataList;
    }
    function loadgridhangdieuchuyen(idkho) {
        kendo.ui.progress($("#gridhangdieuchuyen"), true);
        $scope.gridhangdieuchuyenOptions = {
            sortable: true,
            height: function () {
                return 300;
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
            save: function (e) {
                savechitietdieuchuyen(e);
            },
            update: function (e) {
                e.success();
            },
            columns: listColumnsgridhangdieuchuyen()
        };

        khoHangDataService.getlistmathangtheokho(idkho, 0).then(function (response) {
            $scope.gridhangdieuchuyenData = {
                data: response.data,
                schema: {
                    model: {
                        fields: {
                            maHang: {
                                type: "string",
                                editable: false
                            },
                            tenHang: {
                                type: "string",
                                editable: false
                            },
                            tenDonVi: {
                                type: "string",
                                editable: false
                            },
                            soLuongTon: {
                                type: "number",
                                editable: false
                            },
                            soLuongChuyen: {
                                type: "number",
                                editable: true,
                                validation: {
                                    min: 0
                                },
                            },

                        }
                    }
                },
                pageSize: 20
            };

            kendo.ui.progress($("#gridhangdieuchuyen"), false);
        });
    }
    function savechitietdieuchuyen(e) {
        e.preventDefault();
        if (e.values.soLuongChuyen > e.model.soLuongTon) {
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_soluongchuyenkhongthelonhonsoluongtonkho') }, 'warning');

            e.model.soLuongChuyen = e.model.soLuongTon;
        }
        else {
            e.model.soLuongChuyen = e.values.soLuongChuyen;
        }

        $("#gridhangdieuchuyen").data("kendoGrid").refresh();
    }

    function validate() {
        let flag = true;
        let msg = '';

        $scope.object.ID_PhieuDieuChuyen = 0;
        $scope.object.DienGiai = $scope.ghichu;
        if ($scope.khoxuatselect != undefined)
            $scope.object.ID_KhoXuat = ($scope.khoxuatselect.iD_Kho < 0) ? 0 : $scope.khoxuatselect.iD_Kho;
        else
            $scope.object.ID_KhoXuat = 0;
        if ($scope.khonhapselect != undefined)
            $scope.object.ID_KhoNhap = ($scope.khonhapselect.iD_Kho < 0) ? 0 : $scope.khonhapselect.iD_Kho;
        else
            $scope.object.ID_KhoNhap = 0;

        let myGridAdd = $("#gridhangdieuchuyen").data("kendoGrid");
        let dataSource = myGridAdd.dataSource.data();
        let ChiTiet = [];
        for (i = 0; i < dataSource.length; i++) {
            if (dataSource[i].soLuongChuyen > 0) {
                if (dataSource[i].soLuongChuyen > dataSource[i].soLuongTon) {
                    flag = false;
                    msg = $.i18n('label_soluongchuyenkhongthelonhonsoluongtonkho');
                    break;
                }

                var obj = {
                    ID_HangHoa: dataSource[i].iD_Hang,
                    SoLuong: dataSource[i].soLuongChuyen
                }
                ChiTiet.push(obj);
            }
        }
        $scope.object.ChiTiet = ChiTiet;

        if (flag && ($scope.object.ID_KhoXuat <= 0 || $scope.object.ID_KhoXuat == undefined)) {
            flag = false;
            msg = $.i18n('label_khohangxuatkhongduocdetrong');
            $("#khoxuat").focus();
        }
        if (flag && ($scope.object.ID_KhoNhap <= 0 || $scope.object.ID_KhoNhap == undefined)) {
            flag = false;
            msg = $.i18n('label_khohangnhapkhongduocdetrong');
            $("#khonhap").focus();
        }

        if (flag && ChiTiet.length <= 0) {
            flag = false;
            msg = $.i18n('label_chuachonmathangdieuchuyen');
        }

        if (flag && ($scope.object.ID_KhoNhap == $scope.object.ID_KhoXuat)) {
            flag = false;
            msg = $.i18n('label_khohangnhapkhongthetrungvoikhohangxuat');
            $("#khonhap").focus();
        }

        if (!flag)
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n(msg) }, 'warning');

        return flag;
    }

    //event
    $scope.khoxuatOnChange = function () {
        $scope.khoxuatselect = this.khoxuatselect;

        let idkhoxuat = -1;
        if($scope.khoxuatselect != undefined)
            idkhoxuat = ($scope.khoxuatselect.iD_Kho <= 0) ? -1 : $scope.khoxuatselect.iD_Kho;

        loadgridhangdieuchuyen(idkhoxuat);
    }
    $scope.khonhapOnChange = function () {
        $scope.khonhapselect = this.khonhapselect;
    }

    $scope.loadphieudieuchuyen = function () {
        loadgrid();
        loadgriddetail(0);
    }
    $scope.themphieudieuchuyen = function () {
        $scope.formdetail.center().open();

        loadgridhangdieuchuyen(-1);

        $("#khoxuat").data("kendoComboBox").value("");
        $("#khonhap").data("kendoComboBox").value("");
        $scope.ghichu = '';
        $scope.object = {};
        $scope.khoxuatselect = undefined;
        $scope.khonhapselect = undefined;

    }
    $scope.xuatexcel = function () {
        commonOpenLoadingText("#btn_xuatExcel");

        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        khoHangDataService.excellistphieudieuchuyen(fromdate, todate).then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayravuilongloadlaitrang') }, 'warning');

            commonCloseLoadingText("#btn_xuatExcel")
        });
    }

    $scope.luuphieudieuchuyen = function () {
        if (validate()) {
            commonOpenLoadingText("#btn_luuphieudieuchuyen");
            khoHangDataService.themmoiphieudieuchuyen($scope.object).then(function (result) {
                if (result.flag) {
                    $scope.formdetail.center().close();
                    $scope.object = {};
                    $scope.loadphieudieuchuyen();
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                }
                else
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');

                commonCloseLoadingText("#btn_luuphieudieuchuyen")
            })
        }
    }
    $scope.huyluuphieudieuchuyen = function () {
        $scope.formdetail.center().close();
    }
    $scope.fromDateChanged = function () {
        if ($scope.obj_TuNgay == null || $scope.obj_TuNgay > $scope.obj_DenNgay) {
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_tungaykhongthelonhondenngay') }, "error");
            $scope.obj_TuNgay = $scope.minDate;
        } else {
            $scope.minDate = $scope.obj_TuNgay;
        }
    }
    $scope.toDateChanged = function () {
        if ($scope.obj_DenNgay == null || $scope.obj_TuNgay > $scope.obj_DenNgay) {
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_tungaykhongthelonhondenngay') }, "error");
            $scope.obj_DenNgay = $scope.maxDate;
        } else {
            $scope.maxDate = $scope.obj_DenNgay;
        }
    }
    init();

})