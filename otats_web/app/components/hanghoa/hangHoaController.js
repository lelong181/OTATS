angular.module('app').controller('hangHoaController', function ($rootScope, $scope, $timeout, $location, Notification, ComboboxDataService, hangHoaDataService, printer) {
    CreateSiteMap();

    let idNhom = 0;
    let id_MatHang = 0;
    let idloaikhachhang = -1;
    let suDungBangGia = false;
    let datacomboloaikhachhang = [];

    let image_url = '';

    function init() {
        getquyen();

        initcauhinhbanggialoaikhachhang();

        inittreeview();
        initcombobox();
        $scope.editorOptions = {
            tools: [
                "bold",
                "italic",
                "underline",
                "strikethrough",
                "justifyLeft",
                "justifyCenter",
                "justifyRight",
                "justifyFull",
                "insertUnorderedList",
                "insertOrderedList",
                "indent",
                "outdent",
                "createLink",
                "unlink",
                "insertImage",
                "insertFile",
                "subscript",
                "superscript",
                "tableWizard",
                "createTable",
                "addRowAbove",
                "addRowBelow",
                "addColumnLeft",
                "addColumnRight",
                "deleteRow",
                "deleteColumn",
                "mergeCellsHorizontally",
                "mergeCellsVertically",
                "splitCellHorizontally",
                "splitCellVertically",
                "viewHtml",
                "formatting",
                "cleanFormatting",
                "copyFormat",
                "applyFormat",
                "fontName",
                "fontSize",
                "foreColor",
                "backColor",
                "print"
            ],
            messages: {
                bold: "Bold",
                italic: "Italic",
                underline: "Underline",
                strikethrough: "Strikethrough",
                superscript: "Superscript",
                subscript: "Subscript",
                justifyCenter: "Center text",
                justifyLeft: "Align text left",
                justifyRight: "Align text right",
                justifyFull: "Justify",
                insertUnorderedList: "Insert unordered list",
                insertOrderedList: "Insert ordered list",
                indent: "Indent",
                outdent: "Outdent",
                createLink: "Insert hyperlink",
                unlink: "Remove hyperlink",
                insertImage: "Thêm ảnh",
                insertFile: "Thêm file",
                insertHtml: "Thêm mã html",
                fontName: "Chọn font chữ",
                fontNameInherit: "(Mặc định)",
                fontSize: "Chọn cỡ chữ",
                fontSizeInherit: "(Mặc định)",
                formatBlock: "Định dạng",
                formatting: "Định dạng",
                style: "Styles",
                viewHtml: "Xem dưới dạng mã html",
                overwriteFile: "Tệp có tên \"{0}\" đã tồn tại trong thư mục. Bạn có muốn ghi đè?",
                imageWebAddress: "Địa chỉ ảnh",
                imageAltText: "Mô tả ảnh",
                fileWebAddress: "Địa chỉ file",
                fileTitle: "Mô tả file",
                linkWebAddress: "Link web",
                linkText: "Mô tả",
                linkToolTip: "Chữ nổi",
                linkOpenInNewWindow: "Mở link trên tab mới",
                dialogInsert: "Thêm",
                dialogUpdate: "Cập nhật",
                dialogCancel: "Hủy",
                dialogCancel: "Hủy",
                createTable: "Tạo bảng",
                addColumnLeft: "Thêm cột bên trái",
                addColumnRight: "Thêm cột bên phải",
                addRowAbove: "Thêm dòng bên trên",
                addRowBelow: "Thêm dòng bên dưới",
                deleteRow: "Xóa dòng",
                deleteColumn: "Xóa cột",
                imageWidth: "Độ rộng (px)",
                imageHeight: "Độ cao (px)"
            }
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

    function initcauhinhbanggialoaikhachhang() {
        ComboboxDataService.getcauhinhchung().then(function (result) {

            if (result.flag) {
                suDungBangGia = result.data.suDungBangGiaLoaiKhachHang;
            } else {
                suDungBangGia = false;
            }
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

        $("#treeview").kendoTreeView({
            dataSource: dataSource,
            dataTextField: "name",
            dataValueField: "id",
            select: onSelectNhom,
            template: "<img src='" + SERVERIMAGE + "#= item.anhDaiDien #' style='width: 30px;' />#= item.name # ",
        });

        loadtreeView();
    }
    function loadtreeView() {
        hangHoaDataService.getListNhomMatHang().then(function (result) {
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

        let tree = $("#treeview").data("kendoTreeView");
        tree.setDataSource(dataSource);
        tree.expand(".k-item");
        tree.select(".k-first");

        let selectedNode = tree.select();
        idNhom = tree.dataItem(selectedNode).id;
        loadgrid();
    }
    function onSelectNhom(e) {
        idNhom = $("#treeview").getKendoTreeView().dataItem(e.node).id;
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
        dataList.push({
            field: "anhDaiDien", title: $.i18n('header_anhdaidien'),
            template: function (dataItem) {
                if (dataItem.anhDaiDien == null || dataItem.anhDaiDien == '')
                    return ''
                else {
                    let src = SERVERIMAGE + dataItem.anhDaiDien;
                    return '<img src="' + src + '" alt="" class="rounded-circle" style="    width: 100px;height: 100px;} ">';
                }
            },
            attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: false, width: "130px"
        });
        dataList.push({ field: "maHang", title: $.i18n('header_mahang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px" });
        dataList.push({ field: "tenHang", title: $.i18n('header_tenhang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "220px" });
        dataList.push({ field: "tenDonVi", title: $.i18n('header_donvi'), attributes: { class: "text-center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "80px" });
        dataList.push({ field: "giaBuon", title: $.i18n('header_giabanbuon'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), attributes: { class: "text-right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px" });
        dataList.push({ field: "giaLe", title: $.i18n('header_giabanle'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), attributes: { class: "text-right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px" });
        dataList.push({ field: "tenDanhMuc", title: $.i18n('header_danhmuc'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px" });
        dataList.push({ field: "ghiChuGia", title: $.i18n('header_ghichugia'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px" });

        dataList.push({
            field: "tacvu",
            title: $.i18n('header_banggialoaikhachhang'),
            template: function (e) {

                return '<button ng-click="openformbanggialoaikhachhang()" class="btn btn-link btn-menubar" title ="' + $.i18n('header_banggialoaikhachhang') + '" ><i class="fas fa-plus-circle fas-sm color-infor"></i></button> '
            },
            attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "tacvu",
            title: "Cấu hình hoa hồng",
            template: function (e) {

                return '<button ng-click="openformloinhuan()" class="btn btn-link btn-menubar"><i class="fas fa-money-bill fas-sm color-success"></i></button> '
            },
            attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "maHang",
            title: "Barcode",
            template: function (e) {

                return '<button ng-click="printLabel(\'' + e.maHang + '\',\'' + e.tenHang + '\',' + e.giaLe + ')" class="btn btn-link btn-menubar" title ="In tem" ><i class="fas fa-barcode fas-sm color-infor"></i></button> '
            },
            attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });

        return dataList;
    }
    function loadgrid() {
        kendo.ui.progress($("#grid"), true);
        $scope.gridOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".toolbarmenu").height());
                return heightGrid - 38;
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

        hangHoaDataService.getlist(idNhom).then(function (result) {
            $scope.gridData = {
                data: result.data,
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
            kendo.ui.progress($("#grid"), false);

            showhidecolums();
        });
    }
    function showhidecolums() {
        let grid = $("#grid").data("kendoGrid");

        if (suDungBangGia) {
            grid.showColumn("tacvu");
        }
        else {
            grid.hideColumn("tacvu");
        }

        grid.refresh();
    }

    function validationOpenDetail(listRowsSelected) {
        let flag = true;
        let msg = "";
        if (listRowsSelected.length > 1) {
            msg = $.i18n("label_canchonmotmathangdethuchien");
            flag = false;
        } else if (listRowsSelected.length == 0) {
            flag = false;
            msg = $.i18n("label_chuachonmathangdethuchien");
        }
        if (!flag) {
            Notification({ title: $.i18n('label_thongbao'), message: msg }, "error");
        }
        return flag;
    }
    function openFormDetail(_idMatHang) {
        image_url = '';
        $scope.showbanggialoaikhachhang = ($scope.permission.sua > 0 && _idMatHang > 0 && suDungBangGia);
        $scope.idhang = _idMatHang;

        $scope.gridDichVuData = {
            data: [],
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
                        hanSuDung: {
                            type: "number", editable: true, validation: { min: 1 }
                        },
                        giaBan: {
                            type: "number", editable: true
                        },
                        loai: {
                            type: "number", editable: true
                        },
                        tenHienThi: {
                            type: "text", editable: true
                        }
                    }
                }
            },
            pageSize: 20
        };



        $scope.gridDichVuOptions = {
            data: new kendo.data.DataSource({
                data: [],
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
                            hanSuDung: {
                                type: "number", editable: true, validation: { min: 1 }
                            },
                            giaBan: {
                                type: "number", editable: true
                            },
                            loai: {
                                type: "number", editable: true
                            },
                            iD_NhaCungCap: {
                                type: "number", editable: true
                            },
                            tenHienThi: {
                                type: "text", editable: true
                            }
                        }
                    }
                },
                pageSize: 20
            }),
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
            save: function (e) {
                setTimeout(function () {
                    e.sender.refresh();
                    e.sender.dataSource.fetch(function () {
                    });
                })
            },
            columns: listDichVuColumnsgrid()
        };


        if (_idMatHang > 0)
            loadeditmathangform(_idMatHang);
        else
            loadaddmathangform();

        $("#files_mathang").kendoUpload({
            multiple: false,
            select: onUploadImageSuccess,
            validation: {
                allowedExtensions: [".jpg", ".jpeg", ".png"]
            },
            showFileList: false
        });
        $("#files_mathang").closest(".k-upload").find("span").text($.i18n("label_chonanhdaidien"));

        $scope.formdetail.center().maximize().open();
    }

    function loadeditmathangform(_idMatHang) {
        hangHoaDataService.getbyid(_idMatHang).then(function (result) {
            $scope.objectHang = result.data;
            if ($scope.objectHang.isDichVu > 0) {
                $scope.objectHang.isDichVu = true;
            }
            if ($scope.objectHang.iD_DANHMUC > 0) {
                $scope.nhomhanghoaSelected = $scope.objectHang.iD_DANHMUC;
                $("#nhomhanghoa").data("kendoDropDownTree").value($scope.objectHang.iD_DANHMUC);
            }
            else {
                $scope.nhomhanghoaSelected = undefined;
                $("#nhomhanghoa").data("kendoDropDownTree").value("");
            }

            if ($scope.objectHang.idDonVi > 0)
                $("#donvitinh").data("kendoComboBox").value($scope.objectHang.idDonVi);
            else
                $("#donvitinh").data("kendoComboBox").value("");
            if ($scope.objectHang.iD_NhaCungCap > 0)
                $("#nhacungcap").data("kendoComboBox").value($scope.objectHang.iD_NhaCungCap);
            else
                $("#nhacungcap").data("kendoComboBox").value("");
            if ($scope.objectHang.iD_NhanHieu > 0)
                $("#nhanhieu").data("kendoComboBox").value($scope.objectHang.iD_NhanHieu);
            else
                $("#nhanhieu").data("kendoComboBox").value("");

            if ($scope.objectHang.anhDaiDien != '') {
                image_url = $scope.objectHang.anhDaiDien;
                $("#previewnhanvien").html('<div class="imgprevew"><img src="' + SERVERIMAGE + $scope.objectHang.anhDaiDien + '" style="width:154px;max-height:179px;" /></div>')
            }
            else {
                $("#previewnhanvien").html('')
            }
            if ($scope.objectHang.lstDichVu != null) {
                $scope.gridDichVuData = {
                    data: $scope.objectHang.lstDichVu,
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
                                hanSuDung: {
                                    type: "number", editable: true, validation: { min: 1 }
                                },
                                giaBan: {
                                    type: "number", editable: true
                                },
                                loai: {
                                    type: "number", editable: true
                                },
                                tenHienThi: {
                                    type: "text", editable: true
                                }
                            }
                        }
                    },
                    pageSize: 20
                };
            }
        });
    }
    function loadaddmathangform() {
        $scope.objectHang = {
            anhDaiDien: "",
            danhSachAnh: [],
            ghiChuGia: "",
            giaBuon: 0,
            giaLe: 0,
            iD_DANHMUC: 0,
            iD_NhaCungCap: 0,
            iD_NhanHieu: 0,
            idDonVi: 0,
            idMatHang: 0,
            idqllh: 0,
            khuyenMai: "",
            linkGioiThieu: "",
            maHang: "",
            moTa: "",
            moTaNgan: "",
            soLuong: 0,
            soLuongDieuChuyenKho: 0,
            soLuongTon: 0,
            tenDanhMuc: "",
            tenDonVi: "",
            tenHang: "",
        }

        if (idNhom > 0) {
            $scope.nhomhanghoaSelected = idNhom;
            $("#nhomhanghoa").data("kendoDropDownTree").value(idNhom);
        }
        else {
            $scope.nhomhanghoaSelected = undefined;
            $("#nhomhanghoa").data("kendoDropDownTree").value("");
        }
        $("#previewnhanvien").html('');

        $("#donvitinh").data("kendoComboBox").value("");
        $("#nhacungcap").data("kendoComboBox").value("");
        $("#nhanhieu").data("kendoComboBox").value("");
    }

    function initcombobox() {
        ComboboxDataService.getDataDonViTinh().then(function (result) {
            $scope.donvitinhData = result.data;
        });
        ComboboxDataService.getDataNhanHieu().then(function (result) {
            $scope.nhanhieuData = result.data;
        });
        ComboboxDataService.getDataNhaCungCap().then(function (result) {
            $scope.nhacungcapData = result.data;
        });

        loadnhomhang();
    }

    function loadnhomhang() {
        ComboboxDataService.getDataNhomMatHang().then(function (result) {
            let _data = result.data;
            _data = _data.filter(function (item) {
                return item.iD_DANHMUC > 0
            })

            $scope.nhomchaData = _data;
        });

        ComboboxDataService.getDataTreeNhomMatHang().then(function (result) {
            let data = result.data;
            data = data.filter(function (item) {
                return item.id > 0
            })

            $scope.nhomhanghoaOptions = {
                dataTextField: "tenMatHang",
                dataValueField: "id",
                valuePrimitive: true,
                dataSource: new kendo.data.HierarchicalDataSource({
                    data: data,
                    schema: {
                        model: {
                            children: "childs"
                        }
                    }
                })
            };
        })
    }

    function validatethemsua() {
        let flag = true;
        let msg = '';

        if ($scope.donvitinhselect != undefined)
            $scope.objectHang.idDonVi = ($scope.donvitinhselect.iD_DonVi < 0) ? 0 : $scope.donvitinhselect.iD_DonVi;
        if ($scope.nhacungcapselect != undefined)
            $scope.objectHang.iD_NhaCungCap = ($scope.nhacungcapselect.iD_NhaCungCap < 0) ? 0 : $scope.nhacungcapselect.iD_NhaCungCap;
        if ($scope.nhanhieuselect != undefined)
            $scope.objectHang.iD_NhanHieu = ($scope.nhanhieuselect.iD_NhanHieu < 0) ? 0 : $scope.nhanhieuselect.iD_NhanHieu;
        if ($scope.nhomhanghoaSelected != undefined)
            $scope.objectHang.iD_DANHMUC = $scope.nhomhanghoaSelected;

        if ($scope.objectHang.maHang == '' || $scope.objectHang.maHang == undefined) {
            flag = false;
            msg = $.i18n('label_mahangkhongduocdetrong');
            $("#maHang").focus();
        }

        if (flag && ($scope.objectHang.tenHang == '' || $scope.objectHang.tenHang == undefined)) {
            flag = false;
            msg = $.i18n('label_tenhangkhongduocdetrong');
            $("#tenHang").focus();
        }

        if (flag && $scope.objectHang.iD_DANHMUC <= 0) {
            flag = false;
            msg = $.i18n('label_nhomkhongduocdetrong');
            $("#nhomhanghoa").focus();
        }

        if (flag && $scope.objectHang.idDonVi <= 0) {
            flag = false;
            msg = $.i18n('label_donvikhongduocdetrong');
            $("#donvitinh").focus();
        }

        if (!flag)
            Notification({ title: $.i18n('label_thongbao'), message: msg }, 'warning');

        return flag;
    }
    function validatethemsuanhom() {
        let flag = true;
        let msg = '';

        if ($scope.nhomchaselect != undefined)
            $scope.objectNhom.ID_Parent = ($scope.nhomchaselect.iD_DANHMUC < 0) ? 0 : $scope.nhomchaselect.iD_DANHMUC;

        if ($scope.objectNhom.TenNhom == '' || $scope.objectNhom.TenNhom == undefined) {
            flag = false;
            msg = $.i18n('label_tennhomkhongduocdetrong');
            $("#nhomMatHang").focus();
        }

        if (flag && $scope.objectNhom.ID_Nhom > 0 && $scope.objectNhom.ID_Parent == idNhom) {
            flag = false;
            msg = $.i18n('label_khongthechonnhomchachinhlanhomsua');
        }

        if (!flag)
            Notification({ title: $.i18n('label_thongbao'), message: msg }, 'warning');

        return flag;
    }

    function openFormNhom(_idparent, _idnhom, _name, _anhdaidien) {
        if (_idnhom < 0)
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_khongthechinhsuanhommacdinh') }, 'warning');
        else {
            $("#files_nhommathang").kendoUpload({
                multiple: false,
                select: onUploadNhomImageSuccess,
                validation: {
                    allowedExtensions: [".jpg", ".jpeg", ".png"]
                },
                showFileList: false
            });
            $("#files_nhommathang").closest(".k-upload").find("span").text($.i18n("label_chonanhdaidien"));
            $scope.formnhom.center().open();
            $scope.objectNhom = {
                ID_Parent: _idparent,
                ID_Nhom: _idnhom,
                TenNhom: _name,
                AnhDaiDien: _anhdaidien
            }

            if (_idparent > 0)
                $("#nhomcha").data("kendoComboBox").value(_idparent);
            else
                $("#nhomcha").data("kendoComboBox").value("");


            if (_anhdaidien != '' && _anhdaidien != null) {
                $("#previewnhommathang").html('<div class="imgprevew"><img src="' + SERVERIMAGE + $scope.objectNhom.AnhDaiDien + '" style="width:100px;max-height:179px;" /></div>')
            }
            else {
                $("#previewnhommathang").html('')
            }
        }

    }

    let fileUpload = '';
    function onUploadExcelSuccess(e) {
        commonOpenLoadingText("#btn_capnhatimport");
        commonOpenLoadingText("#btn_capnhatbanggiaimport");

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
            commonCloseLoadingText("#btn_capnhatimport")
            commonCloseLoadingText("#btn_capnhatbanggiaimport")
        }).fail(function (a, b, c) {
            fileUpload = '';
            commonCloseLoadingText("#btn_capnhatimport")
            commonCloseLoadingText("#btn_capnhatbanggiaimport")
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_khongthedocduocfiledulieuvuilongkiemtravachonlai') }, 'warning');
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

    function onUploadImageSuccess(e) {
        let data = new FormData();
        data.append('file', e.files[0].rawFile);
        let files = e.files[0];
        if (files.extension.toLowerCase() != ".jpg" && files.extension.toLowerCase() != ".png" && files.extension.toLowerCase() != ".jpeg") {
            e.preventDefault();
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n("label_vuilongchonfileanhjpgpngjpeg") }, 'warning');
        } else {
            hangHoaDataService.uploadAnhDaiDien(data).then(function (result) {
                $("#previewnhanvien").html('<div class="imgprevew"><img src="' + urlApi + result.url + '" style="width:154px;max-height:179px;" /></div>')
                image_url = result.url;
                if (!result.flag)
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
            })
        }
    }

    function onUploadNhomImageSuccess(e) {
        let data = new FormData();
        data.append('file', e.files[0].rawFile);
        let files = e.files[0];
        if (files.extension.toLowerCase() != ".jpg" && files.extension.toLowerCase() != ".png" && files.extension.toLowerCase() != ".jpeg") {
            e.preventDefault();
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n("label_vuilongchonfileanhjpgpngjpeg") }, 'warning');
        } else {
            hangHoaDataService.uploadAnhDaiDien(data).then(function (result) {
                $("#previewnhommathang").html('<div class="imgprevew"><img src="' + urlApi + result.url + '" style="width:100px;max-height:179px;" /></div>')
                $scope.objectNhom.AnhDaiDien = result.url;
                if (!result.flag)
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
            })
        }
    }

    function listColumnsgrid_giaTheoLoaiKH() {
        let dataList = [];

        dataList.push({
            headerAttributes: { "class": "table-header-cell" }, template: function (dataItem) {
                return '<button class="btn btn-link btn-menubar" ng-click="deleteRowLoaiKhachHang()"><i class="fas fa-trash-alt fas-md color-danger"></i> </button>';
            }, width: "60px"
        });
        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({ field: "tenLoaiKhachHang", title: $.i18n('header_tenloaikhachhang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "250px" });
        dataList.push({ field: "giaBanBuon", title: $.i18n('header_giabanbuon'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), attributes: { class: "text-right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px" });
        dataList.push({ field: "giaBanLe", title: $.i18n('header_giabanle'), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), attributes: { class: "text-right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px" });
        dataList.push({ field: "ghiChu", title: $.i18n('header_ghichu'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid });

        return dataList;
    }

    function listColumnsgrid_LoiNhuan() {
        let dataList = [];

        dataList.push({
            headerAttributes: { "class": "table-header-cell" }, template: function (dataItem) {
                return '<button class="btn btn-link btn-menubar" ng-click="deleteRowLoiNhuan(' + dataItem.id + ')"><i class="fas fa-trash-alt fas-md color-danger"></i> </button>';
            }, width: "60px"
        });
        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({
            field: "iD_NhomTaiKhoan", title: $.i18n('header_tennhom'),
            template: function (e) {
                if (e.iD_NhomTaiKhoan == -1) { return "Mặc định" }
                else { return e.tenNhom; }
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "250px"
        });
        dataList.push({ field: "soLuongToiThieu", title: "SL Từ", format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), attributes: { class: "text-right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px" });
        dataList.push({ field: "soLuongToiDa", title: "SL Đến", format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), attributes: { class: "text-right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px" });
        dataList.push({ field: "tyLeHoaHong", title: "Hoa hồng (%)", format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), attributes: { class: "text-right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px" });
        dataList.push({ field: "tienHoaHong", title: "Hoa hồng (VNĐ)", format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), attributes: { class: "text-right" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px" });

        return dataList;
    }

    function listDichVuColumnsgrid() {
        let dataList = [];

        dataList.push({
            field: 'iD_DichVu',
            title: "Thao tác",
            attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell" }, template: function (dataItem) {
                return '<button class="btn btn-link btn-menubar" ng-click="deleteRowDichVu(\'' + dataItem.uid + '\')"><i class="fas fa-trash-alt fas-md color-danger"></i> </button>';
            },
            width: "60px",
            filterable: {
                cell: {
                    operator: "contains",
                    showOperators: false,
                    template: function (e) {
                        e.element.parent().html("<a class='k-button' title='Thêm dịch vụ' style='width:100%; height:25px;' ng-click='addRowDichVu()'><i class='fa fa-plus'></i></a>")
                    }
                }
            },
        });
        dataList.push({
            field: "tenHienThi", title: "Dịch vụ", headerAttributes: {
                class: "table-header-cell",


                style: "text-align: center"
            },
            editor: function (container, options) {
                hangHoaDataService.getalldichvu().then(function (result) {

                    $('<input name="' + options.field + '"/>').appendTo(container).kendoComboBox({
                        dataTextField: 'tenDichVu',
                        dataValueField: 'id',
                        autoBind: false,
                        valuePrimitive: false,
                        dataSource: result.data,
                        change: function (e) {
                            console.log(e.sender.dataItem());
                            let model = e.sender.dataItem()
                            options.model.iD_DichVu = model.id;
                            options.model.giaBan = model.giaBan;
                            options.model.tenHienThi = model.tenDichVu;
                        },
                    })
                });
            }
            , filterable: false, width: "250px"
        });
        dataList.push({
            field: "tenNhaCungCap", title: "Nhà cung cấp", headerAttributes: {
                class: "table-header-cell",


                style: "text-align: center"
            },
            editor: function (container, options) {
                ComboboxDataService.getDataNhaCungCap().then(function (result) {

                    $('<input name="' + options.field + '"/>').appendTo(container).kendoComboBox({
                        dataTextField: 'tenNhaCungCap',
                        dataValueField: 'iD_NhaCungCap',
                        autoBind: false,
                        valuePrimitive: false,
                        dataSource: result.data,
                        change: function (e) {
                            console.log(e.sender.dataItem());
                            let model = e.sender.dataItem()
                            options.model.iD_NhaCungCap = model.iD_NhaCungCap;
                            options.model.tenNhaCungCap = model.tenNhaCungCap;
                        },
                    })
                });
            }
            , filterable: false, width: "150px"
        });
        dataList.push({
            field: "giaBan",
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            title: "Giá gốc dịch vụ", attributes: { class: "text-center" },
            editor: function (container, options) {
                $('<input name="' + options.field + '"/>').appendTo(container).kendoNumericTextBox({
                    culture: "vi-VN"

                })
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: false, width: "150px"
        });
        dataList.push({
            field: "soLuong",
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            title: "Số lượng", attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: false, width: "150px"
        });
        dataList.push({
            field: "hanSuDung",
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            title: "Số ngày sử dụng", attributes: { class: "text-center" },
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
            editor: function (container, options) {
                $('<input name="' + options.field + '"/>').appendTo(container).kendoComboBox({
                    dataTextField: 'text',
                    dataValueField: 'value',
                    autoBind: false,
                    valuePrimitive: false,
                    dataSource: [{ text: 'Optional', value: 1 }, { text: 'Fixed', value: 2 }],
                    change: function (e) {
                        console.log(e.sender.dataItem());
                        let model = e.sender.dataItem()
                        options.model.loai = model.value;
                    },
                })
            },
            headerAttributes: {
                "class": "table-header-cell",
                style: "text-align: center"
            }, filterable: false, width: "150px"
        });

        return dataList;
    }

    function loadBangGiaLoaiKH(_idMatHang) {
        $scope.gridOptions_GiaLoaiKH = {
            sortable: true,
            height: function () {
                let heightGrid = $(window).height();
                return heightGrid - 130;
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
            columns: listColumnsgrid_giaTheoLoaiKH()
        };
        hangHoaDataService.getBangGia(_idMatHang).then(function (result) {
            $scope.gridDataGiaLoaiKH = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                            tenLoaiKhachHang: {
                                type: "string",
                                editable: false
                            },
                            giaBanBuon: {
                                type: "number",
                                editable: true,
                                validation: {
                                    min: 0
                                },
                                nullable: false
                            },
                            giaBanLe: {
                                type: "number",
                                editable: true,
                                validation: {
                                    min: 0
                                },
                                nullable: false
                            },
                            ghiChu: {
                                type: "string",
                                editable: true
                            }
                        }
                    }
                },
                pageSize: 20
            };
        });
    }

    function openFormDetail_bangGiaLoaiKH(_idMatHang) {
        if (_idMatHang > 0) {
            $scope.formBangGia.center().maximize().open();

            initbanggialoaikhachhang(_idMatHang);
        }
    }


    function loadBangLoiNhuan(_idMatHang) {
        $scope.gridOptionLoiNhuan = {
            sortable: true,
            height: function () {
                let heightGrid = $(window).height();
                return heightGrid - 130;
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
                e.model.iD = e.model.id;
                console.log(e);
                //let obj = {
                //    iD: 0,
                //    iD_HangHoa: id_MatHang,
                //    iD_NhomTaiKhoan: $scope.nhomnhanvienSelected,
                //    soLuongToiThieu: 0,
                //    soLuongToiDa: 0,
                //    tyLeHoaHong: 0,
                //    tienHoaHong: 0
                //}
                hangHoaDataService.setBangLoiNhuan(e.model).then(function (result) {
                    if (result.flag) {
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                    }
                    else
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');

                });
            },
            pageable: $scope.lang == 'vi-vn' ? pageableShort_vi : pageableShort_en,
            columns: listColumnsgrid_LoiNhuan()
        };
        hangHoaDataService.getBangLoiNhuan(_idMatHang).then(function (result) {
            $scope.gridDataLoiNhuan = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                            iD: {
                                type: "number",
                                editable: true,
                            },
                            tenNhom: {
                                type: "string",
                                editable: false
                            },
                            iD_NhomTaiKhoan: {
                                type: "number",
                                editable: false
                            },
                            soLuongToiThieu: {
                                type: "number",
                                editable: true,
                                validation: {
                                    min: 0
                                },
                                nullable: false
                            },
                            soLuongToiDa: {
                                type: "number",
                                editable: true,
                                validation: {
                                    min: 0
                                },
                                nullable: false
                            },
                            tyLeHoaHong: {
                                type: "number",
                                editable: true,
                                validation: {
                                    min: 0
                                },
                                nullable: false
                            },
                            tienHoaHong: {
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
        });
    }

    function openFormDetail_loinhuan(_idMatHang) {
        if (_idMatHang > 0) {
            console.log($scope.formLoiNhuan);
            $scope.formLoiNhuan.center().maximize().open();
            loadBangLoiNhuan(_idMatHang);
            ComboboxDataService.getDataTreeNhomNhanVien().then(function (result) {
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
            })
        }
    }


    function initbanggialoaikhachhang(_idMatHang) {
        $scope.disabled = true;
        loadBangGiaLoaiKH(_idMatHang);

        hangHoaDataService.getLoaiKhachHangByIdMatHang(_idMatHang).then(function (result) {
            $scope.loaikhachhangData = result.data;
            $('#loaikhachhang').data('kendoComboBox').value("");

            datacomboloaikhachhang = result.data;
            $scope.disabled = false;
        });
    }


    //event
    $scope.dongbodichvu = function () {
        hangHoaDataService.dongbodichvu().then(function (result) {
            if (result.flag) {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                loadgrid();
            }
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
        });
    }

    $scope.deleteRowLoaiKhachHang = function (e) {
        let myGrid = $('#gridGiaLoaiKH').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;

        let myCombo = $("#loaikhachhang").data("kendoComboBox");
        let item = {
            iD_LoaiKhachHang: dataItem.iDLoaiKhachHang,
            tenLoaiKhachHang: dataItem.tenLoaiKhachHang
        }
        myCombo.dataSource.add(item);
        myGrid.dataSource.remove(dataItem);
    }
    $scope.donvitinhOnChange = function () {
        $scope.donvitinhselect = this.donvitinhselect;
    }
    $scope.nhomhanghoaOnChange = function () {
        $scope.nhomhanghoaSelected = this.nhomhanghoaSelected;
    }
    $scope.nhacungcapOnChange = function () {
        $scope.nhacungcapselect = this.nhacungcapselect;
    }
    $scope.nhanhieuOnChange = function () {
        $scope.nhanhieuselect = this.nhanhieuselect;
    }
    $scope.nhomchaOnChange = function () {
        $scope.nhomchaselect = this.nhomchaselect;
    }

    $scope.luuMatHang = function () {
        if (validatethemsua()) {
            $scope.objectHang.anhDaiDien = image_url;
            $scope.objectHang.giaBuon = ($scope.objectHang.giaBuon == '' || $scope.objectHang.giaBuon == undefined) ? 0 : $scope.objectHang.giaBuon;
            $scope.objectHang.giaLe = ($scope.objectHang.giaLe == '' || $scope.objectHang.giaLe == undefined) ? 0 : $scope.objectHang.giaLe;
            $scope.objectHang.isDichVu = $scope.objectHang.isDichVu ? 1 : 0;
            $scope.objectHang.lstDichVu = $scope.gridDichVu.dataSource.data()
            //console.log($scope.objectHang);
            commonOpenLoadingText("#btn_luumathang");
            hangHoaDataService.themsuahang($scope.objectHang).then(function (result) {
                if (result.flag) {
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                    loadgrid();
                    $scope.formdetail.center().close();
                }
                else
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');

                commonCloseLoadingText("#btn_luumathang");
            });
        }
    }
    $scope.huyLuuMatHang = function () {
        $scope.formdetail.center().close();
    }

    $scope.luuNhomMatHang = function () {
        if (validatethemsuanhom()) {

            commonOpenLoadingText("#btn_luunhommathang");
            hangHoaDataService.themsuanhomhang($scope.objectNhom).then(function (result) {
                if (result.flag) {
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                    loadtreeView();
                    $scope.formnhom.center().close();

                    commonCloseLoadingText("#btn_luunhommathang");

                    loadnhomhang();
                }
                else
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
            });
        }
    }
    $scope.huyLuuNhomMatHang = function () {
        $scope.formnhom.center().close();
    }

    $scope.addnhom = function () {
        openFormNhom(idNhom, 0, '', '');
    }
    $scope.editnhom = function () {
        let tree = $("#treeview").data("kendoTreeView");
        let selectedNode = tree.select();
        let dataItem = tree.dataItem(selectedNode);
        openFormNhom(dataItem.iD_Parent, dataItem.id, dataItem.tenMatHang, dataItem.anhDaiDien);
    }
    $scope.deletenhom = function () {
        if (idNhom <= 0)
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_khongthexoanhommacdinh') }, 'warning');
        else {
            openConfirm($.i18n("label_saukhixoanganhhangcacmathangchuyenlencaptren")
                , 'apDungXoaNhom', null, idNhom);
        }
    }
    $scope.apDungXoaNhom = function (data) {
        hangHoaDataService.xoanhomhang(data).then(function (result) {
            if (result.flag) {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                loadtreeView();
            }
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
        });
    }

    $scope.addHangHoa = function () {
        id_MatHang = 0;
        openFormDetail(0);
    }
    $scope.editHangHoa = function () {
        let listRowsSelected = commonGetRowSelected("#grid");
        if (validationOpenDetail(listRowsSelected)) {
            id_MatHang = listRowsSelected[0].idMatHang;
            openFormDetail(id_MatHang);
        }
    }
    $scope.xuatExcel = function () {
        commonOpenLoadingText("#btn_export");

        hangHoaDataService.exportExcel(idNhom).then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayra_loadlaitrang') }, 'warning');
            commonCloseLoadingText("#btn_export")
        });
    }
    $scope.taiFileMau = function () {
        commonOpenLoadingText("#btn_taifilemauhang");

        hangHoaDataService.taiFileMau().then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayravuilongloadlaitrang') }, 'warning');
            commonCloseLoadingText("#btn_taifilemauhang")
        });
    }
    $scope.importExcel = function () {
        $scope.formimport.center().open();
        $scope.showhidebanggiaimport = false;
    }

    $scope.openbanggialoaikhachhang = function () {
        openFormDetail_bangGiaLoaiKH(id_MatHang);
    }

    $scope.capNhatBangGiaTuExcel = function () {
        if (fileUpload == '') {
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_khongcofilemaudethuchienvuilongkiemtralai') }, 'warning');
        } else {
            commonOpenLoadingText("#btn_capnhatbanggiaimport");

            hangHoaDataService.importbanggiamathang(fileUpload).then(function (result) {
                if (result.flag)
                    if (result.data.status == 200) {
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_tacvuthuchienthanhcong') }, 'success');
                        $scope.formimport.center().close();
                        loadgrid();
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

                commonCloseLoadingText("#btn_capnhatbanggiaimport")
            });
        }
    }
    $scope.capNhatTuExcel = function () {
        if (fileUpload == '') {
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_khongcofilemaudethuchienvuilongkiemtralai') }, 'warning');
        } else {
            commonOpenLoadingText("#btn_capnhatimport");

            hangHoaDataService.importmathang(fileUpload).then(function (result) {
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
    }
    $scope.huyCapNhatExcel = function () {
        $scope.formimport.center().close();
    }

    $scope.taiFileMauCapNhatGia = function () {
        commonOpenLoadingText("#btn_taifilemaubanggiahang");

        hangHoaDataService.taiFileMauBangGia(idNhom).then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayra_loadlaitrang') }, 'warning');
            commonCloseLoadingText("#btn_taifilemaubanggiahang")
        });
    }
    $scope.importExcelGiaHang = function () {
        $scope.formimport.center().open();
        $scope.showhidebanggiaimport = true;
    }
    $scope.deleteHangHoa = function () {
        let arr = $("#grid").data("kendoGrid").selectedKeyNames();
        if (arr.length <= 0)
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_chuachonmathangdethuchien') }, 'warning');
        else {
            let data = [];
            for (let i = 0; i < arr.length; i++) {
                data.push(parseInt(arr[i]));
            }
            openConfirm($.i18n("label_bancochacchanmuonxoakhong"), 'apDungXoaHang', null, data);
        }
    }
    $scope.apDungXoaHang = function (data) {
        hangHoaDataService.xoahang(data).then(function (result) {
            if (result.flag) {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                loadgrid();
            }
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
        });
    }

    $scope.openformbanggialoaikhachhang = function (e) {
        let myGrid = $('#grid').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;
        id_MatHang = dataItem.idMatHang;
        openFormDetail_bangGiaLoaiKH(id_MatHang);
    }

    $scope.openformloinhuan = function (e) {
        let myGrid = $('#grid').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;
        id_MatHang = dataItem.idMatHang;
        openFormDetail_loinhuan(id_MatHang);
    }

    $scope.hoanTat = function () {
        commonOpenLoadingText("#btn_hoanTat");

        let myGridAdd = $("#gridGiaLoaiKH").data("kendoGrid");
        let data = myGridAdd.dataSource.data();
        hangHoaDataService.hoantatGiaTheoLoaiKH(data).then(function (result) {
            if (result.flag) {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                $scope.formBangGia.center().close();
            }
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');

            commonCloseLoadingText("#btn_hoanTat");
        });
    }
    $scope.boSung = function () {
        let myGridAdd = $("#gridGiaLoaiKH").data("kendoGrid");

        if (datacomboloaikhachhang.length > 0) {
            angular.forEach(datacomboloaikhachhang, function (ct) {
                let obj = {
                    id: 0,
                    idMatHang: id_MatHang,
                    idLoaiKhachHang: ct.iD_LoaiKhachHang,
                    tenLoaiKhachHang: ct.tenLoaiKhachHang,
                    giaBanBuon: 0,
                    giaBanLe: 0,
                    ghiChu: ''
                }
                myGridAdd.dataSource.add(obj);
            });
            datacomboloaikhachhang = [];
            $scope.loaikhachhangData = [];
        } else {
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n("label_khongcoloaikhachhangdethem") }, "warning");
        }

        $('#loaikhachhang').data('kendoComboBox').value("");
        idloaikhachhang = -1;
    }
    $scope.themDong = function () {
        if (idloaikhachhang <= 0) {
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_chuachonloaikhachhang') }, 'warning');
        } else {
            let myGridAdd = $("#gridGiaLoaiKH").data("kendoGrid");
            let data = myGridAdd.dataSource.data();

            let obj = {
                id: 0,
                idMatHang: id_MatHang,
                idLoaiKhachHang: $scope.loaikhachhangselect.iD_LoaiKhachHang,
                tenLoaiKhachHang: $scope.loaikhachhangselect.tenLoaiKhachHang,
                giaBanBuon: 0,
                giaBanLe: 0,
                ghiChu: ''
            }
            myGridAdd.dataSource.add(obj);

            let myCombo = $("#loaikhachhang").data("kendoComboBox");
            let dataCombo = myCombo.dataSource.data();
            angular.forEach(dataCombo, function (item) {
                if (item.iD_LoaiKhachHang == idloaikhachhang) {
                    myCombo.dataSource.remove(item);
                }
            });
            $('#loaikhachhang').data('kendoComboBox').value("");

            datacomboloaikhachhang = datacomboloaikhachhang.filter((item) => { return (item.iD_LoaiKhachHang != idloaikhachhang) })

            idloaikhachhang = -1;
        }
    }

    $scope.themDongLoiNhuan = function () {
        commonOpenLoadingText("#btn_themloinhuan");
        if ($scope.nhomnhanvienSelected == undefined || $scope.nhomnhanvienSelected == null) {
            $scope.nhomnhanvienSelected = -1;
        };
        let myGridAdd = $("#gridLoiNhuan").data("kendoGrid");
        let data = myGridAdd.dataSource.data();
        let existed = false
        //$.each(data, function (index, item) {
        //    if (item.iD_NhomTaiKhoan == $scope.nhomnhanvienSelected) {
        //        Notification({ title: $.i18n('label_thongbao'), message: "Cấu hình cho nhóm tài khoản đã tồn tại" }, 'warning');
        //        existed = true;
        //    }
        //})
        if (!existed) {
            let obj = {
                iD: 0,
                iD_HangHoa: id_MatHang,
                iD_NhomTaiKhoan: $scope.nhomnhanvienSelected,
                soLuongToiThieu: 0,
                soLuongToiDa: 0,
                tyLeHoaHong: 0,
                tienHoaHong: 0
            }
            hangHoaDataService.setBangLoiNhuan(obj).then(function (result) {
                if (result.flag) {
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                    loadBangLoiNhuan(id_MatHang);
                    $('#nhomnhanvien').data('kendoDropDownTree').value("");

                }
                else
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');

                commonCloseLoadingText("#btn_themloinhuan");
            });
        }
    }

    $scope.loaikhachhangOnChange = function (kendoEvent) {
        $scope.loaikhachhangselect = this.loaikhachhangselect;
        try {
            idloaikhachhang = $scope.loaikhachhangselect.iD_LoaiKhachHang;
        } catch (ex) { }
    }

    $scope.nhomnhanvienOnChange = function () {
        $scope.nhomnhanvienSelected = this.nhomnhanvienSelected;

    };

    init();

    $("#grid").on("dblclick", "tr[role='row']", function () {
        let grid = $("#grid").data("kendoGrid");
        let row = grid.table.find("[data-uid=" + $(this).attr("data-uid") + "]");

        grid.clearSelection();
        grid.select(row);

        let selectedItem = grid.dataItem(grid.select());
        id_MatHang = selectedItem.idMatHang;
        openFormDetail(id_MatHang);
    })

    $scope.printLabel = function (mahang, tenhang, giaban) {
        $scope.labelforprint = mahang;
        $scope.nameforprint = tenhang;
        $scope.priceforprint = kendo.toString(giaban, 'N0');
        printer.printFromScope("/app/components/hanghoa/inTemHang.html", $scope);
    }

    $scope.addRowDichVu = function () {
        $scope.gridDichVu.addRow();
    }

    $scope.deleteRowDichVu = function (uid) {
        var dataRow = $scope.gridDichVu.dataSource.getByUid(uid);
        $scope.gridDichVu.dataSource.remove(dataRow);
    }

    $scope.deleteRowLoiNhuan = function (id) {
        let obj = {
            iD: id,
            TrangThai: 0
        }
        hangHoaDataService.setTrangThaiBangLoiNhuan(obj).then(function (result) {
            if (result.flag) {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                loadBangLoiNhuan(id_MatHang);
            }
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
        });
    }
})