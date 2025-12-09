angular.module('app').controller('phanQuyenChucNangController', function ($scope, $timeout, Notification, phanQuyenDataService) {
    CreateSiteMap();

    let __idnhom = 0;

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
        __idnhom = tree.dataItem(selectedNode).iD_Nhom;
        loadgridweb();
        loadgridapp();
    }
    function onSelectNhom(e) {
        __idnhom = $("#treeview").getKendoTreeView().dataItem(e.node).iD_Nhom;
        loadgridweb();
        loadgridapp();
    }

    function templateCheckBoxGridWEB(nameField, idfield) {
        let temp = '<input type="checkbox" id="' + nameField + '#=' + idfield + '#" ng-click="changeCheckBoxWeb($event, \'' + nameField + '\')"  #= (' +
            nameField + ' == 1) ?\'checked="checked"\' : (' + nameField + '== 2)? \'disabled = true\': "" # class="chkbx k-checkbox" /> '
            + '<label class="k-checkbox-label" for="' + nameField + '#=' + idfield + '#"></label>';
        return temp;
    }
    function listColumnsgridweb() {
        let dataList = [];

        dataList.push({ field: "tenChucNang", title: $.i18n('header_tenchucnang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "200px" });
        dataList.push({
            field: "xem", title: $.i18n('header_xem'),
            headerTemplate: '<input type="checkbox" ng-model="xemallweb" ng-click="changeallquyenxemweb()" id="quyenxemall" class="k-checkbox"/><label class="k-checkbox-label" for="quyenxemall">' + $.i18n('header_xem') + '</label>',
            template: templateCheckBoxGridWEB("xem", 'iD_ChucNang'), attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "50px"
        });
        dataList.push({
            field: "them", title: $.i18n('header_them'),
            headerTemplate: '<input type="checkbox" ng-model="themallweb" ng-click="changeallquyenthemweb()" id="quyenthemall" class="k-checkbox"/><label class="k-checkbox-label" for="quyenthemall">' + $.i18n('header_them') + '</label>',
            template: templateCheckBoxGridWEB("them", 'iD_ChucNang'), attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "60px"
        });
        dataList.push({
            field: "sua", title: $.i18n('header_sua'),
            headerTemplate: '<input type="checkbox" ng-model="suaallweb" ng-click="changeallquyensuaweb()" id="quyensuaall" class="k-checkbox"/><label class="k-checkbox-label" for="quyensuaall">' + $.i18n('header_sua') + '</label>',
            template: templateCheckBoxGridWEB("sua", 'iD_ChucNang'), attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "50px"
        });
        dataList.push({
            field: "xoa", title: $.i18n('header_xoa'), attributes: { class: "text-center" },
            headerTemplate: '<input type="checkbox" ng-model="xoaallweb" ng-click="changeallquyenxoaweb()" id="quyenxoaall" class="k-checkbox"/><label class="k-checkbox-label" for="quyenxoaall">' + $.i18n('header_xoa') + '</label>',
            template: templateCheckBoxGridWEB("xoa", 'iD_ChucNang'),
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "50px"
        });

        return dataList;
    }
    function loadgridweb() {
        $scope.xemallweb = false;
        $scope.themallweb = false;
        $scope.suaallweb = false;
        $scope.xoaallweb = false;
        kendo.ui.progress($("#gridweb"), true);
        $scope.gridwebOptions = {
            sortable: false,
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
            pageable: false,
            columns: listColumnsgridweb()
        };

        phanQuyenDataService.getchucnangweb(__idnhom).then(function (result) {
            $scope.gridwebData = {
                data: result.data,
                schema: {
                    model: {
                        id: 'iD_ChucNang',
                        fields: {
                            iD_ChucNang: {
                                type: "number"
                            }
                        }
                    }
                },
                pageSize: 500
            };
            kendo.ui.progress($("#gridweb"), false);
        });
    }
    function reloadloadgridweb(_data) {
        kendo.ui.progress($("#gridweb"), true);

        $timeout(function () {
            $scope.gridwebData = {
                data: _data,
                schema: {
                    model: {
                        id: 'iD_ChucNang',
                        fields: {
                            iD_ChucNang: {
                                type: "number"
                            }
                        }
                    }
                },
                pageSize: 500
            };
            kendo.ui.progress($("#gridweb"), false);
        }, 100);
    }

    function templateCheckBoxGridAPP(nameField, idfield) {
        let temp = '<input type="checkbox" id=quyen"' + '#=' + idfield + '#" ng-click="changeCheckBoxApp($event, \'' + nameField + '\')"  #= (' +
            nameField + ' == 1) ?\'checked="checked"\' : (' + nameField + '== 2)? \'disabled = true\': "" # class="chkbx k-checkbox" /> '
            + '<label class="k-checkbox-label" for=quyen"' + '#=' + idfield + '#"></label>';
        return temp;
    }
    function listColumnsgridapp() {
        let dataList = [];

        dataList.push({
            field: "tenChucNang", title: $.i18n('header_tenchucnangapp'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px"
        });
        dataList.push({
            field: "xem", title: $.i18n('header_quyen'),
            headerTemplate: '<input type="checkbox" ng-model="allapp" ng-click="changeallquyenapp()" id="quyenall" class="k-checkbox"/><label class="k-checkbox-label" for="quyenall">' + $.i18n('header_quyen') + '</label>',
            template: templateCheckBoxGridAPP("xem", "iD_ChucNang"), attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "50px"
        });

        return dataList;
    }
    function loadgridapp() {
        $scope.allapp = false;
        kendo.ui.progress($("#gridapp"), true);
        $scope.gridappOptions = {
            sortable: false,
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
            pageable: false,
            columns: listColumnsgridapp()
        };

        phanQuyenDataService.getchucnangapp(__idnhom).then(function (result) {
            $scope.gridappData = {
                data: result.data,
                schema: {
                    model: {
                        id: 'iD_ChucNang',
                        fields: {
                            iD_ChucNang: {
                                type: "number"
                            }
                        }
                    }
                },
                pageSize: 500
            };
            kendo.ui.progress($("#gridapp"), false);
        });
    }

    function reloadloadgridapp(_data) {
        kendo.ui.progress($("#gridapp"), true);

        $timeout(function () {
            $scope.gridappData = {
                data: _data,
                schema: {
                    model: {
                        id: 'iD_ChucNang',
                        fields: {
                            iD_ChucNang: {
                                type: "number"
                            }
                        }
                    }
                },
                pageSize: 500
            };
            kendo.ui.progress($("#gridapp"), false);
        }, 100);
    }

    //event
    $scope.changeCheckBoxWeb = function (e, name) {
        var checked = e.currentTarget.checked;
        var dataItem = $("#gridweb").data("kendoGrid").dataItem(e.currentTarget.closest("tr"));
        if (checked) {
            dataItem[name] = 1;
        } else {
            dataItem[name] = 0;
        }
    }
    $scope.changeCheckBoxApp = function (e, name) {
        var checked = e.currentTarget.checked;
        var dataItem = $("#gridapp").data("kendoGrid").dataItem(e.currentTarget.closest("tr"));
        if (checked) {
            dataItem[name] = 1;
        } else {
            dataItem[name] = 0;
        }
    }

    $scope.changeallquyenapp = function () {
        $scope.allapp = !$scope.allapp;
        let quyen = ($scope.allapp) ? 1 : 0;
        let ListChucNangAPP = [];
        let dataapp = $("#gridapp").data("kendoGrid").dataSource.data();
        for (let i = 0; i < dataapp.length; i++) {
            let item = {
                iD_ChucNang: dataapp[i].iD_ChucNang,
                tenChucNang: dataapp[i].tenChucNang,
                xem: quyen,
                them: quyen,
                sua: quyen,
                xoa: quyen
            }

            ListChucNangAPP.push(item);
        }

        reloadloadgridapp(ListChucNangAPP);
    }

    $scope.changeallquyenxemweb = function () {
        $scope.xemallweb = !$scope.xemallweb;
        let quyenxem = ($scope.xemallweb) ? 1 : 0;
        let ListChucNangWEB = [];
        let dataweb = $("#gridweb").data("kendoGrid").dataSource.data();
        for (let i = 0; i < dataweb.length; i++) {
            let item = {
                iD_ChucNang: dataweb[i].iD_ChucNang,
                tenChucNang: dataweb[i].tenChucNang,
                xem: quyenxem,
                them: dataweb[i].them,
                sua: dataweb[i].sua,
                xoa: dataweb[i].xoa
            }
            ListChucNangWEB.push(item);
        }

        reloadloadgridweb(ListChucNangWEB);
    }
    $scope.changeallquyenthemweb = function () {
        $scope.themallweb = !$scope.themallweb;
        let quyenthem = ($scope.themallweb) ? 1 : 0;
        let ListChucNangWEB = [];
        let dataweb = $("#gridweb").data("kendoGrid").dataSource.data();
        for (let i = 0; i < dataweb.length; i++) {
            let item = {
                iD_ChucNang: dataweb[i].iD_ChucNang,
                tenChucNang: dataweb[i].tenChucNang,
                xem: dataweb[i].xem,
                them: quyenthem,
                sua: dataweb[i].sua,
                xoa: dataweb[i].xoa
            }
            ListChucNangWEB.push(item);
        }

        reloadloadgridweb(ListChucNangWEB);
    }
    $scope.changeallquyensuaweb = function () {
        $scope.suaallweb = !$scope.suaallweb;
        let quyensua = ($scope.suaallweb) ? 1 : 0;
        let ListChucNangWEB = [];
        let dataweb = $("#gridweb").data("kendoGrid").dataSource.data();
        for (let i = 0; i < dataweb.length; i++) {
            let item = {
                iD_ChucNang: dataweb[i].iD_ChucNang,
                tenChucNang: dataweb[i].tenChucNang,
                xem: dataweb[i].xem,
                them: dataweb[i].them,
                sua: quyensua,
                xoa: dataweb[i].xoa
            }
            ListChucNangWEB.push(item);
        }

        reloadloadgridweb(ListChucNangWEB);
    }
    $scope.changeallquyenxoaweb = function () {
        $scope.xoaallweb = !$scope.xoaallweb;
        let quyenxoa = ($scope.xoaallweb) ? 1 : 0;
        let ListChucNangWEB = [];
        let dataweb = $("#gridweb").data("kendoGrid").dataSource.data();
        for (let i = 0; i < dataweb.length; i++) {
            let item = {
                iD_ChucNang: dataweb[i].iD_ChucNang,
                tenChucNang: dataweb[i].tenChucNang,
                xem: dataweb[i].xem,
                them: dataweb[i].them,
                sua: dataweb[i].sua,
                xoa: quyenxoa
            }
            ListChucNangWEB.push(item);
        }

        reloadloadgridweb(ListChucNangWEB);
    }

    $scope.capnhat = function () {
        commonOpenLoadingText("#btn_capnhat");

        let ListChucNangWEB = [];
        let ListChucNangAPP = [];

        let dataweb = $("#gridweb").data("kendoGrid").dataSource.data();
        for (let i = 0; i < dataweb.length; i++) {
            let item = {
                ID_ChucNang: dataweb[i].iD_ChucNang,
                Xem: dataweb[i].xem,
                Them: dataweb[i].them,
                Sua: dataweb[i].sua,
                Xoa: dataweb[i].xoa
            }
            if (item.Xem == 1)
                ListChucNangWEB.push(item);
        }

        let dataapp = $("#gridapp").data("kendoGrid").dataSource.data();
        for (let i = 0; i < dataapp.length; i++) {
            let item = {
                ID_ChucNang: dataapp[i].iD_ChucNang,
                Xem: dataapp[i].xem,
                Them: dataapp[i].them,
                Sua: dataapp[i].sua,
                Xoa: dataapp[i].xoa
            }
            if (item.Xem == 1)
                ListChucNangAPP.push(item);
        }

        let data = {
            ID_Nhom: __idnhom,
            ListChucNangWEB: ListChucNangWEB,
            ListChucNangAPP: ListChucNangAPP
        };

        console.log(data);

        phanQuyenDataService.capnhatphanquyennhom(data).then(function (result) {
            if (result.flag) {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
            }
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');

            commonCloseLoadingText("#btn_capnhat");
        });
    }

    init();

})