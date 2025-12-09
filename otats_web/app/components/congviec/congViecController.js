const newLocal = angular.module('app').controller('congViecController', function($scope, $rootScope, $timeout, Notification, ComboboxDataService, congViecDataService) {
    CreateSiteMap();

    let data_all = [];
    let data_chuatiepnhan = [];
    let data_dangxuly = [];
    let data_daketthuc = [];
    let idnhanvien = 0;
    let idnhomnhanvien = 0;

    function init() {
        loaddatagrid();
        initCombo();

    }
    function initCombo() {
        ComboboxDataService.getDataNhomNhanVien().then(function (result) {
            let ar = result.data;
            ar.shift();
            $scope.nhomnhanvienData = ar;
        });
        
        $scope.dscongviec_file_option = {
            multiple: true,
            validation: {
                allowedExtensions: [".xls", ".xlsx", ".doc", ".txt", ".docx", ".png", ".jpeg", ".jpg"]
            },
            showFileList: true,
            remove: function () {
                $scope.filedinhkem = "";
            }
        }
    }
    function loaddatagrid() {
        congViecDataService.getListCongViec().then(function(response) {
            data_all = response.data.table;
            data_chuatiepnhan = response.data.table2;
            data_dangxuly = response.data.table4;
            data_daketthuc = response.data.table6;
            loadgridChuaTiepNhan();
        });
    }
    function validatethem() {
        let flag = true;
        let msg = '';

        if ($scope.objectCongViec.tenCongViec == '' || $scope.objectCongViec.tenCongViec == undefined) {
            flag = false;
            msg = $.i18n("label_tencongvieckhongduocdetrong");
            $("#tenCongViec").focus();
        }
        if ($scope.ngayhethan == '' || $scope.ngayhethan == undefined) {
            flag = false;
            msg = $.i18n("label_ngayhethankhongduocdetrong");
            $("#ngayhethan").focus();
        }
        if ($scope.objectCongViec.diaDiemDi == '' || $scope.objectCongViec.diaDiemDi == undefined) {
            flag = false;
            msg = $.i18n("label_diadiemdikhongduocdetrong");
            $("#diaDiemDi").focus();
        }
        if ($scope.objectCongViec.soNguoiNhan == '' || $scope.objectCongViec.soNguoiNhan == undefined) {
            flag = false;
            msg = $.i18n("label_songuoinhankhongduocdetrong");
            $("#soNguoiNhan").focus();
        }
        if ($scope.objectCongViec.noiDungCongViec == '' || $scope.objectCongViec.noiDungCongViec == undefined) {
            flag = false;
            msg = $.i18n("label_noidungcongvieckhongduocdetrong");
            $("#noiDungCongViec").focus();
        }
        if ($scope.objectCongViec.diaDiemDen == '' || $scope.objectCongViec.diaDiemDen == undefined) {
            flag = false;
            msg = $.i18n("label_diadiemdenkhongduocdetrong");
            $("#diaDiemDen").focus();
        }

        if (!flag)
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n(msg) }, 'warning');
        return flag;
    }

    function listColumnsgridChuaTiepNhan() {
        let dataList = [];
        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { class: "text-center" },
        });
        dataList.push({
            field: "ngayTao", title: $.i18n("header_ngaytao"), attributes: { style: "text-align: center" },
            template: function (dataItem) {
                return kendo.htmlEncode(kendo.toString(dataItem.ngayTao, formatDate));
            },
            headerAttributes: {
                "class": "table-header-cell", style: "text-align: center"
            }, filterable: false, width: "100px"
        });
        dataList.push({
            field: "tenNguoiGui", title: $.i18n("header_nguoiguicongviec"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "170px"
        });
        dataList.push({
            field: "tenNhom", title: $.i18n("header_nhomnhancongviec"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "170px"
        });
        dataList.push({
            field: "nguoiDuocGiao", title: $.i18n("header_nguoiduocgiao"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "170px"
        });
        dataList.push({
            field: "tenCongViec", title: $.i18n("header_tencongviec"),
            template: function(dataItem) {
                return "<a href='' class='color-primary' ng-click='formdetailGrid(" + kendo.htmlEncode(dataItem.iD_CongViec) + ")'>" + kendo.htmlEncode(dataItem.tenCongViec) + "</a>";
            },
            footerAttributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px"
        });
        dataList.push({
            field: "noiDung", title: $.i18n("header_noidung"), footerAttributes: { style: "text-align: right" }, attributes: { style: "text-align: right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "400px"
        });
        dataList.push({
            field: "diaDiemDi", title: $.i18n("header_diadiemdi"), footerAttributes: { style: "text-align: right" }, attributes: { style: "text-align: right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "300px"
        });
        dataList.push({
            field: "diaDiemDen", title: $.i18n("header_diadiemden"), footerAttributes: { style: "text-align: right" }, attributes: { style: "text-align: right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "300px"
        });
        dataList.push({
            field: "ngayHetHan", title: $.i18n("header_ngayhethan"),
            template: function (dataItem) {
                return kendo.htmlEncode(kendo.toString(dataItem.ngayHetHan, formatDate));
            },
            footerAttributes: { style: "text-align: right" }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: false, width: "150px"
        });
        dataList.push({
            field: "tenTrangThai", title: $.i18n("header_trangthai"),
            template: function (dataItem) {
                return "<a href='' class='color-primary' ng-click='formdetailstateGrid(" + kendo.htmlEncode(dataItem.iD_CongViec) + ")'>" + kendo.htmlEncode(dataItem.tenTrangThai) + "</a>";
            },
            footerAttributes: { style: "text-align: right" }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        return dataList;
    }
    function listColumnsgridDangXuLy() {
        let dataList = [];
        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { class: "text-center" },
        });
        dataList.push({
            field: "ngayTao", title: $.i18n("header_ngaytao"), attributes: { style: "text-align: center" },
            template: function (dataItem) {
                return kendo.htmlEncode(kendo.toString(dataItem.ngayTao, formatDate));
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "tenNguoiGui", title: $.i18n("header_nguoiguicongviec"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "170px"
        });
        dataList.push({
            field: "tenNhom", title: $.i18n("header_nhomnhancongviec"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "170px"
        });
        dataList.push({
            field: "nguoiDuocGiao", title: $.i18n("header_nguoiduocgiao"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "170px"
        });
        dataList.push({
            field: "tenCongViec", title: $.i18n("header_tencongviec"),
            template: function(dataItem) {
                return "<a href='' class='color-primary' ng-click='formdetailGrid(" + kendo.htmlEncode(dataItem.iD_CongViec) + ")'>" + kendo.htmlEncode(dataItem.tenCongViec) + "</a>";
            },
            footerAttributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px"
        });
        dataList.push({
            field: "noiDung", title: $.i18n("header_noidung"), footerAttributes: { style: "text-align: right" }, attributes: { style: "text-align: right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "400px"
        });
        dataList.push({
            field: "diaDiemDi", title: $.i18n("header_diadiemdi"), footerAttributes: { style: "text-align: right" }, attributes: { style: "text-align: right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "300px"
        });
        dataList.push({
            field: "diaDiemDen", title: $.i18n("header_diadiemden"), footerAttributes: { style: "text-align: right" }, attributes: { style: "text-align: right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "300px"
        });
        dataList.push({
            field: "ngayHetHan", title: $.i18n("header_ngayhethan"),
            template: function (dataItem) {
                return kendo.htmlEncode(kendo.toString(dataItem.ngayHetHan, formatDate));
            },
            footerAttributes: { style: "text-align: right" }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "ngayNhan", title: $.i18n("header_ngaynhan"),
            template: function (dataItem) {
                return kendo.htmlEncode(kendo.toString(dataItem.ngayNhan, formatDateTime));
            },
            footerAttributes: { style: "text-align: right" }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "tenTrangThai", title: $.i18n("header_trangthai"), template: function(dataItem) {
                return "<a href='' class='color-primary' ng-click='formdetailstateGrid(" + kendo.htmlEncode(dataItem.iD_CongViec) + ")'>" + kendo.htmlEncode(dataItem.tenTrangThai) + "</a>";
            }, footerAttributes: { style: "text-align: right" }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        return dataList;
    }
    function listColumnsgridDaKetThuc() {
        let dataList = [];
        
        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { class: "text-center" },
        });
        dataList.push({
            field: "ngayTao", title: $.i18n("header_ngaytao"), attributes: { style: "text-align: center" },
            template: function (dataItem) {
                return kendo.htmlEncode(kendo.toString(dataItem.ngayTao, formatDate));
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "tenNguoiGui", title: $.i18n("header_nguoiguicongviec"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "170px"
        });
        dataList.push({
            field: "tenNhom", title: $.i18n("header_nhomnhancongviec"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "170px"
        });
        dataList.push({
            field: "nguoiDuocGiao", title: $.i18n("header_nguoiduocgiao"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "170px"
        });
        dataList.push({
            field: "tenCongViec", title: $.i18n("header_tencongviec"),
            template: function(dataItem) {
                return "<a href='' class='color-primary' ng-click='formdetailGrid(" + kendo.htmlEncode(dataItem.iD_CongViec) + ")'>" + kendo.htmlEncode(dataItem.tenCongViec) + "</a>";
            }, footerAttributes: { style: "text-align: center" },  headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px"
        });
        dataList.push({
            field: "noiDung", title: $.i18n("header_noidung"), footerAttributes: { style: "text-align: right" }, attributes: { style: "text-align: right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "400px"
        });
        dataList.push({
            field: "diaDiemDi", title: $.i18n("header_diadiemdi"), footerAttributes: { style: "text-align: right" }, attributes: { style: "text-align: right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "300px"
        });
        dataList.push({
            field: "diaDiemDen", title: $.i18n("header_diadiemden"), footerAttributes: { style: "text-align: right" }, attributes: { style: "text-align: right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "300px"
        });
        dataList.push({
            field: "ngayHetHan", title: $.i18n("header_ngayhethan"),
            template: function (dataItem) {
                return kendo.htmlEncode(kendo.toString(dataItem.ngayHetHan, formatDate));
            },
            footerAttributes: { style: "text-align: right" }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "ngayNhan", title: $.i18n("header_ngaynhan"),
            template: function (dataItem) {
                return kendo.htmlEncode(kendo.toString(dataItem.ngayNhan, formatDateTime));
            },
            footerAttributes: { style: "text-align: right" }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "ngayHoanThanh", title: $.i18n("header_ngayhoanthanh"),
            template: function (dataItem) {
                return kendo.htmlEncode(kendo.toString(dataItem.ngayHoanThanh, formatDateTime));
            },
            footerAttributes: { style: "text-align: right" }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "tenTrangThai", title: $.i18n("header_trangthai"), template: function(dataItem) {
                return "<a href='' class='color-primary' ng-click='formdetailstateGrid(" + kendo.htmlEncode(dataItem.iD_CongViec) + ")'>" + kendo.htmlEncode(dataItem.tenTrangThai) + "</a>";
            }, footerAttributes: { style: "text-align: right" }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        return dataList;
    }
    function listColumnsgridTatCa() {
        let dataList = [];
        
        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { class: "text-center" },
        });
        dataList.push({
            field: "ngayTao", title: $.i18n("header_ngaytao"), attributes: { style: "text-align: center" },
            template: function (dataItem) {
                return kendo.htmlEncode(kendo.toString(dataItem.ngayTao, formatDate));
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "tenNguoiGui", title: $.i18n("header_nguoiguicongviec"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "170px"
        });
        dataList.push({
            field: "tenNhom", title: $.i18n("header_nhomnhancongviec"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "170px"
        });
        dataList.push({
            field: "nguoiDuocGiao", title: $.i18n("header_nguoiduocgiao"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "170px"
        });
        dataList.push({
            field: "tenCongViec", title: $.i18n("header_tencongviec"),
            template: function(dataItem) {
                return "<a href='' class='color-primary' ng-click='formdetailGrid(" + kendo.htmlEncode(dataItem.iD_CongViec) + ")'>" + kendo.htmlEncode(dataItem.tenCongViec) + "</a>";
            }, footerAttributes: { style: "text-align: center" },  headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px"
        });
        dataList.push({
            field: "noiDung", title: $.i18n("header_noidung"), footerAttributes: { style: "text-align: right" }, attributes: { style: "text-align: right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "400px"
        });
        dataList.push({
            field: "diaDiemDi", title: $.i18n("header_diadiemdi"), footerAttributes: { style: "text-align: right" }, attributes: { style: "text-align: right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "300px"
        });
        dataList.push({
            field: "diaDiemDen", title: $.i18n("header_diadiemden"), footerAttributes: { style: "text-align: right" }, attributes: { style: "text-align: right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "300px"
        });
        dataList.push({
            field: "ngayHetHan", title: $.i18n("header_ngayhethan"),
            template: function (dataItem) {
                return kendo.htmlEncode(kendo.toString(dataItem.ngayHetHan, formatDate));
            },
            footerAttributes: { style: "text-align: right" }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "ngayNhan", title: $.i18n("header_ngaynhan"),
            template: function (dataItem) {
                if (dataItem.ngayNhan != null) {
                    let date = new Date(dataItem.ngayNhan);
                    if (date == null || date.getFullYear() < 1900)
                        return '';
                    else
                        return kendo.htmlEncode(kendo.toString(date, formatDateTime));
                }
                else
                    return '';
            },
            footerAttributes: { style: "text-align: right" }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "ngayHoanThanh", title: $.i18n("header_ngayhoanthanh"),
            template: function (dataItem) {
                if (dataItem.ngayHoanThanh != null) {
                    let date = new Date(dataItem.ngayHoanThanh);
                    if (date == null || date.getFullYear() < 1900)
                        return '';
                    else
                        return kendo.htmlEncode(kendo.toString(date, formatDateTime));
                }
                else
                    return '';
            },
            footerAttributes: { style: "text-align: right" }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "tenTrangThai", title: $.i18n("header_trangthai"), template: function(dataItem) {
                return "<a href='' class='color-primary' ng-click='formdetailstateGrid(" + kendo.htmlEncode(dataItem.iD_CongViec) + ")'>" + kendo.htmlEncode(dataItem.tenTrangThai) + "</a>";
            }, footerAttributes: { style: "text-align: center" }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        return dataList;
    }

    function listColumnsgriddetail() {
        let dataList = [];

        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { class: "text-center" },
        });
        dataList.push({
            field: "tenNhanVien", title: $.i18n("header_nhanvien"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "170px"
        });
        dataList.push({
            field: "tenTrangThai", title: $.i18n("header_trangthai"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "170px"
        });
        dataList.push({
            field: "ngayHoanThanh", title: $.i18n("header_ngayhoanthanh"), attributes: { style: "text-align: center" },
            template: function (dataItem) {
                let d = dataItem.ngayHoanThanh;
                if (d == null)
                    return ''
                else
                    return kendo.htmlEncode(kendo.toString(d, formatDate));
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        
        return dataList;
    }
    function listColumnsgridstate() {
        let dataList = [];

        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { class: "text-center" },
        });
        dataList.push({
            field: "tenNhanVien", title: $.i18n("header_nhanvien"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "170px"
        });
        dataList.push({
            field: "tenTrangThai", title: $.i18n("header_trangthai"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "170px"
        });
        dataList.push({
            field: "ngayCapNhat", title: $.i18n("header_ngaycapnhat"), attributes: { style: "text-align: center" },
            template: function (dataItem) {
                let d = dataItem.ngayCapNhat;
                if (d == null)
                    return ''
                else
                    return kendo.htmlEncode(kendo.toString(d, formatDate));
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });

        return dataList;
    }

    function loadgridChuaTiepNhan() {
        $scope.gridOptions_chuatiepnhan = {
            sortable: true,
            height: function() {
                var heightGrid = $(window).height() - ($(".sitemap").height() + $(".toolbarmenu").height());
                return heightGrid - 140;
            },
            dataBinding: function() {
                RecordNumber = (this.dataSource.page() - 1) * this.dataSource.pageSize();
            },
            resizable: true,
            editable: false,
            filterable: {
                mode: "row"
            },
            pageable: $scope.lang == 'vi-vn' ? pageableShort_vi : pageableShort_en,
            columns: listColumnsgridChuaTiepNhan()
        };
        $scope.gridData_chuatiepnhan = {
            data: data_chuatiepnhan,
            schema: {
                model: {
                    fields: {
                        ngayTao: {
                            type: "date"
                        },
                        ngayHetHan: {
                            type: "date"
                        },
                    }
                }
            },
            pageSize: 20,
        };
    }
    function loadgridDangXuLy() {
        $scope.gridOptions_dangxuly = {
            sortable: true,
            height: function() {
                var heightGrid = $(window).height() - ($(".sitemap").height());
                return heightGrid - 140;
            },
            dataBinding: function() {
                RecordNumber = (this.dataSource.page() - 1) * this.dataSource.pageSize();
            },
            resizable: true,
            editable: false,
            filterable: {
                mode: "row"
            },
            pageable: $scope.lang == 'vi-vn' ? pageableShort_vi : pageableShort_en,
            columns: listColumnsgridDangXuLy()
        };
        $scope.gridData_dangxuly = {
            data: data_dangxuly,
            schema: {
                model: {
                    fields: {
                        ngayTao: {
                            type: "date"
                        }, ngayHetHan: {
                            type: "date"
                        },
                        ngayNhan: {
                            type: "date"
                        },
                    }
                }
            },
            pageSize: 20,
        };
    }
    function loadgridDaKetThuc() {
        $scope.gridOptions_daketthuc = {
            sortable: true,
            height: function() {
                var heightGrid = $(window).height() - ($(".sitemap").height());
                return heightGrid - 140;
            },
            dataBinding: function() {
                RecordNumber = (this.dataSource.page() - 1) * this.dataSource.pageSize();
            },
            resizable: true,
            editable: false,
            filterable: {
                mode: "row"
            },
            pageable: $scope.lang == 'vi-vn' ? pageableShort_vi : pageableShort_en,
            columns: listColumnsgridDaKetThuc()
        };
        $scope.gridData_daketthuc = {
            data: data_daketthuc,
            schema: {
                model: {
                    fields: {
                        ngayTao: {
                            type: "date"
                        },
                        ngayHetHan: {
                            type: "date"
                        },
                        ngayNhan: {
                            type: "date"
                        },
                        ngayHoanThanh: {
                            type: "date"
                        },
                    }
                }
            },
            pageSize: 20,
        };
    }
    function loadgridTatCa() {
        $scope.gridOptions_tatca = {
            sortable: true,
            height: function() {
                var heightGrid = $(window).height() - ($(".sitemap").height());
                return heightGrid - 140;
            },
            dataBinding: function() {
                RecordNumber = (this.dataSource.page() - 1) * this.dataSource.pageSize();
            },
            resizable: true,
            editable: false,
            filterable: {
                mode: "row"
            },
            pageable: $scope.lang == 'vi-vn' ? pageableShort_vi : pageableShort_en,
            columns: listColumnsgridTatCa()
        };
        $scope.gridData_tatca = {
            data: data_all,
            schema: {
                model: {
                    fields: {
                        ngayTao: {
                            type: "date"
                        },
                        ngayHetHan: {
                            type: "date"
                        },
                        ngayNhan: {
                            type: "date"
                        },
                        ngayHoanThanh: {
                            type: "date"
                        },
                    }
                }
            },
            pageSize: 20,
        };
    }
    
    $scope.chuaTiepNhan = function() {
        loadgridChuaTiepNhan();
    };
    $scope.dangXuLy = function() {
        $timeout(loadgridDangXuLy, 200);
    };
    $scope.daKetThuc = function() {
        $timeout(loadgridDaKetThuc, 200);
    };
    $scope.tatCa = function() {
        $timeout(loadgridTatCa, 200);
    };

    $scope.themMoi = function() {
        let d = new Date();
        $scope.ngayhethan = d;
        $scope.minDate = d;

        $scope.objectCongViec = {};
        $scope.selectedIdNhanVien = [];
        $scope.selectedIdNhom = [];
        $scope.filedinhkem = '';

        $scope.formdetail.center().maximize().open();
    };
    $scope.huy = function() {
        $scope.formdetail.center().close();
    };
    $scope.luuCongViec = function() {
        if (validatethem()) {
            let ngayhethan = '';
            let idnhom = 0;

            $scope.objectCongViec.checkIn = $scope.objectCongViec.checkIn ? 1 : 0;
            $scope.objectCongViec.chupAnh = $scope.objectCongViec.chupAnh ? 1 : 0;
            $scope.objectCongViec.hoanThanh = $scope.objectCongViec.hoanThanh ? 1 : 0;

            if ($scope.nhomnhanvienselect != undefined)
                idnhom = $scope.nhomnhanvienselect.iD_Nhom;

            let obj = {
                TenCongViec: $scope.objectCongViec.tenCongViec,
                BatBuocCheckin: $scope.objectCongViec.checkIn,
                BatBuocChupAnh: $scope.objectCongViec.chupAnh,
                BatBuocDeadline: $scope.objectCongViec.hoanThanh,
                DiaDiemDen: $scope.objectCongViec.diaDiemDen,
                DiaDiemDi: $scope.objectCongViec.diaDiemDi,
                ID_CongViec: 0,
                ID_NhanVienDuocGiao: $scope.selectedIdNhanVien,
                ID_NhomNhan: idnhom,
                KinhDoDiemDen: 0,
                KinhDoDiemDi: 0,
                NgayHetHan: kendo.toString($scope.ngayhethan, formatDateTimeFilter),
                FileGiaoViec: $scope.filedinhkem,
                NoiDung: $scope.objectCongViec.noiDungCongViec,
                SoNguoiNhan: $scope.objectCongViec.soNguoiNhan,
                SoTien: $scope.objectCongViec.soTienThuong,
                SoTienHang: $scope.objectCongViec.soTienHang,
                ViDoDiemDen: 0,
                ViDoDiemDi: 0,
            };

            congViecDataService.addCongViec(obj).then(function(result) {
                if (result.flag) {
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                    loaddatagrid();
                    $scope.formdetail.center().close();
                }
                else
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
            });
        }
    };
    $scope.nhomnhanvienOnChange = function() {
        $scope.nhomnhanvienselect = this.nhomnhanvienselect;
        let idnhom = $scope.nhomnhanvienselect.iD_Nhom;

        congViecDataService.getlistnhanvienbymultiidnhom({ IDNhom: idnhom }).then(function (result) {
            if (result.flag) {
                $scope.nhanvienData = result.data;
            }
            else
                $scope.nhanvienData = [];
        });
    };
    $scope.nhanvienOnChange= function() {
        $scope.selectedIdNhanVien = this.selectedIdNhanVien;  
    };
    
    $scope.changeDiemDi = function(e) {
        let geocoder = new google.maps.Geocoder();
        geocoder.geocode({ 'location': e.latLng }, function (results, status) {
            if (status === 'OK') {
                if (results[0]) {
                    $("#diaDiemDi").val(results[0].formatted_address);
                    $scope.objectCongViec.diaDiemDi = results[0].formatted_address;
                } else {
                    alert($.i18n("label_khongtimduocdiachi"));
                }
            } else {
                alert($.i18n("label_daxayraloi") + status);
            }
        });
    }
    $scope.changeDiemDen = function(e) {
        let geocoder = new google.maps.Geocoder();
        geocoder.geocode({ 'location': e.latLng }, function (results, status) {
            console.log(status);
            if (status === 'OK') {
                if (results[0]) {
                    $("#diaDiemDen").val(results[0].formatted_address);
                    $scope.objectCongViec.diaDiemDen = results[0].formatted_address;
                } else {
                    alert($.i18n("label_khongtimduocdiachi"));
                }
            } else {
                alert($.i18n("label_daxayraloi") + status);
            }
        });
    }

    $scope.formdetailGrid = function (idcongviec) {
        $scope.formchitietcongviec.center().open();
        $scope.griddetailOptions = {
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
            columns: listColumnsgriddetail()
        };
        congViecDataService.getChiTietCongViec(idcongviec).then(function (response) {
            $scope.griddetailData = {
                data: response.data,
                schema: {
                    model: {
                        fields: {
                            ngayHoanThanh: {
                                type: "date"
                            }
                        }
                    }
                },
                pageSize: 20,
            };
        });
    }
    $scope.formdetailstateGrid = function (idcongviec) {
        $scope.formchitiettrangthai.center().open();
        $scope.gridstateOptions = {
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
            columns: listColumnsgridstate()
        };
        congViecDataService.getChiTietTrangThaiCongViec(idcongviec).then(function (response) {
            $scope.gridstateData = {
                data: response.data,
                schema: {
                    model: {
                        fields: {
                            ngayCapNhat: {
                                type: "date"
                            }
                        }
                    }
                },
                pageSize: 20,
            };
        });
    }
    
    init();

    $scope.onUploadImageSuccess = function(e) {
        var data = new FormData();
        var files = e.files[0];
        for (var i = 0; i < e.files.length; i++) {
            data.append('file', e.files[i].rawFile);
        }

        congViecDataService.uploadmultifile(data).then(function (result) {
            if (result.flag) {
                $scope.filedinhkem = result.data
            }
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
        });
    }
});