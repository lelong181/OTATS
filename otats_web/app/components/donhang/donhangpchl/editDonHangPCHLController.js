angular.module('app').controller('editDonHangPCHLController', function ($rootScope, $scope, $state, $stateParams, $timeout, Notification, donHangDataService, ComboboxDataService, printer) {
    CreateSiteMap();

    let __iddonhang = 0;
    let __chiTietDonHangXoa = [];
    let __idkhachhang = 0;
    let __idnhom = 0;
    let __arridhangchitiet = [];
    let __site = [];
    let __image_url;

    let __format = "{0:n0}";
    let __digits = 0;

    $scope.objectDonHang = {};
    $scope.sotienthanhtoan = 0;
    $scope.thanhtoantoanbo = true;
    $scope.ghichuthanhtoan = '';
    $scope.ghiChuGiaoHang = '';
    $scope.lyDoHuy = '';

    let data_tttt = [
        { id: 1, name: $.i18n('header_chuathanhtoan') },
        { id: 2, name: $.i18n('label_dangthanhtoan') },
        { id: 3, name: $.i18n('header_thanhtoanmotphan') },
        { id: 4, name: $.i18n('header_dathanhtoan') },
    ]

    //config
    function init() {

        if ($rootScope.isAdmin == 1)
            getquyen();
        else {
            $scope.permission = {
                them: 1, sua: 1, xoa: 1
            };
        }

        __iddonhang = $stateParams.iddonhang;
        getChiTietDonHang();
        getAllSite()
        inittreeview();
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

        loadtreeView();
    }
    function loadtreeView() {
        donHangDataService.getnhommathangtheophanquyen().then(function (result) {
            let data = result.data;
            if ($rootScope.lang != 'vi-vn')
                data[0].name = data[0].name.replace("Tất cả mặt hàng", "ALL");
            setDataTreeview(data);
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
        __idnhom = tree.dataItem(selectedNode).id;
        loadgridhangchon();
    }
    function listColumnsgridhangchon() {
        let dataList = [];

        dataList.push({
            field: "chiTiet", title: $.i18n('button_them'),
            template: '<button ng-click="addrow()" class="btn btn-link btn-menubar" title ="' + $.i18n('button_themhang') + '" ><i class="fas fa-plus-circle fas-sm color-infor"></i></button> ',
            attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "60px"
        });
        //dataList.push({
        //    title: "#", template: "#= ++RecordNumber #",
        //    width: "50px",
        //    headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
        //    attributes: { class: "text-center" },
        //});
        //dataList.push({ field: "maHang", title: $.i18n('header_mahang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px" });
        dataList.push({ field: "tenHang", title: $.i18n('header_tenhang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "220px" });
        dataList.push({ field: "tenDonVi", title: $.i18n('header_donvi'), attributes: { class: "text-center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "80px" });
        dataList.push({ field: "giaBuon", title: $.i18n('header_giabanbuon'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), attributes: { class: "text-right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px" });
        dataList.push({ field: "giaLe", title: $.i18n('header_giabanle'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), attributes: { class: "text-right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px" });
        //dataList.push({ field: "soLuongTon", title: $.i18n('header_soluongton'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), attributes: { class: "text-right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px" });

        return dataList;
    }
    function loadgridhangchon() {
        kendo.ui.progress($("#gridHangChon"), true);
        $scope.gridHangChonOptions = {
            sortable: true,
            height: function () {
                return 550;
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

        donHangDataService.getdanhsachmathangbyidkhachhangidnhom(__idkhachhang, __idnhom).then(function (result) {
            let arr_hangchonall = result.data;
            let arr_hangchonchuachon = arr_hangchonall.filter((item) => {
                return (__arridhangchitiet.indexOf(item.idMatHang) == -1)
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
        __idnhom = $("#treehangchon").getKendoTreeView().dataItem(e.node).id;
        loadgridhangchon();
    }

    function getquyen() {
        let url = 'danhsachdonhangpchl'
        ComboboxDataService.getquyen(url).then(function (result) {
            $scope.permission = result.data;
            if ($scope.permission.iD_ChucNang <= 0) {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_khongcoquyentruycapchucnang') }, "error");
                $location.path('/danhsachdonhangpchl')
            }
        });
    }

    function getAllSite() {
        donHangDataService.getallsite().then(function (result) {
            if (result.flag) {
                __site = result.data;
            } else {

            }
        })
    }

    function getChiTietDonHang() {
        ComboboxDataService.getDataTrangThaiDonHang().then(function (result) {
            $scope.listtrangthaidonhang = setlisttrangthaidonhang(result.data);
        })

        donHangDataService.getById(__iddonhang).then(function (result) {
            modifiedformfield(result.data.donHang);
            loadgrid();
        })

    }

    function modifiedformfield(data) {
        let ngaytao = new Date(data.ngayTao);
        data.ngayTao = kendo.toString(ngaytao, formatDateTime);

        let lastupdate = new Date(data.lastUpdate_ThoiGian_NhanVien);
        if (lastupdate != null && lastupdate.getFullYear() > 1900)
            data.lastUpdate_ThoiGian_NhanVien = kendo.toString(lastupdate, formatDateTime);
        else
            data.lastUpdate_ThoiGian_NhanVien = '';

        $scope.objectDonHang = data;

        $scope.trangthaidonhang = { iD_TrangThaiDonHang: $scope.objectDonHang.iD_TrangThaiDongHang, tenTrangThai: $scope.objectDonHang.tenTrangThaiDongHang };

        $scope.maKH = (data.maKH.trim() == '') ? '-' : data.maKH;
        $scope.dienThoai = (data.dienThoai.trim() == '') ? '-' : data.dienThoai;
        $scope.xuatHoaDon = data.xuatHoaDon;
        $scope.inVeTaiQuay = data.inVeTaiQuay;
        $scope.diaChiTao = (data.diaChiTao.trim() == '') ? '-' : data.diaChiTao;
        $scope.ghiChu = (data.ghiChu.trim() == '') ? '-' : data.ghiChu;
        $scope.lyDo = (data.lyDo.trim() == '') ? '-' : data.lyDo;

        $scope.nametrangthaithanhtoan = data_tttt[data.iD_TrangThaiThanhToan - 1].name;

        $scope.objprint = data;
        $scope.objprint.tenTrangThaiThanhToan = $scope.nametrangthaithanhtoan;

        //loadlistimages();
    }

    function loadlistimages() {
        if ($scope.objectDonHang.danhsachanh.length > 0) {
            $scope.listimages = $scope.objectDonHang.danhsachanh[0].danhsachanh;
            $scope.tongsoanh = $scope.listimages.length;
        } else
            $scope.tongsoanh = $scope.objectDonHang.danhsachanh.length;
    }

    function setlisttrangthaidonhang(data) {
        var list = [];

        angular.forEach(data, function (value) {
            list.push({ iD_TrangThaiDonHang: value.iD_TrangThaiDonHang, tenTrangThai: value.tenTrangThai });
        });

        return list;
    }

    function listColumnsgrid() {
        var dataList = [];

        dataList.push({
            field: "idchitietmathang", title: $.i18n('button_xoa'),
            template: '<button ng-click="deleterow()" class="btn btn-link btn-menubar" title ="' + $.i18n('label_xoadong') + '" ><i class="fas fa-trash fas-sm color-danger"></i></button> ',
            filterable: {
                cell: {
                    operator: "contains",
                    showOperators: false,
                    template: function (e) {
                        e.element.parent().html("<a class='k-button' title='Thêm mặt hàng' style='width:100%; height:25px;' ng-click='openWindowHangChon()'><i class='fa fa-plus'></i></a>")
                    }
                }
            },
            attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, width: "60px"
        });

        //dataList.push({
        //    title: "#", template: "#= ++RecordNumber #",
        //    width: "50px",
        //    headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
        //    attributes: { class: "text-center" },
        //});
        dataList.push({
            field: "tacvu", title: $.i18n('header_tacvu'),
            template: function (e) {
                if (e.isDichVu > 0) {
                    return '<button ng-click="openformchitiet()" class="btn btn-link btn-menubar" title ="' + $.i18n('label_chonmathang') + '" ><i class="fas fa-plus fas-sm color-infor"></i></button> ';
                }
                else {
                    return '';
                }
            },
            width: "70px",
            filterable: false,
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({
            field: "tacvu", title: $.i18n('header_chitiet'),
            template: '<button ng-click="openformchitietve()" class="btn btn-link btn-menubar" title ="' + $.i18n('label_chonmathang') + '" ><i class="fas fa-tags fas-sm color-infor"></i></button> ',
            width: "70px",
            filterable: false,
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        //dataList.push({
        //    field: "mahang", title: $.i18n('header_mahang'),
        //    footerTemplate: $.i18n('label_total'),
        //    headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px"
        //});
        dataList.push({ field: "tenhang", title: $.i18n('header_tenhang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({
            field: "ngay", title: "Ngày sử dụng",
            format: "{0:dd/MM/yyyy}",
            editor: dateEditor,
            attributes: { style: "text-align: center" }, footerAttributes: { style: "text-align: right" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        });
        dataList.push({
            field: "soluong", title: $.i18n('header_soluong'),
            editor: numberEditor,
            template: function (e) {
                if (e.isDichVu) {
                    return "-";
                } else {
                    return e.soluong;
                }
            },
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            attributes: { style: "text-align: right" }, footerAttributes: { style: "text-align: right" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        });
        //dataList.push({
        //    field: "soton", title: $.i18n('header_soluongton'),
        //    format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
        //    attributes: { style: "text-align: right" }, footerAttributes: { style: "text-align: right" },
        //    headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        //});
        dataList.push({
            field: "giaban", title: $.i18n('header_giaban'),
            editor: numberEditor,
            template: function (e) {
                if (e.isDichVu) {
                    return "-";
                } else {
                    return kendo.toString(e.giaban, $rootScope.UserInfo.dinhDangSo);
                }
            },
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            attributes: { style: "text-align: right" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "120px"
        });
        //dataList.push({
        //    field: "dagiao", title: $.i18n('header_dagiao'),
        //    format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
        //    footerTemplate: formatNumberInFooterGrid('dagiao.sum', $rootScope.UserInfo.dinhDangSo),
        //    attributes: { style: "text-align: right" }, footerAttributes: { style: "text-align: right" },
        //    headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        //});
        //dataList.push({
        //    field: "giaoHang", title: $.i18n('header_giaohang'),
        //    format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
        //    attributes: { style: "text-align: right" }, footerAttributes: { style: "text-align: right" },
        //    headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        //});
        dataList.push({ field: "tendonvi", title: $.i18n('header_donvi'), attributes: { class: "text-center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px" });
        //dataList.push({
        //    field: "chietkhauphantram", title: $.i18n('header_chietkhauphantram'),
        //    format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
        //    attributes: { style: "text-align: right" },
        //    headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        //});
        //dataList.push({
        //    field: "chietkhautien", title: $.i18n('header_chietkhautien'),
        //    format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
        //    footerTemplate: formatNumberInFooterGrid('chietkhautien.sum', $rootScope.UserInfo.dinhDangSo),
        //    attributes: { style: "text-align: right" }, footerAttributes: { style: "text-align: right" },
        //    headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "120px"
        //});
        //dataList.push({
        //    field: "tongtienchietkhau", title: $.i18n('header_tongtienchietkhau'),
        //    format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
        //    footerTemplate: formatNumberInFooterGrid('tongtienchietkhau.sum', $rootScope.UserInfo.dinhDangSo),
        //    attributes: { style: "text-align: right" }, footerAttributes: { style: "text-align: right" },
        //    headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        //});
        //dataList.push({
        //    field: "tenkhoxuat", title: $.i18n('header_chonkhoxuathang'),
        //    editor: khoXuatDropDownEditor,
        //    headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "200px"
        //});
        dataList.push({
            field: "tongTien", title: $.i18n('header_tongtien'),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('tongTien.sum', $rootScope.UserInfo.dinhDangSo),
            attributes: { style: "text-align: right" }, footerAttributes: { style: "text-align: right" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        //dataList.push({
        //    field: "tenKho", title: $.i18n('header_tenkho'),
        //    headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "250px"
        //});
        //dataList.push({ field: "ghichu", title: $.i18n('header_ghichu'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px" });

        return dataList;
    }
    function loadgrid() {
        $scope.gridOptions = {

            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".body-donhang__chitiet-infor").height());
                if (heightGrid > 400)
                    return heightGrid - 100;
                else
                    return 400;
            },
            dataBinding: function () {
                RecordNumber = (this.dataSource.page() - 1) * this.dataSource.pageSize();
            },
            resizable: true,
            editable: ($scope.objectDonHang.isProcess > 0 || $scope.objectDonHang.iD_TrangThaiThanhToan > 1) ? false : true,

            filterable: {
                mode: "row"
            },
            pageable: false,
            save: function (e) {
                savechitietdonhang(e);
            },
            update: function (e) {
                e.success();
            },
            columns: listColumnsgrid()
        };
        donHangDataService.getChiTietHangHoa(__iddonhang).then(function (result) {
            $scope.list = result.data;
            $.each($scope.list, function (index, item) {
                __arridhangchitiet.push(item.idhanghoa)
            });
            let dataSource = new kendo.data.DataSource({
                data: result.data,
                schema: {
                    id: "idchitietdonhang",
                    model: {
                        fields: {
                            xoa: {
                                type: "string",
                                editable: false,
                                nullable: true
                            },
                            mahang: {
                                type: "string",
                                editable: false,
                                nullable: true
                            },
                            tenhang: {
                                type: "string",
                                editable: false,
                                nullable: true
                            },
                            giaban: {
                                type: "number",
                                editable: false,
                                nullable: true
                            },
                            tenhinhthucban: {
                                type: "string",
                                editable: false,
                                nullable: true
                            },
                            soluong: {
                                type: "number",
                                validation: {
                                    min: 0
                                },
                                editable: true,
                                nullable: true
                            },
                            dagiao: {
                                type: "number",
                                editable: false,
                                nullable: true
                            },
                            giaoHang: {
                                type: "number",
                                validation: {
                                    min: 0
                                },
                                editable: true,
                                nullable: true
                            },
                            tendonvi: {
                                type: "string",
                                editable: false,
                                nullable: true
                            },
                            tongTien: {
                                type: "number",
                                editable: false,
                                nullable: true
                            },
                            tongtienchietkhau: {
                                type: "number",
                                editable: false,
                                nullable: true
                            },
                            chietkhauphantram: {
                                type: "number",
                                editable: false,
                                nullable: true
                            },
                            chietkhautien: {
                                type: "number",
                                editable: false,
                                nullable: true
                            },
                            tenKho: {
                                type: "string",
                                editable: false,
                                nullable: true
                            },
                            ghichu: {
                                type: "string",
                                editable: true,
                                nullable: true
                            },
                            tenkhoxuat: {
                                nullable: true,
                                type: "string",
                                defaultValue: {
                                    tenKho: ''
                                }
                            },
                            ngay: {
                                type: "date",
                                editable: true
                            },
                        }
                    }
                },
                pageSize: 500,
                aggregate: [
                    { field: "soluong", aggregate: "sum" },
                    { field: "dagiao", aggregate: "sum" },
                    { field: "giaoHang", aggregate: "sum" },
                    { field: "chietkhautien", aggregate: "sum" },
                    { field: "tongtienchietkhau", aggregate: "sum" },
                    { field: "tongTien", aggregate: "sum" }
                ]
            });
            $("#grid").data("kendoGrid").setDataSource(dataSource);
        })
    }

    function savechitietdonhang(e) {
        if (validatechitietdonhang(e)) {
            if (e.values.soluong != null) {
                e.model.soluong = e.values.soluong;
                e.model.tongTien = e.values.soluong * e.model.giaban;
                e.model.tongtienchietkhau = (parseFloat(e.model.tongTien) * parseFloat(e.model.chietkhauphantram) / 100) + parseFloat(e.model.chietkhautien);

                $("#grid").data("kendoGrid").refresh();

                $timeout(updatethanhtien, 200);
            }
        } else {
            e.preventDefault();
            if (e.values.soluong != null) {
                e.values.soluong = e.model.soluong;
                e.model.dirtyFields.soluong = false;
                e.model.dirty = false;
            } else if (e.values.giaoHang != null) {
                e.values.giaoHang = e.model.giaoHang;
                e.model.dirtyFields.giaoHang = false;
                e.model.dirty = false;
            }
        }
    }
    function validatechitietdonhang(e) {
        let flag = true;
        let msg = "";

        if (e.values.soluong != null) {
            if (e.model.dagiao > 0) {
                msg = $.i18n("label_mathangdagiaokhongduocphepsua");
                flag = false;
            }

            if (flag && e.values.soluong < e.model.dagiao) {
                msg = $.i18n("label_soluongsuanhohonsoluongdagiao");
                flag = false;
            }
        }

        if (flag && e.values.giaoHang != null) {
            let soLuongConLai = e.model.soluong - e.model.dagiao;
            if (e.values.giaoHang > soLuongConLai) {
                msg = $.i18n("label_soluonggiaolonhonsoluongcangiaoconlaicuamathang") + e.model.tenhang;
                flag = false;
            }
        }

        if (!flag) {
            Notification({ title: $.i18n('label_thongbao'), message: msg }, 'warning');
        }

        return flag;
    }

    function updatethanhtien() {
        let grid = $("#grid").data("kendoGrid");
        let tongtiendonhang = grid.dataSource.aggregates().tongTien.sum;
        let tongtienchietkhaumathang = grid.dataSource.aggregates().tongtienchietkhau.sum;
        let tienDaThanhToan = $scope.objectDonHang.tienDaThanhToan * 1;

        let phantramchietkhaudonhang = $scope.objectDonHang.chietKhauPhanTram * 1 + $scope.objectDonHang.chietKhauPhanTramKhac * 1;
        let tienchietkhaudonhang = $scope.objectDonHang.chietKhauTien * 1 + $scope.objectDonHang.chietKhauTienKhac * 1;
        let tongtienchietkhau = tongtienchietkhaumathang + tienchietkhaudonhang + (tongtiendonhang - tongtienchietkhaumathang) * phantramchietkhaudonhang / 100;

        $scope.objectDonHang.tongTienChietKhau = tongtienchietkhau;
        $scope.objectDonHang.tongTien = tongtiendonhang - tongtienchietkhau;
        $scope.objectDonHang.conLai = tongtiendonhang - tongtienchietkhau - tienDaThanhToan;
        let img = '<img style="width: 190px; margin: auto;" src="https://img.vietqr.io/image/' + $scope.nhanvien.chucVu + '-' + $scope.nhanvien.queQuan + '-compact2.png?amount='
            + $scope.objectDonHang.conLai
            + '&addInfo=' + $scope.objectDonHang.maThamChieu
            + '"/>';
        $("#qrthanhtoan").html(img);
    }

    function khoXuatDropDownEditor(container, options) {
        $('<input name="' + options.field + '"/>')
            .appendTo(container)
            .kendoDropDownList({
                autoBind: false,
                dataTextField: "tenKho",
                dataValueField: "tenKho",
                select: function (e) {
                    options.model.tenkhoxuat = e.dataItem.tenKho;
                    options.model.idkhoxuat = e.dataItem.iD_Kho;
                },
                suggest: true,
                dataSource: {
                    transport: {
                        read: function (operation) {
                            ComboboxDataService.getDataKhoHangByIDHang(options.model.idhanghoa).then(function (result) {
                                operation.success(result.data);
                            })
                        }
                    },
                },
            });
    }

    function listColumnsgridThanhToan() {
        var dataList = [];

        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({ field: "tenNhanVien", title: $.i18n('header_tennhanvien'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px" });
        dataList.push({
            field: "ngayThanhToan", title: $.i18n('header_ngaythanhtoan'),
            template: function (dataItem) {
                return kendo.htmlEncode(kendo.toString(dataItem.ngayThanhToan, formatDate));
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "120px"
        });
        dataList.push({
            field: "soTien", title: $.i18n('header_sotiendathanhtoan'),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            attributes: { style: "text-align: right" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "ghiChu", title: $.i18n('header_ghichu'),
            template: function (item) {
                if (item.ghiChu == null || item.ghiChu == 'null')
                    return ''
                else
                    return item.ghiChu
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "200px"
        });
        dataList.push({
            field: "imageUrl", title: "Hình ảnh",
            template: function (dataItem) {
                if (dataItem.imageUrl == null || dataItem.imageUrl == '')
                    return ''
                else {
                    let src = SERVERIMAGE + dataItem.imageUrl;
                    return '<img src="' + src + '" alt="" style="max-height:50px;">';
                }
            },
            attributes: { style: "text-align: center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        return dataList;
    }
    function loadgridThanhToan() {
        $scope.gridthanhtoanOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height());
                return heightGrid - 150;
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
            columns: listColumnsgridThanhToan()
        };

        donHangDataService.getLichSuThanhToan(__iddonhang).then(function (result) {
            $scope.gridthanhtoanData = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                            ngayThanhToan: {
                                type: "date"
                            }
                        }
                    }
                },
                pageSize: 20
            };
            $("#gridthanhtoan").kendoTooltip({
                filter: "img",
                position: "left",
                content: function (e) {
                    var target = e.target;
                    return "<img style='width:280px;' src='" + target[0].currentSrc + "' />"
                }
            }).data("kendoTooltip");
        })


    }

    function listColumnsgridGiaoHang() {
        var dataList = [];

        dataList.push({
            field: "lanGiao", title: $.i18n('header_langiao'),
            groupHeaderTemplate: "Lần giao #=value# ( Ngày giao: #= kendo.toString(items[0].ngayGiao, formatDateTime) #)",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "80px"
        });
        dataList.push({ field: "maHang", title: $.i18n('header_mahang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px" });
        dataList.push({ field: "tenHang", title: $.i18n('header_tenhang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({
            field: "soLuongTong", title: $.i18n('header_tongsoluong'),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), attributes: { style: "text-align: right" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        });
        dataList.push({
            field: "soLuongGiao", title: $.i18n('header_soluongiao'),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), attributes: { style: "text-align: right" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        });
        dataList.push({
            field: "conLai", title: $.i18n('header_conlai'),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), attributes: { style: "text-align: right" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        });
        dataList.push({ field: "tenKho", title: $.i18n('header_tenkho'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({ field: "ghiChu", title: $.i18n('header_ghichu'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px" });

        return dataList;
    }
    function loadgridGiaoHang() {
        $scope.gridGiaoHangOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height());
                return heightGrid - 90;
            },
            resizable: true,
            editable: false,
            filterable: {
                mode: "row"
            },
            pageable: $scope.lang == 'vi-vn' ? pageableShort_vi : pageableShort_en,
            columns: listColumnsgridGiaoHang()
        };

        donHangDataService.getLichSuGiaoHang(__iddonhang).then(function (result) {
            let data = result.data;
            let arrnew = [];
            angular.forEach(data, function (obj) {
                var flagTmp = false;
                angular.forEach(arrnew, function (item) {
                    if (obj.idMatHang == item.idMatHang && item.hinhThucBan == obj.hinhThucBan) {
                        item.soLuongGiao += obj.soLuongGiao;
                        obj.conLai = parseFloat(obj.soLuongTong) - parseFloat(item.soLuongGiao);
                        flagTmp = true;
                        return true;
                    }
                })
                if (!flagTmp) {
                    obj.conLai = parseFloat(obj.soLuongTong) - parseFloat(obj.soLuongGiao);
                    var objTmp = {
                        soLuongGiao: obj.soLuongGiao,
                        idMatHang: obj.idMatHang,
                        hinhThucBan: obj.hinhThucBan
                    }
                    arrnew.push(objTmp);
                }

            });
            $scope.gridGiaoHangData = {
                data: data,
                schema: {
                    model: {
                        fields: {
                            iD_KhachHang: {
                                type: "number"
                            }
                        }
                    }
                },
                pageSize: 20,
                group: {
                    field: "lanGiao"
                }
            };
        })

    }

    function listColumnsgridPhanQuyen() {
        var dataList = [];

        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({ field: "tenNhanVien", title: $.i18n('header_tennhanvien'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "400px" });
        dataList.push({
            field: "xem", title: $.i18n('header_xem'),
            template: templateCheckBoxGrid("xem"), attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        });
        dataList.push({
            field: "vaoDiem", title: $.i18n('header_vaodiem'),
            template: templateCheckBoxGrid("vaoDiem"), attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        });
        dataList.push({
            field: "giaoHang", title: $.i18n('header_giaohang'),
            template: templateCheckBoxGrid("giaoHang"), attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        });
        dataList.push({
            field: "thanhToan", title: $.i18n('header_thanhtoan'), attributes: { class: "text-center" },
            template: templateCheckBoxGrid("thanhToan"),
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        });

        return dataList;
    }
    function loadgridPhanQuyen() {
        $scope.gridPhanQuyenOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".toolbarmenu").height());
                return heightGrid - 100;
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
            columns: listColumnsgridPhanQuyen()
        };

        donHangDataService.getNhanVienPhanQuyen(__iddonhang).then(function (result) {
            $scope.gridPhanQuyenData = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                            tenNhanVien: {
                                type: "string"
                            },
                            xem: {
                                type: "string"
                            },
                            vaoDiem: {
                                type: "number"
                            },
                            giaoHang: {
                                type: "number"
                            },
                            thanhToan: {
                                type: "number"
                            }
                        }
                    }
                },
                pageSize: 20
            };
        })
    }

    function listColumnsgridXuLy() {
        var dataList = [];

        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({
            //field: "site.bankCode",
            field: "bankCode",
            groupHeaderTemplate: function (e) { return getHeaderXuLyDonHangDichVu(e); },
            //groupHeaderTemplate: 'Sitecode: #= value # (Tổng tiền: #= thanhTien.sum#)'
            //    + '<img style="width: 120px; margin: auto;" src="https://img.vietqr.io/image/#= site.bankCode#-#= site.bankAccNumber#-compact.png?amount=#= thanhTien.sum#&addInfo=lsi'
            //    + $scope.objectDonHang.iD_DonHang
            //    + '/>',
            title: 'Site', headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({ field: "dichVu.tenDichVu", title: 'Dịch vụ', headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px" });
        dataList.push({
            field: "giaBan",
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), attributes: { style: "text-align: right" },
            title: 'Giá', headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px"
        });
        dataList.push({
            field: "soLuong",
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), attributes: { style: "text-align: right" },
            title: 'Số lượng', attributes: { class: "text-right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px"
        });
        dataList.push({
            field: "thanhTien", title: 'Thành tiền',
            aggregates: ["sum"],
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), attributes: { style: "text-align: right" },
            footerTemplate: formatNumberInFooterGrid('sum', $rootScope.UserInfo.dinhDangSo),
            footerAttributes: { style: "text-align: right" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px"
        });
        dataList.push({
            field: "", title: 'Vé điện tử',
            template: function (e) {
                if ($scope.objectDonHang.iD_TrangThaiDongHang == 3) {
                    let link = "https://server.lscloud.vn/Booking/PrintTicketByBookingCode?ID_ChiTietDonHang="
                        + e.iD_ChiTietDonHang
                        + "&ID_DonHang=" + e.iD_DonHang
                        + "&ID_MatHang=" + e.iD_MatHang
                        + "&ID_DichVu=" + e.iD_DichVu
                        + "&BookingCode=" + e.bookingCode;
                    return "<a href='" + link + "' target='_blank'> <i class='fas fa-link'></i> Xem vé</a>"
                } else {
                    return "";
                }
            },
            attributes: { style: "text-align: center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px"
        });

        dataList.push({
            field: "", title: 'In vé giấy',
            template: function (e) {
                if ($scope.objectDonHang.iD_TrangThaiDongHang == 3) {
                    let link = "https://server.lscloud.vn/Booking/PrintQRTicketByBookingCode?ID_ChiTietDonHang="
                        + e.iD_ChiTietDonHang
                        + "&ID_DonHang=" + e.iD_DonHang
                        + "&ID_MatHang=" + e.iD_MatHang
                        + "&ID_DichVu=" + e.iD_DichVu
                        + "&BookingCode=" + e.bookingCode;
                    return "<a href='" + link + "' target='_blank'> <i class='fas fa-print'></i> In vé</a>"
                } else {
                    return "";
                }
            },
            attributes: { style: "text-align: center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px"
        });


        return dataList;
    }

    function loadgridXuLy() {

        $scope.gridXuLyOptions = {
            dataSource: new kendo.data.DataSource(
                {
                    data: [],
                    schema: {
                        model: {
                            fields: {
                                thanhTien: { type: "number" }
                            }
                        }
                    },
                    pageSize: 20,
                    group: {
                        field: "bankCode",
                        aggregates: [
                            { field: "thanhTien", aggregate: "sum" }

                        ]
                    },
                    aggregate: [
                        { field: "thanhTien", aggregate: "sum" }
                    ]
                }
            ),
            groupable: true,
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".toolbarmenu").height());
                return heightGrid - 100;
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
            columns: listColumnsgridXuLy()
        };


        donHangDataService.getChiTietDichVu(__iddonhang).then(function (result) {
            $scope.gridXuLyData =
            {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                            thanhTien: { type: "number" }
                        }
                    }
                },
                pageSize: 20,
                group: {
                    field: "bankCode",
                    aggregates: [
                        { field: "thanhTien", aggregate: "sum" }

                    ]
                },
                aggregate: [
                    { field: "thanhTien", aggregate: "sum" }
                ]
            }

            $("#gridXuLy").kendoTooltip({
                filter: "img",
                position: "left",
                content: function (e) {
                    var target = e.target;
                    console.log(target[0].currentSrc);
                    return "<img style='width:280px;' src='" + target[0].currentSrc + "' />"
                }
            }).data("kendoTooltip");



            //$("#gridXuLy").data("kendoGrid").setDataSource(ds);
        })
    }

    function templateCheckBoxGrid(nameField) {
        var temp = '<input type="checkbox" ng-click="changeCheckBoxPhanQuyen($event, \'' + nameField + '\')"  #= (' +
            nameField + ' == 1) ?\'checked="checked"\' : (' + nameField + '== 2)? \'disabled = true\': "" # class="chkbx" />'; 4
        return temp;
    }

    function listColumnsgridTraHang() {
        let dataList = [];

        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({
            field: "ngayTraHang", title: $.i18n('header_thoigian'),
            template: function (dataItem) {
                return kendo.htmlEncode(kendo.toString(dataItem.ngayTraHang, formatDate));
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "120px"
        });
        dataList.push({ field: "tenQuanLy", title: $.i18n('header_nguoithuchien'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });

        return dataList;
    }
    function loadgridTraHang() {
        $scope.gridTraHangOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".toolbarmenu").height());
                return heightGrid - 100;
            },
            dataBinding: function () {
                RecordNumber = (this.dataSource.page() - 1) * this.dataSource.pageSize();
            },
            resizable: true,
            editable: false,
            selectable: "row",
            filterable: {
                mode: "row"
            },
            pageable: $scope.lang == 'vi-vn' ? pageableShort_vi : pageableShort_en,
            columns: listColumnsgridTraHang(),
            change: onChangeTraHang,
        };

        donHangDataService.getLichSuTraHang(__iddonhang).then(function (result) {
            $scope.gridTraHangData = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                            ngayTraHang: {
                                type: "date"
                            }
                        }
                    }
                },
                pageSize: 20
            };

            setTimeout(function () {
                var grid = $("#gridTraHangDetail").data("kendoGrid");
                grid.select("tr:eq(0)");
            }, 10)
        })
    }
    function listColumnsgridchitietve() {
        let dataList = [];

        dataList.push({
            field: "maVeDichVu", title: "Mã vé",
            attributes: { style: "text-align: center" },
            footerAttributes: { style: "text-align: right" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, width: "150px"
        });

        dataList.push({
            field: "tenHienThi", title: "Loại vé",
            attributes: { style: "text-align: center" },
            footerAttributes: { style: "text-align: right" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }
        });

        dataList.push({
            field: "trangThai", title: "Trạng thái",
            template: function (e) {
                switch (e.tenTrangThai) {
                    case "INACTIVE":
                        return "Chưa sử dụng";
                        break;
                    case "ACTIVE":
                        return "Đã sử dụng";
                        break;
                    case "CLOSE":
                        return "Đã sử dụng";
                        break;
                    case "LOCK":
                        return "Quá hạn sử dụng";
                        break;
                    default:
                        return "Không xác định";
                        break;
                }
                return "";
            },
            attributes: { style: "text-align: center" },
            footerAttributes: { style: "text-align: right" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, width: "120px"
        });

        dataList.push({
            field: "hanSuDung", title: "Hạn sử dụng",
            format: "{0:dd/MM/yyyy}",
            attributes: { style: "text-align: center" },
            footerAttributes: { style: "text-align: right" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, width: "150px"
        });

        dataList.push({
            field: "maBookingDichVu", title: "Booking Code",
            attributes: { style: "text-align: center" },
            footerAttributes: { style: "text-align: right" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, width: "120px"
        });

        return dataList;
    }

    function loadgridchitietve(_data) {
        $scope.gridchitietveOptions = {
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
            pageable: $scope.lang == 'vi-vn' ? pageableShort_vi : pageableShort_en,
            columns: listColumnsgridchitietve()
        };

        $timeout(function () {
            $scope.gridchitietveData = {
                data: _data,
                schema: {
                    model: {
                        fields: {
                            hanSuDung: {
                                type: "date"
                            },
                            xoa: {
                                type: "string",
                                editable: false,
                            },

                            maVeDichVu: {
                                type: "string",
                                validation: {
                                    min: 0
                                }
                            },
                            maBookingDichVu: {
                                type: "string",
                                validation: {
                                    min: 0
                                }
                            },
                            giaban: {
                                type: "number",
                                validation: {
                                    min: 0
                                }
                            }
                        }
                    }
                },

                pageSize: 20
            };

        }, 500);

    }

    function onChangeTraHang(arg) {
        let listid = []

        let grid = $("#gridTraHang").data("kendoGrid");
        grid.select().each(function () {
            let dataItem = grid.dataItem(this);
            listid.push({ idLichSuTraHang: dataItem.idLichSuTraHang });
        });

        if (listid.length >= 1) {
            loadgridTraHangDetail(listid[0].idLichSuTraHang);
        } else {
            loadgridTraHangDetail(0);
        }
    }

    function listColumnsgridTraHangDetail() {
        let dataList = [];

        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({ field: "maHang", title: $.i18n('header_mahang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px" });
        dataList.push({ field: "tenHang", title: $.i18n('header_tenhang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px" });
        dataList.push({ field: "tenDonVi", title: $.i18n('header_donvi'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px" });
        dataList.push({
            field: "soLuong", title: $.i18n('header_soluong'),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), attributes: { style: "text-align: right" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });

        return dataList;
    }
    function loadgridTraHangDetail(idLichSuTraHang) {
        $scope.gridTraHangDetailOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".toolbarmenu").height());
                return heightGrid - 100;
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
            columns: listColumnsgridTraHangDetail()
        };

        donHangDataService.getChiTietLichSuTraHang(idLichSuTraHang).then(function (result) {
            $scope.gridTraHangDetailData = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                            maHang: {
                                type: "string"
                            }
                        }
                    }
                },
                pageSize: 20
            };
        })
    }

    function listColumnsgridPhieuTraHang() {
        var dataList = [];

        dataList.push({
            field: "lanGiao", title: $.i18n('header_langiao'),
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "80px"
        });
        dataList.push({ field: "maHang", title: $.i18n('header_mahang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px" });
        dataList.push({ field: "tenHang", title: $.i18n('header_tenhang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({
            field: "soLuongTong", title: $.i18n('header_tongsoluong'),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), attributes: { style: "text-align: right" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "120px"
        });
        dataList.push({
            field: "soLuongGiao", title: $.i18n('header_soluongiao'),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), attributes: { style: "text-align: right" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "120px"
        });
        dataList.push({
            field: "soLuongTraLai", title: $.i18n('header_soluongtra'),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "120px"
        });
        dataList.push({ field: "tenKho", title: $.i18n('header_tenkho'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({ field: "ngayGiao", title: $.i18n('header_ngaygiaohang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({ field: "tenNhanVien", title: $.i18n('header_nhanviengiao'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });

        return dataList;
    }
    function loadgridPhieuTraHang() {
        $scope.gridPhieuTraHangOptions = {
            sortable: true,
            height: function () {
                return 400;
            },
            resizable: true,
            editable: true,
            save: function (e) {
                savechitiettrahang(e);
            },
            update: function (e) {
                e.success();
            },
            filterable: {
                mode: "row"
            },
            pageable: $scope.lang == 'vi-vn' ? pageableShort_vi : pageableShort_en,
            columns: listColumnsgridPhieuTraHang()
        };

        donHangDataService.getLichSuGiaoHang(__iddonhang).then(function (result) {
            $scope.gridPhieuTraHangData = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                            lanGiao: {
                                type: "number",
                                editable: false
                            },
                            maHang: {
                                type: "string",
                                editable: false
                            },
                            tenHang: {
                                type: "string",
                                editable: false
                            },
                            tenKho: {
                                type: "string",
                                editable: false
                            },
                            soLuongTong: {
                                type: "number",
                                editable: false
                            },
                            soLuongGiao: {
                                type: "number",
                                editable: false
                            },
                            ngayGiao: {
                                type: "string",
                                editable: false
                            },
                            tenNhanVien: {
                                type: "string",
                                editable: false
                            },
                            soLuongTraLai: {
                                type: "number",
                                editable: true,
                                validation: {
                                    min: 0
                                },
                            },

                        }
                    }
                },
                pageSize: 20
            };
        })
    }

    function savechitiettrahang(e) {
        e.preventDefault();
        if (e.values.soLuongTraLai > e.model.soLuongGiao) {
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n("label_soluonggtralonhonsoluongdagiao") }, 'warning');

            e.model.soLuongTraLai = e.model.soLuongGiao;
        }
        else {
            e.model.soLuongTraLai = e.values.soLuongTraLai;
        }

        $("#gridPhieuTraHang").data("kendoGrid").refresh();
    }

    function validateHuyDonHang() {
        var flag = true;
        var msg = "";

        if ($scope.objectDonHang.iD_TrangThaiGiaoHang > 1 || $scope.objectDonHang.isProcess >= 1) {
            msg = $.i18n("label_donhangdaxulykhongthehuy");
            flag = false;
        }

        if (!flag) {
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n(msg) }, 'warning');
        }
        return flag;
    }
    function validateGiaoHang() {
        let flag = true;
        let msg = "";

        let dataSource = $("#grid").data("kendoGrid").dataSource.data();
        let myGrid = $("#grid").data("kendoGrid");
        let soLuonGiaoHang = myGrid.dataSource.aggregates().giaoHang.sum;

        let data = []
        if ($scope.giaoHangToanBo) {
            soLuonGiaoHang = myGrid.dataSource.aggregates().soluong.sum - myGrid.dataSource.aggregates().dagiao.sum;
            data = dataSource;
        } else {
            data = dataSource.filter(x => (x.giaoHang > 0))
        }

        if ($scope.objectDonHang.isProcess == 2) {
            msg = $.i18n("label_donhangdahuykhongthegiao");
            flag = false;
        }

        if (flag && soLuonGiaoHang <= 0) {
            msg = $.i18n("label_banchuachonmathangdevanchuyen");
            flag = false;
        }

        if (flag) {
            let trongkho = data.filter(x => (x.idkhoxuat <= 0))
            if (trongkho.length > 0) {
                msg = $.i18n("label_chuachonkhodethuchiengiaohang");
                flag = false;
            }
        }

        if (!flag) {
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n(msg) }, 'warning');
        }

        return flag;
    }
    function validateThanhToan() {
        var msg = "";
        var flag = true;
        //if ($scope.sotienthanhtoan == 0 || $scope.sotienthanhtoan == null) {
        //    msg = $.i18n("label_nhapvapsotiencanthanhtoan");
        //    flag = false;
        //}

        if (flag && $scope.sotienthanhtoan > $scope.objectDonHang.conLai) {
            msg = $.i18n("label_tongtienchonthanhtoanlonhonsotienconlaicanthanhtoan");
            flag = false;
        }

        //if (flag && __image_url == "") {
        //    msg = "Vui lòng chọn ảnh xác nhận chuyển khoản thanh toán thành công!";
        //    flag = false;
        //}

        if (!flag) {
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n(msg) }, 'warning');
        }


        return flag;
    }

    function onUploadImageSuccess(e) {
        let data = new FormData();
        data.append('file', e.files[0].rawFile);
        var reader = new FileReader();
        reader.onload = (function () {
            var data = { base64String: reader.result };
            donHangDataService.checkAnhThanhToan(data).then(function (result) {
                console.log(result);
            });
        })
        reader.readAsDataURL(e.files[0].rawFile);
        let files = e.files[0];
        if (files.extension.toLowerCase() != ".jpg" && files.extension.toLowerCase() != ".png" && files.extension.toLowerCase() != ".jpeg") {
            e.preventDefault();
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_vuilongchonfileanhjpgpngjpeg') }, 'warning');
        } else {
            donHangDataService.uploadAnhThanhToan(data).then(function (result) {
                $("#preview").html('<div class="imgprevew"><img src="' + urlApi + result.url + '" style="width:70%" /></div>')
                __image_url = result.url;
                if (!result.flag)
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
            })
        }
    }

    //event
    $scope.getPathImage = function (path) {
        return SERVERIMAGE + path;
    }

    $scope.fomartnumber = function (data) {
        return kendo.toString(data, $rootScope.UserInfo.dinhDangSo);
    }
    $scope.fomartnumberpercent = function (data) {
        return kendo.toString(data, 'n2');
    }

    $scope.clickdetail = function () {
        $timeout(getChiTietDonHang, 200);
    }
    $scope.clickthanhtoan = function () {
        $timeout(loadgridThanhToan, 200);
        __image_url = "";
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
    }
    $scope.clickgiaohang = function () {
        $timeout(loadgridGiaoHang, 200);
    }
    $scope.clickphanquyen = function () {
        $timeout(loadgridPhanQuyen, 200);
    }
    $scope.clickxuly = function () {
        $timeout(loadgridXuLy, 200);
    }
    $scope.clicktrahang = function () {
        $timeout(loadgridTraHang, 200);
        $timeout(loadgridTraHangDetail, 200);
    }

    $scope.openkhachhang = function () {
        let url = $state.href('editkhachhang', { idkhachhang: $scope.objectDonHang.iD_KhachHang });
        window.open(url, '_blank');

        //$state.go('editkhachhang', { idkhachhang: $scope.objectDonHang.iD_KhachHang });
    }

    $scope.openchuongtrinhkhuyenmai = function () {
        if ($scope.objectDonHang.iD_CTKM > 0) {
            let url = $state.href('editkhuyenmai', { idkhuyenmai: $scope.objectDonHang.iD_CTKM });
            window.open(url, '_blank');
        }
    }

    $scope.deleterow = function (e) {
        let flag = true;
        let message = "";

        let myGrid = $('#grid').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;

        let tongTien = $scope.objectDonHang.tongTien * 1;
        let tongTienChietKhau = $scope.objectDonHang.tongTienChietKhau * 1;
        let tienDaThanhToan = $scope.objectDonHang.tienDaThanhToan * 1;
        let tienXoa = dataItem.tongTien * 1;
        let daGiaoXoa = dataItem.dagiao * 1;

        let sumTienSauKhiDelete = (tongTien + tongTienChietKhau) - tienXoa;

        if ($scope.objectDonHang.isProcess > 0) {
            flag = false;
            message = $.i18n('label_donhangdaxulykhongthehuy');
        } else if (tienDaThanhToan > sumTienSauKhiDelete) {
            flag = false;
            message = $.i18n('label_tongtiensaukhixoanhohonsotiendathanhtoan');
        } else if (daGiaoXoa > 0) {
            flag = false;
            message = $.i18n('label_mathangdagiaokhongthehuy!');
        } else {
            __chiTietDonHangXoa.push(dataItem);
        }

        if (flag) {
            __arridhangchitiet = __arridhangchitiet.filter(item => item != dataItem.idhanghoa);
            try {
                myGrid.dataSource.remove(dataItem);
            } catch (ex) { }

            loadgridhangchon();
            updatethanhtien();
        } else
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n(message) }, 'warning');
    }

    $scope.luudonhang = function () {
        commonOpenLoadingText("#btn_luudonhang");

        let data = $("#grid").data("kendoGrid").dataSource.data();
        let _chitietdonhang = [];

        let _tongChietKhauMatHang = 0;
        let _thanhTienMatHang = 0;
        for (let i = 0; i < data.length; i++) {
            let lstdv = [];
            $.each(data[i].lstDichVu, function (index, item) {
                lstdv.push({
                    ID_HangHoa: item.iD_HangHoa,
                    ID_DichVu: item.iD_DichVu,
                    SoLuong: item.soLuong,
                    GiaBan: item.giaBan,
                    Loai: item.loai,
                    TenHienThi: item.tenHienThi,
                })
            })
            let item = {
                idchitietdonhang: data[i].idchitietdonhang,
                iddonhang: data[i].iddonhang,
                idhanghoa: data[i].idhanghoa,
                soluong: data[i].soluong,
                tongTien: data[i].tongTien,
                ghichu: data[i].ghichu,
                hinhthucban: data[i].hinhthucban,
                giakhac: data[i].giakhac,
                giaban: data[i].giaban,
                giabuon: data[i].giabuon,
                giale: data[i].giale,
                chietkhauphantram_banle: data[i].chietkhauphantram_banle,
                chietkhautien_banle: data[i].chietkhautien_banle,
                chietkhauphantram_banbuon: data[i].chietkhauphantram_banbuon,
                chietkhautien_banbuon: data[i].chietkhautien_banbuon,
                phantramhaohut: data[i].phantramhaohut,
                soluonghaohut: data[i].soluonghaohut,
                idnhanvien: data[i].idnhanvien,
                idKho: data[i].idKho,
                idquanly: data[i].idquanly,
                chietkhauphantram: data[i].chietkhauphantram,
                chietkhautien: data[i].chietkhautien,
                tongtienchietkhau: data[i].tongtienchietkhau,
                idctkm: data[i].idctkm,
                iddanhmuc: data[i].iddanhmuc,
                loaihanghoa: data[i].loaihanghoa,
                idhaohut: data[i].idhaohut,
                HangKhuyenMai: data[i].HangKhuyenMai,
                lstDichVu: lstdv,
                ngay: kendo.toString(data[i].ngay, formatDateTimeFilter)
            }
            _tongChietKhauMatHang += data[i].tongtienchietkhau;
            _thanhTienMatHang += (data[i].tongTien - data[i].tongtienchietkhau);

            if (item.hinhthucban == 2)
                item.giakhac = item.giaban;

            _chitietdonhang.push(item);
        }

        let tongtienchietkhau_donhang = 0;
        if ($scope.objectDonHang.iD_CTKM > 0) {
            tongtienchietkhau_donhang = (_thanhTienMatHang * $scope.objectDonHang.chietKhauPhanTramTheoCTKM * 1 / 100)
                + (_thanhTienMatHang * $scope.objectDonHang.chietKhauPhanTramKhac * 1 / 100)
                + $scope.objectDonHang.chietKhauTienTheoCTKM * 1
                + $scope.objectDonHang.chietKhauTienKhac * 1;
            $scope.objectDonHang.tongTienChietKhau = tongtienchietkhau_donhang + _tongChietKhauMatHang;
        }

        $scope.objectDonHang.tongtien = _thanhTienMatHang - tongtienchietkhau_donhang * 1;
        //$scope.objectDonHang.iD_TrangThaiDongHang = $scope.trangthaidonhang.iD_TrangThaiDonHang;

        let obj = {
            donHang: $scope.objectDonHang,
            chitietdonhang: _chitietdonhang,
            chiTietDonHangXoa: __chiTietDonHangXoa
        };

        donHangDataService.savedv(obj).then(function (result) {
            if (result.flag) {
                getChiTietDonHang();
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_capnhatdonhangthanhcong') }, 'success');
            } else {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixarakhongtheluudonhang') }, 'warning');
            }

            commonCloseLoadingText("#btn_luudonhang");
        })
    }

    $scope.xulydonhang = function () {
        commonOpenLoadingText("#btn_xulydonhang");
        $("#btn_xulydonhang").hide();
        donHangDataService.xuLyDonHang($scope.objectDonHang.iD_DonHang).then(function (result) {
            if (result.flag) {
                getChiTietDonHang();
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_capnhatdonhangthanhcong') }, 'success');
            } else {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixarakhongtheluudonhang') }, 'warning');
            }
            $("#btn_xulydonhang").show();
            commonCloseLoadingText("#btn_xulydonhang");
            loadgridXuLy();
        })
    }


    $scope.huydonhang = function () {
        if (validateHuyDonHang()) {
            $scope.formhuydonhang.center().open();
        }
    }
    $scope.trahang = function () {
        $scope.formtrahang.center().open();
        loadgridPhieuTraHang();
    }
    $scope.giaohang = function () {
        $scope.giaoHangToanBo = false;
        if (validateGiaoHang()) {
            $scope.formgiaohang.center().open();
        }
    }
    $scope.giaotatcahang = function () {
        $scope.giaoHangToanBo = true;
        if (validateGiaoHang()) {
            $scope.formgiaohang.center().open();
        }
    }
    $scope.inhoadon = function () {
        //$scope.objprint
        //$scope.list
        $scope.objprint.tencongty = $rootScope.UserInfo.tencongty;
        if ($rootScope.lang == 'vi-vn')
            printer.printFromScope("/app/components/donhang/inDonHangPOSView.html", $scope);
        /*printer.printFromScope("/app/components/donhang/inDonHangTakeAwayView.html", $scope);*/
        else
            printer.printFromScope("/app/components/donhang/inDonHangENView.html", $scope);
    }
    $scope.quayvedonhang = function () {
        $state.go('danhsachdonhangpchl');
    }

    $scope.donghuydonhang = function () {
        $scope.formhuydonhang.center().close();
    }
    $scope.apdunghuydonhang = function () {
        openConfirm($.i18n("label_bancochacchanmuonhuydonhang"), 'apDungXoaDonHang', null, __iddonhang);
    }
    $scope.apDungXoaDonHang = function () {
        commonOpenLoadingText("#btn_apdunghuydonhang");

        donHangDataService.huydonhang(__iddonhang, $scope.lyDoHuy).then(function (result) {
            if (result.flag) {
                getChiTietDonHang();
                $scope.formhuydonhang.center().close();
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_capnhathuyhangthanhcong') }, 'success');
            } else {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloisayrakhongthehuydonhang') }, 'warning');
            }

            commonCloseLoadingText("#btn_apdunghuydonhang");
        })
    }

    $scope.donggiaohang = function () {
        $scope.formgiaohang.center().close();
    }
    $scope.apdunggiaohang = function () {
        commonOpenLoadingText("#btn_apdunggiaohang");

        let dataSource = $("#grid").data("kendoGrid").dataSource.data();

        let chitietgiaohang = [];
        angular.forEach(dataSource, function (obj) {
            let sogiao = obj.giaoHang;
            if ($scope.giaoHangToanBo) {
                sogiao = parseFloat(obj.soluong) - parseFloat(obj.dagiao);
            }
            if (sogiao > 0) {
                let item = {
                    ghichu: $scope.ghiChuGiaoHang,
                    hinhthucban: obj.hinhthucban,
                    idhang: obj.idhanghoa,
                    soluonggiao: sogiao,
                    idkho: obj.idkhoxuat,
                    idhaohut: obj.idhaohut,
                }
                chitietgiaohang.push(item);
            }
        });

        let dulieugiaohang = {
            "iddonhang": __iddonhang,
            "idquanly": $scope.objectDonHang.iD_QLLH,
            "chitietgiaohang": chitietgiaohang,
            "token": "6e22b116f5111220741848ccd290e9e9062522d88a1fb00ba9b168db7a480271"
        }
        var type = $scope.giaoHangToanBo ? "giaohang" : "save";
        let obj = {
            "type": type,
            "dulieugiaohang": JSON.stringify(dulieugiaohang),
            "url": SERVERIMAGE + "/AppGiaoHang.aspx"
        }

        donHangDataService.giaohang(obj).then(function (result) {
            if (result.flag) {
                $scope.formgiaohang.center().close();
                getChiTietDonHang();

                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
            } else {
                $scope.formgiaohang.center().close();
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
            }

            $scope.ghiChuGiaoHang = '';

            commonCloseLoadingText("#btn_apdunggiaohang");
        })
    }

    $scope.capnhattrahang = function () {

        let data = $("#gridPhieuTraHang").data("kendoGrid").dataSource.data();

        let _chiTietHangTra = [];
        for (var i = 0; i < data.length; i++) {
            if (data[i].soLuongTraLai > 0) {
                var item = {
                    idLichSuGiaoHang: data[i].id,
                    idhanghoa: data[i].idMatHang,
                    soluong: data[i].soLuongTraLai,
                    giaban: data[i].giaBan,
                    idKho: data[i].idKho,
                    hinhthucban: data[i].hinhthucban
                }
                _chiTietHangTra.push(item);
            }
        }

        if (data == null || data.length == 0) {
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n("label_banchuachonmathangtra") }, 'warning');
        } else {

            commonOpenLoadingText("#btn_capnhattrahang");
            let obj = {
                idDonHang: __iddonhang,
                chiTietHangTra: _chiTietHangTra
            };

            donHangDataService.trahang(obj).then(function (result) {
                if (result.flag) {
                    $scope.formtrahang.center().close();
                    getChiTietDonHang();

                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                } else {
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
                }

                commonCloseLoadingText("#btn_capnhattrahang");
            })
        }
    }

    $scope.soTienThanhToanOnchange = function () {
        if ($scope.sotienthanhtoan > $scope.objectDonHang.conLai) {
            $scope.sotienthanhtoan = $scope.objectDonHang.conLai;

            Notification({ title: $.i18n('label_thongbao'), message: $.i18n("label_sotiennhapvaolonhonsotieconlai") }, 'warning');
        }
    }
    $scope.thanhtoan = function () {
        if ($scope.thanhtoantoanbo)
            $scope.sotienthanhtoan = $scope.objectDonHang.conLai;
        if (validateThanhToan()) {
            commonOpenLoadingText("#btn_thanhtoan");

            let type = ($scope.sotienthanhtoan == $scope.objectDonHang.conLai) ? "thanhtoan" : "save";
            let obj = {
                "type": type,
                "iddonhang": __iddonhang,
                "idnhanvien": ($rootScope.isAdmin == 1) ? 0 : $rootScope.UserInfo.iD_QuanLy,
                "ghichu": $scope.ghichuthanhtoan,
                "idquanly": ($rootScope.isAdmin == 1) ? $rootScope.UserInfo.iD_QuanLy : 0,
                "tien": $scope.sotienthanhtoan,
                "token": "6e22b116f5111220741848ccd290e9e9062522d88a1fb00ba9b168db7a480271",
                "url": __image_url
            }

            donHangDataService.thanhtoan(obj).then(function (result) {
                if (result.flag) {
                    loadgridThanhToan();

                    let sotienthanhtoan = $scope.sotienthanhtoan * 1;
                    let tienDaThanhToan = $scope.objectDonHang.tienDaThanhToan * 1;
                    let conLai = $scope.objectDonHang.conLai * 1;
                    let tongTien = $scope.objectDonHang.tongTien * 1;

                    tienDaThanhToan += sotienthanhtoan;
                    conLai = tongTien - tienDaThanhToan;

                    $scope.objectDonHang.tienDaThanhToan = tienDaThanhToan;
                    $scope.objectDonHang.conLai = conLai;


                    $scope.sotienthanhtoan = 0;
                    $scope.thanhtoantoanbo = false;
                    $scope.ghichuthanhtoan = '';
                    __image_url = "";
                    $("#preview").html("");
                    if ($scope.objectDonHang.conLai == 0) {
                        getChiTietDonHang();
                    }
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                } else {
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
                }

                commonCloseLoadingText("#btn_thanhtoan");
            })
        }
    }

    $scope.capnhatphanquyen = function () {
        commonOpenLoadingText("#btn_capnhatphanquyen");

        let obj = $("#gridPhanQuyen").data("kendoGrid").dataSource.data()

        donHangDataService.phanquyen(obj).then(function (result) {
            if (result.flag) {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
            } else {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
            }

            commonCloseLoadingText("#btn_capnhatphanquyen");
        })
    }
    $scope.changeCheckBoxPhanQuyen = function (e, name) {
        var checked = e.currentTarget.checked;
        var dataItem = $("#gridPhanQuyen").data("kendoGrid").dataItem(e.currentTarget.closest("tr"));
        if (checked) {
            dataItem[name] = 1;
        } else {
            dataItem[name] = 0;
        }
    }

    $scope.openWindowHangChon = function () {
        __idkhachhang = $scope.objectDonHang.iD_KhachHang;
        loadtreeView();
        $scope.windowHangChon.center().open();
    }


    $scope.addrow = function (e) {

        let myGrid = $('#gridHangChon').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;

        let giaban = dataItem.giaLe;
        let item = {
            chietkhauphantram: 0,
            chietkhauphantram_banbuon: 0,
            chietkhauphantram_banle: 0,
            chietkhautien: 0,
            chietkhautien_banbuon: 0,
            chietkhautien_banle: 0,
            tongtienchietkhau: 0,
            chietkhautien: 0,
            ghichu: "",
            giaban: dataItem.giaLe,
            giabuon: dataItem.giaBuon,
            giakhac: 0,
            hinhthucban: 1,
            idctkm: 0,
            giale: dataItem.giaLe,
            idchitietdonhang: 0,
            isDichVu: dataItem.isDichVu,
            lstDichVu: dataItem.lstDichVu,
            mahang: dataItem.maHang,
            soluong: 1,
            tendonvi: dataItem.tenDonVi,
            tenhang: dataItem.tenHang,
            tongTien: dataItem.giaLe,
            idhanghoa: dataItem.idMatHang,
            iddonhang: $scope.objectDonHang.iD_DonHang
        }

        try {
            let myGridChiTiet = $("#grid").data("kendoGrid");
            myGridChiTiet.dataSource.add(item);
            myGridChiTiet.refresh();
        } catch (ex) { }

        __arridhangchitiet.push(dataItem.idMatHang);

        myGrid.dataSource.remove(dataItem);

        $timeout(function () {
            updatethanhtien();
        }, 100);

    }

    $scope.openformchitietve = function () {
        let row = $(this).closest("tr");
        DATAITEM = row.prevObject[0].dataItem;
        let arrchitietve = JSON.parse(JSON.stringify(DATAITEM.dschitietmathang));

        $scope.formdetailve.center().open();
        loadgridchitietve(arrchitietve);
    }

    $scope.openformchitiet = function () {
        let row = $(this).closest("tr");
        DATAITEM = row.prevObject[0].dataItem;
        let arrchitiet = JSON.parse(JSON.stringify(DATAITEM.lstDichVu));

        $.each(arrchitiet, function (index, item) {
            item.tong = item.soLuong * item.giaBan;
        })
        __curidhanghoa = DATAITEM.idhanghoa;
        __curhinhthucban = DATAITEM.hinhthucban;
        __curidhaohut = DATAITEM.idhaohut;

        $scope.formdetail.center().open();

        loadgridchitiet(arrchitiet);
    }

    function listColumnsgridchitiet() {
        let dataList = [];
        dataList.push({
            field: "tenHienThi", title: "Dịch vụ", headerAttributes: {
                class: "table-header-cell",
                style: "text-align: center"
            }
            , filterable: false, width: "250px"
        });
        dataList.push({
            field: "giaBan",
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            title: "Giá gốc dịch vụ", attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: false, width: "150px"
        });
        dataList.push({
            field: "soLuong",
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            title: "Số lượng", attributes: { class: "text-center" },
            editor: function (container, options) {
                if (options.model.loai == 1) {
                    //optional
                    $('<input name="' + options.field + '"/>').appendTo(container).kendoNumericTextBox({
                        value: 0,
                        min: 0,
                        change: function (e) {
                            options.model.soLuong = e.sender.value();
                        }
                    })
                } else {
                    //fixed
                    $('<input name="' + options.field + '"/>').appendTo(container).kendoNumericTextBox({
                        max: options.model.soLuong,
                        min: options.model.soLuong
                    })
                }
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: false, width: "150px"
        });
        dataList.push({
            field: "loai",
            title: "Loại",
            attributes: { class: "text-center" },
            template: function (e) {
                if (e.loai == 1) {
                    return "Optional";
                } else if (e.loai == 2) {
                    return "Fixed";
                }
            },
            headerAttributes: {
                "class": "table-header-cell",
                style: "text-align: center"
            }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "tong", title: $.i18n('header_tongtien'),
            editor: numberEditor,
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            attributes: { style: "text-align: right" }, footerAttributes: { style: "text-align: right" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        });

        return dataList;
    }

    function loadgridchitiet(_data) {
        $scope.gridchitietOptions = {
            sortable: true,
            height: function () {
                return 350;
            },
            dataBinding: function () {
                RecordNumber = (this.dataSource.page() - 1) * this.dataSource.pageSize();
            },
            resizable: true,
            editable: ($scope.objectDonHang.isProcess > 0 || $scope.objectDonHang.iD_TrangThaiThanhToan > 1) ? false : true,
            filterable: {
                mode: "row"
            },
            pageable: $scope.lang == 'vi-vn' ? pageableShort_vi : pageableShort_en,
            save: function (e) {
                savechitietdonhangchitiet(e);
            },
            update: function (e) {
                e.success();
            },
            columns: listColumnsgridchitiet()
        };

        $timeout(function () {
            $scope.gridchitietData = {
                data: _data,
                schema: {
                    model: {
                        ID: 'id',
                        fields: {
                            iD_HangHoa: {
                                type: "number", editable: false
                            },
                            iD_DichVu: {
                                type: "number", editable: false
                            },
                            soLuong: {
                                type: "number", editable: true
                            },
                            giaBan: {
                                type: "number", editable: false
                            },
                            tong: {
                                type: "number", editable: false
                            },
                            loai: {
                                type: "number", editable: false
                            },
                            tenHienThi: {
                                type: "text", editable: false
                            }
                        }
                    }
                },
                pageSize: 20,
                aggregate: [
                    { field: "tong", aggregate: "sum" }
                ]
            };
        }, 500);

    }

    function getHeaderXuLyDonHangDichVu(e) {
        console.log(e);
        //let site = __site.filter(s => (s.bankCode == e.value))[0];
        let label = "<div style='width:100%;display:flex'> <p style='width:200px'>Thông tin thanh toán</p>"
            + "<p style='width:150px'>Số tiền: " + kendo.toString(e.thanhTien.sum, $rootScope.UserInfo.dinhDangSo) + "</p>"
            + "<p style='width:150px'> BookingCode: " + e.items[0].bookingCode + "</p></<div>"
        let img = '<img style="width: 50px; margin: auto;" src="https://img.vietqr.io/image/' +
            e.items[0].bankCode + '-' + e.items[0].bankAccNumber + '-compact2.png?amount='
            + e.thanhTien.sum
            + '&addInfo=bookingcode '
            + e.items[0].bookingCode
            + ' ' + e.items[0].ghiChu
            + '&accountName='
            + e.items[0].ghiChu
            + '"/>';
        //if ($rootScope.UserInfo.level == 1) {
        //if (e.items[0].bookingCode != null) {
        return label + img;
        //} else {
        //    return "Vui lòng thực hiện thanh toán!"
        //}
        //} else {
        /*    return label;*/
        //}
    }

    function savechitietdonhangchitiet(e) {
        let grid = e.sender;
        let model = e.model;

        let ischange = false;

        let _giaban = model.giaBan;
        let _soluong = model.soLuong;

        if (e.values.soLuong != null) {
            _soluong = e.values.soLuong;
            ischange = true;
        }

        if (e.values.giaBan != null) {
            _giaban = e.values.giaBan;
            ischange = true;
        }

        if (ischange) {
            model.tong = _soluong * _giaban;
        }

        grid.refresh();
    }

    function numberEditor(container, options) {
        if (!options.model.isDichVu) {
            $('<input name="' + options.field + '"/>')
                .appendTo(container)
                .kendoNumericTextBox({
                    min: 0,
                    format: __format,
                    decimals: __digits
                })
        }
    }

    function dateEditor(container, options) {
        let input = $('<input name="' + options.field + '"/>');
        input.appendTo(container)
            .kendoDatePicker({
                dateInput: false,
                min: new Date(),
                max: new Date().addDays(1)
            })
        input.attr("disabled", "disabled");
    }


    $scope.luuchitiet = function () {
        let chitietdh = $("#grid").data("kendoGrid").dataSource.data();

        let items = chitietdh.filter(ct => (ct.idhanghoa == __curidhanghoa));
        if (items.length > 0) {
            ct = items[0];
            let _tongTien = 0;
            let _listdichvu = $scope.gridchitiet.dataSource.data();
            //let _khuyenMai = ct.khuyenmai_combo;
            //let _listChiTietKhuyenMai = ct.khuyenmai_combo.chiTietCTKM;

            //lấy chi tiết khuyến mãi của mặt hàng
            //let _chiTietKhuyenMai = null;
            //if (_listChiTietKhuyenMai != null) {
            //    let km = _listChiTietKhuyenMai.filter(x => (x.iD_Hang == ct.idhanghoa))
            //    if (km.length > 0)
            //        _chiTietKhuyenMai = km[0];
            //}

            //if (_khuyenMai == null || _chiTietKhuyenMai == null) {
            //    ct.chietkhauphantram = 0;
            //    ct.chietkhautien = 0;
            //    ct.tongtienchietkhau = 0;
            //}
            if (_listdichvu != null) {
                $.each(_listdichvu, function (index, item) {
                    _tongTien += item.tong;
                })

            }

            //tính tổng tiền chiết khấu và tổng tiền
            ct.tongTien = _tongTien;
            DATAITEM.set('lstDichVu', _listdichvu);
            DATAITEM.set('dschitietmathang', ct.dschitietmathang);
            DATAITEM.set('soluong', ct.soluong);
            DATAITEM.set('chietkhauphantram', ct.chietkhauphantram);
            DATAITEM.set('chietkhautien', ct.chietkhautien);
            DATAITEM.set('tongtienchietkhau', ct.tongtienchietkhau);
            DATAITEM.set('ghichu', ct.ghichu);
            DATAITEM.set('tongTien', ct.tongTien);

        }

        $scope.formdetail.center().close();

        $("#grid").data("kendoGrid").refresh();
        $timeout(function () {
            updatethanhtien();

            if ($scope.$root.$$phase != '$apply' && $scope.$root.$$phase != '$digest') {
                $scope.$apply();
            }
        }, 100);
    }

    init();

})