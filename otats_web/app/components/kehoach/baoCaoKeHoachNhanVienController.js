angular.module('app').controller('baoCaoKeHoachNhanVienController', function ($location, $rootScope, $scope, Notification, ComboboxDataService, baoCaoKeHoachDataService) {
    CreateSiteMap();
    let idnhanvien = 0;
    var idkhachhang = 0;
    function init() {
        initdate();
        initcombo();
        getquyen();
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
    function initdate() {
        var dateNow = new Date();
        $scope.obj_TuNgay = new Date(dateNow);
        $scope.obj_DenNgay = new Date(dateNow);
        $scope.maxDate = $scope.obj_DenNgay;
        $scope.minDate = $scope.obj_TuNgay;
    }
    function initcombo() {
        ComboboxDataService.getDataNhanVien().then(function (result) {
            $scope.nhanvienData = result.data;
            $scope.nhanvienData2 = result.data;
            if (!result.flag) {
                Notification({ title: $.i18n("label_thongbao"), message: $.i18n("label_coloixayra_khongtheloaddanhsachnhanvien") }, 'warning');
            }
            loadgrid();

        });
        //ComboboxDataService.getDataKhachHang().then(function (result) {
        //    $scope.khachhangData = result.data;
        //});
    }
    function listColumnsgrid() {
        var dataList = [];

        dataList.push({ headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, selectable: true, width: "40px" });
        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({
            field: "thoiGianCheckInDuKien", title: $.i18n("header_ngay"), template: function (dataItem) {
                return kendo.htmlEncode(kendo.toString(dataItem.thoiGianCheckInDuKien, formatDate));
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { class: "text-center" }, filterable: false, width: "120px"
        });
        dataList.push({
            field: "tenNhanVien", title: $.i18n("header_tennhanvien"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px"
        });
        dataList.push({
            field: "tenKhachHang", title: $.i18n("header_khachhang"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "thoiGianCheckInDuKien", title: $.i18n("header_dukienvao"), template: function (dataItem) {
                return kendo.htmlEncode(kendo.toString(dataItem.thoiGianCheckInDuKien, formatTime));
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { class: "text-center" }, filterable: false, width: "100px"
        });
        dataList.push({
            field: "thoiGianCheckInThucTe", title: $.i18n("header_thuctevao"), template: function (dataItem) {
                if (dataItem.thoiGianCheckInThucTe.getFullYear() == 1900) {
                    return "";
                }
                return kendo.htmlEncode(kendo.toString(dataItem.thoiGianCheckInThucTe, formatTime));
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { class: "text-center" }, filterable: false, width: "100px"
        });
        dataList.push({
            field: "thoiGianCheckOutDuKien", title: $.i18n("header_dukienra"), template: function (dataItem) {
                return kendo.htmlEncode(kendo.toString(dataItem.thoiGianCheckOutDuKien, formatTime));
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { class: "text-center" }, filterable: false, width: "100px"
        });
        dataList.push({
            field: "thoiGianCheckOutThucTe", title: $.i18n("header_thuctera"), template: function (dataItem) {
                if (dataItem.thoiGianCheckOutThucTe.getFullYear() == 1900) {
                    return "";
                }
                return kendo.htmlEncode(kendo.toString(dataItem.thoiGianCheckOutThucTe, formatTime));
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { class: "text-center" }, filterable: false, width: "100px"
        });
        dataList.push({
            field: "ngayTao", title: $.i18n("header_ngaytao"), template: function (dataItem) {
                return kendo.htmlEncode(kendo.toString(dataItem.ngayTao, formatDate));
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { class: "text-center" }, filterable: false, width: "100px"
        });
        dataList.push({
            field: "trangThai_Text", title: $.i18n("header_trangthai"),
            template: '#if (trangThai == 1){# <span title="#= $.i18n(text_color_mota) #"  class="trangthaivaodiem" style="background-color:#= text_color #;">' + $.i18n('label_davaodiem') + '</span> #}else if(trangThai == 0){#<span title="#= $.i18n(text_color_mota) #" class="trangthaivaodiem" style="background-color:#= text_color #;">' + $.i18n('label_chuavaodiem') + '</span>#}#',
            //template: '<span title="#= text_color_mota #"  class="trangthaivaodiem" style="background-color:#= text_color #;">#= trangThai_Text #</span>',
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { class: "text-center" }, filterable: defaultFilterableGrid, width: "200px"
        });
        dataList.push({
            field: "ngay_NhanHuy",
            // format: "{0:dd/MM/yyyy}",
            template: function (e) {
                try {
                    if (e.ngay_NhanHuy) {
                        if (e.ngay_NhanHuy.getFullYear() > 1900) {
                            return kendo.toString(e.ngay_NhanHuy, 'dd/MM/yyyy HH:mm');
                        } else {
                            return "";
                        }
                    } else {
                        return "";
                    }
                } catch (ex) {
                    var d = new Date(e.ngay_NhanHuy);
                    if (d.getFullYear()) {
                        if (d.getFullYear() > 1900) {
                            return kendo.toString(d, 'dd/MM/yyyy HH:mm');
                        }
                    }
                    return "";
                }
            },
            title: $.i18n("header_ngaynhantuchoi"),
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { class: "text-center" }, filterable: false, width: "200px"
        });

        dataList.push({
            field: "ghiChu_NhanHuy",
            title: $.i18n("header_ghichu"),
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { class: "text-center" }, filterable: false, width: "200px"
        });

        return dataList;
    }
    function loadgrid() {
        kendo.ui.progress($("#grid"), true);
        commonOpenLoadingText("#btn_bm022xembaocao");

        $scope.gridOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".content-filter").height());
                return heightGrid - 50;
            },
            persistSelection: true,
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

        //let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        //let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        var tungay = $scope.obj_TuNgay;
        var tungays = tungay.getFullYear() + "-" + parseInt(tungay.getMonth() + 1) + "-" + tungay.getDate() + " 00:00:00";
        var denngay = $scope.obj_DenNgay;
        var denngays = denngay.getFullYear() + "-" + parseInt(denngay.getMonth() + 1) + "-" + denngay.getDate() + " 00:00:00";
        console.log($scope.idnhanvienselect);
        let idnhanvien = 0;
        if ($scope.idnhanvienselect != undefined)
            idnhanvien = ($scope.idnhanvienselect < 0) ? 0 : $scope.idnhanvienselect;
        let formData = {
            TuNgay: tungays,
            ID_NhanVien: idnhanvien,
            DenNgay: denngays,
        }
        baoCaoKeHoachDataService.getBaoCaoKeHoachNhanVien(formData).then(function (result) {
            $scope.gridData = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                            thoiGianCheckInDuKien: { type: "date" },
                            thoiGianCheckInThucTe: { type: "date" },
                            thoiGianCheckOutDuKien: { type: "date" },
                            thoiGianCheckOutThucTe: { type: "date" },
                            ngayTao: { type: "date" }
                        }
                    }
                },
                pageSize: 20,
            };

            kendo.ui.progress($("#grid"), false);
            commonCloseLoadingText("#btn_bm022xembaocao")
        });

    }

    function validationOpenDetail(listRowsSelected) {
        let flag = true;
        let msg = "";
        if (listRowsSelected.length > 1) {
            msg = $.i18n("label_canchonmotnhanviendethuchien");
            flag = false;
        } else if (listRowsSelected.length == 0) {
            flag = false;
            msg = $.i18n("label_chuachonnhanviendethuchien");
        }
        if (!flag) {
            Notification({ title: $.i18n("label_thongbao"), message: $.i18n(msg) }, "error");
        }
        return flag;
    }
    function openFormDetail(data) {
        $scope.formdetail.center().open();
        if (data != null) {
            $scope.idkehoach = data.idKeHoach;
            loadeditThongtinKeHoach(data);
        }
        else {
            $scope.idkehoach = 0;
            loadaddThongtinKeHoach();
        }
    }
    function loadeditThongtinKeHoach(data) {
        $scope.obj = {
            ID: data.idKeHoach,
            ID_NhanVien: data.idNhanVien,
            ID_KhachHang: data.idKhachHang,
            Ngay: data.thoiGianCheckInDuKien,
            BatDau: data.thoiGianCheckInDuKien,
            KetThuc: data.thoiGianCheckOutDuKien,
            ViecCanLam: data.viecCanLam,
            GhiChu: data.ghiChu
        };

        if ($scope.obj.ID_NhanVien > 0)
            $scope.nhanvienselect = $scope.obj.ID_NhanVien;

        ComboboxDataService.getDataKhachHangByNhanVien($scope.nhanvienselect).then(function (result) {
            $scope.khachhangData = result.data;
            if ($scope.obj.ID_KhachHang > 0)
                $scope.khachhangselect = $scope.obj.ID_KhachHang;

            $("#nhanVien").data("kendoComboBox").value($scope.obj.ID_NhanVien);
            $("#khachHang").data("kendoComboBox").value($scope.obj.ID_KhachHang);
        });
        $scope.obj_ngayBDGanNhat = $scope.obj.BatDau;
        $scope.obj_ngayBDTiepTheo = $scope.obj.KetThuc;
    }
    function loadaddThongtinKeHoach() {
        var d = new Date();

        var datebegin = new Date();
        datebegin.setMinutes(datebegin.getMinutes() + 15);
        var dateend = new Date();
        dateend.setHours(19);
        dateend.setMinutes(0);
        dateend.setSeconds(0);
        dateend.setMilliseconds(0);
        $scope.obj_ngayBDGanNhat = datebegin;
        $scope.obj_ngayBDTiepTheo = dateend;
        $scope.obj = {
            viecCanLam: '',
            ghiChu: '',
            ID: 0,
            ID_NhanVien: 0,
            ID_KhachHang: 0,
            Ngay: datebegin,
            BatDau: datebegin,
            KetThuc: dateend
        }

        $("#nhanVien").data("kendoComboBox").value("");
        $("#khachHang").data("kendoComboBox").value("");
    }


    function validatethemsua() {
        let flag = true;
        let msg = '';

        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect < 0) ? 0 : $scope.nhanvienselect;
        if ($scope.khachhangselect != undefined)
            idkhachhang = ($scope.khachhangselect < 0) ? 0 : $scope.khachhangselect;

        //if ($scope.obj.viecCanLam == '' || $scope.obj.viecCanLam == undefined) {
        //    flag = false;
        //    msg = 'Việc cần làm không được để trống';
        //    $("#viecCanLam").focus();
        //}

        //if (flag && ($scope.obj.ghiChu == '' || $scope.obj.ghiChu == undefined)) {
        //    flag = false;
        //    msg = 'Ghi chú không để trống';
        //    $("#ghiChu").focus();
        //}

        if (flag && idnhanvien <= 0) {
            flag = false;
            msg = $.i18n("label_nhanvienkhongduocdetrong");
            $("#nhanVien").focus();
        }
        if (flag && idkhachhang <= 0) {
            flag = false;
            msg = $.i18n('label_khachhangkhongduocdetrong');
            $("#khachHang").focus();
        }

        if (flag && $scope.obj_ngayBDTiepTheo <= $scope.obj_ngayBDGanNhat) {
            flag = false;
            msg = $.i18n('label_thoigiankehoachkhongphuhop');
        }

        if (!flag)
            Notification({ title: $.i18n("label_thongbao"), message: $.i18n(msg) }, 'warning');

        return flag;
    }

    function openConfirm(message, acceptAction, cancelAction, data) {
        var scope = angular.element("#mainContentId").scope();
        $(" <div id='confirmDelete'></div>").appendTo("body").kendoDialog({
            width: "450px",
            closable: true,
            modal: true,
            title: $.i18n("label_xacnhan"),
            content: message,
            actions: [
                {
                    text: $.i18n("button_huy"), primary: false, action: function () {
                        if (cancelAction != null) {
                            scope[cancelAction](data);
                        }
                    }
                },
                {
                    text: $.i18n("button_dongy"), primary: true, action: function () {
                        scope[acceptAction](data);
                    }
                }
            ],
        })
    }

    //event
    $scope.xemBaoCao = function () {
        loadgrid();
    }
    $scope.nhanvienOnChange = function () {
        console.log($scope.idnhanvienselect);
        ComboboxDataService.getDataKhachHangByNhanVien($scope.nhanvienselect).then(function (result) {
            $scope.khachhangData = result.data;
        });
    }
    $scope.fromDateChanged = function () {
        $scope.minDate = $scope.obj_TuNgay;
    }
    $scope.toDateChanged = function () {
        $scope.maxDate = $scope.obj_DenNgay;
    }
    $scope.luuThongTinKeHoach = function () {
        if (validatethemsua()) {
            let ngayBDGanNha = '';
            let ngayBDTiepTheo = '';
            if ($scope.obj_ngayBDGanNhat != undefined && $scope.obj_ngayBDGanNhat != null)
                ngayBDGanNha = kendo.toString($scope.obj_ngayBDGanNhat, formatDateTimeFilter);
            if ($scope.obj_ngayBDTiepTheo != undefined && $scope.obj_ngayBDTiepTheo != null)
                ngayBDTiepTheo = kendo.toString($scope.obj_ngayBDTiepTheo, formatDateTimeFilter);

            //let obj = {
            //    "ID": 0, "ID_NhanVien": "317", "ID_KhachHang": "34690", "Ngay": "2019-9-12 18:33:00", "BatDau": "2019-9-12 18:33:00", "KetThuc": "2019-9-13 18:33:00", "ViecCanLam": "á", "GhiChu": "aa"
            //};

            $scope.obj.ID_NhanVien = ($scope.nhanvienselect < 0) ? 0 : $scope.nhanvienselect;
            $scope.obj.ID_KhachHang = ($scope.khachhangselect < 0) ? 0 : $scope.khachhangselect;
            $scope.obj.Ngay = ngayBDGanNha;
            $scope.obj.BatDau = ngayBDGanNha;
            $scope.obj.KetThuc = ngayBDTiepTheo;            
            commonOpenLoadingText("#btn_luuThongTinKH");
            if ($scope.obj.ID > 0) {
                baoCaoKeHoachDataService.suaBaoCaoKeHoach(kendo.stringify($scope.obj)).then(function (result) {
                    if (result.flag) {
                        if (result.data.success) {
                            Notification({ title: $.i18n("label_thongbao"), message: $.i18n(result.data.message) }, 'success');
                            loadgrid();
                            $scope.formdetail.center().close();
                            commonCloseLoadingText("#btn_luuThongTinKH");
                        } else {
                            Notification({ title: $.i18n("label_thongbao"), message: $.i18n(result.data.message) }, 'warning');
                            commonCloseLoadingText("#btn_luuThongTinKH");
                        }
                    }
                    else
                        Notification({ title: $.i18n("label_thongbao"), message: $.i18n("label_luuthatbai") }, 'warning');
                });
            } else {
                baoCaoKeHoachDataService.luuBaoCaoKeHoach(kendo.stringify($scope.obj)).then(function (result) {
                    if (result.flag) {
                        if (result.data.success) {
                            Notification({ title: $.i18n("label_thongbao"), message: $.i18n(result.data.message) }, 'success');
                            loadgrid();
                            $scope.formdetail.center().close();
                            commonCloseLoadingText("#btn_luuThongTinKH");
                        } else {
                            Notification({ title: $.i18n("label_thongbao"), message: $.i18n(result.data.message) }, 'warning');
                            commonCloseLoadingText("#btn_luuThongTinKH");
                        }
                    }
                    else
                        Notification({ title: $.i18n("label_thongbao"), message: $.i18n("label_luuthatbai") }, 'warning');
                });
            }
        }
    }
    $scope.huyLuuThongTinKeHoach = function () {
        $scope.formdetail.center().close();
    }

    $scope.addKeHoach = function () {
        openFormDetail(null);
    }
    $scope.editKeHoach = function () {
        let listRowsSelected = commonGetRowSelected("#grid");
        console.log(listRowsSelected);
        if (validationOpenDetail(listRowsSelected)) {
            openFormDetail(listRowsSelected[0]);
        }
    }
    $scope.deleteKeHoach = function () {
        let grid = $("#grid").data("kendoGrid");
        let arr = [];
        grid.select().each(function () {
            arr.push(grid.dataItem(this).idKeHoach);
        });
        console.log(arr)
        if (arr.length <= 0)
            Notification({ title: $.i18n("label_thongbao"), message: $.i18n("label_chuachonnhanviendethuchien") }, 'warning');
        else {
            let data = [];
            for (let i = 0; i < arr.length; i++) {
                data.push(parseInt(arr[i]));
            }
            openConfirm($.i18n("label_bancochacchanmuonxoathongtinkehoachnay"), 'apDungXoaKeHoach', null, data);
        }
    }
    $scope.apDungXoaKeHoach = function (data) {
        baoCaoKeHoachDataService.xoaBaoCaoKeHoach(data).then(function (result) {
            if (result.flag) {
                if (result.data.count > 0) {
                    Notification({ title: $.i18n("label_thongbao"), message: $.i18n("label_xoakehoachthanhcong") }, 'success');
                    loadgrid();
                } else {
                    Notification({ title: $.i18n("label_thongbao"), message: $.i18n("label_xoathatbaixinvuilongthulai") }, 'warning');
                }
            }
            else
                Notification({ title: $.i18n("label_thongbao"), message: $.i18n("label_xoathatbaixinvuilongthulai") }, 'warning');
        });
    }

    init();

    $("#grid").on("dblclick", "tr[role='row']", function () {
        let grid = $("#grid").data("kendoGrid");
        let row = grid.table.find("[data-uid=" + $(this).attr("data-uid") + "]");

        grid.clearSelection();
        grid.select(row);

        let selectedItem = grid.dataItem(grid.select());
        openFormDetail(selectedItem);
    })


})
