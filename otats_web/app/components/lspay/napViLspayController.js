angular.module('app').controller('napViLspayController', function ($state, $rootScope, $scope, $location, Notification, ComboboxDataService, lspayDataService, donHangDataService) {
    CreateSiteMap();

    function init() {
        initdate();
        initcombo();
        getquyen();
        if (!$("#files").data("kendoUpload")) {
            $("#files").kendoUpload({
                multiple: false,
                select: onUploadImageSuccess,
                validation: {
                    allowedExtensions: [".jpg", ".jpeg", ".png"]
                },
                showFileList: false
            });
        }
        $("#files").closest(".k-upload").find("span").html('<i class="fas fa-image" style="margin-right:3px;"> </i>' + $.i18n("label_chonanhchuyenkhoan"));
        $("#preview").kendoTooltip({
            filter: "img",
            position: "right",
            content: function (e) {
                var target = e.target;
                return "<img style='width:280px;' src='" + target[0].currentSrc + "' />"
            }
        }).data("kendoTooltip");
        clearNapVi();
        $("#btncancelpayment").hide();
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
        $scope.obj_TuNgay = new Date(dateNow.setHours(0, 0, 0));
        $scope.obj_DenNgay = new Date(dateNow.setHours(23, 59, 59));
        $scope.maxDate = $scope.obj_DenNgay;
        $scope.minDate = $scope.obj_TuNgay;
    }
    function initcombo() {
        ComboboxDataService.getDataNhomNhanVien().then(function (result) {
            $scope.nhomnhanvienData = result.data;
            if ($scope.nhomnhanvienData.length == 1) {
                $scope.nhomnhanvienselect = $scope.nhomnhanvienData[0]
            }
            var idnhom = 0;
            if ($scope.nhomnhanvienselect != undefined)
                idnhom = ($scope.nhomnhanvienselect.iD_Nhom < 0) ? 0 : $scope.nhomnhanvienselect.iD_Nhom;
            lspayDataService.getTongSoDu(idnhom).then(function (result) {
                $scope.sodukhadung = result.data;
            });
            lspayDataService.getSoDuDauKy(idnhom, kendo.toString($scope.obj_TuNgay, 'yyyy-MM-dd')).then(function (result) {
                $scope.sodudauky = result.data;
            });
            loadgrid(idnhom);
            loadgridlichsunap(idnhom);
        });
    }
    function listColumnsgrid() {
        var dataList = [];

        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" }
        });
        dataList.push({
            field: "tenNhanVien", title: $.i18n('header_tennhanvien'), attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px"
        });
        dataList.push({
            field: "ngayTao", format: "{0:HH:mm:ss dd/MM/yyyy}", title: $.i18n('header_thoigian'), attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "loaiBienDong", title: $.i18n('header_loaibiendong'), attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "soTien", format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            template: function (e) {
                if (e.soTien > 0) {
                    return "";
                } else {
                    return "<b style='color:red'>" + kendo.toString(e.soTien, 'N0') + "</b>"
                }
            },
            title: "Ghi nợ (giảm)", attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px"
        });
        dataList.push({
            field: "soTien", format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            template: function (e) {
                if (e.soTien > 0) {
                    return "<b style='color:green'>" + kendo.toString(e.soTien, 'N0') + "</b>"
                } else {
                    return "";
                }
            },
            title: "Ghi có (tăng)", attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px"
        });
        dataList.push({
            field: "ghiChu", title: $.i18n('header_ghichu'), attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid
        });

        return dataList;
    }

    function listColumnsgridLichSuNap() {
        var dataList = [];

        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" }
        });
        dataList.push({
            field: "tacvu", title: $.i18n('header_tacvu'),
            template: function (e) {
                if (e.trangThai == 0 && $scope.permission.sua == 1) {
                    return '<button ng-click="duyetNapVi(' + e.id + ')" class="k-button" title ="Xác nhận" ><i class="fas fa-check fas-sm color-infor mr-1"></i> Xác nhận </button> ';
                }
                else if (e.trangThai == 1) {
                    return '<i class="fas fa-check fas-sm color-success"></i>';
                } else {
                    return '<i class="fas fa-times fas-sm color-danger"></i>';
                }
            },
            width: "70px",
            filterable: false,
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({
            field: "ngayTao", format: "{0:HH:mm:ss dd/MM/yyyy}", title: $.i18n('header_thoigian'), attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "soTien", format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            title: "Số tiền", attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px"
        });
        dataList.push({
            field: "imgUrl", title: "Ảnh",
            template: function (dataItem) {
                if (dataItem.imgUrl == null || dataItem.imgUrl == '')
                    return ''
                else {
                    return '<img src="' + dataItem.imgUrl + '" alt="" style="width: 100px;} ">';
                }
            },
            attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: false, width: "130px"
        });

        return dataList;
    }
    function loadgrid(idnhom) {
        kendo.ui.progress($("#grid"), true);
        commonOpenLoadingText("#btn_bm034xembaocao");
        $scope.gridOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - 330;
                return heightGrid < 100 ? 500 : heightGrid;
            },
            excelExport: function (e) {
                excelExport(e);
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
        lspayDataService.getAllBienDong(idnhom, fromdate, todate).then(function (result) {
            $scope.gridData = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                            ngayTao: {
                                type: "date"
                            },
                            soTien: {
                                type: "number"
                            },
                        }
                    }
                },
                pageSize: 20,
                aggregate: [
                    { field: "soTien", aggregate: "sum" }
                ]

            };
            $scope.sonaptrongky = 0;
            $scope.sotieutrongky = 0;
            $.each(result.data, function (index, item) {
                if (item.soTien > 0) {
                    $scope.sonaptrongky += item.soTien;
                } else {
                    $scope.sotieutrongky += item.soTien;
                }
            })
            kendo.ui.progress($("#grid"), false);
            commonCloseLoadingText("#btn_bm034xembaocao")
        });
    }

    function loadgridlichsunap(idnhom) {
        kendo.ui.progress($("#gridLichSuNap"), true);
        $scope.gridLichSuNapOptions = {
            sortable: true,
            height: 580,
            excelExport: function (e) {
                excelExport(e);
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
            columns: listColumnsgridLichSuNap(),
        };

        lspayDataService.getLichSuNap(idnhom).then(function (result) {
            $scope.gridLichSuNapData = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                            ngayTao: {
                                type: "date"
                            },
                            soTien: {
                                type: "number"
                            },
                        }
                    }
                },
                pageSize: 20

            };
            kendo.ui.progress($("#gridLichSuNap"), false);
            $("#gridLichSuNap").kendoTooltip({
                filter: "img",
                position: "left",
                content: function (e) {
                    var target = e.target;
                    return "<img style='width:280px;' src='" + target[0].currentSrc + "' />"
                }
            }).data("kendoTooltip");
        });


    }


    function clearNapVi() {
        $scope.napvi = {};
        $("#preview").html("");
        $scope.showXuLyNapVi = false;
        $scope.xuLyNapViModel = {};

    }

    function onUploadImageSuccess(e) {
        let data = new FormData();
        data.append('file', e.files[0].rawFile);
        var reader = new FileReader();
        reader.onload = (function () {
            var data = { base64String: reader.result };
            donHangDataService.checkAnhThanhToan(data).then(function (result) {
                console.log(result);
                let fulltext = result.data.data.responses[0].fullTextAnnotation.text;
                let arrs = [];
                console.log(fulltext);
                arrs = fulltext.split('\n');
                console.log(arrs);

                for (var i = 0; i <= 5; i++) {
                    if (arrs[i].indexOf("TECHCOMBANK") == 0) {
                        $scope.napvi.TenNganHang = "TECHCOMBANK"
                    } else if (arrs[i].indexOf("MB") == 0) {
                        $scope.napvi.TenNganHang = "MBBANK"
                    }
                    else if (arrs[i].indexOf("VCB") == 0) {
                        $scope.napvi.TenNganHang = "VIETCOMBANK"
                    }
                }
                for (var j = 0; j < arrs.length; j++) {
                    let idx = arrs[j].indexOf("VND");
                    if (idx == 0) {
                        $scope.napvi.SoTien = parseInt(arrs[j].substring(3, arrs[j].length).trim().replaceAll(",", "").replaceAll(".", ""));
                    } else if (idx > 0) {
                        $scope.napvi.SoTien = parseInt(arrs[j].substring(0, idx).trim().replaceAll(",", "").replaceAll(".", ""));
                    }

                }

                let idx2 = fulltext.indexOf("2204426666");
                if (idx2 >= 0) {
                    $scope.napvi.TaiKhoanNhan = "2204426666 - CT TNHH DT TM DV TRANG AN";
                } else {
                    $scope.napvi.TaiKhoanNhan = "Tài khoản nhận tiền không hợp lệ!"
                }
            });
        })
        reader.readAsDataURL(e.files[0].rawFile);
        let files = e.files[0];
        if (files.extension.toLowerCase() != ".jpg" && files.extension.toLowerCase() != ".png" && files.extension.toLowerCase() != ".jpeg") {
            e.preventDefault();
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_vuilongchonfileanhjpgpngjpeg') }, 'warning');
        } else {
            donHangDataService.uploadAnhThanhToan(data).then(function (result) {
                $("#preview").html('<div class="imgprevew"><img src="' + urlApi + result.url + '" style="width:20%" /></div>')
                $scope.napvi.ImgUrl = urlApi + result.url;
                $scope.xuLyNapViModel.ImgUrl = urlApi + result.url;
                if (!result.flag)
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
            })
        }
    }
    //event

    $scope.xemBaoCao = function () {

        var idnhanvien = 0;
        var idnhom = 0;
        if ($scope.nhomnhanvienselect != undefined)
            idnhom = ($scope.nhomnhanvienselect.iD_Nhom < 0) ? 0 : $scope.nhomnhanvienselect.iD_Nhom;
        lspayDataService.getTongSoDu(idnhom).then(function (result) {
            $scope.sodukhadung = result.data;
        });
        lspayDataService.getSoDuDauKy(idnhom, kendo.toString($scope.obj_TuNgay, 'yyyy-MM-dd')).then(function (result) {
            $scope.sodudauky = result.data;
        });
        loadgrid(idnhom);
    }
    //$scope.XuatExcel = function () {
    //    commonOpenLoadingText("#btn_bm034xuatexcel");
    //    var idnhanvien = 0;
    //    var idnhom = 0;
    //    let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
    //    let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
    //    if ($scope.nhanvienselect != undefined)
    //        idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
    //    if ($scope.nhomnhanvienselect != undefined)
    //        idnhom = ($scope.nhomnhanvienselect.iD_Nhom < 0) ? 0 : $scope.nhomnhanvienselect.iD_Nhom;
    //    baoCaoNhanVienDataService.getExcelBaoCaoLichSuMatTinHieu(idnhom, idnhanvien, fromdate, todate).then(function (result) {
    //        if (result.flag)
    //            commonDownFile(result.data);
    //        else
    //            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayravuilongloadlaitrang') }, 'warning');

    //        commonCloseLoadingText("#btn_bm023xuatexcel")
    //    });

    //}

    $scope.cancelPayment = function () {
        $("#qrthanhtoan").html('');
        $("#onepaythanhtoan").html('');
        clearNapVi();
        $("#btncancelpayment").hide();
        $scope.showXuLyNapVi = false;
        $scope.xuLyNapViModel = {};
    }

    $scope.createPayment = function () {
        if ($scope.SoTien > 0 && $scope.nhomnhanvienselect != undefined) {
            lspayDataService.getQRDynamic($scope.nhomnhanvienselect.iD_Nhom, $scope.SoTien).then(function (result) {
                console.log(result);
                let img = '<label>Nạp ví chuyển khoản trực tiếp</label><img style="width: 190px; margin: auto;" src="https://img.vietqr.io/image/970436-2204426666-compact2.png?amount='
                    + $scope.SoTien
                    + '&addInfo=lspay' + $scope.nhomnhanvienselect.iD_Nhom
                    + '"/>';
                $("#qrthanhtoan").html(img);
                //let onepay = '<label>Nạp ví qua Onepay</label><img style="width: 190px; margin: auto;" src="data:image/png;base64,' + result.data.invoice.qr.image + '"/>';
                //$("#onepaythanhtoan").html(onepay);
                $scope.showXuLyNapVi = true;
                $scope.xuLyNapViModel = {
                    //ID: result.data.invoice.description.split('_')[2],
                    ID: result.data.reference.split('_')[2],

                    SoTien: $scope.SoTien,
                    ImgUrl: ''
                };
            });

            //let onepay = '<label>Nạp ví qua Onepay</label><a target="_blank" href="' + SERVERIMAGE + 'Booking/ProcessPaymentLspay?ID_NhomTaiKhoan=' + $scope.nhomnhanvienselect.iD_Nhom + '&SoTien=' + $scope.SoTien + '"><img style="width: 160px;margin: auto;margin-top: 10px;" src="https://play-lh.googleusercontent.com/mvedVCbQg6ADKUYYraVLOlmOfOy2Rz66kEPvbmxt5xZ2TTa90Go9jBD2dJrwWmEo5g8"/></a>';
            //$("#onepaythanhtoan").html(onepay);
            clearNapVi();
            $("#btncancelpayment").show();
        } else {
            Notification({ title: $.i18n('label_thongbao'), message: "Vui lòng chọn ví và nhập số tiền cần nạp!" }, 'warning');
        }
    }

    $scope.xuLyNapVi = function () {
        console.log($scope.xuLyNapViModel);
        lspayDataService.xuLyNapVi($scope.xuLyNapViModel).then(function (result) {
            console.log(result);
            loadgridlichsunap($scope.nhomnhanvienselect.iD_Nhom);
        });
    }

    $scope.duyetNapVi = function (id) {
        lspayDataService.xacNhanNapVi(id).then(function (result) {
            console.log(result);
            loadgridlichsunap($scope.nhomnhanvienselect.iD_Nhom);
        });
    }

    $scope.nhomnhanvienOnChange = function () {
        $scope.nhomnhanvienselect = this.nhomnhanvienselect;
        idnhom = $scope.nhomnhanvienselect.iD_Nhom;
        loadgridlichsunap(idnhom);
        lspayDataService.getTongSoDu(idnhom).then(function (result) {
            $scope.sodukhadung = result.data;
        });
    };
    $scope.fromDateChanged = function () {
        $scope.minDate = $scope.obj_TuNgay;
    }
    $scope.toDateChanged = function () {
        $scope.maxDate = $scope.obj_DenNgay;
    }
    init();

})
