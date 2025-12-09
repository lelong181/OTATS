angular.module('app').controller('chuyenKhachHangController', function ($scope, $state, Notification, nhanVienDataService) {
    CreateSiteMap();

    let idnhanvien = 0;

    function init() {
        loadgrid_employeesFrom();
        loadgrid_customer(-1);
        loadgrid_employeesTo();
    }
    function listColumnsgrid_employeesFrom() {
        let dataList = [];
        dataList.push({
            field: "tenDayDu", title: $.i18n('header_nhanvien'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid,
        });
        return dataList;
    }
    function loadgrid_employeesFrom() {
        $scope.gridOptions_employeesFrom = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height());
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
            pageable: $scope.lang == 'vi-vn' ? pageableShort_vi : pageableShort_en_nodisplay,
            selectable: "row",
            change: onChange,
            columns: listColumnsgrid_employeesFrom()
        };
        nhanVienDataService.dsnhanvien().then(function (result) {
            $scope.gridData_employeesFrom = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                        }
                    }
                },
                pageSize: 5000,
            };
        });
    }
    function onChange(arg) {
        let listid = []

        let grid = $("#grid_employeesFrom").data("kendoGrid");
        grid.select().each(function () {
            let dataItem = grid.dataItem(this);
            listid.push({ idnhanvien: dataItem.idnv });
        });
        console.log(listid);
        if (listid.length == 1) {
            idnhanvien = listid[0].idnhanvien;

            loadgrid_customer(idnhanvien);
        } else {
            loadgrid_customer(0);
        }
    }

    function listColumnsgrid_customer() {
        var dataList = [];
        dataList.push({ title: "#", template: "#= ++RecordNumber #", width: "50px", headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, selectable: true, width: "40px" });
        dataList.push({
            field: "tenKhachHang", title: $.i18n('header_tenkhachhang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid,
        });
        dataList.push({
            field: "diaChi", title: $.i18n('header_diachi'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid,
        });
        return dataList;
    }
    function loadgrid_customer(idnhanvien) {
        $scope.gridOptions_customer = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height());
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
            columns: listColumnsgrid_customer()
        };
        nhanVienDataService.dskhachhang(idnhanvien).then(function (result) {
            $scope.gridData_customer = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                        }
                    }
                },
                pageSize: 5000,
            };
        });
    }

    function listColumnsgrid_employeesTo() {
        var dataList = [];
        dataList.push({ title: "#", width: "50px", headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, selectable: true, width: "40px" });
        dataList.push({
            field: "tenDayDu", title: $.i18n('header_nhanvien'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid,
        });
        return dataList;
    }
    function loadgrid_employeesTo() {
        $scope.gridOptions_employeesTo = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height());
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
            columns: listColumnsgrid_employeesTo()
        };
        nhanVienDataService.dsnhanvien().then(function (result) {
            $scope.gridData_employeesTo = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                        }
                    }
                },
                pageSize: 5000,
            };
        });
    }
    function validate() {

    }

    $scope.chuyenQuyen = function () {
        let listid = []
        let listidnv = []
        let flag = true;
        let msg = '';

        let grid = $("#grid_customer").data("kendoGrid");
        grid.select().each(function () {
            let dataItem = grid.dataItem(this);
            listid.push(dataItem.iD_KhachHang);

        });

        let grid_2 = $("#grid_employeesTo").data("kendoGrid");
        grid_2.select().each(function () {
            let dataItem = grid_2.dataItem(this);
            listidnv.push(dataItem.idnv);

        });

        console.log(listid)
        console.log(listidnv)

        if (flag && idnhanvien <= 0) {
            flag = false;
            msg = $.i18n('label_chuachonnhanviendethuchienchuyenquyen');
        }

        if (flag && listid.length <= 0) {
            flag = false;
            msg = $.i18n('label_chuachonkhachhangdethuchienchuyenquyen');
        }

        if (flag && listidnv.length <= 0) {
            flag = false;
            msg = $.i18n('label_chuachonnhanviendethuchienchuyenquyen');
        }

        if (flag) {
            let data = { ID_NhanVien_Old: idnhanvien, List_ID_NhanVien: listidnv, List_ID_KhachHang: listid }
            nhanVienDataService.dsnhanvien_capnhat(data).then(function (result) {
                if (result.flag) {
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n("label_chuyenquyenthanhcong") }, 'success');
                    loadgrid_employeesFrom();
                    loadgrid_customer(-1);
                    loadgrid_employeesTo();
                }
                else
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n("label_chuyenquyenkhongthanhcongvuilongthulai") }, 'warning');
            });
        } else
            Notification({ title: $.i18n('label_thongbao'), message: msg }, 'warning');
    }

    init();

})