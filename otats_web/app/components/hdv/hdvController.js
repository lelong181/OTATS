angular.module('app').controller('hdvController', function ($scope, $location, $state, $stateParams, $timeout, Notification, ComboboxDataService, hdvDataService, nhanVienDataService) {
    CreateSiteMap();
    function init() {
        getquyen();
        loadgrid();
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

    function listColumnsgrid() {
        var dataList = [];

        dataList.push({ headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, selectable: true, width: "40px" });
        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({
            field: "tenDangNhap", title: $.i18n('header_tendangnhap'),
            template: function (dataItem) {
                return "<a href='' class='color-primary' ng-click='openDetailFromGrid(" + kendo.htmlEncode(dataItem.idnv) + ")'>" + kendo.htmlEncode(dataItem.tenDangNhap) + "</a>";
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px"
        });
        dataList.push({ field: "tenDayDu", title: $.i18n('header_tendaydu'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "175px" });
        dataList.push({ field: "dienThoai", title: $.i18n('header_sodienthoai'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px" });
        dataList.push({ field: "email", title: "Email", headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px" });
        dataList.push({ field: "maTheHDV", title: "Mã thẻ HDV", headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px" });
        dataList.push({ field: "cccd", title: "CCCD", headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "120px" });

        dataList.push({
            title: $.i18n('header_thaotac'), attributes: { style: "text-align: center" },
            template: function (dataItem) {
                if (dataItem.trangThai == 0) {
                    return '<button ng-click="active(' + kendo.htmlEncode(dataItem.id) + ')" class="btn btn-outline-info btn-menubar" title ="Active" ><i class="fas fa-unlock"></i> Kích hoạt</button>';
                } else {
                    return '<button ng-click="inactive(' + kendo.htmlEncode(dataItem.id) + ')" class="btn btn-outline-info btn-menubar" title ="Inactive" ><i class="fas fa-lock"></i> Khoá tài khoản</button>';
                }
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: false, width: "150px"
        });

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

        hdvDataService.getlist().then(function (result) {
            let data = result.data;

            $scope.gridData = {
                data: data,
                schema: {
                    model: {
                        id: 'id'
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
            msg = $, i18n("label_canchonmotnhanviendethuchien");
            flag = false;
        } else if (listRowsSelected.length == 0) {
            flag = false;
            msg = $.i18n("label_chuachonnhanviendethuchien");
        }
        if (!flag) {
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n(msg) }, "error");
        }
        return flag;
    }
    function openFormResetPass(id) {
        if (id > 0) {
            $scope.objectpass = {
                idnhanvien: id,
                pass: '',
                repass: ''
            }
            $scope.formresetpass.center().open();
        } else {
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_loinhanvienvuilongchonlai') }, "error");
        }

    }

    function validateresetpass() {
        let flag = true;
        let msg = '';

        if ($scope.objectpass.pass == '' || $scope.objectpass.pass == undefined) {
            flag = false;
            msg = $.i18n('label_matkhaukhongduocdetrong');
            $("#pass").focus();
        }

        if (flag && ($scope.objectpass.repass == '' || $scope.objectpass.repass == undefined)) {
            flag = false;
            msg = $.i18n('label_xacnhanmatkhaukhongduocdetrong');
            $("#repass").focus();
        }

        if (flag && $scope.objectpass.pass.length < 8) {
            flag = false;
            msg = $.i18n('label_matkhaucododaiitnhat8kytu');
            $("#pass").focus();
        }

        if (flag && $scope.objectpass.pass != $scope.objectpass.repass) {
            flag = false;
            msg = $.i18n('label_xacnhanmatkhaukhongkhop');
            $("#repass").focus();
        }

        if (!flag)
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n(msg) }, 'warning');

        return flag;
    }

    function validatethemsuanhanvien() {
        let flag = true;
        let msg = '';

        if (flag && ($scope.objectNhanVien.TenDayDu == '' || $scope.objectNhanVien.TenDayDu == undefined)) {
            flag = false;
            msg = $.i18n('label_tendaydukhongduocdetrong');
            $("#tenDayDu").focus();
        }

        if (!flag)
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n(msg) }, 'warning');

        return flag;
    }

    let fileUpload = '';
    function onUploadExcelSuccess(e) {
        var data = new FormData();
        data.append('file', e.files[0].rawFile);
        var files = e.files[0];
        $.ajax({
            url: urlApi + '/api/uploadfile/savefileExcel',
            processData: false,
            contentType: false,
            data: data,
            type: 'POST'
        }).done(function (result) {
            fileUpload = result;
        }).fail(function (a, b, c) {
            fileUpload = '';
        });
    }

    $("#files").closest(".k-upload").find("span").text($.i18n("label_chontep"))

    //event

    function openFormDetail(id) {
        $scope.formnhanvien.open().center();
    }

    $scope.addNhanVien = function () {
        openFormDetail(0);
    }
    $scope.editNhanVien = function () {
        let listRowsSelected = commonGetRowSelected("#grid");
        if (validationOpenDetail(listRowsSelected)) {
            openFormDetail(listRowsSelected[0].id);
        }
    }
    $scope.deleteNhanVien = function () {
        let arr = $("#grid").data("kendoGrid").selectedKeyNames();
        if (arr.length <= 0)
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_chuachonnhanviendethuchien') }, 'warning');
        else {
            let data = [];
            for (let i = 0; i < arr.length; i++) {
                data.push(parseInt(arr[i]));
            }
            openConfirm($.i18n("label_bancochacchanxoanhomnhanvien"), 'apDungXoaNhanVien', null, data);
        }
    }

    $scope.apDungXoaNhanVien = function (data) {
        hdvDataService.del(data).then(function (result) {
            if (result.flag) {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                loadgrid();
            }
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
        });
    }

    $scope.luuNhanVien = function () {
        if (validatethemsuanhanvien()) {
            hdvDataService.saveinsertnhanvienkhongdangnhap($scope.objectNhanVien).then(function (result) {
                if (result.flag) {
                    $scope.formnhanvien.center().close();
                    loadgrid();
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                }
                else
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
            })
        }
    }
    $scope.huyLuuNhanVien = function () {
        $scope.formnhanvien.center().close();
    }

    $scope.datLaiMatKhau = function () {
        let listRowsSelected = commonGetRowSelected("#grid");
        console.log(listRowsSelected);
        if (validationOpenDetail(listRowsSelected)) {
            openFormResetPass(listRowsSelected[0].id);
        }
    }

    $scope.luuMatKhau = function () {
        if (validateresetpass()) {
            if ($scope.objectpass.idnhanvien > 0) {
                commonOpenLoadingText("#btn_luumatkhau");
                nhanVienDataService.resetpass($scope.objectpass.idnhanvien, $scope.objectpass.pass).then(function (result) {
                    if (result.flag) {
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
                        $scope.formresetpass.center().close();
                    }
                    else
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');

                    commonCloseLoadingText("#btn_luumatkhau");
                });
            }
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_loinhanvienvuilongchonlai') }, 'warning');
        }
    }
    $scope.huyLuuMatKhau = function () {
        $scope.formresetpass.center().close();
    }

    $scope.openDetailFromGrid = function (idnv) {
        openFormDetail(idnv);
    }

    $scope.active = function (id) {
        kendo.ui.progress($("#grid"), true);
        hdvDataService.activeHDV(id).then(function (result) {
            if (result.flag)
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
            loadgrid();
        });
    }

    $scope.inactive = function (id) {
        kendo.ui.progress($("#grid"), true);
        hdvDataService.inactiveHDV(id).then(function (result) {
            if (result.flag)
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'success');
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.message) }, 'warning');
            loadgrid();
        });
    }


    init();

    $("#grid").on("dblclick", "tr[role='row']", function () {
        let grid = $("#grid").data("kendoGrid");
        let row = grid.table.find("[data-uid=" + $(this).attr("data-uid") + "]");

        grid.clearSelection();
        grid.select(row);

        let selectedItem = grid.dataItem(grid.select());
        openFormDetail(selectedItem.id);
    })

})