var Authorization;
var permission = { them: 1, sua: 1, xoa: 1 }
angular.module('app').controller('keHoachNhanVienController', function ($scope, $http, $location, $rootScope, Notification, ComboboxDataService) {
    Authorization = $rootScope.Authorization;
    dialogRoot = $("#kehoachnhanvien_dialogRoot").kendoDialog().data("kendoDialog").close();
    CreateSiteMap();
    hideLoadingPage();
    var editItem = null;


    $scope.init = function () {
        getquyen();

        $scope.schedulerOptions = {
            date: new Date(),
            startTime: new Date("2013/1/1 00:00 AM"),
            height: function () {
                var heightGrid = $(document).height() - 150;
                //console.log(heightGrid);
                return 550;
            },
            footer: {
                command: false
            },
            views: [
                "day",
                "week",
                { type: "month", selected: true }
            ],
            //timezone: "Etc/UTC",
            eventTemplate: $("#eventschedule_template").html(),
            dataSource: {
                offlineStorage: null,
                serverSorting: false,
                serverPaging: false,
                serverFiltering: false,
                serverGrouping: false,
                serverAggregates: false,
                batch: true,
                inPlaceSort: false,
                transport: {
                    read: {
                        url: urlApi + '/api/kehoachnhanvien/getkehoachbynhanvien',
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
                    update: {
                        url: urlApi + 'api/kehoachnhanvien/update',
                        dataType: "json",
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
                    create: {
                        url: urlApi + '/api/kehoachnhanvien/create',
                        dataType: "json",
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
                    destroy: {
                        url: urlApi + '/api/kehoachnhanvien/delete',
                        dataType: "json",
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
                    parameterMap: function (options, operation) {
                        if (operation == "create") {
                            var batdau = $("#kehoachnv_giocheckin").data("kendoDateTimePicker").value();
                            var batdaus = batdau.getFullYear() + "-" + parseInt(batdau.getMonth() + 1) + "-" + batdau.getDate() + " " + batdau.getHours() + ":" + batdau.getMinutes() + ":00";
                            var ketthuc = $("#kehoachnv_giocheckout").data("kendoDateTimePicker").value();
                            var ketthucs = ketthuc.getFullYear() + "-" + parseInt(ketthuc.getMonth() + 1) + "-" + ketthuc.getDate() + " " + ketthuc.getHours() + ":" + ketthuc.getMinutes() + ":00";
                            if ($("#kehoachnv_khachhang").data("kendoComboBox").value() == "") {
                                //notification.show({ kValue: "Bạn cần chọn khách hàng" }, "error");
                                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_bancanchonkhachhang') }, 'error');

                                return null;
                            }
                            if (batdau > ketthuc) {
                                //notification.show({ kValue: "Lập kế hoạch không thành công, thời gian không phù hợp" }, "error");
                                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_lapkehoachkhongthanhcongthoigiankhongphuhop') }, 'error');

                                return null;
                            }
                            var data = {
                                ID: 0,
                                ID_NhanVien: $("#kehoachnv_nhanvien").data("kendoComboBox").value(),
                                ID_KhachHang: $("#kehoachnv_khachhang").data("kendoComboBox").value(),
                                //Ngay: $("#kehoachnv_ngay").data("kendoDatePicker").value(),
                                Ngay: batdaus,
                                BatDau: batdaus,
                                KetThuc: ketthucs,
                                ViecCanLam: $("#kehoachnv_vieccanlam").val(),
                                GhiChu: $("#kehoachnv_ghichu").val()
                            }
                            showLoadingPage();
                            return $scope.permission.them == 1 ? kendo.stringify(data) : null;
                        }
                        else if (operation == "update") {
                            if ($scope.permission.sua != 1) {
                                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_bankhongcoquyenthuchienthaotac') }, 'error');
                            } else {
                                var batdau = $("#kehoachnv_giocheckin").data("kendoDateTimePicker").value();
                                var batdaus = batdau.getFullYear() + "-" + parseInt(batdau.getMonth() + 1) + "-" + batdau.getDate() + " " + batdau.getHours() + ":" + batdau.getMinutes() + ":00";
                                var ketthuc = $("#kehoachnv_giocheckout").data("kendoDateTimePicker").value();
                                var ketthucs = ketthuc.getFullYear() + "-" + parseInt(ketthuc.getMonth() + 1) + "-" + ketthuc.getDate() + " " + ketthuc.getHours() + ":" + ketthuc.getMinutes() + ":00";
                                if ($("#kehoachnv_khachhang").data("kendoComboBox").value() < 0) {
                                    //notification.show({ kValue: "Bạn cần chọn khách hàng" }, "error");
                                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_bancanchonkhachhang') }, 'error');

                                    return null;
                                }
                                var data = {
                                    ID: editItem.idKeHoach,
                                    ID_NhanVien: $("#kehoachnv_nhanvien").data("kendoComboBox").value(),
                                    ID_KhachHang: $("#kehoachnv_khachhang").data("kendoComboBox").value(),
                                    //Ngay: $("#kehoachnv_ngay").data("kendoDatePicker").value(),
                                    Ngay: batdaus,
                                    BatDau: batdaus,
                                    KetThuc: ketthucs,
                                    ViecCanLam: $("#kehoachnv_vieccanlam").val(),
                                    GhiChu: $("#kehoachnv_ghichu").val()
                                }
                            }
                            return $scope.permission.sua == 1 ? kendo.stringify(data) : null;
                        }
                        else if (operation == "destroy") {
                            if ($scope.permission.xoa != 1) {
                                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_bankhongcoquyenthuchienthaotac') }, 'error');
                            } else {
                                var data = {
                                    ID: editItem.idKeHoach,
                                    ID_NhanVien: 0,
                                    ID_KhachHang: 0,
                                    Ngay: new Date(),
                                    BatDau: new Date(),
                                    KetThuc: new Date(),
                                    ViecCanLam: "",
                                    GhiChu: ""
                                }
                                if (editItem.start > new Date()) {

                                    return $scope.permission.xoa == 1 ? kendo.stringify(data) : null;
                                } else {
                                    //notification.show({ kValue: "Bạn không thể xóa kế hoạch này" }, "error");
                                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_bankhongthexoakehoachnay') }, 'error');

                                    data.ID = 0;
                                    return $scope.permission.xoa == 1 ? kendo.stringify(data) : null;
                                }
                            }
                        }
                        else if (operation == "read") {
                            var view = $scope.scheduler.view();
                            var idnhanvien = 0;
                            if ($("#kehoachnhanvien_idnv").data("kendoComboBox") != undefined) {
                                idnhanvien = $("#kehoachnhanvien_idnv").data("kendoComboBox").value();
                            }
                            var batdau = view.startDate();
                            var batdaus = batdau.getFullYear() + "-" + parseInt(batdau.getMonth() + 1) + "-" + batdau.getDate() + " 00:00:00";
                            var ketthuc = view.endDate();
                            var ketthucs = ketthuc.getFullYear() + "-" + parseInt(ketthuc.getMonth() + 1) + "-" + ketthuc.getDate() + " 23:59:00";
                            return JSON.stringify({
                                ID_NhanVien: idnhanvien,
                                start: batdaus,
                                end: ketthucs
                            });
                        }
                    }
                },
                requestEnd: function (e) {
                    //check the "response" argument to skip the local operations
                    hideLoadingPage();
                    if (e.type === "create" && e.response) {
                        if (e.response.success) {
                            Notification({ title: $.i18n('label_thongbao'), message: $.i18n(e.response.message) }, 'success');

                            //notification.show({ kValue: $.i18n(e.response.message) }, "success");
                        } else {
                            //notification.show({ kValue: $.i18n(e.response.message) }, "error");
                            Notification({ title: $.i18n('label_thongbao'), message: $.i18n(e.response.message) }, 'error');
                        }
                    }
                },
                sync: function (e) {
                    console.log(e);
                    $scope.scheduler.dataSource.read();
                },
                schema: {
                    model: {
                        id: "idKeHoach",
                        fields: {
                            idKeHoach: { from: "idKeHoach", type: "number" },
                            title: { from: "tenKhachHang", defaultValue: "" },
                            start: { type: "date", from: "thoiGianCheckInDuKien" },
                            end: { type: "date", from: "thoiGianCheckOutDuKien" },
                            checkinthucte: { type: "date", from: "thoiGianCheckInThucTe" },
                            checkoutthucte: { type: "date", from: "thoiGianCheckOutThucTe" },
                            idKhachHang: { from: "idKhachHang" },
                            tenNhanVien: { from: "tenNhanVien" },
                            idNhanVien: { from: "idNhanVien" },
                            viecCanLam: { from: "viecCanLam" },
                            ghiChu: { from: "ghiChu" },
                            description: { from: "diaChi" },
                            text_color: { from: "text_color" },

                        }
                    }
                },
            },
            editable: {
                create: true,
                update: false,
                destroy: true,
                template: $("#scheduler_customEditorTemplate").html(),
                window: {
                    title: $.i18n('label_thongtinkehoachnhanvien'),
                    open: function (e) {
                        if (editItem != null) {
                            $("#kehoachnv_vieccanlam").val(editItem.viecCanLam);
                            $("#kehoachnv_ghichu").val(editItem.ghiChu);
                        } else {
                            $("#kehoachnv_vieccanlam").val("");
                            $("#kehoachnv_ghichu").val("");
                        }
                        if ($("#kehoachnv_giocheckin").data("kendoDateTimePicker") == undefined) {
                            if (editItem == null) {
                                var datebegin = new Date();
                                datebegin.setMinutes(datebegin.getMinutes() + 15);
                                $("#kehoachnv_giocheckin").kendoDateTimePicker({
                                    value: datebegin,
                                    format: "{0:dd/MM/yyyy HH:mm}",
                                    dateInput: false
                                });
                            } else {
                                $("#kehoachnv_giocheckin").kendoDateTimePicker({
                                    value: editItem.start,
                                    format: "{0:dd/MM/yyyy HH:mm}",
                                    dateInput: false
                                });
                            }

                        }
                        if ($("#kehoachnv_giocheckout").data("kendoDateTimePicker") == undefined) {
                            var dateend = new Date();
                            dateend.setHours(19);
                            dateend.setMinutes(0);
                            dateend.setSeconds(0);
                            dateend.setMilliseconds(0);
                            if (editItem == null) {
                                $("#kehoachnv_giocheckout").kendoDateTimePicker({
                                    value: dateend,
                                    format: "{0:dd/MM/yyyy HH:mm}",
                                    dateInput: false
                                });
                            } else {
                                $("#kehoachnv_giocheckout").kendoDateTimePicker({
                                    value: editItem.end,
                                    format: "{0:dd/MM/yyyy HH:mm}",
                                    dateInput: false
                                });
                            }
                        }

                        //if ($("#kehoachnv_ngay").data("kendoDatePicker") == undefined) {
                        //	if (editItem == null) {
                        //		$("#kehoachnv_ngay").kendoDatePicker({
                        //			value: new Date(),
                        //			dateInput: false
                        //		});
                        //	} else {
                        //		$("#kehoachnv_ngay").kendoDatePicker({
                        //			value: editItem.start,
                        //			dateInput: false
                        //		});
                        //	}
                        //}
                        // Create or reload combo nhân viên
                        //$http({
                        //    method: 'GET',
                        //    url: urlApi + '/api/kehoachnhanvien/getallnhanvien'
                        //}).then(function successCallback(dsNhanVien) {


                        var dsNhanVienDataSource = new kendo.data.DataSource({
                            data: $scope.nhanviendata
                        })
                        if ($("#kehoachnv_nhanvien").data("kendoComboBox") == undefined) {
                            $("#kehoachnv_nhanvien").kendoComboBox({
                                dataTextField: "tenDayDu",
                                dataValueField: "idnv",
                                dataSource: dsNhanVienDataSource,
                                filter: "contains",
                                suggest: true,
                                delay: 1000,
                                select: function (e) {

                                    $http({
                                        method: 'GET',
                                        url: urlApi + '/api/nhanvienapp/GetKhachHangDaCapQuyen?idNhanvien=' + e.dataItem.idnv
                                    }).then(function successCallback(dsKhachHang) {
                                        var data = [];
                                        //data.push({ tenDayDu: "Tất cả", idKhachHang: 0 });
                                        $.each(dsKhachHang.data, function (index, item) {
                                            data.push({ tenDayDu: item.tenKhachHang, idKhachHang: item.iD_KhachHang })
                                        })
                                        var dsKhachHangDataSource = new kendo.data.DataSource({
                                            data: data
                                        })
                                        $("#kehoachnv_khachhang").data("kendoComboBox").setDataSource(dsKhachHangDataSource);
                                        $("#kehoachnv_khachhang").data("kendoComboBox").value("");
                                    });
                                }
                            });
                        } else {
                            $("#kehoachnv_nhanvien").data("kendoComboBox").setDataSource(dsNhanVienDataSource);
                        }
                        if (editItem != null) {
                            $("#kehoachnv_nhanvien").data("kendoComboBox").value(editItem.idNhanVien)
                        }
                        //});
                        // End create or reload combo nhân viên

                        // Create combo khách hàng
                        if (editItem != null) {
                            $http({
                                method: 'GET',
                                url: urlApi + '/api/kehoachnhanvien/getkhachhangbynhanvien?ID_NhanVien=' + editItem.idNhanVien
                            }).then(function successCallback(dsKhachHang) {
                                var dsKhachHangDataSource = new kendo.data.DataSource({
                                    data: dsKhachHang.data
                                })
                                if ($("#kehoachnv_khachhang").data("kendoComboBox") == undefined) {
                                    $("#kehoachnv_khachhang").kendoComboBox({
                                        dataTextField: "tenDayDu",
                                        dataValueField: "idKhachHang",
                                        dataSource: dsKhachHangDataSource,
                                        filter: "contains",
                                        suggest: true,
                                        delay: 1000
                                    });
                                } else {
                                    $("#kehoachnv_khachhang").data("kendoComboBox").setDataSource(dsKhachHangDataSource);
                                }
                                $("#kehoachnv_khachhang").data("kendoComboBox").value(editItem.idKhachHang)
                            });
                        } else {
                            if ($("#kehoachnv_khachhang").data("kendoComboBox") == undefined) {
                                $("#kehoachnv_khachhang").kendoComboBox({
                                    dataTextField: "tenDayDu",
                                    dataValueField: "idKhachHang",
                                    filter: "contains",
                                    suggest: true,
                                    delay: 1000
                                });
                            }
                        }

                        // End create or reload combo khách hàng
                    }
                }
            },
            messages: {
                today: $.i18n('label_ngayhientai'),
                next: $.i18n('label_trangsau'),
                previous: $.i18n('label_trangtruoc'),
                views: {
                    day: $.i18n('label_ngay'),
                    month: $.i18n('label_thang'),
                    week: $.i18n('label_tuan')
                },
                cancel: $.i18n('button_huy'),
                save: $.i18n('button_luukehoach'),
                destroy: $.i18n('button_xoakehoach'),
                deleteWindowTitle: $.i18n('label_xacnhanxoalich'),
                editable: {
                    confirmation: $.i18n('label_bancochacmuonxoakehoach')
                }
            },
            selectable: true,
            save: scheduler_save,
            remove: scheduler_remove,
            edit: scheduler_edit,
            add: scheduler_add,
            cancel: scheduler_cancel,
            navigate: function (e) {
                console.log(e);
                e.sender.date(e.date);
                e.sender.dataSource.read();
            },
        }

        $scope.theContent = kendo.template($("#templateSchedulerTooltipNhanVien").html());
        $scope.customEvent = kendo.template($("#eventschedule_template").html());

        //$("#scheduler").kendoTooltip({
        //    autoHide: true,
        //    filter: "div.k-event",
        //    content: kendo.template($("#templateSchedulerTooltipNhanVien").html()),
        //    position: "bottom"
        //});


        function scheduler_edit(e) {
            if (e.event.idKeHoach != "") {
                editItem = e.event;
                //e.sender.editEvent(e.event);
            }
        }

        function scheduler_save() {
            if (editItem != null) {
                if (editItem.start < new Date()) {
                    openDialog(dialogRoot, $.i18n('label_khongthesuakehoachtrongquakhu'));
                    return;
                }
                if ($("#kehoachnv_khachhang").data("kendoComboBox").value() == "") {
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_khongthesuakehoachvitrongkhachhang') }, 'warning');
                    return;
                }
                var batdau = $("#kehoachnv_giocheckin").data("kendoDateTimePicker").value();
                var batdaus = batdau.getFullYear() + "-" + parseInt(batdau.getMonth() + 1) + "-" + batdau.getDate() + " " + batdau.getHours() + ":" + batdau.getMinutes() + ":00";
                var ketthuc = $("#kehoachnv_giocheckout").data("kendoDateTimePicker").value();
                var ketthucs = ketthuc.getFullYear() + "-" + parseInt(ketthuc.getMonth() + 1) + "-" + ketthuc.getDate() + " " + ketthuc.getHours() + ":" + ketthuc.getMinutes() + ":00";
                if (ketthuc < new Date || ketthuc < batdau || batdau < new Date) {
                    openDialog(dialogRoot, $.i18n('label_thoigiankehoachkhongphuhop'));
                    return;
                }
                var data = {
                    ID: editItem.idKeHoach,
                    ID_NhanVien: $("#kehoachnv_nhanvien").data("kendoComboBox").value(),
                    ID_KhachHang: $("#kehoachnv_khachhang").data("kendoComboBox").value(),
                    //Ngay: $("#kehoachnv_ngay").data("kendoDatePicker").value(),
                    Ngay: batdaus,
                    BatDau: batdaus,
                    KetThuc: ketthucs,
                    ViecCanLam: $("#kehoachnv_vieccanlam").val(),
                    GhiChu: $("#kehoachnv_ghichu").val()
                }
                $http({
                    method: 'POST',
                    url: urlApi + '/api/kehoachnhanvien/update',
                    data: data
                }).then(function successCallback(response) {
                    if (response.data) {
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_capnhatkehoachthanhcong') }, 'success');
                        $("#scheduler").data("kendoScheduler").dataSource.read();

                    }
                });
            }
        }

        function scheduler_remove(e) {
            if (e.event.idKeHoach != "") {
                editItem = e.event;
            }
        }

        function scheduler_cancel(e) {
            e.sender.cancelEvent();
            e.preventDefault();
        }

        function scheduler_add(e) {
            editItem = null;
        }


    }
    $scope.init();

    function getquyen() {
        let path = $location.path();
        let url = path.replace('/', '')
        ComboboxDataService.getquyen(url).then(function (result) {
            $scope.permission = result.data;
            permission = result.data;
            if ($scope.permission.iD_ChucNang <= 0) {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_khongcoquyentruycapchucnang') }, "error");
                $location.path('/home')
            }
        });
    }

    $scope.nhanviendata = [];
    angular.element(document).ready(function () {
        $("#scheduler").kendoTooltip({
            autoHide: true,
            filter: "div.k-event",
            content: kendo.template($("#templateSchedulerTooltipNhanVien").html()),
            position: "bottom"
        });
        var dropDown = $("<button style='margin-right:5px' class='k-button' onclick='themKeHoach()'>" + $.i18n('button_themkehoach') + "</button><input style='width:150px;margin-right:5px' id='kehoachnhanvien_idnv'/>");
        $($scope.scheduler.toolbar).prepend(dropDown);




        $http({
            method: 'GET',
            url: urlApi + '/api/kehoachnhanvien/getallnhanvien'
        }).then(function successCallback(dsNhanVien) {
            $scope.nhanviendata = dsNhanVien.data;
            var data = [];
            data.push({ tenDayDu: "Tất cả", idnv: 0 });
            $.each(dsNhanVien.data, function (index, item) {
                data.push({ tenDayDu: item.tenDayDu, idnv: item.idnv })
            })
            var dsNhanVienDataSource = new kendo.data.DataSource({
                data: data
            })
            if ($("#kehoachnhanvien_idnv").data("kendoComboBox") == undefined) {
                $("#kehoachnhanvien_idnv").kendoComboBox({
                    dataTextField: "tenDayDu",
                    dataValueField: "idnv",
                    width: 400,
                    dataSource: dsNhanVienDataSource,
                    filter: "contains",
                    suggest: true,
                    delay: 1000,
                    change: function () {
                        $scope.scheduler.dataSource.read();
                    }
                });
                $("#kehoachnhanvien_idnv").data("kendoComboBox").value(0);
            }
        })

        $("#scheduler").on("dblclick", ".k-event", function (e) {
            if ($scope.permission.sua <= 0) {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_bankhongcoquyenthuchienthaotac') }, 'warning');
                return
            }


            var scheduler = $("#scheduler").getKendoScheduler();
            var event = scheduler.occurrenceByUid($(this).data("uid"));
            console.log(event);
            data = scheduler.dataSource.data();
            var filterData = [];
            $.each(data, function (index, item) {
                if (item.nguoiTao == event.nguoiTao && item.ngayTao == event.ngayTao) {
                    var dataItem = {
                        iD_KeHoach: item.idKeHoach,
                        tenNhanVien: item.tenNhanVien,
                        iD_NhanVien: item.idNhanVien,
                        tenKhachHang: item.title,
                        iD_KhachHang: item.idKhachHang,
                        gioCheckIn: item.start,
                        gioCheckOut: item.end,
                        viecCanLam: item.viecCanLam,
                        ghiChu: item.ghiChu
                    }
                    filterData.push(dataItem);
                }
            })
            if (event.start < new Date()) {
                //openDialog(dialogRoot, "Không thể sửa kế hoạch trong quá khứ");
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_khongthesuakehoachtrongquakhu') }, 'warning');
                return;
            }
            if (event.nguoiTao == 0) {
                scheduler.editEvent(event);
            } else {
                if (filterData.length > 1) {
                    $("#grid").data("kendoGrid").dataSource.data(filterData);
                    console.log($("#grid").data("kendoGrid").dataSource.data());
                    $("#windowThemKeHoachMulti").data("kendoWindow").center().open();
                } else if (filterData.length == 1) {
                    scheduler.editEvent(event);
                }
            }
        });


        $("#windowThemKeHoachMulti").kendoWindow({
            width: 1200,
            height: 500,
            title: $.i18n('label_bangthongtinkehoachmoi'),
            modal: true,
            open: function (e) {
                $scope.gridOptions = {
                    data: [],
                    schema: {
                        model: {
                            id: "id",
                            fields: {
                                iD_KeHoach: { type: 'number', editable: false },
                                tenNhanVien: { type: 'number', editable: true },
                                iD_NhanVien: { type: 'number', editable: true },
                                tenKhachHang: { type: 'number', editable: true },
                                iD_KhachHang: { type: 'number', editable: true },
                                gioCheckIn: { type: 'date', editable: true },
                                gioCheckOut: { type: 'date', editablle: true },
                                viecCanLam: { type: 'text', editablle: true },
                                ghiChu: { type: 'text', editable: true }
                            }
                        }
                    },
                    pageSize: 100
                }
            },
            close: function (e) {

            },
            visible: false

        })

        $scope.gridOptions = {
            sortable: true,
            height: 458,
            dataBinding: function () {
                RecordNumber = (this.dataSource.page() - 1) * this.dataSource.pageSize();
            },
            resizable: true,
            editable: true,
            filterable: {
                mode: "row"
            },
            save: function (e) {
                if (e.values.gioCheckIn) {
                    try {
                        setTimeout(function () {
                            e.sender.refresh();
                        })
                    } catch (ex) {
                        e.sender.refresh();
                    }
                }
            },
            pageable: false,
            columns: [
                {
                    title: " ",
                    field: 'iD_KeHoach',
                    template: function (e) {
                        return "<i onclick='DeleteRowKeHoach(\"" + e.uid + "\")' style='color:red' class='fa fa-trash'></i>";
                    },
                    width: 60,
                    attributes: {
                        class: "text-center"
                    },
                    editor: function (container, options) {
                        $("<i onclick='DeleteRowKeHoach(\"" + options.model.uid + "\")' style='color:red' class='fa fa-trash'></i>").appendTo(container);
                    },
                    headerAttributes: {
                        style: "text-align: center; font-size: 12px; ",
                        class: "table-header-cell"
                    },
                    filterable: {
                        cell: {
                            operator: "contains",
                            showOperators: false,
                            template: function (e) {
                                e.element.parent().html("<a class='k-button' title='" + $.i18n('button_themkehoach') + "' style='width:100%; height:25px;margin:0px;' onclick='AddRowKeHoach()'><i class='fa fa-plus'></i></a>")
                            }
                        }
                    },
                },
                {
                    field: "iD_NhanVien",
                    template: function (e) {
                        if (e.iD_NhanVien > 0) {
                            return e.tenNhanVien;
                        } else {
                            return "<b class='danger'>" + $.i18n('label_chuachonnhanvien') + "</b>";
                        }
                    },
                    title: $.i18n('header_tennhanvien'),
                    headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
                    filterable: defaultFilterableGrid,
                    width: "120px",
                    editor: function (container, options) {
                        $('<input name="' + options.field + '"/>').appendTo(container).kendoComboBox({
                            dataTextField: "tenDayDu",
                            dataValueField: "idnv",
                            filter: "contains",
                            autoBind: false,
                            change: function (e) {
                                options.model.tenNhanVien = e.sender.text();
                                options.model.iD_NhanVien = e.sender.value();
                            },
                            dataSource: new kendo.data.DataSource({
                                data: $scope.nhanviendata
                            }),
                        })
                    }
                },
                {
                    field: "iD_KhachHang",
                    template: function (e) {
                        if (e.iD_KhachHang > 0) {
                            return e.tenKhachHang;
                        } else {
                            return "<b class='danger'>" + $.i18n('label_chuachonkhachhang') + "</b>";
                        }
                    },
                    title: $.i18n('header_khachhang'),
                    headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
                    filterable: defaultFilterableGrid,
                    width: "150px",
                    editor: function (container, options) {
                        $http({
                            method: 'GET',
                            url: urlApi + '/api/kehoachnhanvien/getkhachhangbynhanvien?ID_NhanVien=' + options.model.iD_NhanVien
                        }).then(function successCallback(dsKhachHang) {
                            var dsKhachHangDataSource = new kendo.data.DataSource({
                                data: dsKhachHang.data
                            })

                            $('<input name="' + options.field + '"/>').appendTo(container).kendoComboBox({
                                dataTextField: "tenDayDu",
                                dataValueField: "idKhachHang",
                                filter: "contains",
                                autoBind: false,
                                change: function (e) {
                                    options.model.tenKhachHang = e.sender.text();
                                    options.model.iD_KhachHang = e.sender.value();
                                },
                                dataSource: dsKhachHangDataSource
                            })
                        });
                    }
                },
                {
                    field: "gioCheckIn",
                    title: $.i18n('label_ngaygiobatdau'),
                    template: function (dataItem) {
                        return kendo.htmlEncode(kendo.toString(dataItem.gioCheckIn, formatDate + " " + formatTime));
                    },
                    headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
                    attributes: { class: "text-center" },
                    filterable: false,
                    width: "120px",
                    editor: function (container, options) {
                        var gioCheckIn_editor = $('<input name="' + options.field + '"/>').appendTo(container).kendoDateTimePicker({
                            value: options.model.gioCheckIn,
                            format: "{0:dd/MM/yyyy HH:mm}",
                            dateInput: false,
                            min: new Date(),
                            change: function (e) {
                                options.model.gioCheckIn = e.sender.value();
                                if (options.model.gioCheckOut < options.model.gioCheckIn)
                                    options.model.gioCheckOut = e.sender.value();
                            }
                        });
                    }
                },
                {
                    field: "gioCheckOut",
                    title: $.i18n('label_ngaygioketthuc'),
                    template: function (dataItem) {
                        return kendo.htmlEncode(kendo.toString(dataItem.gioCheckOut, formatDate + " " + formatTime));
                    },
                    headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
                    attributes: { class: "text-center" },
                    filterable: false,
                    width: "120px",
                    editor: function (container, options) {
                        var gioCheckOut_editor = $('<input name="' + options.field + '"/>').appendTo(container).kendoDateTimePicker({
                            value: options.model.gioCheckIn,
                            format: "{0:dd/MM/yyyy HH:mm}",
                            dateInput: false,
                            min: options.model.gioCheckOut,
                            change: function (e) {
                                options.model.gioCheckOut = e.sender.value();
                            }
                        });
                        gioCheckOut_editor.data("kendoDateTimePicker").min(options.model.gioCheckIn);
                    }
                },
                {
                    field: "viecCanLam",
                    title: $.i18n('label_vieccanlam'),
                    headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
                    filterable: defaultFilterableGrid,
                    width: "200px"
                },
                {
                    field: "ghiChu",
                    title: $.i18n('header_ghichu'),
                    headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
                    filterable: defaultFilterableGrid,
                    width: "200px"
                }
            ]
        };

    })

    $scope.saveMultiKeHoach = function () {
        var grid = $scope.grid;
        dataSource = grid.dataSource.data();
        var param = [];
        $.each(dataSource, function (index, item) {
            var data = {
                ID: (item.iD_KeHoach != undefined) ? item.iD_KeHoach : 0,
                ID_NhanVien: item.iD_NhanVien,
                ID_KhachHang: item.iD_KhachHang,
                Ngay: kendo.toString(item.gioCheckIn, "yyyy/MM/dd HH:mm:ss"),
                BatDau: kendo.toString(item.gioCheckIn, "yyyy/MM/dd HH:mm:ss"),
                KetThuc: kendo.toString(item.gioCheckOut, "yyyy/MM/dd HH:mm:ss"),
                ViecCanLam: item.viecCanLam,
                GhiChu: (item.ghiChu != undefined) ? item.ghiChu : 0
            }
            param.push(data);
        })
        console.log(param);
        $http({
            method: 'POST',
            url: urlApi + '/api/kehoachnhanvien/createMulti',
            data: param,

        }).then(function successCallback(dsKhachHang) {
            $("#windowThemKeHoachMulti").data("kendoWindow").close();
            $scope.scheduler.dataSource.read();
        })
    }


})
function themKeHoach() {
    if (permission.them > 0) {
        $("#grid").data("kendoGrid").dataSource.data([]);
        $("#windowThemKeHoachMulti").data("kendoWindow").center().open();
    } else
        Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_bankhongcoquyenthuchienthaotac') }, 'warning');
}

function huyMultiKeHoach() {
    $("#grid").data("kendoGrid").dataSource.data([]);
    $("#windowThemKeHoachMulti").data("kendoWindow").close();
}

function AddRowKeHoach(e) {
    if (permission.them <= 0) {
        Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_bankhongcoquyenthuchienthaotac') }, 'warning');
        return
    }

    var grid = $("#grid").data("kendoGrid");
    dataSource = grid.dataSource.data();
    lastRow = dataSource[dataSource.length - 1];
    console.log(dataSource);

    if (lastRow != undefined) {
        var newRow = {
            iD_KeHoach: 0,
            tenNhanVien: lastRow.tenNhanVien,
            iD_NhanVien: lastRow.iD_NhanVien,
            tenKhachHang: lastRow.tenKhachHang,
            iD_KhachHang: lastRow.iD_KhachHang,
            gioCheckIn: lastRow.gioCheckIn,
            gioCheckOut: lastRow.gioCheckOut,
            viecCanLam: '',
            ghiChu: ''
        };
        grid.dataSource.add(newRow);

    } else {
        // grid.addRow();
        var datebegin = new Date();
        datebegin.setMinutes(datebegin.getMinutes() + 15);
        var dateend = new Date();
        dateend.setHours(19);
        dateend.setMinutes(0);
        dateend.setSeconds(0);
        dateend.setMilliseconds(0);
        var newRow = {
            iD_KeHoach: 0,
            tenNhanVien: $.i18n('label_chonnhanvien'),
            iD_NhanVien: 0,
            tenKhachHang: $.i18n('button_chonkhachhang'),
            iD_KhachHang: 0,
            gioCheckIn: datebegin,
            gioCheckOut: dateend,
            viecCanLam: '',
            ghiChu: ''
        };
        grid.dataSource.add(newRow);
    }

}
function DeleteRowKeHoach(uid) {
    var dataRow = $('#grid').data("kendoGrid").dataSource.getByUid(uid);
    $('#grid').data("kendoGrid").dataSource.remove(dataRow);
}

