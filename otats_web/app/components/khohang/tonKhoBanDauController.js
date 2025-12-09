angular.module('app').controller('tonKhoBanDauController', function ($rootScope, $scope, $timeout, Notification, ComboboxDataService, khoHangDataService) {
    CreateSiteMap();

    let AR_CHITIET = []
    let AR_MATHANG = []

    let iD_PhieuTonDau = 0;
    $scope.object = { PhieuTonDau: {}, ChiTietMatHangTonDau: [] };

    function init() {
        initcombobox();

        $scope.obj_NgayChot = new Date();
        $scope.nhanvien = $rootScope.UserInfo.tenAdmin;
        $scope.soluong = 0;

        loadgrid();
    }

    function initcombobox() {
        ComboboxDataService.getDataKhoHang().then(function (result) {
            $scope.khohangData = result.data;
        });
    }
    function loadMatHang(data) {
        $('#mathang').data('kendoComboBox').value("");

        $scope.mathangData = data;
        $scope.mathangselect = undefined;
    };

    function listColumnsgrid() {
        let dataList = [];

        dataList.push({
            field: "xoa", title: $.i18n('header_xoa'),
            template: '<button ng-click="deleterow()" class="btn btn-link btn-menubar" title ="' + $.i18n("button_xoa") + '" ><i class="fas fa-trash fas-sm color-danger"></i></button> ',
            attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "50px"
        });
        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({
            field: "maHang", title: $.i18n('header_mahang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "tenHang", title: $.i18n('header_tenhang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "300px"
        });
        dataList.push({
            field: "tenDonVi", title: $.i18n('header_donvi'),
            attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "soLuong", title: $.i18n('header_soluong'),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            attributes: { style: "text-align: center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });

        return dataList;
    }
    function loadgrid(data) {
        $scope.gridOptions = {
            sortable: true,
            height: function () {
                let heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".content-filter").height() + $(".toolbarmenu").height());
                return heightGrid < 100 ? 500 : heightGrid;
            },
            dataBinding: function () {
                RecordNumber = (this.dataSource.page() - 1) * this.dataSource.pageSize();
            },
            resizable: true,
            editable: true,
            filterable: {
                mode: "row"
            },
            pageable: $scope.lang == 'vi-vn' ? pageableShort_vi : pageableShort_en,
            columns: listColumnsgrid()
        };

        $scope.gridData = {
            data: data,
            schema: {
                model: {
                    fields: {
                        iD_ChiTietPhieuTonDau: {
                            type: "number",
                            editable: false,
                            nullable: true
                        },
                        iD_HangHoa: {
                            type: "number",
                            editable: false,
                            nullable: true
                        },
                        xoa: {
                            type: "string",
                            editable: false,
                            nullable: true
                        },
                        maHang: {
                            type: "string",
                            editable: false,
                            nullable: true
                        },
                        tenHang: {
                            type: "string",
                            editable: false,
                            nullable: true
                        },
                        tenDonVi: {
                            type: "string",
                            editable: false,
                            nullable: true
                        },
                        soLuong: {
                            type: "number",
                            editable: true,
                            validation: {
                                min: 0
                            },
                            nullable: false
                        }
                    }
                }
            },
            pageSize: 20
        };
    }
    function validate() {
        let flag = true;
        let msg = '';

        let idkho = 0;
        if ($scope.khohangselect != undefined)
            idkho = ($scope.khohangselect.iD_Kho < 0) ? 0 : $scope.khohangselect.iD_Kho;

        let myGridAdd = $("#grid").data("kendoGrid");
        let dataSource = myGridAdd.dataSource.data();
        let DataPhieu = {
            ID_PhieuTonDau: iD_PhieuTonDau,
            ID_KhoHang: idkho,
            NgayChotTon: kendo.toString($scope.obj_NgayChot, formatDateTimeFilter),
        };
        let DataChiTiet = [];

        for (i = 0; i < dataSource.length; i++) {
            var obj = {
                ID_ChiTietPhieuTonDau: dataSource[i].iD_ChiTietPhieuTonDau,
                ID_PhieuTonDau: dataSource[i].iD_PhieuTonDau,
                ID_HangHoa: dataSource[i].iD_HangHoa,
                SoLuong: dataSource[i].soLuong
            }
            DataChiTiet.push(obj);
        }

        $scope.object = { PhieuTonDau: DataPhieu, ChiTietMatHangTonDau: DataChiTiet };

        if (idkho <= 0 && flag) {
            flag = false;
            msg = $.i18n('label_chuachonkhohang');
        }
        if ($scope.obj_NgayChot == null && flag) {
            flag = false;
            msg = $.i18n('label_chuachonngaychot');
        }
        if (dataSource.length <= 0 && flag) {
            flag = false;
            msg = $.i18n('label_chuanhapthongtinmathang');
        }

        if (!flag)
            Notification({ title: $.i18n('label_thongbao'), message: msg }, 'warning');

        return flag;
    }

    let fileUpload = '';
    function onUploadExcelSuccess(e) {
        var data = new FormData();
        data.append('file', e.files[0].rawFile);
        var files = e.files[0];
        $.ajax({
            url: urlApi + '/api/uploadfile/savefileExcel',
            processData: false,
            contentType: false,
            data: data,
            type: 'POST'
        }).done(function (result) {
            fileUpload = result;
        }).fail(function (a, b, c) {
            fileUpload = '';
        });
    }
    $("#filesExcel").kendoUpload({
        multiple: false,
        select: onUploadExcelSuccess,
        validation: {
            allowedExtensions: [".xls", ".xlsx"]
        },
        showFileList: true
    });
    $("#filesExcel").closest(".k-upload").find("span").text($.i18n('label_taifiledulieulen'));
    $("#files").closest(".k-upload").find("span").text($.i18n('label_chontep'));

    //event
    $scope.khohangOnChange = function () {
        $scope.khohangselect = this.khohangselect;

        let idkho = 0;
        if ($scope.khohangselect != undefined)
            idkho = ($scope.khohangselect.iD_Kho < 0) ? 0 : $scope.khohangselect.iD_Kho;

        commonOpenLoadingText("#btn_themhang");
        commonOpenLoadingText("#btn_themtatcahang");
        khoHangDataService.getphieutondau(idkho).then(function (response) {
            if (response.flag) {
                let dataPhieuTonDau = response.data.phieuTonDau;
                iD_PhieuTonDau = dataPhieuTonDau.iD_PhieuTonDau;

                if (iD_PhieuTonDau > 0) {
                    $scope.obj_NgayChot = dataPhieuTonDau.ngayChotTon;
                    $scope.dateNgayChot.readonly(true);
                }
                else {
                    $scope.obj_NgayChot = new Date();
                    $scope.dateNgayChot.readonly(false);
                }

                
                if (response.data.danhSachMatHang != null)
                    AR_MATHANG = response.data.danhSachMatHang;
                if (response.data.chiTietMatHangTonDau != null)
                    AR_CHITIET = response.data.chiTietMatHangTonDau;

                loadMatHang(AR_MATHANG);
                loadgrid(AR_CHITIET);

                commonCloseLoadingText("#btn_themhang");
                commonCloseLoadingText("#btn_themtatcahang");
            }
        });
    }
    $scope.mathangOnChange = function () {
        $scope.mathangselect = this.mathangselect;
    }

    $scope.themdong = function () {
        let idMatHang = 0;
        if ($scope.mathangselect != undefined)
            idMatHang = ($scope.mathangselect.idMatHang < 0) ? 0 : $scope.mathangselect.idMatHang;

        if (idMatHang <= 0) {
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_chuachonmathang') }, "error");
        } else {
            let myGridAdd = $("#grid").data("kendoGrid");
            let data = myGridAdd.dataSource.data();

            let obj = {
                iD_ChiTietPhieuTonDau: 0,
                iD_HangHoa: idMatHang,
                maHang: $scope.mathangselect.maHang,
                tenHang: $scope.mathangselect.tenHang,
                tenDonVi: $scope.mathangselect.tenDonVi,
                soLuong: $scope.soluong,
                ghiChu: ''
            }
            myGridAdd.dataSource.add(obj);
            AR_CHITIET.push(obj);

            let myCombo = $("#mathang").data("kendoComboBox");
            let dataCombo = myCombo.dataSource.data();
            angular.forEach(dataCombo, function (item) {
                if (item.idMatHang == idMatHang) {
                    myCombo.dataSource.remove(item);
                }
            });
            $('#mathang').data('kendoComboBox').value("");
            $scope.selectMatHang = undefined;
            AR_MATHANG = AR_MATHANG.filter(x => (x.idMatHang != idMatHang));
        }
    }
    $scope.themtatcadong = function () {
        let idkho = 0;
        if ($scope.khohangselect != undefined)
            idkho = ($scope.khohangselect.iD_Kho < 0) ? 0 : $scope.khohangselect.iD_Kho;
        if (idkho <= 0) {
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_chuachonkhohang') }, "error");
        } else {
            //let myGridAdd = $("#grid").data("kendoGrid");
            //let data = myGridAdd.dataSource.data();

            //let myCombo = $("#mathang").data("kendoComboBox");
            //let dataCombo = myCombo.dataSource.data();
            //console.log(dataCombo);
            if (AR_MATHANG.length > 0) {
                for (i = 0; i < AR_MATHANG.length; i++) {
                    let obj = {
                        iD_ChiTietPhieuTonDau: 0,
                        iD_HangHoa: AR_MATHANG[i].idMatHang,
                        maHang: AR_MATHANG[i].maHang,
                        tenHang: AR_MATHANG[i].tenHang,
                        tenDonVi: AR_MATHANG[i].tenDonVi,
                        soLuong: $scope.soluong,
                        ghiChu: ''
                    }
                    AR_CHITIET.push(obj);
                    //myGridAdd.dataSource.add(obj);
                }
                AR_MATHANG = [];
                loadMatHang(AR_MATHANG);
                loadgrid(AR_CHITIET);
            } else {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_khongconmathangdethem') }, "warning");
                commonCloseLoadingText("#btn_themtatcahang");
            }
        }
    }
    $scope.deleterow = function (e) {
        let myGrid = $('#grid').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;

        let myCombo = $("#mathang").data("kendoComboBox");
        let item = {
            idMatHang: dataItem.iD_HangHoa,
            maHang: dataItem.maHang,
            tenHang: dataItem.tenHang,
            tenDonVi: dataItem.tenDonVi
        }
        myCombo.dataSource.add(item);
        AR_MATHANG.push(item);

        myGrid.dataSource.remove(dataItem);
        AR_CHITIET = AR_CHITIET.filter(x => (x.iD_HangHoa != dataItem.iD_HangHoa));
    }

    $scope.luuphieutondau = function () {
        if (validate()) {
            commonOpenLoadingText("#luuphieutondau");
            khoHangDataService.themphieutondau($scope.object).then(function (result) {
                if (result.flag) {
                    $scope.object = { PhieuTonDau: {}, ChiTietMatHangTonDau: [] };
                    Notification({ title: $.i18n('label_thongbao'), message: result.message }, 'success');
                }
                else
                    Notification({ title: $.i18n('label_thongbao'), message: result.message }, 'warning');

                commonCloseLoadingText("#luuphieutondau")
            })
        }
    }
    $scope.taifilemau = function () {
        commonOpenLoadingText("#btn_taifilemau");

        khoHangDataService.taifilemauphieutondau().then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayravuilongloadlaitrang') }, 'warning');

            commonCloseLoadingText("#btn_taifilemau")
        });
    }
    $scope.importexcel = function () {
        $scope.formimport.center().open();
    }
    $scope.capNhatTuExcel = function () {
        commonOpenLoadingText("#btn_capnhatimport");

        khoHangDataService.importphieutondau(fileUpload).then(function (result) {
            if (result.flag)
                if (result.data.status == 200) {
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_tacvuthuchienthanhcong') }, 'success');
                    $scope.formimport.center().close();
                    $scope.khohangOnChange();
                }
                else if (result.data.status == 201)
                    commonDownFile(result.data);
                else if (result.data.status == 204)
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_filemaukhongdungdinhdanghoackhongtontaibanghi') }, 'warning');
                else if (result.data.status == 411)
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_soluongbanghikhongthevuotqua2000dong') }, 'warning');
                else
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloisayravuilongkiemtralaifilecapnhat') }, 'warning');
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayravuilongloadlaitrang') }, 'warning');

            commonCloseLoadingText("#btn_capnhatimport")
        });
    }
    $scope.huyCapNhatExcel = function () {
        $scope.formimport.center().close();
    }

    init();

})