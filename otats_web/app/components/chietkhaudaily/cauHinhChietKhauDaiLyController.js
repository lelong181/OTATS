angular.module('app').controller('cauHinhChietKhauDaiLyController', function ($rootScope, $scope, Notification, chietKhauDataService) {
    CreateSiteMap();


    function init() {
        loadgridnhomnhanvien();
    }

    function listColumnsgridnhomnhanvien() {
        var dataList = [];

        dataList.push({
            field: "chiTiet", title: "Cấu hình",
            template: '<button ng-click="openformsetnguongnhomnhanvien()" class="btn btn-link btn-menubar" title ="Đặt mức hoa hồng" ><i class="fas fa-plus-circle fas-sm color-infor"></i></button> ',
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
            field: "hoaHong", title: "Mức hoa hồng",
            template: function (dataItem) {
                if (dataItem.hoaHong == null) {
                    return kendo.htmlEncode('-');
                } else {
                    return kendo.toString(dataItem.hoaHong, $rootScope.UserInfo.dinhDangSo);
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
        chietKhauDataService.getdatanhomnhanvien().then(function (result) {
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
    $scope.openformsetnguongnhomnhanvien = function (e) {
        let myGrid = $('#gridnhomnhanvien').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;

        $scope.idnhom = dataItem.iD_Nhom;
        $scope.nhom = dataItem.tenNhom == undefined ? '' : dataItem.tenNhom;
        $scope.hoaHong = dataItem.hoaHong == null ? 0 : dataItem.hoaHong;


        $scope.formsetnguong.center().open();
    }
    $scope.setHoaHong = function () {

        chietKhauDataService.sethoahongnhomnhanvien($scope.idnhom, $scope.hoaHong).then(function (result) {
            if (result.flag) {
                $scope.formsetnguong.close();
                init();
                Notification({ title: $.i18n('label_thongbao'), message: "Lưu thông tin thành công" }, 'success');
            }
            else
                Notification({ title: $.i18n('label_thongbao'), message: "Lưu thông tin thất bại" }, 'warning');
        });
    }


    $scope.huysetHoaHong = function () {
        type = 0;
        $scope.formsetnguong.close();
    }

    init();

})