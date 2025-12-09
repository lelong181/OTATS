angular.module('app').controller('bm009_BaoCaoMatHangKhachHangController', function ($rootScope, $scope, Notification, ComboboxDataService, baoCaoKhachHangDataService, donHangDataService) {
    CreateSiteMap();

    //config
    function init() {
        initdate();
        initcombo();
        loadgrid(0, 0, 0, '');
    }

    function initdate() {
        var dateNow = new Date();
        $scope.obj_TuNgay = new Date(dateNow.setHours(0, 0, 0));
        $scope.obj_DenNgay = new Date(dateNow.setHours(23, 59, 59));
        $scope.maxDate = $scope.obj_DenNgay;
        $scope.minDate = $scope.obj_TuNgay;
    }
    function initcombo() {
        //load list mặt hàng
        ComboboxDataService.getDataMatHang().then(function (result) {
            console.log(result);
            $scope.mathangData = result.data;
            if (!result.flag) {
                Notification({ message: 'Không thể load danh sách mặt hàng, vui lòng tải lại trang' }, 'warning');
            }
        });


        //load list loại khách hàng
        ComboboxDataService.getLoaiKhachHang().then(function (result) {
            $scope.loaikhachhangData = result.data;

            $scope.loaikhachhangOptions = {
                filter: "contains",
                suggest: true
            }
        });

        //load list khách hàng
        ComboboxDataService.getDataKhachHang().then(function (result) {
            $scope.khachhangData = result.data;
            if (!result.flag) {
                Notification({ message: 'Không thể load danh sách khách hàng, vui lòng tải lại trang' }, 'warning');
            }
        });


        //load list nhân viên
        donHangDataService.getcombonhanvienlap().then(function (result) {
            $scope.nhanvienData = result.data;
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
        dataList.push({
            field: "tenKhachHang", title: $.i18n('header_khachhang'),
            attributes: {
                style: "text-align: center"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "dienThoai", title: $.i18n('header_sodienthoai'), attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "maHang", title: $.i18n('header_mahang'), attributes: {

                style: "text-align: center"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px"
        });
        dataList.push({
            field: "tenHang", title: $.i18n('header_mathang'), attributes: {

                style: "text-align: center"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "tenDonVi", title: $.i18n('header_donvi'), attributes: {

                style: "text-align: center"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "80px"
        });
        dataList.push({
            field: "thoiGian", title: $.i18n('header_thoigian'), template: function (dataItem) {
                return kendo.htmlEncode(kendo.toString(dataItem.thoiGian, formatDate));
            }, attributes: {

                style: "text-align: center"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        //dataList.push({
        //    field: "tenNhanVien", title: $.i18n('header_nhanvienquanly'), attributes: {

        //        style: "text-align: center"
        //    }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        //});
        dataList.push({
            field: "soLuong", title: $.i18n('header_soluong'), attributes: {
                style: "text-align: center"
            },
            footerTemplate: formatNumberInFooterGrid('soLuong.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: right" }, attributes: {

                style: "text-align: right"
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "80px"
        });
        dataList.push({
            field: "soDon", title: "Số đơn", attributes: {
                style: "text-align: center"
            },
            footerTemplate: formatNumberInFooterGrid('soDon.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: right" }, attributes: {

                style: "text-align: right"
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "80px"
        });
        dataList.push({
            field: "tongTien", title: $.i18n('header_tongtien'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('tongTien.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: right" }, attributes: {

                style: "text-align: right"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        //dataList.push({
        //    field: "tongTienChietKhau", title: $.i18n('header_tongtienchietkhau'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
        //    footerTemplate: formatNumberInFooterGrid('tongTienChietKhau.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: right" }, attributes: {

        //        style: "text-align: right"
        //    }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        //});

        return dataList;
    }
    function loadgrid(idmathang, idloaikhachhang, idkhachhang, idnhanvien) {
        kendo.ui.progress($("#grid"), true);
        commonOpenLoadingText("#btn_bm009xembaocao");

        $scope.gridOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".content-filter").height());
                console.log(heightGrid);
                return heightGrid < 100 ? 500 : heightGrid;
            },
            excel: {
                allPages: true
            },
            excelExport: function (e) {
                //excelExport(e);
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
        baoCaoKhachHangDataService.getBaoCaoMatHang_KH(idmathang, idloaikhachhang, idkhachhang, idnhanvien, fromdate, todate).then(function (result) {
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
                            thoiGian: {
                                type: "date"
                            },
                            tongTienChietKhau: {
                                type: "number"
                            },
                            tongTien: {
                                type: "number"
                            },
                            soLuong: {
                                type: "number"
                            },
                            soDon: {
                                type: "number"
                            },
                        }
                    }
                },
                pageSize: 20,
                aggregate: [
                    { field: "tongTien", aggregate: "sum" },
                    { field: "soLuong", aggregate: "sum" },
                    { field: "soDon", aggregate: "sum" },
                    { field: "tongTienChietKhau", aggregate: "sum" },
                ]

            };
            kendo.ui.progress($("#grid"), false);
            commonCloseLoadingText("#btn_bm009xembaocao")
        });
    }

    //event
    $scope.xemBaoCao = function () {

        var idkhachhang = 0;
        var idmathang = 0;
        var idloaikhachhang = 0;
        var idnhanvien = [];

        if ($scope.khachhangselect != undefined)
            idkhachhang = ($scope.khachhangselect.iD_KhachHang < 0) ? 0 : $scope.khachhangselect.iD_KhachHang;
        if ($scope.mathangselect != undefined)
            idmathang = ($scope.mathangselect.id < 0) ? 0 : $scope.mathangselect.id;
        if ($scope.loaikhachhangselect != undefined)
            idloaikhachhang = ($scope.loaikhachhangselect.iD_LoaiKhachHang < 0) ? 0 : $scope.loaikhachhangselect.iD_LoaiKhachHang;
        if ($scope.nhanvienselect != undefined) {
            $.each($scope.nhanvienselect, function (index, item) {
                idnhanvien.push(item.idnv)

            })
        }
        loadgrid(idmathang, idloaikhachhang, idkhachhang, idnhanvien.join(","));
    }
    $scope.XuatExcel = function () {
        //commonOpenLoadingText("#btn_bm009xuatexcel");

        //var idkhachhang = 0;
        //var idmathang = 0;
        //let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        //let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        //if ($scope.khachhangselect != undefined)
        //    idkhachhang = ($scope.khachhangselect.iD_KhachHang < 0) ? 0 : $scope.khachhangselect.iD_KhachHang;
        //if ($scope.mathangselect != undefined)
        //    idmathang = ($scope.mathangselect.id < 0) ? 0 : $scope.mathangselect.id;

        //baoCaoKhachHangDataService.getExcelBaoCaoMatHang_KH( idmathang, idkhachhang, fromdate, todate).then(function (result) {
        //    if (result.flag)
        //        commonDownFile(result.data);
        //    else
        //        Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayravuilongloadlaitrang') }, 'warning');

        //    commonCloseLoadingText("#btn_bm009xuatexcel")
        //});
        $("#grid").data("kendoGrid").saveAsExcel();
    }

    $scope.nhanvienOnChange = function () {
        $scope.nhanvienselect = this.nhanvienselect;
        console.log($scope.nhanvienselect);
    }

    $scope.khachhangOnChange = function () {
        $scope.khachhangselect = this.khachhangselect;
    }
    $scope.mathangOnChange = function () {
        $scope.mathangselect = this.mathangselect;
    }
    $scope.fromDateChanged = function () {
        $scope.minDate = $scope.obj_TuNgay;
    }
    $scope.toDateChanged = function () {
        $scope.maxDate = $scope.obj_DenNgay;
    }
    $scope.loaikhachhangOnChange = function () {
        $scope.loaikhachhangselect = this.loaikhachhangselect;
        console.log($scope.loaikhachhangselect);
        //load list khách hàng
        ComboboxDataService.getDataKhachHang().then(function (result) {
            let small_arr = result.data.filter((item) => {
                return (
                    item.iD_LoaiKhachHang == $scope.loaikhachhangselect.iD_LoaiKhachHang
                    ||
                    $scope.loaikhachhangselect == null
                );
            });
            $scope.khachhangData = small_arr;
            if (!result.flag) {
                Notification({ message: 'Không thể load danh sách khách hàng, vui lòng tải lại trang' }, 'warning');
            }
        });

    }

    init();

})

