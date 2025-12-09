angular.module('app').controller('nhanVienController', function ($scope, $location, $state, $stateParams, $timeout, Notification, ComboboxDataService, nhanVienDataService) {
    CreateSiteMap();

    let param_tructuyen = -1;
    let idNhom = 0;

    function init() {
        getquyen();

        initparam();
        initcombo();

        inittreeview();
    }

    function initparam() {
        param_tructuyen = ($stateParams.tructuyen == undefined) ? -1 : $stateParams.tructuyen;
    }

    function initcombo() {
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
                            children: "childs"
                        }
                    }
                })
            };
        })
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
            dataTextField: "tenHienThi_NhanVien",
            dataValueField: "iD_Nhom",
            select: onSelectNhom,
        });

        loadtreeView();
    }
    function loadtreeView() {
        nhanVienDataService.getListNhomNhanVien().then(function (result) {
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
        idNhom = tree.dataItem(selectedNode).iD_Nhom;
        loadgrid();
    }
    function onSelectNhom(e) {
        idNhom = $("#treeview").getKendoTreeView().dataItem(e.node).iD_Nhom;
        loadgrid();
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
            field: "tenDangNhap", title: $.i18n('header_tendangnhap'),
            template: function (dataItem) {
                return "<a href='' class='color-primary' ng-click='openDetailFromGrid(" + kendo.htmlEncode(dataItem.idnv) + ")'>" + kendo.htmlEncode(dataItem.tenDangNhap) + "</a>";
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px"
        });
        dataList.push({ field: "tenDayDu", title: $.i18n('header_tendaydu'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "175px" });
        dataList.push({ field: "dienThoai", title: $.i18n('header_sodienthoai'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px" });
        dataList.push({
            field: "trangThaiTrucTuyen", title: $.i18n('header_trangthaitructuyen'),
            template: function (dataItem) {
                if (dataItem.trangThaiTrucTuyen == null)
                    return '<span class="color-dark">Ngoại tuyến</span>'
                else {
                    if (dataItem.trucTuyen == 1) {
                        return '<span class="color-primary">' + dataItem.trangThaiTrucTuyen + '</span>'
                    } else if (dataItem.trucTuyen == 2) {
                        return '<span class="color-danger">' + dataItem.trangThaiTrucTuyen + '</span>'
                    } else {
                        return '<span class="color-dark">' + dataItem.trangThaiTrucTuyen + '</span>'
                    }
                }

            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "toaDo", title: $.i18n('header_toado'), attributes: { style: "text-align: center" },
            template: function (dataItem) {
                if (dataItem.kinhDo == 0 && dataItem.viDo == 0)
                    return '<button ng-click="xemvitrichuacotoado()" class="btn btn-link btn-menubar" title ="' + $.i18n('label_vitrinhanvien') + '" ><i class="fas fa-map-marker-alt fas-sm color-gray"></i></button>';
                else
                    return '<button ng-click="xemvitri(' + kendo.htmlEncode(dataItem.viDo) + ',' + kendo.htmlEncode(dataItem.kinhDo) + ')" class="btn btn-link btn-menubar" title ="' + $.i18n('label_vitrinhanvien') + '" ><i class="fas fa-map-marker-alt fas-sm color-infor"></i></button>';
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: false, width: "90px"
        });
        dataList.push({
            field: "thoiGianCapNhat", title: $.i18n('header_thoigiancapnhat'),
            attributes: { style: "text-align: left" },
            template: function (dataItem) {
                if (dataItem.thoiGianCapNhat.getFullYear() > 1900)
                    return kendo.htmlEncode(kendo.toString(dataItem.thoiGianCapNhat, formatDateTime));
                else
                    return '';
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: false, width: "150px"
        });
        dataList.push({ field: "tinhTrangPin", title: $.i18n('header_tinhtrangpin'), attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px" });
        dataList.push({ field: "phienBan", title: $.i18n('header_phienban'), attributes: { style: "text-align: center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px" });
        dataList.push({ field: "tenNhom", title: $.i18n('header_tennhom'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px" });
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
            filterable: false, width: "150px"
        });
        dataList.push({
            title: $.i18n('header_thaotac'), attributes: { style: "text-align: center" },
            template: function (dataItem) {
                return '<button ng-click="resetimei(' + kendo.htmlEncode(dataItem.idnv) + ')" class="btn btn-outline-info btn-menubar" title ="Reset Imei" ><i class="fas fa-sync-alt"></i> ' + $.i18n('button_resetimei') + '</button>';
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: false, width: "150px"
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

        nhanVienDataService.getlist(idNhom).then(function (result) {
            let data = result.data;
            if (param_tructuyen > -1) {
                let arr = result.data;
                let small_arr = arr.filter((item) => {
                    return (item.trucTuyen == param_tructuyen)
                })

                data = small_arr;
            }

            $scope.gridData = {
                data: data,
                schema: {
                    model: {
                        id: 'idnv',
                        fields: {
                            idnv: {
                                type: "number"
                            },
                            thoiGianCapNhat: {
                                type: "date"
                            }
                        }
                    }
                },
                pageSize: 20
            };
            kendo.ui.progress($("#grid"), false);

            showtooltip();
        });
    }

    function showtooltip() {
        $("#grid").kendoTooltip({
            filter: "td:nth-child(12)",
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

    function loadcombobox() {
        ComboboxDataService.getDataNhomNhanVien().then(function (result) {
            let arr = result.data;
            let small_arr = arr.filter((item) => {
                return (item.iD_Nhom > 0)
            })

            $scope.nhomchaData = small_arr;

            $timeout(function () {
                if ($scope.objectNhom.iD_PARENT > 0)
                    $("#nhomcha").data("kendoComboBox").value($scope.objectNhom.iD_PARENT);
                else
                    $("#nhomcha").data("kendoComboBox").value("");
            }, 10);
        });
    }

    function openFormDetail(id) {
        console.log(id);
        $state.go('editnhanvien', { idnhanvien: id, idnhom: idNhom });
    }
    function validationOpenDetail(listRowsSelected) {
        let flag = true;
        let msg = "";
        if (listRowsSelected.length > 1) {
            msg = $,i18n("label_canchonmotnhanviendethuchien");
            flag = false;
        } else if (listRowsSelected.length == 0) {
            flag = false;
            msg = $.i18n("label_chuachonnhanviendethuchien");
        }
        if (!flag) {
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n(msg) }, "error");
        }
        return flag;
    }
    function openFormResetPass(id) {
        if (id > 0) {
            $scope.objectpass = {
                idnhanvien: id,
                pass: '',
                repass: ''
            }
            $scope.formresetpass.center().open();
        } else {
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_loinhanvienvuilongchonlai') }, "error");
        }

    }
    function openFormNhom(idparent, id, code, name, sitecode) {
        if (id < 0)
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_khongthechinhsuanhommacdinh') }, 'warning');
        else {
            $scope.formnhom.center().open();
            $scope.objectNhom = {
                iD_Nhom: id,
                iD_PARENT: idparent,
                maNhom: code,
                tenNhom: name,
                siteCode: sitecode
            }

            loadcombobox();
        }

    }
    function validatethemsuanhom() {
        let flag = true;
        let msg = '';

        if ($scope.nhomchaselect != undefined)
            $scope.objectNhom.iD_PARENT = ($scope.nhomchaselect.iD_Nhom < 0) ? 0 : $scope.nhomchaselect.iD_Nhom;

        if ($scope.objectNhom.maNhom == '' || $scope.objectNhom.maNhom == undefined) {
            flag = false;
            msg = $.i18n('label_manhomkhongduocdetrong');
            $("#maNhom").focus();
        }

        if (flag && ($scope.objectNhom.tenNhom == '' || $scope.objectNhom.tenNhom == undefined)) {
            flag = false;
            msg = $.i18n('label_tennhomkhongduocdetrong');
            $("#tenNhom").focus();
        }

        if (flag && ($scope.objectNhom.iD_PARENT == $scope.objectNhom.iD_Nhom) && $scope.objectNhom.iD_Nhom > 0) {
            flag = false;
            msg = $.i18n('label_khongthechonnhomchalachinhno');
        }

        if (!flag)
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n(msg) }, 'warning');

        return flag;
    }
    function validateresetpass() {
        let flag = true;
        let msg = '';

        if ($scope.objectpass.pass == '' || $scope.objectpass.pass == undefined) {
            flag = false;
            msg = $.i18n('label_matkhaukhongduocdetrong');
            $("#pass").focus();
        }

        if (flag && ($scope.objectpass.repass == '' || $scope.objectpass.repass == undefined)) {
            flag = false;
            msg = $.i18n('label_xacnhanmatkhaukhongduocdetrong');
            $("#repass").focus();
        }

        if (flag && $scope.objectpass.pass.length < 8) {
            flag = false;
            msg = $.i18n('label_matkhaucododaiitnhat8kytu');
            $("#pass").focus();
        }

        if (flag && $scope.objectpass.pass != $scope.objectpass.repass) {
            flag = false;
            msg = $.i18n('label_xacnhanmatkhaukhongkhop');
            $("#repass").focus();
        }

        if (!flag)
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n(msg) }, 'warning');

        return flag;
    }

    function openFormNhanVienKhongDangNhap(_iD_NhanVien, _ID_Nhom, _tenDayDu, _dienThoai) {
        $scope.formnhanvien.center().open();
        $scope.objectNhanVien = {
            IDNV: _iD_NhanVien,
            ID_Nhom: (_ID_Nhom > 0) ? _ID_Nhom : 0,
            TenDayDu: _tenDayDu,
            DienThoai: _dienThoai
        }

        if ($scope.objectNhanVien.ID_Nhom > 0)
            $scope.nhomnhanvienSelected = $scope.objectNhanVien.ID_Nhom;
        else
            $scope.nhomnhanvienSelected = undefined;

        if ($scope.$root.$$phase != '$apply' && $scope.$root.$$phase != '$digest') {
            $scope.$apply();
        }
    }
    function validatethemsuanhanvien() {
        let flag = true;
        let msg = '';

        if (flag && ($scope.objectNhanVien.TenDayDu == '' || $scope.objectNhanVien.TenDayDu == undefined)) {
            flag = false;
            msg = $.i18n('label_tendaydukhongduocdetrong');
            $("#tenDayDu").focus();
        }
        $scope.objectNhanVien.ID_Nhom = $scope.nhomnhanvienSelected;
        if (flag && ($scope.nhomnhanvienSelected == undefined || $scope.objectNhanVien.ID_Nhom <= 0)) {
            flag = false;
            msg = $.i18n('label_nhomnhanvienkhongduocdetrong');
            $("#nhomnhanvien").focus();
        }

        if (!flag)
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n(msg) }, 'warning');

        return flag;
    }

    let fileUpload = '';
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
            fileUpload = result;
        }).fail(function (a, b, c) {
            fileUpload = '';
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
    $scope.nhomnhanvienOnChange = function () {
        $scope.nhomnhanvienSelected = this.nhomnhanvienSelected;
    }
    $scope.nhomchaOnChange = function () {
        $scope.nhomchaselect = this.nhomchaselect;
    }

    $scope.addnhom = function () {
        openFormNhom(idNhom, 0, '', '','');
    }
    $scope.editnhom = function () {
        let tree = $("#treeview").data("kendoTreeView");
        let selectedNode = tree.select();
        let dataItem = tree.dataItem(selectedNode);
        openFormNhom(dataItem.iD_PARENT, dataItem.iD_Nhom, dataItem.maNhom, dataItem.tenNhom, dataItem.siteCode);
    }
    $scope.deletenhom = function () {
        if (idNhom <= 0)
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_khongthexoanhommacdinh') }, 'warning');
        else {
            openConfirm($.i18n("label_bancochacchanxoanhomnhanvien")
                , 'apDungXoaNhom', null, idNhom);
        }
    }
    $scope.apDungXoaNhom = function (data) {
        nhanVienDataService.delnhomnhanvien(data).then(function (result) {
            if (result.flag) {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                loadtreeView();
            }
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
        });
    }

    $scope.addNhanVien = function () {
        openFormDetail(0);
    }
    $scope.editNhanVien = function () {
        let listRowsSelected = commonGetRowSelected("#grid");
        if (validationOpenDetail(listRowsSelected)) {
            if (listRowsSelected[0].loai == 3)
                openFormNhanVienKhongDangNhap(listRowsSelected[0].idnv, listRowsSelected[0].iD_Nhom, listRowsSelected[0].tenDayDu, listRowsSelected[0].dienThoai)
            else
                openFormDetail(listRowsSelected[0].idnv);
        }
    }
    $scope.deleteNhanVien = function () {
        let arr = $("#grid").data("kendoGrid").selectedKeyNames();
        if (arr.length <= 0)
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_chuachonnhanviendethuchien') }, 'warning');
        else {
            let data = [];
            for (let i = 0; i < arr.length; i++) {
                data.push(parseInt(arr[i]));
            }
            openConfirm($.i18n("label_bancochacchanxoanhomnhanvien"), 'apDungXoaNhanVien', null, data);
        }
    }
    $scope.apDungXoaNhanVien = function (data) {
        nhanVienDataService.del(data).then(function (result) {
            if (result.flag) {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                loadgrid();
            }
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
        });
    }

    $scope.addNhanVienKhongDangNhap = function () {
        openFormNhanVienKhongDangNhap(0,idNhom,'','')
    }
    $scope.luuNhanVien = function () {
        if (validatethemsuanhanvien()) {
            nhanVienDataService.saveinsertnhanvienkhongdangnhap($scope.objectNhanVien).then(function (result) {
                if (result.flag) {
                    $scope.formnhanvien.center().close();
                    loadgrid();
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                }
                else
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
            })
        }
    }
    $scope.huyLuuNhanVien = function () {
        $scope.formnhanvien.center().close();
    }

    $scope.xuatExcel = function () {
        commonOpenLoadingText("#btn_xuatexcelnhanvien");

        var dataSource = $("#grid").data("kendoGrid").dataSource;
        var filter = dataSource.filter();
        var filterSite = { idNhom: idNhom }
        if (filter !== undefined) {
            var list_filters = filter.filters
            list_filters.forEach(function (item) {
                filterSite[item.field] = item.value;
            });
        }

        nhanVienDataService.exportExcel(filterSite).then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayra_loadlaitrang') }, 'warning');

            commonCloseLoadingText("#btn_xuatexcelnhanvien");
        });
    }
    $scope.taiFileMau = function () {
        commonOpenLoadingText("#btn_taifilemaunhanvien");

        nhanVienDataService.taiFileMau().then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayra_loadlaitrang') }, 'warning');
            commonCloseLoadingText("#btn_taifilemaunhanvien")
        });
    }
    $scope.importExcel = function () {
        $scope.formimport.center().open();
    }
    $scope.capNhatTuExcel = function () {
        commonOpenLoadingText("#btn_capnhatimport");

        nhanVienDataService.importnhanvien(fileUpload).then(function (result) {
            if (result.flag)
                if (result.data.status == 200) {
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_tacvuthuchienthanhcong') }, 'success');
                    $scope.formimport.center().close();
                    $scope.loadgrid();
                }
                else if (result.data.status == 201)
                    commonDownFile(result.data);
                else if (result.data.status == 204)
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_filemaukhongdungdinhdanghoackhongtontaibanghi') }, 'warning');
                else if (result.data.status == 411)
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_soluongbanghikhongthevuotqua2000dong') }, 'warning');
                else
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloisayravuilongkiemtralaifilecapnhat') }, 'warning');
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayravuilongloadlaitrang') }, 'warning');

            commonCloseLoadingText("#btn_capnhatimport")
        });
    }
    $scope.huyCapNhatExcel = function () {
        $scope.formimport.center().close();
    }

    $scope.datLaiMatKhau = function () {
        let listRowsSelected = commonGetRowSelected("#grid");
        if (validationOpenDetail(listRowsSelected)) {
            openFormResetPass(listRowsSelected[0].idnv);
        }
    }

    $scope.luuMatKhau = function () {
        if (validateresetpass()) {
            if ($scope.objectpass.idnhanvien > 0) {
                commonOpenLoadingText("#btn_luumatkhau");
                nhanVienDataService.resetpass($scope.objectpass.idnhanvien, $scope.objectpass.pass).then(function (result) {
                    if (result.flag) {
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                        $scope.formresetpass.center().close();
                    }
                    else
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');

                    commonCloseLoadingText("#btn_luumatkhau");
                });
            }
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_loinhanvienvuilongchonlai') }, 'warning');
        }
    }
    $scope.huyLuuMatKhau = function () {
        $scope.formresetpass.center().close();
    }
    $scope.luuNhomNhanVien = function () {
        if (validatethemsuanhom()) {
            if ($scope.objectNhom.iD_Nhom > 0) {
                commonOpenLoadingText("#btn_luunhomnhanvien");
                nhanVienDataService.saveeditnhomnhanvien($scope.objectNhom).then(function (result) {
                    if (result.flag) {
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                        loadtreeView();
                        $scope.formnhom.center().close();
                    }
                    else
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');

                    commonCloseLoadingText("#btn_luunhomnhanvien");
                });
            }
            else {
                commonOpenLoadingText("#btn_luunhomnhanvien");
                nhanVienDataService.saveinsertnhomnhanvien($scope.objectNhom).then(function (result) {
                    if (result.flag) {
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                        loadtreeView();
                        $scope.formnhom.center().close();
                    }
                    else
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');

                    commonCloseLoadingText("#btn_luunhomnhanvien");
                });
            }
        }
    }
    $scope.huyLuuNhomNhanVien = function () {
        $scope.formnhom.center().close();
    }

    $scope.openDetailFromGrid = function (idnv) {
        openFormDetail(idnv);
    }
    $scope.xemvitrichuacotoado = function () {
        Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_nhanvienkhongcothongtinvitri') }, 'warning');
    }
    $scope.xemvitri = function (vido, kinhdo) {
        let url = 'https://www.google.com/maps/dir/' + vido.toString() + ',' + kinhdo.toString();
        window.open(url, '_blank');
    }
    $scope.resetimei = function (id) {
        nhanVienDataService.resetimei(id).then(function (result) {
            if (result.flag)
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
        });
    }
    $scope.chuyenKhachHang = function () {
        $state.go('chuyenquyen', { idnhanvien: 0 });
    }

    init();

    $("#grid").on("dblclick", "tr[role='row']", function () {
        let grid = $("#grid").data("kendoGrid");
        let row = grid.table.find("[data-uid=" + $(this).attr("data-uid") + "]");

        grid.clearSelection();
        grid.select(row);

        let selectedItem = grid.dataItem(grid.select());
        if (selectedItem.loai == 3)
            openFormNhanVienKhongDangNhap(selectedItem.idnv, selectedItem.iD_Nhom, selectedItem.tenDayDu, selectedItem.dienThoai)
        else
            openFormDetail(selectedItem.idnv);
    })

})