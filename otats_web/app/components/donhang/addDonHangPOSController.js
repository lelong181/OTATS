

angular.module('app').controller('addDonHangPOSController', function ($rootScope, $scope, $state, $stateParams, $timeout, Notification, donHangDataService, ComboboxDataService, printer) {
    CreateSiteMap();

    let __arridhangchitiet = [];
    let __arrhinhthucban = [
        { id: 0, name: $.i18n('label_giabanle') },
        { id: 1, name: $.i18n('label_giabanbuon') },
        { id: 2, name: $.i18n('label_giakhac') },
    ]
    let __arrtypeview = [
        { id: 1, name: "Grid view" },
        { id: 2, name: "List view " },
    ]
    let __lsaccountcode = "";
    let __lsbookingcode = "";
    let __arrkhuyenmaiingrid = [];
    let __arrkhuyenmaidonhang = [];
    let __arrhaohutingrid = [];
    let __idnhom = 0;
    let __idkhachhang = 0;
    let __idhinhthucban = __arrhinhthucban[0].id;
    let __namehinhthucban = __arrhinhthucban[0].name;
    let __combohinhthucban = __arrhinhthucban[0];
    let __combotypeview = __arrtypeview[0];
    let __idkhuyenmai = 0;
    let __idnhanvien = $rootScope.UserInfo.iD_QuanLy;
    let __sudungbanggia = false;

    let __curidhanghoa = 0;
    let __curhinhthucban = -1;
    let __curidhaohut = -1;
    let DATAITEM = null;

    let __format = "{0:n0}";
    let __digits = 0;

    function init() {
        initcauhinh();

        initfield();
        initCombobox();

        inittreeview();
        loadgrid();
    }

    function initfield() {
        $scope.mindate = new Date();
        $scope.obj_TuNgay = new Date();
        $scope.makh = '';
        $scope.dienthoai = '';
        $scope.ghichu = '';
        $scope.tienhang = 0;
        $scope.tienchietkhau = 0;
        $scope.tienthanhtoan = 0;
        $scope.chietkhautien = 0;
        $scope.chietkhauphantram = 0;
        $scope.chieukhauphantramkhac = 0;
        $scope.chieukhautienkhac = 0;

    }
    function initCombobox() {
        //donHangDataService.getcombonhanvienlap().then(function (result) {
        //    $scope.nhanvienData = result.data;

        //    if (__idnhanvien > 0) {
        //        $timeout(function () {
        //            $("#nhanvien").data("kendoComboBox").value(__idnhanvien);
        //        }, 100);
        //    } else {
        //        $("#nhanvien").data("kendoComboBox").value("")
        //    }
        //});
        ComboboxDataService.getLoaiKhachHang().then(function (result) {
            $scope.loaikhachhangData = result.data;
        });

        loadlistkhachhang();

        donHangDataService.getcombokhuyenmai(-1).then(function (result) {
            $scope.khuyenmaiData = result.data;
            __arrkhuyenmaidonhang = result.data;
        });
        donHangDataService.getcombokhuyenmai(0).then(function (result) {
            __arrkhuyenmaiingrid = result.data;
            __arrkhuyenmaiingrid.unshift({
                tenCTKM: $.i18n("label_chonchuongtrinhkhuyenmai"),
                iD_CTKM: -1
            });
        });

        donHangDataService.getcombohaohut().then(function (result) {
            __arrhaohutingrid = result.data;
            __arrhaohutingrid.unshift({
                tenLoaiHaoHut: $.i18n("label_chonloaihaohut"),
                id: -1
            });
        });

        $scope.hinhthucbanData = __arrhinhthucban;
        $scope.typeViewData = __arrtypeview;
        $scope.comboTypeViewOptions = {
            clearButton: false
        }
        $timeout(function () {
            $scope.khachhangOptions = {
                filter: "contains",
                suggest: true,
                footerTemplate: '<div class="footer-combobox">' +
                    '<button ng-click="openWindowThemKhachHang()" class="btn btn-info btn-sm ml-2 mt-2"><i class="fas fa-plus"></i> ' +
                    $.i18n('button_themmoi') +
                    '</button>' +
                    '</div>',
            }

            $("#hinhthucban").data("kendoComboBox").value(__idhinhthucban);
            $scope.typeViewSelect = __combotypeview;
        }, 100);
    }
    function initcauhinh() {
        __format = "{0:" + $rootScope.UserInfo.dinhDangSo + "}";
        __digits = ($rootScope.UserInfo.dinhDangTienSoThapPhan == 1) ? 2 : 0;

        ComboboxDataService.getcauhinhchung().then(function (result) {
            __sudungbanggia = result.data.suDungBangGiaLoaiKhachHang;
        });
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

        $scope.selectOptions = {
            dataSource: dataSource,
            checkAll: true,
            placeholder: "Chọn ngành hàng",
            dataTextField: "name",
            dataValueField: "id",
            valuePrimitive: true,
            checkboxes: true,
            autoBind: false,
            autoClose: false,
            change: onSelectNhom
        };


        loadtreeView();
    }
    function loadtreeView() {
        donHangDataService.getnhommathangtheophanquyen().then(function (result) {
            let data = result.data;
            data.splice(0, 1);
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

        $scope.dropdownHangChon.setDataSource(dataSource);
        $(".k-checkbox-label").text("Chọn tất cả ngành hàng");
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
        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({
            field: "anhDaiDien", title: $.i18n('header_anhdaidien'),
            template: function (dataItem) {
                if (dataItem.anhDaiDien == null || dataItem.anhDaiDien == '')
                    return ''
                else {
                    let src = SERVERIMAGE + dataItem.anhDaiDien;
                    return '<img src="' + src + '" alt="" class="img-avatar rounded-circle">';
                }
            },
            attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: false, width: "100px"
        });
        dataList.push({ field: "maHang", title: $.i18n('header_mahang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "80px" });
        dataList.push({ field: "tenHang", title: $.i18n('header_tenhang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "220px" });
        dataList.push({ field: "tenDonVi", title: $.i18n('header_donvi'), attributes: { class: "text-center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "80px" });
        dataList.push({ field: "giaBuon", title: $.i18n('header_giabanbuon'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), attributes: { class: "text-right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px" });
        dataList.push({ field: "giaLe", title: $.i18n('header_giabanle'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), attributes: { class: "text-right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px" });
        //dataList.push({ field: "soLuongTon", title: $.i18n('header_soluongton'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), attributes: { class: "text-right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px" });

        return dataList;
    }
    function loadgridhangchon() {
        kendo.ui.progress($("#gridHangChon"), true);
        //Gridview
        $scope.gridHangChonOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height());
                return heightGrid - 80;
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
        //Listview
        $scope.listHangChonOptions = {

            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height());
                return heightGrid - 80;
            },
            dataSource: {
                schema: {
                    model: { id: "ProductID" },
                    fields: [
                        { UnitQty: "number" },
                        { Price: "number" }
                    ]
                },
                aggregate: [
                    { field: "Price", aggregate: "sum" },
                ],
                data: []
            },
            selectable: true,
            pageable: false,
            //change: onlistviewchange,
            template: kendo.template($("#templateOrder").html())
        }


        donHangDataService.getdanhsachmathangbyidkhachhangmultiidnhom(__idkhachhang, __idnhom).then(function (result) {
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
                pageSize: 9999
            };
            kendo.ui.progress($("#gridHangChon"), false);
            kendo.ui.progress($("#listviewHangChon"), false);

        });
    }

    function onlistviewchange(e) {
        var lview = e.sender;
        var data = lview.dataSource.view(),
            selected = $.map(lview.select(), function (i) {
                console.log(i);
                var dataItem = data[$(i).index()];
                let myGrid = $('#listviewHangChon').data("kendoListView");

                let giaban = (__idhinhthucban == 0) ? dataItem.giaLe : (__idhinhthucban == 1) ? dataItem.giaBuon : 0;

                let item = {
                    mahang: dataItem.maHang,
                    tenhang: dataItem.tenHang,
                    isdichvu: dataItem.isDichVu,
                    sitecode: dataItem.siteCode,
                    servicerateid: dataItem.serviceRateID,
                    soluong: 1,
                    soton: dataItem.soLuongTon,
                    idhanghoa: dataItem.idMatHang,
                    chietkhautien: 0,
                    chietkhauphantram: 0,
                    tongtien: giaban,
                    giaban: giaban,
                    tongtienchietkhau: 0,
                    hinhthucban: __idhinhthucban,
                    hinhthucban_name: __namehinhthucban,
                    hinhthucban_combo: __combohinhthucban,
                    phantramhaohut: 0,
                    soluonghaohut: 0,
                    giale: dataItem.giaLe,
                    giabuon: dataItem.giaBuon,
                    khuyenmai_combo: __arrkhuyenmaiingrid[0],
                    idctkm: __arrkhuyenmaiingrid[0].iD_CTKM,
                    tenCTKM: __arrkhuyenmaiingrid[0].tenCTKM,
                    haohut_combo: __arrhaohutingrid[0],
                    tenLoaiHaoHut: __arrhaohutingrid[0].tenLoaiHaoHut,
                    idhaohut: __arrhaohutingrid[0].id,
                    dschitietmathang: []
                }

                try {
                    let myGridChiTiet = $("#grid").data("kendoGrid");
                    myGridChiTiet.dataSource.add(item);
                } catch (ex) { }

                __arridhangchitiet.push(dataItem.idMatHang);

                myGrid.dataSource.remove(dataItem);

                $timeout(function () {
                    updatethanhtien();
                }, 100);
            });
    }

    function onSelectNhom(e) {
        __idnhom = $scope.dropdownHangChon.value().join(',');
        console.log($scope.dropdownHangChon.value());
        kendo.ui.progress($("#gridHangChon"), true);
        kendo.ui.progress($("#listviewHangChon"), true);
        setTimeout(function () { loadgridhangchon() }, 1500);
    }

    function loadlistkhachhang() {
        __idkhachhang = 0;

        donHangDataService.getlistkhachhangbyidnhanvien(__idnhanvien).then(function (result) {
            $scope.khachhangData = result.data;
            $scope.makh = '';
            $scope.dienthoai = '';
        });
    }

    function listColumnsgrid() {
        var dataList = [];

        dataList.push({
            field: "xoa", title: $.i18n('button_xoa'),
            template: '<button ng-click="deleterowgrid()" class="btn btn-link btn-menubar" title ="' + $.i18n('button_xoa') + '" ><i class="fas fa-trash fas-sm color-danger"></i></button> ',
            attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "60px"
        });
        dataList.push({
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, selectable: true, width: "60px",
            attributes: { class: "text-center" },
            footerTemplate: '<button ng-show="objprint.maThamChieu != null" ng-click="printLabel()" class="btn btn-link btn-menubar" title ="In tem" ><i class="fas fa-print fas-sm color-success"></i></button> ',
        });
        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        //dataList.push({
        //    field: "tacvu", title: $.i18n('header_tacvu'),
        //    template: '<button ng-click="openformchitiet()" class="btn btn-link btn-menubar" title ="' + $.i18n('label_chonmathang') + '" ><i class="fas fa-tags fas-sm color-infor"></i></button> ',
        //    width: "100px",
        //    filterable: false,
        //    headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
        //    attributes: { class: "text-center" },
        //});
        dataList.push({
            field: "mahang", title: $.i18n('header_mahang'),
            footerTemplate: $.i18n('label_total'),
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "80px"
        });
        dataList.push({ field: "tenhang", title: $.i18n('header_tenhang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({
            field: "soluong", title: $.i18n('header_soluong'),
            editor: numberEditor,
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            attributes: { style: "text-align: right" }, footerAttributes: { style: "text-align: right" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "80px"
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
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            attributes: { style: "text-align: right" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "120px"
        });
        //dataList.push({
        //    field: "hinhthucban_name", title: $.i18n('header_hinhthucban'),
        //    editor: hinhThucBanDropDownEditor,
        //    headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
        //    filterable: defaultFilterableGrid, width: "120px"
        //});
        dataList.push({
            field: "tongtien", title: $.i18n('header_tongtien'),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('tongtien.sum', $rootScope.UserInfo.dinhDangSo),
            attributes: { style: "text-align: right" }, footerAttributes: { style: "text-align: right" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "tongtienchietkhau", title: $.i18n('header_tongtienchietkhau'),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('tongtienchietkhau.sum', $rootScope.UserInfo.dinhDangSo),
            attributes: { style: "text-align: right" }, footerAttributes: { style: "text-align: right" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });

        dataList.push({
            field: "tenCTKM", title: $.i18n('header_khuyenmai'),
            editor: khuyenmaiDropDownEditor,
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "250px"
        });

        dataList.push({
            field: "chietkhautien", title: $.i18n('header_chietkhautien'),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('chietkhautien.sum', $rootScope.UserInfo.dinhDangSo),
            attributes: { style: "text-align: right" }, footerAttributes: { style: "text-align: right" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "120px"
        });
        dataList.push({
            field: "chietkhauphantram", title: $.i18n('header_chietkhauphantram'),
            format: formatNumberInGrid('n2'),
            attributes: { style: "text-align: right" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        });

        //dataList.push({
        //    field: "tenLoaiHaoHut", title: $.i18n('header_haohut'),
        //    editor: haohutDropDownEditor,
        //    headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "200px"
        //});
        //dataList.push({
        //    field: "phantramhaohut", title: $.i18n('header_haohutphantram'),
        //    format: formatNumberInGrid('n2'),
        //    attributes: { style: "text-align: right" },
        //    headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        //});

        return dataList;
    }
    function loadgrid() {
        $scope.gridOptions = {
            sortable: true,
            height: function () {
                return 380;
            },
            dataBinding: function () {
                RecordNumber = (this.dataSource.page() - 1) * this.dataSource.pageSize();
            },
            resizable: true,
            editable: true,
            filterable: {
                mode: "row"
            },
            //pageable: $scope.lang == 'vi-vn' ? pageableShort_vi : pageableShort_en,
            pageable: false,
            save: function (e) {
                savechitietdonhang(e);
            },
            update: function (e) {
                e.success();
            },
            columns: listColumnsgrid()
        };

        $timeout(function () {
            $scope.gridData = {
                data: [],
                schema: {
                    model: {
                        id: "idhanghoa",
                        fields: {
                            xoa: {
                                type: "string",
                                editable: false,
                            },
                            tacvu: {
                                type: "string",
                                editable: false,
                            },
                            mahang: {
                                type: "string",
                                editable: false,
                            },
                            tenhang: {
                                type: "string",
                                editable: false,
                            },
                            idhanghoa: {
                                type: "number",
                                editable: false,
                            },
                            soluong: {
                                type: "number",
                                validation: {
                                    min: 0
                                }
                            },
                            giaban: {
                                type: "number",
                                validation: {
                                    min: 0
                                }
                            },
                            hinhthucban_name: {
                                type: "string",
                            },
                            idctkm: {
                                type: "number",
                                editable: false,
                            },
                            tenCTKM: {
                                type: "string",
                            },
                            idhaohut: {
                                type: "number",
                                editable: false,
                            },
                            tenLoaiHaoHut: {
                                type: "string",
                            },
                            chietkhautien: {
                                type: "number",
                                editable: false,
                            },
                            chietkhauphantram: {
                                type: "number",
                                editable: false,
                            },
                            tongtien: {
                                type: "number",
                                editable: false,
                            },
                            soton: {
                                type: "number",
                                editable: false,
                            },
                            tongtienchietkhau: {
                                type: "number",
                                editable: false,
                            },
                            phantramhaohut: {
                                type: "number",
                                editable: false,
                            },
                            soluonghaohut: {
                                type: "number",
                                editable: false,
                            },

                        }
                    }
                },
                pageSize: 9999,
                aggregate: [
                    { field: "tongtien", aggregate: "sum" },
                    { field: "tongtienchietkhau", aggregate: "sum" },
                    { field: "chietkhautien", aggregate: "sum" }
                ]
            };
        }, 500);

    }
    function reloadloadgrid(_data) {
        $timeout(function () {
            $scope.gridData = {
                data: _data,
                schema: {
                    model: {
                        fields: {
                            xoa: {
                                type: "string",
                                editable: false,
                            },
                            tacvu: {
                                type: "string",
                                editable: false,
                            },
                            mahang: {
                                type: "string",
                                editable: false,
                            },
                            tenhang: {
                                type: "string",
                                editable: false,
                            },
                            idhanghoa: {
                                type: "number",
                                editable: false,
                            },
                            soluong: {
                                type: "number",
                                validation: {
                                    min: 0
                                }
                            },
                            giaban: {
                                type: "number",
                                validation: {
                                    min: 0
                                }
                            },
                            hinhthucban_name: {
                                type: "string",
                            },
                            idctkm: {
                                type: "number",
                                editable: false,
                            },
                            tenCTKM: {
                                type: "string",
                            },
                            idhaohut: {
                                type: "number",
                                editable: false,
                            },
                            tenLoaiHaoHut: {
                                type: "string",
                            },
                            chietkhautien: {
                                type: "number",
                                editable: false,
                            },
                            chietkhauphantram: {
                                type: "number",
                                editable: false,
                            },
                            tongtien: {
                                type: "number",
                                editable: false,
                            },
                            soton: {
                                type: "number",
                                editable: false,
                            },
                            tongtienchietkhau: {
                                type: "number",
                                editable: false,
                            },
                            phantramhaohut: {
                                type: "number",
                                editable: false,
                            },
                            soluonghaohut: {
                                type: "number",
                                editable: false,
                            },

                        }
                    }
                },
                //pageSize: 20,
                aggregate: [
                    { field: "tongtien", aggregate: "sum" },
                    { field: "tongtienchietkhau", aggregate: "sum" },
                    { field: "chietkhautien", aggregate: "sum" }
                ]
            };
        }, 100);

    }

    function getlistkhuyenmaiingrid(options) {
        let model = options.model;
        let list = [];
        __arrkhuyenmaiingrid.forEach(function (obj) {
            if (obj.chiTietCTKM != null) {
                angular.forEach(obj.chiTietCTKM, function (data) {
                    if (model.hinhthucban != 2 && model.idhanghoa == data.iD_Hang) {
                        list.push(obj);
                    }
                });
            }
        })

        return list;
    }

    function numberEditor(container, options) {
        $('<input  name="' + options.field + '"/>')
            .appendTo(container)
            .kendoNumericTextBox({
                min: 0,
                format: __format,
                decimals: __digits
            })
    }
    function hinhThucBanDropDownEditor(container, options) {
        $('<input name="hinhthucban_combo"/>')
            .appendTo(container)
            .kendoDropDownList({
                autoBind: false,
                dataTextField: "name",
                dataValueField: "id",
                suggest: true,
                dataSource: {
                    transport: {
                        read: function (operation) {
                            operation.success(__arrhinhthucban);
                        }
                    },
                },
            });
    }
    function khuyenmaiDropDownEditor(container, options) {
        $('<input name="khuyenmai_combo"/>')
            .appendTo(container)
            .kendoDropDownList({
                autoBind: false,
                dataTextField: "tenCTKM",
                dataValueField: "iD_CTKM",
                suggest: true,
                dataSource: {
                    transport: {
                        read: function (operation) {
                            let _data = getlistkhuyenmaiingrid(options) || [];
                            operation.success(_data);
                        }
                    },
                },
            });
    }
    function haohutDropDownEditor(container, options) {
        $('<input name="haohut_combo"/>')
            .appendTo(container)
            .kendoDropDownList({
                autoBind: false,
                dataTextField: "tenLoaiHaoHut",
                dataValueField: "id",
                suggest: true,
                dataSource: {
                    transport: {
                        read: function (operation) {
                            operation.success(__arrhaohutingrid);
                        }
                    },
                },
            });
    }

    function savechitietdonhang(e) {
        let grid = e.sender;
        let model = e.model;

        let isChange = true;

        let _soLuong = model.soluong;
        let _tongTien = 0;
        let _giaBan = model.giaban;
        let _hinhThucBan = model.hinhthucban;
        let _khuyenMai = model.khuyenmai_combo;
        let _listChiTietKhuyenMai = null;
        if (_khuyenMai != null)
            _listChiTietKhuyenMai = model.khuyenmai_combo.chiTietCTKM;

        //Thay đổi số lượng
        if (e.values.soluong != null) {
            _soLuong = e.values.soluong;
            model.dschitietmathang = [];
            if (e.model.mahang === "TCNLNN") {
                let num = parseInt(e.values.soluong / 2);
                let num2 = parseInt(e.values.soluong) % 2;
                let myGrid = $('#gridHangChon').data("kendoGrid");
                let myGridChiTiet = $("#grid").data("kendoGrid");
                if (num > 0) {
                    let dataItem = myGrid.dataSource.get(191);
                    let ddataItem = myGridChiTiet.dataSource.get(191);
                    try {
                        if (ddataItem == undefined) {
                            let giaban = (__idhinhthucban == 0) ? dataItem.giaLe : (__idhinhthucban == 1) ? dataItem.giaBuon : 0;
                            let item = {
                                mahang: dataItem.maHang,
                                tenhang: dataItem.tenHang,
                                soluong: num,
                                soton: dataItem.soLuongTon,
                                idhanghoa: dataItem.idMatHang,
                                isDichVu: dataItem.isDichVu,
                                chietkhautien: 0,
                                chietkhauphantram: 0,
                                tongtien: giaban * (parseInt(e.values.soluong) - 1) / 2,
                                giaban: giaban,
                                tongtienchietkhau: 0,
                                hinhthucban: __idhinhthucban,
                                hinhthucban_name: __namehinhthucban,
                                hinhthucban_combo: __combohinhthucban,
                                phantramhaohut: 0,
                                soluonghaohut: 0,
                                giale: dataItem.giaLe,
                                giabuon: dataItem.giaBuon,
                                khuyenmai_combo: __arrkhuyenmaiingrid[0],
                                idctkm: __arrkhuyenmaiingrid[0].iD_CTKM,
                                tenCTKM: __arrkhuyenmaiingrid[0].tenCTKM,
                                ngay: e.model.ngay,
                                dschitietmathang: [],
                                lstDichVu: dataItem.lstDichVu
                            }
                            myGridChiTiet.dataSource.add(item);
                            __arridhangchitiet.push(dataItem.idMatHang);
                            myGrid.dataSource.remove(dataItem);
                        } else {
                            ddataItem.soluong = num;
                            ddataItem.tongtien = ddataItem.giaban * ddataItem.soluong;
                        }

                    } catch (ex) { }
                }

                if (num2 == 1) {

                    let ddataItem2 = myGridChiTiet.dataSource.get(192);
                    let dataItem2 = myGrid.dataSource.get(192);


                    try {
                        if (ddataItem2 == undefined) {
                            let giaban2 = (__idhinhthucban == 0) ? dataItem2.giaLe : (__idhinhthucban == 1) ? dataItem2.giaBuon : 0;
                            let item2 = {
                                mahang: dataItem2.maHang,
                                tenhang: dataItem2.tenHang,
                                soluong: 1,
                                soton: dataItem2.soLuongTon,
                                idhanghoa: dataItem2.idMatHang,
                                isDichVu: dataItem2.isDichVu,
                                chietkhautien: 0,
                                chietkhauphantram: 0,
                                tongtien: giaban2,
                                giaban: giaban2,
                                tongtienchietkhau: 0,
                                hinhthucban: __idhinhthucban,
                                hinhthucban_name: __namehinhthucban,
                                hinhthucban_combo: __combohinhthucban,
                                phantramhaohut: 0,
                                soluonghaohut: 0,
                                giale: dataItem2.giaLe,
                                giabuon: dataItem2.giaBuon,
                                khuyenmai_combo: __arrkhuyenmaiingrid[0],
                                idctkm: __arrkhuyenmaiingrid[0].iD_CTKM,
                                tenCTKM: __arrkhuyenmaiingrid[0].tenCTKM,
                                ngay: e.model.ngay,
                                dschitietmathang: [],
                                lstDichVu: dataItem2.lstDichVu
                            }
                            myGridChiTiet.dataSource.add(item2);
                            __arridhangchitiet.push(dataItem2.idMatHang);
                            myGrid.dataSource.remove(dataItem2);

                        } else {
                            ddataItem2.soluong = 1;
                            ddataItem2.tongtien = ddataItem2.giaban * ddataItem2.soluong;
                        }
                    } catch (ex) { }


                } else {
                    let dataItem2 = myGridChiTiet.dataSource.get(192);
                    myGridChiTiet.dataSource.remove(dataItem2);

                    __arridhangchitiet = __arridhangchitiet.filter(item => item != dataItem2.idhanghoa);

                }
                loadgridhangchon();
                $timeout(function () {
                    updatethanhtien();
                }, 100);
            }
        }

        //Thay đổi giá bán
        if (e.values.giaban != null) {
            if (model.hinhthucban == 0) {
                e.preventDefault();
                model.giaban = model.giale;

                _giaBan = model.giaban;

                isChange = false;

                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_khongthethaydoigiabanle') }, 'warning');
            }

            if (model.hinhthucban == 1) {
                e.preventDefault();
                model.giaban = model.giabuon;

                _giaBan = model.giaban;


                isChange = false;

                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_khongthethaydoigiabanbuon') }, 'warning');
            }

            if (model.hinhthucban == 2) {
                model.giakhac = e.values.giaban;

                _giaBan = e.values.giaban;
            }
        }

        //Thay đổi chương trình khuyến mãi
        if (e.values.khuyenmai_combo != null) {
            _khuyenMai = e.values.khuyenmai_combo;
            _listChiTietKhuyenMai = _khuyenMai.chiTietCTKM;
            model.tenCTKM = _khuyenMai.tenCTKM;
            model.idctkm = _khuyenMai.iD_CTKM;

        }

        //Thay đổi hình thức bán
        if (e.values.hinhthucban_combo != null) {

            //let hinhThucBan = e.values.hinhthucban_combo;
            let dataSource = grid.dataSource.data();
            let idhinhthucban = e.values.hinhthucban_combo.id;
            let flagNew = true;

            let val = dataSource.filter(function (obj) {
                return (idhinhthucban == obj.hinhthucban && model.hinhthucban != obj.hinhthucban && model.idhanghoa == obj.idhanghoa && model.idhaohut == obj.idhaohut)
            })
            if (val.length > 0)
                flagNew = false;

            if (!flagNew) {
                e.preventDefault();

                isChange = false;

                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_hinhthucbantrung_khongthethaydoi') }, 'warning');
                grid.refresh();
            } else {
                model.hinhthucban = idhinhthucban;
                model.hinhthucban_name = e.values.hinhthucban_combo.name;
                if (model.hinhthucban == 0) {
                    model.giaban = model.giale;
                }
                if (model.hinhthucban == 1) {
                    model.giaban = model.giabuon;

                }
                if (model.hinhthucban == 2) {
                    model.giaban = 0;
                }
                _giaBan = model.giaban;
                _hinhThucBan = model.hinhthucban;
            }
        }

        //Thay đổi hao hụt
        if (e.values.haohut_combo != null) {
            let haohut = e.values.haohut_combo;
            let dataSource = grid.dataSource.data();
            let flagNew = true;

            let hh = dataSource.filter(obj => (haohut.id == obj.idhaohut && model.hinhthucban == obj.hinhthucban && model.idhanghoa == obj.idhanghoa && model.idhaohut != obj.idhaohut))
            if (hh.length > 0)
                flagNew = false;

            //angular.forEach(dataSource, function (obj) {
            //    if (haohut.id == obj.idhaohut && model.hinhthucban == obj.hinhthucban && model.idhanghoa == obj.idhanghoa && model.idhaohut != obj.idhaohut) {
            //        flagNew = false;
            //    }
            //});

            if (!flagNew) {
                e.preventDefault();

                isChange = false;

                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_khongthethaydoihaohut') }, 'warning');
                grid.refresh();
            } else {
                model.tenLoaiHaoHut = haohut.tenLoaiHaoHut;
                if (haohut.id == -1) {
                    model.idhaohut = haohut.id;
                    model.phantramhaohut = 0;
                    model.soluonghaohut = 0;
                } else {
                    model.idhaohut = haohut.id;
                    model.phantramhaohut = haohut.tiLe;
                    model.soluonghaohut = 0;
                }
            }
        }

        //tính toán lại số liệu
        if (isChange) {
            //lấy chi tiết khuyến mãi của mặt hàng
            let _chiTietKhuyenMai = null;
            if (_listChiTietKhuyenMai != null) {
                let ct = _listChiTietKhuyenMai.filter(x => (x.iD_Hang == model.idhanghoa))
                if (ct.length > 0)
                    _chiTietKhuyenMai = ct[0];
            }

            if (_khuyenMai == null || _chiTietKhuyenMai == null) {
                model.chietkhauphantram = 0;
                model.chietkhautien = 0;
                model.tongtienchietkhau = 0;
            }

            //Bán lẻ
            if (_hinhThucBan == 0) {
                if (_chiTietKhuyenMai != null) {
                    model.chietkhauphantram = _chiTietKhuyenMai.chietKhauPhanTram_BanLe;
                    model.chietkhautien = _chiTietKhuyenMai.chietKhauTien_BanLe;
                    model.ghichu = _chiTietKhuyenMai.ghiChuGia;
                } else {
                    model.chietkhauphantram = 0;
                    model.chietkhautien = 0;
                    model.ghichu = "";
                }
            }

            //bán buôn
            if (_hinhThucBan == 1) {
                if (_chiTietKhuyenMai != null) {
                    model.chietkhauphantram = _chiTietKhuyenMai.chietKhauPhanTram_BanBuon;
                    model.chietkhautien = _chiTietKhuyenMai.chietKhauTien_BanBuon;
                    model.ghichu = _chiTietKhuyenMai.ghiChuGia;
                } else {
                    model.chietkhauphantram = 0;
                    model.chietkhautien = 0;
                    model.ghichu = "";
                }
            }
            //bán giá khác
            if (_hinhThucBan == 2) {
                model.chietkhauphantram = 0;
                model.chietkhautien = 0;
                model.ghichu = "";
                model.tenCTKM = __arrkhuyenmaiingrid[0].tenCTKM;
                model.idctkm = __arrkhuyenmaiingrid[0].iD_CTKM;
                model.khuyenmai_combo = null;
            }

            _tongTien = _soLuong * _giaBan;

            if (_chiTietKhuyenMai != null) {
                //Nếu loại đáp ứng theo số lượng
                if (_khuyenMai.loai == 3 || _khuyenMai.loai == 4 || _khuyenMai.loai == 5) {
                    let from = _chiTietKhuyenMai.soLuongDatKM_Tu;
                    let to = (_chiTietKhuyenMai.soLuongDatKM_Den == 0) ? 999999999999 : _chiTietKhuyenMai.soLuongDatKM_Den;
                    if (!(_soLuong >= from && _soLuong <= to)) {
                        model.chietkhauphantram = 0;
                        model.chietkhautien = 0;
                    }
                }

                //Nếu loại đáp ứng theo tổng tiền
                if (_khuyenMai.loai == 6 || _khuyenMai.loai == 7 || _khuyenMai.loai == 8) {
                    let from = _chiTietKhuyenMai.tongTienDatKM_Tu;
                    let to = (_chiTietKhuyenMai.tongTienDatKM_Den == 0) ? 9999999999999999 : _chiTietKhuyenMai.tongTienDatKM_Den;
                    if (!(_tongTien >= from && _tongTien <= to)) {
                        model.chietkhauphantram = 0;
                        model.chietkhautien = 0;
                    }
                }
            }

            //tính tổng tiền chiết khấu và tổng tiền
            model.tongtien = _tongTien;
            model.tongtienchietkhau = tinhchietkhaugrid(model.tongtien * 1, model.chietkhauphantram * 1, model.chietkhautien * _soLuong);
        }

        try {
            grid.refresh();
        } catch (ex) { }

        $timeout(function () {
            updatethanhtien();
        }, 100);

    }

    function tinhchietkhaugrid(tongtien, chietkhauphantram, chietkhautien) {
        let sumMoneyDiscount = (parseFloat(tongtien) * (parseFloat(chietkhauphantram) / 100) + parseFloat(chietkhautien));
        return sumMoneyDiscount;
    }
    function updatethanhtien() {
        let grid = $("#grid").data("kendoGrid");
        let tongtiendonhang = grid.dataSource.aggregates().tongtien.sum;
        let tongtienchietkhaumathang = grid.dataSource.aggregates().tongtienchietkhau.sum;

        let phantramchietkhaudonhang = $scope.chietkhauphantram * 1 + $scope.chieukhauphantramkhac * 1;
        let tienchietkhaudonhang = $scope.chietkhautien * 1 + $scope.chieukhautienkhac * 1;

        $scope.tienhang = tongtiendonhang;
        $scope.tienchietkhau = tongtienchietkhaumathang + tienchietkhaudonhang + (tongtiendonhang - tongtienchietkhaumathang) * phantramchietkhaudonhang / 100;
        $scope.tienthanhtoan = tongtiendonhang - $scope.tienchietkhau;

    }

    function listColumnsgridchitiet() {
        let dataList = [];

        dataList.push({
            field: "xoa", title: $.i18n('button_xoa'),
            template: '<button ng-click="deleterowgridchitiet()" class="btn btn-link btn-menubar" title ="' + $.i18n('button_xoa') + '" ><i class="fas fa-trash fas-sm color-danger"></i></button> ',
            attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "60px"
        });

        dataList.push({
            field: "chieudai", title: $.i18n('header_kichthuoc'),
            editor: numberEditor,
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            attributes: { style: "text-align: right" }, footerAttributes: { style: "text-align: right" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        });

        dataList.push({
            field: "soluong", title: $.i18n('header_soluong'),
            editor: numberEditor,
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            attributes: { style: "text-align: right" }, footerAttributes: { style: "text-align: right" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        });

        dataList.push({
            field: "tong", title: $.i18n('header_tongsoluong'),
            editor: numberEditor,
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            attributes: { style: "text-align: right" }, footerAttributes: { style: "text-align: right" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        });

        dataList.push({
            field: "mota", title: $.i18n('header_ghichu'),
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
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
            editable: true,
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
                        ID: 'iD_HangHoa',
                        fields: {
                            xoa: {
                                type: "string",
                                editable: false,
                            },
                            iD_MatHang: {
                                type: "number",
                                validation: {
                                    min: 0
                                }
                            },
                            chieudai: {
                                type: "number",
                                validation: {
                                    min: 0
                                }
                            },
                            chieurong: {
                                type: "number",
                                validation: {
                                    min: 0
                                }
                            },
                            chieucao: {
                                type: "number",
                                validation: {
                                    min: 0
                                }
                            },
                            soluong: {
                                type: "number",
                                validation: {
                                    min: 0
                                }
                            },
                            giaban: {
                                type: "number",
                                validation: {
                                    min: 0
                                }
                            },
                            tongtien: {
                                type: "number",
                                editable: false,
                                validation: {
                                    min: 0
                                }
                            },
                            tong: {
                                type: "number",
                                editable: false,
                                validation: {
                                    min: 0
                                }
                            },
                            mota: {
                                type: "string",
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

    function savechitietdonhangchitiet(e) {
        let grid = e.sender;
        let model = e.model;

        let ischange = false;

        let _kichthuoc = model.chieudai;
        let _soluong = model.soluong;

        if (e.values.soluong != null) {
            _soluong = e.values.soluong;
            ischange = true;
        }

        if (e.values.chieudai != null) {
            _kichthuoc = e.values.chieudai;
            ischange = true;
        }

        if (ischange) {
            model.tong = _soluong * _kichthuoc;
        }

        grid.refresh();
    }

    function validatethem() {
        let message = ""
        let flag = true;

        let _chitietdonhang = $("#grid").data("kendoGrid").dataSource.data();

        if (flag && __idkhachhang <= 0) {
            message = $.i18n('label_chuachonkhachhang');
            flag = false;
        }
        if (flag && __idnhanvien <= 0) {
            message = $.i18n('label_chuachonnhanvienlap');
            flag = false;
        }
        if (flag && (_chitietdonhang == null || _chitietdonhang.length == 0)) {
            message = $.i18n('label_chuachonmathang');
            flag = false;
        }
        if (flag) {
            let arval = _chitietdonhang.filter(function (x) {
                return ((x.isdichvu == 0) && (x.soluong <= 0 || x.soluong > x.soton))
            })
            if (arval.length > 0) {
                message = $.i18n('label_soluongcuamathangkhongthelonhonsoton');
                flag = false;
            }
        }
        if (!flag)
            Notification({ title: $.i18n('label_thongbao'), message: message }, 'warning');

        if (flag)
            flag = validatekhuyenmaidonhang();
        if (flag)
            flag = validatekhuyenmaimathang(_chitietdonhang);

        return flag;
    }
    function validatekhuyenmaidonhang() {
        let message = "";
        let flag = true;

        let arr = __arrkhuyenmaidonhang.filter(x => (x.iD_CTKM == __idkhuyenmai))
        if (arr.length > 0) {
            let item = arr[0];
            let ngayApDung = new Date(item.ngayApDung)
            let ngayKetThuc = new Date(item.ngayKetThuc)
            if (item.tongTienDatKM_Den == 0)
                item.tongTienDatKM_Den = 9999999999999999;

            if (flag && item.trangThai == 0) {
                message = $.i18n('label_chuongtrinhkhuyenmaidonhangdahethieuluc');
                flag = false;
            }

            if (flag && ($scope.obj_TuNgay < ngayApDung || $scope.obj_TuNgay > ngayKetThuc)) {
                message = $.i18n('label_chuongtrinhkhuyenmaidonhangdahetthoigianapdung');
                flag = false;
            }

            if (flag && (item.loai == 9 || item.loai == 10 || item.loai == 11)) {
                if ($scope.tienhang < item.tongTienDatKM_Tu || $scope.tienhang > item.tongTienDatKM_Den) {
                    message = $.i18n('label_tongtiendonhangkhongdapungdieukiencuachuongtrinhkhuyenmai');
                    flag = false;
                }
            }

        }

        if (!flag)
            Notification({ title: $.i18n('label_thongbao'), message: message }, 'warning');

        return flag;
    }
    function validatekhuyenmaimathang(_chitietdonhang) {
        let message = "";
        let flag = true;

        let arr = _chitietdonhang.filter(x => (x.idctkm > 0))
        if (arr.length > 0) {
            arr.forEach(function (item) {
                let arkm = __arrkhuyenmaiingrid.filter(x => (x.iD_CTKM == item.idctkm))
                if (arkm.length > 0) {
                    let ctkm = arkm[0];
                    console.log(ctkm)
                    let chiTietCTKM = ctkm.chiTietCTKM;

                    let ngayApDung = new Date(ctkm.ngayApDung)
                    let ngayKetThuc = new Date(ctkm.ngayKetThuc)

                    if (flag && ctkm.trangThai == 0) {
                        message = $.i18n('label_chuongtrinhkhuyenmaidonhangdahethieuluc');
                        flag = false;
                    }

                    if (flag && ($scope.obj_TuNgay < ngayApDung || $scope.obj_TuNgay > ngayKetThuc)) {
                        message = $.i18n('label_chuongtrinhkhuyenmaidonhangdahetthoigianapdung');
                        flag = false;
                    }

                    if (chiTietCTKM != null) {
                        let rows = chiTietCTKM.filter(x => (x.iD_Hang == item.idhanghoa))
                        if (rows.length > 0) {
                            let row = rows[0];
                            if (row.tongTienDatKM_Den == 0)
                                row.tongTienDatKM_Den = 9999999999999999;
                            if (row.soLuongDatKM_Den == 0)
                                row.soLuongDatKM_Den = 9999999999999999;

                            if (flag && (ctkm.loai == 3 || ctkm.loai == 4 || ctkm.loai == 5)) {
                                if (item.soluong < row.soLuongDatKM_Tu || item.soluong > row.soLuongDatKM_Den) {
                                    message = $.i18n('label_soluongcuamathang') + item.mahang + ' - ' + item.tenhang + $.i18n('label_khongdapungdieukiencuachuongtrinhkhuyenmai');
                                    flag = false;
                                }
                            }

                            if (flag && (ctkm.loai == 6 || ctkm.loai == 7 || ctkm.loai == 8)) {
                                if (item.tongtien < row.tongTienDatKM_Tu || item.tongtien > row.tongTienDatKM_Den) {
                                    message = $.i18n('label_tongthanhtiencuamathang') + item.mahang + ' - ' + item.tenhang + $.i18n('label_khongdapungdieukiencuachuongtrinhkhuyenmai');
                                    flag = false;
                                }
                            }
                        }
                    }
                }
            })
        }

        if (!flag)
            Notification({ title: $.i18n('label_thongbao'), message: message }, 'warning');

        return flag;
    }

    function validatethemsuakhachhang() {
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

        //if (flag && ($scope.khachhangobj.diaChi == '' || $scope.khachhangobj.diaChi == undefined)) {
        //    flag = false;
        //    msg = $.i18n("label_diachikhongduocbotrong");
        //    $("#DiaChi").focus();
        //}

        if (!flag)
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n(msg) }, 'warning');

        return flag;
    }
    function khachhangOnChange(_maKH, _dienThoai) {
        //1.Kiểm tra sử dụng bảng giá loại khách hàng
        //2.load lại danh sách mặt hàng
        //3. clear chi tiết mặt hàng đơn hàng
        if (__sudungbanggia) {
            __arridhangchitiet = [];
            loadgridhangchon();
            loadgrid();
        }

        //4.Thực hiện thao tác cập nhật theo khách hàng mới
        setTimeout(function () {
            $scope.$apply(function () {
                $scope.makh = _maKH;
                $scope.dienthoai = _dienThoai;
            });
        }, 20);
    }
    //event
    $scope.openWindowThemKhachHang = function () {
        $scope.formaddcustomer.center().open();
        $("#MaKhachHang").focus();
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
            iD_LoaiKhachHang: $scope.loaikhachhangselect == null ? 0 : $scope.loaikhachhangselect.iD_LoaiKhachHang,
            iD_NhanVien: __idnhanvien,
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
            lastUpdate_ID_QuanLy: 0,
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
    }
    $scope.luuKhachHang = function () {
        if (validatethemsuakhachhang()) {
            let formData = {
                MaKH: $scope.khachhangobj.maKH,
                Ten: $scope.khachhangobj.ten,
                DiaChi: $scope.khachhangobj.diaChi,
                KinhDo: 0,
                ViDo: 0,
                DuongPho: '',
                ID_KhuVuc: 0,
                ID_Tinh: 0,
                ID_Quan: 0,
                ID_Phuong: 0,
                SoDienThoaiMacDinh: $scope.khachhangobj.soDienThoai,
                Fax: '',
                SoDienThoai1: $scope.khachhangobj.soDienThoai,
                SoDienThoai2: '',
                SoDienThoai3: '',
                ID_LoaiKhachHang: $scope.loaikhachhangselect == null ? 0 : $scope.loaikhachhangselect.iD_LoaiKhachHang,
                ID_NhomKH: 0,
                ID_Cha: 0,
                NguoiLienHe: '',
                Email: $scope.khachhangobj.email,
                Website: '',
                DiaChiXuatHoaDon: '',
                SoTKNganHang: '',
                MaSoThue: '',
                GhiChu: $scope.khachhangobj.ghiChu,
                ImgUrl: '',
                IDQLLH: $rootScope.UserInfo.iD_QLLH,
                ID_NhanVien: __idnhanvien,
                ID_QuanLy: $rootScope.UserInfo.iD_QuanLy,
                IDKhachHang: 0
            }
            commonOpenLoadingText("#btn_luukhachhang");
            donHangDataService.themkhachhangtudonhang(formData).then(function (result) {
                if (result.flag) {
                    __idkhachhang = result.data;
                    $scope.formaddcustomer.center().close();

                    let obj = {
                        iD_QLLH: $rootScope.UserInfo.iD_QLLH
                        , iD_KhachHang: result.data
                        , tenKhachHang: $scope.khachhangobj.ten
                        , kinhDo: 0
                        , viDo: 0
                        , iD_NhanVien: __idnhanvien
                        , tenDayDu: ""
                        , nhom: ""
                        , maSoThue: ""
                        , ngaySinh: "1900-01-01T00:00:00"
                        , diaChi: $scope.khachhangobj.diaChi
                        , dienThoai: $scope.khachhangobj.soDienThoai
                        , nguoiLienHe: ""
                        , soTKNganHang: ""
                        , email: ""
                        , fax: ""
                        , website: ""
                        , ghiChu: $scope.khachhangobj.ghiChu
                        , insertedtime: "1900-01-01T00:00:00"
                        , trangThai: 0
                        , maKH: $scope.khachhangobj.maKH
                        , diagioihanhchinhid: 0
                        , iD_KhuVuc: 0
                        , iD_Tinh: 0
                        , iD_Quan: 0
                        , iD_Phuong: 0
                        , trangThaiXoa: 0
                        , ngayXoa: "1900-01-01T00:00:00"
                        , iD_QuanLyXoa: 0
                        , duongPho: ''
                        , iD_QuanLy: 0
                        , dienThoai2: ""
                        , dienThoai3: ""
                        , dienThoaiMacDinh: $scope.khachhangobj.soDienThoai
                        , iD_LoaiKhachHang: $scope.loaikhachhangselect == null ? 0 : $scope.loaikhachhangselect.iD_LoaiKhachHang
                        , iD_NhomKH: 0
                        , iD_Cha: 0
                        , ghiChuKhiXoa: ""
                        , congNoChoPhep: 0
                        , yeuCauXoa_ID_NhanVien: 0
                        , yeuCauXoa_ThoiGian: "1900-01-01T00:00:00"
                        , lastUpdate_ID_NhanVien: 0
                        , lastUpdate_ID_QuanLy: 0
                        , lastUpdate_ThoiGian_NhanVien: "1900-01-01T00:00:00"
                        , lastUpdate_ThoiGian_QuanLy: "1900-01-01T00:00:00"
                        , iD_PhanLoaiChucNang: 0
                        , daChayCapNhatToaDo: 0
                        , diaChiXuatHoaDon: ""
                    };
                    let combobox = $("#khachhang").data("kendoComboBox");
                    combobox.dataSource.add(obj);

                    khachhangOnChange(obj.maKH, obj.dienThoai);
                    setTimeout(function () {
                        combobox.value(__idkhachhang);
                    }, 50);

                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                }
                else
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');

                commonCloseLoadingText("#btn_luukhachhang")
            });
        }
    }
    $scope.dongFormKhachHang = function () {
        $scope.formaddcustomer.center().close();
    }

    $scope.fomartnumber = function (data) {
        return kendo.toString(data, $rootScope.UserInfo.dinhDangSo);
    }
    $scope.fomartnumberpercent = function (data) {
        return kendo.toString(data, 'n2');
    }

    $scope.luudonhang = function () {
        if (validatethem()) {
            let tongchietkhautien = $scope.chietkhautien + $scope.chieukhautienkhac;
            let tongchietkhauphantram = $scope.chietkhauphantram + $scope.chieukhauphantramkhac;

            let _chitietdonhang = $("#grid").data("kendoGrid").dataSource.data();

            let data = {
                idnhanvien: __idnhanvien,
                idcuahang: __idkhachhang,
                tongtien: $scope.tienthanhtoan,
                ghichu: $scope.ghichu,
                mathamchieu: '',
                thoigiantao: kendo.toString($scope.obj_TuNgay, formatDateTimeFilter),
                idcheckin: 0,
                idctkm: __idkhuyenmai,
                tongtienchietkhau: $scope.tienchietkhau,
                chietkhautien: tongchietkhautien,
                chietkhauphantram: tongchietkhauphantram,
                chietKhauPhanTramKhac: $scope.chieukhauphantramkhac,
                chietKhauTienKhac: $scope.chieukhautienkhac,
                chietkhauphantram_theoctkm: $scope.chietkhauphantram,
                chietkhautien_theoctkm: $scope.chietkhautien,
                idnhanvientao: 0,
                kinhdo: 0,
                vido: 0,
                diachitao: "",
                diachixuathoadon: "",
                chitietdonhang: _chitietdonhang,
                tienHang: 0,
                trangthaigiaohang: 0,
                trangthaithanhtoan: 0,
                trangthaidonhang: 0,
                tenctkm: "",
                macuahang: $scope.makh,
                tenkhachhang: "",
                tiendathanhtoan: 0,
                phantramhaohut: 0,
                soluonghaohut: 0,
                sodienthoai: "",
                diachikhachhang: "",
                LS_AccountCode: __lsaccountcode,
                LS_BookingCode: __lsbookingcode
            }

            donHangDataService.themvathanhtoandonhang(data).then(function (result) {
                if (result.flag) {
                    $scope.inve(result.data.chitietdonhang);
                    //$state.go('danhsachdonhang');
                    $scope.huydonhang();
                    Notification({ title: $.i18n('label_thongbao'), message: result.message }, 'success');
                }
                else
                    Notification({ title: $.i18n('label_thongbao'), message: result.message }, 'warning');
            });
        }
    }

    $scope.huydonhang = function () {
        //$state.go('danhsachdonhang');
        __idkhachhang = 0;
        $scope.loaikhachhangselect = null;
        $scope.khuyenmaiselect = null;
        $scope.khachhangselect = null;
        $scope.tienthanhtoan = 0;
        $scope.tienhang = 0;
        $scope.ghichu = "";
        $scope.tienchietkhau = 0;
        $scope.chieukhauphantramkhac = 0;
        $scope.chietkhauphantram = 0;
        $scope.chietkhautien = 0;
        $scope.makh = "";
        $scope.dienthoai = "";
        __lsaccountcode = "";
        __lsbookingcode = "";
        __idkhuyenmai = 0;
        __arridhangchitiet = [];
        _chitietdonhang = [];
        reloadloadgrid([]);
        loadgridhangchon();
        $scope.objprint = null;
    }

    //$scope.nhanvienOnChange = function () {
    //    $scope.nhanvienselect = this.nhanvienselect;
    //    if ($scope.nhanvienselect != undefined)
    //        __idnhanvien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
    //    else
    //        __idnhanvien = 0;

    //    loadlistkhachhang();
    //}
    $scope.loaikhachhangOnChange = function () {
        $scope.loaikhachhangselect = this.loaikhachhangselect;
        console.log($scope.loaikhachhangselect);
        __idkhachhang = 0;
        $scope.khachhang.value("");
        khachhangOnChange("", "");
        let combokhachhang = $scope.khachhang;
        var khachhangDataSource = combokhachhang.dataSource;
        if ($scope.loaikhachhangselect != null) {
            khachhangDataSource.filter([]);
            var filter = {
                logic: "and",
                filters: [] // ready for filter from each of the dropdowns and search bar
            };
            var searchFilter = {
                logic: "or",
                filters: [
                    { field: "iD_LoaiKhachHang", operator: "equals", value: $scope.loaikhachhangselect.iD_LoaiKhachHang },
                ]
            };
            filter.filters.push(searchFilter);

            khachhangDataSource.query({
                filter: filter
            });
        } else {
            khachhangDataSource.filter([]);
        }
    }
    $scope.khuyenmaiOnChange = function () {
        debugger
        $scope.khuyenmaiselect = this.khuyenmaiselect;
        if ($scope.khuyenmaiselect != undefined)
            __idkhuyenmai = ($scope.khuyenmaiselect.iD_CTKM < 0) ? 0 : $scope.khuyenmaiselect.iD_CTKM;
        else
            __idkhuyenmai = 0;

        setTimeout(function () {
            $scope.$apply(function () {
                if ($scope.khuyenmaiselect != undefined) {
                    $scope.chietkhautien = $scope.khuyenmaiselect.chietKhauTien * 1;
                    $scope.chietkhauphantram = $scope.khuyenmaiselect.chietKhauPhanTram * 1;
                } else {
                    $scope.chietkhautien = 0;
                    $scope.chietkhauphantram = 0;
                }

                updatethanhtien();
            });
        }, 20);
    }

    $scope.khachhangOnChange = function () {
        $scope.khachhangselect = this.khachhangselect;

        let makh = '';
        let dienthoai = '';

        if ($scope.khachhangselect != undefined) {
            __idkhachhang = ($scope.khachhangselect.iD_KhachHang < 0) ? 0 : $scope.khachhangselect.iD_KhachHang;
            makh = $scope.khachhangselect.maKH;
            dienthoai = $scope.khachhangselect.dienThoai;
        }
        else
            __idkhachhang = 0;

        khachhangOnChange(makh, dienthoai);

        ////1.Kiểm tra sử dụng bảng giá loại khách hàng
        ////2.load lại danh sách mặt hàng
        ////3. clear chi tiết mặt hàng đơn hàng
        //if (__sudungbanggia) {
        //    __arridhangchitiet = [];
        //    loadgridhangchon();
        //    loadgrid();
        //}

        ////4.Thực hiện thao tác cập nhật theo khách hàng mới
        //setTimeout(function () {
        //    $scope.$apply(function () {
        //        if ($scope.khachhangselect != undefined) {
        //            $scope.makh = $scope.khachhangselect.maKH;
        //            $scope.dienthoai = $scope.khachhangselect.dienThoai;
        //        } else {
        //            $scope.makh = '';
        //            $scope.dienthoai = '';
        //        }
        //    });
        //}, 20);
    }
    $scope.hinhthucbanOnChange = function () {
        $scope.hinhthucbanselect = this.hinhthucbanselect;
        if ($scope.hinhthucbanselect != undefined) {
            __combohinhthucban = $scope.hinhthucbanselect;
            __idhinhthucban = ($scope.hinhthucbanselect.id < 0) ? -1 : $scope.hinhthucbanselect.id;
            __namehinhthucban = ($scope.hinhthucbanselect.name < 0) ? -1 : $scope.hinhthucbanselect.name;
        }
        else {
            __combohinhthucban = { id: -1, name: '' }
            __idhinhthucban = -1;
            __namehinhthucban = '';
        }
    }
    $scope.typeViewOnChange = function () {
        $scope.typeViewSelect = this.typeViewSelect;
        if ($scope.typeViewSelect != undefined) {
            loadgridhangchon();
        }
        else {
        }
    }
    $scope.chieukhauphantramkhacOnchange = function () {
        $timeout(function () {
            updatethanhtien();
        }, 100);
    }
    $scope.chieukhautienkhacOnchange = function () {
        $timeout(function () {
            updatethanhtien();
        }, 100);
    }

    $scope.addrow = function (e) {
        if (__idkhachhang == 0) {
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_chuachonkhachhang') }, 'warning');
        } else if (__idhinhthucban == -1) {
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_chuachonhinhthucban') }, 'warning');
        } else {
            let myGrid = $('#gridHangChon').data("kendoGrid");
            let myListView = $('#listviewHangChon').data("kendoListView");
            let row = $(this).closest("tr");
            let dataItem = row.prevObject[0].dataItem;
            let giaban = (__idhinhthucban == 0) ? dataItem.giaLe : (__idhinhthucban == 1) ? dataItem.giaBuon : 0;
            let item = {
                mahang: dataItem.maHang,
                tenhang: dataItem.tenHang,
                isdichvu: dataItem.isDichVu,
                sitecode: dataItem.siteCode,
                servicerateid: dataItem.serviceRateID,
                soluong: 1,
                soton: dataItem.soLuongTon,
                idhanghoa: dataItem.idMatHang,
                chietkhautien: 0,
                chietkhauphantram: 0,
                tongtien: giaban,
                giaban: giaban,
                tongtienchietkhau: 0,
                hinhthucban: __idhinhthucban,
                hinhthucban_name: __namehinhthucban,
                hinhthucban_combo: __combohinhthucban,
                phantramhaohut: 0,
                soluonghaohut: 0,
                giale: dataItem.giaLe,
                giabuon: dataItem.giaBuon,
                khuyenmai_combo: __arrkhuyenmaiingrid[0],
                idctkm: __arrkhuyenmaiingrid[0].iD_CTKM,
                tenCTKM: __arrkhuyenmaiingrid[0].tenCTKM,
                haohut_combo: __arrhaohutingrid[0],
                tenLoaiHaoHut: __arrhaohutingrid[0].tenLoaiHaoHut,
                idhaohut: __arrhaohutingrid[0].id,
                dschitietmathang: []
            }
            try {
                let myGridChiTiet = $("#grid").data("kendoGrid");
                myGridChiTiet.dataSource.add(item);
                __arridhangchitiet.push(dataItem.idMatHang);
                myGrid.dataSource.remove(dataItem);
                myListView.dataSource.remove(dataItem);
            } catch (ex) { }



            $timeout(function () {
                updatethanhtien();
            }, 100);
        }
    }
    $scope.deleterowgrid = function () {
        let myGrid = $('#grid').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;

        __arridhangchitiet = __arridhangchitiet.filter(item => item != dataItem.idhanghoa);

        try {
            myGrid.dataSource.remove(dataItem);
        } catch (ex) { }

        loadgridhangchon();

        $timeout(function () {
            updatethanhtien();
        }, 100);
    }

    $scope.openformchitiet = function () {
        let row = $(this).closest("tr");
        DATAITEM = row.prevObject[0].dataItem;
        let arrchitiet = JSON.parse(JSON.stringify(DATAITEM.dschitietmathang));

        __curidhanghoa = DATAITEM.idhanghoa;
        __curhinhthucban = DATAITEM.hinhthucban;
        __curidhaohut = DATAITEM.idhaohut;

        $scope.formdetail.center().open();

        loadgridchitiet(arrchitiet);
    }

    $scope.themdongchitiet = function () {
        let myGridAdd = $("#gridchitiet").data("kendoGrid");
        let data = myGridAdd.dataSource.data();

        let obj = {
            iD_MatHang: __curidhanghoa,
            chieudai: 1,
            chieurong: 0,
            chieucao: 0,
            soluong: 1,
            giaban: 0,
            tongtien: 0,
            tong: 1,
            mota: ""
        }

        myGridAdd.dataSource.add(obj);
    }
    $scope.deleterowgridchitiet = function () {
        let myGrid = $('#gridchitiet').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;
        try {
            myGrid.dataSource.remove(dataItem);
        } catch (ex) { }
    }

    $scope.luuchitiet = function () {
        let chitietdh = $("#grid").data("kendoGrid").dataSource.data();

        let items = chitietdh.filter(ct => (ct.idhanghoa == __curidhanghoa && ct.hinhthucban == __curhinhthucban && ct.idhaohut == __curidhaohut));
        if (items.length > 0) {
            ct = items[0];

            ct.dschitietmathang = $("#gridchitiet").data("kendoGrid").dataSource.data();
            ct.soluong = $("#gridchitiet").data("kendoGrid").dataSource.aggregates().tong.sum;

            let _soLuong = ct.soluong;
            let _tongTien = 0;
            let _giaBan = ct.giaban;
            let _hinhThucBan = ct.hinhthucban;
            let _khuyenMai = ct.khuyenmai_combo;
            let _listChiTietKhuyenMai = ct.khuyenmai_combo.chiTietCTKM;

            //lấy chi tiết khuyến mãi của mặt hàng
            let _chiTietKhuyenMai = null;
            if (_listChiTietKhuyenMai != null) {
                let km = _listChiTietKhuyenMai.filter(x => (x.iD_Hang == ct.idhanghoa))
                if (km.length > 0)
                    _chiTietKhuyenMai = km[0];
            }

            if (_khuyenMai == null || _chiTietKhuyenMai == null) {
                ct.chietkhauphantram = 0;
                ct.chietkhautien = 0;
                ct.tongtienchietkhau = 0;
            }

            //Bán lẻ
            if (_hinhThucBan == 0) {
                if (_chiTietKhuyenMai != null) {
                    ct.chietkhauphantram = _chiTietKhuyenMai.chietKhauPhanTram_BanLe;
                    ct.chietkhautien = _chiTietKhuyenMai.chietKhauTien_BanLe;
                    ct.ghichu = _chiTietKhuyenMai.ghiChuGia;
                } else {
                    ct.chietkhauphantram = 0;
                    ct.chietkhautien = 0;
                    ct.ghichu = "";
                }
            }

            //bán buôn
            if (_hinhThucBan == 1) {
                if (_chiTietKhuyenMai != null) {
                    ct.chietkhauphantram = _chiTietKhuyenMai.chietKhauPhanTram_BanBuon;
                    ct.chietkhautien = _chiTietKhuyenMai.chietKhauTien_BanBuon;
                    ct.ghichu = _chiTietKhuyenMai.ghiChuGia;
                } else {
                    ct.chietkhauphantram = 0;
                    ct.chietkhautien = 0;
                    ct.ghichu = "";
                }
            }
            //bán giá khác
            if (_hinhThucBan == 2) {
                ct.chietkhauphantram = 0;
                ct.chietkhautien = 0;
                ct.ghichu = "";
            }

            _tongTien = _soLuong * _giaBan;

            if (_chiTietKhuyenMai != null) {
                //Nếu loại đáp ứng theo số lượng
                if (_khuyenMai.loai == 3 || _khuyenMai.loai == 4 || _khuyenMai.loai == 5) {
                    let from = _chiTietKhuyenMai.soLuongDatKM_Tu;
                    let to = (_chiTietKhuyenMai.soLuongDatKM_Den == 0) ? 999999999999 : _chiTietKhuyenMai.soLuongDatKM_Den;
                    if (!(_soLuong >= from && _soLuong <= to)) {
                        ct.chietkhauphantram = 0;
                        ct.chietkhautien = 0;
                    }
                }

                //Nếu loại đáp ứng theo tổng tiền
                if (_khuyenMai.loai == 6 || _khuyenMai.loai == 7 || _khuyenMai.loai == 8) {
                    let from = _chiTietKhuyenMai.tongTienDatKM_Tu;
                    let to = (_chiTietKhuyenMai.tongTienDatKM_Den == 0) ? 9999999999999999 : _chiTietKhuyenMai.tongTienDatKM_Den;
                    if (!(_tongTien >= from && _tongTien <= to)) {
                        ct.chietkhauphantram = 0;
                        ct.chietkhautien = 0;
                    }
                }
            }

            //tính tổng tiền chiết khấu và tổng tiền
            ct.tongtien = _tongTien;
            ct.tongtienchietkhau = tinhchietkhaugrid(ct.tongtien * 1, ct.chietkhauphantram * 1, ct.chietkhautien * _soLuong);

            DATAITEM.set('dschitietmathang', ct.dschitietmathang)
            DATAITEM.set('soluong', ct.soluong)
            DATAITEM.set('chietkhauphantram', ct.chietkhauphantram)
            DATAITEM.set('chietkhautien', ct.chietkhautien)
            DATAITEM.set('tongtienchietkhau', ct.tongtienchietkhau)
            DATAITEM.set('ghichu', ct.ghichu)
            DATAITEM.set('tongtien', ct.tongtien)

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

    $scope.applyClientFilters = function (e) {
        let listView = $scope.listviewHangChon;
        var listViewDataSource = listView.dataSource;
        // clear existing datasource
        listViewDataSource.filter([]);
        // extract selected items from each of the dropdown controls and the search box
        var searchString = $(".search-filter").val();
        // setup filter object (using and logic)
        var filter = {
            logic: "and",
            filters: [] // ready for filter from each of the dropdowns and search bar
        };
        // push new filter into array of filters with or logic on each filter




        if (searchString.length > 0) {
            var searchFilter = {
                logic: "or",
                filters: [
                    { field: "tenHang", operator: "contains", value: searchString },
                    { field: "maHang", operator: "contains", value: searchString }
                ]
            };
            filter.filters.push(searchFilter);
        }

        // apply filter query to datasource
        listViewDataSource.query({
            filter: filter
        });
    }

    $scope.clearFilter = function () {
        $(".search-filter").val("");
        $scope.applyClientFilters();
    }

    $scope.doSomething = function () {
        $("#listviewHangChon").children().trigger("click");
    }

    $scope.inhoadon = function (iddonhang) {
        //$scope.objprint
        //$scope.list
        $scope.list = $("#grid").data("kendoGrid").dataSource.data();
        donHangDataService.getById(iddonhang).then(function (result) {
            let data = result.data.donHang;
            let ngaytao = new Date(data.ngayTao);
            data.ngayTao = kendo.toString(ngaytao, formatDateTime);
            let lastupdate = new Date(data.lastUpdate_ThoiGian_NhanVien);
            if (lastupdate != null && lastupdate.getFullYear() > 1900)
                data.lastUpdate_ThoiGian_NhanVien = kendo.toString(lastupdate, formatDateTime);
            else
                data.lastUpdate_ThoiGian_NhanVien = '';
            $scope.objprint = data;
            $scope.objprint.tencongty = $rootScope.UserInfo.tencongty;
            //console.log($scope.objprint);
            //if ($rootScope.lang == 'vi-vn')
            console.log($scope.objprint);
            printer.printFromScope("/app/components/donhang/inDonHangPOSView.html", $scope);
            //$state.go('danhsachdonhang');
            //else
            //printer.printFromScope("/app/components/donhang/inDonHangENView.html", $scope);
        })

    }

    $scope.inve = function (chitietdonhang) {
        $.each(chitietdonhang, function (index, item) {
            window.open("https://server.lscloud.vn/Booking/PrintQRTicketByBookingCode?ID_ChiTietDonHang=" + item.idchitietdonhang
                + "&ID_DonHang=" + item.iddonhang
                + "&ID_MatHang=" + item.lstDichVu[0].iD_HangHoa
                + "&ID_DichVu=" + item.lstDichVu[0].iD_DichVu
                +"&BookingCode="
            )

        })

    }

    $scope.printLabel = function () {
        $scope.objprint.ngayTao = kendo.toString(new Date(), formatDateTime);
        $scope.listtakeaway = [];
        let grid = $("#grid").data("kendoGrid");
        $.each(grid.select(), function (index, item) {
            $scope.listtakeaway.push(grid.dataItem(item));
        })
        printer.printFromScope("/app/components/donhang/inDonHangTakeAwayView.html", $scope);
    }

    $scope.timkiemkhachhang = function () {
        console.log($scope.makh);
        donHangDataService.timkiemkhachhang_accountcode($scope.makh).then(function (result) {
            if (result.flag) {
                console.log(result.data);
                if (result.data.account == null) {
                    Notification({ title: $.i18n('label_thongbao'), message: "Không tìm thấy khách hàng phù hợp với mã" }, 'warning');
                } else {
                    let note = "";
                    $.each(result.data.booking.customerRes, function (index, item) {
                        if (item.customerType == "GUIDE" || item.customerType == "TA_CUSTOMER")
                            note += item.name + "-" + item.phoneNumber + ";";

                    })
                    __idkhachhang = 5;
                    $scope.loaikhachhang.value(3);
                    $scope.khachhang.value(5);
                    $scope.ghichu = note;
                    __lsbookingcode = result.data.booking.bookingCode;
                    __lsaccountcode = result.data.account.accountCode;
                }
            }
            else
                Notification({ title: $.i18n('label_thongbao'), message: "Không tìm thấy khách hàng phù hợp với mã" }, 'warning');
        });
    }

    init();

})