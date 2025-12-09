angular.module('app').controller('chiTieuKPIController', function ($location, $rootScope, $scope, $timeout, Notification, ComboboxDataService, congViecDataService) {
    CreateSiteMap();

    $scope.idNhom = 0;

    function init() {
        inittreeview();
        getquyen();
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
            dataTextField: "tenNhom",
            dataValueField: "iD_Nhom",
            select: onSelectNhom,
        });

        loadtreeView();
    }
    function loadtreeView() {
        congViecDataService.getListNhomNhanVien().then(function (result) {
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
        $scope.idNhom = tree.dataItem(selectedNode).iD_Nhom;
        loadgrid();
    }
    function onSelectNhom(e) {
        $scope.idNhom = $("#treeview").getKendoTreeView().dataItem(e.node).iD_Nhom;
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
            field: "apDung_TuNgay", title: $.i18n("header_apdung"),
            template: function (dataItem) {
                return kendo.htmlEncode(kendo.toString(dataItem.apDung_TuNgay, formatMonth));
            },
            attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        });
        dataList.push({
            field: "tenDangNhap", title: $.i18n("header_tendangnhap"),
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "tenNhanVien", title: $.i18n("header_tennhanvien"),
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "doanhSo", title: $.i18n("header_doanhso"),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), attributes: { class: "text-right" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px"
        });
        dataList.push({
            field: "soDonHang", title: $.i18n("header_sodonhang"),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), attributes: { class: "text-right" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px"
        });
        dataList.push({
            field: "ngayCong", title: $.i18n("header_ngaycong"),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), attributes: { class: "text-right" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px"
        });
        dataList.push({
            field: "luotViengTham", title: $.i18n("header_luotviengtham"),
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo), attributes: { class: "text-right" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px"
        });
        dataList.push({
            field: "tenTrangThai", title: $.i18n("header_trangthai"),
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "ngayTaoKPI", title: $.i18n("header_ngaytao"),
            template: function (dataItem) {
                return kendo.htmlEncode(kendo.toString(dataItem.ngayTaoKPI, formatDateTime));
            },
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
                return heightGrid - 50;
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

        congViecDataService.getlistapibyidnhom($scope.idNhom).then(function (result) {
            $scope.gridData = {
                data: result.data,
                schema: {
                    model: {
                        id: 'iD_ChiTieuKPI',
                        fields: {
                            apDung_TuNgay: {
                                type: "date"
                            },
                            ngayTaoKPI: {
                                type: "date"
                            }
                        }
                    }
                },
                pageSize: 20
            };
            kendo.ui.progress($("#grid"), false);
        });
    }

    function validationOpenDetail(listRowsSelected) {
        let flag = true;
        let msg = "";
        if (listRowsSelected.length > 1) {
            msg = $.i18n('label_canchonmotchitieudethuchien');
            flag = false;
        } else if (listRowsSelected.length == 0) {
            flag = false;
            msg = $.i18n('label_phaichonmotchitieudethuchiensua');
        }
        if (!flag) {
            Notification({ title: $.i18n("label_thongbao"), message: $.i18n(msg) }, "error");
        }
        return flag;
    }
    function openFormDetail(id) {
        $scope.idkpi = id;
        $scope.formdetail.center().open();
        if (id > 0)
            loadeditkpiform();
        else
            loadaddkpiform();
    }
    function loadeditkpiform() {
        let grid = $("#grid").data("kendoGrid");
        let selectedItem = grid.dataItem(grid.select());

        $scope.object = {
            iD_ChiTieuKPI: selectedItem.iD_ChiTieuKPI,
            iD_NhanVien: selectedItem.iD_NhanVien,
            iDQLLH: $rootScope.UserInfo.iD_QLLH,
            doanhSo: selectedItem.doanhSo,
            soDonHang: selectedItem.soDonHang,
            luotViengTham: selectedItem.luotViengTham,
            ngayCong: selectedItem.ngayCong,
            trangThai: selectedItem.trangThai,
            apDung_TuNgay: kendo.toString(selectedItem.apDung_TuNgay, formatDateTimeFilter)
        };
        $scope.dateObject = selectedItem.apDung_TuNgay;

        loadnhanvien();

    }
    function loadaddkpiform() {
        let now = new Date();
        $scope.dateObject = now;
        $scope.object = {
            iD_ChiTieuKPI: 0,
            iD_NhanVien: 0,
            iD_Nhom: 0,
            iDQLLH: $rootScope.UserInfo.iD_QLLH,
            doanhSo: 0,
            soDonHang: 0,
            luotViengTham: 0,
            ngayCong: 0,
            trangThai: 0,
            apDung_TuNgay: kendo.toString(now, formatDateTimeFilter)
        };
        
        if ($scope.shownhom)
            loadnhomnhanvien();
        else
            loadnhanvien();
    }
    function loadnhanvien() {
        congViecDataService.getlistnhanvienbyidnhom($scope.idNhom).then(function (result) {
            let data = result.data;
            $scope.nhanvienData = data.filter(x => (x.idnv > 0));

            $timeout(function () {
                if ($scope.object.iD_NhanVien > 0)
                    $('#nhanvien').data('kendoComboBox').value($scope.object.iD_NhanVien);
                else
                    $('#nhanvien').data('kendoComboBox').value("");
            }, 100)
        });
    }
    function loadnhomnhanvien() {
        ComboboxDataService.getDataNhomNhanVien().then(function (result) {
            let data = result.data;
            $scope.listNhomNhanVien = data.filter(x => (x.iD_Nhom > 0));
        });
    }

    function validatethemsua() {
        let now = new Date();
        let flag = true;
        let msg = '';

        if ($scope.nhanvienselect != undefined)
            $scope.object.iD_NhanVien = ($scope.nhanvienselect.idnv < 0) ? 0 : $scope.nhanvienselect.idnv;
        if ($scope.selectNhomNhanVien != undefined)
            $scope.object.iD_Nhom = ($scope.selectNhomNhanVien.iD_Nhom < 0) ? 0 : $scope.selectNhomNhanVien.iD_Nhom;

        $scope.object.apDung_TuNgay = kendo.toString($scope.dateObject, formatDateTimeFilter);

        if (flag && $scope.shownhom) {
            if ($scope.object.iD_Nhom <= 0) {
                msg += $.i18n('label_khongthedetrongnhomnhanvien');
                flag = false;
            }
        } else {
            if (flag && $scope.object.iD_NhanVien <= 0) {
                flag = false;
                msg = $.i18n("label_nhanvienkhongduocdetrong");
            }
        }
        
        if (flag && ($scope.dateObject.getFullYear() < now.getFullYear() || ($scope.dateObject.getMonth() < now.getMonth() && $scope.dateObject.getFullYear() == now.getFullYear()))) {
            flag = false;
            msg = $.i18n("label_thangapdungkhongduocnhohonthanghientai");
        }

        if (!flag)
            Notification({ title: $.i18n("label_thongbao"), message:$.i18n(msg) }, 'warning');

        return flag;
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
                    text: $.i18n("button_huy"), primary: false, action: function () {
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

    //event
    $scope.nhanvienOnChange = function () {
        $scope.nhanvienselect = this.nhanvienselect;
    }
    $scope.nhomNhanVienOnChange = function (kendoEvent) {
        $scope.selectNhomNhanVien = this.selectNhomNhanVien;
    }
    $scope.monthSelectorOptions = {
        start: "year",
        depth: "year"
    };

    $scope.luukpi = function () {
        if (validatethemsua()) {
            if ($scope.shownhom) {
                commonOpenLoadingText("#btn_luukpi");
                congViecDataService.addapinhom($scope.object).then(function (result) {
                    if (result.flag) {
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                        loadgrid();
                        $scope.formdetail.center().close();

                        commonCloseLoadingText("#btn_luukpi");
                    }
                    else
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
                });
            } else {
                if ($scope.object.iD_ChiTieuKPI <= 0) {
                    commonOpenLoadingText("#btn_luukpi");
                    congViecDataService.addapi($scope.object).then(function (result) {
                        if (result.flag) {
                            Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                            loadgrid();
                            $scope.formdetail.center().close();

                            commonCloseLoadingText("#btn_luukpi");
                        }
                        else
                            Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
                    });
                } else {
                    commonOpenLoadingText("#btn_luukpi");
                    congViecDataService.editapi($scope.object).then(function (result) {
                        if (result.flag) {
                            Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                            loadgrid();
                            $scope.formdetail.center().close();

                            commonCloseLoadingText("#btn_luukpi");
                        }
                        else
                            Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
                    });
                }
            }
        }
    }
    $scope.huykpi = function () {
        $scope.formdetail.center().close();
    }

    $scope.addChiTieuKPINhom = function () {
        $scope.shownhom = true;
        openFormDetail(0);
    }
    $scope.addChiTieuKPI = function () {
        $scope.shownhom = false;
        openFormDetail(0);
    }
    $scope.editChiTieuKPI = function () {
        let listRowsSelected = commonGetRowSelected("#grid");
        if (validationOpenDetail(listRowsSelected)) {
            openFormDetail(listRowsSelected[0].iD_ChiTieuKPI);
        }
    }
    $scope.deleteChiTieuKPI = function () {
        let arr = $("#grid").data("kendoGrid").selectedKeyNames();
        if (arr.length <= 0)
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_phaichonchitieudethuchienxoa') }, 'warning');
        else {
            let data = [];
            for (let i = 0; i < arr.length; i++) {
                data.push(parseInt(arr[i]));
            }
            openConfirm($.i18n('label_bancochacchanmuonxoakhong'), 'apDungXoa', null, data);
        }
    }
    $scope.apDungXoa = function (data) {
        congViecDataService.deleteapi(data).then(function (result) {
            if (result.flag) {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                loadgrid();
            }
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
        });
    }

    init();

    $("#grid").on("dblclick", "tr[role='row']", function () {
        let grid = $("#grid").data("kendoGrid");
        let row = grid.table.find("[data-uid=" + $(this).attr("data-uid") + "]");

        grid.clearSelection();
        grid.select(row);

        let selectedItem = grid.dataItem(grid.select());

        openFormDetail(selectedItem.iD_ChiTieuKPI);
    })

})