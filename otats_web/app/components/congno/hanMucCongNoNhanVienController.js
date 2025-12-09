angular.module('app').controller('hanMucCongNoNhanVienController', function ($rootScope, $scope, $state, Notification, hanMucDataService) {
    CreateSiteMap();

    $scope.idNhom = 0;

    function init() {
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

        $("#treeview").kendoTreeView({
            dataSource: dataSource,
            dataTextField: "tenHienThi_NhanVien",
            dataValueField: "iD_Nhom",
            select: onSelectNhom,
        });

        loadtreeView();
    }
    function loadtreeView() {
        hanMucDataService.getListNhomNhanVien().then(function (result) {
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
        var dataList = [];

        dataList.push({
            field: "chiTiet", title: $.i18n("header_tacvu"),
            template: '<button ng-click="openformsetnguong()" class="btn btn-link btn-menubar" title ="' + $.i18n("label_datnguongcongno") + '" ><i class="fas fa-plus-circle fas-sm color-infor"></i></button> ',
            attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });

        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({
            field: "tenDangNhap", title: $.i18n("header_tendangnhap"),
            template: function (dataItem) {
                return "<a href='' class='color-primary' ng-click='openDetailFromGrid(" + kendo.htmlEncode(dataItem.iD_NhanVien) + ")'>" + kendo.htmlEncode(dataItem.tenDangNhap) + "</a>";
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px"
        });
        dataList.push({ field: "tenNhanVien", title: $.i18n("header_tendaydu"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "175px" });
        dataList.push({
            field: "congNoChoPhep", title: $.i18n("header_congnochophep"),
            template: function (dataItem) {
                if (dataItem.congNoChoPhep == null) {
                    return kendo.htmlEncode('0');
                } else {
                    return kendo.toString(dataItem.congNoChoPhep, $rootScope.UserInfo.dinhDangSo);
                }
            },
            attributes: { style: "text-align: right" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({ field: "dienThoai", title: $.i18n("header_sodienthoai"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px" });
        dataList.push({ field: "tenNhom", title: $.i18n("header_nhom"), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px" });

        return dataList;
    }
    function loadgrid() {
        kendo.ui.progress($("#grid"), true);
        $scope.gridOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height());
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

        hanMucDataService.getdatanhanvien($scope.idNhom).then(function (result) {
            $scope.gridData = {
                data: result.data,
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
        });
    }

    //event
    $scope.openformsetnguong = function (e) {
        let myGrid = $('#grid').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;

        $scope.idnhanvien = dataItem.iD_NhanVien;
        $scope.nhanvien = dataItem.tenNhanVien == undefined ? '' : dataItem.tenNhanVien;
        $scope.nguong = dataItem.congNoChoPhep == null ? 0 : dataItem.congNoChoPhep;

        $scope.formsetnguong.center().open();
    }
    $scope.setnguong = function () {
        let flag = true;

        if ($scope.nguong <= 0) {
            flag = false;
            Notification({ title: $.i18n("label_thongbao"), message: $.i18n("label_nguongcongnokhongthenhohonhoacbangkhong") }, 'warning');
        }

        if (flag) {
            hanMucDataService.setnguongnhanvien($scope.idnhanvien, $scope.nguong).then(function (result) {
                if (result.flag) {
                    $scope.formsetnguong.close();
                    Notification({ title: $.i18n("label_thongbao"), message: $.i18n(result.message) }, 'success');
                    loadtreeView();
                }
                else
                    Notification({ title: $.i18n("label_thongbao"), message: $.i18n(result.message) }, 'warning');
            });
        }
    }
    $scope.huysetnguong = function () {
        $scope.formsetnguong.close();
    }

    $scope.openDetailFromGrid = function (idnv) {
        $state.go('editnhanvien', { idnhanvien: idnv });
    }
    $scope.xemvitrichuacotoado = function () {
        Notification({ title: $.i18n("label_thongbao"), message: $.i18n("label_nhanvienkhongcothongtinvitri") }, 'warning');
    }
    $scope.xemvitri = function (vido, kinhdo) {
        let url = 'https://www.google.com/maps/dir/' + vido.toString() + ',' + kinhdo.toString();
        window.open(url, '_blank');
    }

    init();

})