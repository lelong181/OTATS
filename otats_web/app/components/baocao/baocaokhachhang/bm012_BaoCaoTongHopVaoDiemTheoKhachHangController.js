angular.module('app').controller('bm012_BaoCaoTongHopVaoDiemTheoKhachHangController', function ($rootScope, $scope, $state, Notification, ComboboxDataService, baoCaoKhachHangDataService) {
    CreateSiteMap();


    function init() {
        initdate();
        initcombo();
        loadgrid(0, 0, 0);
    }
    function initdate() {
        var dateNow = new Date();
        $scope.obj_TuNgay = new Date(dateNow.setHours(0, 0, 0));
        $scope.obj_DenNgay = new Date(dateNow.setHours(23, 59, 59));
        $scope.maxDate = $scope.obj_DenNgay;
        $scope.minDate = $scope.obj_TuNgay;
    }
    function initcombo() {
        //load list nhóm nhân viên
        ComboboxDataService.getDataNhomNhanVien().then(function (result) {
            $scope.nhomnhanvienData = result.data;
            if (!result.flag) {
                Notification({ message: $.i18n('label_khongtheloaddanhsachnhanvienvuilongtailaitrang') }, 'warning');
            }
        });
        ComboboxDataService.getDataKhachHang().then(function (result) {
            $scope.khachhangData = result.data;
            if (!result.flag) {
                Notification({ message: $.i18n('label_khongtheloaddanhsachkhachhangvuilongtailaitrang') }, 'warning');
            }
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
            field: "ngay", title: $.i18n('header_thoigian'), template: function (dataItem) {
                return kendo.htmlEncode(kendo.toString(dataItem.ngay, formatDate));
            }, attributes: {style: "text-align: center"}, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "140px"
        });
        dataList.push({
            field: "tenNhanVien", title: $.i18n('header_tennhanvien'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "160px"
        });
        dataList.push({
            field: "tenKhachHang", title: $.i18n('header_tenkhachhang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "170px"
        });
        dataList.push({
            field: "dienThoai", title: $.i18n('header_dienthoai'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px"
        });
        dataList.push({
            field: "diaChi", title: $.i18n('header_diachikhachhang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "300px"
        });
        dataList.push({
            field: "soLanViengTham", title: $.i18n('header_solanvaodiem'), attributes: { style: "text-align: center" },
            template: function (dataItem) {
                return "<a href='' class='color-primary' ng-click='openDetailFromGrid(" + kendo.htmlEncode(dataItem.maNhanVien) + ',' + kendo.htmlEncode(dataItem.maKhachHang) + ")'>" + kendo.htmlEncode(dataItem.soLanViengTham) + "</a>";
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });


        return dataList;
    }
    function loadgrid(idnhanvien, idkhachhang, idnhom) {
        kendo.ui.progress($("#grid"), true);
        commonOpenLoadingText("#btn_bm012xembaocao");
        $scope.gridOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".content-filter").height() + $(".toolbarmenu").height());
                return heightGrid - 30;
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
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);

        baoCaoKhachHangDataService.getBaoCaoTongHopVaoDiemTheoKhachHang(idnhanvien, idkhachhang, idnhom, fromdate, todate).then(function (result) {
            $scope.gridData = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                            iD_KhachHang: {
                                type: "number"
                            },
                            maKH: {
                                type: "string"
                            },
                            ngay: {
                                type: "date"
                            },
                        }
                    }
                },
                pageSize: 20,

            };

            kendo.ui.progress($("#grid"), false);
            commonCloseLoadingText("#btn_bm012xembaocao")
        });

    }
    
    $scope.openDetailFromGrid = function (maNhanVien, maKhachHang) {
        //let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        //let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);

        let myGrid = $('#grid').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;
        
        let fromdate = new Date(JSON.parse(JSON.stringify(dataItem.ngay)));
        fromdate.setHours(0, 0, 0, 0);

        let todate = new Date(JSON.parse(JSON.stringify(dataItem.ngay)));
        todate.setHours(23, 59, 59, 999);

        $state.go('lichsuvaoradiem', { idnhanvien: maNhanVien, idkhachhang: maKhachHang, from: kendo.toString(fromdate, formatDateTimeFilter), to: kendo.toString(todate, formatDateTimeFilter) });
    }
    //event
    $scope.xemBaoCao = function () {

        var idnhanvien = 0;
        var idkhachhang = 0;
        var idnhom = 0;
        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        if ($scope.khachhangselect != undefined)
            idkhachhang = ($scope.khachhangselect.iD_KhachHang < 0) ? 0 : $scope.khachhangselect.iD_KhachHang;
        if ($scope.nhomnhanvienselect != undefined)
            idnhom = ($scope.nhomnhanvienselect.iD_Nhom < 0) ? 0 : $scope.nhomnhanvienselect.iD_Nhom;

        loadgrid(idnhanvien, idkhachhang, idnhom);
    }
    $scope.XuatExcel = function () {
        commonOpenLoadingText("#btn_bm012xuatexcel");
        var idnhanvien = 0;
        var idkhachhang = 0;
        var idnhom = 0;
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        if ($scope.khachhangselect != undefined)
            idkhachhang = ($scope.khachhangselect.iD_KhachHang < 0) ? 0 : $scope.khachhangselect.iD_KhachHang;
        if ($scope.nhomnhanvienselect != undefined)
            idnhom = ($scope.nhomnhanvienselect.iD_Nhom < 0) ? 0 : $scope.nhomnhanvienselect.iD_Nhom;

        baoCaoKhachHangDataService.getExcelBaoCaoTongHopVaoDiemTheoKhachHang(idnhanvien, idkhachhang, idnhom, fromdate, todate).then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayravuilongloadlaitrang') }, 'warning');

            commonCloseLoadingText("#btn_bm012xuatexcel")
        });
    }

    $scope.nhanvienOnChange = function () {
        $scope.nhanvienselect = this.nhanvienselect;
    }
    $scope.khachhangOnChange = function () {
        $scope.khachhangselect = this.khachhangselect;
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
