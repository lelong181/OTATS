angular.module('app').controller('phieuNhapController', function ($rootScope, $scope, Notification, ComboboxDataService, khoHangDataService) {
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
            $scope.khohangData = result.data;
        });
        ComboboxDataService.getDataMatHang().then(function (result) {
            $scope.mathangData = result.data;
        });
    }

    function listColumnsgrid() {
        var dataList = [];

        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({ field: "iD_PhieuNhap", title: $.i18n("header_maphieu"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px" });
        dataList.push({
            field: "ngayNhap", title: $.i18n("header_ngaynhap"),
            template: function (dataItem) {
                return kendo.htmlEncode(kendo.toString(dataItem.ngayNhap, formatDateTime));
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({ field: "tenKho", title: $.i18n("header_khonhap"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px" });
        dataList.push({ field: "tenQuanLy", title: $.i18n("header_nguoinhap"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({ field: "noiDung", title: $.i18n("header_ghichu"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });

        return dataList;
    }
    function loadgrid() {
        commonOpenLoadingText("#btn_loadphieunhap");
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
        khoHangDataService.getlistphieunhap(fromdate, todate).then(function (response) {
            $scope.gridData = {
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

            kendo.ui.progress($("#grid"), false);
            commonCloseLoadingText("#btn_loadphieunhap")
        });
    }
    function onChange(arg) {
        let listid = []

        let grid = $("#grid").data("kendoGrid");
        grid.select().each(function () {
            let dataItem = grid.dataItem(this);
            listid.push({ iD_PhieuNhap: dataItem.iD_PhieuNhap });
        });

        if (listid.length == 1) {
            let iD_PhieuNhap = listid[0].iD_PhieuNhap;
            loadgriddetail(iD_PhieuNhap);
        } else {
            loadgriddetail(0);
        }
    }

    function listColumnsgriddetail() {
        var dataList = [];

        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({ field: "maHang", title: $.i18n("header_mahang"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px" });
        dataList.push({ field: "tenHang", title: $.i18n("header_tenhang"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({ field: "tenDonVi", title: $.i18n("header_donvi"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px" });
        dataList.push({
            field: "soLuong", title: $.i18n("header_soluong"),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            attributes: { style: "text-align: center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });

        return dataList;
    }
    function loadgriddetail(idphieunhap) {
        kendo.ui.progress($("#gridDetail"), false);
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

        khoHangDataService.getlistphieunhapdetail(idphieunhap).then(function (response) {
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

    function listColumnsgridhangnhap() {
        let dataList = [];

        dataList.push({
            field: "chiTiet", title: $.i18n("header_xoa"),
            template: '<button ng-click="deleterow()" class="btn btn-link btn-menubar" title ="' + $.i18n("button_xoa") + '" ><i class="fas fa-trash fas-sm color-danger"></i></button> ',
            attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "50px"
        });
        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });

        dataList.push({ field: "TenHang", title: $.i18n("header_tenhang"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "175px" });
        dataList.push({
            field: "SoLuong", title: $.i18n("header_soluong"),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            attributes: { style: "text-align: center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        });

        return dataList;
    }
    function loadgridhangnhap() {
        $scope.gridhangnhapOptions = {
            sortable: true,
            height: function () {
                return 250;
            },
            dataBinding: function () {
                RecordNumber = (this.dataSource.page() - 1) * this.dataSource.pageSize();
            },
            resizable: true,
            editable: true,
            filterable: false,
            pageable: $scope.lang == 'vi-vn' ? pageableShort_vi : pageableShort_en,
            columns: listColumnsgridhangnhap()
        };

        $scope.gridhangnhapData = {
            data: [],
            schema: {
                model: {
                    fields: {
                        chiTiet: {
                            type: "string",
                            editable: false
                        },
                        ID_HangHoa: {
                            type: "number",
                            editable: false
                        },
                        TenHang: {
                            type: "string",
                            editable: false
                        },
                        SoLuong: {
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

    }
    function validate() {
        let flag = true;
        let msg = '';

        $scope.object.ID_PhieuNhap = 0;
        $scope.object.NoiDung = $scope.ghichu;
        if ($scope.khohangselect != undefined)
            $scope.object.ID_Kho = ($scope.khohangselect.iD_Kho < 0) ? 0 : $scope.khohangselect.iD_Kho;

        let myGridAdd = $("#gridhangnhap").data("kendoGrid");
        let dataSource = myGridAdd.dataSource.data();
        let ChiTiet = [];
        for (i = 0; i < dataSource.length; i++) {
            if (dataSource[i].SoLuong <= 0) {
                flag = false;
                msg = $.i18n("label_soluongkhongthenhohon0");
                break;
            }

            var obj = {
                ID_HangHoa: dataSource[i].ID_HangHoa,
                SoLuong: dataSource[i].SoLuong
            }
            ChiTiet.push(obj);
        }
        $scope.object.ChiTiet = ChiTiet;

        if (flag && ($scope.object.ID_Kho <= 0 || $scope.object.ID_Kho == undefined)) {
            flag = false;
            msg = $.i18n("label_khohangkhongduocdetrong");
            $("#khohang").focus();
        }

        if (flag && ChiTiet.length <= 0) {
            flag = false;
            msg = $.i18n("label_chuachonmathangnhapkho");
        }

        if (!flag)
            Notification({ title: $.i18n("label_thongbao"), message: msg }, 'warning');

        return flag;
    }

    let fileUpload = '';
    function onUploadExcelSuccess(e) {
        var data = new FormData();
        data.append('file', e.files[0].rawFile);
        var files = e.files[0];
        $.ajax({
            url: urlApi + '/api/uploadfile/savefileExcel',
            processData: false,
            contentType: false,
            data: data,
            type: 'POST'
        }).done(function (result) {
            fileUpload = result;
        }).fail(function (a, b, c) {
            fileUpload = '';
        });
    }
    $("#filesExcel").kendoUpload({
        multiple: false,
        select: onUploadExcelSuccess,
        validation: {
            allowedExtensions: [".xls", ".xlsx"]
        },
        showFileList: true
    });
    $("#filesExcel").closest(".k-upload").find("span").text($.i18n("label_taifiledulieulen"))
    $("#files").closest(".k-upload").find("span").text($.i18n("label_chontep"))

    //event
    $scope.khohangOnChange = function () {
        $scope.khohangselect = this.khohangselect;
    }
    $scope.mathangOnChange = function () {
        $scope.mathangselect = this.mathangselect;
    }

    $scope.loadphieunhap = function () {
        loadgrid();
        loadgriddetail(0);
    }
    $scope.themphieunhap = function () {
        $scope.formdetail.center().open();

        loadgridhangnhap();

        $("#khohang").data("kendoComboBox").value("");
        $("#mathang").data("kendoComboBox").value("");
        $scope.soluong = 0;
        $scope.ghichu = '';
    }
    $scope.xuatexcel = function () {
        commonOpenLoadingText("#btn_xuatExcel");

        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        khoHangDataService.excellistphieunhap(fromdate, todate).then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n("label_thongbao"), message: $.i18n("label_coloixayravuilongloadlaitrang") }, 'warning');

            commonCloseLoadingText("#btn_xuatExcel")
        });
    }
    $scope.taifilemau = function () {
        commonOpenLoadingText("#btn_taifilemau");

        khoHangDataService.taifilemauphieunhap().then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n("label_thongbao"), message: $.i18n("label_coloixayravuilongloadlaitrang") }, 'warning');

            commonCloseLoadingText("#btn_taifilemau")
        });
    }
    $scope.importexcel = function () {
        $scope.formimport.center().open();
    }
    $scope.capNhatTuExcel = function () {
        commonOpenLoadingText("#btn_capnhatimport");

        khoHangDataService.importphieunhap(fileUpload).then(function (result) {
            if (result.flag)
                if (result.data.status == 200){
                    Notification({ title: $.i18n("label_thongbao"), message: $.i18n("label_tacvuthuchienthanhcong") }, 'success');
                    $scope.formimport.center().close();
                    loadgrid();
                }
                else if (result.data.status == 201)
                    commonDownFile(result.data);
                else if (result.data.status == 204)
                    Notification({ title: $.i18n("label_thongbao"), message: $.i18n("label_filemaukhongdungdinhdanghoackhongcobanghi") }, 'warning');
                else if (result.data.status == 411)
                    Notification({ title: $.i18n("label_thongbao"), message: $.i18n("label_soluongbanghikhongvuotqua2000dong") }, 'warning');
                else
                    Notification({ title: $.i18n("label_thongbao"), message: $.i18n("label_coloixayra_kiemtralaifilecapnhat") }, 'warning');
            else
                Notification({ title: $.i18n("label_thongbao"), message: $.i18n("label_coloixayra_loadlaitrang") }, 'warning');

            commonCloseLoadingText("#btn_capnhatimport")
        });
    }
    $scope.huyCapNhatExcel = function () {
        $scope.formimport.center().close();
    }

    $scope.luuphieunhap = function () {
        if (validate()) {
            commonOpenLoadingText("#btn_luuphieunhap");
            khoHangDataService.themmoiphieunhap($scope.object).then(function (result) {
                if (result.flag) {
                    $scope.formdetail.center().close();
                    $scope.object = {};
                    $scope.loadphieunhap();
                    Notification({ title: $.i18n("label_thongbao"), message: $.i18n('label_themmoithanhcong') }, 'success');
                }
                else
                    Notification({ title: $.i18n("label_thongbao"), message: $.i18n('label_themmoithatbaivuilongkiemtralaitruongdulieu') }, 'warning');

                commonCloseLoadingText("#btn_luuphieunhap")
            })
        }
    }
    $scope.huyluuphieunhap = function () {
        $scope.formdetail.center().close();
    }

    $scope.themdonghang = function () {
        let idmathang = 0;
        let tenhang = '';

        if ($scope.mathangselect != undefined) {
            idmathang = ($scope.mathangselect.id < 0) ? 0 : $scope.mathangselect.id;
            tenhang = $scope.mathangselect.name;
        }

        if (idmathang <= 0)
            Notification({ title: $.i18n("label_thongbao"), message: $.i18n("label_chuachonmathang") }, "error");
        else {
            var myGridAdd = $("#gridhangnhap").data("kendoGrid");
            var data = myGridAdd.dataSource.data();

            var obj = {
                ID_ChiTietPhieuNhap: 0,
                ID_PhieuNhap: 0,
                ID_Kho: 0,
                ID_HangHoa: idmathang,
                TenHang: tenhang,
                SoLuong: $scope.soluong,
                ThanhTien: 0
            }

            myGridAdd.dataSource.add(obj);
        }
    }
    $scope.deleterow = function (e) {
        let myGrid = $('#gridhangnhap').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;

        myGrid.dataSource.remove(dataItem);

    }
    $scope.fromDateChanged = function () {
        if ($scope.obj_TuNgay == null || $scope.obj_TuNgay > $scope.obj_DenNgay) {
            Notification({ title: $.i18n("label_thongbao"), message: $.i18n("label_tungaykhongthelonhondenngay") }, "error");
            $scope.obj_TuNgay = $scope.minDate;
        } else {
            $scope.minDate = $scope.obj_TuNgay;
        }
    }
    $scope.toDateChanged = function () {
        if ($scope.obj_DenNgay == null || $scope.obj_TuNgay > $scope.obj_DenNgay) {
            Notification({ title: $.i18n("label_thongbao"), message: $.i18n("label_tungaykhongthelonhondenngay") }, "error");
            $scope.obj_DenNgay = $scope.maxDate;
        } else {
            $scope.maxDate = $scope.obj_DenNgay;
        }
    }
    init();

})