angular.module('app').controller('bm052_BaoCaoChiTietChuongTrinhKhuyenMaiController', function ($rootScope, $scope, Notification, ComboboxDataService, baoCaoKhuyenMaiDataService) {
    CreateSiteMap();

    let idctkm = 0;
    let idkho = 0;
    let idnhanvien = 0;
    let idhang = 0;

    function init() {
        initcombo();
        loadgrid();
    }

    function initcombo() {

        ComboboxDataService.getDataNhanVien().then(function (response) {
            $scope.listNhanVien = response.data;
            $scope.selectNhanVien = [];
        });

        ComboboxDataService.getDataKhoHang().then(function (response) {
            $scope.listKhoHang = response.data;
            $scope.selectKhoHang = [];
        });

        ComboboxDataService.getListKhuyenMai().then(function (response) {
            $scope.listChuongTrinhKhuyenMai = response.data;
            $scope.selectChuongTrinhKhuyenMai = [];
        });

        ComboboxDataService.getDataMatHang().then(function (response) {
            $scope.listMatHang = response.data;
            $scope.selectMatHang = [];
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
            field: "maKH", title: $.i18n('header_makhachhang'),
            footerTemplate: $.i18n('label_tong') + ': ',
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px"
        });
        dataList.push({ field: "tenKhachHang", title: $.i18n('header_tenkhachhang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "175px" });
        dataList.push({ field: "diaChi", title: $.i18n('header_diachi'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "300px" });
        dataList.push({ field: "dienThoai", title: $.i18n('header_sodienthoai'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px" });
        dataList.push({ field: "maThamChieu", title: $.i18n('header_donhang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px" });
        dataList.push({ field: "tenHang", title: $.i18n('header_tenhang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "175px" });
        dataList.push({
            field: "soLuongDatKM", title: $.i18n('header_soluongbandatctkm'),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('soLuongDatKM.sum', $rootScope.UserInfo.dinhDangSo),
            footerAttributes: { style: "text-align: center" },
            attributes: { "class": "table-cell", style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "130px"
        });
        dataList.push({
            field: "soLuongKMThucHien", title: $.i18n('header_soluongkhuyenmaithuchien'),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('soLuongKMThucHien.sum', $rootScope.UserInfo.dinhDangSo),
            footerAttributes: { style: "text-align: center" },
            attributes: { "class": "table-cell", style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "130px"
        });
        dataList.push({
            field: "createDate", title: $.i18n('header_thoigian'),
            template: function (dataItem) {
                return kendo.htmlEncode(kendo.toString(dataItem.createDate, formatDateTime));
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "120px"
        });
        dataList.push({ field: "ghiChu", title: $.i18n('header_ghichu'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px" });

        return dataList;
    }
    function loadgrid() {
        kendo.ui.progress($("#grid"), true);
        commonOpenLoadingText("#btn_xembaocao");

        $scope.gridOptions = {
            sortable: true,
            height: function () {
                let heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height());
                return heightGrid - 100;
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

        baoCaoKhuyenMaiDataService.getBaoCaoChiTietChuongTrinhKhuyenMai(idctkm, idkho, idnhanvien, idhang).then(function (result) {
            $scope.gridData = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                            id_NhanVien: { type: "number" },
                            createDate: { type: "date" }
                        }
                    }
                },
                pageSize: 20,
                aggregate: [
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

            var from = new Date($scope.selectChuongTrinhKhuyenMai.ngayApDung);
            var to = new Date($scope.selectChuongTrinhKhuyenMai.ngayKetThuc);

            $scope.tenctkm = 'Thời gian khuyến mãi từ: ' + kendo.toString(from, 'dd/MM/yyyy') + ' đến: ' + kendo.toString(to, 'dd/MM/yyyy');
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
    $scope.nhanVienOnChange = function (kendoEvent) {
        $scope.selectNhanVien = this.selectNhanVien;

        if ($scope.selectNhanVien != undefined)
            idnhanvien = $scope.selectNhanVien.idnv;
        else
            idnhanvien = 0;
    }
    $scope.matHangOnChange = function (kendoEvent) {
        $scope.selectMatHang = this.selectMatHang;

        if ($scope.selectMatHang != undefined)
            idhang = $scope.selectMatHang.id;
        else
            idhang = 0;
    }

    $scope.xemBaoCao = function () {
        loadgrid();
    }
    $scope.XuatExcel = function () {
        commonOpenLoadingText("#btn_xuatexcel");

        baoCaoKhuyenMaiDataService.getExcelBaoCaoChiTietChuongTrinhKhuyenMai(idctkm, idkho, idnhanvien, idhang).then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayravuilongloadlaitrang') }, 'warning');

            commonCloseLoadingText("#btn_xuatexcel")
        });
    }

    init();

})
