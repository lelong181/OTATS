angular.module('app').controller('bm041_BaoCaoDoanhThuCacDiemController', function ($rootScope, $scope, $state, Notification, bieuDoDoanhThuDataService, ComboboxDataService) {
    CreateSiteMap();

    //config
    function init() {
        initdate();

        ComboboxDataService.getSite().then(function (result) {
            $scope.sitedata = result.data;
            $scope.siteselect = $scope.sitedata[0];
            loadgrid();
        });

        loadnhomhang();
    }

    function initdate() {
        var dateNow = new Date();
        $scope.obj_TuNgay = new Date(dateNow.setHours(0, 0, 0));
        $scope.obj_DenNgay = new Date(dateNow.setHours(23, 59, 59));
        $scope.maxDate = $scope.obj_DenNgay;
        $scope.minDate = $scope.obj_TuNgay;
    }
    function listColumnsgrid() {
        var dataList = [];
        dataList.push({
            field: "maHang",
            title: "Mã Hàng",
            locked: true,
            attributes: {
                style: "text-align: center"
            },
            groupFooterAttributes: {
                style: "text-align: right"
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: defaultFilterableGrid, width: "120px"
        });

        dataList.push({
            field: "tenHang", title: "Tên Hàng",
            locked: true,

            attributes: {
                style: "text-align: center"
            },
            groupFooterAttributes: {
                style: "text-align: right"
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: defaultFilterableGrid, width: "350px"
        });

        dataList.push({
            field: "giaBanLe",
            locked: true,

            title: "Giá Bán Lẻ",
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            attributes: {
                style: "text-align: center"
            },
            groupFooterAttributes: {
                style: "text-align: right"
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: defaultFilterableGrid, width: "120px"
        });

        dataList.push({
            title: "Bóc Tách Doanh Thu Chi Tiết",
            attributes: {
                style: "text-align: center"
            },
            groupFooterAttributes: {
                style: "text-align: right"
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: defaultFilterableGrid,
            width: "120px",
            columns: [
                {
                    field: "trangaN_GiaBan",
                    format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
                    title: "Tràng An",
                    attributes: {
                        style: "text-align: center"
                    },
                    groupFooterAttributes: {
                        style: "text-align: right"
                    },
                    headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
                    filterable: defaultFilterableGrid,
                    width: "120px"
                },
                {
                    field: "baidinH_GiaBan",
                    title: "Bái Đính",
                    format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
                    attributes: {
                        style: "text-align: center"
                    },
                    groupFooterAttributes: {
                        style: "text-align: right"
                    },
                    headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
                    filterable: defaultFilterableGrid,
                    width: "120px"
                },
                {
                    field: "tamcoC_GiaBan",
                    title: "Tam Cốc",
                    format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
                    attributes: {
                        style: "text-align: center"
                    },
                    groupFooterAttributes: {
                        style: "text-align: right"
                    },
                    headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
                    filterable: defaultFilterableGrid,
                    width: "120px"
                },
                {
                    field: "pchL_GiaBan",
                    title: "Phố Cổ",
                    format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
                    attributes: {
                        style: "text-align: center"
                    },
                    groupFooterAttributes: {
                        style: "text-align: right"
                    },
                    headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
                    filterable: defaultFilterableGrid,
                    width: "120px"
                },
                {
                    field: "taG_GiaBan",
                    title: "Green Pine",
                    format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
                    attributes: {
                        style: "text-align: center"
                    },
                    groupFooterAttributes: {
                        style: "text-align: right"
                    },
                    headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
                    filterable: defaultFilterableGrid,
                    width: "120px"
                }
            ]
        });

        dataList.push({
            title: "Số Lượng Vé Bán Tại Các Điểm",
            attributes: {
                style: "text-align: center"
            },
            groupFooterAttributes: {
                style: "text-align: right"
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: defaultFilterableGrid,
            width: "120px",
            columns: [
                {
                    field: "trangaN_SoLuong",
                    title: "Tràng An",
                    attributes: {
                        style: "text-align: center"
                    },
                    groupFooterAttributes: {
                        style: "text-align: right"
                    },
                    aggregates: ["sum"],
                    footerTemplate: function (e) {
                        if (e.trangaN_SoLuong) {
                            return kendo.toString(e.trangaN_SoLuong.sum);
                        }
                    },

                    headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
                    footerAttributes: { style: "text-align: center; font-weight:700" },
                    filterable: defaultFilterableGrid,
                    width: "120px"
                },
                {
                    field: "baidinH_SoLuong",
                    title: "Bái Đính",
                    attributes: {
                        style: "text-align: center"
                    },
                    groupFooterAttributes: {
                        style: "text-align: right"
                    },
                    aggregates: ["sum"],
                    footerTemplate: function (e) {
                        if (e.baidinH_SoLuong) {
                            return kendo.toString(e.baidinH_SoLuong.sum);
                        }
                    },

                    headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
                    footerAttributes: { style: "text-align: center; font-weight:700" },
                    filterable: defaultFilterableGrid,
                    width: "120px"
                },
                {
                    field: "tamcoC_SoLuong",
                    title: "Tam Cốc",
                    attributes: {
                        style: "text-align: center"
                    },
                    groupFooterAttributes: {
                        style: "text-align: right"
                    },
                    aggregates: ["sum"],
                    footerTemplate: function (e) {
                        if (e.tamcoC_SoLuong) {
                            return kendo.toString(e.tamcoC_SoLuong.sum);
                        }
                    },

                    headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
                    footerAttributes: { style: "text-align: center; font-weight:700" },
                    filterable: defaultFilterableGrid,
                    width: "120px"
                },
                {
                    field: "pchL_SoLuong",
                    title: "Phố Cổ",
                    attributes: {
                        style: "text-align: center"
                    },
                    groupFooterAttributes: {
                        style: "text-align: right"
                    },
                    aggregates: ["sum"],
                    footerTemplate: function (e) {
                        if (e.pchL_SoLuong) {
                            return kendo.toString(e.pchL_SoLuong.sum);
                        }
                    },

                    headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
                    footerAttributes: { style: "text-align: center; font-weight:700" },
                    filterable: defaultFilterableGrid,
                    width: "120px"
                },
                {
                    field: "taG_SoLuong",
                    title: "Green Pine",
                    attributes: {
                        style: "text-align: center"
                    },
                    groupFooterAttributes: {
                        style: "text-align: right"
                    },
                    aggregates: ["sum"],
                    footerTemplate: function (e) {
                        if (e.taG_SoLuong) {
                            return kendo.toString(e.taG_SoLuong.sum);
                        }
                    },

                    headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
                    footerAttributes: { style: "text-align: center; font-weight:700" },
                    filterable: defaultFilterableGrid,
                    width: "120px"
                },
                {
                    field: "tong_SoLuong",
                    title: "Tổng",
                    attributes: {
                        style: "text-align: center"
                    },
                    groupFooterAttributes: {
                        style: "text-align: right"
                    },
                    aggregates: ["sum"],
                    footerTemplate: function (e) {
                        if (e.tong_SoLuong) {
                            return kendo.toString(e.tong_SoLuong.sum);
                        }
                    },

                    headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
                    footerAttributes: { style: "text-align: center; font-weight:700" },
                    filterable: defaultFilterableGrid,
                    width: "120px"
                }
            ]
        });

        dataList.push({
            title: "Tổng doanh thu DV của các khu",
            field: "tong_DoanhThuCacKhu",
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            attributes: {
                style: "text-align: center"
            },
            groupFooterAttributes: {
                style: "text-align: right"
            },
            aggregates: ["sum"],
            footerTemplate: function (e) {
                if (e.tong_DoanhThuCacKhu) {
                    return kendo.toString(e.tong_DoanhThuCacKhu.sum, $rootScope.UserInfo.dinhDangSo);
                }
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center; white-space: break-spaces;" },
            footerAttributes: { style: "text-align: center; font-weight:700" },

            filterable: defaultFilterableGrid,
            width: "120px"
        });

        dataList.push({
            title: "Số Tiền Phải Trả Các Đơn Vị Khác",
            attributes: {
                style: "text-align: center"
            },
            groupFooterAttributes: {
                style: "text-align: right"
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: defaultFilterableGrid,
            width: "120px",
            columns: [
                {
                    field: "trangaN_TraLai",
                    title: "Tràng An",
                    format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
                    attributes: {
                        style: "text-align: center"
                    },
                    groupFooterAttributes: {
                        style: "text-align: right"
                    },
                    aggregates: ["sum"],
                    footerTemplate: function (e) {
                        if (e.trangaN_TraLai) {
                            return kendo.toString(e.trangaN_TraLai.sum, $rootScope.UserInfo.dinhDangSo);
                        }
                    },

                    headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
                    footerAttributes: { style: "text-align: center; font-weight:700" },
                    filterable: defaultFilterableGrid,
                    width: "120px"
                },
                {
                    field: "baidinH_TraLai",
                    title: "Bái Đính",
                    format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
                    attributes: {
                        style: "text-align: center"
                    },
                    groupFooterAttributes: {
                        style: "text-align: right"
                    },
                    aggregates: ["sum"],
                    footerTemplate: function (e) {
                        if (e.baidinH_TraLai) {
                            return kendo.toString(e.baidinH_TraLai.sum, $rootScope.UserInfo.dinhDangSo);
                        }
                    },

                    headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
                    footerAttributes: { style: "text-align: center; font-weight:700" },
                    filterable: defaultFilterableGrid,
                    width: "120px"
                },
                {
                    field: "tamcoC_TraLai",
                    title: "Tam Cốc",
                    format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
                    attributes: {
                        style: "text-align: center"
                    },
                    groupFooterAttributes: {
                        style: "text-align: right"
                    },
                    aggregates: ["sum"],
                    footerTemplate: function (e) {
                        if (e.tamcoC_TraLai) {
                            return kendo.toString(e.tamcoC_TraLai.sum, $rootScope.UserInfo.dinhDangSo);
                        }
                    },

                    headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
                    footerAttributes: { style: "text-align: center; font-weight:700" },
                    filterable: defaultFilterableGrid,
                    width: "120px"
                },
                {
                    field: "pchL_TraLai",
                    title: "Phố Cổ",
                    format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
                    attributes: {
                        style: "text-align: center"
                    },
                    groupFooterAttributes: {
                        style: "text-align: right"
                    },
                    aggregates: ["sum"],
                    footerTemplate: function (e) {
                        if (e.pchL_TraLai) {
                            return kendo.toString(e.pchL_TraLai.sum, $rootScope.UserInfo.dinhDangSo);
                        }
                    },

                    headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
                    footerAttributes: { style: "text-align: center; font-weight:700" },
                    filterable: defaultFilterableGrid,
                    width: "120px"
                },
                {
                    field: "taG_TraLai",
                    title: "Green Pine",
                    format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
                    attributes: {
                        style: "text-align: center"
                    },
                    groupFooterAttributes: {
                        style: "text-align: right"
                    },
                    aggregates: ["sum"],
                    footerTemplate: function (e) {
                        if (e.taG_TraLai) {
                            return kendo.toString(e.taG_TraLai.sum, $rootScope.UserInfo.dinhDangSo);
                        }
                    },

                    headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
                    footerAttributes: { style: "text-align: center; font-weight:700" },
                    filterable: defaultFilterableGrid,
                    width: "120px"
                },
                {
                    field: "tongTien_TraLai",
                    title: "Tổng",
                    format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
                    attributes: {
                        style: "text-align: center"
                    },
                    groupFooterAttributes: {
                        style: "text-align: right"
                    },
                    aggregates: ["sum"],
                    footerTemplate: function (e) {
                        if (e.tongTien_TraLai) {
                            return kendo.toString(e.tongTien_TraLai.sum, $rootScope.UserInfo.dinhDangSo);
                        }
                    },

                    headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
                    footerAttributes: { style: "text-align: center; font-weight:700" },
                    filterable: defaultFilterableGrid,
                    width: "120px"
                }
            ]
        });

        dataList.push({
            title: "Tổng doanh thu tại khu",
            field: "tong_DoanhThuTaiKhu",
            format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
            attributes: {
                style: "text-align: center"
            },
            groupFooterAttributes: {
                style: "text-align: right"
            },
            aggregates: ["sum"],
            footerTemplate: function (e) {
                if (e.tong_DoanhThuTaiKhu) {
                    return kendo.toString(e.tong_DoanhThuTaiKhu.sum, $rootScope.UserInfo.dinhDangSo);
                }
            },

            headerAttributes: { "class": "table-header-cell", style: "text-align: center; white-space: break-spaces;" },
            footerAttributes: { style: "text-align: center; font-weight:700" },

            filterable: defaultFilterableGrid,
            width: "120px"
        });

        dataList.push({
            title: "Phải Thu",
            attributes: {
                style: "text-align: center"
            },
            groupFooterAttributes: {
                style: "text-align: right"
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: defaultFilterableGrid,
            width: "120px",
            columns: [
                {
                    field: "trangaN_PhaiThu",
                    title: "Tràng An",
                    format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
                    //hidden: $scope.siteselect.siteCode == "TRANGAN" ? true : false,
                    attributes: {
                        style: "text-align: center"
                    },
                    groupFooterAttributes: {
                        style: "text-align: right"
                    },
                    aggregates: ["sum"],
                    footerTemplate: function (e) {
                        if (e.trangaN_PhaiThu) {
                            return kendo.toString(e.trangaN_PhaiThu.sum, $rootScope.UserInfo.dinhDangSo);
                        }
                    },

                    headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
                    footerAttributes: { style: "text-align: center; font-weight:700" },

                    filterable: defaultFilterableGrid,
                    width: "120px"
                },
                {
                    field: "baidinH_PhaiThu",
                    title: "Bái Đính",
                    format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
                    //hidden: $scope.siteselect.siteCode == "BAIDINH" ? true : false,
                    attributes: {
                        style: "text-align: center"
                    },
                    groupFooterAttributes: {
                        style: "text-align: right"
                    },
                    aggregates: ["sum"],
                    footerTemplate: function (e) {
                        if (e.baidinH_PhaiThu) {
                            return kendo.toString(e.baidinH_PhaiThu.sum, $rootScope.UserInfo.dinhDangSo);
                        }
                    },

                    headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
                    footerAttributes: { style: "text-align: center; font-weight:700" },
                    filterable: defaultFilterableGrid,
                    width: "120px"
                },
                {
                    field: "tamcoC_PhaiThu",
                    title: "Tam Cốc",
                    format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
                    //hidden: $scope.siteselect.siteCode == "TAMCOC" ? true : false,
                    attributes: {
                        style: "text-align: center"
                    },
                    groupFooterAttributes: {
                        style: "text-align: right"
                    },
                    aggregates: ["sum"],
                    footerTemplate: function (e) {
                        if (e.tamcoC_PhaiThu) {
                            return kendo.toString(e.tamcoC_PhaiThu.sum, $rootScope.UserInfo.dinhDangSo);
                        }
                    },

                    headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
                    footerAttributes: { style: "text-align: center; font-weight:700" },
                    filterable: defaultFilterableGrid,
                    width: "120px"
                },
                {
                    field: "pchL_PhaiThu",
                    title: "Phố Cổ",
                    format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
                    //hidden: $scope.siteselect.siteCode == "PCHL" ? true : false,
                    attributes: {
                        style: "text-align: center"
                    },
                    groupFooterAttributes: {
                        style: "text-align: right"
                    },
                    aggregates: ["sum"],
                    footerTemplate: function (e) {
                        if (e.pchL_PhaiThu) {
                            return kendo.toString(e.pchL_PhaiThu.sum, $rootScope.UserInfo.dinhDangSo);
                        }
                    },

                    headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
                    footerAttributes: { style: "text-align: center; font-weight:700" },
                    filterable: defaultFilterableGrid,
                    width: "120px"
                },
                {
                    field: "taG_PhaiThu",
                    title: "Green Pine",
                    format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
                    //hidden: $scope.siteselect.siteCode == "TAG" ? true : false,
                    attributes: {
                        style: "text-align: center"
                    },
                    groupFooterAttributes: {
                        style: "text-align: right"
                    },
                    aggregates: ["sum"],
                    footerTemplate: function (e) {
                        if (e.taG_PhaiThu) {
                            return kendo.toString(e.taG_PhaiThu.sum, $rootScope.UserInfo.dinhDangSo);
                        }
                    },

                    headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
                    footerAttributes: { style: "text-align: center; font-weight:700" },
                    filterable: defaultFilterableGrid,
                    width: "120px"
                },
                {
                    field: "tongTien_PhaiThu",
                    title: "Tổng",
                    format: formatNumberInGrid($rootScope.UserInfo.dinhDangSo),
                    attributes: {
                        style: "text-align: center"
                    },
                    groupFooterAttributes: {
                        style: "text-align: right"
                    },
                    aggregates: ["sum"],
                    footerTemplate: function (e) {
                        if (e.tongTien_PhaiThu) {
                            return kendo.toString(e.tongTien_PhaiThu.sum, $rootScope.UserInfo.dinhDangSo);
                        }
                    },

                    headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
                    footerAttributes: { style: "text-align: center; font-weight:700" },
                    filterable: defaultFilterableGrid,
                    width: "120px"
                }
            ]
        });

        return dataList;
    }

    function loadgrid() {
        $scope.datenow = kendo.toString(new Date(), "dd/MM/yyyy HH:mm");
        kendo.ui.progress($("#grid"), true);
        commonOpenLoadingText("#btn_bm009xembaocao");
        $scope.gridOptions = {
            dataSource: new kendo.data.DataSource(
                {
                    data: [],
                    schema: {
                        model: {
                            fields: {
                                trangaN_SoLuong: { type: "number" },
                                baidinH_SoLuong: { type: "number" },
                                tamcoC_SoLuong: { type: "number" },
                                pchL_SoLuong: { type: "number" },
                                taG_SoLuong: { type: "number" },
                                tong_SoLuong: { type: "number" },

                                trangaN_TraLai: { type: "number" },
                                baidinH_TraLai: { type: "number" },
                                tamcoC_TraLai: { type: "number" },
                                pchL_TraLai: { type: "number" },
                                taG_TraLai: { type: "number" },
                                tongTien_TraLai: { type: "number" },

                                trangaN_PhaiThu: { type: "number" },
                                baidinH_PhaiThu: { type: "number" },
                                tamcoC_PhaiThu: { type: "number" },
                                pchL_PhaiThu: { type: "number" },
                                taG_PhaiThu: { type: "number" },
                                TongTien_PhaiThu: { type: "number" },
                                tong_DoanhThuTaiKhu: { type: "number" },
                                tong_DoanhThuCacKhu: { type: "number" },
                            }
                        }
                    },
                    pageSize: 200,
                    aggregate: [
                        { field: "trangaN_SoLuong", aggregate: "sum" },
                        { field: "baidinH_SoLuong", aggregate: "sum" },
                        { field: "tamcoC_SoLuong", aggregate: "sum" },
                        { field: "pchL_SoLuong", aggregate: "sum" },
                        { field: "taG_SoLuong", aggregate: "sum" },
                        { field: "tong_SoLuong", aggregate: "sum" },

                        { field: "trangaN_TraLai", aggregate: "sum" },
                        { field: "baidinH_TraLai", aggregate: "sum" },
                        { field: "tamcoC_TraLai", aggregate: "sum" },
                        { field: "pchL_TraLai", aggregate: "sum" },
                        { field: "taG_TraLai", aggregate: "sum" },
                        { field: "tongTien_TraLai", aggregate: "sum" },

                        { field: "trangaN_PhaiThu", aggregate: "sum" },
                        { field: "baidinH_PhaiThu", aggregate: "sum" },
                        { field: "tamcoC_PhaiThu", aggregate: "sum" },
                        { field: "pchL_PhaiThu", aggregate: "sum" },
                        { field: "taG_PhaiThu", aggregate: "sum" },
                        { field: "tongTien_PhaiThu", aggregate: "sum" },
                        { field: "tong_DoanhThuTaiKhu", aggregate: "sum" },
                        { field: "tong_DoanhThuCacKhu", aggregate: "sum" },
                    ]
                }
            ),
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".content-filter").height()) - 70;
                return heightGrid < 100 ? 500 : heightGrid;
            },
            excel: {
                allPages: true
            },
            excelExport: function (e) {

            },
            resizable: true,
            editable: false,
            groupable: false,
            filterable: {
                mode: "row"
            },
            pageable: $scope.lang == 'vi-vn' ? pageableShort_vi : pageableShort_en,
            columns: listColumnsgrid()
        };
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        let sitecode = $scope.siteselect == undefined ? '' : $scope.siteselect.siteCode

        let i = listColumnsgrid();

        //switch (sitecode) {
        //    case 'TRANGAN':               
        //        i[8].columns[0].hidden = true;              
        //        break;
        //    case 'BAIDINH':
        //        i[8].columns[1].hidden = true;
        //        break;
        //    case 'TAMCOC':
        //        i[8].columns[2].hidden = true;
        //        break;
        //    case 'PCHL':
        //        i[8].columns[3].hidden = true;
        //        break;
        //    case 'TAG':
        //        i[8].columns[4].hidden = true;
        //        break;
        //}
        //try {
        //    $("#grid").data("kendoGrid").setOptions({
        //        columns: i,
        //    })
        //} catch {

        //}
        

        bieuDoDoanhThuDataService.getBaoCaoDoanhThuCacDiem($scope.nhomhanghoaSelected, fromdate, todate, sitecode).then(function (result) {
            $scope.gridData = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                            trangaN_SoLuong: { type: "number" },
                            baidinH_SoLuong: { type: "number" },
                            tamcoC_SoLuong: { type: "number" },
                            pchL_SoLuong: { type: "number" },
                            taG_SoLuong: { type: "number" },
                            tong_SoLuong: { type: "number" },

                            trangaN_TraLai: { type: "number" },
                            baidinH_TraLai: { type: "number" },
                            tamcoC_TraLai: { type: "number" },
                            pchL_TraLai: { type: "number" },
                            taG_TraLai: { type: "number" },
                            tongTien_TraLai: { type: "number" },

                            trangaN_PhaiThu: { type: "number" },
                            baidinH_PhaiThu: { type: "number" },
                            tamcoC_PhaiThu: { type: "number" },
                            pchL_PhaiThu: { type: "number" },
                            taG_PhaiThu: { type: "number" },
                            tongTien_PhaiThu: { type: "number" },
                            tong_DoanhThuTaiKhu: { type: "number" },
                            tong_DoanhThuCacKhu: { type: "number" },
                        }
                    }
                },
                pageSize: 200,

                aggregate: [
                    { field: "trangaN_SoLuong", aggregate: "sum" },
                    { field: "baidinH_SoLuong", aggregate: "sum" },
                    { field: "tamcoC_SoLuong", aggregate: "sum" },
                    { field: "pchL_SoLuong", aggregate: "sum" },
                    { field: "taG_SoLuong", aggregate: "sum" },
                    { field: "tong_SoLuong", aggregate: "sum" },

                    { field: "trangaN_TraLai", aggregate: "sum" },
                    { field: "baidinH_TraLai", aggregate: "sum" },
                    { field: "tamcoC_TraLai", aggregate: "sum" },
                    { field: "pchL_TraLai", aggregate: "sum" },
                    { field: "taG_TraLai", aggregate: "sum" },
                    { field: "tongTien_TraLai", aggregate: "sum" },

                    { field: "trangaN_PhaiThu", aggregate: "sum" },
                    { field: "baidinH_PhaiThu", aggregate: "sum" },
                    { field: "tamcoC_PhaiThu", aggregate: "sum" },
                    { field: "pchL_PhaiThu", aggregate: "sum" },
                    { field: "taG_PhaiThu", aggregate: "sum" },
                    { field: "tongTien_PhaiThu", aggregate: "sum" },
                    { field: "tong_DoanhThuTaiKhu", aggregate: "sum" },
                    { field: "tong_DoanhThuCacKhu", aggregate: "sum" },
                ]
            };

            kendo.ui.progress($("#grid"), false);
            commonCloseLoadingText("#btn_bm009xembaocao")
        });
    }

    function loadnhomhang() {
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
        $scope.nhomhanghoaSelected = 10;
    }

    //event
    $scope.xemBaoCao = function () {
        loadgrid();
    }
    $scope.XuatExcel = function () {
        $("#grid").data("kendoGrid").saveAsExcel();
    }

    $scope.siteOnChange = function () {
        $scope.siteselect = this.siteselect;
    }

    $scope.fromDateChanged = function () {
        $scope.minDate = $scope.obj_TuNgay;
    }
    $scope.toDateChanged = function () {
        $scope.maxDate = $scope.obj_DenNgay;
    }
    $scope.nhomhanghoaOnChange = function () {
        $scope.nhomhanghoaSelected = this.nhomhanghoaSelected;
    }
    init();
})

