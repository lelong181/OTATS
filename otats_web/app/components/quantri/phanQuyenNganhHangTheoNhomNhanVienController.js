angular.module('app').controller('phanQuyenNganhHangTheoNhomNhanVienController', function ($location, $scope, Notification, phanQuyenDataService, ComboboxDataService) {
    CreateSiteMap();

    $scope.idNhom = 0;

    function init() {
        initCombobox();
        inittreeview();
        getquyen();
    }
    function initCombobox() {
        phanQuyenDataService.getdatanganhhang().then(function (result) {
            let ar = result.data;

            $scope.nganhhangData = ar.filter(obj => obj.iD_DANHMUC > 0);
        });
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
        phanQuyenDataService.getListNhomNhanVien().then(function (result) {
            let ar = result.data;

            let data = ar.filter(obj => obj.iD_Nhom > 0);

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
            field: "chiTiet", title: $.i18n('header_xoa'),
            template: '<button ng-disabled="(permission.xoa <= 0)" ng-click="deleterow()" class="btn btn-link btn-menubar" title ="' + $.i18n('button_xoa') + '" ><i class="fas fa-trash fas-sm color-danger"></i></button> ',
            attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "50px"
        });

        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({ field: "tenDanhMuc", title: $.i18n('header_nganhhang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "400px" });
        
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

        phanQuyenDataService.getdatanganhhangphanquyen($scope.idNhom).then(function (result) {
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
    $scope.nganhhangOnChange = function () {
        $scope.nganhhangselect = this.nganhhangselect;
    }

    $scope.themnganhhang = function () {
        if ($scope.idNhom < 0)
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_khongthechinhsuanhommacdinh') }, 'warning');
        else {
            $scope.formsetquyen.center().open();

            $("#nganhhang").data("kendoComboBox").value("");

            let tree = $("#treeview").data("kendoTreeView");
            let selectedNode = tree.select();
            let dataItem = tree.dataItem(selectedNode);

            $scope.nhom = dataItem.tenNhom == undefined ? '' : dataItem.tenNhom;
        }
    }
    $scope.deleterow = function (e) {
        let myGrid = $('#grid').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;

        phanQuyenDataService.xoaphanquyennganhhangnhom($scope.idNhom, dataItem.iD_DANHMUC).then(function (result) {
            if (result.flag) {
                $scope.formsetquyen.close();
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                loadgrid();
            }
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
        });
    }

    $scope.setquyen = function () {
        let flag = true;

        let idnganhhang = 0;
        if ($scope.nganhhangselect != undefined)
            idnganhhang = ($scope.nganhhangselect.iD_DANHMUC < 0) ? 0 : $scope.nganhhangselect.iD_DANHMUC;

        if (idnganhhang <= 0) {
            flag = false;
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_nganhhangkhongduocphepdetrong') }, 'warning');
        }

        if (flag) {
            phanQuyenDataService.themnganhhangnhom($scope.idNhom, idnganhhang).then(function (result) {
                if (result.flag) {
                    $scope.formsetquyen.close();
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                    loadgrid();
                }
                else
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
            });
        }
    }
    $scope.huysetquyen = function () {
        $scope.formsetquyen.close();
    }

    init();

})