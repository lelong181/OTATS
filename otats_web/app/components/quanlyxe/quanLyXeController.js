angular.module('app').controller('quanLyXeController', function ($location, $rootScope, $scope, $state, Notification, ComboboxDataService, quanLyXeDataService) {
    CreateSiteMap();

    function init() {
        initcombobox();
        loadgrid();
        getquyen();
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
    let columnThaoTac = function () {
        var template = '<button ng-click="openform_SuDung()" class="btn btn-link btn-menubar" title ="' + $.i18n('label_lichsusudung') + '" ><i class="fas fa-history fas-sm color-infor"></i></button> '
            + '<button ng-click="openform_BaoDuong()" class="btn btn-link btn-menubar" title ="' + $.i18n('label_lichsubaoduong') + '" ><i class="fas fa-business-time fas-sm color-infor"></i></button> ';

        var obj = {
            template: template,
            title: $.i18n('header_tacvu'), width: "120px", attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
        }
        return obj;
    }
    function listColumnsgrid() {
        var dataList = [];

        dataList.push({ headerAttributes: { "class": "table-header-cell" }, selectable: true, width: "40px" });
        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({ field: "bienKiemSoat", title: $.i18n('header_bienkiemsoat'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px" });
        dataList.push({ field: "tenNhanVien", title: $.i18n('header_tennhanvien'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px" });
        dataList.push({ field: "namSX", title: $.i18n('header_namsanxuat'), attributes: { class: "text-center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px" });
        dataList.push({
            field: "ngayBDGanNhat", title: $.i18n('header_ngaybaoduonggannhat'),
            template: function (dataItem) {
                return kendo.htmlEncode(kendo.toString(dataItem.ngayBDGanNhat, formatDateTime));
            }, attributes: { class: "text-center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: false, width: "150px"
        });
        dataList.push({
            field: "ngayBDTiepTheo", title: $.i18n('header_ngaybaoduongtieptheo'),
            template: function (dataItem) {
                return kendo.htmlEncode(kendo.toString(dataItem.ngayBDTiepTheo, formatDateTime));
            }, attributes: { class: "text-center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: false, width: "150px"
        });
        dataList.push({ field: "chuKy", title: $.i18n('header_chuky'), attributes: { class: "text-center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px" });
        dataList.push({ field: "soCho", title: $.i18n('header_socho'), attributes: { class: "text-center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px" });
        dataList.push(columnThaoTac());

        return dataList;
    }
    function loadgrid() {
        kendo.ui.progress($("#grid"), true);
        $scope.gridOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".toolbarmenu").height());
                return heightGrid - 50;
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

        quanLyXeDataService.getlist().then(function (result) {
            $scope.gridData = {
                data: result.data,
                schema: {
                    model: {
                        id: 'id',
                        fields: {
                            ngayBDGanNhat: {
                                type: "date"
                            },
                            ngayBDTiepTheo: {
                                type: "date"
                            },
                        }
                    }
                },
                pageSize: 20
            };
            kendo.ui.progress($("#grid"), false);
        });
    }

    function validationOpenDetail(listRowsSelected) {
        let flag = true;
        let msg = "";
        if (listRowsSelected.length > 1) {
            msg = $.i18n('label_canchonmotxedethuchien');
            flag = false;
        } else if (listRowsSelected.length == 0) {
            flag = false;
            msg = $.i18n('label_chuachonxedethuchien');
        }
        if (!flag) {
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n(msg) }, "error");
        }
        return flag;
    }
    function openFormDetail(id) {
        $scope.idxe = id;
        $scope.formdetail.center().open();
        if (id > 0)
            loadeditThongtinxe(id);
        else
            loadaddThongtinxe();
    }
    function loadeditThongtinxe(id) {
        quanLyXeDataService.getbyid(id).then(function (result) {
            $scope.objectXe = result.data;

            if ($scope.objectXe.iD_NhanVien > 0)
                $("#nhanVien").data("kendoComboBox").value($scope.objectXe.iD_NhanVien);
            else
                $("#nhanVien").data("kendoComboBox").value("");

            $scope.obj_ngayBDGanNhat = new Date($scope.objectXe.ngayBDGanNhat);
            $scope.obj_ngayBDTiepTheo = new Date($scope.objectXe.ngayBDTiepTheo);

        });
    }
    function loadaddThongtinxe() {
        var d = new Date();
        $scope.obj_ngayBDGanNhat = d;
        $scope.obj_ngayBDTiepTheo = d;
        $scope.objectXe = {
            iD_Xe: 0,
            iD_NhanVien: 0,
            bienKiemSoat: '',
            loaiXe: '',
            chuKyBaoDuong: 0,
            namSanXuat: '',
            soCho: 0,
            moTa: ''
        }

        $("#nhanVien").data("kendoComboBox").value("");
    }
    function initcombobox() {
        ComboboxDataService.getDataNhanVien().then(function (result) {
            $scope.nhanvienData = result.data;
            if (!result.flag) {
                Notification({ message: $.i18n('label_khongtheloaddanhsachnhanvienvuilongtailaitrang') }, 'warning');
            }
        });
    }

    function validatethemsua() {
        let flag = true;
        let msg = '';

        if ($scope.objectXe.bienKiemSoat == '' || $scope.objectXe.bienKiemSoat == undefined) {
            flag = false;
            msg = $.i18n('label_bienkiemsoatkhongduocdetrong');
            $("#bienKiemSoat").focus();
        }

        if (flag && $scope.objectXe.iD_NhanVien <= 0) {
            flag = false;
            msg = $.i18n('label_nhanvienquanlykhongduocdetrong');
        }

        let namsanxuat = 0;
        if ($scope.objectXe.namSanXuat != '') {
            namsanxuat = Number($scope.objectXe.namSanXuat);

            if (flag && !namsanxuat) {
                namsanxuat = 0;
                flag = false;
                msg = $.i18n('label_namsanxuatkhongdungdinhdang');
                $("#namSX").focus();
            }
        }

        if (!flag)
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n(msg) }, 'warning');

        return flag;
    }

    function openConfirm(message, acceptAction, cancelAction, data) {
        var scope = angular.element("#mainContentId").scope();
        $(" <div id='confirmDelete'></div>").appendTo("body").kendoDialog({
            width: "450px",
            closable: true,
            modal: true,
            title: $.i18n('label_xacnhan'),
            content: message,
            actions: [
                {
                    text: $.i18n('button_huy'), primary: false, action: function () {
                        if (cancelAction != null) {
                            scope[cancelAction](data);
                        }
                    }
                },
                {
                    text: $.i18n('button_dongy'), primary: true, action: function () {
                        scope[acceptAction](data);
                    }
                }
            ],
        })
    }

    //Lịch sử bảo dưỡng
    function openformBaoDuong(id) {
        $scope.formdetail_BaoDuong.center().open();
        loadgrid_BaoDuong(id);
    }
    function loadgrid_BaoDuong(id) {
        kendo.ui.progress($("#grid"), true);
        $scope.gridOptions_BaoDuong = {
            sortable: true,
            height: function () {
                return 400;
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
            columns: listColumnsgridBaoDuong()
        };

        quanLyXeDataService.lichsubaoduong(id).then(function (result) {
            $scope.gridData_BaoDuong = {
                data: result.data,
                schema: {
                    model: {
                        id: 'id',
                        fields: {
                            ngayBaoDuong: {
                                type: "date"
                            },
                            ngayBDTiepTheo: {
                                type: "date"
                            },
                        }
                    }
                },
                pageSize: 20
            };
            kendo.ui.progress($("#grid"), false);
        });
    }
    function listColumnsgridBaoDuong() {
        let dataList = [];

        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({ field: "bienKiemSoat", title: $.i18n('header_bienkiemsoat'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px" });
        dataList.push({
            field: "ngayBaoDuong", title: $.i18n('header_ngaybaoduong'),
            template: function (dataItem) {
                return kendo.htmlEncode(kendo.toString(dataItem.ngayBaoDuong, formatDateTime));
            }, attributes: { class: "text-center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "soLuongAnh", title: $.i18n('header_soluonganh'),
            template: function (dataItem) {
                return "<a href='' class='color-primary' ng-click='openAlbumImage()'>" + kendo.htmlEncode(dataItem.soLuongAnh) + "</a>";
            },
            attributes: { class: "text-center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px"
        });
        dataList.push({ field: "diaDiemBaoDuong", title: $.i18n('header_diadiembaoduong'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "250px" });
        dataList.push({ field: "diaChiBaoDuong", title: $.i18n('header_diachi'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "250px" });
        dataList.push({ field: "noiDung", title: $.i18n('header_noidung'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "250px" });
        dataList.push({
            field: "chiPhi", title: $.i18n('header_chiphi'),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "250px"
        });
        dataList.push({
            field: "ngayBDTiepTheo", title: $.i18n('header_ngaybaoduongtieptheo'),
            template: function (dataItem) {
                return kendo.htmlEncode(kendo.toString(dataItem.ngayBDTiepTheo, formatDateTime));
            }, attributes: { class: "text-center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });

        return dataList;
    }

    //Lịch sử sử dụng
    function openformSuDung(id) {
        $scope.formdetail_SuDung.center().open();
        loadgrid_SuDung(id);
    }
    function loadgrid_SuDung(id) {
        kendo.ui.progress($("#grid"), true);
        $scope.gridOptions_SuDung = {
            sortable: true,
            height: function () {
                return 400;
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
            columns: listColumnsgridSuDung()
        };

        quanLyXeDataService.lichsusudung(id).then(function (result) {
            $scope.gridData_SuDung = {
                data: result.data,
                schema: {
                    model: {
                        id: 'id',
                        fields: {
                            apDungTuNgay: {
                                type: "date"
                            },
                            apDungDenNgay: {
                                type: "date"
                            },
                        }
                    }
                },
                pageSize: 20
            };
            kendo.ui.progress($("#grid"), false);
        });
    }
    function listColumnsgridSuDung() {
        let dataList = [];

        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({ field: "bienKiemSoat", title: $.i18n('header_bienkiemsoat'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px" });
        dataList.push({ field: "tenNhanVien", title: $.i18n('header_tennhanvien'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "250px" });
        dataList.push({
            field: "apDungTuNgay", title: $.i18n('header_ngaybatdau'),
            template: function (dataItem) {
                d = dataItem.apDungTuNgay;
                if (d.getFullYear() <= 1900)
                    return '';
                else
                    return kendo.htmlEncode(kendo.toString(dataItem.apDungTuNgay, formatDateTime));
            }, attributes: { class: "text-center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "apDungDenNgay", title: $.i18n('header_ngayketthuc'),
            template: function (dataItem) {
                d = dataItem.apDungDenNgay;
                if (d.getFullYear() <= 1900)
                    return '';
                else
                    return kendo.htmlEncode(kendo.toString(dataItem.apDungDenNgay, formatDateTime));
            }, attributes: { class: "text-center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: false, width: "150px"
        });

        return dataList;
    }

    //event
    $scope.xuatExcel = function () {
        commonOpenLoadingText("#btn_exportexcel");

        quanLyXeDataService.exportExcel().then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayravuilongloadlaitrang') }, 'warning');

            commonCloseLoadingText("#btn_exportexcel");
        });
    }
    $scope.openform_BaoDuong = function () {
        let myGrid = $('#grid').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;

        id = dataItem.id;
        openformBaoDuong(id);
    }
    $scope.openform_SuDung = function () {
        let myGrid = $('#grid').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;

        id = dataItem.id;

        openformSuDung(id);
    }

    $scope.openAlbumImage = function () {
        let myGrid = $('#grid').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;
        var url = $state.href('album', { idlichsubaoduong: dataItem.iD_Xe_LichSuBaoDuong });
        window.open(url, '_blank');
    }

    $scope.nhanvienOnChange = function () {
        $scope.nhanvienselect = this.nhanvienselect;

        let idnhanvien = 0;
        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;

        $scope.objectXe.iD_NhanVien = idnhanvien;
    }

    $scope.luuThongTinXe = function () {
        if (validatethemsua()) {
            let ngayBDGanNha = '';
            let ngayBDTiepTheo = '';

            if ($scope.obj_ngayBDGanNhat != undefined && $scope.obj_ngayBDGanNhat != null)
                ngayBDGanNha = kendo.toString($scope.obj_ngayBDGanNhat, formatDateTimeFilter);

            if ($scope.obj_ngayBDTiepTheo != undefined && $scope.obj_ngayBDTiepTheo != null)
                ngayBDTiepTheo = kendo.toString($scope.obj_ngayBDTiepTheo, formatDateTimeFilter);

            let obj = {
                BienKiemSoat: $scope.objectXe.bienKiemSoat,
                ChuKyBaoDuong: $scope.objectXe.chuKyBaoDuong,
                ID_NhanVien: $scope.objectXe.iD_NhanVien,
                ID_Xe: $scope.objectXe.iD_Xe,
                LoaiXe: $scope.objectXe.loaiXe,
                MoTa: $scope.objectXe.moTa,
                NamSanXuat: $scope.objectXe.namSanXuat,
                NgayBDGanNhat: ngayBDGanNha,
                NgayBDTiepTheo: ngayBDTiepTheo,
                //NgayTao: $scope.objectXe.ngayTao,
                SoCho: $scope.objectXe.soCho,
            };

            commonOpenLoadingText("#btn_luuThongTinXe");
            quanLyXeDataService.themsuaxe(obj).then(function (result) {
                if (result.flag) {
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                    loadgrid();
                    $scope.formdetail.center().close();
                }
                else
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');

                commonCloseLoadingText("#btn_luuThongTinXe");
            });
        }
    }
    $scope.huyLuuThongTinXe = function () {
        $scope.formdetail.center().close();
    }

    $scope.addXe = function () {
        openFormDetail(0);
    }
    $scope.editXe = function () {
        let listRowsSelected = commonGetRowSelected("#grid");
        if (validationOpenDetail(listRowsSelected)) {
            openFormDetail(listRowsSelected[0].id);
        }
    }

    $scope.deleteXe = function () {
        let arr = $("#grid").data("kendoGrid").selectedKeyNames();
        console.log(arr);

        if (arr.length <= 0)
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_chuachonxenao') }, 'warning');
        else {
            let data = [];
            for (let i = 0; i < arr.length; i++) {
                data.push(parseInt(arr[i]));
            }
            if (data == []) {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_khongthexoanhungxedacothogtinbaoduong') }, 'warning');
            } else {
                openConfirm($.i18n('label_bancochacmuonxoaxenay'), 'apDungXoaXe', null, data);
            }
        }
    }
    $scope.apDungXoaXe = function (data) {
        quanLyXeDataService.xoaxe(data).then(function (result) {
            if (result.flag) {
                Notification({ title: $.i18n('label_thongbao'), message: result.message }, 'success');
            }
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');

            loadgrid();
        });
    }

    init();

    $("#grid").on("dblclick", "tr[role='row']", function () {
        let grid = $("#grid").data("kendoGrid");
        let row = grid.table.find("[data-uid=" + $(this).attr("data-uid") + "]");

        grid.clearSelection();
        grid.select(row);

        let selectedItem = grid.dataItem(grid.select());

        openFormDetail(selectedItem.id);
    })

})