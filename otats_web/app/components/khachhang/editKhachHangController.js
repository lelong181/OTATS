angular.module('app').controller('editkhachHangController', function ($rootScope, $scope, $location, $state, $stateParams, $timeout, NgMap, Notification, khachHangDataService, ComboboxDataService, danhMucDataService) {
    CreateSiteMap();

    let idkhachhang = 0;
    let idkhuvuc = 0;
    let idtinh = 0;
    let idquan = 0;
    let idphuong = 0;
    let idloaikhachhang = 0;
    let idkenhbanhang = 0;
    let idkenhbanhangcaptren = 0;
    let url_anhdaidien = '';
    let image_url = '';
    let mapkhachhang;
    let marker;

    function init() {
        idkhachhang = $stateParams.idkhachhang;
        $scope.idkhachhang = idkhachhang;

        getquyen();

        initMap();
        getChiTietKhachHang();
    }

    function getquyen() {
        let url = 'khachhang'
        ComboboxDataService.getquyen(url).then(function (result) {
            $scope.permission = result.data;
            if ($scope.permission.iD_ChucNang <= 0 || ($scope.permission.them <= 0 && idkhachhang <= 0)) {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_khongcoquyentruycapchucnang')}, "error");
                $location.path('/khachhang')
            }
        });
    }

    function initMap() {
        NgMap.getMap().then(function (map) {
            console.log(map);
            mapkhachhang = map;

            marker = map.markers['marker-diadiem'];
        });
    }

    function initCombobox() {
        $scope.loaikhachhangOptions = {
            filter: "contains",
            suggest: true,
            footerTemplate: '<div class="footer-combobox">' +
                '<button ng-click="openWindowThemLoaiKhachHang()" class="k-link k-button k-button-menu"><i class="fas fa-plus fas-sm color-infor" ></i> ' + $.i18n('button_themmoi')+'</button>' +
                '</div>',
        }

        $scope.windowthemloaikhOption = {
            visible: false,
            actions: [
                //"Pin",
                //"Minimize",
                //"Maximize",
                "Close"
            ],
            open: function () {
                if ($("#IconHienThi").data("kendoDropDownList") == undefined) {
                    Create_IconHienThi();
                }
                if ($("#gridLoaiKhachHang").data("kendoGrid") == undefined) {
                    Create_gridLoaiKhachHang();
                }
                var window = $("#windowThemLoaiKhachHang").data("kendoWindow");
                $scope.LoadGridLoaiKhachHang();
            },
            activate: function () {
                $("#TenLoaiKH").focus();
            }
        }
        ComboboxDataService.getDataKhuVuc().then(function (result) {
            $scope.khuvucData = result.data;

            if ($scope.khachhangobj.iD_KhuVuc > 0) {
                idkhuvuc = $scope.khachhangobj.iD_KhuVuc;
                $timeout(function () { $("#khuvuc").data("kendoComboBox").value($scope.khachhangobj.iD_KhuVuc); }, 100);
            } else {
                $("#khuvuc").data("kendoComboBox").value("")
            }

        });

        ComboboxDataService.getTinhThanh().then(function (result) {
            $scope.tinhthanhData = result.data;

            if ($scope.khachhangobj.iD_Tinh > 0) {
                idtinh = $scope.khachhangobj.iD_Tinh;
                $timeout(function () { $("#tinhthanh").data("kendoComboBox").value($scope.khachhangobj.iD_Tinh); }, 100);
            } else {
                $("#tinhthanh").data("kendoComboBox").value("")
            }
        });

        ComboboxDataService.getQuanHuyen($scope.khachhangobj.iD_Tinh).then(function (result) {
            $scope.quanhuyenData = result.data;

            if ($scope.khachhangobj.iD_Quan > 0) {
                idquan = $scope.khachhangobj.iD_Quan;
                $timeout(function () { $("#quanhuyen").data("kendoComboBox").value($scope.khachhangobj.iD_Quan); }, 100);
            } else {
                $("#quanhuyen").data("kendoComboBox").value("")
            }
        });

        ComboboxDataService.getXaPhuong($scope.khachhangobj.iD_Quan).then(function (result) {
            $scope.phuongxaData = result.data;

            if ($scope.khachhangobj.iD_Phuong > 0) {
                idphuong = $scope.khachhangobj.iD_Phuong;
                $timeout(function () { $("#phuongxa").data("kendoComboBox").value($scope.khachhangobj.iD_Phuong); }, 100);
            } else {
                $("#phuongxa").data("kendoComboBox").value("")
            }
        });

        ComboboxDataService.getLoaiKhachHang().then(function (result) {
            $scope.loaikhachhangData = result.data;

            if ($scope.khachhangobj.iD_LoaiKhachHang > 0) {
                idloaikhachhang = $scope.khachhangobj.iD_LoaiKhachHang;
                $timeout(function () { $("#loaikhachhang").data("kendoComboBox").value($scope.khachhangobj.iD_LoaiKhachHang); }, 100);
            } else {
                $("#loaikhachhang").data("kendoComboBox").value("")
            }
        });

        //ComboboxDataService.getKenhBanHang().then(function (result) {
        //    $scope.kenhbanhangData = result.data;

        //    if ($scope.khachhangobj.iD_NhomKH > 0) {
        //        idkenhbanhang = $scope.khachhangobj.iD_NhomKH;
        //        $timeout(function () { $("#kenhbanhang").data("kendoComboBox").value($scope.khachhangobj.iD_NhomKH); }, 100);
        //    } else {
        //        $("#kenhbanhang").data("kendoComboBox").value("")
        //    }
        //});

        //ComboboxDataService.getKenhBanHangCapTren($scope.khachhangobj.iD_NhomKH).then(function (result) {
        //    $scope.kenhbanhangcaptrenData = result.data;

        //    if ($scope.khachhangobj.iD_Cha > 0) {
        //        idkenhbanhangcaptren = $scope.khachhangobj.iD_Cha;
        //        $timeout(function () { $("#kenhbanhangcaptren").data("kendoComboBox").value($scope.khachhangobj.iD_Cha); }, 100);
        //    } else {
        //        $("#kenhbanhangcaptren").data("kendoComboBox").value("")
        //    }
        //});

        $timeout(function () {
            $("#files").kendoUpload({
                multiple: false,
                select: onUploadImageSuccess,
                validation: {
                    allowedExtensions: [".jpg", ".jpeg", ".png"]
                },
                showFileList: false
            });
            $("#files").closest(".k-upload").find("span").text($.i18n("label_chonanhdaidien"));
        }, 10);
    }


    function Create_gridLoaiKhachHang() {
        $scope.gridLoaiKhachHangOptions = {
            height: 380,
            scrollable: true,
            persistSelection: true,
            resizable: true,
            sortable: true,
            filterable: {
                mode: "row"
            },
            pageable: pageableShort,
            dataBinding: function () {
                RecordNumber = (this.dataSource.page() - 1) * this.dataSource.pageSize();
            },
            columns: [{
                title: "#", template: "#= ++RecordNumber #",
                width: "50px",
                headerAttributes: { "class": "table-header-cell", style: "text-align: center;padding-bottom: 26px;" },
                attributes: { class: "text-center" },
            },
            {
                field: "tenLoaiKhachHang",
                title: $.i18n("header_tenloaikhachhang"),
                //width: "50px",
                filterable: defaultFilterableGrid,
                headerAttributes: { "class": "table-header-cell", style: "text-align: center" }

            },
            {
                field: "iD_LoaiKhachHang",
                title: $.i18n("header_tacvu"),
                width: "200px",
                filterable: false,
                headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
                attributes: {
                    style: "text-align: center; font-size: 12px"
                },
                template: function (e) {
                    var temp = '';
                    temp += "<button  class='btn btn-link btn-menubar' ng-click=SuaLoaiKhachHang(" + e.iD_LoaiKhachHang + ")><i class='fas fa-edit fas-sm color-infor'></i></button>";
                    temp += "<button  class='btn btn-link btn-menubar' ng-click=XoaLoaiKhachHang(" + e.iD_LoaiKhachHang + ")><i class='fas fa-trash fas-sm color-danger'></i></button>";
                    return temp;
                }
            }
            ]

        }
    }


    function Create_IconHienThi() {
        $("#IconHienThi").kendoDropDownList({
            dataTextField: "text",
            dataValueField: "value",
            dataSource: [
                { text: "01", value: "/assets/img/iconloaikhachhang/01.png" },
                { text: "02", value: "/assets/img/iconloaikhachhang/02.png" },
                { text: "03", value: "/assets/img/iconloaikhachhang/03.png" },
                { text: "04", value: "/assets/img/iconloaikhachhang/04.png" },
                { text: "05", value: "/assets/img/iconloaikhachhang/05.png" },
                { text: "06", value: "/assets/img/iconloaikhachhang/06.png" },
                { text: "07", value: "/assets/img/iconloaikhachhang/07.png" },
                { text: "08", value: "/assets/img/iconloaikhachhang/08.png" }
            ],
            template: '<span style="display:block;width:20px;height:20px;background-image: url(\'../#:data.value#\')"></span>',
            valueTemplate: '<span style="display:block;float:left;width:20px;height:20px;background-image: url(\'../#:data.value#\')"></span>'
        });

    }

    function getChiTietKhachHang() {
        if (idkhachhang > 0) {
            khachHangDataService.getById(idkhachhang).then(function (result) {
                $scope.khachhangobj = result.data;

                initCombobox();
                didendiachi();

                if ($scope.khachhangobj.danhsachanh.length > 0) {
                    url_anhdaidien = $scope.khachhangobj.danhsachanh[0].path;
                    $("#previewnhanvien").html('<div class="imgprevew"><img src="' + SERVERIMAGE + url_anhdaidien + '" style="width:154px;height:179px;max-height:179px;" /></div>')
                }
            })
        } else {
            $scope.khachhangobj = {
                danhsachanh: [],
                diaChi: "",
                diaChiXuatHoaDon: "",
                diagioihanhchinhid: 0,
                duongPho: "",
                email: "",
                fax: "",
                ghiChu: "",
                ghiChuKhiXoa: "",
                iD_Cha: 0,
                iD_KhuVuc: 0,
                iD_LoaiKhachHang: 0,
                iD_NhanVien: 0,
                iD_NhomKH: 0,
                iD_Phuong: 0,
                iD_Quan: 0,
                iD_QuanLy: 0,
                iD_Tinh: 0,
                idKhachHang: 0,
                idqllh: 0,
                imgurl: "",
                imgurl2: "",
                imgurl3: "",
                imgurl4: "",
                kinhDo: 0,
                lastUpdate_ID_NhanVien: 0,
                lastUpdate_ID_QuanLy: 47,
                lastUpdate_Ten_NhanVien: "",
                lastUpdate_Ten_QuanLy: "",
                lastUpdate_ThoiGian_NhanVien: "0001-01-01T00:00:00",
                lastUpdate_ThoiGian_QuanLy: "0001-01-01T00:00:00",
                maKH: "",
                maSoThue: "",
                ngaySinh: "0001-01-01T00:00:00",
                ngayTao: "0001-01-01T00:00:00",
                nguoiDaiDien: "",
                nguoiLienHe: "",
                soDienThoai: "",
                soDienThoai2: "",
                soDienThoai3: "",
                soDienThoaiMacDinh: "",
                soTKNganHang: "",
                ten: "",
                tenDayDu: "",
                tenKenhBanHang: "",
                tenLoaiKhachHang: "",
                tenNhanVien: "",
                tenPhuong: "",
                tenQuan: "",
                tenTinh: "",
                tenVietTat: "",
                tinh: "",
                trangThai: 0,
                viDo: 0,
                website: ""
            }

            initCombobox();
        }
    }
    function validatethemsualoaikhachhang() {
        let flag = true;
        let msg = '';

        if ($("#TenLoaiKH").val() == '') {
            flag = false;
            msg = $.i18n("label_tenloaikhachhangkhongduocdetrong");
            $("#TenLoaiKH").focus();
        }

        if (!flag)
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n(msg) }, 'warning');

        return flag;
    }
    function validatethemsua() {
        let flag = true;
        let msg = '';

        if (flag && ($scope.khachhangobj.ten == '' || $scope.khachhangobj.ten == undefined)) {
            flag = false;
            msg = $.i18n("label_tenkhachhangkhongduocdetrong");
            $("#TenKhachHang").focus();
        }

        if (flag && ($scope.khachhangobj.soDienThoai == '' || $scope.khachhangobj.soDienThoai == undefined)) {
            flag = false;
            msg = $.i18n("label_sodienthoaikhongduocbotrong");
            $("#SoDienThoai").focus();
        }

        if (flag && ($scope.khachhangobj.email == '' || $scope.khachhangobj.email == undefined)) {
            flag = false;
            msg = $.i18n("label_emailkhongduocbotrong");
            $("#Email").focus();
        }

        if (flag && ($scope.khachhangobj.diaChi == '' || $scope.khachhangobj.diaChi == undefined)) {
            flag = false;
            msg = $.i18n("label_diachikhongduocbotrong");
            $("#DiaChi").focus();
        }

        if (!flag)
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n(msg) }, 'warning');

        return flag;
    }

    function onUploadImageSuccess(e) {
        let data = new FormData();
        data.append('file', e.files[0].rawFile);
        let files = e.files[0];
        if (files.extension.toLowerCase() != ".jpg" && files.extension.toLowerCase() != ".png" && files.extension.toLowerCase() != ".jpeg") {
            e.preventDefault();
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_vuilongchonfileanhjpgpngjpeg') }, 'warning');
        } else {
            khachHangDataService.uploadAnhDaiDien(data).then(function (result) {
                $("#previewnhanvien").html('<div class="imgprevew"><img src="' + urlApi + result.url + '" style="width:154px;height:179px;max-height:179px;" /></div>')
                url_anhdaidien = result.url;
                image_url = result.url;
                if (!result.flag)
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
            })
        }
    }

    function didendiachi() {
        let diachi = $scope.khachhangobj.diaChi;

        let geocoder = new google.maps.Geocoder();
        geocoder.geocode({ 'address': diachi }, function (results, status) {
            if (status === 'OK') {
                if (results[0]) {
                    marker.setPosition(results[0].geometry.location);
                    mapkhachhang.panTo(results[0].geometry.location);
                    mapkhachhang.setZoom(13);

                } else {
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_khongtimduocdiachi') }, 'warning');
                }
            } else {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_khongtimduocdiachi') }, 'error');
            }
        });
    }

    //event

    $scope.SuaLoaiKhachHang = function (id) {
        danhMucDataService.getbyidloaiKhachHang(id).then(function (result) {
            console.log(result);
            $("#IDLoaiKhachHang").val(result.data.iD_LoaiKhachHang);
            $("#TenLoaiKH").val(result.data.tenLoaiKhachHang);
            $("#IconHienThi").data("kendoDropDownList").value(result.data.iconHienThi);
        })
    }

    $scope.XoaLoaiKhachHang = function (id) {
        danhMucDataService.deleteLoaiKhachHang(id).then(function (result) {
            if (result.flag) {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                $scope.LoadGridLoaiKhachHang();
            }
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
        });
    }

    $scope.openWindowThemLoaiKhachHang = function () {
        $("#windowThemLoaiKhachHang").data("kendoWindow").center().open();
    }
    $scope.LoadGridLoaiKhachHang = function () {
        ComboboxDataService.getLoaiKhachHang().then(function (result) {
            $scope.loaikhachhangData = result.data;
            $scope.gridLoaiKhachHangData = new kendo.data.DataSource({
                data: result.data,
                schema: {
                    model: {
                        id: "iD_LoaiKhachHang"
                    }
                },
                pageSize: 10
            });
        })
    }
    $scope.khuvucOnChange = function () {
        $scope.khuvucselect = this.khuvucselect;
    }
    $scope.tinhthanhOnChange = function () {
        $scope.tinhthanhselect = this.tinhthanhselect;

        let idTinh = 0;
        if ($scope.tinhthanhselect != undefined)
            idTinh = ($scope.tinhthanhselect.iD_Tinh < 0) ? 0 : $scope.tinhthanhselect.iD_Tinh;

        ComboboxDataService.getQuanHuyen(idTinh).then(function (result) {
            $scope.quanhuyenData = result.data;

            $("#quanhuyen").data("kendoComboBox").value("");
        });
    }
    $scope.quanhuyenOnChange = function () {
        $scope.quanhuyenselect = this.quanhuyenselect;

        let idQuan = 0;
        if ($scope.quanhuyenselect != undefined)
            idQuan = ($scope.quanhuyenselect.iD_Quan < 0) ? 0 : $scope.quanhuyenselect.iD_Quan;

        ComboboxDataService.getXaPhuong(idQuan).then(function (result) {
            $scope.phuongxaData = result.data;

            $("#phuongxa").data("kendoComboBox").value("")
        });
    }
    $scope.phuongxaOnChange = function () {
        $scope.phuongxaselect = this.phuongxaselect;
    }
    $scope.loaikhachhangOnChange = function () {
        $scope.loaikhachhangselect = this.loaikhachhangselect;
    }
    $scope.kenhbanhangOnChange = function () {
        $scope.kenhbanhangselect = this.kenhbanhangselect;

        let idKenhBanHang = 0;
        if ($scope.kenhbanhangselect != undefined)
            idKenhBanHang = ($scope.kenhbanhangselect.iD_KenhBanHang < 0) ? 0 : $scope.kenhbanhangselect.iD_KenhBanHang;

        ComboboxDataService.getKenhBanHangCapTren(idKenhBanHang).then(function (result) {
            $scope.kenhbanhangcaptrenData = result.data;
        });
    }
    $scope.kenhbanhangcaptrenOnChange = function () {
        $scope.kenhbanhangcaptrenselect = this.kenhbanhangcaptrenselect;
    }

    $scope.LuuKhachHang = function () {
        if (validatethemsua()) {
            if ($scope.khuvucselect != undefined)
                idkhuvuc = ($scope.khuvucselect.iD_KhuVuc < 0) ? 0 : $scope.khuvucselect.iD_KhuVuc;
            else
                idkhuvuc = 0;
            if ($scope.tinhthanhselect != undefined)
                idtinh = ($scope.tinhthanhselect.iD_Tinh < 0) ? 0 : $scope.tinhthanhselect.iD_Tinh;
            else
                idtinh = 0;
            if ($scope.quanhuyenselect != undefined)
                idquan = ($scope.quanhuyenselect.iD_Quan < 0) ? 0 : $scope.quanhuyenselect.iD_Quan;
            else
                idquan = 0;
            if ($scope.phuongxaselect != undefined)
                idphuong = ($scope.phuongxaselect.iD_Phuong < 0) ? 0 : $scope.phuongxaselect.iD_Phuong;
            else
                idphuong = 0;
            if ($scope.loaikhachhangselect != undefined)
                idloaikhachhang = ($scope.loaikhachhangselect.iD_LoaiKhachHang < 0) ? 0 : $scope.loaikhachhangselect.iD_LoaiKhachHang;
            else
                idloaikhachhang = 0;
            if ($scope.kenhbanhangselect != undefined)
                idkenhbanhang = ($scope.kenhbanhangselect.iD_KenhBanHang < 0) ? 0 : $scope.kenhbanhangselect.iD_KenhBanHang;
            else
                idkenhbanhang = 0;
            if ($scope.kenhbanhangcaptrenselect != undefined)
                idkenhbanhangcaptren = ($scope.kenhbanhangcaptrenselect.iD_KhachHang < 0) ? 0 : $scope.kenhbanhangcaptrenselect.iD_KhachHang;
            else
                idkenhbanhangcaptren = 0;

            let formData = {
                MaKH: $scope.khachhangobj.maKH,
                Ten: $scope.khachhangobj.ten,
                DiaChi: $scope.khachhangobj.diaChi,
                KinhDo: $("#kinhdo").val(),
                ViDo: $("#vido").val(),
                DuongPho: $scope.khachhangobj.duongPho,
                ID_KhuVuc: idkhuvuc,
                ID_Tinh: idtinh,
                ID_Quan: idquan,
                ID_Phuong: idphuong,
                SoDienThoaiMacDinh: $scope.khachhangobj.soDienThoai,
                Fax: $scope.khachhangobj.fax,
                SoDienThoai1: $scope.khachhangobj.soDienThoai,
                SoDienThoai2: $scope.khachhangobj.soDienThoai2,
                SoDienThoai3: $scope.khachhangobj.soDienThoai3,
                ID_LoaiKhachHang: idloaikhachhang,
                ID_NhomKH: idkenhbanhang,
                ID_Cha: idkenhbanhangcaptren,
                NguoiLienHe: $scope.khachhangobj.nguoiLienHe,
                Email: $scope.khachhangobj.email,
                Website: $scope.khachhangobj.website,
                DiaChiXuatHoaDon: $scope.khachhangobj.diaChiXuatHoaDon,
                SoTKNganHang: $scope.khachhangobj.soTKNganHang,
                MaSoThue: $scope.khachhangobj.maSoThue,
                GhiChu: $scope.khachhangobj.ghiChu,
                ImgUrl: image_url,
                IDQLLH: $rootScope.UserInfo.iD_QLLH,
                ID_NhanVien: $rootScope.UserInfo.iD_QuanLy,
                ID_QuanLy: $rootScope.UserInfo.iD_QuanLy,
                IDKhachHang: idkhachhang
            }

            khachHangDataService.save(formData).then(function (result) {
                if (result.flag) {
                    $state.go('khachhang');
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                }
                else
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');

            });
        }
    }
    $scope.HuyKhachHang = function () {
        $state.go('khachhang');
    }

    $scope.timdiachi = function () {
        let diachi = $scope.khachhangobj.diaChi;

        let geocoder = new google.maps.Geocoder();
        geocoder.geocode({ 'address': diachi }, function (results, status) {
            if (status === 'OK') {
                if (results[0]) {
                    let duongpho = "";
                    for (let i = 0; i < results[0].address_components.length; i++) {
                        if (results[0].address_components[i].types.length > 0) {
                            for (let j = 0; j < results[0].address_components[i].types.length; j++) {
                                if (results[0].address_components[i].types[j] == "route") {
                                    duongpho = results[0].address_components[i].long_name;
                                }
                            }
                        }
                    }
                    marker.setPosition(results[0].geometry.location);
                    mapkhachhang.panTo(results[0].geometry.location);
                    mapkhachhang.setZoom(13);

                    $("#vido").val(marker.getPosition().lat().toFixed(6));
                    $("#kinhdo").val(marker.getPosition().lng().toFixed(6));
                    $("#duongpho").val(duongpho);

                } else {
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_khongtimduocdiachi') }, 'warning');
                }
            } else {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_khongtimduocdiachi') }, 'error');
            }
        });
    }

    $scope.changediadiem = function (e) {
        $("#vido").val(e.latLng.lat().toFixed(6));
        $("#kinhdo").val(e.latLng.lng().toFixed(6));

        let geocoder = new google.maps.Geocoder();
        geocoder.geocode({ 'location': e.latLng }, function (results, status) {
            if (status === 'OK') {
                if (results[0]) {
                    //$("#diaChi").val(results[0].formatted_address);
                    //$("#duongPho").val(results[0].formatted_address);

                    $scope.khachhangobj.diaChi = results[0].formatted_address;
                    $scope.khachhangobj.duongPho = results[0].formatted_address;
                } else {
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_khongtimduocdiachi') }, 'warning');
                }
            } else {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_khongtimduocdiachi') }, 'error');
            }
        });
    }

    $scope.ThemLoaiKhachHang = function () {
        if (validatethemsualoaikhachhang()) {
            var tenloai = $("#TenLoaiKH").val();
            var icon = $("#IconHienThi").val();
            var id = $("#IDLoaiKhachHang").val();
            let data = {
                ID_LoaiKhachHang: id,
                IconHienThi: icon,
                TenLoaiKhachHang: tenloai
            }
            danhMucDataService.saveLoaiKhachHang(data).then(function (result) {
                if (result.flag) {
                    $scope.HuyLoaiKhachHang();
                    $scope.LoadGridLoaiKhachHang();
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                }
                else
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
            });
        }
    }
    $scope.HuyLoaiKhachHang = function () {
        $("#IDLoaiKhachHang").val(0);
        $("#TenLoaiKH").val("");
        $("#IconHienThi").data("kendoDropDownList").value("");
    }
    init();
})