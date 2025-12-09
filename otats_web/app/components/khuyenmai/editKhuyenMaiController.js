angular.module('app').controller('editKhuyenMaiController', function ($rootScope, $scope, $state, $stateParams, $timeout, Notification, ComboboxDataService, khuyenMaiDataService) {
    CreateSiteMap();

    $scope.object = {};

    let __idkhuyenmai = 0;
    let __idhinhthuc = 1;
    let __idmathangdetang = 0;
    let __idnhommathangchon = 0;

    let __arrhangtangall = [];
    let __arridhangtangone = [];

    let __arridhangchitiet = [];

    let image_url = '';

    function init() {
        __idkhuyenmai = $stateParams.idkhuyenmai;
        $scope.idkhuyenmai = __idkhuyenmai;

        getquyen();

        initform();

        inithangtang();
        inittreeview();
        inittreenhomchon();
    }

    function getquyen() {
        let url = 'khuyenmai'
        ComboboxDataService.getquyen(url).then(function (result) {
            $scope.permission = result.data;
            if ($scope.permission.iD_ChucNang <= 0 || ($scope.permission.them <= 0 && __idkhuyenmai <= 0)) {
                Notification({ title: $.i18n("label_thongbao"), message: $.i18n('label_khongcoquyentruycapchucnang') }, "error");
                $location.path('/khuyenmai')
            }
        });
    }

    function inithangtang() {
        khuyenMaiDataService.GetChiTietHangTang(__idkhuyenmai).then(function (response) {
            __arrhangtangall = response.data;
        });
    }

    function initform() {
        khuyenMaiDataService.getbyid(__idkhuyenmai).then(function (response) {
            $scope.objectctkm = response.data;
            $scope.hieuluc = (response.data.trangThai == 1)
            if (__idkhuyenmai <= 0) {
                $scope.hieuluc = true;
                $scope.objectctkm.tenCTKM = '';
                $scope.objectctkm.ghiChu = '';
                $scope.objectctkm.tongTienDatKM_Tu = 0;
                $scope.objectctkm.tongTienDatKM_Den = 0;
                $scope.objectctkm.chietKhauPhanTram = 0;
                $scope.objectctkm.chietKhauTien = 0;
            }


            fillingform(response.data);
        });

        $scope.luyke = false;
        $scope.chietkhauphantrambanbuon = 0;
        $scope.chietkhautienbanbuon = 0;
        $scope.chietkhauphantrambanle = 0;
        $scope.chietkhautienbanle = 0;

        $("#files_khuyenmai").kendoUpload({
            multiple: false,
            select: onUploadImageSuccess,
            validation: {
                allowedExtensions: [".jpg", ".jpeg", ".png"]
            },
            showFileList: false
        });
        $("#files_khuyenmai").closest(".k-upload").find("span").text($.i18n("label_chonanhdaidien"));
    }
    function fillingform(data) {
        $scope.obj_TuNgay = new Date(data.ngayApDung);
        $scope.obj_DenNgay = new Date(data.ngayKetThuc);
        $scope.maxDate = $scope.obj_DenNgay;
        $scope.minDate = $scope.obj_TuNgay;

        if (data.anhDaiDien != '') {
            $("#preview").html('<div class="imgprevew"><img src="' + SERVERIMAGE + data.anhDaiDien + '" style="width:154px;height:179px;max-height:179px;" /></div>')
        }
        else {
            $("#preview").html('')
        }

        fillingcombobox(data.loai);
    }
    function fillingcombobox(loai) {
        khuyenMaiDataService.getlistLoaiKhuyenMai().then(function (respone) {
            $scope.loaikhuyenmaiData = respone.data;

            if (loai > 0)
                __idhinhthuc = loai;

            showdetailkhuyenmai();

            $timeout(function () {
                $("#loaikhuyenmai").data("kendoComboBox").value(__idhinhthuc);
            }, 50);
        });
    }
    function showdetailkhuyenmai() {
        //Chiết khấu đơn hàng (%)
        if (__idhinhthuc == 1) {
            $scope.showkhuyenmaidonhang = true;
            $scope.showtongtienhang = false;
            $scope.showchietkhauphantram = true;
            $scope.showchietkhautien = true;

            $scope.showchitiet = false;
            $scope.showhangtang = false;
        }
        //Chiết khấu SP (%)
        if (__idhinhthuc == 2) {
            $scope.showkhuyenmaidonhang = false;

            $scope.showchitiet = true;
            $scope.showhangtang = false;
        }
        //Mua sản phẩm - đạt số lượng - chiết khấu SP (%)
        if (__idhinhthuc == 3) {
            $scope.showkhuyenmaidonhang = false;

            $scope.showchitiet = true;
            $scope.showhangtang = false;
        }
        //Mua sản phẩm - đạt số lượng - tặng sản phẩm
        if (__idhinhthuc == 4) {
            $scope.showkhuyenmaidonhang = false;

            $scope.showchitiet = true;
            $scope.showhangtang = true;
        }
        //Mua sản phẩm - đạt số lượng - tặng tiền
        if (__idhinhthuc == 5) {
            $scope.showkhuyenmaidonhang = false;

            $scope.showchitiet = true;
            $scope.showhangtang = false;
        }
        //Mua sản phẩm - đạt số tiền - chiết khấu SP (%)
        if (__idhinhthuc == 6) {
            $scope.showkhuyenmaidonhang = false;

            $scope.showchitiet = true;
            $scope.showhangtang = false;
        }
        //Mua sản phẩm - đạt số tiền - tặng sản phẩm
        if (__idhinhthuc == 7) {
            $scope.showkhuyenmaidonhang = false;

            $scope.showchitiet = true;
            $scope.showhangtang = true;
        }
        //Mua sản phẩm - đạt số tiền - tặng tiền
        if (__idhinhthuc == 8) {
            $scope.showkhuyenmaidonhang = false;

            $scope.showchitiet = true;
            $scope.showhangtang = false;
        }
        //Tổng tiền hàng - chiết khấu đơn hàng (%)
        if (__idhinhthuc == 9) {
            $scope.showkhuyenmaidonhang = true;
            $scope.showtongtienhang = true;
            $scope.showchietkhauphantram = true;
            $scope.showchietkhautien = false;

            $scope.showchitiet = false;
            $scope.showhangtang = false;
        }
        //Tổng tiền hàng - tặng sản phẩm
        if (__idhinhthuc == 10) {
            $scope.showkhuyenmaidonhang = true;
            $scope.showtongtienhang = true;
            $scope.showchietkhauphantram = false;
            $scope.showchietkhautien = false;

            $scope.showchitiet = false;
            $scope.showhangtang = true;
        }
        //Tổng tiền hàng - tặng tiền
        if (__idhinhthuc == 11) {
            $scope.showkhuyenmaidonhang = true;
            $scope.showtongtienhang = true;
            $scope.showchietkhauphantram = false;
            $scope.showchietkhautien = true;

            $scope.showchitiet = false;
            $scope.showhangtang = false;
        }

        if (__idhinhthuc >= 2 && __idhinhthuc <= 8)
            loadgrid($scope.objectctkm.chiTietCTKM);
    }

    function listColumnsgrid() {
        let dataList = [];

        dataList.push({
            field: "chiTiet", title: $.i18n("header_xoa"),
            template: '<button ng-click="deleterow()" class="btn btn-link btn-menubar" title ="' + $.i18n("header_xoa") + '" ><i class="fas fa-trash fas-sm color-danger"></i></button> ',
            attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "55px"
        });
        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });

        dataList.push({
            field: "tacvu", title: $.i18n("header_tacvu"),
            template: '<button ng-click="openformchonhangtang()" class="btn btn-link btn-menubar" title ="' + $.i18n("label_chonmathangtang") + '" ><i class="fas fa-tags fas-sm color-infor"></i></button> ',
            width: "100px",
            filterable: false,
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({ field: "soLuongHangTang", title: $.i18n("label_hangKM"), attributes: { class: "text-center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px" });

        dataList.push({ field: "tenMatHang", title: $.i18n("header_tenhang"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "300px" });
        dataList.push({ field: "maHang", title: $.i18n("header_mahang"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({ field: "donViTinh", title: $.i18n("header_donvi"), attributes: { class: "text-center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px" });
        dataList.push({
            field: "giaBanBuon", title: $.i18n("header_giabanbuon"),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            attributes: { style: "text-align: right" }, footerAttributes: { style: "text-align: right" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "chietKhauPhanTram_BanBuon", title: $.i18n("header_chietkhauphantrambanbuon"),
            format: formatNumberInGrid('n2'),
            attributes: { style: "text-align: right" }, footerAttributes: { style: "text-align: right" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "chietKhauTien_BanBuon", title: $.i18n("header_chietkhautienbanbuon"),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            attributes: { style: "text-align: right" }, footerAttributes: { style: "text-align: right" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "giaBanLe", title: $.i18n("header_giabanle"),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            attributes: { style: "text-align: right" }, footerAttributes: { style: "text-align: right" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "chietKhauPhanTram_BanLe", title: $.i18n("header_chietkhauphantrambanle"),
            format: formatNumberInGrid('n2'),
            attributes: { style: "text-align: right" }, footerAttributes: { style: "text-align: right" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "chietKhauTien_BanLe", title: $.i18n("header_chietkhautienbanle"),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            attributes: { style: "text-align: right" }, footerAttributes: { style: "text-align: right" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "soLuongDatKM_Tu", title: $.i18n("header_soluongtoithieu"),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            attributes: { style: "text-align: right" }, footerAttributes: { style: "text-align: right" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "soLuongDatKM_Den", title: $.i18n("header_soluongtoida"),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            attributes: { style: "text-align: right" }, footerAttributes: { style: "text-align: right" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "tongTienDatKM_Tu", title: $.i18n("header_tongtientoithieu"),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            attributes: { style: "text-align: right" }, footerAttributes: { style: "text-align: right" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "tongTienDatKM_Den", title: $.i18n("header_tongtientoida"),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            attributes: { style: "text-align: right" }, footerAttributes: { style: "text-align: right" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });

        return dataList;
    }
    function loadgrid(_data) {
        kendo.ui.progress($("#gridChiTiet"), true);
        $scope.gridChiTietOptions = {
            sortable: true,
            height: function () {
                return 350;
            },
            dataBinding: function () {
                RecordNumber = (this.dataSource.page() - 1) * this.dataSource.pageSize();
            },
            resizable: true,
            editable: true,
            filterable: {
                mode: "row"
            },
            save: function (e) {
                savechitietkhuyenmai(e);
            },
            update: function (e) {
                e.success();
            },
            pageable: $scope.lang == 'vi-vn' ? pageableShort_vi : pageableShort_en,
            columns: listColumnsgrid()
        };

        let arr_hangchitietfinal = [];
        let arr_hangchitiet = _data;
        if (arr_hangchitiet != null) {
            __arridhangchitiet = arr_hangchitiet.map((ar, index, arr) => {
                return ar.iD_Hang
            })

            arr_hangchitietfinal = arr_hangchitiet.map((ar, index, arr) => {
                let nobj = ar;
                nobj.soLuongHangTang = counthangtang(ar.iD_Hang);
                return nobj
            })
        }
        
        $scope.gridChiTietData = {
            data: arr_hangchitietfinal,
            schema: {
                model: {
                    id: 'idMatHang',
                    fields: {
                        maHang: {
                            type: "string",
                            editable: false,
                            nullable: true
                        },
                        tenMatHang: {
                            type: "string",
                            editable: false,
                            nullable: true
                        },
                        tacvu: {
                            type: "string",
                            editable: false,
                            nullable: true
                        },
                        soLuongHangTang: {
                            type: "string",
                            editable: false,
                            nullable: true
                        },
                        donViTinh: {
                            type: "string",
                            editable: false,
                            nullable: true
                        },
                        giaBanBuon: {
                            type: "number",
                            editable: false,
                            nullable: true
                        },
                        giaBanLe: {
                            type: "number",
                            editable: false,
                            nullable: true
                        },
                        chietKhauTien_BanBuon: {
                            type: "number",
                            validation: {
                                min: 0
                            },
                            editable: true,
                            nullable: true
                        },
                        chietKhauTien_BanLe: {
                            type: "number",
                            validation: {
                                min: 0
                            },
                            editable: true,
                            nullable: true
                        },
                        soLuongDatKM_Den: {
                            type: "number",
                            validation: {
                                min: 0
                            },
                            editable: true,
                            nullable: true
                        },
                        soLuongDatKM_Tu: {
                            type: "number",
                            validation: {
                                min: 0
                            },
                            editable: true,
                            nullable: true
                        },
                        tongTienDatKM_Den: {
                            type: "number",
                            validation: {
                                min: 0
                            },
                            editable: true,
                            nullable: true
                        },
                        tongTienDatKM_Tu: {
                            type: "number",
                            validation: {
                                min: 0
                            },
                            editable: true,
                            nullable: true
                        },
                        chietKhauPhanTram_BanBuon: {
                            type: "number",
                            validation: {
                                min: 0,
                                max: 100
                            },
                            editable: true,
                            nullable: true
                        },
                        chietKhauPhanTram_BanLe: {
                            type: "number",
                            validation: {
                                min: 0,
                                max: 100
                            },
                            editable: true,
                            nullable: true
                        }
                    }
                }
            },
            pageSize: 20
        };

        kendo.ui.progress($("#gridChiTiet"), false);

        if (arr_hangchitietfinal.length > 0)
            $scope.luyke = (arr_hangchitietfinal[0].apDungBoiSo == 1) ? true: false;
        else
            $scope.luyke = false;

        $timeout(function () {
            showcolumschitiet();
            loadtreeView();
        }, 200)
    }
    function reloadgrid(_data) {
        if (!_data) {
            let myGridAdd = $("#gridChiTiet").data("kendoGrid");
            _data = myGridAdd.dataSource.data();
        }

        let arr_hangchitietfinal = _data.map((ar, index, arr) => {
            let nobj = ar;
            nobj.soLuongHangTang = counthangtang(ar.iD_Hang);
            return nobj
        })

        $scope.gridChiTietData = {
            data: arr_hangchitietfinal,
            schema: {
                model: {
                    id: 'idMatHang',
                    fields: {
                        maHang: {
                            type: "string",
                            editable: false,
                            nullable: true
                        },
                        tenMatHang: {
                            type: "string",
                            editable: false,
                            nullable: true
                        },
                        tacvu: {
                            type: "string",
                            editable: false,
                            nullable: true
                        },
                        soLuongHangTang: {
                            type: "string",
                            editable: false,
                            nullable: true
                        },
                        donViTinh: {
                            type: "string",
                            editable: false,
                            nullable: true
                        },
                        giaBanBuon: {
                            type: "number",
                            editable: false,
                            nullable: true
                        },
                        giaBanLe: {
                            type: "number",
                            editable: false,
                            nullable: true
                        },
                        chietKhauTien_BanBuon: {
                            type: "number",
                            validation: {
                                min: 0
                            },
                            editable: true,
                            nullable: true
                        },
                        chietKhauTien_BanLe: {
                            type: "number",
                            validation: {
                                min: 0
                            },
                            editable: true,
                            nullable: true
                        },
                        soLuongDatKM_Den: {
                            type: "number",
                            validation: {
                                min: 0
                            },
                            editable: true,
                            nullable: true
                        },
                        soLuongDatKM_Tu: {
                            type: "number",
                            validation: {
                                min: 0
                            },
                            editable: true,
                            nullable: true
                        },
                        tongTienDatKM_Den: {
                            type: "number",
                            validation: {
                                min: 0
                            },
                            editable: true,
                            nullable: true
                        },
                        tongTienDatKM_Tu: {
                            type: "number",
                            validation: {
                                min: 0
                            },
                            editable: true,
                            nullable: true
                        },
                        chietKhauPhanTram_BanBuon: {
                            type: "number",
                            validation: {
                                min: 0,
                                max: 100
                            },
                            editable: true,
                            nullable: true
                        },
                        chietKhauPhanTram_BanLe: {
                            type: "number",
                            validation: {
                                min: 0,
                                max: 100
                            },
                            editable: true,
                            nullable: true
                        }
                    }
                }
            },
            pageSize: 20
        };

        $("#gridChiTiet").data("kendoGrid").refresh();
    }

    function counthangtang(_idhang) {
        let sum = 0;
        for (i = 0; i < __arrhangtangall.length; i++) {
            if (__arrhangtangall[i].iD_Hang == _idhang) {
                sum += __arrhangtangall[i].soLuong
            }
        }

        return sum;
    }

    function showcolumschitiet() {
        let grid = $("#gridChiTiet").data("kendoGrid");

        if (__idhinhthuc === 4 || __idhinhthuc === 7) {
            try {
                grid.showColumn("tacvu");
                grid.showColumn("soLuongHangTang");
            }
            catch (err) {
                //console.log(err);
            }
        }
        else {
            try {
                grid.hideColumn("tacvu");
                grid.hideColumn("soLuongHangTang");
            }
            catch (err) {
                //console.log(err);
            }
        }

        if (__idhinhthuc === 2 || __idhinhthuc === 3 || __idhinhthuc === 6) {
            grid.showColumn("chietKhauPhanTram_BanBuon");
            grid.showColumn("chietKhauPhanTram_BanLe");
        }
        else {
            grid.hideColumn("chietKhauPhanTram_BanBuon");
            grid.hideColumn("chietKhauPhanTram_BanLe");
        }

        if (__idhinhthuc === 2 || __idhinhthuc === 5 || __idhinhthuc === 8) {
            grid.showColumn("chietKhauTien_BanBuon");
            grid.showColumn("chietKhauTien_BanLe");
        }
        else {
            grid.hideColumn("chietKhauTien_BanBuon");
            grid.hideColumn("chietKhauTien_BanLe");
        }

        if (__idhinhthuc === 3 || __idhinhthuc === 4 || __idhinhthuc === 5) {
            grid.showColumn("soLuongDatKM_Tu");
            grid.showColumn("soLuongDatKM_Den");
        }
        else {
            grid.hideColumn("soLuongDatKM_Tu");
            grid.hideColumn("soLuongDatKM_Den");
        }

        if (__idhinhthuc === 6 || __idhinhthuc === 7 || __idhinhthuc === 8) {
            grid.showColumn("tongTienDatKM_Tu");
            grid.showColumn("tongTienDatKM_Den");
        }
        else {
            grid.hideColumn("tongTienDatKM_Tu");
            grid.hideColumn("tongTienDatKM_Den");
        }

        grid.refresh();
    }
    function savechitietkhuyenmai(e) {
        if (validatechitietkhuyenmai(e)) {
            $("#gridChiTiet").data("kendoGrid").refresh();
        } else {
            e.preventDefault();
            if (e.values.soLuongDatKM_Tu != null) {
                e.values.soLuongDatKM_Tu = e.model.soLuongDatKM_Tu;
                e.model.dirtyFields.soLuongDatKM_Tu = false;
                e.model.dirty = false;
            }

            if (e.values.soLuongDatKM_Den != null) {
                e.values.soLuongDatKM_Den = e.model.soLuongDatKM_Den;
                e.model.dirtyFields.soLuongDatKM_Den = false;
                e.model.dirty = false;
            }

            if (e.values.tongTienDatKM_Tu != null) {
                e.values.tongTienDatKM_Tu = e.model.tongTienDatKM_Tu;
                e.model.dirtyFields.tongTienDatKM_Tu = false;
                e.model.dirty = false;
            }

            if (e.values.tongTienDatKM_Den != null) {
                e.values.tongTienDatKM_Den = e.model.tongTienDatKM_Den;
                e.model.dirtyFields.tongTienDatKM_Den = false;
                e.model.dirty = false;
            }
        }
    }
    function validatechitietkhuyenmai(e) {
        let flag = true;
        let msg = "";

        if (flag && (e.values.soLuongDatKM_Tu != null && e.values.soLuongDatKM_Den == null)) {
            let soluongtu = e.values.soLuongDatKM_Tu;
            let soluongden = e.model.soLuongDatKM_Den;

            if (flag && soluongtu > 0 && soluongden && soluongtu > soluongden) {
                msg = $.i18n('label_soluongtukhongthelonhonsoluongden');
                flag = false;
            }
        }
        if (flag && (e.values.soLuongDatKM_Tu == null && e.values.soLuongDatKM_Den != null)) {
            let soluongtu = e.model.soLuongDatKM_Tu;
            let soluongden = e.values.soLuongDatKM_Den;

            if (flag && soluongtu > 0 && soluongden && soluongtu > soluongden) {
                msg = $.i18n('label_soluongdenkhongthenhohonsoluongtu');
                flag = false;
            }
        }
        if (flag && (e.values.soLuongDatKM_Tu != null && e.values.soLuongDatKM_Den != null)) {
            let soluongtu = e.values.soLuongDatKM_Tu;
            let soluongden = e.values.soLuongDatKM_Den;

            if (flag && soluongtu > 0 && soluongden && soluongtu > soluongden) {
                msg = $.i18n('label_soluongdenkhongthenhohonsoluongtu');
                flag = false;
            }
        }

        if (flag && (e.values.tongTienDatKM_Tu != null && e.values.tongTienDatKM_Den == null)) {
            let tongtientu = e.values.tongTienDatKM_Tu;
            let tongtienden = e.model.tongTienDatKM_Den;

            if (flag && tongtientu > 0 && tongtienden > 0 && tongtientu > tongtienden) {
                msg = $.i18n('label_tongtientukhongthelonhontongtienden');
                flag = false;
            }
        }
        if (flag && (e.values.tongTienDatKM_Tu == null && e.values.tongTienDatKM_Den != null)) {
            let tongtientu = e.model.tongTienDatKM_Tu;
            let tongtienden = e.values.tongTienDatKM_Den;

            if (flag && tongtientu > 0 && tongtienden > 0 && tongtientu > tongtienden) {
                msg = $.i18n('label_tongtiendenkhongthenhohontongtientu');
                flag = false;
            }
        }
        if (flag && (e.values.tongTienDatKM_Tu != null && e.values.tongTienDatKM_Den != null)) {
            let tongtientu = e.values.tongTienDatKM_Tu;
            let tongtienden = e.values.tongTienDatKM_Den;

            if (flag && tongtientu > 0 && tongtienden > 0 && tongtientu > tongtienden) {
                msg = $.i18n('label_tongtiendenkhongthenhohontongtientu');
                flag = false;
            }
        }

        if (!flag) {
            Notification({ title: $.i18n("label_thongbao"), message: msg }, 'warning');
        }

        return flag;
    }

    function inittreeview() {
        let dataSource = new kendo.data.HierarchicalDataSource({
            data: [],
            schema: {
                model: {
                    children: "childs"
                }
            }
        })

        $("#treehangchon").kendoTreeView({
            dataSource: dataSource,
            dataTextField: "name",
            dataValueField: "id",
            select: onSelectNhom,
        });

        //loadtreeView();
    }
    function loadtreeView() {
        khuyenMaiDataService.getListNhomMatHang().then(function (result) {
            setDataTreeview(result.data);
        });
    }
    function setDataTreeview(data) {
        let dataSource = new kendo.data.HierarchicalDataSource({
            data: data,
            schema: {
                model: {
                    children: "childs"
                }
            }
        })

        let tree = $("#treehangchon").data("kendoTreeView");
        tree.setDataSource(dataSource);
        tree.expand(".k-item");
        tree.select(".k-first");

        let selectedNode = tree.select();
        $scope.idNhom = tree.dataItem(selectedNode).id;
        loadgridhangchon();
    }
    function listColumnsgridhangchon() {
        let dataList = [];

        dataList.push({
            field: "chiTiet", title: $.i18n("header_them"),
            template: '<button ng-click="addrow()" class="btn btn-link btn-menubar" title ="' + $.i18n("button_themhang") + '" ><i class="fas fa-plus-circle fas-sm color-infor"></i></button> ',
            attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "70px"
        });
        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({ field: "maHang", title: $.i18n("header_mahang"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px" });
        dataList.push({ field: "tenHang", title: $.i18n("header_tenhang"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "220px" });
        dataList.push({ field: "tenDonVi", title: $.i18n("header_donvi"), attributes: { class: "text-center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "80px" });
        dataList.push({ field: "giaBanBuon", title: $.i18n("header_giabanbuon"), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), attributes: { class: "text-right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px" });
        dataList.push({ field: "giaBanLe", title: $.i18n("header_giabanle"), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), attributes: { class: "text-right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px" });
        dataList.push({ field: "tenDanhMuc", title: $.i18n("header_danhmuc"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "220px" });

        return dataList;
    }
    function loadgridhangchon() {
        kendo.ui.progress($("#gridHangChon"), true);
        $scope.gridHangChonOptions = {
            sortable: true,
            height: function () {
                return 300;
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
            columns: listColumnsgridhangchon()
        };

        khuyenMaiDataService.getlistmathangctkm(__idkhuyenmai, $scope.idNhom).then(function (result) {
            let arr_hangchonall = result.data;
            let arr_hangchonchuachon = arr_hangchonall.filter((item) => {
                return (__arridhangchitiet.indexOf(item.iD_Hang) == -1)
            })

            $scope.gridHangChonData = {
                data: arr_hangchonchuachon,
                schema: {
                    model: {
                        id: 'idMatHang',
                        fields: {
                            iD_Hang: {
                                type: "number"
                            }
                        }
                    }
                },
                pageSize: 20
            };
            kendo.ui.progress($("#gridHangChon"), false);
        });
    }
    function onSelectNhom(e) {
        $scope.idNhom = $("#treehangchon").getKendoTreeView().dataItem(e.node).id;
        loadgridhangchon();
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
                    text: $.i18n("button_dong"),primary: false, action: function () {
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
    function validate() {
        let flag = true;
        let msg = '';

        let myGridAdd = $("#gridChiTiet").data("kendoGrid");
        
        let dataSource = [];
        if(myGridAdd != undefined)
            dataSource = myGridAdd.dataSource.data();
        let DataChiTiet = [];
        let DanhSachHangTang = [];

        if (__idhinhthuc >= 2 && __idhinhthuc <= 8) {
            for (i = 0; i < dataSource.length; i++) {
                let chietKhauPhanTram_BanBuon = 0;
                let chietKhauTien_BanBuon = 0;
                let chietKhauPhanTram_BanLe = 0;
                let chietKhauTien_BanLe = 0;
                let soLuongDatKM_Den = 0;
                let soLuongDatKM_Tu = 0;
                let tongTienDatKM_Den = 0;
                let tongTienDatKM_Tu = 0;

                //Chiết khấu SP (%)
                if (__idhinhthuc == 2) {
                    chietKhauPhanTram_BanBuon = dataSource[i].chietKhauPhanTram_BanBuon;
                    chietKhauPhanTram_BanLe = dataSource[i].chietKhauPhanTram_BanLe;
                }
                //Mua sản phẩm - đạt số lượng - chiết khấu SP (%)
                if (__idhinhthuc == 3) {
                    chietKhauPhanTram_BanBuon = dataSource[i].chietKhauPhanTram_BanBuon;
                    chietKhauPhanTram_BanLe = dataSource[i].chietKhauPhanTram_BanLe;
                    soLuongDatKM_Den = dataSource[i].soLuongDatKM_Den;
                    soLuongDatKM_Tu = dataSource[i].soLuongDatKM_Tu;
                }
                //Mua sản phẩm - đạt số lượng - tặng sản phẩm
                if (__idhinhthuc == 4) {
                    soLuongDatKM_Den = dataSource[i].soLuongDatKM_Den;
                    soLuongDatKM_Tu = dataSource[i].soLuongDatKM_Tu;
                }
                //Mua sản phẩm - đạt số lượng - tặng tiền
                if (__idhinhthuc == 5) {
                    soLuongDatKM_Den = dataSource[i].soLuongDatKM_Den;
                    soLuongDatKM_Tu = dataSource[i].soLuongDatKM_Tu;
                    chietKhauTien_BanBuon = dataSource[i].chietKhauTien_BanBuon;
                    chietKhauTien_BanLe = dataSource[i].chietKhauTien_BanLe;
                }
                //Mua sản phẩm - đạt số tiền - chiết khấu SP (%)
                if (__idhinhthuc == 6) {
                    chietKhauPhanTram_BanBuon = dataSource[i].chietKhauPhanTram_BanBuon;
                    chietKhauPhanTram_BanLe = dataSource[i].chietKhauPhanTram_BanLe;
                    tongTienDatKM_Den = dataSource[i].tongTienDatKM_Den;
                    tongTienDatKM_Tu = dataSource[i].tongTienDatKM_Tu;
                }
                //Mua sản phẩm - đạt số tiền - tặng sản phẩm
                if (__idhinhthuc == 7) {
                    tongTienDatKM_Den = dataSource[i].tongTienDatKM_Den;
                    tongTienDatKM_Tu = dataSource[i].tongTienDatKM_Tu;
                }
                //Mua sản phẩm - đạt số tiền - tặng tiền
                if (__idhinhthuc == 8) {
                    tongTienDatKM_Den = dataSource[i].tongTienDatKM_Den;
                    tongTienDatKM_Tu = dataSource[i].tongTienDatKM_Tu;
                    chietKhauTien_BanBuon = dataSource[i].chietKhauTien_BanBuon;
                    chietKhauTien_BanLe = dataSource[i].chietKhauTien_BanLe;
                }

                let obj = {
                    ID_CTKM: __idkhuyenmai,
                    ID_Hang: dataSource[i].iD_Hang,
                    ChietKhauPhanTram_BanBuon: chietKhauPhanTram_BanBuon,
                    ChietKhauTien_BanBuon: chietKhauTien_BanBuon,
                    ChietKhauPhanTram_BanLe: chietKhauPhanTram_BanLe,
                    ChietKhauTien_BanLe: chietKhauTien_BanLe,
                    SoLuongDatKM_Den: soLuongDatKM_Den,
                    SoLuongDatKM_Tu: soLuongDatKM_Tu,
                    TongTienDatKM_Den: tongTienDatKM_Den,
                    TongTienDatKM_Tu: tongTienDatKM_Tu,
                    ApDungBoiSo: ($scope.luyke) ? 1 : 0
                }
                DataChiTiet.push(obj);
            }
        }
        if (__idhinhthuc == 4 || __idhinhthuc == 7)
            DanhSachHangTang = __arrhangtangall;


        if (flag && ($scope.objectctkm.tenCTKM == '' || $scope.objectctkm.tenCTKM == undefined)) {
            flag = false;
            msg = $.i18n('label_tenchuongtrinhkhuyenmaikhongduocdetrong');
            $("#tenctkm").focus();
        }
        if ($scope.obj_TuNgay == null && flag) {
            flag = false;
            msg = $.i18n('label_tungaykhongduocdetrong');
            $("#tungay").focus();
        }
        if ($scope.obj_DenNgay == null && flag) {
            flag = false;
            msg = $.i18n('label_denngaykhongduocdetrong');
            $("#denngay").focus();
        }
        if (__idhinhthuc <= 0 && flag) {
            flag = false;
            msg = $.i18n('label_loaikhuyenmaikhongduocdetrong');
        }

        //Tổng tiền hàng - chiết khấu đơn hàng (%) - tặng tiền
        if (__idhinhthuc == 9 || __idhinhthuc == 10 || __idhinhthuc == 11) {
            let tu = ($scope.objectctkm.tongTienDatKM_Tu == 0) ? 0 : $scope.objectctkm.tongTienDatKM_Tu;
            let den = ($scope.objectctkm.tongTienDatKM_Den == 0) ? 999999999999999 : $scope.objectctkm.tongTienDatKM_Den;
            if (flag && tu > den) {
                flag = false;
                msg = $.i18n('label_tonggiatridonhangtoidakhongnhohontonggiatridonhangtoithieu');
            }
        }

        if (!flag)
            Notification({ title: $.i18n("label_thongbao"), message: msg }, 'warning');
        else {
            let chietKhauTien = 0;
            let chietKhauPhanTram = 0;
            let tongTienDatKM_Den = 0;
            let tongTienDatKM_Tu = 0;

            //Chiết khấu đơn hàng (%)
            if (__idhinhthuc == 1) {
                chietKhauTien = $scope.objectctkm.chietKhauTien;
                chietKhauPhanTram = $scope.objectctkm.chietKhauPhanTram;
            }
            //Tổng tiền hàng - chiết khấu đơn hàng (%)
            if (__idhinhthuc == 9) {
                chietKhauPhanTram = $scope.objectctkm.chietKhauPhanTram;
                tongTienDatKM_Den = $scope.objectctkm.tongTienDatKM_Den;
                tongTienDatKM_Tu = $scope.objectctkm.tongTienDatKM_Tu;
            }
            //Tổng tiền hàng - tặng sản phẩm
            if (__idhinhthuc == 10) {
                tongTienDatKM_Den = $scope.objectctkm.tongTienDatKM_Den;
                tongTienDatKM_Tu = $scope.objectctkm.tongTienDatKM_Tu;
            }
            //Tổng tiền hàng - tặng tiền
            if (__idhinhthuc == 11) {
                chietKhauTien = $scope.objectctkm.chietKhauTien;
                tongTienDatKM_Den = $scope.objectctkm.tongTienDatKM_Den;
                tongTienDatKM_Tu = $scope.objectctkm.tongTienDatKM_Tu;
            }

            $scope.object = {
                KhuyenMaiObj: {
                    ID_NhanVien: 0,
                    ID_CTKM: __idkhuyenmai,
                    TenCTKM: $scope.objectctkm.tenCTKM,
                    Loai: __idhinhthuc,
                    GhiChu: $scope.objectctkm.ghiChu,
                    NgayApDung: kendo.toString($scope.obj_TuNgay, formatDateTimeFilter),
                    NgayKetThuc: kendo.toString($scope.obj_DenNgay, formatDateTimeFilter),
                    ChietKhauTien: chietKhauTien,
                    ChietKhauPhanTram: chietKhauPhanTram,
                    TongTienDatKM_Den: tongTienDatKM_Den,
                    TongTienDatKM_Tu: tongTienDatKM_Tu,
                    ChiTietCTKM: DataChiTiet
                },
                DanhSachHangTang: DanhSachHangTang,
                url_img: image_url
            };
        }

        return flag;
    }

    function inittreenhomchon() {
        let dataSource = new kendo.data.HierarchicalDataSource({
            data: [],
            schema: {
                model: {
                    children: "childs"
                }
            }
        })

        $("#treenhomhang").kendoTreeView({
            dataSource: dataSource,
            dataTextField: "name",
            dataValueField: "id",
            select: onSelectNhomHangTang,
        });
    }
    function loadtreenhomchon() {
        khuyenMaiDataService.getListNhomMatHang().then(function (result) {
            setDataTreenhomchon(result.data);
        });
    }
    function setDataTreenhomchon(data) {
        let dataSource = new kendo.data.HierarchicalDataSource({
            data: data,
            schema: {
                model: {
                    children: "childs"
                }
            }
        })

        let tree = $("#treenhomhang").data("kendoTreeView");
        tree.setDataSource(dataSource);
        tree.expand(".k-item");
        tree.select(".k-first");

        let selectedNode = tree.select();
        __idnhommathangchon = tree.dataItem(selectedNode).id;
        loadgridhangtangchon();
    }
    function listColumnsgridhangtangchon() {
        let dataList = [];

        dataList.push({
            field: "chiTiet", title: $.i18n("header_them"),
            template: '<button ng-click="addrowhangtang()" class="btn btn-link btn-menubar" title ="' + $.i18n("button_themhang") + '" ><i class="fas fa-plus-circle fas-sm color-infor"></i></button> ',
            attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "70px"
        });
        dataList.push({ field: "maHang", title: $.i18n("header_mahang"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px" });
        dataList.push({ field: "tenHang", title: $.i18n("header_tenhang"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "220px" });
        dataList.push({ field: "tenDonVi", title: $.i18n("header_donvi"), attributes: { class: "text-center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "80px" });
        dataList.push({ field: "giaBuon", title: $.i18n("header_giabanbuon"), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), attributes: { class: "text-right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "120px" });
        dataList.push({ field: "giaLe", title: $.i18n("header_giabanle"), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), attributes: { class: "text-right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "120px" });
        dataList.push({ field: "tenDanhMuc", title: $.i18n("header_danhmuc"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "220px" });

        return dataList;
    }
    function loadgridhangtangchon() {
        kendo.ui.progress($("#gridHangChon"), true);
        $scope.gridhangtangchonOptions = {
            sortable: true,
            height: function () {
                return 300;
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
            columns: listColumnsgridhangtangchon()
        };

        khuyenMaiDataService.getlisthangtangchon(__idnhommathangchon).then(function (result) {
            let arr_hangtangchonall = result.data;
            let arr_hangtangchonchuachon = arr_hangtangchonall.filter((item) => {
                return (__arridhangtangone.indexOf(item.idMatHang) == -1)
            })

            $scope.gridhangtangchonData = {
                data: arr_hangtangchonchuachon,
                schema: {
                    model: {
                        id: 'idMatHang',
                        fields: {
                            iD_Hang: {
                                type: "number"
                            }
                        }
                    }
                },
                pageSize: 20
            };
            kendo.ui.progress($("#gridHangChon"), false);
        });
    }
    function onSelectNhomHangTang(e) {
        __idnhommathangchon = $("#treenhomhang").getKendoTreeView().dataItem(e.node).id;
        loadgridhangtangchon();
    }

    function listColumnsgridtang() {
        let dataList = [];

        dataList.push({
            field: "xoa", title: $.i18n("header_xoa"),
            template: '<button ng-click="deleterowhangtang()" class="btn btn-link btn-menubar" title ="' + $.i18n("header_xoa") + '" ><i class="fas fa-trash fas-sm color-danger"></i></button> ',
            attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "50px"
        });
        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({ field: "maHangTang", title: $.i18n("header_mahang"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px" });
        dataList.push({ field: "tenHangTang", title: $.i18n("header_tenhang"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px" });

        dataList.push({
            field: "soLuong", title: $.i18n("header_soluong"),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            attributes: { style: "text-align: center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        });
        dataList.push({
            field: "tenDonVi", title: $.i18n("header_donvi"),
            attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px"
        });

        return dataList;
    }
    function loadgridtang() {
        kendo.ui.progress($("#gridtang"), true);
        $scope.gridtangOptions = {
            sortable: true,
            height: 300,
            dataBinding: function () {
                RecordNumber = (this.dataSource.page() - 1) * this.dataSource.pageSize();
            },
            resizable: true,
            editable: true,
            filterable: {
                mode: "row"
            },
            pageable: $scope.lang == 'vi-vn' ? pageableShort_vi : pageableShort_en,
            columns: listColumnsgridtang()
        };

        let __arrhangtangone = __arrhangtangall.filter((item) => {
            return (item.iD_Hang == __idmathangdetang)
        })

        __arridhangtangone = __arrhangtangone.map((ar, index, arr) => {
            return ar.iD_HangHoa
        })

        $scope.gridtangData = {
            data: __arrhangtangone,
            schema: {
                model: {
                    fields: {
                        iD_Hang: {
                            type: "number",
                            editable: false,
                            nullable: true
                        },
                        xoa: {
                            type: "string",
                            editable: false,
                            nullable: true
                        },
                        maHangTang: {
                            type: "string",
                            editable: false,
                            nullable: true
                        },
                        tenHangTang: {
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
                                min: 1
                            },
                            nullable: false
                        }
                    }
                }
            },
            pageSize: 20
        };
        kendo.ui.progress($("#gridtang"), false);

        loadtreenhomchon();
    }

    function onUploadImageSuccess(e) {
        let data = new FormData();
        data.append('file', e.files[0].rawFile);
        let files = e.files[0];
        if (files.extension.toLowerCase() != ".jpg" && files.extension.toLowerCase() != ".png" && files.extension.toLowerCase() != ".jpeg") {
            e.preventDefault();
            Notification({ title: $.i18n("label_thongbao"), message: $.i18n("label_vuilongchonfileanhjpgpngjpeg") }, 'warning');
        } else {
            khuyenMaiDataService.uploadAnhDaiDien(data).then(function (result) {
                $("#preview").html('<div class="imgprevew"><img src="' + urlApi + result.url + '" style="width:154px;height:179px;max-height:179px;" /></div>')
                image_url = result.url;
                if (!result.flag)
                    Notification({ title: $.i18n("label_thongbao"), message: $.i18n(result.message) }, 'warning');
            })
        }
    }

    //event
    $scope.fromDateChanged = function () {
        $scope.minDate = $scope.obj_TuNgay;
    }
    $scope.toDateChanged = function () {
        $scope.maxDate = $scope.obj_DenNgay;
    }
    $scope.loaikhuyenmaiOnChange = function () {
        $scope.loaikhuyenmaiselect = this.loaikhuyenmaiselect;

        __idhinhthuc = $scope.loaikhuyenmaiselect.iD_HinhThucKM;

        showdetailkhuyenmai();
    }

    $scope.capnhatall = function () {
        let myGridAdd = $("#gridChiTiet").data("kendoGrid");
        let dataSource = myGridAdd.dataSource.data();
        let data = [];

        let chietKhauTien_BanBuon = $scope.chietkhautienbanbuon * 1;
        let chietKhauTien_BanLe = $scope.chietkhautienbanle * 1;
        let chietKhauPhanTram_BanBuon = $scope.chietkhauphantrambanbuon * 1;
        let chietKhauPhanTram_BanLe = $scope.chietkhauphantrambanle * 1;

        if (dataSource.length > 0) {
            for (i = 0; i < dataSource.length; i++) {
                let item = {
                    apDungBoiSo: dataSource[i].apDungBoiSo,
                    chietKhauPhanTram_BanBuon: chietKhauPhanTram_BanBuon,
                    chietKhauPhanTram_BanLe: chietKhauPhanTram_BanLe,
                    chietKhauTien_BanBuon: chietKhauTien_BanBuon,
                    chietKhauTien_BanLe: chietKhauTien_BanLe,
                    soLuongHangTang: dataSource[i].soLuongHangTang,
                    donViTinh: dataSource[i].donViTinh,
                    ghiChu: dataSource[i].ghiChu,
                    ghiChuGia: dataSource[i].ghiChuGia,
                    giaBanBuon: dataSource[i].giaBanBuon,
                    giaBanLe: dataSource[i].giaBanLe,
                    iD_CTKM: dataSource[i].iD_CTKM,
                    iD_ChiTietCTKM: dataSource[i].iD_ChiTietCTKM,
                    iD_DANHMUC: dataSource[i].iD_DANHMUC,
                    iD_Hang: dataSource[i].iD_Hang,
                    maHang: dataSource[i].maHang,
                    soLuongDatKM_Den: dataSource[i].soLuongDatKM_Den,
                    soLuongDatKM_Tu: dataSource[i].soLuongDatKM_Tu,
                    tenDanhMuc: dataSource[i].tenDanhMuc,
                    tenMatHang: dataSource[i].tenMatHang,
                    tongTienDatKM_Den: dataSource[i].tongTienDatKM_Den,
                    tongTienDatKM_Tu: dataSource[i].tongTienDatKM_Tu
                }

                data.push(item);
            }

            reloadgrid(data);
        } else
            Notification({ title: $.i18n("label_thongbao"), message: $.i18n("label_chuacomathangdecapnhat") }, 'warning');
    }

    $scope.addrow = function (e) {
        let myGrid = $('#gridHangChon').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;

        let myGridChiTiet = $("#gridChiTiet").data("kendoGrid");
        let item = {
            apDungBoiSo: ($scope.luyke) ? 1 : 0,
            chietKhauPhanTram_BanBuon: $scope.chietkhauphantrambanbuon,
            chietKhauPhanTram_BanLe: $scope.chietkhauphantrambanle,
            chietKhauTien_BanBuon: $scope.chietkhautienbanbuon,
            chietKhauTien_BanLe: $scope.chietkhautienbanle,
            soLuongHangTang: 0,
            donViTinh: dataItem.tenDonVi,
            ghiChu: "",
            ghiChuGia: dataItem.ghiChuGia,
            giaBanBuon: dataItem.giaBanBuon,
            giaBanLe: dataItem.giaBanLe,
            iD_CTKM: __idkhuyenmai,
            iD_ChiTietCTKM: 0,
            iD_DANHMUC: dataItem.iD_DANHMUC,
            iD_Hang: dataItem.iD_Hang,
            maHang: dataItem.maHang,
            soLuongDatKM_Den: 0,
            soLuongDatKM_Tu: 0,
            tenDanhMuc: dataItem.tenDanhMuc,
            tenMatHang: dataItem.tenHang,
            tongTienDatKM_Den: 0,
            tongTienDatKM_Tu: 0
        }
        myGridChiTiet.dataSource.add(item);
        myGrid.dataSource.remove(dataItem);

        __arridhangchitiet.push(dataItem.iD_Hang);
    }
    $scope.deleterow = function (e) {
        let myGrid = $('#gridChiTiet').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;
        myGrid.dataSource.remove(dataItem);

        __arridhangchitiet = __arridhangchitiet.filter(item => item != dataItem.iD_Hang);
    }

    $scope.luukhuyenmai = function () {
        if (validate()) {
            if (__idkhuyenmai > 0)
                khuyenMaiDataService.suakhuyenmai($scope.object).then(function (result) {
                    if (result.flag) {
                        $state.go('khuyenmai');
                        Notification({ title: $.i18n("label_thongbao"), message: $.i18n(result.message) }, 'success');
                    }
                    else
                        Notification({ title: $.i18n("label_thongbao"), message: $.i18n(result.message) }, 'warning');
                })
            else
                khuyenMaiDataService.themkhuyenmai($scope.object).then(function (result) {
                    if (result.flag) {
                        $state.go('khuyenmai');
                        Notification({ title: $.i18n("label_thongbao"), message: $.i18n(result.message) }, 'success');
                    }
                    else
                        Notification({ title: $.i18n("label_thongbao"), message: $.i18n(result.message) }, 'warning');
                })
        }
    }
    $scope.huyluukhuyenmai = function () {
        openConfirm($.i18n("label_bancochacchanmuonhuyluuchuongtrinhkhuyenmaikhong"), 'apdunghuyluukhuyenmai', null, null);
    }
    $scope.apdunghuyluukhuyenmai = function () {
        $state.go('khuyenmai');
    }

    $scope.openformchonhangtang = function () {
        let myGrid = $('#gridChiTiet').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;

        __idmathangdetang = dataItem.iD_Hang;
        __arridhangtangone = [];

        loadgridtang();

        $scope.formdetailhangtang.center().open();
    }

    $scope.addrowhangtang = function (e) {
        let myGrid = $('#gridhangtangchon').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;

        let myGridChiTiet = $("#gridtang").data("kendoGrid");
        let item = {
            apDungBoiSo: 0,
            iD_CTKM: 0,
            iD_CTKM1: 0,
            iD_CTKM_ChiTiet: 0,
            iD_ChiTietKM_HangTang: 0,
            iD_Hang: __idmathangdetang,
            iD_HangHoa: dataItem.idMatHang,
            insertedTime: "",
            maHang: "",
            maHangTang: dataItem.maHang,
            soLuong: 1,
            soLuongDatKM_Den: 0,
            soLuongDatKM_Tu: 0,
            tenCTKM: "",
            tenDonVi: dataItem.tenDonVi,
            tenHang: "",
            tenHangTang: dataItem.tenHang,
            tongTien: 0,
            tongTienDatKM_Den: 0,
            tongTienDatKM_Tu: 0,
        }

        myGridChiTiet.dataSource.add(item);
        myGrid.dataSource.remove(dataItem);

        __arridhangtangone.push(dataItem.idMatHang);

    }
    $scope.deleterowhangtang = function (e) {
        let myGrid = $('#gridtang').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;

        let myGridAdd = $("#gridhangtangchon").data("kendoGrid");
        let item = {
            anhDaiDien: '',
            danhSachAnh: '',
            ghiChuGia: "",
            giaBuon: -1,
            giaLe: -1,
            iD_DANHMUC: 0,
            iD_NhaCungCap: 0,
            iD_NhanHieu: 0,
            idDonVi: 0,
            idMatHang: dataItem.iD_HangHoa,
            idqllh: 1,
            khuyenMai: "",
            linkGioiThieu: '',
            maHang: dataItem.maHangTang,
            moTa: '',
            soLuong: 0,
            soLuongDieuChuyenKho: 0,
            soLuongTon: 0,
            tenDanhMuc: "",
            tenDonVi: dataItem.tenDonVi,
            tenHang: dataItem.tenHangTang,
        }
        myGridAdd.dataSource.add(item);

        myGrid.dataSource.remove(dataItem);

        __arridhangtangone = __arridhangtangone.filter(item => item != dataItem.iD_HangHoa);
    }

    $scope.luuhangtang = function () {
        __arrhangtangall = __arrhangtangall.filter((item) => {
            return (item.iD_Hang != __idmathangdetang)
        })

        let myGridAdd = $("#gridtang").data("kendoGrid");
        let dataSource = myGridAdd.dataSource.data();

        for (i = 0; i < dataSource.length; i++) {
            let obj = {
                apDungBoiSo: 0,
                iD_CTKM: __idkhuyenmai,
                iD_CTKM1: __idkhuyenmai,
                iD_CTKM_ChiTiet: 0,
                iD_ChiTietKM_HangTang: 0,
                iD_Hang: __idmathangdetang,
                iD_HangHoa: dataSource[i].iD_HangHoa,
                soLuong: dataSource[i].soLuong,
                insertedTime: "",
                maHang: "",
                maHangTang: dataSource[i].maHangTang,
                soLuongDatKM_Den: 0,
                soLuongDatKM_Tu: 0,
                tenCTKM: "",
                tenDonVi: dataSource[i].tenDonVi,
                tenHang: "",
                tenHangTang: dataSource[i].tenHangTang,
                tongTien: 0,
                tongTienDatKM_Den: 0,
                tongTienDatKM_Tu: 0,
            }
            __arrhangtangall.push(obj);
        }

        reloadgrid();

        $scope.formdetailhangtang.center().close();
    }
    $scope.huyluuhangtang = function () {
        $scope.formdetailhangtang.center().close();
    }

    init();
})