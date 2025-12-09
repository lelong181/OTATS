angular.module('app').controller('keHoachNhanVienController', function ($scope, $http, $location, $rootScope, Notification) {
    dialogRoot = $("#kehoachnhanvien_dialogRoot").kendoDialog().data("kendoDialog").close();
    CreateSiteMap();
    hideLoadingPage();
    var editItem = null;
    $scope.init = function () {


        $scope.schedulerOptions = {
            date: new Date(),
            startTime: new Date("2013/1/1 00:00 AM"),
            //endTime: new Date(),
            //workDayStart: new Date("2013/1/1 00:00 AM"),
            //workDkayEnd: new Date("2013/1/1 23:59 PM"),
            height: function () {
                var heightGrid = $(document).height() - ($("#navbar").height() + $(".sitemap").height());
                return heightGrid < 100 ? 500 : heightGrid;
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
                            return kendo.stringify(data);
                        }
                        else if (operation == "update") {
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
                            return kendo.stringify(data);
                        }
                        else if (operation == "destroy") {
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
                                return kendo.stringify(data);
                            } else {
                                //notification.show({ kValue: "Bạn không thể xóa kế hoạch này" }, "error");
                                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_bankhongthexoakehoachnay') }, 'error');

                                data.ID = 0;
                                return kendo.stringify(data);
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
                            Notification({ title: $.i18n('label_thongbao'), message: e.response.message }, 'success');

                            //notification.show({ kValue: e.response.message }, "success");
                        } else {
                            //notification.show({ kValue: e.response.message }, "error");
                            Notification({ title: $.i18n('label_thongbao'), message: e.response.message }, 'error');
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
                update: true,
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
                            if (editItem == null) {
                                $("#kehoachnv_giocheckout").kendoDateTimePicker({
                                    value: new Date(),
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
                        $http({
                            method: 'GET',
                            url: urlApi + '/api/kehoachnhanvien/getallnhanvien'
                        }).then(function successCallback(dsNhanVien) {


                            var dsNhanVienDataSource = new kendo.data.DataSource({
                                data: dsNhanVien.data
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
                                            url: urlApi + '/api/kehoachnhanvien/getkhachhangbynhanvien?ID_NhanVien=' + e.dataItem.idnv
                                        }).then(function successCallback(dsKhachHang) {
                                            var data = [];
                                            data.push({ tenDayDu: "Tất cả", idKhachHang: 0 });
                                            $.each(dsKhachHang.data, function (index, item) {
                                                data.push({ tenDayDu: item.tenDayDu, idKhachHang: item.idKhachHang })
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
                        });
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
                    month: $.i18n('button_thang'),
                    week: $.i18n('label_tuan')
                },
                cancel: $.i18n('header_huy'),
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
            cancel: scheduler_cancel
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
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_capnhatkehoachthanhcong ') }, 'success');
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

        var dropDown = $("<input style='width:400px;' id='kehoachnhanvien_idnv'/>");
        //$($scope.scheduler.toolbar).prepend(dropDown);
    }
    $scope.init();
    console.log($scope.scheduler);

    $http({
        method: 'GET',
        url: urlApi + '/api/kehoachnhanvien/getallnhanviencombo'
    }).then(function successCallback(dsNhanVien) {
        var dsNhanVienDataSource = new kendo.data.DataSource({
            data: dsNhanVien.data
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
                    $("#scheduler").data("kendoScheduler").dataSource.read();
                }
            });
        }
    })

})