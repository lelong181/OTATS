angular.module('app').controller('khachHangController', function ($rootScope, $scope, $location, $state, Notification, ComboboxDataService, khachHangDataService) {
    CreateSiteMap();

    function init() {
        getquyen();

        loadgrid();
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

    function listColumnsgrid() {
        var dataList = [];

        dataList.push({ headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, selectable: true, width: "40px" });
        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({
            field: "ten",
            title: $.i18n("header_tenkhachhang"),
            template: function (dataItem) {
                return "<a href='' class='color-primary' ng-click='openDetailFromGrid(" + kendo.htmlEncode(dataItem.iD_KhachHang) + ")'>" + kendo.htmlEncode(dataItem.ten) + "</a>";
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px"
        });
        dataList.push({ field: "maKH", title: $.i18n("header_makhachhang"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px" });
        dataList.push({ field: "soDienThoai", title: $.i18n("header_sodienthoai"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px" });
        dataList.push({ field: "email", title: $.i18n("header_email"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px" });
        dataList.push({ field: "tenNhanVien", title: $.i18n("header_nhanvientao"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({ field: "tenLoaiKhachHang", title: $.i18n("header_loaikhachhang"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({ field: "tenNhomKH", title: $.i18n("header_kenhbanhang"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });

        dataList.push({
            title: $.i18n("header_add"),
            template: function (dataItem) {
                if (dataItem.anhDaiDien == null || dataItem.anhDaiDien == '')
                    return ''
                else {
                    let src = SERVERIMAGE + dataItem.anhDaiDien;
                    return '<img src="' + src + '" alt="" class="img-avatar rounded-circle">';
                }
            },
            attributes: { class: "text-center" },
            title: $.i18n("header_anhdaidien"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px"
        });
        dataList.push({ field: "diaChi", title: $.i18n("header_diachi"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "440px" });
        dataList.push({
            field: "toaDo1", title: $.i18n("header_toado"),
            template: function (dataItem) {
                if (dataItem.toaDo == '' || dataItem.toaDo == '0.0000000000000, 0.0000000000000')
                    return '<button ng-click="xemvitrichuacotoado()" class="btn btn-link btn-menubar" title ="' + $scope.bindTextI18n("label_vitrikhachhang") + '" ><i class="fas fa-map-marker-alt fas-sm color-gray"></i></button>';
                else
                    return '<button ng-click="xemvitri(' + kendo.htmlEncode(dataItem.viDo) + ',' + kendo.htmlEncode(dataItem.kinhDo) + ')" class="btn btn-link btn-menubar" title ="' + $.i18n('label_vitrikhachhang') + '" ><i class="fas fa-map-marker-alt fas-sm color-infor"></i></button>';
            },
            attributes: { class: "text-center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: defaultFilterableGrid, width: "80px"
        });
        dataList.push({ field: "tenTinh", title: $.i18n("label_tinhthanh"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px" });
        dataList.push({ field: "tenQuan", title: $.i18n("label_quanhuyen"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({ field: "tenPhuong", title: $.i18n("header_phuongxa"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
      
        dataList.push({ field: "tenLoaiKhachHang", title: $.i18n("header_loaikhachhang"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({ field: "tenNhomKH", title: $.i18n("header_kenhbanhang"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({ field: "maSoThue", title: $.i18n("header_masothue"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({ field: "nguoiLienHe", title: $.i18n("header_nguoilienhe"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({ field: "ghiChu", title: $.i18n("header_ghichu"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px" });
        dataList.push({
            field: "ngayTao", title: $.i18n("header_ngaytao"), template: function (dataItem) { return kendo.htmlEncode(kendo.toString(dataItem.ngayTao, formatDate)); }, attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        });

        return dataList;
    }
    function loadgrid() {
        kendo.ui.progress($("#grid"), true);
        $scope.gridOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".toolbarmenu").height());
                return heightGrid - 40;
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
        khachHangDataService.getlist().then(function (result) {
            $scope.gridData = {
                data: result.data,
                schema: {
                    model: {
                        id: 'iD_KhachHang',
                        fields: {
                            iD_KhachHang: {
                                type: "number"
                            },
                            maKH: {
                                type: "string"
                            },
                            ngayTao: {
                                type: "date"
                            }
                        }
                    }
                },
                pageSize: 20
            };
            kendo.ui.progress($("#grid"), false);

            showtooltip();

            if (!result.flag) {
                Notification({ title: $.i18n("label_thongbao"), message: $.i18n("label_coloixayra_khongtheloaddanhsachkhachhang") }, 'warning');
            }
        });
    }
    function showtooltip() {
        $("#grid").kendoTooltip({
            filter: "td:nth-child(5)",
            position: "left",
            content: function (e) {
                let dataItem = $("#grid").data("kendoGrid").dataItem(e.target.closest("tr"));
                if (dataItem.anhDaiDien == null || dataItem.anhDaiDien == '')
                    return $.i18n('label_khongcoanhdaidien')
                else {
                    let src = SERVERIMAGE + dataItem.anhDaiDien;
                    return '<img src="' + src + '" alt="" class="avatar-tooltip">';
                }
            }
        }).data("kendoTooltip");
    }

    function openFormDetail(id) {
        $state.go('editkhachhang', { idkhachhang: id });
    }
    function validationOpenDetail(listRowsSelected) {
        let flag = true;
        let msg = "";
        if (listRowsSelected.length > 1) {
            msg = $.i18n('label_canchonmotkhachhangdethuchien');
            flag = false;
        } else if (listRowsSelected.length == 0) {
            flag = false;
            msg = $.i18n('label_chuachonkhachhangdethuchien');
        }
        if (!flag) {
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n(msg) }, "error");
        }
        return flag;
    }

    function validatethemsua(_obj) {
        let flag = true;
        let msg = '';
        let regexEmail = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

        if (flag && (_obj.ten == '' || _obj.ten == undefined)) {
            flag = false;
            msg = $.i18n("label_tenkhachhangkhongduocdetrong");
        }

        if (flag && (_obj.soDienThoai == '' || _obj.soDienThoai == undefined)) {
            flag = false;
            msg = $.i18n("label_sodienthoaikhongduocbotrong");
        }

        if (flag && (_obj.diaChi == '' || _obj.diaChi == undefined)) {
            flag = false;
            msg = $.i18n("label_diachikhongduocbotrong");
        }

        if (flag && _obj.email != '' && !regexEmail.test(_obj.email)) {
            flag = false;
            msg = $.i18n('label_emailkhongdungdinhdang');
        }

        if (!flag)
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n(msg) }, 'warning');

        return flag;
    }

    $scope.fileUpload = '';
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
            $scope.$apply(function () {
                $scope.fileUpload = result;
            });

        }).fail(function (a, b, c) {
            $scope.$apply(function () {
                $scope.fileUpload = '';
            });
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
    $("#filesExcel").closest(".k-upload").find("span").text($.i18n("label_taifiledulieulen"))
    $("#files").closest(".k-upload").find("span").text($.i18n("label_chontep"))

    //event
    $scope.themMoiKhachHang = function () {
        openFormDetail(0);
    }

    $scope.openEditMultiKhachHang = function () {
        uid_arr = [];
        var dataSourceGridKhachHangEdit = new kendo.data.DataSource({
            data: [],
            schema: {
                model: {
                    id: "iD_KhachHang"
                }
            },
            pageSize: 20
        });
        var gridkhachhang = $("#grid").data("kendoGrid");
        //console.log(gridkhachhang.select());
        if (gridkhachhang.select().length > 0) {
            gridkhachhang.select().each(function () {
                var dataItem = gridkhachhang.dataItem(this);
                if (uid_arr.indexOf(dataItem.iD_KhachHang) < 0) {
                    uid_arr.push(dataItem.iD_KhachHang);
                    dataSourceGridKhachHangEdit.add(dataItem);
                }
            })
            if (uid_arr.length > 1) {
                //var gridd = $("#khachhang_grid_edit").data("kendoGrid");
                $scope.khachhang_grid_editData = dataSourceGridKhachHangEdit;
                $scope.khachhang_window_editmulti.maximize().center().open();
            } else {
                $scope.suaKhachHang();
            }
        } else {
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_canchonmotkhachahngdethuchien') }, "error");
        }
    }

    $scope.suaKhachHang = function () {
        let listRowsSelected = commonGetRowSelected("#grid");
        if (validationOpenDetail(listRowsSelected)) {
            openFormDetail(listRowsSelected[0].iD_KhachHang);
        }
    }
    $scope.xuatExcel = function () {
        commonOpenLoadingText("#btn_xuatexcelkhachhang");

        var dataSource = $("#grid").data("kendoGrid").dataSource;
        var filter = dataSource.filter();
        var filterSite = {}

        if (filter !== undefined) {
            var list_filters = filter.filters
            list_filters.forEach(function (item) {
                filterSite[item.field] = item.value;
            });
        }

        khachHangDataService.exportExcel(filterSite).then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayravuilongloadlaitrang') }, 'warning');
            commonCloseLoadingText("#btn_xuatexcelkhachhang")
        });
    }
    $scope.taiFileMau = function () {
        commonOpenLoadingText("#btn_taifilemaukhachhang");

        khachHangDataService.taiFileMau().then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayravuilongloadlaitrang') }, 'warning');
            commonCloseLoadingText("#btn_taifilemaukhachhang")
        });
    }
    $scope.xoaKhachHang = function () {
        let arr = $("#grid").data("kendoGrid").selectedKeyNames();
        if (arr.length <= 0)
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_chuachonkhachhang') }, 'warning');
        else {
            let data = [];
            for (let i = 0; i < arr.length; i++) {
                data.push(parseInt(arr[i]));
            }
            openConfirm($.i18n('label_bancochacchanmuonxoakhachhangkhong'), 'apDungXoaKhachHang', null, data);
        }
    }
    $scope.apDungXoaKhachHang = function (data) {
        khachHangDataService.del(data).then(function (result) {
            if (result.flag) {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                loadgrid();
            }
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
        });
    }

    $scope.xoaAllKhachHang = function () {
        openConfirm($.i18n('label_bancochacchanmuonxoakhachhangkhong'), 'apDungXoaAllKhachHang', null, null);
    }
    $scope.apDungXoaAllKhachHang = function () {
        khachHangDataService.delall().then(function (result) {
            if (result.flag) {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                loadgrid();
            }
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
        });
    }

    $scope.importExcel = function () {
        $scope.fileUpload = '';
        $scope.formimport.center().open();
    }
    $scope.capNhatTuExcel = function () {
        commonOpenLoadingText("#btn_capnhatimport");

        khachHangDataService.importkhachhang($scope.fileUpload).then(function (result) {
            if (result.flag)
                if (result.data.status == 200) {
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_tacvuthuchienthanhcong') }, 'success');
                    $scope.formimport.center().close();
                    $scope.loadgrid();
                }
                else if (result.data.status == 201)
                    commonDownFile(result.data);
                else if (result.data.status == 204)
                    Notification({
                        title: $.i18n('label_thongbao'), message: $.i18n('label_filemaukhongdungdinhdanghoackhongtontaibanghi')
                    }, 'warning');
                else if (result.data.status == 411)
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_soluongbanghikhongvuotqua2000dong') }, 'warning');
                else
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloisayravuilongkiemtralaifilecapnhat') }, 'warning');
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayra_loadlaitrang') }, 'warning');

            commonCloseLoadingText("#btn_capnhatimport")
        });
    }
    $scope.huyCapNhatExcel = function () {
        $scope.formimport.center().close();
    }

    $scope.openDetailFromGrid = function (iD_KhachHang) {
        openFormDetail(iD_KhachHang);
    }
    $scope.xemvitrichuacotoado = function () {
        Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_khachhangchuacothongtinvitri') }, 'warning');
    }
    $scope.xemvitri = function (vido, kinhdo) {
        let url = 'https://www.google.com/maps/dir/' + vido.toString() + ',' + kinhdo.toString();
        window.open(url, '_blank');
    }

    init();

    $("#grid").on("dblclick", "tr[role='row']", function () {
        let grid = $("#grid").data("kendoGrid");
        let row = grid.table.find("[data-uid=" + $(this).attr("data-uid") + "]");

        grid.clearSelection();
        grid.select(row);

        let selectedItem = grid.dataItem(grid.select());

        openFormDetail(selectedItem.iD_KhachHang);
    })
    var edittingFlag = false
    var currentRowEditUID = "";
    $("#khachhang_grid_edit").on("dblclick", "tr[role='row']", function () {
        edittingFlag = true
        currentRowEditUID = $(this).attr("data-uid");
        var row = $("#grid").data("kendoGrid").table.find("[data-uid=" + $(this).attr("data-uid") + "]");
        $("#khachhang_grid_edit").data("kendoGrid").editRow(row);
        //$scope.openEditWindow();
    })
    $("#khachhang_grid_edit").on("click", "tr[role='row']", function () {
        var row = $("#grid").data("kendoGrid").table.find("[data-uid=" + $(this).attr("data-uid") + "]");
        if (edittingFlag && $(this).attr("data-uid") != currentRowEditUID) {
            $("#khachhang_grid_edit").data("kendoGrid").saveChanges();
            edittingFlag = false;
        }
        //$scope.openEditWindow();
    })


    $scope.khachhang_grid_editOptions = {
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        },
        height: "calc(100vh - 80px)",
        persistSelection: true,
        resizable: true,
        pageable: pageableShort,
        sortable: true,
        filterable: filterable,
        scrollable: true,
        autoFitColumn: true,
        editable: {
            mode: "inline"
        },
        change: function () {
            this.editRow(this.select());
        },
        columns: [{
            title: "STT",
            template: "#= ++record #",
            width: 50,
            attributes: {
                class: "text-center"
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }
        },
        {
            field: "ten",
            //template: '#if (imgurl != null){# <span style="height:50px;line-height:50px;">#= ten #</span> #}else{#<span>#= ten #</span>#}#',
            title: $.i18n("label_tenkhachhang"),
            width: "230px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid,
        },
        {
            field: "maKH",
            title: $.i18n("label_makhachhang"),
            width: "230px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid,
        },
        {
            field: "diaChi",
            title: $.i18n("header_diachi"),
            width: "230px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid,
        },
        {
            field: "tenKhuVuc",
            title: $.i18n("label_khuvuc"),
            width: "230px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid,
            editor: function (container, options) {
                ComboboxDataService.getDataKhuVuc().then(function (result) {
                    var dsKhuVucDatasource = new kendo.data.DataSource({
                        data: result.data
                    });

                    ComBoEditKhuVuc = $('<input />')
                        .appendTo(container)
                        .kendoComboBox({
                            dataTextField: "tenKhuVuc",
                            dataValueField: "iD_KhuVuc",
                            filter: "contains",
                            clearButton: false,
                            suggest: true,
                            delay: 1000,
                            select: function (e) {
                                options.model.iD_KhuVuc = e.dataItem.iD_KhuVuc;
                                options.model.tenKhuVuc = e.dataItem.tenKhuVuc;
                            },
                            dataSource: dsKhuVucDatasource
                        }).data("kendoComboBox");
                    if (options.model.iD_KhuVuc > 0) {
                        ComBoEditKhuVuc.value(options.model.iD_KhuVuc);
                    }
                })

            }
        },
        {
            field: "tenTinh",
            title: $.i18n("header_tinhthanh"),
            width: "230px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid,
            editor: function (container, options) {
                container.html("");
                var ComboEditTinh = $('<input id="ComboEditTinh" />')
                    .appendTo(container)
                    .kendoComboBox({
                        dataTextField: "tenTinh",
                        dataValueField: "iD_Tinh",
                        clearButton: false,
                        filter: "contains",
                        select: function (e) {
                            options.model.iD_Tinh = e.dataItem.iD_Tinh;
                            options.model.tenTinh = e.dataItem.tenTinh;
                            options.model.iD_Quan = "";
                            options.model.tenQuan = "";
                            options.model.iD_Phuong = "";
                            options.model.tenPhuong = "";
                            $("#khachhang_grid_edit").data("kendoGrid").options.columns[6].editor(container.next(), options);
                            $("#khachhang_grid_edit").data("kendoGrid").options.columns[7].editor(container.next().next(), options);
                        },
                        suggest: true,
                    }).data("kendoComboBox");
                //if (options.model.iD_KhuVuc > 0) {
                ComboboxDataService.getTinhThanh().then(function (result) {
                    var dsTinhDatasource = new kendo.data.DataSource({
                        data: result.data
                    })
                    ComboEditTinh.setDataSource(dsTinhDatasource);
                    if (options.model.iD_Tinh > 0) {
                        ComboEditTinh.value(options.model.iD_Tinh);
                    }
                })
                //}
                return ComboEditTinh;
            }
        },
        {
            field: "tenQuan",
            title: $.i18n("header_quanhuyen"),
            width: "230px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid,
            editor: function (container, options) {
                container.html("");
                var ComboEditQuan = $('<input />')
                    .appendTo(container)
                    .kendoComboBox({
                        dataTextField: "tenQuan",
                        dataValueField: "iD_Quan",
                        clearButton: false,
                        filter: "contains",
                        select: function (e) {
                            options.model.iD_Quan = e.dataItem.iD_Quan;
                            options.model.tenQuan = e.dataItem.tenQuan;
                            options.model.iD_Phuong = "";
                            options.model.tenPhuong = "";
                            $("#khachhang_grid_edit").data("kendoGrid").options.columns[7].editor(container.next(), options);
                        },
                        suggest: true,
                    }).data("kendoComboBox");
                if (options.model.iD_Tinh > 0) {
                    ComboboxDataService.getQuanHuyen(options.model.iD_Tinh).then(function (result) {
                        var dsQuanDatasource = new kendo.data.DataSource({
                            data: result.data
                        })
                        ComboEditQuan.setDataSource(dsQuanDatasource);
                        if (options.model.iD_Quan > 0) {
                            ComboEditQuan.value(options.model.iD_Quan);
                        }
                    })
                }
                return ComboEditQuan;
            }
        },
        {
            field: "tenPhuong",
            title: $.i18n("header_phuongxa"),
            width: "230px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid,
            editor: function (container, options) {
                container.html("");
                var ComboEditPhuong = $('<input />')
                    .appendTo(container)
                    .kendoComboBox({
                        dataTextField: "tenPhuong",
                        dataValueField: "iD_Phuong",
                        clearButton: false,
                        filter: "contains",
                        select: function (e) {
                            options.model.iD_Phuong = e.dataItem.iD_Phuong;
                            options.model.tenPhuong = e.dataItem.tenPhuong;
                        },
                        suggest: true,
                    }).data("kendoComboBox");
                if (options.model.iD_Quan > 0) {
                    ComboboxDataService.getXaPhuong(options.model.iD_Quan).then(function (result) {
                        var dsPhuongatasource = new kendo.data.DataSource({
                            data: result.data
                        })
                        ComboEditPhuong.setDataSource(dsPhuongatasource);
                        if (options.model.iD_Phuong > 0) {
                            ComboEditPhuong.value(options.model.iD_Phuong);
                        }
                    })
                }
                return ComboEditPhuong;
            }
        },
        {
            field: "soDienThoai",
            title: $.i18n("label_dienthoai"),
            width: "230px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid
        },
        {
            field: "soDienThoai2",
            title: $.i18n("label_dienthoai") + "1",
            width: "230px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid
        },
        {
            field: "soDienThoai3",
            title: $.i18n("label_dienthoai") + "2",
            width: "230px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid
        },
        {
            field: "email",
            title: $.i18n("label_email"),
            width: "230px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid
        },
        {
            field: "tenLoaiKhachHang",
            title: $.i18n("label_tenloaikhachhang"),
            width: "230px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid,
            editor: function (container, options) {
                var ComboEditLoaiKhachHang = $('<input />')
                    .appendTo(container)
                    .kendoComboBox({
                        dataTextField: "tenLoaiKhachHang",
                        dataValueField: "iD_LoaiKhachHang",
                        clearButton: false,
                        filter: "contains",
                        select: function (e) {
                            options.model.iD_LoaiKhachHang = e.dataItem.iD_LoaiKhachHang;
                            options.model.tenLoaiKhachHang = e.dataItem.tenLoaiKhachHang;
                        },
                        suggest: true,
                    }).data("kendoComboBox");
                ComboboxDataService.getLoaiKhachHang().then(function (result) {
                    var dsLoaiKhachHangDatasource = new kendo.data.DataSource({
                        data: result.data
                    })
                    console.log(options.model);
                    ComboEditLoaiKhachHang.setDataSource(dsLoaiKhachHangDatasource);
                    if (options.model.iD_LoaiKhachHang > 0) {
                        ComboEditLoaiKhachHang.value(options.model.iD_LoaiKhachHang);
                    }
                })
                return ComboEditLoaiKhachHang;
            }

        },
        {
            field: "tenNhomKH",
            title: $.i18n("header_tennhomkhachhang"),
            width: "230px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid,
            editor: function (container, options) {
                var ComboEditNhom = $('<input />')
                    .appendTo(container)
                    .kendoComboBox({
                        dataTextField: "tenKenhBanHang",
                        dataValueField: "iD_KenhBanHang",
                        clearButton: false,
                        filter: "contains",
                        select: function (e) {
                            options.model.iD_NhomKH = e.dataItem.iD_KenhBanHang;
                            options.model.tenNhomKH = e.dataItem.tenKenhBanHang;
                            $("#khachhang_grid_edit").data("kendoGrid").options.columns[14].editor(container.next(), options);
                        },
                        suggest: true,
                    }).data("kendoComboBox");
                ComboboxDataService.getKenhBanHang().then(function (result) {
                    var dsNhomDatasource = new kendo.data.DataSource({
                        data: result.data
                    })
                    ComboEditNhom.setDataSource(dsNhomDatasource);
                    if (options.model.iD_NhomKH > 0) {
                        ComboEditNhom.value(options.model.iD_NhomKH);
                    }
                })
                return ComboEditNhom;
            }
        },
        {
            field: "tenKenhCapTren",
            title: $.i18n("label_kenhbanhangcaptren"),
            width: "230px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid,
            editor: function (container, options) {
                container.html("");
                var ComboEditKenhCapTren = $('<input />')
                    .appendTo(container)
                    .kendoComboBox({
                        dataTextField: "tenKhachHang",
                        dataValueField: "iD_KhachHang",
                        clearButton: false,
                        filter: "contains",
                        select: function (e) {
                            options.model.iD_KenhCapTren = e.dataItem.iD_KhachHang;
                            options.model.tenKenhCapTren = e.dataItem.tenKhachHang;
                        },
                        suggest: true,
                    }).data("kendoComboBox");
                ComboboxDataService.getKenhBanHangCapTren(options.model.iD_NhomKH).then(function (result) {
                    var dsKenhBanHangCapTrenDatasource = new kendo.data.DataSource({
                        data: result.data
                    })
                    ComboEditKenhCapTren.setDataSource(dsKenhBanHangCapTrenDatasource);
                    if (options.model.iD_KenhCapTren > 0) {
                        ComboEditKenhCapTren.value(options.model.iD_KenhCapTren);
                    }
                })
                return ComboEditKenhCapTren;
            }
        },
        {
            field: "maSoThue",
            title: $.i18n("label_masothue"),
            width: "230px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid
        },
        {
            field: "nguoiLienHe",
            title: $.i18n("label_nguoilienhe"),
            width: "230px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid
        },
        {
            field: "ghiChu",
            title: $.i18n("label_ghichu"),
            width: "230px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid
        }
        ]
    }

    $scope.closekhachhang_window_editmulti = function () {
        $("#khachhang_grid_edit").data("kendoGrid").setDataSource(new kendo.data.DataSource({
            data: []
        }));
        $("#khachhang_window_editmulti").data("kendoWindow").close();
        $scope.loadgrid();
    }

    $scope.SaveMulti = function () {
        let data = $("#khachhang_grid_edit").data("kendoGrid").dataSource.data();
        let arr = [];
        let flag = true;

        for (let i = 0; i < data.length; i++) {
            let value = data[i];
            if (!validatethemsua(value)) {
                flag = false;
                break
            }
            let formData = {
                MaKH: value.maKH,
                Ten: value.ten,
                DiaChi: value.diaChi,
                KinhDo: value.kinhDo,
                ViDo: value.viDo,
                DuongPho: value.duongPho,
                ID_KhuVuc: value.iD_KhuVuc,
                ID_Tinh: value.iD_Tinh,
                ID_Quan: value.iD_Quan,
                ID_Phuong: value.iD_Phuong,
                SoDienThoaiMacDinh: value.dienThoai,
                Fax: value.fax,
                SoDienThoai1: value.soDienThoai,
                SoDienThoai2: value.soDienThoai2,
                SoDienThoai3: value.soDienThoai3,
                ID_LoaiKhachHang: value.iD_LoaiKhachHang,
                ID_NhomKH: value.iD_NhomKH,
                ID_Cha: value.iD_KenhCapTren,
                NguoiLienHe: value.nguoiLienHe,
                Email: value.email,
                Website: value.website,
                DiaChiXuatHoaDon: value.diaChiXuatHoaDon,
                SoTKNganHang: value.soTKNganHang,
                MaSoThue: value.maSoThue,
                GhiChu: value.ghiChu,
                ImgUrl: "",
                IDQLLH: $rootScope.UserInfo.iD_QLLH,
                ID_NhanVien: $rootScope.UserInfo.iD_QuanLy,
                ID_QuanLy: $rootScope.UserInfo.iD_QuanLy,
                IDKhachHang: value.iD_KhachHang
            }
            arr.push(formData);
        }

        //$.each(data, function (index, value) {
        //    var formData = {
        //        MaKH: value.maKH,
        //        Ten: value.ten,
        //        DiaChi: value.diaChi,
        //        KinhDo: value.kinhDo,
        //        ViDo: value.viDo,
        //        DuongPho: value.duongPho,
        //        ID_KhuVuc: value.iD_KhuVuc,
        //        ID_Tinh: value.iD_Tinh,
        //        ID_Quan: value.iD_Quan,
        //        ID_Phuong: value.iD_Phuong,
        //        SoDienThoaiMacDinh: value.dienThoai,
        //        Fax: value.fax,
        //        SoDienThoai1: value.soDienThoai,
        //        SoDienThoai2: value.soDienThoai2,
        //        SoDienThoai3: value.soDienThoai3,
        //        ID_LoaiKhachHang: value.iD_LoaiKhachHang,
        //        ID_NhomKH: value.iD_NhomKH,
        //        ID_Cha: value.iD_KenhCapTren,
        //        NguoiLienHe: value.nguoiLienHe,
        //        Email: value.email,
        //        Website: value.website,
        //        DiaChiXuatHoaDon: value.diaChiXuatHoaDon,
        //        SoTKNganHang: value.soTKNganHang,
        //        MaSoThue: value.maSoThue,
        //        GhiChu: value.ghiChu,
        //        ImgUrl: "",
        //        IDQLLH: $rootScope.UserInfo.iD_QLLH,
        //        ID_NhanVien: $rootScope.UserInfo.iD_QuanLy,
        //        ID_QuanLy: $rootScope.UserInfo.iD_QuanLy,
        //        IDKhachHang: value.iD_KhachHang
        //    }
        //    arr.push(formData);
        //})
        //console.log(arr);
        if (flag)
            khachHangDataService.savemulti(arr).then(function (response) {
                if (response.flag == true) {
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(response.message) }, 'success');
                } else {
                    $scope.closeWindow();
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(response.message) }, 'warning');
                }
                $scope.closekhachhang_window_editmulti();
            })
    }


})