angular.module('app').controller('phanQuyenKhachHangTheoNhanVienController', function (ComboboxDataService, $scope, $http, $location, Notification, phanQuyenDataService) {
    CreateSiteMap();

    let __idnhanvien = -1;

    let __arrkhachhangdaphan = [];
    let __arridkhachangchon = [];
    let __arridkhachhangthemquyen = [];

    function init() {
        getquyen();
        $scope.khachhangsaochep = '-';
        loadgridnhanvien();
        initgridphanquyen();
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

    function listColumnsgridnhanvien() {
        let dataList = [];

        dataList.push({ headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { class: "text-center" }, selectable: true, width: "40px" });
        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({
            field: "tenDayDu", title: $.i18n('header_tennhanvien'),
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px"
        });
        dataList.push({ field: "tenDangNhap", title: $.i18n('header_tendangnhap'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px" });

        return dataList;
    }
    function loadgridnhanvien() {
        kendo.ui.progress($("#gridNhanVien"), true);
        $scope.gridNhanVienOptions = {
            sortable: true,
            height: function () {
                let heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".group-btn").height());
                return heightGrid - 35;
            },
            dataBinding: function () {
                RecordNumber = (this.dataSource.page() - 1) * this.dataSource.pageSize();
            },
            dataBound: function (e) {
                e.sender.select("tr:eq(0)");
            },
            resizable: true,
            editable: false,
            filterable: {
                mode: "row"
            },
            selectable: "multiple, row",
            change: onChange,
            pageable: $scope.lang == 'vi-vn' ? pageableShort_vi : pageableShort_en,
            columns: listColumnsgridnhanvien()
        };

        phanQuyenDataService.getlistnhanvien().then(function (result) {
            $scope.gridNhanVienData = {
                data: result.data,
                schema: {
                    model: {
                        id: 'idnv',
                        fields: {
                            idnv: {
                                type: "number"
                            }
                        }
                    }
                },
                pageSize: 20
            };

            kendo.ui.progress($("#gridNhanVien"), false);
        });
    }
    function onChange(arg) {
        let grid = $("#gridNhanVien").data("kendoGrid");
        let listid = grid.selectedKeyNames();
        if (listid.length == 1)
            __idnhanvien = listid[0];
        else
            __idnhanvien = -1;

        loadgridphanquyen();
    }

    function listColumnsgridphanquyen() {
        let dataList = [];

        dataList.push({ headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { class: "text-center" }, selectable: true, width: "40px" });

        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({
            field: "maKH", title: $.i18n('header_makhachhang'),
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px"
        });
        dataList.push({
            field: "tenKhachHang", title: $.i18n('header_tenkhachhang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "175px"
        });
        dataList.push({
            field: "iD_Quyen", title: $.i18n('header_xemdaily'),
            template: function (e) {
                if (e.iD_Quyen != null && e.iD_Quyen.includes(";1;", 0)) {
                    return '<input type="checkbox" ng-disabled="(permission.sua <= 0)" id="xemdaily' + e.iD_KhachHang + '" ng-click="XoaQuyen(' + e.iD_KhachHang + ',\'' + e.iD_Quyen + '\',1)" checked="checked" class="k-checkbox">'
                        + '<label class="k-checkbox-label" for="xemdaily' + e.iD_KhachHang + '"></label>';
                } else {
                    return '<input type="checkbox" ng-disabled="(permission.sua <= 0)" id="xemdaily' + e.iD_KhachHang + '" ng-click="ThemQuyen(' + e.iD_KhachHang + ',\'' + e.iD_Quyen + '\',1)"  class="k-checkbox">'
                        + '<label class="k-checkbox-label" for="xemdaily' + e.iD_KhachHang + '"></label>';
                }
            },
            attributes: {
                style: "text-align: center;",
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: false,
            width: "100px"
        });
        dataList.push({
            field: "iD_Quyen", title: $.i18n('header_vaodiem'),
            template: function (e) {
                if (e.iD_Quyen != null && e.iD_Quyen.includes(";2;", 0)) {
                    return '<input type="checkbox" ng-disabled="(permission.sua <= 0)" id="giaohang' + e.iD_KhachHang + '" ng-click="XoaQuyen(' + e.iD_KhachHang + ',\'' + e.iD_Quyen + '\',2)" checked="checked" class="k-checkbox">'
                        + '<label class="k-checkbox-label" for="giaohang' + e.iD_KhachHang + '"></label>';
                } else {
                    return '<input type="checkbox" ng-disabled="(permission.sua <= 0)" id="giaohang' + e.iD_KhachHang + '" ng-click="ThemQuyen(' + e.iD_KhachHang + ',\'' + e.iD_Quyen + '\',2)"  class="k-checkbox">'
                        + '<label class="k-checkbox-label" for="giaohang' + e.iD_KhachHang + '"></label>';
                }
            },
            attributes: {
                style: "text-align: center;",
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: false,
            width: "100px"
        });
        dataList.push({
            field: "iD_Quyen", title: $.i18n('header_giaohang'),
            template: function (e) {
                if (e.iD_Quyen != null && e.iD_Quyen.includes(";3;", 0)) {
                    return '<input type="checkbox" ng-disabled="(permission.sua <= 0)" id="vaodiem' + e.iD_KhachHang + '" ng-click="XoaQuyen(' + e.iD_KhachHang + ',\'' + e.iD_Quyen + '\',3)" checked="checked" class="k-checkbox">'
                        + '<label class="k-checkbox-label" for="vaodiem' + e.iD_KhachHang + '"></label>';
                } else {
                    return '<input type="checkbox" ng-disabled="(permission.sua <= 0)" id="vaodiem' + e.iD_KhachHang + '" ng-click="ThemQuyen(' + e.iD_KhachHang + ',\'' + e.iD_Quyen + '\',3)"  class="k-checkbox">'
                        + '<label class="k-checkbox-label" for="vaodiem' + e.iD_KhachHang + '"></label>';
                }
            },
            attributes: {
                style: "text-align: center;",
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: false,
            width: "100px"
        });
        dataList.push({
            field: "iD_Quyen", title: $.i18n('header_thanhtoan'),
            template: function (e) {
                if (e.iD_Quyen != null && e.iD_Quyen.includes(";4;", 0)) {
                    return '<input type="checkbox" ng-disabled="(permission.sua <= 0)" id="thanhtoan' + e.iD_KhachHang + '" ng-click="XoaQuyen(' + e.iD_KhachHang + ',\'' + e.iD_Quyen + '\',4)" checked="checked" class="k-checkbox">'
                        + '<label class="k-checkbox-label" for="thanhtoan' + e.iD_KhachHang + '"></label>';
                } else {
                    return '<input type="checkbox" ng-disabled="(permission.sua <= 0)" id="thanhtoan' + e.iD_KhachHang + '" ng-click="ThemQuyen(' + e.iD_KhachHang + ',\'' + e.iD_Quyen + '\',4)"  class="k-checkbox">'
                        + '<label class="k-checkbox-label" for="thanhtoan' + e.iD_KhachHang + '"></label>';
                }
            },
            attributes: {
                style: "text-align: center;",
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: false,
            width: "100px"
        });

        dataList.push({
            field: "diaChi", title: $.i18n('header_diachi'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "250px"
        });
        return dataList;
    }
    function initgridphanquyen() {
        kendo.ui.progress($("#gridPhanQuyen"), true);
        $scope.gridPhanQuyenOptions = {
            sortable: true,
            height: function () {
                let heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".group-btn").height());
                return heightGrid - 35;
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
            columns: listColumnsgridphanquyen()
        };

        phanQuyenDataService.getdskhachhangdacapquyen(__idnhanvien).then(function (result) {
            __arrkhachhangdaphan = [];
            __arridkhachangchon = __arrkhachhangdaphan.map((ar, index, arr) => {
                return ar.iD_KhachHang
            })
            $scope.gridPhanQuyenData = {
                data: __arrkhachhangdaphan,
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
            kendo.ui.progress($("#gridPhanQuyen"), false);
        });
    }
    function loadgridphanquyen() {
        kendo.ui.progress($("#gridPhanQuyen"), true);

        phanQuyenDataService.getdskhachhangdacapquyen(__idnhanvien).then(function (result) {
            __arrkhachhangdaphan = result.data;
            __arridkhachangchon = __arrkhachhangdaphan.map((ar, index, arr) => {
                return ar.iD_KhachHang
            })
            $scope.gridPhanQuyenData = {
                data: __arrkhachhangdaphan,
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
            kendo.ui.progress($("#gridPhanQuyen"), false);
        });
    }

    function listColumnsgridkhachhang() {
        let dataList = [];

        dataList.push({
            field: "tacvu", title: $.i18n('header_tacvu'),
            template: '<button ng-click="themnhanvienquyen()" class="btn btn-link btn-menubar" title ="' + $.i18n('button_themnhanvienquyen') + '" ><i class="fas fa-plus-circle fas-sm color-infor"></i></button> ',
            attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "70px"
        });
        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({
            field: "maKH", title: $.i18n('header_makhachhang'),
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px"
        });
        dataList.push({
            field: "tenKhachHang", title: $.i18n('header_tenkhachhang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "175px"
        });
        dataList.push({
            field: "diaChi", title: $.i18n('header_diachi'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid
        });

        return dataList;
    }
    function loadgridkhachhang() {
        kendo.ui.progress($("#gridkhachhang"), true);
        $scope.gridkhachhangOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height();
                return heightGrid - 350;
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
            columns: listColumnsgridkhachhang()
        };

        phanQuyenDataService.getdskhachhangchuacapquyen(__idnhanvien).then(function (result) {
            let arr_khachhangall = result.data;
            let arr_khachhangchuachon = arr_khachhangall.filter((item) => {
                return (__arridkhachangchon.indexOf(item.iD_KhachHang) == -1)
            })

            $scope.gridkhachhangData = {
                data: arr_khachhangchuachon,
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
            kendo.ui.progress($("#gridkhachhang"), false);
        });
    }

    function listColumnsgridchon() {
        let dataList = [];

        dataList.push({
            field: "tacvu", title: $.i18n('header_tacvu'),
            template: '<button ng-click="xoanhanvienquyen()" class="btn btn-link btn-menubar" title ="' + $.i18n('button_xoa') + '" ><i class="fas fa-trash fas-sm color-danger"></i></button> ',
            attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "70px"
        });
        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({
            field: "maKH", title: $.i18n('header_makhachhang'),
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px"
        });
        dataList.push({
            field: "tenKhachHang", title: $.i18n('header_tenkhachhang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "175px"
        });
        dataList.push({ field: "diaChi", title: $.i18n('header_diachi'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid });

        return dataList;
    }
    function loadgridchon() {
        __arridkhachangchon = __arrkhachhangdaphan.map((ar, index, arr) => {
            return ar.iD_KhachHang
        })

        kendo.ui.progress($("#gridchon"), true);
        $scope.gridchonOptions = {
            sortable: true,
            height: function () {
                return 220;
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
            columns: listColumnsgridchon()
        };

        $scope.gridchonData = {
            data: [],
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
        kendo.ui.progress($("#gridchon"), false);
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
    init();

    $scope.taiFileMau = function () {
        commonOpenLoadingText("#btn_taifilemau");

        phanQuyenDataService.taifilemauphanquyen().then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayravuilongloadlaitrang') }, 'warning');

            commonCloseLoadingText("#btn_taifilemau")
        });
    }
    $scope.importExcel = function () {
        $scope.formimport.center().open();
    }
    $scope.xuatExcel = function () {
        commonOpenLoadingText("#btn_xuatexcel");

        phanQuyenDataService.exportExcel().then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayravuilongloadlaitrang') }, 'warning');

            commonCloseLoadingText("#btn_xuatexcel");
        });
    }

    $scope.capNhatTuExcel = function () {
        commonOpenLoadingText("#btn_capnhatimport");

        phanQuyenDataService.importphanquyen(fileUpload).then(function (result) {
            if (result.flag)
                if (result.data.status == 200) {
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_tacvuthuchienthanhcong') }, 'success');
                    $scope.formimport.center().close();
                    loadgridphanquyen();
                }
                else if (result.data.status == 201)
                    commonDownFile(result.data);
                else if (result.data.status == 204)
                    Notification({
                        title: $.i18n('label_thongbao'), message: $.i18n('label_filemaukhongdungdinhdanghoackhongtontaibanghi')
                    }, 'warning');
                else if (result.data.status == 411)
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_soluongbanghikhongvuotqua2000dong') }, 'warning');
                else
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloisayravuilongkiemtralaifilecapnhat') }, 'warning');
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayra_loadlaitrang') }, 'warning');

            commonCloseLoadingText("#btn_capnhatimport")
        });
    }
    $scope.huyCapNhatExcel = function () {
        $scope.formimport.center().close();
    }

    $scope.openformaddkhachhang = function () {
        if (__idnhanvien > 0) {
            $scope.xemdaily = true;
            $scope.vaodiem = true;
            $scope.giaohang = true;
            $scope.thanhtoan = true;

            __arridkhachhangthemquyen = [];

            loadgridkhachhang();
            loadgridchon();

            $scope.formdetailphanquyen.center().maximize().open();
        } else
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_canchonmotnhanviendethuchien') }, 'error');
    }
    $scope.xoaallquyen = function () {
        let grid = $('#gridPhanQuyen').data("kendoGrid");
        let listid = grid.selectedKeyNames();

        let flag = true;
        if (__idnhanvien <= 0) {
            flag = false;
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_canchonmotnhanviendethuchien') }, 'error');
        }

        if (flag && listid.length <= 0) {
            flag = false;
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_canchonkhachhangdethuchien') }, 'error');
        }

        if (flag) {
            openConfirm($.i18n("label_bancochacchanmuonxoakhong"), 'apDungXoaAllQuyen', null, listid);
        }
    }
    $scope.apDungXoaAllQuyen = function (_listid) {
        phanQuyenDataService.bophanquyenkhachhangchonhanvien(_listid, __idnhanvien).then(function (result) {
            if (result.flag) {
                loadgridphanquyen();
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
            } else {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'error');
            }
        });
    }

    $scope.ThemQuyen = function (_idkhachhang, _curquyen, _idquyen) {
        if (_curquyen == "null") {
            _curquyen = ";" + _idquyen + ";";
        } else {
            _curquyen += _idquyen + ";";
        }
        this.dataItem.iD_Quyen = _curquyen;
        $scope.gridPhanQuyen.refresh();
        let data = {
            ID_NhanVien: __idnhanvien,
            ID_KhachHang: _idkhachhang,
            ID_Quyen: _curquyen
        }
        phanQuyenDataService.updatePhanQuyen(data).then(function (result) {
            if (result.flag) {
                if (result.data.success) {
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.data.msg) }, 'success');
                } else {
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.data.msg) }, 'error');
                }
            } else {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.data.msg) }, 'error');
            }
        })
    }
    $scope.XoaQuyen = function (_idkhachhang, _curquyen, _idquyen) {
        if (_curquyen != "null")
            _curquyen = _curquyen.replace(_idquyen + ';', '');

        this.dataItem.iD_Quyen = _curquyen;
        $scope.gridPhanQuyen.refresh();
        let data = {
            ID_NhanVien: __idnhanvien,
            ID_KhachHang: _idkhachhang,
            ID_Quyen: _curquyen
        }
        phanQuyenDataService.updatePhanQuyen(data).then(function (result) {
            if (result.flag) {
                if (result.data.success) {
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.data.msg) }, 'success');
                } else {
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.data.msg) }, 'error');
                }
            } else {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.data.msg) }, 'error');
            }
        })
    }

    $scope.themnhanvienquyen = function (e) {
        let myGrid = $('#gridkhachhang').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;

        let myGridAdd = $("#gridchon").data("kendoGrid");
        myGridAdd.dataSource.add(dataItem);

        myGrid.dataSource.remove(dataItem);

        __arridkhachangchon.push(dataItem.iD_KhachHang);
        __arridkhachhangthemquyen.push(dataItem.iD_KhachHang);
    }
    $scope.xoanhanvienquyen = function (e) {
        let myGrid = $('#gridchon').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;

        let myGridAdd = $("#gridkhachhang").data("kendoGrid");
        myGridAdd.dataSource.add(dataItem);

        myGrid.dataSource.remove(dataItem);

        __arridkhachangchon = __arridkhachangchon.filter(item => item != dataItem.iD_KhachHang);
        __arridkhachhangthemquyen = __arridkhachhangthemquyen.filter(item => item != dataItem.iD_KhachHang);
    }

    $scope.luuphanquyen = function () {
        if (__arridkhachhangthemquyen.length > 0) {
            let idquyen = ';';
            if ($scope.xemdaily)
                idquyen += '1;';
            if ($scope.vaodiem)
                idquyen += '2;';
            if ($scope.giaohang)
                idquyen += '3;';
            if ($scope.thanhtoan)
                idquyen += '4;';
            if (idquyen == ';') {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_chuachonquyenapdungdethuchien') }, 'error');
            } else {
                commonOpenLoadingText("#btn_luuphanquyen");
                phanQuyenDataService.phanquyenkhachhangchonhanvien(__arridkhachhangthemquyen, __idnhanvien, idquyen).then(function (result) {
                    if (result.flag) {
                        $scope.formdetailphanquyen.center().maximize().close();
                        loadgridphanquyen();
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                    } else {
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'error');
                    }

                    commonCloseLoadingText("#btn_luuphanquyen");
                });
            }
        }
        else
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_chuacodulieukhachhangdethuchien') }, 'error');
    }
    $scope.huyluuphanquyen = function () {
        $scope.formdetailphanquyen.center().maximize().close();
    }

})