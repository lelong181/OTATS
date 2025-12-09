angular.module('app').controller('bm024_BaoCaoLichSuSuaChuaController', function ($state, $rootScope, $scope, Notification, ComboboxDataService, baoCaoNhanVienDataService) {
    CreateSiteMap();

    function init() {
        initdate();
        loadgrid(0, 0);
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
            title: "#", template: "#= ++RecordNumber #",
            width: "50px", headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({
            field: "tenNhanVien", title: $.i18n('header_tennhanvien'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "130px"
        });
        dataList.push({
            field: "bienKiemSoat", title: $.i18n('header_bienkiemsoat'), attributes: { class: "text-center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px"
        });
        dataList.push({
            field: "ngayBaoDuongExport", title: $.i18n('header_ngaybaoduong'), attributes: { class: "text-center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "140px"
        });
        dataList.push({
            field: "chiPhi", title: $.i18n('header_chiphi'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), attributes: { class: "text-right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "120px"
        });
        dataList.push({
            field: "soAnh", title: $.i18n('header_soanh'),
            template: function (dataItem) {
                return "<a href='' class='color-primary' ng-click='openAlbumImage()'>" + kendo.htmlEncode(dataItem.soAnh) + "</a>";
            },
            attributes: { class: "text-center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "60px"
        });
        dataList.push({
            field: "diaChiBaoDuong", title: $.i18n('header_diachi'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "250px"
        });
        dataList.push({
            field: "noiDung", title: $.i18n('header_ghichu'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px"
        });

        return dataList;
    }
    function loadgrid(idkhachhang, idnhanvien) {
        kendo.ui.progress($("#grid"), true);
        commonOpenLoadingText("#btn_bm024xembaocao");

        $scope.gridOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".content-filter").height() );
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
            columns: listColumnsgrid()
        };
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);

        baoCaoNhanVienDataService.getBaoCaoLichSuSuaChua(idkhachhang, idnhanvien, fromdate, todate).then(function (result) {
            $scope.gridData = {
                data: result.data,
                schema: {
                    model: {
                        fields: {

                            chiPhi: {
                                type: "number"
                            },
                            //ngayBaoDuongExport: {
                            //    type: "date"
                            //},
                        }
                    }
                },
                pageSize: 20,
            };

            kendo.ui.progress($("#grid"), false);
            commonCloseLoadingText("#btn_bm024xembaocao")
        });

    }

    //event
    $scope.xemBaoCao = function () {
        var idnhanvien = 0;
        var idkhachhang = 0;
        loadgrid(idkhachhang, idnhanvien);
    }
    $scope.XuatExcel = function () {
        commonOpenLoadingText("#btn_bm024xuatexcel");
        var idnhanvien = 0;
        var idkhachhang = 0;
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);

        baoCaoNhanVienDataService.getExcelBaoCaoLichSuSuaChua(idkhachhang, idnhanvien, fromdate, todate).then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayravuilongloadlaitrang') }, 'warning');

            commonCloseLoadingText("#btn_bm024xuatexcel")
        });

    }
    $scope.fromDateChanged = function () {
        $scope.minDate = $scope.obj_TuNgay;
    }
    $scope.toDateChanged = function () {
        $scope.maxDate = $scope.obj_DenNgay;
    }

    $scope.openAlbumImage = function () {
        let myGrid = $('#grid').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;

        id = dataItem.iD_Xe_LichSuBD;

        var url = $state.href('album', { idlichsubaoduong: id });
        window.open(url, '_blank');
    }

    init();

})
