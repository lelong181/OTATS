angular.module('app').controller('bm008_BaoCaoKhachHangTheoKhuVucController', function ($rootScope, $scope, Notification, ComboboxDataService, baoCaoKhachHangDataService) {
    CreateSiteMap();
    
    function init() {
        initcombo();
        loadgrid(0, 0);
    }
    
    function initcombo() {
        ComboboxDataService.getTinhThanh().then(function (result) {
            $scope.tinhthanhData = result.data;
            
        });
        ComboboxDataService.getQuanHuyen(0).then(function (result) {
            $scope.quanhuyenData = result.data;
            
        });
    }
    function listColumnsgrid() {
        var dataList = [];
        
        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px", footerTemplate: $.i18n("label_tong"),footerAttributes: { style: "text-align: center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({
            field: "tenTinh", title: $.i18n("header_tinhthanh"),
            attributes: {
                "class": "table-cell",
                style: "text-align: center"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "tenQuan", title: $.i18n("header_quanhuyen"), attributes: {
                "class": "table-cell",
                style: "text-align: center"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "soKhachHang", title: $.i18n("header_sokhachhang"), format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            footerTemplate: formatNumberInFooterGrid('soKhachHang.sum', $rootScope.UserInfo.dinhDangSo), footerAttributes: { style: "text-align: center" }, attributes: {
                "class": "table-cell",
                style: "text-align: center"
            }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        
        return dataList;
    }
    function loadgrid(idtinhthanh, idquanhuyen) {
        kendo.ui.progress($("#grid"), true);
        commonOpenLoadingText("#btn_bm008xembaocao");

        $scope.gridOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".content-filter").height());
                return heightGrid - 30;
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
       
        baoCaoKhachHangDataService.getBaoCaoKhachHangTheoKhuVuc(idtinhthanh, idquanhuyen).then(function (result) {
            $scope.gridData = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                            soKhachHang: {
                                type: "number"
                            },
                            
                        }
                    }
                },
                pageSize: 20,
                aggregate: [
                  { field: "soKhachHang", aggregate: "sum" }
                ]
            };
            kendo.ui.progress($("#grid"), false);
            commonCloseLoadingText("#btn_bm008xembaocao")
        });

        
    }
    //event
    $scope.xemBaoCao = function () {

        var idtinhthanh = 0;
        var idquanhuyen = 0;

        if ($scope.tinhthanhselect != undefined)
            idtinhthanh = ($scope.tinhthanhselect.iD_Tinh < 0) ? 0 : $scope.tinhthanhselect.iD_Tinh;
        if ($scope.quanhuyenselect != undefined)
            idquanhuyen = ($scope.quanhuyenselect.iD_Quan < 0) ? 0 : $scope.quanhuyenselect.iD_Quan;

        loadgrid(idtinhthanh, idquanhuyen);
    }
    $scope.XuatExcel = function () {
        commonOpenLoadingText("#btn_bm008xuatexcel");

        var idtinhthanh = 0;
        var idquanhuyen = 0;

        if ($scope.tinhthanhselect != undefined)
            idtinhthanh = ($scope.tinhthanhselect.iD_Tinh < 0) ? 0 : $scope.tinhthanhselect.iD_Tinh;
        if ($scope.quanhuyenselect != undefined)
            idquanhuyen = ($scope.quanhuyenselect.iD_Quan < 0) ? 0 : $scope.quanhuyenselect.iD_Quan;


        baoCaoKhachHangDataService.getExcelBaoCaoKhachHangTheoKhuVuc(idtinhthanh, idquanhuyen).then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayravuilongloadlaitrang') }, 'warning');

            commonCloseLoadingText("#btn_bm008xuatexcel")
        });
    }

    $scope.tinhthanhOnChange = function () {
        $scope.tinhthanhselect = this.tinhthanhselect;

        $("#quanhuyen").data("kendoComboBox").value("");
        var idtinhthanh = 0;
        if ($scope.tinhthanhselect != undefined)
            idtinhthanh = ($scope.tinhthanhselect.iD_Tinh < 0) ? 0 : $scope.tinhthanhselect.iD_Tinh;
        
        ComboboxDataService.getQuanHuyen(idtinhthanh).then(function (result) {
            $scope.quanhuyenData = result.data;
        });

    }
    $scope.quanhuyenOnChange = function () {
        $scope.quanhuyenselect = this.quanhuyenselect;
    }
    init();

})
