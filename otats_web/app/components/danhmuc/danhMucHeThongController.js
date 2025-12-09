angular.module('app').controller('danhMucHeThongController', function ($scope, $rootScope, $location, Notification, ComboboxDataService, danhMucDataService) {
    CreateSiteMap();

    let id = 1;
    let idloaikhachhang = 0;
    let loai = 0;
    let idchecklist = 0;
    let idtrangthai = 0;
    let idhaohut = 0;
    let idkhohang = 0;
    let idphanhoi = 0;
    let idkenhbanhang = 0;
    let idDvtinh = 0;
    let idnhomhang = 0;

    function init() {
        showdetail();
        comboBox();
    }
    function comboBox() {
        let arr = [
            { value: $.i18n('label_khoitaodonhang'), id_loai: 0 },
            { value: $.i18n('button_hoantat'), id_loai: 1 },
            { value: $.i18n('header_xulydonhang'), id_loai: 2 },
        ]
        $scope.loaiData = arr;
        danhMucDataService.comboDataKenhBanHang().then(function (result) {
            $scope.kenhHangData = result.data;
        });
        danhMucDataService.comboDataNganhHang().then(function (result) {
            $scope.nganhHangData = result.data;
        });
    }
    function columnThaoTac() {
        let template = '<button ng-click="btn_Sua()" class="btn btn-link btn-menubar" title ="Sửa" ><i class="fas fa-edit fas-sm color-infor"></i></button> '
            + '<button ng-click="btn_Xoa()" class="btn btn-link btn-menubar" title ="Xóa" ><i class="fas fa-trash-alt fas-sm color-danger"></i></button> ';

        let obj = {
            template: template,
            title: "Tác vụ", attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
        }
        return obj;
    }
    function listColumnsgrid() {
        let dataList = [];

        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        //loại khách hàng
        dataList.push({ field: "tenLoaiKhachHang", title: "Tên loại khách hàng", headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "800px" });
        //trạng thái đơn hàng
        dataList.push({ field: "tenTrangThai", title: "Tên trạng thái", headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "320px" });
        dataList.push({ field: "mauTrangThai", title: "Màu", headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "120px" });
        dataList.push({ field: "macDinh", title: "Khởi tạo", headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "120px" });
        dataList.push({ field: "ketThuc", title: "Kết thúc", headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "120px" });
        dataList.push({ field: "iD_TrangThaiDonHang", title: "Xử lý đơn hàng", headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "120px" });
        //loại hao hụt
        dataList.push({ field: "maLoaiHaoHut", title: "Mã loại hao hụt", headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "170px" });
        dataList.push({ field: "tenLoaiHaoHut", title: "Tên loại hao hụt", headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "240px" });
        dataList.push({ field: "tiLe", title: "Tỷ lệ", headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "130px" });
        dataList.push({ field: "ghiChu", title: "Ghi chú", headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "170px" });
        //kho hàng
        dataList.push({ field: "maKho", title: "Mã kho hàng", headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px" });
        dataList.push({ field: "tenKho", title: "Tên kho hàng", headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "200px" });
        dataList.push({ field: "diaChi", title: "Địa chỉ kho hàng", headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "300px" });
        dataList.push({            
            field: "trangThai", title: "Trạng thái", headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px",
            template: function (e) {
                if (e.trangThai == 1) { return "Hoạt động" } else if (e.trangThai == 0) { return "Không hoạt động" }
            },
        });
        //phản hồi khách hàng
        dataList.push({ field: "tenPhanHoi", title: "Tên phản hồi", headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "800px" });
        //kênh bán hàng
        dataList.push({ field: "name", title: "Tên kênh bán hàng", headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "800px" });
        //checklist
        dataList.push({ field: "tenCheckList", title: "Tên loại checklist", headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "800px" });
        //đơn vị tính
        dataList.push({ field: "tenDonVi", title: "Tên đơn vị tính", headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "800px" });
        //nhóm mặt hàng
        dataList.push({ field: "tenDanhMuc", title: "Tên nhóm mặt hàng", headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "800px" });

        dataList.push(columnThaoTac());

        return dataList;
    }
    function showcolumschitiet() {
        let grid = $("#grid").data("kendoGrid");

        if (id === 1) {
            grid.showColumn("tenLoaiKhachHang");
        }
        else {
            grid.hideColumn("tenLoaiKhachHang");
        }

        if (id === 2) {
            grid.showColumn("tenTrangThai");
            grid.showColumn("mauTrangThai");
            grid.showColumn("macDinh");
            grid.showColumn("ketThuc");
            grid.showColumn("iD_TrangThaiDonHang");
        }
        else {
            grid.hideColumn("tenTrangThai");
            grid.hideColumn("mauTrangThai");
            grid.hideColumn("macDinh");
            grid.hideColumn("ketThuc");
            grid.hideColumn("iD_TrangThaiDonHang");
        }

        if (id === 3) {
            grid.showColumn("maLoaiHaoHut");
            grid.showColumn("tenLoaiHaoHut");
            grid.showColumn("tiLe");
            grid.showColumn("ghiChu");
        }
        else {
            grid.hideColumn("maLoaiHaoHut");
            grid.hideColumn("tenLoaiHaoHut");
            grid.hideColumn("tiLe");
            grid.hideColumn("ghiChu");
        }

        if (id === 4) {
            grid.showColumn("maKho");
            grid.showColumn("tenKho");
            grid.showColumn("diaChi");
            grid.showColumn("trangThai");
            grid.setOptions({ height: $(window).height() - 325 })
        }
        else {
            grid.hideColumn("maKho");
            grid.hideColumn("tenKho");
            grid.hideColumn("diaChi");
            grid.hideColumn("trangThai");
        }

        if (id === 5) {
            grid.showColumn("tenPhanHoi");
        }
        else {
            grid.hideColumn("tenPhanHoi");
        }

        if (id === 6) {
            grid.showColumn("name");
        }
        else {
            grid.hideColumn("name");
        }

        if (id === 7) {
            grid.showColumn("tenCheckList");
        }
        else {
            grid.hideColumn("tenCheckList");
        }

        if (id === 8) {
            grid.showColumn("tenDonVi");
        }
        else {
            grid.hideColumn("tenDonVi");
        }

        if (id === 9) {
            grid.showColumn("tenDanhMuc");
        }
        else {
            grid.hideColumn("tenDanhMuc");
        }

        grid.refresh();
    }
    function showdetail() {
        $scope.show1 = (id === 1);
        $scope.show2 = (id === 2);
        $scope.show3 = (id === 3);
        $scope.show4 = (id === 4);
        $scope.show5 = (id === 5);
        $scope.show6 = (id === 6);
        $scope.show7 = (id === 7);
        $scope.show8 = (id === 8);
        $scope.show9 = (id === 9);

        loadgrid();

    }
    
    function loadgrid() {
        kendo.ui.progress($("#grid"), true);
        $scope.gridOptions = {
            sortable: true,
            height: function () {
                //let heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".toolbarmenu").height());
                return 400;
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

        if (id === 1) {
            
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
        else if (id === 2) {
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
        else if (id === 3) {
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
        else if (id === 4) {
            danhMucDataService.getKhoHang().then(function (result) {
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
        else if (id === 5) {
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
        else if (id === 6) {
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
        else if (id === 7) {
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
        else if (id === 8) {
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
        else if (id === 9) {
            danhMucDataService.getNhomMatHang().then(function (result) {
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
    function openConfirm(message, acceptAction, cancelAction, _dataitem) {
        let scope = angular.element("#mainContentId").scope();
        $(" <div id='confirmDelete'></div>").appendTo("body").kendoDialog({
            width: "450px",
            closable: true,
            modal: true,
            title: "Xác nhận!",
            content: message,
            actions: [
                {
                    text: 'Hủy', primary: false, action: function () {
                        if (cancelAction != null) {
                            scope[cancelAction](_dataitem);
                        }
                    }
                },
                {
                    text: 'Đồng ý', primary: true, action: function () {
                        scope[acceptAction](_dataitem);
                    }
                }
            ],
        })
    }
    $scope.loaddanhmuc = function (_id) {
        id = _id;
        //console.log(id);
        showdetail();
    }
    //event
    $scope.btn_Sua = function () {
        let myGrid = $('#grid').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;


        if (id === 1) {
            idloaikhachhang = dataItem.iD_LoaiKhachHang;
            let item = {
                tenLoaiKhachHang: dataItem.tenLoaiKhachHang,
                iconHienThi: dataItem.IconHienThi,
                iD_LoaiKhachHang: idloaikhachhang,
            }
            $("#tenLoaiKhachHang").text(dataItem.tenLoaiKhachHang);
            $("#iconHienThi").text(dataItem.IconHienThi);
            danhMucDataService.getbyidloaiKhachHang(idloaikhachhang).then(function (result) {
                $scope.objectKhachHang = result.data;
            });

        }
        else if (id === 2) {
            idtrangthai = dataItem.iD_TrangThaiDonHang;
            if ($scope.loaiselect != undefined)
                loai = ($scope.loaiselect.id_loai < 0) ? 0 : $scope.loaiselect.id_loai;
            let item = {
                tenTrangThai: dataItem.tenTrangThai,
                iD_TrangThaiDonHang: idtrangthai,
                macDinh: dataItem.loai,
            }
            $("#tenTrangThai").text(dataItem.tenTrangThai);
            danhMucDataService.getbyidTrangThaiDonHang(idtrangthai).then(function (result) {
                $scope.obj = result.data;
            });

        }
        else if (id === 3) {
            idhaohut = dataItem.maLoaiHaoHut;
            let item = {
                tenHaoHut: dataItem.tenLoaiHaoHut,
                maLoaiHaoHut: idhaohut,
                ghiChu: dataItem.ghiChu,
                tyLeHaoHut: 0,

            }
            $("#maHaoHut").text(dataItem.maLoaiHaoHut);
            $("#tenHaoHut").text(dataItem.tenLoaiHaoHut);
            $("#tyLeHaoHut").text(dataItem.tiLe);
            $("#ghiChu").text(dataItem.ghiChu);
            console.log(dataItem.tenLoaiHaoHut)
            danhMucDataService.getbyidLoaiHaoHut(idhaohut).then(function (result) {
                $scope.obj = result.data;
            });

        }
        else if (id === 4) {
            idkho = dataItem.iD_Kho;
            danhMucDataService.getbyidKhoHang(idkho).then(function (result) {
                $scope.obj = result.data;
            });
        }
        else if (id === 5) {
            idphanhoi = dataItem.iD_PhanHoi;
            let item = {
                tenPhanHoi: dataItem.tenPhanHoi,
                iD_PhanHoi: idphanhoi,
            }
            $("#tenPhanHoi").text(dataItem.tenPhanHoi);
            danhMucDataService.getbyidPhanHoi(idphanhoi).then(function (result) {
                $scope.obj = result.data;
            });

        }
        else if (id === 6) {
            idkenhbanhang = dataItem.id;
            $("#tenKenhHang").text(dataItem.name);
            danhMucDataService.getbyidKenhBanHang(idkenhbanhang).then(function (result) {
                $scope.obj = result.data;
            });

        }
        else if (id === 7) {
            idchecklist = dataItem.iD_CheckList;
            let item = {
                tenChecklist: dataItem.tenCheckList,
                iD_CheckList: idchecklist,
            }
            $("#tenChecklist").text(dataItem.tenCheckList);
            danhMucDataService.getbyidCheckList(idchecklist).then(function (result) {
                $scope.obj = result.data;
            });

        }
        else if (id === 8) {
            idDvtinh = dataItem.iD_DonVi;
            $("#tenDonVi").text(dataItem.tenDonVi);
            danhMucDataService.getbyidDonViTinh(idDvtinh).then(function (result) {
                $scope.obj = result.data;
            });

        }
        else if (id === 9) {
            idnhomhang = dataItem.iD_DANHMUC;
            let item = {
                tenNganhHang: dataItem.tenDanhMuc,
                iD_DANHMUC: idnhomhang,
            }
            $("#tenNganhHang").text(dataItem.tenDanhMuc);

            console.log(dataItem.iD_DANHMUC)
            danhMucDataService.getbyidNhomMatHang(idnhomhang).then(function (result) {
                $scope.obj = result.data;
            });

        }
    }
    $scope.luuThayDoi = function () {
        if (id === 1) {
            let tenloai = $("#tenLoaiKhachHang").val();
            let formData = {
                IconHienThi: "images/kh/01.png",
                ID_LoaiKhachHang: idloaikhachhang,
                NgayTao: new Date().toISOString(),
                TenLoaiKhachHang: tenloai,
                ID_QLLH: $rootScope.UserInfo.iD_QLLH,
            }

            danhMucDataService.saveLoaiKhachHang(formData).then(function (result) {
                console.log(result)
                if (result.flag) {
                    Notification({ title: $.i18n('label_thongbao'), message: 'Lưu thành công' }, 'success');
                    $scope.huy;
                    loadgrid();
                }
                else
                    Notification({ title: $.i18n('label_thongbao'), message: 'Lưu thất bại' }, 'warning');
            });
        }
        else if (id === 2) {
            let tenTrangThai = $("#tenTrangThai").val();
            let formData = {
                MauTrangThai: '#0b2a0b',
                NgayTao: new Date().toISOString(),
                KetThuc: 0,
                MacDinh: 0,
                iD_TrangThaiDonHang: idtrangthai,
                tenTrangThai: tenTrangThai,
            }
            console.log(formData)
            danhMucDataService.saveTrangThaiDonHang(formData).then(function (result) {
                if (result.flag) {
                    Notification({ title: $.i18n('label_thongbao'), message: 'Lưu thành công' }, 'success');
                    $scope.huy;
                    loadgrid();
                }
                else
                    Notification({ title: $.i18n('label_thongbao'), message: 'Lưu thất bại' }, 'warning');

            });
        }
        else if (id === 3) {
            let maLoaiHaoHut = $("#maHaoHut").val();
            let tenHaoHut = $("#tenHaoHut").val();
            let tile = $("#tyLeHaoHut").data("kendoNumericTextBox").value();
            let ghiChu = $("#ghiChu").val();
            let id_haohut = $("#ID_HaoHut").val();
            if (id_haohut == "") {
                id_haohut = 0;
            } else {
                url = "sua_haohut";
            }
            let formData = {
                ID: id_haohut,
                TiLe: tile,
                GhiChu: ghiChu,
                MaLoaiHaoHut: maLoaiHaoHut,
                TenLoaiHaoHut: tenHaoHut,
            }
            console.log(formData)
            danhMucDataService.saveLoaiHaoHut(formData).then(function (result) {
                if (result.flag) {
                    Notification({ title: $.i18n('label_thongbao'), message: 'Lưu thành công' }, 'success');
                    $scope.huy;
                    loadgrid();
                }
                else
                    Notification({ title: $.i18n('label_thongbao'), message: 'Lưu thất bại' }, 'warning');

            });
        }
        else if (id === 4) {
            let formData = {
                ID_Kho: $scope.obj.iD_Kho ? $scope.obj.iD_Kho : 0,
                TenKho: $scope.obj.tenKho,
                MaKho: $scope.obj.maKho,
                DiaChi: $scope.obj.diaChi ? $scope.obj.diaChi : "",
                TrangThai: $('#HoatDong:checkbox:checked').length,
            }
            console.log(formData);
            danhMucDataService.saveKhoHang(formData).then(function (result) {
                if (result.flag) {
                    Notification({ title: $.i18n('label_thongbao'), message: 'Lưu thành công' }, 'success');
                    $scope.huy;
                    loadgrid();
                }
                else
                    Notification({ title: $.i18n('label_thongbao'), message: 'Lưu thất bại' }, 'warning');

            });
        }
        else if (id === 5) {
            let tenPhanHoi = $("#tenPhanHoi").val();
            let formData = {
                iD_PhanHoi: idphanhoi,
                TenPhanHoi: tenPhanHoi,
            }
            console.log(formData)
            danhMucDataService.savePhanHoi(formData).then(function (result) {
                if (result.flag) {
                    Notification({ title: $.i18n('label_thongbao'), message: 'Lưu thành công' }, 'success');
                    $scope.huy;
                    loadgrid();
                }
                else
                    Notification({ title: $.i18n('label_thongbao'), message: 'Lưu thất bại' }, 'warning');

            });
        }
        else if (id === 6) {
            let tenKenhHang = $("#tenKenhHang").val();
            let formData = {
                ID_KenhCapTren: 0,
                ID_KenhBanHang: idkenhbanhang,
                TenKenhBanHang: tenKenhHang,
            }
            console.log(formData)
            danhMucDataService.saveKenhBanHang(formData).then(function (result) {
                if (result.flag) {
                    Notification({ title: $.i18n('label_thongbao'), message: 'Lưu thành công' }, 'success');
                    $scope.huy;
                    loadgrid();
                }
                else
                    Notification({ title: $.i18n('label_thongbao'), message: 'Lưu thất bại' }, 'warning');

            });
        }
        else if (id === 7) {
            let tenChecklist = $("#tenChecklist").val();
            let formData = {
                ID_CheckList: idchecklist,
                TenCheckList: tenChecklist,
            }
            console.log(formData)
            danhMucDataService.saveCheckList(formData).then(function (result) {
                if (result.flag) {
                    Notification({ title: $.i18n('label_thongbao'), message: 'Lưu thành công' }, 'success');
                    $scope.huy;
                    loadgrid();
                }
                else
                    Notification({ title: $.i18n('label_thongbao'), message: 'Lưu thất bại' }, 'warning');

            });
        }
        else if (id === 8) {
            let tenDonVi = $("#tenDonVi").val();
            let formData = {
                ID_DonVi: idDvtinh,
                TenDonVi: tenDonVi,
            }
            console.log(formData)
            danhMucDataService.saveDonViTinh(formData).then(function (result) {
                if (result.flag) {
                    Notification({ title: $.i18n('label_thongbao'), message: 'Lưu thành công' }, 'success');
                    $scope.huy;
                    loadgrid();
                }
                else
                    Notification({ title: $.i18n('label_thongbao'), message: 'Lưu thất bại' }, 'warning');

            });
        }
        else if (id === 9) {
            let tenNganhHang = $("#tenNganhHang").val();
            let formData = {
                ID_Parent: 0,
                ID_Nhom: idnhomhang,
                TenNhom: tenNganhHang,
            }
            console.log(formData)
            danhMucDataService.saveNhomMatHang(formData).then(function (result) {
                if (result.flag) {
                    Notification({ title: $.i18n('label_thongbao'), message: 'Lưu thành công' }, 'success');
                    $scope.huy;
                    loadgrid();
                }
                else
                    Notification({ title: $.i18n('label_thongbao'), message: 'Lưu thất bại' }, 'warning');

            });
        }
    }
    $scope.btn_Xoa = function () {
        let myGrid = $('#grid').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;
        openConfirm("Bạn có chắc chắn muốn xóa không?", 'apDungXoa', null, dataItem);
        console.log('thaats bai')
    }
    $scope.apDungXoa = function (dataItem) {
        if (id === 1) {
            danhMucDataService.deleteLoaiKhachHang(dataItem.iD_LoaiKhachHang).then(function (result) {
                if (result.flag) {
                    Notification({ title: $.i18n('label_thongbao'), message: 'Xóa thành công' }, 'success');
                    $scope.huy;
                    loadgrid();
                }
                else
                    Notification({ title: $.i18n('label_thongbao'), message: 'Xóa thất bại' }, 'warning');
            });
        }
        else if (id === 2) {
            danhMucDataService.deleteTrangThaiDonHang(dataItem.iD_TrangThaiDonHang).then(function (result) {

                if (result.flag) {
                    Notification({ title: $.i18n('label_thongbao'), message: 'Xóa thành công' }, 'success');
                    $scope.huy;
                    loadgrid();
                }
                else
                    Notification({ title: $.i18n('label_thongbao'), message: 'Xóa thất bại' }, 'warning');
            });
        }
        else if (id === 3) {
            danhMucDataService.deleteLoaiHaoHut(dataItem.maLoaiHaoHut).then(function (result) {
                if (result.flag) {
                    Notification({ title: $.i18n('label_thongbao'), message: 'Xóa thành công' }, 'success');
                    $scope.huy;
                    loadgrid();
                }
                else
                    Notification({ title: $.i18n('label_thongbao'), message: 'Xóa thất bại' }, 'warning');
            });
        }
        else if (id === 4) {
            danhMucDataService.deleteKhoHang(dataItem.iD_Kho).then(function (result) {
                if (result.flag) {
                    Notification({ title: $.i18n('label_thongbao'), message: 'Xóa thành công' }, 'success');
                    $scope.huy;
                    loadgrid();
                }
                else
                    Notification({ title: $.i18n('label_thongbao'), message: 'Xóa thất bại' }, 'warning');
            });
        }
        else if (id === 5) {
            danhMucDataService.deletePhanHoi(dataItem.id).then(function (result) {
                if (result.flag) {
                    Notification({ title: $.i18n('label_thongbao'), message: 'Xóa thành công' }, 'success');
                    $scope.huy;
                    loadgrid();
                }
                else
                    Notification({ title: $.i18n('label_thongbao'), message: 'Xóa thất bại' }, 'warning');
            });
        }
        else if (id === 6) {
            danhMucDataService.deleteKenhBanHang(dataItem.id).then(function (result) {
                if (result.flag) {
                    Notification({ title: $.i18n('label_thongbao'), message: 'Xóa thành công' }, 'success');
                    $scope.huy;
                    loadgrid();
                }
                else
                    Notification({ title: $.i18n('label_thongbao'), message: 'Xóa thất bại' }, 'warning');
            });
        }
        else if (id === 7) {
            danhMucDataService.deleteCheckList(dataItem.iD_CheckList).then(function (result) {
                if (result.flag) {
                    Notification({ title: $.i18n('label_thongbao'), message: 'Xóa thành công' }, 'success');
                    $scope.huy;
                    loadgrid();
                }
                else
                    Notification({ title: $.i18n('label_thongbao'), message: 'Xóa thất bại' }, 'warning');
            });
        }
        else if (id === 8) {
            danhMucDataService.deleteDonViTinh(dataItem.iD_DonVi).then(function (result) {
                if (result.flag) {
                    Notification({ title: $.i18n('label_thongbao'), message: 'Xóa thành công' }, 'success');
                    $scope.huy;
                    loadgrid();
                }
                else
                    Notification({ title: $.i18n('label_thongbao'), message: 'Xóa thất bại' }, 'warning');
            });
        }
        else if (id === 9) {
            danhMucDataService.deleteNhomMatHang(dataItem.iD_DANHMUC).then(function (result) {
                if (result.flag) {
                    Notification({ title: $.i18n('label_thongbao'), message: 'Xóa thành công' }, 'success');
                    $scope.huy;
                    loadgrid();
                }
                else
                    Notification({ title: $.i18n('label_thongbao'), message: 'Xóa thất bại' }, 'warning');
            });
        }
        console.log('thaats bai2')
    }
    $scope.huy = function () {
        $scope.obj = null;
        $("#tenLoaiKhachHang").val("");
        $("#iconHienThi").val("");

        $("#tenPhanHoi").val("");
        $("#tenTrangThai").val("");
        $("#loai").val("");

        $("#maHaoHut").val("");
        $("#tenHaoHut").val("");
        $("#tyLeHaoHut").data("kendoNumericTextBox").value("");
        $("#ghiChu").val("");

    }

    $scope.loaiOnChange = function () {
        $scope.loaiselect = this.loaiselect;
    }
    init();

})