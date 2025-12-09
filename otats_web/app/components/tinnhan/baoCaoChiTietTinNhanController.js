angular.module('app').controller('baoCaoChiTietTinNhanController', function ($scope, Notification, tinNhanDataService, $stateParams) {
    CreateSiteMap();

    let param_idtinnhan = 0;

    let __idnhanvien = 0;

    function init() {
        param_idtinnhan = ($stateParams.idtinnhan == undefined) ? 0 : $stateParams.idtinnhan;

        loadgrid(0);
    }

    function listColumnsgrid() {
        let dataList = [];

        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });

        dataList.push({
            field: "tenNhanVien", title: "Tên nhân viên",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "noiDung", title: "Nội dung",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "250px"
        });
        dataList.push({
            field: "ngayGui", title: "Thời gian gửi",
            template: function (dataItem) {
                return kendo.htmlEncode(kendo.toString(dataItem.ngayGui, formatDateTime));
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "trangThaiHienThi", title: "Trạng thái",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "ngayXemHienThi", title: "Thời gian xem",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });
        dataList.push({
            field: "tenQuanLy", title: "Tên quản lý",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });
        dataList.push({
            field: "loaiGui", title: "Loại gửi",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px"
        });

        dataList.push({
            field: "chiTiet", title: "Trả lời",
            template: '<button ng-click="openformtraloi()" class="btn btn-link btn-menubar" title ="Trả lời" ><i class="fas fa-paper-plane fas-sm color-infor"></i></button> ',
            attributes: { class: "text-center" },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: false, width: "150px"
        });


        return dataList;
    }
    function loadgrid(idnhanvien) {
        kendo.ui.progress($("#grid"), true);
        $scope.gridOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height());
                return heightGrid < 100 ? 500 : heightGrid;
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
            columns: listColumnsgrid(),
        };

        tinNhanDataService.baocaochitiettinnhan(param_idtinnhan).then(function (result) {
            $scope.gridData = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                            ngayGui: {
                                type: "date"
                            },
                        }
                    }
                },
                pageSize: 20
            };
            kendo.ui.progress($("#grid"), false);
        });
    }
    //event
    $scope.openformtraloi = function (e) {
        let myGrid = $('#grid').data("kendoGrid");
        let row = $(this).closest("tr");
        let dataItem = row.prevObject[0].dataItem;

        __idnhanvien = dataItem.iD_NHANVIEN;
        $scope.noidung = '';

        $scope.formtraloi.center().open();
    }

    $scope.traloitinnhan = function () {
        if ($scope.noidung == '') {

        } else {
            let listid = []
            listid.push(__idnhanvien);
            let data = {
                ids: listid,
                noiDung: $scope.noidung
            }
            tinNhanDataService.guiTinNhan(data).then(function (result) {
                if (result.flag) {
                    if (result.data.success) {
                        $scope.formtraloi.center().close();
                        $scope.noidung = "";
                        Notification({ title: $.i18n('label_thongbao'), message: result.data.msg }, 'success');
                    } else {
                        Notification({ title: $.i18n('label_thongbao'), message: result.data.msg }, 'error');
                    }
                } else {
                    Notification({ title: $.i18n('label_thongbao'), message: result.data.msg }, 'error');
                }
            });
        }
    }
    $scope.dongtraloi = function () {
        $scope.formtraloi.center().close();
    }

    init();

})