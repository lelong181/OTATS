angular.module('app').controller('bm029_BaoCaoTongHopRaVaoDiemController', function ($rootScope, $scope, Notification, ComboboxDataService, baoCaoNhanVienDataService) {
    CreateSiteMap();

    function init() {
        initdate();
        initcombo();
        loadgrid(0, 0);
    }
    function initdate() {
        var dateNow = new Date();
        $scope.obj_TuNgay = new Date(dateNow.setHours(0, 0, 0));
        $scope.obj_DenNgay = new Date(dateNow.setHours(23, 59, 59));
        $scope.maxDate = $scope.obj_DenNgay;
        $scope.minDate = $scope.obj_TuNgay;
    }
    function initcombo() {
        ComboboxDataService.getDataNhomNhanVien().then(function (result) {
            $scope.nhomnhanvienData = result.data;
        });
        ComboboxDataService.getlistnhanvienbymultiidnhom({ IDNhom: -2 }).then(function (result) {
            if (result.flag) {
                $scope.nhanvienData = result.data;
            }
            else
                $scope.nhanvienData = [];
        });
    }
    function listColumnsgrid() {
        var dataList = [];

        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { class: "text-center" },
        });
        dataList.push({
            field: "tenNhanVien", title: $.i18n('header_tennhanvien'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px"
        });
        dataList.push({
            field: "thoiGianDangNhap", title: $.i18n('header_thoigiandangnhap'), template: function (dataItem) {
                if (dataItem.thoiGianDangNhap == null || dataItem.thoiGianDangNhap.getFullYear() < 1900)
                    return '';
                else
                    return kendo.htmlEncode(kendo.toString(dataItem.thoiGianDangNhap, formatDateTime));
            }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "200px"
        });
        dataList.push({
            field: "thoiGianDangXuat", title: $.i18n('header_thoigiandangxuat'), template: function (dataItem) {
                if (dataItem.thoiGianDangXuat == null || dataItem.thoiGianDangXuat.getFullYear() < 1900)
                    return '';
                else
                    return kendo.htmlEncode(kendo.toString(dataItem.thoiGianDangXuat, formatDateTime));
            }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "200px"
        });
        dataList.push({
            field: "", title: $.i18n('header_toadodangnhap'), template: function (dataItem) {
                if (dataItem.viDoDangNhap == '' || dataItem.kinhDoDangNhap == '')
                    return '<button ng-click="xemvitrichuacotoado()" class="btn btn-link btn-menubar" ><i class="fas fa-map-marker-alt fas-sm color-gray"></i></button>';
                else
                    return '<button ng-click="xemvitri(' + kendo.htmlEncode(dataItem.viDoDangNhap) + ',' + kendo.htmlEncode(dataItem.kinhDoDangNhap) + ')" class="btn btn-link btn-menubar" ><i class="fas fa-map-marker-alt fas-sm color-infor"></i></button>';
            }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "200px"
        });
        dataList.push({
            field: "", title: $.i18n('header_toadodangxuat'), template: function (dataItem) {
                if (dataItem.viDoDangXuat == '' || dataItem.kinhDoDangXuat == '')
                    return '<button ng-click="xemvitrichuacotoado()" class="btn btn-link btn-menubar" ><i class="fas fa-map-marker-alt fas-sm color-gray"></i></button>';
                else
                    return '<button ng-click="xemvitri2(' + kendo.htmlEncode(dataItem.viDoDangXuat) + ',' + kendo.htmlEncode(dataItem.kinhDoDangXuat) + ')" class="btn btn-link btn-menubar" ><i class="fas fa-map-marker-alt fas-sm color-infor"></i></button>';
            }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "200px"
        });

        return dataList;
    }
    function loadgrid(idnhom, idnhanvien) {
        kendo.ui.progress($("#grid"), true);
        commonOpenLoadingText("#btn_bm029xembaocao");
        $scope.gridOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".content-filter").height());
                return heightGrid < 100 ? 500 : heightGrid;
            },
            excelExport: function (e) {
                excelExport(e);
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
            columns: listColumnsgrid(),
        };

        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        baoCaoNhanVienDataService.getBaoCaoTongHopRaVaoDiem(idnhom, idnhanvien, fromdate, todate).then(function (result) {
            $scope.gridData = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                            thoiGianDangNhap: {
                                type: "date"
                            },
                            thoiGianDangXuat: {
                                type: "date"
                            },
                        }
                    }
                },
                pageSize: 20,
            };

            kendo.ui.progress($("#grid"), false);
            commonCloseLoadingText("#btn_bm029xembaocao")
        });
    }
    //event
    $scope.xemvitrichuacotoado = function () {
        Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_chuacothongtinvitri') }, 'warning');
    }
    $scope.xemvitri = function (viDoDangNhap, kinhDoDangNhap) {
        let url = 'https://www.google.com/maps/dir/' + viDoDangNhap.toString() + ',' + kinhDoDangNhap.toString();
        window.open(url, '_blank');
    }
    $scope.xemvitri2 = function (viDoDangXuat, kinhDoDangXuat) {
        let url = 'https://www.google.com/maps/dir/' + viDoDangXuat.toString() + ',' + kinhDoDangXuat.toString();
        window.open(url, '_blank');
    }
    $scope.xemBaoCao = function () {
        var idnhanvien = 0;
        var idnhom = 0;
        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        if ($scope.nhomnhanvienselect != undefined)
            idnhom = ($scope.nhomnhanvienselect.iD_Nhom < 0) ? 0 : $scope.nhomnhanvienselect.iD_Nhom;

        loadgrid(idnhom, idnhanvien);
    }
    $scope.XuatExcel = function () {
        commonOpenLoadingText("#btn_bm029xuatexcel");
        var idnhanvien = 0;
        var idnhom = 0;
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        if ($scope.nhomnhanvienselect != undefined)
            idnhom = ($scope.nhomnhanvienselect.iD_Nhom < 0) ? 0 : $scope.nhomnhanvienselect.iD_Nhom;
        baoCaoNhanVienDataService.getExcelBaoCaoTongHopRaVaoDiem(idnhom, idnhanvien, fromdate, todate).then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayravuilongloadlaitrang') }, 'warning');

            commonCloseLoadingText("#btn_bm029xuatexcel")
        });
    }

    $scope.nhanvienOnChange = function () {
        $scope.nhanvienselect = this.nhanvienselect;
    }
    $scope.nhomnhanvienOnChange = function () {
        $scope.nhomnhanvienselect = this.nhomnhanvienselect;
        idnhom = $scope.nhomnhanvienselect.iD_Nhom;
        ComboboxDataService.getlistnhanvienbymultiidnhom({ IDNhom: idnhom }).then(function (result) {
            if (result.flag) {
                $scope.nhanvienData = result.data;
            }
            else
                $scope.nhanvienData = [];
        });
    };
    $scope.fromDateChanged = function () {
        $scope.minDate = $scope.obj_TuNgay;
    }
    $scope.toDateChanged = function () {
        $scope.maxDate = $scope.obj_DenNgay;
    }
    init();

})
