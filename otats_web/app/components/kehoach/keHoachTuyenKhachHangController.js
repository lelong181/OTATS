angular.module('app', ["kendo.directives"]).controller('keHoachTuyenKhachHangController', function ($scope, $http, $location, $rootScope, Notification, keHoachTuyenKhachHangDataService) {
    CreateSiteMap();
    hideLoadingPage();


    $scope.init = function () {
        $scope.loadgrid();
        $scope.objTuyen = {
            Id: 0,
            TenTuyen: '',
            MoTa: '',
            DsNhanVien: [],
            DsNhom: [],
            DsKhachhang: [],
            LoaiTanSuat: 0,
            Ngay: [],
            Thu: [],
            NgayKetThuc: new Date()
        }
        $scope.ngayKetThucOptions = {
            min: new Date()
        }
    }

    $scope.listFieldsgrid = {
        iD_KhachHang: {
            type: "number"
        },
        maKH: {
            type: "string"
        }
    };
    $scope.listColumnsgrid = function () {
        var dataList = [];
        dataList.push({ headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, selectable: true, width: "25px" });
        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({ field: "tenTuyen", title: $.i18n('header_tentuyen'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({ field: "moTa", title: $.i18n('label_mota'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({ field: "soLuongKhachHang", title: $.i18n('header_soluongkhachhang'), attributes: { class: "text-center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({ field: "soLuongNhanVien", title: $.i18n('header_soluongnhanvien'), attributes: { class: "text-center" }, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({ field: "soLuongNhomNhanVien", title: $.i18n('header_soluongnhomnhanvien'), attributes: { class:"text-center"}, headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        //dataList.push({ field: "ghiChu", title: "Ghi chú", headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({
            title: $.i18n('header_bandotuyen'),
            template: function (e) {
                return '<button class="btn btn-link btn-menubar" ng-click="openMap(' + e.id + ')" title="' + $.i18n('header_bandotuyen') + '"><i class="fas fa-map-marked-alt fas-lg color-primary"></i></button>';
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, attributes: { class: "text-center" }, filterable: false, width: "60px"
        });

        return dataList;
    }
    $scope.listColumnsgridKhachHang = function () {
        var dataList = [];
        dataList.push({
            selectable: true,
            width: 40
        });
        dataList.push({
            title: "#", template: "#= ++RecordNumber #",
            width: "50px",
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            attributes: { class: "text-center" },
        });
        dataList.push({ field: "tenKhachHang", title: $.i18n('header_tenkhachhang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({ field: "maKH", title: $.i18n('header_makhachhang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({ field: "diaChi", title: $.i18n('header_diachi'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({ field: "tenTinh", title: $.i18n('header_tinhtp'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({ field: "tenQuan", title: $.i18n('header_quanhuyen'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({ field: "tenPhuong", title: $.i18n('header_phuongxa'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({ field: "dienThoai", title: $.i18n('header_dienthoai'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({ field: "email", title: $.i18n('header_email'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({ field: "tenLoaiKhachHang", title: $.i18n('header_loaikhachhang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({ field: "tenNhomKH", title: $.i18n('header_tennhomkhachhang'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        dataList.push({ field: "nguoiLienHe", title: $.i18n('header_nguoilienhe'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "150px" });
        return dataList;
        console.log(dataList);
    }
    $scope.loadgrid = function () {

        $scope.gridData = new kendo.data.DataSource({
            pageSize: 20,
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true,
            batch: false,
            requestStart: function (e) {
                kendo.ui.progress($("#tuyen_grid"), true);
                firstload = false;
            },
            requestEnd: function (e) {
                hideLoadingPage();
                kendo.ui.progress($("#tuyen_grid"), false);
            },
            transport: {
                read: {
                    url: urlApi + '/api/tuyenkhachhang/getalltuyen',
                    contentType: "application/json",
                    beforeSend: function (req) {
                        if ($rootScope.Authorization == undefined || $rootScope.Authorization == null || $rootScope.Authorization == "") {
                            var url = location.origin + '/login.aspx';
                            window.open(url, "_self");
                        } else {
                            req.setRequestHeader('Authorization', $rootScope.Authorization);
                            kendo.ui.progress($("#tuyen_grid"), false)
                        }
                    }
                },
                parameterMap: function (data, operation) {
                    if (operation == "read") {
                        if (data.filter) {
                            $.each(data.filter.filters, function (index, item) {
                                item.value = item.value.toLowerCase();
                            })
                        }
                        var param = {
                            request: data
                        }
                        return JSON.stringify(param);
                    }

                }
            },
            schema: {
                model: {
                    id: "id",
                },
                total: 'total',
                data: 'data'
            }

        });

        $scope.gridOptions = {
            excel: {
                filterable: true
            },
            excelExport: function (e) {
                var columns = e.workbook.sheets[0].columns;
                var sheet = e.workbook.sheets[0];
                sheet.title = $.i18n('label_danhsachtuyen');
                for (var rowIndex = 0; rowIndex < sheet.rows.length; rowIndex++) {
                    var row = sheet.rows[rowIndex];
                    var flag = false;
                    if (rowIndex == 0) {
                        flag = true;
                    }
                    for (var cellIndex = 0; cellIndex < row.cells.length; cellIndex++) {
                        if (flag) {
                            row.cells[cellIndex].textAlign = 'center';
                            row.cells[cellIndex].bold = true;
                        }
                        row.cells[cellIndex].borderBottom = { color: "#000", size: 1 };
                        row.cells[cellIndex].borderTop = { color: "#000", size: 1 };
                        row.cells[cellIndex].borderRight = { color: "#000", size: 1 };
                        row.cells[cellIndex].borderleft = { color: "#000", size: 1 };
                    }
                }
            },
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".toolbarmenu").height());
                return heightGrid - 70;
            },
            dataBinding: function () {
                RecordNumber = (this.dataSource.page() - 1) * this.dataSource.pageSize();
            },
            persistSelection: true,
            resizable: true,
            pageable: $scope.lang == 'vi-vn' ? pageableShort_vi : pageableShort_en,
            sortable: true,
            filterable: filterable,
            scrollable: true,
            autoFitColumn: true,
            columns: $scope.listColumnsgrid()
        };

        $scope.gridKhachHangTrongTuyenData = new kendo.data.DataSource({
            data: [
            ],
            schema: {
                model: {
                    children: "iD_KhachHang"
                }
            },
            pageSize: 20
        });
        $scope.gridKhachHangTrongTuyenOptions = {
            dataBinding: function () {
                RecordNumber = (this.dataSource.page() - 1) * this.dataSource.pageSize();
            },
            height: function () {
                var heightGrid = $(document).height() - 270;
                return heightGrid < 100 ? 500 : heightGrid;
            },
            persistSelection: true,
            resizable: true,
            pageable: $scope.lang == 'vi-vn' ? pageableShort_vi : pageableShort_en,
            sortable: true,
            filterable: filterable,
            scrollable: true,
            autoFitColumn: true,
            columns: $scope.listColumnsgridKhachHang()
        }

        $scope.gridKhachHangTrongNgoaiData = new kendo.data.DataSource({
            pageSize: 20,
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true,
            batch: false,
            requestStart: function (e) {
            },
            requestEnd: function (e) {
                if ($scope.GridKhachHangTrongTuyen.dataSource != undefined) {
                    var dataSource_KhachDaChon = $scope.GridKhachHangTrongTuyen.dataSource.data();
                    $scope.GridKhachHangNgoaiTuyen.clearSelection();
                    $.each(dataSource_KhachDaChon, function (index, item) {
                        var dataSourceKhach = $scope.GridKhachHangNgoaiTuyen.dataSource;
                        dataitem = dataSourceKhach.get(item.iD_KhachHang);
                        if (dataitem != undefined) {
                            var row = $scope.GridKhachHangNgoaiTuyen.table.find("[data-uid=" + dataitem.uid + "]");
                            $scope.GridKhachHangNgoaiTuyen.select(row);
                        }
                    })
                }
            },
            transport: {
                read: {
                    url: urlApi + '/api/khachhang/getallKendobyNhanVienDT',
                    contentType: "application/json",
                    beforeSend: function (req) {
                        if ($rootScope.Authorization == undefined || $rootScope.Authorization == null || $rootScope.Authorization == "") {
                            var url = location.origin + '/login.aspx';
                            window.open(url, "_self");
                        } else {
                            req.setRequestHeader('Authorization', $rootScope.Authorization);
                        }

                    }
                },
                parameterMap: function (data, operation) {
                    if (operation == "read") {
                        if (data.filter) {
                            $.each(data.filter.filters, function (index, item) {
                                item.value = item.value.toLowerCase();
                            })
                        }
                        var param = {
                            request: data,
                            ID_NhanViens: $scope.NhanVien.value().toString()
                        }
                        return JSON.stringify(param);
                    }

                }
            },
            schema: {
                model: {
                    id: "iD_KhachHang",
                },
                total: 'total',
                data: 'data'
            }

        })
        $scope.gridKhachHangNgoaiTuyenOptions = {
            dataBinding: function () {
                RecordNumber = (this.dataSource.page() - 1) * this.dataSource.pageSize();
            },
            height: function () {
                var heightGrid = $(document).height() - 230;
                return heightGrid < 100 ? 500 : heightGrid;
            },
            persistSelection: true,
            resizable: true,
            pageable: $scope.lang == 'vi-vn' ? pageableShort_vi : pageableShort_en,
            sortable: true,
            filterable: filterable,
            scrollable: true,
            autoFitColumn: true,
            columns: $scope.listColumnsgridKhachHang()
        }
        hideLoadingPage();
    }

    $scope.init();

    $scope.exportExcelTuyen = function () {

        keHoachTuyenKhachHangDataService.getExcelTuyenKhachHang().then(function (result) {
            if (result.flag)
                commonDownFile(result.data);
            else
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_coloixayra_loadlaitrang') }, 'warning');

            commonCloseLoadingText("#btn_xuatexcel")
        });
        //kendo.ui.progress($("html"), true);
        //$scope.grid.options.excel = {
        //    fileName: "DSTuyen" + kendo.toString(new Date, "yyyy-MM-dd") + ".xlsx",
        //    filterable: true,
        //    allPages: true
        //}
        //$scope.grid.saveAsExcel();
        //setTimeout(function () {
        //    kendo.ui.progress($("html"), false);
        //}, 4000)

    }

    $scope.LoadDSTuyen = function () {
        kendo.ui.progress($("#tuyen_grid"), true);
        var page = $scope.grid.dataSource.page();
        $scope.grid.refresh();
        $scope.grid.dataSource.page(page);
        kendo.ui.progress($("#tuyen_grid"), false);
    }

    $scope.closeWindowTuyen = function () {
        document.getElementById("tuyen_formTuyen").reset();
        //DisableValidate("tuyen_TenTuyen");
        $("#tuyen_windowTuyen").data("kendoWindow").close();
    }

    $scope.openCreateTuyenWindow = function () {
        //document.getElementById("tuyen_formTuyen").reset();
        $("#tuyen_windowTuyen").data("kendoWindow").close();
        $scope.objTuyen = {
            Id: 0,
            TenTuyen: '',
            MoTa: '',
            DsNhanVien: [],
            DsNhom: [],
            DsKhachhang: [],
            LoaiTanSuat: '',
            Ngay: [],
            Thu: [],
            NgayKetThuc: new Date()
        }
        let dataSourceGridKhachHangTrongTuyen = new kendo.data.DataSource({
            data: [
            ],
            schema: {
                model: {
                    id: "iD_KhachHang"
                }
            },
            pageSize: 20
        });
        console.log($("#LoaiTanSuat").data("kendoComboBox").value(''));
        $scope.GridKhachHangTrongTuyen.setDataSource(dataSourceGridKhachHangTrongTuyen);
        $("#tuyen_windowTuyen").data("kendoWindow").maximize().center().open();
        $("#ngayketthuc").on("keydown", function (e) {
            e.preventDefault();
        });
    }

    function validate() {
        let flag = true;
        let msg = '';
        if ($scope.objTuyen.TenTuyen == '' || $scope.objTuyen.TenTuyen == undefined) {
            flag = false;
            msg = $.i18n('label_tentuyenkhongduocbotrong');
            $("#TenTuyen").focus();
        }

        if (!flag)
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n(msg) }, 'warning');

        return flag;
    }

    $scope.LuuTuyen = function () {
        if (validate()) {
            var dskhachhang = [];
            var dataSource_KhachDaChon = $scope.GridKhachHangTrongTuyen.dataSource.data();
            var ngayselect = $("#tuyen_NgayViengTham").data("kendoMultiDateSelect").values();
            $.each(dataSource_KhachDaChon, function (index, item) {
                dskhachhang.push(item.iD_KhachHang)
            })
            var ngay = [];
            $.each(ngayselect, function (index, item) {
                var sdate = item.getFullYear() + "-" + parseInt(item.getMonth() + 1) + "-" + item.getDate() + " 00:00:00";
                ngay.push(sdate);
            })
            var thu = $scope.objTuyen.Thu;
            //$.each($scope.objTuyen.Thu, function (index, item) {
            //    thu.push(item.value);
            //})
            var data = $scope.objTuyen;
            data.NgayKetThuc = kendo.toString($scope.objTuyen.NgayKetThuc,'yyyy/MM/ddT12:00:00')
            data.Ngay = ngay;
            data.DsKhachhang = dskhachhang;
            data.Thu = thu;
            kendo.ui.progress($("#tuyen_windowTuyen"), true);
            keHoachTuyenKhachHangDataService.themSuaTuyen(data).then(function (response) {
                if (response.data) {
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(response.data.msg) }, "success");
                    $scope.LoadDanhSachTuyen();
                    $scope.closeWindow();
                } else {
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(response.data.msg) }, "error");
                }
                dataSourceGridKhachHangTrongTuyen = new kendo.data.DataSource({
                    data: [
                    ],
                    schema: {
                        model: {
                            id: "iD_KhachHang"
                        }
                    },
                    pageSize: 20
                });
                kendo.ui.progress($("#tuyen_windowTuyen"), false);
            });
        }
    }

    function openConfirm(message, acceptAction, cancelAction, data) {
        var scope = angular.element("#mainContentId").scope();
        $(" <div id='confirmDelete'></div>").appendTo("body").kendoDialog({
            width: "450px",
            closable: true,
            modal: true,
            title: $.i18n('label_xacnhan'),
            content: message,
            actions: [
                {
                    text: $.i18n('button_huy'), primary: false, action: function () {
                        if (cancelAction != null) {
                            scope[cancelAction](data);
                        }
                    }
                },
                {
                    text: $.i18n('button_dongy'), primary: true, action: function () {
                        scope[acceptAction](data);
                    }
                }
            ],
        })
    }

    $scope.deleteItemTuyen = function () {
        var arr = $("#tuyen_grid").data("kendoGrid").selectedKeyNames();
        if (arr.length < 1) {
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_vuilongchonmotbanghidexoa') }, "error");
            //openDialog(dialogRoot, "Vui lòng chọn ít nhất 1 bản ghi để xóa");
        } else {
            //openConfirm(dialogRoot, "Bạn có chắc chắn muốn xóa thông tin tuyến?", XoaTuyen, function () { });
            var data = [];
            for (var i = 0; i < arr.length; i++) {
                data.push(parseInt(arr[i]));
            }
            openConfirm($.i18n('label_bancochacchanmuonxoathongtintuyen')
                , 'XoaTuyen', null, data);
        }
    }

    $scope.XoaTuyen = function (data) {
        $http({
            method: 'POST',
            url: urlApi + '/api/tuyenkhachhang/deleteTuyen',
            data: data
        }).then(function successCallback(response) {
            if (response.data) {
                Notification({
                    title: $.i18n('label_thongbao'), message: $.i18n("label_xoathanhcongtuyen") }, "success");
                //notification.show({ kValue: "Xóa thành công " + arr.length + " tuyến" }, "success");
                $scope.LoadDSTuyen();
            } else {
                Notification({ title: $.i18n('label_xoathatbaixinvuilongthulai') }, "error");

            }
        });
        $("#tuyen_grid").data("kendoGrid").clearSelection();
    }

    $scope.XoaKhachDaChon = function () {
        kendo.ui.progress($("#tuyen_windowTuyen"), true);
        var arr = $scope.GridKhachHangTrongTuyen.selectedKeyNames();
        var grid = $scope.GridKhachHangTrongTuyen;
        var rows = grid.select();
        rows.each(function (index, row) {
            var selectedItem = grid.dataItem(row);
            $scope.GridKhachHangTrongTuyen.removeRow(row);
        })
        kendo.ui.progress($("#tuyen_windowTuyen"), false);
    }

    $scope.LoadDanhSachTuyen = function () {
        kendo.ui.progress($("#tuyen_grid"), true);
        var page = $("#tuyen_grid").data('kendoGrid').dataSource.page();
        $("#tuyen_grid").data("kendoGrid").refresh();
        $("#tuyen_grid").data('kendoGrid').dataSource.page(page);
        kendo.ui.progress($("#tuyen_grid"), false);
    }

    $scope.closeWindow = function () {
        //document.getElementById("tuyen_formTuyen").reset();
        //DisableValidate("tuyen_TenTuyen");
        //$("#preview[page='khachhang']").html('');
        $("#tuyen_windowTuyen").data("kendoWindow").close();
        $scope.objTuyen = {
            Id: 0,
            TenTuyen: '',
            MoTa: '',
            DsNhanVien: [],
            DsNhom: [],
            DsKhachhang: [],
            LoaiTanSuat: 0,
            Ngay: [],
            Thu: [],
            NgayKetThuc: new Date()
        }
        let dataSourceGridKhachHangTrongTuyen = new kendo.data.DataSource({
            data: [
            ],
            schema: {
                model: {
                    id: "iD_KhachHang"
                }
            },
            pageSize: 20
        });
        $scope.GridKhachHangTrongTuyen.setDataSource(dataSourceGridKhachHangTrongTuyen);
    }

    $scope.lstNgayViengThamTuan = [];

    $scope.loaiTanSuatOnChange = function () {
        $scope.NgayViengThamTuan.value([]);
        var type = $scope.objTuyen.LoaiTanSuat;
        if (type == 2) {// theo tuần
            $scope.lstNgayViengThamTuan = $scope.dataSourceTuan;
        } else if (type == 3) {// theo tháng
            $scope.lstNgayViengThamTuan = $scope.dataSourceThang;
        }
    }

    $("#tuyen_WindowMap").kendoWindow({
        modal: true,
        width: "968px",
        height: "596px",
        title: $.i18n('header_bandotuyen'),
        visible: false,
        actions: [
            "Close"
        ],
        open: function () {

        }
    });

    $scope.openMap = function(id) {
        $("#tuyen_WindowMap").data("kendoWindow").center().maximize().open();
        if ($scope.maptuyen == undefined) {
            intMap();
        } else {

        }
        $http({
            url: urlApi + '/api/tuyenkhachhang/getpoints?ID=' + id,
            beforeSend: function (req) {
                req.setRequestHeader('Authorization', token);
            }
        }).then(function successCallback(response) {
            result = response.data;
            waypts = [];
            var startpoint = new google.maps.LatLng(result[0].lat, result[0].lon);
            var endpoint = new google.maps.LatLng(result[result.length - 1].lat, result[result.length - 1].lon)
            for (var i = 1; i < result.length - 1; i++) {
                if (result[i].lon > 0 && result[i].lat > 0) {
                    console.log(result[i].lat + " - " + result[i].lon);
                    waypts.push({
                        location: new google.maps.LatLng(result[i].lat, result[i].lon),
                        stopover: true
                    });
                }
            }

            directionsService.route({
                origin: startpoint,
                destination: endpoint,
                waypoints: waypts,
                optimizeWaypoints: true,
                travelMode: 'DRIVING'
            }, function (response, status) {
                if (status === 'OK') {
                    directionsDisplay.setDirections(response);
                    var route = response.routes[0];
                } else {
                    //openDialog(dialogRoot, "Không thể tìm vị trí khách hàng");
                    Notification({ title: $.i18n('label_khongthetimvitrikhachhang') }, "error");
                }
            });

        })
    }

    function intMap() {
        directionsService = new google.maps.DirectionsService;
        directionsDisplay = new google.maps.DirectionsRenderer;
        $scope.maptuyen = new google.maps.Map(document.getElementById('MapTuyen'), {
            zoom: 6,
            center: { lat: 20.975761, lng: 105.835959 }
        });
        directionsDisplay.setMap($scope.maptuyen);

    }

    $scope.dataSourceThang = new kendo.data.DataSource({
        data: [
            { text: "Ngày 1", value: 1 },
            { text: "Ngày 2", value: 2 },
            { text: "Ngày 3", value: 3 },
            { text: "Ngày 4", value: 4 },
            { text: "Ngày 5", value: 5 },
            { text: "Ngày 6", value: 6 },
            { text: "Ngày 7", value: 7 },
            { text: "Ngày 8", value: 8 },
            { text: "Ngày 9", value: 9 },
            { text: "Ngày 10", value: 10 },
            { text: "Ngày 11", value: 11 },
            { text: "Ngày 12", value: 12 },
            { text: "Ngày 13", value: 13 },
            { text: "Ngày 14", value: 14 },
            { text: "Ngày 15", value: 15 },
            { text: "Ngày 16", value: 16 },
            { text: "Ngày 17", value: 17 },
            { text: "Ngày 18", value: 18 },
            { text: "Ngày 19", value: 19 },
            { text: "Ngày 20", value: 20 },
            { text: "Ngày 21", value: 21 },
            { text: "Ngày 22", value: 22 },
            { text: "Ngày 23", value: 23 },
            { text: "Ngày 24", value: 24 },
            { text: "Ngày 25", value: 25 },
            { text: "Ngày 26", value: 26 },
            { text: "Ngày 27", value: 27 },
            { text: "Ngày 28", value: 28 },
            { text: "Ngày 29", value: 29 },
            { text: "Ngày 30", value: 30 },
            { text: "Ngày 31", value: 31 },
        ]
    })

    $scope.dataSourceTuan = new kendo.data.DataSource({
        data: [
            { text: "Thứ 2", value: 2 },
            { text: "Thứ 3", value: 3 },
            { text: "Thứ 4", value: 4 },
            { text: "Thứ 5", value: 5 },
            { text: "Thứ 6", value: 6 },
            { text: "Thứ 7", value: 7 },
            { text: "Chủ nhật", value: 1 }
        ]
    })

    $scope.lstLoaiTanSuat = new kendo.data.DataSource({
        data: [
            { tenKieu: $.i18n('label_tansuattheotuan'), kieu: 2 },
            { tenKieu: $.i18n('label_tansuattheothang'), kieu: 3 }
        ]
    })


    var myWindow = $("#tuyen_windowTuyen");
    myWindow.kendoWindow({
        modal: true,
        width: "968px",
        height: "596px",
        title: $.i18n('label_thongtintuyenkhachhang'),
        visible: false,
        draggable: false,
        actions: [
            "Close"
        ],
        open: function () {

            if ($('#tuyen_NgayViengTham').data("kendoMultiDateSelect") == undefined) {
                var multiDateSelect = $('#tuyen_NgayViengTham').kendoMultiDateSelect({
                    autoClose: false,
                    min: new Date(),
                }).data('kendoMultiDateSelect');
            }
            $("#tuyen_NgayViengTham").data("kendoMultiDateSelect").values([]);
            arr_ngay = [];
            //if ($scope.GridKhachHangTrongTuyen == undefined) {
            //    CreateGridKhachHangTrongTuyen();
            //}

        }
    })

    var WindowChonKhachHang = $("#tuyen_WindowChonKhachHang");
    if ($("#tuyen_WindowChonKhachHang").data("kendoWindow") == undefined) {
        WindowChonKhachHang.kendoWindow({
            width: "968px",
            height: "500px",
            title: $.i18n('label_chonkhachhangtrongtuyen'),
            visible: false,
            modal: true,
            open: function () {
                var page = $scope.GridKhachHangNgoaiTuyen.dataSource.page();
                $scope.GridKhachHangNgoaiTuyen.refresh();
                $scope.GridKhachHangNgoaiTuyen.dataSource.page(page);
            },
            actions: [
                "Close"
            ]
        }).data("kendoWindow");
    }

    $scope.openWindowSelect_KhachHang = function () {
        $("#tuyen_WindowChonKhachHang").data("kendoWindow").center().open();
    }
    $scope.openEditTuyenWindow = function () {
        var arr = $("#tuyen_grid").data("kendoGrid").selectedKeyNames();
        console.log(arr);
        if (arr.length != 1) {
            Notification({ title: $.i18n('label_vuilongchonmotbanghidesua') }, "error");
            //openDialog(dialogRoot, "Vui lòng chọn 1 bản ghi để sửa");
        } else {
            arr_ngay = [];
            $http({
                url: urlApi + '/api/tuyenkhachhang/getbyid?ID=' + arr[0],
                method: 'GET',
                beforeSend: function () {
                    if ($rootScope.Authorization == undefined || $rootScope.Authorization == null || $rootScope.Authorization == "") {
                        var url = location.origin + '/login.aspx';
                        window.open(url, "_self");
                    }
                }
            }).then(function successCallback(response) {
                $scope.objTuyen = {
                    Id: response.data.tuyen.id,
                    TenTuyen: response.data.tuyen.tenTuyen,
                    MoTa: response.data.tuyen.moTa,
                    DsNhanVien: response.data.dsnhanvien,
                    DsNhom: response.data.dsnhom,
                    DsKhachhang: response.data.dskhachhang,
                    LoaiTanSuat: response.data.lichviengtham.loaiTanSuat,
                    Ngay: response.data.lichviengtham.lichViengTham,
                    Thu: response.data.lichviengtham.cacNgayThucHien,
                    NgayKetThuc: response.data.lichviengtham.ngayKetThuc
                }

                $scope.NhomNhanVien.value(response.data.dsnhom);
                $scope.NhanVien.setDataSource(new kendo.data.DataSource({
                    data: []
                }))
                $http({
                    method: 'POST',
                    url: urlApi + '/api/nhanvienapp/getallbynhom',
                    data: {
                        IDNhom: response.data.dsnhom.join()
                    }
                }).then(function successCallback(dsNhom) {
                    var dataSourceNhanVien = new kendo.data.DataSource({
                        data: dsNhom.data
                    })
                    $scope.NhanVien.setDataSource(dataSourceNhanVien);
                    if (response.data.dsnhanvien.length > 0) {
                        $scope.NhanVien.value(response.data.dsnhanvien);
                    }
                });
                dataSourceGridKhachHangTrongTuyen = new kendo.data.DataSource({
                    data: [
                    ],
                    schema: {
                        model: {
                            id: "iD_KhachHang"
                        }
                    },
                    pageSize: 20
                });
                $.each(response.data.dskhachhang, function (index, item) {
                    dataSourceGridKhachHangTrongTuyen.add(item);
                });
                $("#tuyen_windowTuyen").data("kendoWindow").maximize().center().open();
                $("#ngayketthuc").on("keydown", function (e) {
                    e.preventDefault();
                });
                $scope.GridKhachHangTrongTuyen.setDataSource(dataSourceGridKhachHangTrongTuyen);

                if (response.data.lichviengtham.loaiTanSuat > 0) {
                    $scope.LoaiTanSuat.value(response.data.lichviengtham.loaiTanSuat);
                }
                if (response.data.lichviengtham.loaiTanSuat == 2) {// theo tuần
                    $scope.lstNgayViengThamTuan = $scope.dataSourceTuan;
                } else if (response.data.lichviengtham.loaiTanSuat == 3) {// theo tháng
                    $scope.lstNgayViengThamTuan = $scope.dataSourceThang;
                }
                $scope.objTuyen.Thu = response.data.lichviengtham.cacNgayThucHien;
                arr_ngay = [];
                $.each(response.data.lichviengtham.lichViengTham, function (index, item) {
                    arr_ngay.push(new Date(item));
                })
                $("#tuyen_NgayViengTham").data("kendoMultiDateSelect").value(arr_ngay);

            })
        }
    }

    $scope.closeWindowChonKhachHang = function () {
        $("#tuyen_WindowChonKhachHang").data("kendoWindow").close();
    }
    dataSourceGridKhachHangTrongTuyen = new kendo.data.DataSource({
        data: [
        ],
        schema: {
            model: {
                id: "iD_KhachHang"
            }
        },
        pageSize: 20
    });
    $scope.ChonKhachHang = function () {

        var gridselect = $scope.GridKhachHangNgoaiTuyen;
        console.log();
        var uid_arr = [];
        $.each(dataSourceGridKhachHangTrongTuyen.data(), function (index, item) {
            console.log(item);

            uid_arr.push(item.iD_KhachHang);
        })
        gridselect.select().each(function () {
            var dataItem = gridselect.dataItem(this);
            if (uid_arr.indexOf(dataItem.iD_KhachHang) < 0) {
                uid_arr.push(dataItem.iD_KhachHang);
                dataSourceGridKhachHangTrongTuyen.add(dataItem);
            }
        })
        $scope.GridKhachHangTrongTuyen.setDataSource(dataSourceGridKhachHangTrongTuyen);
        var gridd = $scope.GridKhachHangTrongTuyen;
        for (var i = 0; i < gridd.columns.length; i++) {
            if (!gridd.columns[i].selectable) {
                gridd.autoFitColumn(gridd.columns[i]);
            }
        }
        $("#tuyen_WindowChonKhachHang").data("kendoWindow").close();
    }



    // Khai báo combo nhóm

    $http({
        method: 'GET',
        url: urlApi + '/api/nhomnhanvien/getallcombox'
    }).then(function successCallback(dsNhom) {
        var dataSourceNhom = new kendo.data.DataSource({
            data: dsNhom.data
        })
        //if ($("#tuyen_NhomNhanVien").data("kendoMultiSelect") == undefined) {
        //    $("#tuyen_NhomNhanVien").kendoMultiSelect({
        //        dataTextField: "tenNhom",
        //        dataValueField: "iD_Nhom",
        //        dataSource: dataSourceNhom,
        //        autoBind: true,
        //        autoClose: false,
        //        close: onChange_tuyen_NhomNhanVien,
        //        footerTemplate: '<a onclick="tuyen_NhomNhanVien_SelectAll()" class="btn-multiselect-selectall">Chọn tất cả</a>'
        //    })
        //}
        $scope.optionNhomNhanVien = {
            height: 300,
            footerTemplate: '<a ng-click="tuyen_NhomNhanVien_SelectAll()" class="btn-multiselect-selectall">Chọn tất cả</a>'
        }
        $scope.lstNhomNhanVien = dataSourceNhom;
    });

    $scope.optionNhanVien = {
        autoBind: true,
        autoClose: false,
        height: 300,
        footerTemplate: '<a ng-click="tuyen_NhanVien_SelectAll()" class="btn-multiselect-selectall">Chọn tất cả</a>'
    }

    $scope.optionNgayViengThamTuan = {
        autoBind: true,
        autoClose: false,
        height: 300
    }

    $scope.tuyen_NhomNhanVien_SelectAll = function () {
        var multi_nhom = $scope.NhomNhanVien;
        var values = $.map(multi_nhom.dataSource.data(), function (dataItem) {
            return dataItem.iD_Nhom;
        });
        multi_nhom.value(values);
    }

    $scope.tuyen_NhanVien_SelectAll = function () {
        var multi_nv = $scope.NhanVien;
        var values = $.map(multi_nv.dataSource.data(), function (dataItem) {
            return dataItem.idnv;
        });
        multi_nv.value(values);
    }


    $scope.nhomNhanVienOnChange = function (arg) {
        var curr_val_tuyen = $scope.NhanVien.value();
        $scope.NhanVien.setDataSource(new kendo.data.DataSource({
            data: []
        }))
        if (arg.sender.value().length != $scope.NhomNhanVien.dataSource.data().length) {
            $http({
                method: 'POST',
                url: urlApi + '/api/nhanvienapp/getallbynhom',
                data: {
                    IDNhom: arg.sender.value().join()
                }
            }).then(function successCallback(dsNhom) {
                var dataSourceNhanVien = new kendo.data.DataSource({
                    data: dsNhom.data
                })
                $scope.NhanVien.setDataSource(dataSourceNhanVien);
                $scope.NhanVien.value(curr_val_tuyen);
            })
        } else {
            $http({
                method: 'POST',
                url: urlApi + '/api/nhanvienapp/getallbynhom',
                data: {
                    IDNhom: arg.sender.value().join()
                }
            }).then(function successCallback(dsNhom) {
                var dataSourceNhanVien = new kendo.data.DataSource({
                    data: dsNhom
                })
                $scope.NhanVien.setDataSource(dataSourceNhanVien);
                $scope.NhanVien.value(curr_val_tuyen);
            });
        }

    }


    $("#tuyen_grid").on("dblclick", "tr[role='row']", function () {
        $("#tuyen_grid").data("kendoGrid").clearSelection();
        var row = $("#tuyen_grid").data("kendoGrid").table.find("[data-uid=" + $(this).attr("data-uid") + "]");
        $("#tuyen_grid").data("kendoGrid").select(row);
        $scope.openEditTuyenWindow();
    })
})