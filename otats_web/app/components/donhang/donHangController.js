angular.module('app').controller('donHangController', function ($rootScope, $scope, $location, $state, $stateParams, $timeout, Notification, ComboboxDataService, donHangDataService) {
    CreateSiteMap();

    let param_from = '';
    let param_to = '';

    let param_idmathang = 0;
    let param_idkhachhang = 0;
    let param_idnhanvien = 0;

    let param_idtrangthaixem = -1;
    let param_idtrangthaihoantat = -1;
    let param_idtrangthaigiaohang = 0;
    let param_idtrangthaithanhtoan = 0;
    let param_donhangtaidiem = 0;

    function init() {
        if ($rootScope.isAdmin == 1)
            getquyen();

        $scope.showthemmoi = ($rootScope.isAdmin == 0);
        $scope.showthemmoi = true;

        initparam();

        initdate();
        initcombo();

        let data = {
            IdNhanVien: ($rootScope.isAdmin == 0) ? $rootScope.UserInfo.iD_QuanLy : param_idnhanvien,
            IdMatHang: param_idmathang,
            idKhachHang: param_idkhachhang,
            trangthaixem: param_idtrangthaixem,
            ttht: param_idtrangthaihoantat,
            ttgh: param_idtrangthaigiaohang,
            tttt: param_idtrangthaithanhtoan,
            from: kendo.toString($scope.obj_TuNgay, formatDateTimeFilter),
            to: kendo.toString($scope.obj_DenNgay, formatDateTimeFilter),
            donhangtaidiem: param_donhangtaidiem,
            ListIDNhom: ''
        }

        loadgrid(data);
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

    function initparam() {
        param_from = ($stateParams.from == undefined) ? '' : $stateParams.from;
        param_to = ($stateParams.to == undefined) ? '' : $stateParams.to;

        param_idmathang = ($stateParams.idmathang == undefined) ? 0 : $stateParams.idmathang;
        param_idkhachhang = ($stateParams.idkh == undefined) ? 0 : $stateParams.idkh;
        param_idnhanvien = ($stateParams.idnv == undefined) ? 0 : $stateParams.idnv;

        param_idtrangthaixem = ($stateParams.idtrangthaixem == undefined) ? -1 : $stateParams.idtrangthaixem;
        param_idtrangthaihoantat = ($stateParams.ttht == undefined) ? -1 : $stateParams.ttht;
        param_idtrangthaigiaohang = ($stateParams.ttgh == undefined) ? 0 : $stateParams.ttgh;
        param_idtrangthaithanhtoan = ($stateParams.tttt == undefined) ? 0 : $stateParams.tttt;
        param_donhangtaidiem = ($stateParams.donhangtaidiem == undefined) ? undefined : $stateParams.donhangtaidiem;
    }

    function initdate() {
        var dateNow = new Date();

        if (param_from != '') {
            $scope.obj_TuNgay = new Date(param_from);
        } else
            $scope.obj_TuNgay = new Date(dateNow.setHours(0, 0, 0));

        if (param_to != '') {
            $scope.obj_DenNgay = new Date(param_to);
        } else
            $scope.obj_DenNgay = new Date(dateNow.setHours(23, 59, 59));

        $scope.maxDate = $scope.obj_DenNgay;
        $scope.minDate = $scope.obj_TuNgay;
    }
    function initcombo() {
        let lang = $rootScope.lang.substring(0, 2);
        ComboboxDataService.getDataNhomNhanVien().then(function (result) {
            let ar = result.data;
            ar.shift();
            $scope.nhomnhanvienData = ar;
        });
        ComboboxDataService.getDataNhanVien().then(function (result) {
            $scope.nhanvienData = result.data;

            if (param_idnhanvien > 0) {
                let arr_all = result.data;
                let arr_one = arr_all.filter((item) => {
                    return (item.idnv == param_idnhanvien)
                })

                if (arr_one.length > 0)
                    $scope.nhanvienselect = arr_one[0]
                $timeout(function () {
                    $("#nhanvien").data("kendoComboBox").value(param_idnhanvien)
                }, 100);

            }

        });

        ComboboxDataService.getDataTrangThaiGiaoHangDonHang(lang).then(function (result) {
            $scope.trangthaigiaohangData = result.data;
        });
        ComboboxDataService.getDataTrangThaiThanhToanDonHang(lang).then(function (result) {
            $scope.trangthaithanhtoanData = result.data;
        });
        ComboboxDataService.getDataTrangThaiHoanTatDonHang(lang).then(function (result) {
            $scope.trangthaihoantatData = result.data;
        });
        ComboboxDataService.getDataTrangThaiXemDonHang(lang).then(function (result) {
            $scope.trangthaixemData = result.data;

            if (param_idtrangthaixem > -1) {
                let arr_all = result.data;
                let arr_one = arr_all.filter((item) => {
                    return (item.id == param_idtrangthaixem)
                })

                if (arr_one.length > 0)
                    $scope.trangthaixemselect = arr_one[0]

                $("#trangthaixem").data("kendoComboBox").value(param_idtrangthaixem)
            }

            settextparam();
        });
    }

    function listColumnsgrid() {
        let dataList = [];

        dataList.push({ headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, selectable: true, width: "40px" });
        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({
            field: "maThamChieu", title: $.i18n("header_madonhang"),
            template: function (dataItem) {
                return "<a href='' class='color-primary' ng-click='openDetailFromGrid(" + kendo.htmlEncode(dataItem.iD_DonHang) + ")'>" + kendo.htmlEncode(dataItem.maThamChieu) + "</a>";
            },
            footerTemplate: $.i18n("header_tongtien"),
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px"
        });
        dataList.push({ field: "tenKhachHang", title: $.i18n("header_tenkhachhang"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px" });
        dataList.push({
            field: "tenTrangThaiDongHang", title: $.i18n("header_trangthaidonhang"),
            template: function (e) {
                return e.tenTrangThaiDongHang;
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px"
        });
        dataList.push({ field: "tenNhanVien", title: $.i18n("header_tennhanvien"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        //dataList.push({ field: "maKH", title: $.i18n("header_makhachhang"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px" });
        //dataList.push({ field: "dienThoai", title: $.i18n("header_sodienthoai"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px" });
        //dataList.push({ field: "diaChi", title: $.i18n("header_diachi"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "300px" });
        dataList.push({ field: "ghiChu", title: $.i18n("header_ghichu"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px" });
        dataList.push({
            field: "tongTien", title: $.i18n("header_tongtien"),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), footerTemplate: formatNumberInFooterGrid('tongTien.sum', $rootScope.UserInfo.dinhDangSo),
            attributes: { style: "text-align: right" }, footerAttributes: { style: "text-align: right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: false, width: "120px"
        });
        dataList.push({
            field: "tienDaThanhToan", title: $.i18n("header_dathanhtoan"),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), footerTemplate: formatNumberInFooterGrid('tienDaThanhToan.sum', $rootScope.UserInfo.dinhDangSo),
            attributes: { style: "text-align: right" }, footerAttributes: { style: "text-align: right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: false, width: "120px"
        });
        dataList.push({
            field: "conLai", title: $.i18n("header_conlai"),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), footerTemplate: formatNumberInFooterGrid('conLai.sum', $rootScope.UserInfo.dinhDangSo),
            attributes: { style: "text-align: right" }, footerAttributes: { style: "text-align: right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: false, width: "120px"
        });
        dataList.push({ field: "ngayLap", format:"{0:dd/MM/yyyy}", title: $.i18n("header_ngaylap"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: true, width: "150px" });
        dataList.push({ field: "ngayXuatVe", format: "{0:dd/MM/yyyy}", title: "Ngày xuất vé", headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: true, width: "150px" });

        dataList.push({
            field: "isProcess_Name", title: $.i18n("header_trangthaihoantat"),
            template: function (dataItem) {
                return "<span> <i class='fas fa-circle fas-sm' ng-class='{warning: dataItem.isProcess == 0, primary: dataItem.isProcess == 1, danger: dataItem.isProcess == 2}'></i> " + kendo.htmlEncode(dataItem.isProcess_Name) + "</span>";
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px"
        });
        //dataList.push({
        //    field: "tenTrangThaiGiaoHang", title: $.i18n("header_trangthaigiaohang"),
        //    template: function (dataItem) {
        //        return "<span> <i class='fas fa-truck fas-sm' ng-class='{danger: dataItem.iD_TTGH == 1, infor: dataItem.iD_TTGH == 2, primary: dataItem.iD_TTGH == 4}'></i> " + kendo.htmlEncode(dataItem.tenTrangThaiGiaoHang) + "</span>";
        //    },
        //    headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        //});

        //dataList.push({
        //    field: "diaChiTao", title: $.i18n("header_vitritao"),
        //    template: function (dataItem) {
        //        if (dataItem.toaDoKhachHang == '' || dataItem.toaDoKhachHang == '0.0000000000000, 0.0000000000000')
        //            return '<a href="" class="color-primary" ng-click="xemvitrichuacotoado()" >' + kendo.htmlEncode(dataItem.diaChiTao) + '</a>';
        //        else
        //            return '<a href="" class="color-primary" ng-click="xemvitri(' + kendo.htmlEncode(dataItem.viDo) + ',' + kendo.htmlEncode(dataItem.kinhDo) + ')" >' + kendo.htmlEncode(dataItem.diaChiTao) + '</a>';
        //    },
        //    headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "500px"
        //});
        //dataList.push({
        //    field: "toaDoKhachHang", title: $.i18n("menu_vitrikhachhang"),
        //    headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "300px"
        //});

        return dataList;
    }
    function loadgrid(_data) {
        kendo.ui.progress($("#grid"), true);
        commonOpenLoadingText("#btn_loaddonghang");
        $scope.gridOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".sitemap").height() + $(".content-filter").height() + $(".toolbarmenu").height());
                if (heightGrid > 400)
                    return heightGrid - 100;
                else
                    return 400;
            },
            dataBinding: function () {
                RecordNumber = (this.dataSource.page() - 1) * this.dataSource.pageSize();
            },
            excel: {
                allPages: true
            },
            resizable: true,
            editable: false,
            filterable: {
                mode: "row"
            },
            pageable: $scope.lang == 'vi-vn' ? pageableShort_vi : pageableShort_en,
            columns: listColumnsgrid()
        };
        donHangDataService.getlist(_data).then(function (result) {
            $scope.gridData = {
                data: result.data,
                schema: {
                    model: {
                        id: 'iD_DonHang',
                        fields: {
                            ngayLap: {
                                type:"date"
                            },
                            ngayXuatVe: {
                                type: "date"
                            },
                            iD_DonHang: {
                                type: "number"
                            },
                            tongTien: {
                                type: "number"
                            },
                            tienDaThanhToan: {
                                type: "number"
                            },
                            conLai: {
                                type: "number"
                            }
                        }
                    }
                },
                pageSize: 20,
                aggregate: [
                  { field: "tongTien", aggregate: "sum" },
                  { field: "tienDaThanhToan", aggregate: "sum" },
                  { field: "conLai", aggregate: "sum" }
                ]
            };

            kendo.ui.progress($("#grid"), false);
            commonCloseLoadingText("#btn_loaddonghang");
        });
    }

    function openFormDetail(_id) {

        let url = $state.href('editdonhang', { iddonhang: _id });
        window.open(url, '_blank');

        //$state.go('editdonhang', { iddonhang: _id });
    }
    function validationOpenDetail(_listRowsSelected) {
        let flag = true;
        let msg = "";
        if (_listRowsSelected.length > 1) {
            msg = $.i18n("label_canchonmotdonhangdethuchien");
            flag = false;
        } else if (_listRowsSelected.length == 0) {
            flag = false;
            msg = $.i18n("label_chuachondonhangdethuchien");
        }
        if (!flag) {
            Notification({ title: $.i18n('label_thongbao'), message: msg }, "error");
        }
        return flag;
    }

    function getdataparam() {

        let dataSource = $("#grid").data("kendoGrid").dataSource;
        let filter = dataSource.filter();

        let data = {
            IdNhanVien: ($rootScope.isAdmin == 0) ? $rootScope.UserInfo.iD_QuanLy : 0,
            IdMatHang: 0,
            idKhachHang: 0,
            trangthaixem: -1,
            ttht: -1,
            ttgh: 0,
            tttt: 0,
            from: kendo.toString($scope.obj_TuNgay, formatDateTimeFilter),
            to: kendo.toString($scope.obj_DenNgay, formatDateTimeFilter),
            donhangtaidiem: 0,
            ListIDNhom: '',
            Filters: filter
        }

        if ($scope.nhanvienselect != undefined)
            data.IdNhanVien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        if ($scope.trangthaixemselect != undefined)
            data.trangthaixem = ($scope.trangthaixemselect.id < 0) ? 0 : $scope.trangthaixemselect.id;
        if ($scope.trangthaigiaohangselect != undefined)
            data.ttgh = ($scope.trangthaigiaohangselect.id < 0) ? 0 : $scope.trangthaigiaohangselect.id;
        if ($scope.trangthaithanhtoanselect != undefined)
            data.tttt = ($scope.trangthaithanhtoanselect.id < 0) ? 0 : $scope.trangthaithanhtoanselect.id;
        if ($scope.trangthaihoantatselect != undefined)
            data.ttht = ($scope.trangthaihoantatselect.id < 0) ? 0 : $scope.trangthaihoantatselect.id;


        if ($scope.selectedIdNhom != null && $scope.selectedIdNhom.length > 0) {
            let result = '';
            let length = $scope.selectedIdNhom.length;
            let sizeArray = length - 1;
            for (let i = 0; i < length; i++) {
                if (i != sizeArray) {
                    result += ($scope.selectedIdNhom[i] + ",");
                } else {
                    result += $scope.selectedIdNhom[i];
                }
            }

            data.ListIDNhom = result;
        }

        return data;
    }
    function settextparam() {
        let trangthai = '';

        if ($scope.trangthaixemselect != undefined)
            trangthai = (($scope.trangthaixemselect.value < 0) ? '' : $scope.trangthaixemselect.value);
        if ($scope.trangthaigiaohangselect != undefined)
            trangthai = trangthai + ' - ' + (($scope.trangthaigiaohangselect.value < 0) ? '' : $scope.trangthaigiaohangselect.value);
        if ($scope.trangthaithanhtoanselect != undefined)
            trangthai = trangthai + ' - ' + (($scope.trangthaithanhtoanselect.value < 0) ? '' : $scope.trangthaithanhtoanselect.value);
        if ($scope.trangthaihoantatselect != undefined)
            trangthai = trangthai + ' - ' + (($scope.trangthaihoantatselect.value < 0) ? '' : $scope.trangthaihoantatselect.value);
        if (trangthai.length <= 0)
            trangthai = $.i18n('label_all');

        $scope.filterTrangThai = trangthai;
    }
    function getliststringnhom() {
        let selectedOptions = $('#nhomnhanvien')[0].selectedOptions;
        let list = '';
        angular.forEach(selectedOptions, function (value, key) {
            list = list + value.innerText + ',';
        });

        return list;
    };
    function getliststringidnhom() {
        let listid = '';
        angular.forEach($scope.selectedIdNhom, function (value, key) {
            listid = listid + value + ',';
        });

        return listid;
    };

    function validateListDelete() {
        let flag = true;
        let message = "";

        let listRowsSelected = commonGetRowSelected("#grid");

        if (listRowsSelected.length > 0) {
            message = $.i18n('label_donhangcoma');
            angular.forEach(listRowsSelected, function (obj) {
                if (obj.isProcess == 0 && obj.iD_TrangThaiDonHang < 3 && obj.iD_TTGH == 1) {
                } else {
                    message += obj.maThamChieu + ', ';
                    flag = false;
                }
            });
            message += $.i18n('label_khongduochuy');
        } else {
            flag = false;
            message = $.i18n('label_banphaichondonhangdethuchienhuy');
        }

        if (!flag) {
            Notification({ title: $.i18n('label_thongbao'), message: message }, "error");
        }
        return flag;
    }

    //even
    $scope.fromDateChanged = function () {
        $scope.minDate = $scope.obj_TuNgay;
    };

    $scope.toDateChanged = function () {
        $scope.maxDate = $scope.obj_DenNgay;
    };

    $scope.openfilterform = function () {
        $scope.formfilter.center().open();
    }
    $scope.donglocdonhang = function () {
        $scope.formfilter.center().close();
        settextparam();
    }
    $scope.apdunglocdonhang = function () {
        $scope.formfilter.center().close();
        settextparam();

        let data = getdataparam();
        loadgrid(data);
    }

    $scope.loaddonhang = function () {
        let data = getdataparam();

        loadgrid(data);
    }
    //$scope.themdonhang = function () {
    //    let url = $state.href('adddonhang');
    //    window.open(url, '_blank');

    //    //$state.go('adddonhang');
    //}
    $scope.themdonhang = function () {
        //let url = $state.href('adddonhangdv');
        //window.open(url, '_blank');

        $state.go('adddonhangdv');
    }
    $scope.suadonhang = function () {
        let listRowsSelected = commonGetRowSelected("#grid");
        if (validationOpenDetail(listRowsSelected)) {
            openFormDetail(listRowsSelected[0].iD_DonHang);
        }
    }
    $scope.xuatexcel = function () {
        //commonOpenLoadingText("#btn_xuatexceldonhang");
        //let data = getdataparam();

        //donHangDataService.exportExcel(data).then(function (result) {
        //    if (result.flag)
        //        commonDownFile(result.data);
        //    else
        //        Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayravuilongloadlaitrang') }, 'warning');
        //    commonCloseLoadingText("#btn_xuatexceldonhang");
        //});
        $("#grid").data("kendoGrid").saveAsExcel();
    }
    $scope.xuatexcelchitiet = function () {
        commonOpenLoadingText("#btn_xuatexcelchitietdonhang");
        let data = getdataparam();

        donHangDataService.exportExcelDetail(data).then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayravuilongloadlaitrang') }, 'warning');
            commonCloseLoadingText("#btn_xuatexcelchitietdonhang");
        });
    }
    $scope.xoadonhang = function () {
        if (validateListDelete()) {
            let arr = $("#grid").data("kendoGrid").selectedKeyNames();
            if (arr.length <= 0)
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_chuachondonhangdethuchien') }, 'warning');
            else {
                openConfirm($.i18n("label_bancochacchanmuonhuydonhang"), 'apDungXoaDonHang', null, arr);
            }
        }
    }

    $scope.apDungXoaDonHang = function (_listid) {
        commonOpenLoadingText("#btn_xoadonhang");

        donHangDataService.huydonhang(_listid, '').then(function (result) {
            if (result.flag) {
                $scope.loaddonhang();
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_capnhathuyhangthanhcong') }, 'success');
            } else {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloisayrakhongthehuydonhang') }, 'warning');
            }

            commonCloseLoadingText("#btn_xoadonhang");
        })
    }

    $scope.openDetailFromGrid = function (iD_DonHang) {
        openFormDetail(iD_DonHang);
    }
    $scope.xemvitrichuacotoado = function () {
        Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_khachhangchuacothongtinvitri') }, 'warning');
    }
    $scope.xemvitri = function (vido, kinhdo) {
        let url = 'https://www.google.com/maps/dir/' + vido.toString() + ',' + kinhdo.toString();
        window.open(url, '_blank');
    }

    $scope.nhanvienOnChange = function () {
        $scope.nhanvienselect = this.nhanvienselect;
    }
    $scope.trangthaixemOnChange = function () {
        $scope.trangthaixemselect = this.trangthaixemselect;
    }
    $scope.trangthaigiaohangOnChange = function () {
        $scope.trangthaigiaohangselect = this.trangthaigiaohangselect;
    }
    $scope.trangthaithanhtoanOnChange = function () {
        $scope.trangthaithanhtoanselect = this.trangthaithanhtoanselect;
    }
    $scope.trangthaihoantatOnChange = function () {
        $scope.trangthaihoantatselect = this.trangthaihoantatselect;
    }

    init();

    $("#grid").on("dblclick", "tr[role='row']", function () {
        let grid = $("#grid").data("kendoGrid");
        let row = grid.table.find("[data-uid=" + $(this).attr("data-uid") + "]");

        grid.clearSelection();
        grid.select(row);

        let selectedItem = grid.dataItem(grid.select());

        openFormDetail(selectedItem.iD_DonHang);
    })

})