angular.module('app').controller('bm051_BaoCaoTongHopChuongTrinhKhuyenMaiController', function ($rootScope, $scope, Notification, ComboboxDataService, baoCaoKhuyenMaiDataService) {
    CreateSiteMap();

    let idctkm = 0;
    let idkho = 0;
    let idnhom = 0;
    
    function init() {
        initcombo();
        loadgrid(0, 0);
    }

    function initcombo() {
        ComboboxDataService.getDataNhomNhanVien().then(function (response) {
            list = response.data;
            list.shift();
            $scope.listNhomNhanVien = list;
            $scope.selectNhomNhanVien = [];
        });

        ComboboxDataService.getDataKhoHang().then(function (response) {
            $scope.listKhoHang = response.data;
            $scope.selectKhoHang = [];
        });

        ComboboxDataService.getListKhuyenMai().then(function (response) {
            $scope.listChuongTrinhKhuyenMai = response.data;
            $scope.selectChuongTrinhKhuyenMai = [];
        });
    }
    function listColumnsgrid() {
        let dataList = [];

        dataList.push({
            title: "STT", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({
            field: "tenNhanVien", title: $.i18n('header_tennhanvien'),
            footerTemplate: $.i18n('label_tong') + ':',
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "175px"
        });
        dataList.push({ field: "tenHang", title: $.i18n('header_tenhang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "175px" });
        dataList.push({ field: "maHang", title: $.i18n('header_mahang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({
            field: "soLuongKH", title: $.i18n('header_soluongdiemban'),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('soLuongKH.sum', $rootScope.UserInfo.dinhDangSo),
            attributes: { "class": "table-cell", style: "text-align: center" },
            footerAttributes: { style: "text-align: center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "soLuongDatKM", title: $.i18n('header_soluongbandatyeucauctkm'),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('soLuongDatKM.sum', $rootScope.UserInfo.dinhDangSo),
            footerAttributes: { style: "text-align: center" },
            attributes: { "class": "table-cell", style: "text-align: center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "200px"
        });
        dataList.push({
            field: "soLuongKMThucHien", title: $.i18n('header_soluongkhuyenmaithuchien'),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerAttributes: { style: "text-align: center" },
            footerTemplate: formatNumberInFooterGrid('soLuongKMThucHien.sum', $rootScope.UserInfo.dinhDangSo),
            attributes: { "class": "table-cell", style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: false, width: "150px"
        });
        dataList.push({ field: "ghiChu", title: $.i18n('header_ghichu'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid });

        return dataList;
    }
    function loadgrid() {
        kendo.ui.progress($("#grid"), true);
        commonOpenLoadingText("#btn_xembaocao");

        $scope.gridOptions = {
            sortable: true,
            height: function () {
                let heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".content-filter").height());
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

        baoCaoKhuyenMaiDataService.getBaoCaoTongHopChuongTrinhKhuyenMai(idctkm, idkho, idnhom).then(function (result) {
            $scope.gridData = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                            id_NhanVien: { type: "number" }
                        }
                    }
                },
                pageSize: 20,
                aggregate: [
                { field: "soLuongKH", aggregate: "sum" },
                { field: "soLuongDatKM", aggregate: "sum" },
                { field: "soLuongKMThucHien", aggregate: "sum" }
                ]
            };
            kendo.ui.progress($("#grid"), false);
            commonCloseLoadingText("#btn_xembaocao")
        });
    }
    
    //event
    $scope.chuongTrinhKhuyenMaiOnChange = function (kendoEvent) {
        $scope.selectChuongTrinhKhuyenMai = this.selectChuongTrinhKhuyenMai;

        if ($scope.selectChuongTrinhKhuyenMai != undefined) {
            idctkm = $scope.selectChuongTrinhKhuyenMai.iD_CTKM;

            let from = new Date($scope.selectChuongTrinhKhuyenMai.ngayApDung);
            let to = new Date($scope.selectChuongTrinhKhuyenMai.ngayKetThuc);

            $scope.tenctkm = $.i18n('label_thoigiankhuyenmaitu') + ': ' + kendo.toString(from, 'dd/MM/yyyy') + $.i18n('label_den') + ' : ' + kendo.toString(to, 'dd/MM/yyyy');
        }
        else {
            idctkm = 0;
            $scope.tenctkm = '';
        }
    }
    $scope.khoHangOnChange = function (kendoEvent) {
        $scope.selectKhoHang = this.selectKhoHang;

        if ($scope.selectKhoHang != undefined)
            idkho = $scope.selectKhoHang.iD_Kho;
        else
            idkho = 0;
    }
    $scope.nhomNhanVienOnChange = function (kendoEvent) {
        $scope.selectNhomNhanVien = this.selectNhomNhanVien;

        if ($scope.selectNhomNhanVien != undefined)
            idnhom = $scope.selectNhomNhanVien.iD_Nhom;
        else
            idnhom = 0;
    }

    $scope.xemBaoCao = function () {
        loadgrid();
    }
    $scope.XuatExcel = function () {
        commonOpenLoadingText("#btn_xuatexcel");
        
        baoCaoKhuyenMaiDataService.getExcelBaoCaoTongHopChuongTrinhKhuyenMai(idctkm, idkho, idnhom).then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayravuilongloadlaitrang') }, 'warning');

            commonCloseLoadingText("#btn_xuatexcel")
        });

    }

    init();

})
