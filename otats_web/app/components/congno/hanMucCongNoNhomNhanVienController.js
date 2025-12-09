angular.module('app').controller('hanMucCongNoNhomNhanVienController', function ($rootScope, $scope, Notification, hanMucDataService) {
    CreateSiteMap();

    let type = 0;//1: Nhóm khách hàng; 2: Nhóm nhân viên

    function init() {
        type = 0;
        loadgridnhomkhachhang();
        loadgridnhomnhanvien();
    }

    function listColumnsgridnhomkhachhang() {
        var dataList = [];

        dataList.push({
            field: "chiTiet", title: $.i18n('header_datnguong'),
            template: '<button ng-click="openformsetnguongnhomkhachhang()" class="btn btn-link btn-menubar" title ="Đặt số dư ví tối thiểu" ><i class="fas fa-plus-circle fas-sm color-infor"></i></button> ',
            attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        });

        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({
            field: "tenLoaiKhachHang", title: $.i18n('header_tennhomkhachhang'),
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "250px"
        });
        dataList.push({
            field: "congNoChoPhep", title: $.i18n('header_congnochophep'),
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

        return dataList;
    }
    function loadgridnhomkhachhang() {
        kendo.ui.progress($("#gridnhomkhachhang"), true);
        $scope.gridnhomkhachhangOptions = {
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
            columns: listColumnsgridnhomkhachhang()
        };
        hanMucDataService.getdataloaikhachhang().then(function (result) {
            $scope.gridnhomkhachhangData = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                            iD_KhachHang: {
                                type: "number"
                            }
                        }
                    }
                },
                pageSize: 20
            };
            kendo.ui.progress($("#gridnhomkhachhang"), false);

            if (!result.flag) {
                Notification({ title: $.i18n("label_thongbao"), message: $.i18n("label_coloixayra_loadlaitrang") }, 'warning');
            }
        });
    }

    function listColumnsgridnhomnhanvien() {
        var dataList = [];

        dataList.push({
            field: "chiTiet", title: $.i18n('header_datnguong'),
            template: '<button ng-click="openformsetnguongnhomnhanvien()" class="btn btn-link btn-menubar" title ="' + $.i18n("label_datnguongcongno") + '" ><i class="fas fa-plus-circle fas-sm color-infor"></i></button> ',
            attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "100px"
        });

        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({
            field: "tenNhom", title: $.i18n('header_tennhomnhanvien'),
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "250px"
        });
        dataList.push({
            field: "congNoGioiHan", title: "Số dư ví tối thiểu",
            template: function (dataItem) {
                if (dataItem.congNoGioiHan == null) {
                    return kendo.htmlEncode('0');
                } else {
                    return kendo.toString(dataItem.congNoGioiHan, $rootScope.UserInfo.dinhDangSo);
                }
            },
            attributes: { style: "text-align: right" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });

        return dataList;
    }
    function loadgridnhomnhanvien() {
        kendo.ui.progress($("#gridnhomnhanvien"), true);
        $scope.gridnhomnhanvienOptions = {
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
            columns: listColumnsgridnhomnhanvien()
        };
        hanMucDataService.getdatanhomnhanvien().then(function (result) {
            $scope.gridnhomnhanvienData = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                            iD_KhachHang: {
                                type: "number"
                            }
                        }
                    }
                },
                pageSize: 20
            };
            kendo.ui.progress($("#gridnhomnhanvien"), false);

            if (!result.flag) {
                Notification({ title: $.i18n("label_thongbao"), message: $.i18n("label_coloixayra_loadlaitrang") }, 'warning');
            }
        });
    }

    //event
    $scope.openformsetnguongnhomkhachhang = function (e) {
        let myGrid = $('#gridnhomkhachhang').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;

        $scope.idnhom = dataItem.iD_LoaiKhachHang;
        $scope.nhom = dataItem.tenLoaiKhachHang == undefined ? '' : dataItem.tenLoaiKhachHang;
        $scope.nguong = dataItem.congNoChoPhep == null ? 0 : dataItem.congNoChoPhep;

        type = 1;

        $scope.formsetnguong.center().open();
    }
    $scope.openformsetnguongnhomnhanvien = function (e) {
        let myGrid = $('#gridnhomnhanvien').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;

        $scope.idnhom = dataItem.iD_Nhom;
        $scope.nhom = dataItem.tenNhom == undefined ? '' : dataItem.tenNhom;
        $scope.nguong = dataItem.congNoGioiHan == null ? 0 : dataItem.congNoGioiHan;

        type = 2;

        $scope.formsetnguong.center().open();
    }
    $scope.setnguong = function () {
        let flag = true;

        //if ($scope.nguong <= 0) {
        //    flag = false;
        //    Notification({ title: $.i18n('label_thongbao'), message: $.i18n("label_nguongcongnokhongthenhohonhoacbangkhong") }, 'warning');
        //}

        if (flag) {
            if (type === 1) {
                hanMucDataService.setnguongloaikhachhang($scope.idnhom, $scope.nguong).then(function (result) {
                    if (result.flag) {
                        $scope.formsetnguong.close();
                        init();
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                    }
                    else
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
                });
            }
            if (type === 2) {
                hanMucDataService.setnguongnhomnhanvien($scope.idnhom, $scope.nguong).then(function (result) {
                    if (result.flag) {
                        $scope.formsetnguong.close();
                        init();
                        Notification({ title: $.i18n('label_thongbao'), message: "Lưu thông tin thành công" }, 'success');
                    }
                    else
                        Notification({ title: $.i18n('label_thongbao'), message: "Lưu thông tin thất bại" }, 'warning');
                });
            }
        }
    }
    $scope.huysetnguong = function () {
        type = 0;
        $scope.formsetnguong.close();
    }

    init();

})