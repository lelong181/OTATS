angular.module('app').controller('phienLamViecController', function ($scope, $state, $stateParams, $timeout, Notification, ComboboxDataService, NhanVienDetail) {
    CreateSiteMap();

    let param_idnhanvien = 0;
    let param_from = '';
    let param_to = '';

    function init() {
        initparam();
        initdate();
        initcombo();
        loadgrid(0, param_idnhanvien);
    }

    function initparam() {
        param_idnhanvien = ($stateParams.idnhanvien == undefined) ? 0 : $stateParams.idnhanvien;
        param_from = ($stateParams.from == undefined) ? '' : $stateParams.from;
        param_to = ($stateParams.to == undefined) ? '' : $stateParams.to;
    }

    function initdate() {
        let dateNow = new Date();

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
        ComboboxDataService.getDataNhomNhanVien().then(function (result) {
            $scope.nhomnhanvienData = result.data;
        });

        ComboboxDataService.getlistnhanvienbymultiidnhom({ IDNhom: -2 }).then(function (result) {
            if (result.flag) {
                $scope.nhanvienData = result.data;
                if (param_idnhanvien > 0) {
                    $timeout(function () { $("#nhanvien").data("kendoComboBox").value(param_idnhanvien); }, 100);
                }
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
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({ field: "tenNhanVien", title: $.i18n('header_tennhanvien'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "175px" });
        dataList.push({
            field: "kmDiChuyen", title: $.i18n('header_tongkmgps'),
            template: function (dataItem) {
                if (dataItem.kmDiChuyen == '-1')
                    return "<a href='' ng-click= 'chitietKm(" + dataItem.stt + ")'>" + kendo.htmlEncode('?') + "</a>";
                else
                    return kendo.htmlEncode(dataItem.kmDiChuyen);
            },
            attributes: {style: "text-align: center"},
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px"
        });
        dataList.push({
            field: "thoigiandangnhap", title: $.i18n('header_ngay'),
            template: function (dataItem) {
                let date = new Date(dataItem.thoigiandangnhap);
                if (date == null || date.getFullYear() < 1900)
                    return '';
                else
                    return kendo.htmlEncode(kendo.toString(date, formatDate));
            },
            attributes: {style: "text-align: center"},
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        });
        dataList.push({
            field: "thoigiandangnhap", title: $.i18n('header_thoigiandangnhap'),
            template: function (dataItem) {
                let date = new Date(dataItem.thoigiandangnhap);
                if (date == null || date.getFullYear() < 1900)
                    return '';
                else
                    return kendo.htmlEncode(kendo.toString(date, formatTime));
            },
            attributes: {style: "text-align: center"},
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "135px"
        });
        dataList.push({
            field: "thoigiandangxuatphien", title: $.i18n('header_thoigiandangxuat'),
            template: function (dataItem) {
                if (dataItem.thoigiandangxuatphien != null) {
                    let date = new Date(dataItem.thoigiandangxuatphien);
                    if (date == null || date.getFullYear() < 1900)
                        return '';
                    else
                        return kendo.htmlEncode(kendo.toString(date, formatTime));
                }
                else
                    return '';
            },
            attributes: {style: "text-align: center"},
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "130px"
        });
        dataList.push({
            field: "imei", title: $.i18n('header_imei'),
            template: function (dataItem) {
                return (dataItem.imei == null | dataItem.imei == 'null') ? '' : dataItem.imei;
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "300px"
        });
        dataList.push({
            field: "tongGianGianMatKetNoi", title: $.i18n('label_tongthoigianmatketnoi'),
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "300px"
        });
        //dataList.push({
        //    field: "tongGianGianMatKetNoi", title: "Tổng thời gian mất kết nối",
        //    attributes: {style: "text-align: center"},
        //    headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "170px"
        //});
        dataList.push({
            field: "chiTiet", title: $.i18n('header_chitiet'),
            template: '<button ng-click="openlichsudichuyen(#=stt#)" class="btn btn-link btn-menubar" title ="' + $.i18n('header_chitiet')+'" ><i class="fas fa-map-marked-alt fas-sm color-infor"></i></button> ',
            attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "80px"
        });

        return dataList;
    }
    function loadgrid(id_Nhom, idnhanvien) {
        kendo.ui.progress($("#grid"), true);
        commonOpenLoadingText("#dataPhienLamViec");

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
            columns: listColumnsgrid()
        };

        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);

        NhanVienDetail.getPhienLamViec(id_Nhom, idnhanvien, fromdate, todate).then(function (result) {
            $scope.data = result.data;

            $scope.gridData = {
                data: result.data,
                schema: {
                    model: {
                        id_Nhom: {
                            type: "number"
                        },
                        maKH: {
                            type: "string"
                        },
                        thoigiandangnhap: {
                            type: "date"
                        },
                        thoigiandangxuatphien: {
                            type: "date"
                        }
                    }
                },
                pageSize: 20
            };
            kendo.ui.progress($("#grid"), false);
            commonCloseLoadingText("#dataPhienLamViec")
        });

    }

    //event
    $scope.xemBaoCao = function () {
        var id_Nhom = 0;
        var idnhanvien = 0;

        if ($scope.nhomnhanvienselect != undefined)
            id_Nhom = ($scope.nhomnhanvienselect.iD_Nhom < 0) ? 0 : $scope.nhomnhanvienselect.iD_Nhom;
        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;

        loadgrid(id_Nhom, idnhanvien);
    }
    $scope.XuatExcel = function (id_Nhom, idnhanvien) {
        commonOpenLoadingText("#dataPhienLamViec_taifile");
        var id_Nhom = 0;
        var idnhanvien = 0;

        if ($scope.nhomnhanvienselect != undefined)
            id_Nhom = ($scope.nhomnhanvienselect.iD_Nhom < 0) ? 0 : $scope.nhomnhanvienselect.iD_Nhom;
        if ($scope.nhanvienselect != undefined)
            idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);

        NhanVienDetail.getExcelPhienLamViec(id_Nhom, idnhanvien, fromdate, todate).then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayravuilongloadlaitrang') }, 'warning');

            commonCloseLoadingText("#dataPhienLamViec_taifile")
        });

    }

    $scope.chitietKm = function (stt) {
        $scope.sokm = 0;
        let d = new Date();
        let item = $scope.data[stt - 1]
        if (item.kmDiChuyen == "-1") {
            $scope.popupSoKm.center().open();

            let todate = !item.thoigiandangxuatphien ? (!item.thoigiandangnhaptieptheo ? d : item.thoigiandangnhaptieptheo) : item.thoigiandangxuatphien;
            let from = kendo.toString(item.thoigiandangnhap, formatDateTimeFilter);
            let to = kendo.toString(todate, formatDateTimeFilter);

            NhanVienDetail.getKMDiChuyen(item.iD_NhanVien, from, to).then(function (response) {
                $scope.sokm = response.data;
            });
        }
    }
    $scope.openlichsudichuyen = function (stt) {
        let d = new Date();
        let item = $scope.data[stt - 1];
        let todate = !item.thoigiandangxuatphien ? (!item.thoigiandangnhaptieptheo ? d : item.thoigiandangnhaptieptheo) : item.thoigiandangxuatphien;
        let from = kendo.toString(item.thoigiandangnhap, formatDateTimeFilter);
        let to = kendo.toString(todate, formatDateTimeFilter);

        let url = $state.href('lotrinhdichuyen', { idnhanvien: item.iD_NhanVien, tungay: from, denngay: to });
        window.open(url, '_blank');
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
    $scope.nhanvienOnChange = function () {
        $scope.nhanvienselect = this.nhanvienselect;
    }
    $scope.fromDateChanged = function () {
        $scope.minDate = $scope.obj_TuNgay;
    }
    $scope.toDateChanged = function () {
        $scope.maxDate = $scope.obj_DenNgay;
    }
    init();

    

})