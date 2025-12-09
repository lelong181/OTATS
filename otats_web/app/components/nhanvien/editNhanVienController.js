angular
    .module("app")
    .controller(
        "editNhanVienController",
        function (
            $scope,
            $location,
            $state,
            $stateParams,
            $timeout,
            Notification,
            ComboboxDataService,
            nhanVienDataService
        ) {
            CreateSiteMap();

            let param_idnhom = 0;
            let idnhanvien = 0;
            let arr_idkhachhang = [];
            let arr = [];

            $scope.objectNhanVien = {};

            function init() {
                param_idnhom =
                    $stateParams.idnhom == undefined ? 0 : $stateParams.idnhom;
                idnhanvien = $stateParams.idnhanvien;
                $scope.idnhanvien = idnhanvien;
                $scope.showedit = idnhanvien > 0;
                getquyen();

                loadchitietbyid();

                loadgrid();
            }
            // =======m16/03/2023=======================//
            $scope.loaikhachhangOnChange = function () {
                $scope.loaikhachhangselect = this.loaikhachhangselect;
            };
            // ===================================//
            function getquyen() {
                let url = "nhanvien";
                ComboboxDataService.getquyen(url).then(function (result) {
                    $scope.permission = result.data;
                    if (
                        $scope.permission.iD_ChucNang <= 0 ||
                        ($scope.permission.them <= 0 && idnhanvien <= 0)
                    ) {
                        Notification(
                            {
                                title: $.i18n("label_thongbao"),
                                message: $.i18n(
                                    "label_khongcoquyentruycapchucnang"
                                ),
                            },
                            "error"
                        );
                        $location.path("/nhanvien");
                    }
                });
            }

            function onUploadImageSuccess(e) {
                let data = new FormData();
                data.append("file", e.files[0].rawFile);
                let files = e.files[0];
                if (
                    files.extension.toLowerCase() != ".jpg" &&
                    files.extension.toLowerCase() != ".png" &&
                    files.extension.toLowerCase() != ".jpeg"
                ) {
                    e.preventDefault();
                    Notification(
                        {
                            title: $.i18n("label_thongbao"),
                            message: $.i18n(
                                "label_vuilongchonfileanhjpgpngjpeg"
                            ),
                        },
                        "warning"
                    );
                } else {
                    nhanVienDataService
                        .uploadAnhDaiDien(data)
                        .then(function (result) {
                            $("#previewnhanvien").html(
                                '<div class="imgprevew"><img src="' +
                                urlApi +
                                result.url +
                                '" style="width:154px;height:179px;max-height:179px;" /></div>'
                            );
                            $scope.objectNhanVien.anhDaiDien = result.url;
                            $scope.objectNhanVien.image_url = result.url;
                            if (!result.flag)
                                Notification(
                                    {
                                        title: $.i18n("label_thongbao"),
                                        message: $.i18n($.i18n(result.message)),
                                    },
                                    "warning"
                                );
                        });
                }
            }
            function deleteImage(imageName) {
                $(this).parent().remove();
            }

            function initcombobox() {
                // mission add new customer 16/03/2023
                $scope.loaikhachhangOptions = {
                    filter: "contains",
                    suggest: true,
                    valuePrimitive: true
                };
                let data = [
                    { id: 0, name: $.i18n("label_nam") },
                    { id: 1, name: $.i18n("label_nu") },
                ];
                $scope.gioiTinhData = data;

                ComboboxDataService.getDataTreeNhomNhanVien().then(function (
                    result
                ) {
                    $scope.nhomnhanvienOptions = {
                        placeholder: $.i18n("label_chonnhomnhanvien"),
                        dataTextField: "tenNhom",
                        dataValueField: "iD_Nhom",
                        valuePrimitive: true,
                        dataSource: new kendo.data.HierarchicalDataSource({
                            data: result.data,
                            schema: {
                                model: {
                                    children: "childs",
                                },
                            },
                        }),
                    };

                    $timeout(function () {
                        $("#files").kendoUpload({
                            multiple: false,
                            select: onUploadImageSuccess,
                            validation: {
                                allowedExtensions: [".jpg", ".jpeg", ".png"],
                            },
                            showFileList: false,
                        });
                        $("#files")
                            .closest(".k-upload")
                            .find("span")
                            .text($.i18n("label_chonanhdaidien"));

                        $("#gioitinh")
                            .data("kendoComboBox")
                            .value($scope.objectNhanVien.gioiTinh);
                    }, 10);
                });
                ComboboxDataService.getLoaiKhachHang().then(function (result) {
                    $scope.loaikhachhangData = result.data;


                    if ($scope.objectNhanVien.iD_NhomKhachHang_MacDinh > 0) {
                        $timeout(function () {
                            $("#loaikhachhang")
                                .data("kendoComboBox")
                                .value(
                                    $scope.objectNhanVien
                                        .iD_NhomKhachHang_MacDinh
                                );
                        }, 100);
                    } else {
                        $("#loaikhachhang").data("kendoComboBox").value("");
                    }
                });

                nhanVienDataService.gethinhthucthanhtoan().then(function (result) {
                    $scope.hinhthucthanhtoanData = result.data;  
                    if (idnhanvien > 0) {
                        nhanVienDataService.getHinhThucThanhToanByNV(idnhanvien).then(function (result) {
                            $scope.hinhthucthanhtoanSelected = result.data;
                        });
                    }  
                });                        
            }
            

            function loadchitietbyid() {
                let obj = {
                    idnv: 0,
                    tenDangNhap: "",
                    tenDayDu: "",
                    matKhau: "",
                    diaChi: "",
                    queQuan: "",
                    ngaySinh: "1900-01-01T00:00:00",
                    email: "",
                    dienThoai: "",
                    phienBan: "2.0.1.9",
                    iD_Nhom: param_idnhom <= 0 ? 0 : param_idnhom,
                    tenNhom: "",
                    truongNhom: 0,
                    dongMay: "",
                    doiMay: "",
                    tenMay: "",
                    imei: "",
                    osVersion: "",
                    os: "",
                    isFakeGPS: false,
                    isCheDoTietKiemPin: false,
                    anhDaiDien: "",
                    anhDaiDien_thumbnail_medium: "",
                    anhDaiDien_thumbnail_small: "",
                    appFakeGPS: "",
                    gioiTinh: 0,
                    iD_ChucVu: 0,
                    chucVu: "",
                    image_url: "",
                };
                if (idnhanvien > 0) {
                    nhanVienDataService
                        .getById(idnhanvien)
                        .then(function (result) {
                            if (result.flag)
                                $scope.objectNhanVien = result.data;
                            else $scope.objectNhanVien = obj;

                            $scope.objectNhanVien.matKhau = "";
                            if ($scope.objectNhanVien.anhDaiDien != "") {
                                $("#previewnhanvien").html(
                                    '<div class="imgprevew"><img src="' +
                                    SERVERIMAGE +
                                    $scope.objectNhanVien.anhDaiDien +
                                    '" style="width:154px;height:179px;max-height:179px;" /></div>'
                                );
                            }
                            initcombobox();
                            formatentity();
                        });
                } else {
                    $scope.objectNhanVien = obj;

                    initcombobox();
                    formatentity();
                }
            }
            function formatentity() {
                if (
                    $scope.objectNhanVien.ngaySinh != null &&
                    $scope.objectNhanVien.ngaySinh != ""
                ) {
                    let datestring = $scope.objectNhanVien.ngaySinh;
                    let dateobj = new Date(datestring);
                    let year = dateobj.getFullYear();

                    if (year > 1900) {
                        $scope.dateObject = dateobj;
                        $scope.dateString = kendo.toString(
                            dateobj,
                            "dd/MM/yyyy"
                        );
                    }
                }

                $scope.truongNhom = $scope.objectNhanVien.truongNhom == 1;
                $scope.isCheDoTietKiemPin = $scope.objectNhanVien
                    .isCheDoTietKiemPin
                    ? $.i18n("label_co")
                    : $.i18n("label_khong");
                $scope.isFakeGPS = $scope.objectNhanVien.isFakeGPS
                    ? $.i18n("label_on")
                    : $.i18n("label_khong");

                if ($scope.objectNhanVien.iD_Nhom > 0)
                    $scope.nhomnhanvienSelected = $scope.objectNhanVien.iD_Nhom;
                else $scope.nhomnhanvienSelected = undefined;

                if ($scope.objectNhanVien.iD_NhomKhachHang_MacDinh > 0)
                    $scope.loaikhachhangselect =
                        $scope.objectNhanVien.iD_NhomKhachHang_MacDinh;
                else $scope.loaikhachhangselect = undefined;
            }

            function listColumnsgrid() {
                var dataList = [];

                dataList.push({
                    headerAttributes: {
                        class: "table-header-cell",
                        style: "text-align: center",
                    },
                    selectable: true,
                    width: "40px",
                });
                dataList.push({
                    title: "#",
                    template: "#= ++RecordNumber #",
                    width: "50px",
                    headerAttributes: {
                        class: "table-header-cell",
                        style: "text-align: center",
                    },
                    attributes: { class: "text-center" },
                });
                dataList.push({
                    field: "tenKhachHang",
                    title: $.i18n("header_tenkhachhang"),
                    headerAttributes: {
                        class: "table-header-cell",
                        style: "text-align: center",
                    },
                    filterable: defaultFilterableGrid,
                    width: "150px",
                });
                dataList.push({
                    field: "tenTinh",
                    title: $.i18n("header_tinhthanh"),
                    headerAttributes: {
                        class: "table-header-cell",
                        style: "text-align: center",
                    },
                    filterable: defaultFilterableGrid,
                    width: "150px",
                });
                dataList.push({
                    field: "tenQuan",
                    title: $.i18n("header_quanhuyen"),
                    headerAttributes: {
                        class: "table-header-cell",
                        style: "text-align: center",
                    },
                    filterable: defaultFilterableGrid,
                    width: "150px",
                });
                dataList.push({
                    field: "tenPhuong",
                    title: $.i18n("header_phuongxa"),
                    headerAttributes: {
                        class: "table-header-cell",
                        style: "text-align: center",
                    },
                    filterable: defaultFilterableGrid,
                    width: "150px",
                });
                dataList.push({
                    field: "diaChi",
                    title: $.i18n("header_diachi"),
                    headerAttributes: {
                        class: "table-header-cell",
                        style: "text-align: center",
                    },
                    filterable: defaultFilterableGrid,
                    width: "400px",
                });

                return dataList;
            }
            function loadgrid() {
                commonOpenLoadingText("#btn_chonkhachhang");
                kendo.ui.progress($("#grid"), true);
                $scope.gridOptions = {
                    sortable: true,
                    height: function () {
                        var heightGrid =
                            $(window).height() -
                            ($(".navbar").height() +
                                $(".sitemap").height() +
                                $(".toolbarmenu").height());
                        return heightGrid - 150;
                    },
                    dataBinding: function () {
                        RecordNumber =
                            (this.dataSource.page() - 1) *
                            this.dataSource.pageSize();
                    },
                    resizable: true,
                    editable: false,
                    filterable: {
                        mode: "row",
                    },
                    pageable:
                        $scope.lang == "vi-vn"
                            ? pageableShort_vi
                            : pageableShort_en,
                    columns: listColumnsgrid(),
                };

                nhanVienDataService
                    .getlistkhachhangcapquyen(idnhanvien)
                    .then(function (result) {
                        arr = result.data;
                        arr_idkhachhang = [];
                        arr_idkhachhang = arr.map((ar, index, arr) => {
                            return ar.iD_KhachHang;
                        });

                        $scope.gridData = {
                            data: arr,
                            schema: {
                                model: {
                                    id: "iD_KhachHang",
                                    fields: {
                                        iD_KhachHang: {
                                            type: "number",
                                        },
                                    },
                                },
                            },
                            pageSize: 20,
                        };
                        kendo.ui.progress($("#grid"), false);
                        commonCloseLoadingText("#btn_chonkhachhang");
                    });
            }
            function reloadgrid() {
                $scope.gridData = {
                    data: arr,
                    schema: {
                        model: {
                            id: "iD_KhachHang",
                            fields: {
                                iD_KhachHang: {
                                    type: "number",
                                },
                            },
                        },
                    },
                    pageSize: 20,
                };

                $("#grid").data("kendoGrid").refresh();
            }

            function listColumnsgridChonKhachHang() {
                var dataList = [];

                dataList.push({
                    headerAttributes: {
                        class: "table-header-cell",
                        style: "text-align: center",
                    },
                    selectable: true,
                    width: "40px",
                });
                dataList.push({
                    title: "#",
                    template: "#= ++RecordNumber #",
                    width: "50px",
                    headerAttributes: {
                        class: "table-header-cell",
                        style: "text-align: center",
                    },
                    attributes: { class: "text-center" },
                });
                dataList.push({
                    field: "ten",
                    title: $.i18n("header_tenkhachhang"),
                    headerAttributes: {
                        class: "table-header-cell",
                        style: "text-align: center",
                    },
                    filterable: defaultFilterableGrid,
                    width: "150px",
                });
                dataList.push({
                    field: "tenTinh",
                    title: $.i18n("header_tinhthanh"),
                    headerAttributes: {
                        class: "table-header-cell",
                        style: "text-align: center",
                    },
                    filterable: defaultFilterableGrid,
                    width: "150px",
                });
                dataList.push({
                    field: "tenQuan",
                    title: $.i18n("header_quanhuyen"),
                    headerAttributes: {
                        class: "table-header-cell",
                        style: "text-align: center",
                    },
                    filterable: defaultFilterableGrid,
                    width: "150px",
                });
                dataList.push({
                    field: "tenPhuong",
                    title: $.i18n("header_phuongxa"),
                    headerAttributes: {
                        class: "table-header-cell",
                        style: "text-align: center",
                    },
                    filterable: defaultFilterableGrid,
                    width: "150px",
                });
                dataList.push({
                    field: "diaChi",
                    title: $.i18n("header_diachi"),
                    headerAttributes: {
                        class: "table-header-cell",
                        style: "text-align: center",
                    },
                    filterable: defaultFilterableGrid,
                    width: "400px",
                });

                return dataList;
            }
            function loadgridChonKhachHang() {
                commonOpenLoadingText("#btn_capnhatchonkhachhang");
                kendo.ui.progress($("#gridChonKhachHang"), true);

                $scope.gridChonKhachHangOptions = {
                    sortable: true,
                    height: function () {
                        return 430;
                    },
                    dataBinding: function () {
                        RecordNumber =
                            (this.dataSource.page() - 1) *
                            this.dataSource.pageSize();
                    },
                    resizable: true,
                    editable: false,
                    filterable: {
                        mode: "row",
                    },
                    pageable:
                        $scope.lang == "vi-vn"
                            ? pageableShort_vi
                            : pageableShort_en,
                    columns: listColumnsgridChonKhachHang(),
                };

                nhanVienDataService
                    .getlistkhachhangchon()
                    .then(function (result) {
                        let arr = result.data;
                        let small_arr = arr.filter((item) => {
                            return (
                                arr_idkhachhang.indexOf(item.iD_KhachHang) ===
                                -1 &&
                                (item.iD_LoaiKhachHang ==
                                    $scope.loaikhachhangselect ||
                                    $scope.loaikhachhangselect == null)
                            );
                        });

                        $scope.gridChonKhachHangData = {
                            data: small_arr,
                            schema: {
                                model: {
                                    id: "iD_KhachHang",
                                    fields: {
                                        iD_KhachHang: {
                                            type: "number",
                                        },
                                    },
                                },
                            },
                            pageSize: 20,
                        };

                        kendo.ui.progress($("#gridChonKhachHang"), false);
                        commonCloseLoadingText("#btn_capnhatchonkhachhang");
                    });
            }

            function validatethemsua() {
                let flag = true;
                let msg = "";
                let regexTenDangNhap =
                    /^[^\%\/\\\&;\?\,\'\" "\;\:\!\-\@\#\^\;*\[\]\(\)\=\+\|\{\}\$\À\Á\Â\Ã\È\É\Ê\Ì\Í\Ò\Ó\Ô\Õ\Ù\Ú\Ă\Đ\Ĩ\Ũ\Ơ\à\á\â\ã\è\é\ê\ì\í\ò\ó\ô\õ\ù\ú\ă\đ\ĩ\ũ\ơ\Ư\Ă\Ạ\Ả\Ấ\Ầ\Ẩ\Ẫ\Ậ\Ắ\Ằ\Ẳ\Ẵ\Ặ\Ẹ\Ẻ\Ẽ\Ề\Ề\Ể\ư\ă\ạ\ả\ấ\ầ\ẩ\ẫ\ậ\ắ\ằ\ẳ\ẵ\ặ\ẹ\ẻ\ẽ\ề\ề\ể\Ễ\Ệ\Ỉ\Ị\Ọ\Ỏ\Ố\Ồ\Ổ\Ỗ\Ộ\Ớ\Ờ\Ở\Ỡ\Ợ\Ụ\Ủ\Ứ\Ừ\ễ\ệ\ỉ\ị\ọ\ỏ\ố\ồ\ổ\ỗ\ộ\ớ\ờ\ở\ỡ\ợ\ụ\ủ\ứ\ừ\Ử\Ữ\Ự\Ỳ\Ỵ\Ý\Ỷ\Ỹ\ử\ữ\ự\ỳ\ỵ\ỷ\ỹ]+$/;
                let regexTendaydu =
                    /^[^\%\/\\\&;\?\,\;\:\!\-\@\#\^\;*\[\]\(\)\=\+\|\{\}\$]+$/;
                let regexEmail =
                    /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

                if (
                    $scope.objectNhanVien.tenDangNhap == "" ||
                    $scope.objectNhanVien.tenDangNhap == undefined
                ) {
                    flag = false;
                    msg = $.i18n("label_tendangnhapkhongduocdetrong");
                    $("#tenDangNhap").focus();
                }

                if (
                    flag &&
                    ($scope.objectNhanVien.tenDayDu == "" ||
                        $scope.objectNhanVien.tenDayDu == undefined)
                ) {
                    flag = false;
                    msg = $.i18n("label_tendaydukhongduocdetrong");
                    $("#tenDayDu").focus();
                }

                if (
                    flag &&
                    idnhanvien <= 0 &&
                    ($scope.objectNhanVien.matKhau == "" ||
                        $scope.objectNhanVien.matKhau == undefined)
                ) {
                    flag = false;
                    msg = $.i18n("label_matkhaukhongduocdetrong");
                    $("#matKhau").focus();
                }

                if (
                    flag &&
                    idnhanvien <= 0 &&
                    $scope.objectNhanVien.matKhau.indexOf(" ") >= 0
                ) {
                    flag = false;
                    msg = $.i18n("label_matkhaukhongduocdekhoangtrang");
                    $("#matKhau").focus();
                }

                if (
                    flag &&
                    idnhanvien <= 0 &&
                    ($scope.objectNhanVien.xacNhanMatKhau == "" ||
                        $scope.objectNhanVien.xacNhanMatKhau == undefined)
                ) {
                    flag = false;
                    msg = $.i18n("label_xacnhanmatkhaukhongduocdetrong");
                    $("#xacNhanMatKhau").focus();
                }

                if (
                    flag &&
                    idnhanvien <= 0 &&
                    $scope.objectNhanVien.matKhau.length < 8
                ) {
                    flag = false;
                    msg = $.i18n("label_matkhaucododaiitnhat8kytu");
                    $("#matKhau").focus();
                }

                if (
                    flag &&
                    idnhanvien <= 0 &&
                    $scope.objectNhanVien.xacNhanMatKhau !=
                    $scope.objectNhanVien.matKhau
                ) {
                    flag = false;
                    msg = $.i18n("label_xacnhanmatkhaukhongkhop");
                    $scope.objectNhanVien.xacNhanMatKhau = "";
                    $("#xacNhanMatKhau").focus();
                }

                $scope.objectNhanVien.iD_Nhom = $scope.nhomnhanvienSelected;
                if (
                    flag &&
                    ($scope.nhomnhanvienSelected == undefined ||
                        $scope.objectNhanVien.iD_Nhom <= 0)
                ) {
                    flag = false;
                    msg = $.i18n("label_nhomnhanvienkhongduocdetrong");
                    $("#nhomnhanvien").focus();
                }

                if (
                    flag &&
                    !regexTenDangNhap.test($scope.objectNhanVien.tenDangNhap)
                ) {
                    flag = false;
                    msg = $.i18n("label_tendangnhapkhongduocchuadaucach");
                    $("#tenDangNhap").focus();
                }

                if (
                    flag &&
                    !regexTendaydu.test($scope.objectNhanVien.tenDayDu)
                ) {
                    flag = false;
                    msg = $.i18n("label_tendaydukhongdcchuakytudacbiet");
                    $("#tenDayDu").focus();
                }

                if (
                    flag &&
                    $scope.objectNhanVien.email != "" &&
                    !regexEmail.test($scope.objectNhanVien.email)
                ) {
                    flag = false;
                    msg = $.i18n("label_emailkhongdungdinhdang");
                    $("#email").focus();
                }

                if (!flag)
                    Notification(
                        {
                            title: $.i18n("label_thongbao"),
                            message: $.i18n(msg),
                        },
                        "warning"
                    );

                return flag;
            }

            //event
            $scope.nhomnhanvienOnChange = function () {
                $scope.nhomnhanvienSelected = this.nhomnhanvienSelected;
            };
            $scope.hinhthucthanhtoanOnChange = function () {
                $scope.hinhthucthanhtoanSelected = this.hinhthucthanhtoanSelected;
            };
            $scope.gioiTinhOnChange = function () {
                $scope.gioiTinh = this.gioiTinh;
            };

            $scope.createpassword = function () {
                commonOpenLoadingText("#btn_createpass");

                let possible = "0123456789";
                let text = "";
                for (i = 0; i < 4; i++) {
                    text += possible.charAt(
                        Math.floor(Math.random() * possible.length)
                    );
                }

                $timeout(function () {
                    commonCloseLoadingText("#btn_createpass");

                    $scope.objectNhanVien.matKhau = text;
                    $scope.objectNhanVien.xacNhanMatKhau = text;
                    $scope.createpass = text;
                    u;
                }, 200);
            };

            $scope.luuNhanVien = function () {
                if (validatethemsua()) {
                    $scope.objectNhanVien.idnv = idnhanvien;
                    $scope.objectNhanVien.chkDoiMatKhau = 0;
                    $scope.objectNhanVien.truongNhom = $scope.truongNhom
                        ? 1
                        : 0;
                    $scope.objectNhanVien.iD_Nhom = $scope.nhomnhanvienSelected;
                    //m16/03/2023
                    $scope.objectNhanVien.iD_NhomKhachHang_MacDinh =
                        $scope.loaikhachhangselect;
                    if ($scope.gioiTinh != undefined)
                        $scope.objectNhanVien.gioiTinh =
                            $scope.gioiTinh.value < 0
                                ? 0
                                : $scope.gioiTinh.value;

                    if ($scope.dateObject != undefined)
                        $scope.objectNhanVien.ngaySinh = kendo.toString(
                            $scope.dateObject,
                            "dd/MM/yyyy"
                        );
                    else $scope.objectNhanVien.ngaySinh = "01/01/1900";

                    var grid = $("#grid").data("kendoGrid");
                    var idsKhachhang = [];
                    $.each(
                        grid.dataSource.options.data,
                        function (index, value) {
                            idsKhachhang.push(value.iD_KhachHang);
                        }
                    );

                    $scope.objectNhanVien.listIdKH = idsKhachhang;

                    var httts = [];
                    if ($scope.hinhthucthanhtoanSelected != undefined) {
                        $.each($scope.hinhthucthanhtoanSelected, function (index, item) {
                            httts.push(item.id)
                        })
                    }
                    httts.join(",");
                    nhanVienDataService.luuNV_HinhThucTT(idnhanvien, httts).then(function (result) {
                            if (result.flag) {
                                Notification(
                                    {
                                        title: $.i18n("label_thongbao"),
                                        message: $.i18n(result.message),
                                    },
                                    "success"
                                );
                            } else Notification({ title: $.i18n("label_thongbao"), message: $.i18n(result.message) }, "warning");
                    });

                    if (idnhanvien > 0) {
                        nhanVienDataService
                            .saveedit($scope.objectNhanVien)
                            .then(function (result) {
                                if (result.flag) {
                                    $state.go("nhanvien");
                                    Notification(
                                        {
                                            title: $.i18n("label_thongbao"),
                                            message: $.i18n(result.message),
                                        },
                                        "success"
                                    );
                                } else Notification({ title: $.i18n("label_thongbao"), message: $.i18n(result.message) }, "warning");
                            });
                       
                    } else {
                        nhanVienDataService
                            .saveinsert($scope.objectNhanVien)
                            .then(function (result) {
                                if (result.flag) {
                                    $state.go("nhanvien");
                                    Notification(
                                        {
                                            title: $.i18n("label_thongbao"),
                                            message: $.i18n(result.message),
                                        },
                                        "success"
                                    );
                                } else Notification({ title: $.i18n("label_thongbao"), message: $.i18n(result.message) }, "warning");
                            });
                    }
                                      
                }
            };
            $scope.huyLuuNhanVien = function () {
                $state.go("nhanvien");
            };

            $scope.chonKhachHang = function () {
                $scope.formchonkhachhang.center().open();
                loadgridChonKhachHang();
            };
            $scope.xoaKhachHang = function () {
                let arr = $("#grid").data("kendoGrid").selectedKeyNames();
                if (arr.length <= 0)
                    Notification(
                        {
                            title: $.i18n("label_thongbao"),
                            message: $.i18n("label_chuachonkhachhang"),
                        },
                        "warning"
                    );
                else {
                    let data = [];
                    for (let i = 0; i < arr.length; i++) {
                        data.push(parseInt(arr[i]));
                    }
                    openConfirm(
                        $.i18n("label_bancochacchanmuonxoakhachhangkhong"),
                        "apDungXoaKhachHang",
                        null,
                        data
                    );
                }
            };
            $scope.apDungXoaKhachHang = function (data) {
                if (idnhanvien > 0) {
                    nhanVienDataService
                        .removephanquyen(idnhanvien, data)
                        .then(function (result) {
                            if (result.flag) {
                                Notification(
                                    {
                                        title: $.i18n("label_thongbao"),
                                        message: $.i18n(result.message),
                                    },
                                    "success"
                                );
                                loadgrid();
                            } else Notification({ title: $.i18n("label_thongbao"), message: $.i18n(result.message) }, "warning");
                        });
                } else {
                    let grid = $("#grid").data("kendoGrid");
                    grid.select().each(function () {
                        let row = $(this).closest("tr");
                        let dataItem = grid.dataItem(row);

                        arr = arr.filter(
                            (item) => item.iD_KhachHang != dataItem.iD_KhachHang
                        );
                        arr_idkhachhang = arr_idkhachhang.filter(
                            (item) => item != dataItem.iD_KhachHang
                        );
                    });

                    reloadgrid();
                }
            };

            $scope.capNhatChonKhachHang = function () {
                commonOpenLoadingText("#btn_capnhatchonkhachhang");
                let listadd = [];

                let gridchoose = $("#gridChonKhachHang").data("kendoGrid");
                let idschoose = [];
                gridchoose.select().each(function () {
                    let dataItem = gridchoose.dataItem(this);

                    if (idschoose.indexOf(dataItem) == -1) {
                        idschoose.push(dataItem);
                    }
                });

                idschoose.forEach((i) => {
                    listadd.push(i["iD_KhachHang"]);

                    i.tenKhachHang = i.ten;
                    arr.push(i);

                    arr_idkhachhang.push(i["iD_KhachHang"]);
                });

                if (idnhanvien > 0) {
                    nhanVienDataService
                        .addphanquyen(idnhanvien, listadd)
                        .then(function (result) {
                            if (result.flag)
                                Notification(
                                    {
                                        title: $.i18n("label_thongbao"),
                                        message: $.i18n(result.message),
                                    },
                                    "success"
                                );
                            else
                                Notification(
                                    {
                                        title: $.i18n("label_thongbao"),
                                        message: $.i18n(result.message),
                                    },
                                    "warning"
                                );

                            commonCloseLoadingText("#btn_capnhatchonkhachhang");
                        });
                }

                reloadgrid();

                $scope.formchonkhachhang.center().close();
            };
            $scope.huyChonKhachHang = function () {
                $scope.formchonkhachhang.center().close();
            };

            init();
        }
    );