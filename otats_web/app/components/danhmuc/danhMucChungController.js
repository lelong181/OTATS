angular.module('app').controller('danhMucChungController', function ($scope, $rootScope, $location, $timeout, Notification, ComboboxDataService, danhMucDataService, donHangDataService) {
    CreateSiteMap();

    let iddanhmuc = 1;

    let idloaikhachhang = 0;
    let iddonvi = 0;
    let idkenhbanhang = 0;
    let idkenhbanhangcapcha = 0;
    let idnganhhang = 0;
    let idnganhhangcapcha = 0;
    let idloaihaohut = 0;
    let idtrangthaidonhang = 0;
    let idloaitrangthaidonhang = 0;
    let idchecklist = 0;
    let idphanhoi = 0;
    let idnhanhieu = 0;
    let idnhacungcap = 0;

    function init() {
        getquyen();
        inittreeview();

        initcomboBox();
        $scope.gridOption_LichSuNapVi = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - 200;
                return heightGrid - 45;
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
            columns: listLSNVColumnsgrid()
        };
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

    function initcomboBox() {
        let arr = [
            { value: $.i18n('label_khoitaodonhang'), id_loai: 1 },
            { value: $.i18n('button_hoantat'), id_loai: 2 },
            { value: $.i18n('header_xulydonhang'), id_loai: 3 },
        ]
        $scope.loaiData = arr;
        $scope.loaiData1 = arr;

        $("#IconHienThi").kendoDropDownList({
            dataTextField: "text",
            dataValueField: "value",
            dataSource: [
                { text: "Icon 01", value: "/assets/img/iconloaikhachhang/01.png" },
                { text: "Icon 02", value: "/assets/img/iconloaikhachhang/02.png" },
                { text: "Icon 03", value: "/assets/img/iconloaikhachhang/03.png" },
                { text: "Icon 04", value: "/assets/img/iconloaikhachhang/04.png" },
                { text: "Icon 05", value: "/assets/img/iconloaikhachhang/05.png" },
                { text: "Icon 06", value: "/assets/img/iconloaikhachhang/06.png" },
                { text: "Icon 07", value: "/assets/img/iconloaikhachhang/07.png" },
                { text: "Icon 08", value: "/assets/img/iconloaikhachhang/08.png" }
            ],
            template: '<span style="display:block;width:20px;height:20px;background-image: url(\'../#:data.value#\')"></span>',
            valueTemplate: '<span style="display:block;float:left;width:20px;height:20px;background-image: url(\'../#:data.value#\')"></span>',
            filter: false,
            suggest: false
        });
    }

    function initkenhbanhangcapcha(_idkenh, _parentid) {
        danhMucDataService.comboDataKenhBanHang().then(function (result) {
            let data = result.data;
            let re = data.filter((item) => { return (item.iD_KenhBanHang != _idkenh) })
            $scope.kenhHangData = re;

            if (_parentid > 0)
                $timeout(function () { $("#kenhHang").data("kendoComboBox").value(_parentid); }, 100);
            else
                $("#kenhHang").data("kendoComboBox").value("")
        });
    }
    function initnganhhangcapcha(_idnganh, _parentid) {
        danhMucDataService.getListNganhHang().then(function (result) {
            let data = result.data;
            let re = data.filter((item) => { return (item.iD_DANHMUC != _idnganh) })
            $scope.nganhHangData = re;

            if (_parentid > 0)
                $timeout(function () { $("#nganhHang").data("kendoComboBox").value(_parentid); }, 100);
            else
                $("#nganhHang").data("kendoComboBox").value("")
        });
    }

    function inittreeview() {
        let data = [
            { id: 1, name: $.i18n('label_danhmucloaikhachhang'), spriteCssClass: "fas fa-folder" },
            { id: 2, name: $.i18n('label_danhmucdonvitinh'), spriteCssClass: "fas fa-folder" },
            { id: 3, name: $.i18n('label_danhmuckenhbanhang'), spriteCssClass: "fas fa-folder" },
            { id: 4, name: $.i18n('label_danhmucnganhhang'), spriteCssClass: "fas fa-folder" },
            { id: 5, name: $.i18n('label_danhmucloaihaohut'), spriteCssClass: "fas fa-folder" },
            { id: 6, name: $.i18n('label_danhmuctrangthaidonhang'), spriteCssClass: "fas fa-folder" },
            { id: 7, name: $.i18n('label_danhmucchecklist'), spriteCssClass: "fas fa-folder" },
            { id: 8, name: $.i18n('label_danhmucphanhoikhachhang'), spriteCssClass: "fas fa-folder" },
            { id: 9, name: $.i18n('label_danhmucnhanhieu'), spriteCssClass: "fas fa-folder" },
            { id: 10, name: $.i18n('label_danhmucnhacungcap'), spriteCssClass: "fas fa-folder" },
            { id: 11, name: $.i18n('label_danhmuchinhthucthanhtoan'), spriteCssClass: "fas fa-folder" }
        ]

        let dataSource = new kendo.data.HierarchicalDataSource({
            data: data,
            schema: {
                model: {
                    children: "childs"
                }
            }
        })

        $("#treedanhmuc").kendoTreeView({
            dataSource: dataSource,
            dataTextField: "name",
            dataValueField: "id",
            select: onSelectDanhMuc,
        });
        let tree = $("#treedanhmuc").data("kendoTreeView");
        tree.expand(".k-item");
        tree.select(".k-first");

        let selectedNode = tree.select();
        iddanhmuc = tree.dataItem(selectedNode).id;
        loadgrid();
    }
    function onSelectDanhMuc(e) {
        iddanhmuc = $("#treedanhmuc").getKendoTreeView().dataItem(e.node).id;
        loadgrid();
    }

    function listColumnsgrid() {
        let dataList = [];

        dataList.push({ headerAttributes: { "class": "table-header-cell" }, selectable: true, width: "40px" });
        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        //loại khách hàng
        dataList.push({ field: "tenLoaiKhachHang", title: $.i18n('header_tenloaikhachhang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid });
        //trạng thái đơn hàng
        dataList.push({ field: "tenTrangThai", title: $.i18n('header_tentrangthai'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid });
        dataList.push({
            field: "mauTrangThai", title: $.i18n('header_mau'),
            template: function (dataItem) {
                return '<button class="btn btn-lg" style="background-color:' + dataItem.mauTrangThai + '"></button>'
            },
            attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "120px"
        });
        dataList.push({
            field: "macDinh", title: $.i18n('header_khoitao'),
            template: function (dataItem) {
                if (dataItem.macDinh == 0)
                    return '<button class="btn btn-link btn-menubar" ><i class="far fa-square fas-sm color-primary"></i></button> ';
                else
                    return '<button class="btn btn-link btn-menubar" ><i class="far fa-check-square fas-sm color-primary"></i></button> ';
            },
            attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "120px"
        });
        dataList.push({
            field: "ketThuc", title: $.i18n('header_ketthuc'),
            template: function (dataItem) {
                if (dataItem.ketThuc == 0)
                    return '<button class="btn btn-link btn-menubar" ><i class="far fa-square fas-sm color-primary"></i></button> ';
                else
                    return '<button class="btn btn-link btn-menubar" ><i class="far fa-check-square fas-sm color-primary"></i></button> ';
            },
            attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "120px"
        });
        dataList.push({
            field: "xuLy", title: $.i18n('header_xulydonhang'),
            template: function (dataItem) {
                if (dataItem.xuLy == 0)
                    return '<button class="btn btn-link btn-menubar" ><i class="far fa-square fas-sm color-primary"></i></button> ';
                else
                    return '<button class="btn btn-link btn-menubar" ><i class="far fa-check-square fas-sm color-primary"></i></button> ';
            },
            attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "120px"
        });
        //loại hao hụt
        dataList.push({ field: "maLoaiHaoHut", title: $.i18n('header_maloaihaohut'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({ field: "tenLoaiHaoHut", title: $.i18n('header_tenloaihaohut'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "250px" });
        dataList.push({ field: "tiLe", title: $.i18n('header_tile'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "130px" });
        dataList.push({ field: "ghiChu", title: $.i18n('header_ghichu'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid });
        //phản hồi khách hàng
        dataList.push({ field: "tenPhanHoi", title: $.i18n('header_tenphanhoi'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid });
        //kênh bán hàng
        dataList.push({ field: "tenKenhBanHang", title: $.i18n('header_tenkenhbanhang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid });
        dataList.push({ field: "tenKenhCapTren", title: $.i18n('header_kenhbanhangcapcha'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid });
        //checklist
        dataList.push({ field: "tenCheckList", title: $.i18n('header_tenloaichecklist'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid });
        //đơn vị tính
        dataList.push({ field: "tenDonVi", title: $.i18n('header_tendonvitinh'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid });
        //ngành hàng
        dataList.push({ field: "tenDanhMuc", title: $.i18n('header_tennganhhang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid });
        dataList.push({ field: "tenNganhHangCapCha", title: $.i18n('header_nganhhangcapcha'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid });

        //Nhãn hiệu
        dataList.push({ field: "tenNhanHieu", title: $.i18n('header_tennhanhieu'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid });
        //Nhà cung cấp
        dataList.push({ field: "tenNhaCungCap", title: $.i18n('header_tennhacungcap'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({ field: "dienThoaiLienHe", title: $.i18n('header_dienthoai'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({ field: "nguoiLienHe", title: $.i18n('header_nguoilienhe'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({ field: "diaChi", title: $.i18n('header_ghichu'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid });
        dataList.push({ field: "profileCode", title: $.i18n('header_profilecode'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid });
        dataList.push({ field: "profileID", title: $.i18n('header_profileid'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid });
        dataList.push({ field: "paymentTypeName", title: $.i18n('header_tenhttt'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid });
        dataList.push({ field: "paymentTypeID", title: $.i18n('header_idhttt'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid });
        dataList.push({ field: "accountReceivableNo", title: $.i18n('header_taikhoannaptruoc'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid });
        dataList.push({
            field: "iD_NhaCungCap", title: "Thao tác", headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid,
            template: function (e) {
                return '<button ng-click="openformchitietvi(' + e.iD_NhaCungCap + ')" class="btn btn-link btn-menubar" title ="Chi tiết ví" ><i class="fas fa-plus-circle fas-sm color-infor"></i> Chi tiết ví</button> '
            },
        });

        //hình thức thanh toán
        dataList.push({ field: "name", title: $.i18n('header_tenhinhthucthanhtoan'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid });

        return dataList;
    }
    function loadgrid() {
        kendo.ui.progress($("#grid"), true);
        $scope.gridOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".toolbarmenu").height());
                return heightGrid - 45;
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

        if (iddanhmuc == 1) {
            danhMucDataService.getLoaiKhachHang().then(function (result) {
                $scope.gridData = {
                    data: result.data,
                    schema: {
                        model: {
                            id: "iD_LoaiKhachHang"
                        }
                    },
                    pageSize: 20
                };
                kendo.ui.progress($("#grid"), false);
                showcolumschitiet();
            });
        }
        else if (iddanhmuc == 2) {
            danhMucDataService.getDonViTinh().then(function (result) {
                $scope.gridData = {
                    data: result.data,
                    schema: {
                        model: {
                        }
                    },
                    pageSize: 20
                };
                kendo.ui.progress($("#grid"), false);
                showcolumschitiet();
            });
        }
        else if (iddanhmuc == 3) {
            danhMucDataService.getKenhBanHang().then(function (result) {
                $scope.gridData = {
                    data: result.data,
                    schema: {
                        model: {
                        }
                    },
                    pageSize: 20
                };
                kendo.ui.progress($("#grid"), false);
                showcolumschitiet();
            });
        }
        else if (iddanhmuc == 4) {
            danhMucDataService.getListNganhHang().then(function (result) {
                $scope.gridData = {
                    data: result.data,
                    schema: {
                        model: {
                        }
                    },
                    pageSize: 20
                };
                kendo.ui.progress($("#grid"), false);
                showcolumschitiet();
            });
        }
        else if (iddanhmuc == 5) {
            danhMucDataService.getLoaiHaoHut().then(function (result) {
                $scope.gridData = {
                    data: result.data,
                    schema: {
                        model: {
                        }
                    },
                    pageSize: 20
                };
                kendo.ui.progress($("#grid"), false);
                showcolumschitiet();
            });
        }
        else if (iddanhmuc == 6) {
            danhMucDataService.getTrangThaiDonHang().then(function (result) {
                $scope.gridData = {
                    data: result.data,
                    schema: {
                        model: {
                        }
                    },
                    pageSize: 20
                };
                kendo.ui.progress($("#grid"), false);
                showcolumschitiet();
            });
        }
        else if (iddanhmuc == 7) {
            danhMucDataService.getCheckList().then(function (result) {
                $scope.gridData = {
                    data: result.data,
                    schema: {
                        model: {
                        }
                    },
                    pageSize: 20
                };
                kendo.ui.progress($("#grid"), false);
                showcolumschitiet();
            });
        }
        else if (iddanhmuc == 8) {
            danhMucDataService.getPhanHoiKhachHang().then(function (result) {
                $scope.gridData = {
                    data: result.data,
                    schema: {
                        model: {
                        }
                    },
                    pageSize: 20
                };
                kendo.ui.progress($("#grid"), false);
                showcolumschitiet();
            });
        }
        else if (iddanhmuc == 9) {
            danhMucDataService.getNhanHieu().then(function (result) {
                $scope.gridData = {
                    data: result.data,
                    schema: {
                        model: {
                        }
                    },
                    pageSize: 20
                };
                kendo.ui.progress($("#grid"), false);
                showcolumschitiet();
            });
        }
        else if (iddanhmuc == 10) {
            danhMucDataService.getNhaCungCap().then(function (result) {
                $scope.gridData = {
                    data: result.data,
                    schema: {
                        model: {
                        }
                    },
                    pageSize: 20
                };
                kendo.ui.progress($("#grid"), false);
                showcolumschitiet();
            });
        }
        else if (iddanhmuc === 11) {
            danhMucDataService.getHinhThucThanhToan().then(function (result) {
                $scope.gridData = {
                    data: result.data,
                    schema: {
                        model: {
                        }
                    },
                    pageSize: 20
                };
                kendo.ui.progress($("#grid"), false);
                showcolumschitiet();
            });
        }
    }
    function showcolumschitiet() {
        let grid = $("#grid").data("kendoGrid");

        if (iddanhmuc == 1) {
            grid.showColumn("tenLoaiKhachHang");
        }
        else {
            grid.hideColumn("tenLoaiKhachHang");
        }

        if (iddanhmuc == 2) {
            grid.showColumn("tenDonVi");
        }
        else {
            grid.hideColumn("tenDonVi");
        }

        if (iddanhmuc == 3) {
            grid.showColumn("tenKenhBanHang");
            grid.showColumn("tenKenhCapTren");
        }
        else {
            grid.hideColumn("tenKenhBanHang");
            grid.hideColumn("tenKenhCapTren");
        }

        if (iddanhmuc == 4) {
            grid.showColumn("tenDanhMuc");
            grid.showColumn("tenNganhHangCapCha");
        }
        else {
            grid.hideColumn("tenDanhMuc");
            grid.hideColumn("tenNganhHangCapCha");
        }

        if (iddanhmuc == 5) {
            grid.showColumn("maLoaiHaoHut");
            grid.showColumn("tenLoaiHaoHut");
            grid.showColumn("tiLe");
            grid.showColumn("ghiChu");
        } else {
            grid.hideColumn("maLoaiHaoHut");
            grid.hideColumn("tenLoaiHaoHut");
            grid.hideColumn("tiLe");
            grid.hideColumn("ghiChu");
        }

        if (iddanhmuc == 6) {
            grid.showColumn("tenTrangThai");
            grid.showColumn("mauTrangThai");
            grid.showColumn("macDinh");
            grid.showColumn("ketThuc");
            grid.showColumn("xuLy");
        }
        else {
            grid.hideColumn("tenTrangThai");
            grid.hideColumn("mauTrangThai");
            grid.hideColumn("macDinh");
            grid.hideColumn("ketThuc");
            grid.hideColumn("xuLy");
        }

        if (iddanhmuc == 7) {
            grid.showColumn("tenCheckList");
        }
        else {
            grid.hideColumn("tenCheckList");
        }

        if (iddanhmuc == 8) {
            grid.showColumn("tenPhanHoi");
        }
        else {
            grid.hideColumn("tenPhanHoi");
        }

        if (iddanhmuc == 9) {
            grid.showColumn("tenNhanHieu");
        }
        else {
            grid.hideColumn("tenNhanHieu");
        }

        if (iddanhmuc == 10) {
            grid.showColumn("tenNhaCungCap");
            grid.showColumn("dienThoaiLienHe");
            grid.showColumn("nguoiLienHe");
            grid.showColumn("diaChi");
            grid.showColumn("paymentTypeID");
            grid.showColumn("paymentTypeName");
            grid.showColumn("profileCode");
            grid.showColumn("profileID");
            grid.showColumn("accountReceivableNo");
            grid.showColumn("iD_NhaCungCap");
        }
        else {
            grid.hideColumn("tenNhaCungCap");
            grid.hideColumn("dienThoaiLienHe");
            grid.hideColumn("nguoiLienHe");
            grid.hideColumn("diaChi");
            grid.hideColumn("paymentTypeID");
            grid.hideColumn("paymentTypeName");
            grid.hideColumn("profileCode");
            grid.hideColumn("profileID");
            grid.hideColumn("accountReceivableNo");
            grid.hideColumn("iD_NhaCungCap");
        }

        if (iddanhmuc === 11) {
            grid.showColumn("name");
        }
        else {
            grid.hideColumn("name");
        }

        grid.refresh();
    }
    function showdetail() {
        $scope.show1 = (iddanhmuc == 1);
        $scope.show2 = (iddanhmuc == 2);
        $scope.show3 = (iddanhmuc == 3);
        $scope.show4 = (iddanhmuc == 4);
        $scope.show5 = (iddanhmuc == 5);
        $scope.show6 = (iddanhmuc == 6);
        $scope.show7 = (iddanhmuc == 7);
        $scope.show8 = (iddanhmuc == 8);
        $scope.show9 = (iddanhmuc == 9);
        $scope.show10 = (iddanhmuc == 10);
        $scope.show11 = (iddanhmuc == 11);
    }

    function validationOpenDetail(listRowsSelected) {
        let flag = true;
        let msg = "";
        if (listRowsSelected.length > 1) {
            msg = $.i18n('label_canchonmotdongdethuchien');
            flag = false;
        } else if (listRowsSelected.length == 0) {
            flag = false;
            msg = $.i18n('label_chuachondongdethuchien');
        }
        if (!flag) {
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n(msg) }, "error");
        }
        return flag;
    }

    function openEditDetailFromGrid(data) {
        $scope.$apply(function () {
            $scope.formdetail.center().open();
            if (iddanhmuc == 1) {
                $scope.id = data.iD_LoaiKhachHang;
                idloaikhachhang = data.iD_LoaiKhachHang;
                $scope.tenLoaiKhachHang = data.tenLoaiKhachHang;
                $("#IconHienThi").data("kendoDropDownList").value(data.iconHienThi);
            } else if (iddanhmuc == 2) {
                $scope.id = data.iD_DonVi;
                iddonvi = data.iD_DonVi;
                $scope.tenDonVi = data.tenDonVi;
            }
            else if (iddanhmuc == 3) {
                $scope.id = data.iD_KenhBanHang;
                idkenhbanhang = data.iD_KenhBanHang;
                idkenhbanhangcapcha = data.iD_KenhCapTren;
                $scope.tenKenhBanHang = data.tenKenhBanHang;

                initkenhbanhangcapcha(idkenhbanhang, idkenhbanhangcapcha);
            }
            else if (iddanhmuc == 4) {
                $scope.id = data.iD_DANHMUC;
                idnganhhang = data.iD_DANHMUC;
                idnganhhangcapcha = data.iD_PARENT;
                $scope.tenNganhHang = data.tenDanhMuc;

                initnganhhangcapcha(idnganhhang, idnganhhangcapcha);
            }
            else if (iddanhmuc == 5) {
                $scope.id = data.id;
                idloaihaohut = data.id;
                $scope.tenHaoHut = data.tenLoaiHaoHut;
                $scope.maHaoHut = data.maLoaiHaoHut;
                $scope.tyLeHaoHut = data.tiLe;
                $scope.ghiChuHaoHut = data.ghiChu;
            }
            else if (iddanhmuc == 6) {
                $scope.id = data.iD_TrangThaiDonHang;
                idtrangthaidonhang = data.iD_TrangThaiDonHang;
                idloaitrangthaidonhang = data.loaiTrangThai;
                $scope.tenTrangThaiDonHang = data.tenTrangThai;
                $scope.color = data.mauTrangThai;
                $scope.guisms = false;
                $scope.noidungsms = '';
                $scope.guiemail = false;
                $scope.noidungemail = '';

                if (idloaitrangthaidonhang > 0)
                    $("#loai").data("kendoComboBox").value(idloaitrangthaidonhang);
                else
                    $("#loai").data("kendoComboBox").value("")
            }
            else if (iddanhmuc == 7) {
                $scope.id = data.iD_CheckList;
                idchecklist = data.iD_CheckList;
                $scope.tenChecklist = data.tenCheckList;
            }
            else if (iddanhmuc == 8) {
                $scope.id = data.iD_PhanHoi;
                idphanhoi = data.iD_PhanHoi;
                $scope.tenPhanHoi = data.tenPhanHoi;
            }
            else if (iddanhmuc == 9) {
                $scope.id = data.iD_NhanHieu;
                idnhanhieu = data.iD_NhanHieu;
                $scope.tenNhanHieu = data.tenNhanHieu;
            }
            else if (iddanhmuc == 10) {
                $scope.id = data.iD_NhaCungCap;
                idnhacungcap = data.iD_NhaCungCap;
                $scope.tenNhaCungCap = data.tenNhaCungCap;
                $scope.dienThoaiLienHe = data.dienThoaiLienHe;
                $scope.nguoiLienHe = data.nguoiLienHe;
                $scope.diaChi = data.diaChi;
            }
            else if (iddanhmuc === 11) {
                $scope.id = data.id;
                $scope.name = (data.name != null) ? data.name : '';
                $scope.virtualPaymentClientURL = (data.virtualPaymentClientURL != null) ? data.virtualPaymentClientURL : '';
                $scope.version = (data.version != null) ? data.version : '';
                $scope.merchantCode = (data.merchantCode != null) ? data.merchantCode : '';
                $scope.merchantName = (data.merchantName != null) ? data.merchantName : '';
                $scope.serviceCode = (data.serviceCode != null) ? data.serviceCode : '';
                $scope.countryCode = (data.countryCode != null) ? data.countryCode : '';
                $scope.payType = (data.payType != null) ? data.payType : '';
                $scope.ccy = (data.ccy != null) ? data.ccy : '';
                $scope.masterMerCode = (data.masterMerCode != null) ? data.masterMerCode : '';
                $scope.merchantType = (data.merchantType != null) ? data.merchantType : '';
                $scope.terminalId = (data.terminalId != null) ? data.terminalId : '';
                $scope.terminalName = (data.terminalName != null) ? data.terminalName : '';
                $scope.user = (data.user != null) ? data.user : '';
                $scope.password = (data.password != null) ? data.password : '';
                $scope.accessCode = (data.accessCode != null) ? data.accessCode : '';
                $scope.hascode = (data.hascode != null) ? data.hascode : '';
                $scope.currency = (data.currency != null) ? data.currency : '';
                $scope.returnURL = (data.returnURL != null) ? data.returnURL : '';
            }

            showdetail();
        })
    }

    function openEditDetail(data) {
        $scope.formdetail.center().open();
        if (iddanhmuc == 1) {
            $scope.id = data.iD_LoaiKhachHang;
            idloaikhachhang = data.iD_LoaiKhachHang;
            $scope.tenLoaiKhachHang = data.tenLoaiKhachHang;
            $("#IconHienThi").data("kendoDropDownList").value(data.iconHienThi);
        } else if (iddanhmuc == 2) {
            $scope.id = data.iD_DonVi;
            iddonvi = data.iD_DonVi;
            $scope.tenDonVi = data.tenDonVi;
        }
        else if (iddanhmuc == 3) {
            $scope.id = data.iD_KenhBanHang;
            idkenhbanhang = data.iD_KenhBanHang;
            idkenhbanhangcapcha = data.iD_KenhCapTren;
            $scope.tenKenhBanHang = data.tenKenhBanHang;

            initkenhbanhangcapcha(idkenhbanhang, idkenhbanhangcapcha);
        }
        else if (iddanhmuc == 4) {
            $scope.id = data.iD_DANHMUC;
            idnganhhang = data.iD_DANHMUC;
            idnganhhangcapcha = data.iD_PARENT;
            $scope.tenNganhHang = data.tenDanhMuc;

            initnganhhangcapcha(idnganhhang, idnganhhangcapcha);
        }
        else if (iddanhmuc == 5) {
            $scope.id = data.id;
            idloaihaohut = data.id;
            $scope.tenHaoHut = data.tenLoaiHaoHut;
            $scope.maHaoHut = data.maLoaiHaoHut;
            $scope.tyLeHaoHut = data.tiLe;
            $scope.ghiChuHaoHut = data.ghiChu;
        }
        else if (iddanhmuc == 6) {
            $scope.id = data.iD_TrangThaiDonHang;
            idtrangthaidonhang = data.iD_TrangThaiDonHang;
            idloaitrangthaidonhang = data.loaiTrangThai;
            $scope.tenTrangThaiDonHang = data.tenTrangThai;
            $scope.color = data.mauTrangThai;
            $scope.guisms = false;
            $scope.noidungsms = '';
            $scope.guiemail = false;
            $scope.noidungemail = '';

            if (idloaitrangthaidonhang > 0)
                $("#loai").data("kendoComboBox").value(idloaitrangthaidonhang);
            else
                $("#loai").data("kendoComboBox").value("")
        }
        else if (iddanhmuc == 7) {
            $scope.id = data.iD_CheckList;
            idchecklist = data.iD_CheckList;
            $scope.tenChecklist = data.tenCheckList;
        }
        else if (iddanhmuc == 8) {
            $scope.id = data.iD_PhanHoi;
            idphanhoi = data.iD_PhanHoi;
            $scope.tenPhanHoi = data.tenPhanHoi;
        }
        else if (iddanhmuc == 9) {
            $scope.id = data.iD_NhanHieu;
            idnhanhieu = data.iD_NhanHieu;
            $scope.tenNhanHieu = data.tenNhanHieu;
        }
        else if (iddanhmuc == 10) {
            $scope.id = data.iD_NhaCungCap;
            idnhacungcap = data.iD_NhaCungCap;
            $scope.tenNhaCungCap = data.tenNhaCungCap;
            $scope.dienThoaiLienHe = data.dienThoaiLienHe;
            $scope.nguoiLienHe = data.nguoiLienHe;
            $scope.diaChi = data.diaChi;
        }
        else if (iddanhmuc === 11) {
            $scope.id = data.id;
            $scope.name = (data.name != null) ? data.name : '';
            $scope.virtualPaymentClientURL = (data.virtualPaymentClientURL != null) ? data.virtualPaymentClientURL : '';
            $scope.version = (data.version != null) ? data.version : '';
            $scope.merchantCode = (data.merchantCode != null) ? data.merchantCode : '';
            $scope.merchantName = (data.merchantName != null) ? data.merchantName : '';
            $scope.serviceCode = (data.serviceCode != null) ? data.serviceCode : '';
            $scope.countryCode = (data.countryCode != null) ? data.countryCode : '';
            $scope.payType = (data.payType != null) ? data.payType : '';
            $scope.ccy = (data.ccy != null) ? data.ccy : '';
            $scope.masterMerCode = (data.masterMerCode != null) ? data.masterMerCode : '';
            $scope.merchantType = (data.merchantType != null) ? data.merchantType : '';
            $scope.terminalId = (data.terminalId != null) ? data.terminalId : '';
            $scope.terminalName = (data.terminalName != null) ? data.terminalName : '';
            $scope.user = (data.user != null) ? data.user : '';
            $scope.password = (data.password != null) ? data.password : '';
            $scope.accessCode = (data.accessCode != null) ? data.accessCode : '';
            $scope.hascode = (data.hascode != null) ? data.hascode : '';
            $scope.currency = (data.currency != null) ? data.currency : '';
            $scope.returnURL = (data.returnURL != null) ? data.returnURL : '';
        }

        showdetail();
    }

    function openNewDetail() {
        $scope.formdetail.center().open();
        $scope.id = 0;

        if (iddanhmuc == 1) {
            idloaikhachhang = 0;
            $scope.tenLoaiKhachHang = '';
        } else if (iddanhmuc == 2) {
            iddonvi = 0;
            $scope.tenDonVi = '';
        }
        else if (iddanhmuc == 3) {
            idkenhbanhang = 0;
            idkenhbanhangcapcha = 0;
            $scope.tenKenhBanHang = '';
            initkenhbanhangcapcha(idkenhbanhang, idkenhbanhangcapcha);
        }
        else if (iddanhmuc == 4) {
            idnganhhang = 0;
            idnganhhangcapcha = 0;
            $scope.tenNganhHang = '';

            initnganhhangcapcha(idnganhhang, idnganhhangcapcha);
        }
        else if (iddanhmuc == 5) {
            idloaihaohut = 0;
            $scope.tenHaoHut = '';
            $scope.maHaoHut = '';
            $scope.tyLeHaoHut = 0;
            $scope.ghiChuHaoHut = '';
        }
        else if (iddanhmuc == 6) {
            idtrangthaidonhang = 0;
            idloaitrangthaidonhang = 0;
            $scope.tenTrangThaiDonHang = '';
            $scope.color = '#339966';
            $scope.guisms = false;
            $scope.noidungsms = '';
            $scope.guiemail = false;
            $scope.noidungemail = '';
            if (idloaitrangthaidonhang > 0)
                $("#loai").data("kendoComboBox").value(idloaitrangthaidonhang);
            else
                $("#loai").data("kendoComboBox").value("")
        }
        else if (iddanhmuc == 7) {
            idchecklist = 0;
            $scope.tenChecklist = '';
        }
        else if (iddanhmuc == 8) {
            idphanhoi = 0;
            $scope.tenPhanHoi = '';
        }
        else if (iddanhmuc == 9) {
            idnhanhieu = 0;
            $scope.tenNhanHieu = '';
        }
        else if (iddanhmuc == 10) {
            idnhacungcap = 0;
            $scope.tenNhaCungCap = '';
            $scope.dienThoaiLienHe = '';
            $scope.nguoiLienHe = '';
            $scope.diaChi = '';
        }
        else if (iddanhmuc == 11) {
            $scope.id = 0;
            $scope.name = '';
            $scope.virtualPaymentClientURL = '';
            $scope.version = '';
            $scope.merchantCode = '';
            $scope.merchantName = '';
            $scope.serviceCode =  '';
            $scope.countryCode = '';
            $scope.payType = '';
            $scope.ccy = '';
            $scope.masterMerCode = '';
            $scope.merchantType = '';
            $scope.terminalId = '';
            $scope.terminalName = '';
            $scope.user = '';
            $scope.password = '';
            $scope.accessCode = '';
            $scope.hascode = '';
            $scope.currency = '';
            $scope.returnURL = '';
        }

        showdetail();
    }

    function listLSNVColumnsgrid() {
        let dataList = [];

        dataList.push({ headerAttributes: { "class": "table-header-cell" }, selectable: true, width: "40px" });
        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({ field: "ngayTao", format: "{0:dd/MM/yyyy HH:mm}", title: "Ngày", headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid });
        dataList.push({ field: "tenNhaCungCap", title: "Nhà cung cấp", headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid });
        dataList.push({ field: "tenVi", title: "Người nạp", headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid });
        dataList.push({ field: "soDu", format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), title: "Số dư trước nạp", headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid });
        dataList.push({ field: "soTien", format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), title: "Số tiền nạp", headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid });
        dataList.push({ field: "tongSoDu", format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), title: "Tổng số dư", headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid });
        dataList.push({
            field: "imgUrl", title: "Hình ảnh",
            template: function (dataItem) {
                if (dataItem.imgUrl == null || dataItem.imgUrl == '')
                    return ''
                else {
                    return '<img src="' + dataItem.imgUrl + '" alt="" style="max-height:50px;">';
                }
            },
            attributes: { style: "text-align: center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        return dataList;
    }

    function clearNapVi() {
        $scope.napvi = {};
        $("#preview").html("");

    }

    function loadLichSuNapVi(id_ncc) {
        clearNapVi();
        $scope.napvi.ID_NhaCungCap = id_ncc;
        danhMucDataService.getLichSuNapVi($scope.napvi.ID_NhaCungCap).then(function (result) {

            let ds = new kendo.data.DataSource({
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
                            tongSoDu: {
                                type: "number"
                            },
                        }
                    }
                },
                pageSize: 20
            })
            $("#gridLichSuNapVi").data("kendoGrid").setDataSource(ds); 
        });


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
        $("#files").closest(".k-upload").find("span").text($.i18n("label_chonanhchuyenkhoan"));
        $("#preview").kendoTooltip({
            filter: "img",
            position: "right",
            content: function (e) {
                var target = e.target;
                return "<img style='width:280px;' src='" + target[0].currentSrc + "' />"
            }
        }).data("kendoTooltip");

        $("#gridLichSuNapVi").kendoTooltip({
            filter: "img",
            position: "left",
            content: function (e) {
                var target = e.target;
                return "<img style='width:280px;' src='" + target[0].currentSrc + "' />"
            }
        }).data("kendoTooltip");
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
                if (!result.flag)
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
            })
        }
    }

    function openEditTrangThaiDonHang(data) {
        $scope.formdetailtrangthaidonhang.center().open();

        idtrangthaidonhang = data.iD_TrangThaiDonHang;
        idloaitrangthaidonhang = data.loaiTrangThai;
        $scope.tenTrangThaiDonHang1 = data.tenTrangThai;
        $scope.color1 = data.mauTrangThai;
        $scope.guisms = (data.guiSMS == 1) ? true : false;
        $scope.noidungsms = data.sMSTemplate;
        $scope.guiemail = (data.guiEmail == 1) ? true : false;
        $scope.noidungemail = data.emailTemplate;
        if (idloaitrangthaidonhang > 0)
            $("#loai1").data("kendoComboBox").value(idloaitrangthaidonhang);
        else
            $("#loai1").data("kendoComboBox").value("")
    }

    function validatesave() {
        let flag = true;
        let msg = '';

        if (iddanhmuc == 1) {
            if (flag && ($scope.tenLoaiKhachHang == '' || $scope.tenLoaiKhachHang == undefined)) {
                flag = false;
                msg = $.i18n('label_tenloaikhachhangkhongduocdetrong');
                $("#tenLoaiKhachHang").focus();
            }
        } else if (iddanhmuc == 2) {
            if (flag && ($scope.tenDonVi == '' || $scope.tenDonVi == undefined)) {
                flag = false;
                msg = $.i18n('label_tendonvitinhkhongduocdetrong');
                $("#tenDonVi").focus();
            }
        }
        else if (iddanhmuc == 3) {
            if (flag && ($scope.tenKenhBanHang == '' || $scope.tenKenhBanHang == undefined)) {
                flag = false;
                msg = $.i18n('label_tenkenhbanhangkhongduocdetrong');
                $("#tenKenhBanHang").focus();
            }
        }
        else if (iddanhmuc == 4) {
            if (flag && ($scope.tenNganhHang == '' || $scope.tenNganhHang == undefined)) {
                flag = false;
                msg = $.i18n('label_tennganhhangkhongduocdetrong');
                $("#tenNganhHang").focus();
            }
        }
        else if (iddanhmuc == 5) {
            if (flag && ($scope.tenHaoHut == '' || $scope.tenHaoHut == undefined)) {
                flag = false;
                msg = $.i18n('label_tenloaihaohutkhongduocdetrong');
                $("#tenHaoHut").focus();
            }

            if (flag && ($scope.maHaoHut == '' || $scope.maHaoHut == undefined)) {
                flag = false;
                msg = $.i18n('label_maloaihaohutkhongduocdetrong');
                $("#maHaoHut").focus();
            }
        }
        else if (iddanhmuc == 6) {
            if (flag && ($scope.tenTrangThaiDonHang == '' || $scope.tenTrangThaiDonHang == undefined)) {
                flag = false;
                msg = $.i18n('label_tentrangthaidonhangkhongduocdetrong');
                $("#tenTrangThai").focus();
            }

            if (flag && idloaitrangthaidonhang <= 0) {
                flag = false;
                msg = $.i18n('label_loaitrangthaikhongduocdetrong');
            }
        }
        else if (iddanhmuc == 7) {
            if (flag && ($scope.tenChecklist == '' || $scope.tenChecklist == undefined)) {
                flag = false;
                msg = $.i18n('label_tenchecklistkhongduocdetrong');
                $("#tenChecklist").focus();
            }
        }
        else if (iddanhmuc == 8) {
            if (flag && ($scope.tenPhanHoi == '' || $scope.tenPhanHoi == undefined)) {
                flag = false;
                msg = $.i18n('label_tenphanhoikhongduocdetrong');
                $("#tenPhanHoi").focus();
            }
        }
        else if (iddanhmuc == 9) {
            if (flag && ($scope.tenNhanHieu == '' || $scope.tenNhanHieu == undefined)) {
                flag = false;
                msg = $.i18n('label_tennhanhieukhongduocdetrong');
                $("#tenNhanHieu").focus();
            }
        }
        else if (iddanhmuc == 10) {
            if (flag && ($scope.tenNhaCungCap == '' || $scope.tenNhaCungCap == undefined)) {
                flag = false;
                msg = $.i18n('label_tennhacungcapkhongduocdetrong');
                $("#tenNhaCungCap").focus();
            }
        }
        else if (iddanhmuc == 11) {
            if (flag && ($scope.name == '' || $scope.name == undefined)) {
                flag = false;
                msg = $.i18n('label_tenhinhthucthanhtoan');
                $("#name").focus();
            }
        }

        if (!flag)
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n(msg) }, 'warning');

        return flag;
    }

    function openConfirm(message, acceptAction, cancelAction, data) {
        var scope = angular.element("#mainContentId").scope();
        $(" <div id='confirmDelete'></div>").appendTo("body").kendoDialog({
            width: "450px",
            closable: true,
            modal: true,
            title: $.i18n('label_xacnhan'),
            content: message,
            actions: [
                {
                    text: $.i18n('button_huy'), primary: false, action: function () {
                        if (cancelAction != null) {
                            scope[cancelAction](data);
                        }
                    }
                },
                {
                    text: $.i18n('button_dongy'), primary: true, action: function () {
                        scope[acceptAction](data);
                    }
                }
            ],
        })
    }

    //event
    $scope.themdanhmuc = function () {
        openNewDetail();
    }
    $scope.suadanhmuc = function () {
        let listRowsSelected = commonGetRowSelected("#grid");
        if (validationOpenDetail(listRowsSelected)) {
            //Tạm bỏ trường hợp thêm gửi email, sms
            //if (iddanhmuc == 6)
            //    openEditTrangThaiDonHang(listRowsSelected[0]);
            //else
            openEditDetail(listRowsSelected[0]);
        }
    }
    $scope.xoadanhmuc = function () {
        let listRowsSelected = commonGetRowSelected("#grid");
        console.log(listRowsSelected);
        if (validationOpenDetail(listRowsSelected)) {
            openConfirm($.i18n('label_bancochacchanmuonxoakhong'), 'apDungXoa', null, listRowsSelected[0]);
        }
    }
    $scope.apDungXoa = function (data) {
        if (iddanhmuc == 1) {
            danhMucDataService.deleteLoaiKhachHang(data.iD_LoaiKhachHang).then(function (result) {
                if (result.flag) {
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                    loadgrid();
                }
                else
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
            });
        } else if (iddanhmuc == 2) {
            danhMucDataService.deleteDonViTinh(data.iD_DonVi).then(function (result) {
                if (result.flag) {
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                    loadgrid();
                }
                else
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
            });
        }
        else if (iddanhmuc == 3) {
            danhMucDataService.deleteKenhBanHang(data.iD_KenhBanHang).then(function (result) {
                if (result.flag) {
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                    loadgrid();
                }
                else
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
            });
        }
        else if (iddanhmuc == 4) {
            danhMucDataService.deleteNganhHang(data.iD_DANHMUC).then(function (result) {
                if (result.flag) {
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                    loadgrid();
                }
                else
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
            });
        }
        else if (iddanhmuc == 5) {
            danhMucDataService.deleteLoaiHaoHut(data.id).then(function (result) {
                if (result.flag) {
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                    loadgrid();
                }
                else
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
            });
        }
        else if (iddanhmuc == 6) {
            danhMucDataService.deleteTrangThaiDonHang(data.iD_TrangThaiDonHang).then(function (result) {
                if (result.flag) {
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                    loadgrid();
                }
                else
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
            });
        }
        else if (iddanhmuc == 7) {
            danhMucDataService.deleteCheckList(data.iD_CheckList).then(function (result) {
                if (result.flag) {
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                    loadgrid();
                }
                else
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
            });
        }
        else if (iddanhmuc == 8) {
            danhMucDataService.deletePhanHoi(data.iD_PhanHoi).then(function (result) {
                if (result.flag) {
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                    loadgrid();
                }
                else
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
            });
        }
        else if (iddanhmuc == 9) {
            danhMucDataService.deleteNhanHieu(data.iD_NhanHieu).then(function (result) {
                if (result.flag) {
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                    loadgrid();
                }
                else
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
            });
        }
        else if (iddanhmuc == 10) {
            danhMucDataService.deleteNhaCungCap(data.iD_NhaCungCap).then(function (result) {
                if (result.flag) {
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                    loadgrid();
                }
                else
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
            });
        }
        else if (iddanhmuc == 11) {
            console.log(data);
            danhMucDataService.deleteHinhThucThanhToan(data.id).then(function (result) {
                if (result.flag) {
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                    loadgrid();
                }
                else
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
            });
        }
    }

    $scope.xuLyNapVi = function () {
        danhMucDataService.xuLyNapVi($scope.napvi).then(function (result) {
            if (result.flag) {
                loadLichSuNapVi($scope.napvi.ID_NhaCungCap);
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
            }
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
        });
    }

    $scope.luudanhmuc = function () {
        if (validatesave()) {
            if (iddanhmuc == 1) {
                let data = {
                    ID_LoaiKhachHang: idloaikhachhang,
                    IconHienThi: $("#IconHienThi").val(),
                    TenLoaiKhachHang: $scope.tenLoaiKhachHang
                }
                danhMucDataService.saveLoaiKhachHang(data).then(function (result) {
                    if (result.flag) {
                        idloaikhachhang = 0;
                        $scope.tenLoaiKhachHang = '';
                        $("#IconHienThi").data("kendoDropDownList").value("");

                        $scope.formdetail.close();

                        loadgrid();
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                    }
                    else
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
                });
            } else if (iddanhmuc == 2) {
                let data = {
                    ID_DonVi: iddonvi,
                    TenDonVi: $scope.tenDonVi
                }
                danhMucDataService.saveDonViTinh(data).then(function (result) {
                    if (result.flag) {
                        iddonvi = 0;
                        $scope.tenDonVi = '';

                        $scope.formdetail.close();

                        loadgrid();
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                    }
                    else
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
                });
            } else if (iddanhmuc == 3) {
                let data = {
                    ID_KenhCapTren: idkenhbanhangcapcha,
                    ID_KenhBanHang: idkenhbanhang,
                    TenKenhBanHang: $scope.tenKenhBanHang
                }
                danhMucDataService.saveKenhBanHang(data).then(function (result) {
                    if (result.flag) {
                        idkenhbanhang = 0;
                        idkenhbanhangcapcha = 0;
                        $scope.tenKenhBanHang = '';

                        $scope.formdetail.close();

                        loadgrid();
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                    }
                    else
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
                });
            } else if (iddanhmuc == 4) {
                let data = {
                    ID_DANHMUC: idnganhhang,
                    ID_PARENT: idnganhhangcapcha,
                    TenDanhMuc: $scope.tenNganhHang
                }
                danhMucDataService.saveNganhHang(data).then(function (result) {
                    if (result.flag) {
                        idnganhhang = 0;
                        idnganhhangcapcha = 0;
                        $scope.tenNganhHang = '';

                        $scope.formdetail.close();

                        loadgrid();
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                    }
                    else
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
                });
            } else if (iddanhmuc == 5) {
                let data = {
                    ID: idloaihaohut,
                    TiLe: $scope.tyLeHaoHut,
                    GhiChu: $scope.ghiChuHaoHut,
                    MaLoaiHaoHut: $scope.maHaoHut,
                    TenLoaiHaoHut: $scope.tenHaoHut,
                }
                danhMucDataService.saveLoaiHaoHut(data).then(function (result) {
                    if (result.flag) {
                        idloaihaohut = 0;
                        $scope.tenHaoHut = '';
                        $scope.maHaoHut = '';
                        $scope.tyLeHaoHut = 0;
                        $scope.ghiChuHaoHut = '';

                        $scope.formdetail.close();

                        loadgrid();
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                    }
                    else
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
                });
            } else if (iddanhmuc == 6) {
                let data = {
                    ID_TrangThaiDonHang: idtrangthaidonhang,
                    TenTrangThai: $scope.tenTrangThaiDonHang,
                    MauTrangThai: $scope.color,
                    MacDinh: (idloaitrangthaidonhang == 1) ? 1 : 0,
                    KetThuc: (idloaitrangthaidonhang == 2) ? 1 : 0,
                }
                danhMucDataService.saveTrangThaiDonHang(data).then(function (result) {
                    if (result.flag) {
                        idtrangthaidonhang = 0;
                        idloaitrangthaidonhang = 0;
                        $scope.tenTrangThaiDonHang = '';
                        $scope.color = '#339966';
                        $scope.guisms = false;
                        $scope.noidungsms = '';
                        $scope.guiemail = false;
                        $scope.noidungemail = '';

                        $scope.formdetail.close();

                        loadgrid();
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                    }
                    else
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
                });
            } else if (iddanhmuc == 7) {
                let data = {
                    ID_CheckList: idchecklist,
                    TenCheckList: $scope.tenChecklist
                }
                danhMucDataService.saveCheckList(data).then(function (result) {
                    if (result.flag) {
                        idchecklist = 0;
                        $scope.tenChecklist = '';

                        $scope.formdetail.close();

                        loadgrid();
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                    }
                    else
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
                });
            } else if (iddanhmuc == 8) {
                let data = {
                    ID_PhanHoi: idphanhoi,
                    TenPhanHoi: $scope.tenPhanHoi
                }
                danhMucDataService.savePhanHoi(data).then(function (result) {
                    if (result.flag) {
                        idphanhoi = 0;
                        $scope.tenPhanHoi = '';

                        $scope.formdetail.close();

                        loadgrid();
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                    }
                    else
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
                });
            } else if (iddanhmuc == 9) {
                let data = {
                    ID_NhanHieu: idnhanhieu,
                    TenNhanHieu: $scope.tenNhanHieu
                }
                danhMucDataService.saveNhanHieu(data).then(function (result) {
                    if (result.flag) {
                        idnhanhieu = 0;
                        $scope.tenNhanHieu = '';

                        $scope.formdetail.close();

                        loadgrid();
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                    }
                    else
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
                });
            } else if (iddanhmuc == 10) {
                let data = {
                    ID_NhaCungCap: idnhacungcap,
                    TenNhaCungCap: $scope.tenNhaCungCap,
                    DiaChi: $scope.diaChi,
                    NguoiLienHe: $scope.nguoiLienHe,
                    DienThoaiLienHe: $scope.dienThoaiLienHe,
                }
                danhMucDataService.saveNhaCungCap(data).then(function (result) {
                    if (result.flag) {
                        idnhacungcap = 0;
                        $scope.tenNhaCungCap = '';
                        $scope.diaChi = '';
                        $scope.nguoiLienHe = '';
                        $scope.dienThoaiLienHe = '';

                        $scope.formdetail.close();

                        loadgrid();
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                    }
                    else
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
                });
            }
            else if (iddanhmuc == 11) {
                let data = {
                    id : $scope.id,
                    name: $scope.name,
                    virtualPaymentClientURL: $scope.virtualPaymentClientURL,
                    version: $scope.version,
                    merchantCode : $scope.merchantCode,
                    merchantName : $scope.merchantName,
                    serviceCode : $scope.serviceCode,
                    countryCode : $scope.countryCode,
                    payType : $scope.payType,
                    ccy : $scope.ccy,
                    masterMerCode : $scope.masterMerCode,
                    merchantType : $scope.merchantType,
                    terminalId : $scope.terminalId,
                    terminalName : $scope.terminalName,
                    user : $scope.user,
                    password : $scope.password,
                    accessCode : $scope.accessCode,
                    hascode : $scope.hascode,
                    currency : $scope.currency,
                    returnURL : $scope.returnURL,
                }
                danhMucDataService.luuHinhThucThanhToan(data).then(function (result) {
                    if (result.flag) {
                        $scope.name = '';
                        $scope.virtualPaymentClientURL = '';
                        $scope.version = '';
                        $scope.merchantCode = '';
                        $scope.merchantName = '';
                        $scope.serviceCode = '';
                        $scope.countryCode = '';
                        $scope.payType = '';
                        $scope.ccy = '';
                        $scope.masterMerCode = '';
                        $scope.merchantType = '';
                        $scope.terminalId = '';
                        $scope.terminalName = '';
                        $scope.user = '';
                        $scope.password = '';
                        $scope.accessCode = '';
                        $scope.hascode = '';
                        $scope.currency = '';
                        $scope.returnURL = '';

                        $scope.formdetail.close();

                        loadgrid();
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                    }
                    else
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
                });
            }
        }
    }
    $scope.huyluudanhmuc = function () {
        $scope.formdetail.close();
    }

    $scope.kenhHangOnChange = function () {
        $scope.kenhHangselect = this.kenhHangselect;

        if ($scope.kenhHangselect != undefined)
            idkenhbanhangcapcha = ($scope.kenhHangselect.iD_KenhBanHang < 0) ? 0 : $scope.kenhHangselect.iD_KenhBanHang;
        else
            idkenhbanhangcapcha = 0;
    }
    $scope.nganhHangOnChange = function () {
        $scope.nganhHangselect = this.nganhHangselect;

        if ($scope.nganhHangselect != undefined)
            idnganhhangcapcha = ($scope.nganhHangselect.iD_DANHMUC < 0) ? 0 : $scope.nganhHangselect.iD_DANHMUC;
        else
            idnganhhangcapcha = 0;
    }
    $scope.loaiOnChange = function () {
        $scope.loaiselect = this.loaiselect;

        if ($scope.loaiselect != undefined)
            idloaitrangthaidonhang = ($scope.loaiselect.id_loai < 0) ? 0 : $scope.loaiselect.id_loai;
        else
            idloaitrangthaidonhang = 0;
    }
    $scope.loaiOnChange1 = function () {
        $scope.loaiselect1 = this.loaiselect1;

        if ($scope.loaiselect1 != undefined)
            idloaitrangthaidonhang = ($scope.loaiselect1.id_loai < 0) ? 0 : $scope.loaiselect1.id_loai;
        else
            idloaitrangthaidonhang = 0;
    }
    $scope.openformchitietvi = function (id) {
        $scope.formVi.open().maximize();
        loadLichSuNapVi(id);
    }

    init();

    $("#grid").on("dblclick", "tr[role='row']", function () {
        let grid = $("#grid").data("kendoGrid");
        let row = grid.table.find("[data-uid=" + $(this).attr("data-uid") + "]");

        grid.clearSelection();
        grid.select(row);

        let selectedItem = grid.dataItem(grid.select());

        openEditDetailFromGrid(selectedItem);
    })

})