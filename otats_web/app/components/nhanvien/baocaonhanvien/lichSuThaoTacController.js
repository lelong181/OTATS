angular.module('app').controller('lichSuThaoTacController', function ($rootScope, $scope, $state, Notification, ComboboxDataService, NhanVienDetail) {
    CreateSiteMap();
    hideLoadingPage();

    $scope.init = function () {
        $scope.initdate();
        $scope.initcombo();
        $scope.loadgrid(0, 0, 0);
    }

    $scope.initdate = function () {
        let dateNow = new Date();

        $scope.obj_TuNgay = new Date(dateNow.setHours(0, 0, 0));
        $scope.obj_DenNgay = new Date(dateNow.setHours(23, 59, 59));
        $scope.maxDate = $scope.obj_DenNgay;
        $scope.minDate = $scope.obj_TuNgay;
    }
    $scope.initcombo = function () {
        //load list thao tác
        if ($rootScope.lang == 'vi-vn')
            $scope.thaotacData = [{ name: "Đăng nhập", id: 1 }, { name: "Đăng xuất", id: 2 }, { name: "Vào điểm", id: 3 }, { name: "Ra điểm", id: 4 }, { name: "Chụp ảnh", id: 5 }, { name: "Lập đơn hàng", id: 6 }];
        else
            $scope.thaotacData = [{ name: "Login", id: 1 }, { name: "Logout", id: 2 }, { name: "Check in", id: 3 }, { name: "Check out", id: 4 }, { name: "Take photos", id: 5 }, { name: "Create order", id: 6 }];

        //load list khách hàng
        ComboboxDataService.getDataKhachHang().then(function (result) {
            $scope.khachhangData = result.data;
            if (!result.flag) {
                Notification({ message: $.i18n('label_khongtheloaddanhsachkhachhangvuilongtailaitrang') }, 'warning');
            }
        });

        //load list nhân viên
        ComboboxDataService.getDataNhanVien().then(function (result) {
            $scope.nhanvienData = result.data;
            if (!result.flag) {
                Notification({ message: $.i18n('label_khongtheloaddanhsachnhanvienvuilongtailaitrang') }, 'warning');
            }
        });

        //let data = new kendo.data.DataSource({
        //    serverPaging: true,
        //    serverFiltering: true,
        //    pageSize: 50,
        //    transport: {
        //        read: function (options) {
        //            console.log(options);
        //            ComboboxDataService.getDataKhachHang_ServerPaging(options.data.take, options.data.skip).then(function (result) {
        //                options.success(result.data);
        //            });
        //        }
        //    }
        //});

        //$("#khachhang").kendoComboBox({
        //    dataTextField: "tenKhachHang",
        //    dataValueField: "iD_KhachHang",
        //    height: 290,
        //    virtual: {
        //        itemHeight: 26,
        //        valueMapper: function (option) {
        //            console.log(option);
        //            //ComboboxDataService.getDataKhachHang().then(function (result) {
        //            //    option.success(result.data);
        //            //});
        //        }
        //    },
        //    dataSource: {
        //        serverPaging: true,
        //        serverFiltering: true,
        //        pageSize: 10,
        //        transport: {
        //            read: function (options) {
        //                //console.log()
        //                ComboboxDataService.getDataKhachHang_ServerPaging(options.data.take, options.data.skip).then(function (result) {
        //                    options.success(result.data);
        //                });
        //            }
        //        }
        //    }
        //});
    }

    function convertValues(value) {
        var data = {};

        value = $.isArray(value) ? value : [value];

        for (var idx = 0; idx < value.length; idx++) {
            data["values[" + idx + "]"] = value[idx];
        }

        return data;
    }

    $scope.listFieldsgrid = {
        iD_KhachHang: {
            type: "number"
        },
        maKH: {
            type: "string"
        }
    };
    $scope.listColumnsgrid = function () {
        var dataList = [];

        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({
            field: "thaoTac", title: $.i18n('header_thaotac'),
            template: kendo.template($("#thaotac").html()),
            attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px"
        });
        dataList.push({
            field: "anhDaiDien", title: $.i18n('header_anhdaidien'),
            template: function (dataitem) {
                let url = (dataitem.anhDaiDien == '') ? 'assets/img/noimage.png' : (SERVERIMAGE + dataitem.anhDaiDien);
                return '<div><img class="img-anhdaidien-grid rounded-circle img-thumbnail" src="' + url + '"> </img></div>';
            },
            attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px"
        });
        dataList.push({
            field: "noiDung", title: $.i18n('header_noidung'),
            template: kendo.template($("#noidung").html()),
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid
        });

        return dataList;
    }
    $scope.loadgrid = function (idKhachHang, idnhanvien, thaoTac) {
        kendo.ui.progress($("#grid"), true);
        commonOpenLoadingText("#btn_xembaocao");
        $scope.gridOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".content-filter").height() + $(".toolbarmenu").height());
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
            columns: $scope.listColumnsgrid()
        };

        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        NhanVienDetail.getLichSuThaoTac(idKhachHang, idnhanvien, thaoTac, fromdate, todate).then(function (result) {
            $scope.gridData = {
                data: result.data,
                schema: {
                    model: {
                        fields: $scope.listFieldsgrid
                    }
                },
                pageSize: 20
            };
            kendo.ui.progress($("#grid"), false);
            commonCloseLoadingText("#btn_xembaocao")
        });
    }

    //event
    $scope.xemBaoCao = function () {
        var idKhachHang = 0;
        var idnhanvien = 0;
        var idthaoTac = 0;

        if ($scope.khachhangselect != undefined)
            idKhachHang = ($scope.khachhangselect.iD_KhachHang < 0) ? 0 : $scope.khachhangselect.iD_KhachHang;
        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        if ($scope.thaotacselect != undefined)
            idthaoTac = ($scope.thaotacselect.id < 0) ? 0 : $scope.thaotacselect.id;

        $scope.loadgrid(idKhachHang, idnhanvien, idthaoTac);
    }
    $scope.xuatExcel = function () {
        commonOpenLoadingText("#btn_xuatexcel");
        var idKhachHang = 0;
        var idnhanvien = 0;
        var idthaoTac = 0;
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);

        if ($scope.khachhangselect != undefined)
            idKhachHang = ($scope.khachhangselect.iD_KhachHang < 0) ? 0 : $scope.khachhangselect.iD_KhachHang;
        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        if ($scope.thaotacselect != undefined)
            idthaoTac = ($scope.thaotacselect.id < 0) ? 0 : $scope.thaotacselect.id;

        NhanVienDetail.getExcelLichSuThaoTac(idKhachHang, idnhanvien, idthaoTac, fromdate, todate).then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayravuilongloadlaitrang') }, 'warning');

            commonCloseLoadingText("#btn_xuatexcel")
        });
    }

    $scope.khachhangOnChange = function () {
        $scope.khachhangselect = this.khachhangselect;
    }
    $scope.thaotacOnChange = function () {
        $scope.thaotacselect = this.thaotacselect;
    }
    $scope.nhanvienOnChange = function () {
        $scope.nhanvienselect = this.nhanvienselect;
    }
    $scope.fromDateChanged = function () {
        $scope.minDate = $scope.obj_TuNgay;
    }
    $scope.toDateChanged = function () {
        $scope.maxDate = $scope.obj_DenNgay;
    }

    //các hàm được defined ở server qua template #noidung
    $scope.openalbumanh = function () {
        let myGrid = $('#grid').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;

        id = dataItem.id;

        var url = $state.href('album', { idalbum: id });
        window.open(url, '_blank');
    }
    $scope.openchitietdonhang = function () {
        let myGrid = $('#grid').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;

        id = dataItem.id;

        var url = $state.href('editdonhang', { iddonhang: id });
        window.open(url, '_blank');
    }
    $scope.openlichsuavaoradiem = function () {
        let myGrid = $('#grid').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;

        id = dataItem.id;

        var url = $state.href('lichsuvaoradiem', { idcheckin: id });
        window.open(url, '_blank');
    }

    $scope.init();

})