angular.module('app').controller('baoCaoGuiTinNhanController', function ($scope, Notification, ComboboxDataService, tinNhanDataService, $stateParams) {
    CreateSiteMap();

    let param_chuadoc = 0;

    function init() {
        param_chuadoc = ($stateParams.chuadoc == undefined) ? 0 : $stateParams.chuadoc;

        initdate();
        initcombo();
        loadgrid(0);
    }
    function initdate() {
        let dateNow = new Date();
        $scope.obj_TuNgay = new Date(dateNow.setHours(0, 0, 0));
        $scope.obj_DenNgay = new Date(dateNow.setHours(23, 59, 59));
        $scope.maxDate = $scope.obj_DenNgay;
        $scope.minDate = $scope.obj_TuNgay;
    }
    function initcombo() {
        ComboboxDataService.getDataNhanVien().then(function (result) {
            $scope.nhanvienData = result.data;
            if (!result.flag) {
                Notification({ message: $.i18n('label_khongtheloaddanhsachnhanvienvuilongtailaitrang') }, 'warning');
            }
            setTimeout(function () {
                $scope.nhanvien.value($stateParams.idnhanvien);
            })
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
            field: "tenNhanVien", title: $.i18n('header_tennhanvien'),
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "noiDung", title: $.i18n('header_noidung'),
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "250px"
        });
        dataList.push({
            field: "ngayGui", title: $.i18n('header_thoigiangui'),
            template: function (dataItem) {
                return kendo.htmlEncode(kendo.toString(dataItem.ngayGui, formatDateTime));
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "trangThaiHienThi", title: $.i18n('header_trangthai'),
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "ngayXemHienThi", title: $.i18n('header_thoigianxem'),
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "tenQuanLy", title: $.i18n('header_tenquanly'),
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "loaiGui", title: $.i18n('header_loaigui'),
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });


        return dataList;
    }
    function loadgrid(idnhanvien) {
        kendo.ui.progress($("#grid"), true);
        commonOpenLoadingText("#btn_xemBaoCao");
        $scope.gridOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".content-filter").height());
                return heightGrid < 100 ? 500 : heightGrid;
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
        tinNhanDataService.baocaotinnhan(idnhanvien, param_chuadoc, fromdate, todate).then(function (result) {
            $scope.gridData = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                            ngayGui: {
                                type: "date"
                            },
                        }
                    }
                },
                pageSize: 20
            };
            kendo.ui.progress($("#grid"), false);
            commonCloseLoadingText("#btn_xemBaoCao")
        });
    }
    //event
    $scope.nhanvienOnChange = function () {
        $scope.nhanvienselect = this.nhanvienselect;
    }

    $scope.xemBaoCao = function () {
        var idnhanvien = 0;
        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        loadgrid(idnhanvien);
    }
    $scope.XuatExcel = function () {
        commonOpenLoadingText("#btn_XuatExcel");

        let idnhanvien = 0;
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);

        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        else
            idnhanvien = 0;

        tinNhanDataService.excelbaocaotinnhan(idnhanvien, param_chuadoc, fromdate, todate).then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayravuilongloadlaitrang') }, 'warning');

            commonCloseLoadingText("#btn_XuatExcel")
        });
    }
    $scope.fromDateChanged = function () {
        $scope.minDate = $scope.obj_TuNgay;
    }
    $scope.toDateChanged = function () {
        $scope.maxDate = $scope.obj_DenNgay;
    }
    init();

})