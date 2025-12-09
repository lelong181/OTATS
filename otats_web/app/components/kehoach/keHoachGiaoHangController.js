angular.module('app').controller('keHoachGiaoHangController', function ($rootScope, $scope, $http, $location, Notification, ComboboxDataService) {
    CreateSiteMap();
    hideLoadingPage();

    $scope.init = function () {
        getquyen();
        $scope.loadscheduler();
    }

    $scope.loadscheduler = function () {
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
            eventTemplate: $("#eventschedule_giaohang_template").html(),
            dataSource: {
                offlineStorage: null,
                serverSorting: false,
                serverPaging: false,
                serverFiltering: true,
                serverGrouping: false,
                serverAggregates: false,
                batch: true,
                inPlaceSort: false,
                transport: {
                    read: {
                        url: urlApi + '/api/kehoachgiaohang/getkehoachgiaohangbynhanvien',
                        contentType: "application/json",
                        beforeSend: function (req) {
                            if ($rootScope.Authorization == undefined || $rootScope.Authorization == null || $rootScope.Authorization == "") {
                                var url = location.origin + '/login.aspx';
                                window.open(url, "_self");
                            } else {
                                req.setRequestHeader('Authorization', $rootScope.Authorization);
                            }
                        },

                    },
                    update: {
                        url: urlApi + '/api/kehoachgiaohang/update',
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
                        url: urlApi + '/api/kehoachgiaohang/create',
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
                        url: urlApi + '/api/kehoachgiaohang/delete',
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
                            var batdau = $("#kehoachgiaohang_giocheckin").data("kendoDateTimePicker").value();
                            var batdaus = batdau.getFullYear() + "-" + parseInt(batdau.getMonth() + 1) + "-" + batdau.getDate() + " " + batdau.getHours() + ":" + batdau.getMinutes() + ":00";
                            var data = {
                                ID: 0,
                                ID_NhanVien: ($("#kehoachgiaohang_nhanvien").data("kendoComboBox").value() == '') ? 0 : $("#kehoachgiaohang_nhanvien").data("kendoComboBox").value(),
                                ID_DonHang: ($("#kehoachgiaohang_donhang").data("kendoComboBox").value() == '') ? 0 : $("#kehoachgiaohang_donhang").data("kendoComboBox").value(),
                                ID_KhachHang: 0,
                                Ngay: batdaus,
                                GhiChu: ($("#kehoachgiaohang_ghichu").val() == null) ? '' : $("#kehoachgiaohang_ghichu").val()
                            }
                            return $scope.permission.them == 1 ? kendo.stringify(data) : null;
                        }
                        else if (operation == "update") {
                            console.log(options);
                        }
                        else if (operation == "destroy") {
                            if ($scope.permission.xoa != 1) {
                                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_bankhongcoquyenthuchienthaotac') }, 'error');
                            } else {
                                var data = {
                                    ID: editItem.taskId,
                                    ID_NhanVien: 0,
                                    ID_KhachHang: 0,
                                    Ngay: new Date(),
                                    BatDau: new Date(),
                                    KetThuc: new Date(),
                                    ViecCanLam: "",
                                    GhiChu: ""
                                }
                                console.log(options);
                                if (options.models[0].thoiGianDuKien < new Date()) {
                                    ID = 0;
                                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_khongthexoakehoachtaithoidiemquakhu') }, 'error');

                                } else {

                                }
                            }
                            return $scope.permission.xoa == 1 ? kendo.stringify(data) : null;
                        }
                        else if (operation == "read") {
                            var view = $scope.scheduler.view();
                            var idnhanvien = 0;
                            if ($("#kehoachgiaohang_idnv").data("kendoComboBox") != undefined) {
                                idnhanvien = $("#kehoachgiaohang_idnv").data("kendoComboBox").value();
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
                    console.log(e);
                    if (e.type === "create" && e.response) {
                        if (e.response.success) {
                            Notification({ title: $.i18n('label_thongbao'), message: $.i18n(e.response.message) }, 'success');
                        } else {
                            Notification({ title: $.i18n('label_thongbao'), message: $.i18n(e.response.message) }, 'error');
                        }
                    }
                    else if (e.type === "update" && e.response) {
                        if (e.response.success) {
                            Notification({ title: $.i18n('label_thongbao'), message: $.i18n(e.response.message) }, 'success');
                        } else {
                            Notification({ title: $.i18n('label_thongbao'), message: $.i18n(e.response.message) }, 'error');
                        }
                    }


                },
                sync: function (e) {
                    $scope.scheduler.dataSource.read();
                },
                schema: {
                    model: {
                        id: "taskId",
                        fields: {
                            taskId: { from: "idKeHoach", type: "number" },
                            title: { from: "maThamChieu", defaultValue: "" },
                            start: { type: "date", from: "thoiGianDuKien" },
                            end: { type: "date", from: "ngayTaoDonHang" },
                            idKhachHang: { from: "iD_KhachHang" },
                            idNhanVien: { from: "iD_NhanVien" },
                            idDonHang: { from: "iD_DonHang" },
                            ghiChu: { from: "ghiChu" },
                            mota: { from: "text_color_mota" },
                            tenNhanVien: { from: "tenNhanVien" },
                            text_color: { from: "text_color " }
                        }
                    }
                },
            },
            editable: {
                template: $("#scheduler_editkehoachgiaohang").html(),
                window: {
                    title: $.i18n('label_thongtinkehoachnhanvien'),
                    open: function (e) {
                        if (editItem != null) {
                            $("#kehoachgiaohang_ghichu").val(editItem.ghiChu);
                        } else {
                            $("#kehoachgiaohang_ghichu").val("");
                        }
                        if ($("#kehoachgiaohang_giocheckin").data("kendoDateTimePicker") == undefined) {
                            if (editItem == null) {
                                $("#kehoachgiaohang_giocheckin").kendoDateTimePicker({
                                    value: new Date(),
                                    dateInput: false
                                });
                            } else {
                                $("#kehoachgiaohang_giocheckin").kendoDateTimePicker({
                                    value: editItem.start,
                                    dateInput: false
                                });
                            }

                        }

                        // Create or reload combo nhân viên
                        $http({
                            method: 'GET',
                            url: urlApi + '/api/baocao/getallnhanvien'
                        }).then(function successCallback(dsNhanVien) {
                            var dsNhanVienDataSource = new kendo.data.DataSource({
                                data: dsNhanVien.data
                            })

                            if ($("#kehoachgiaohang_nhanvien").data("kendoComboBox") == undefined) {
                                $("#kehoachgiaohang_nhanvien").kendoComboBox({
                                    dataTextField: "tenDayDu",
                                    dataValueField: "idnv",
                                    dataSource: dsNhanVienDataSource,
                                    filter: "contains",
                                    suggest: true,
                                    delay: 1000
                                });
                            } else {
                                $("#kehoachgiaohang_nhanvien").data("kendoComboBox").setDataSource(dsNhanVienDataSource);
                            }
                            if (editItem != null) {
                                $("#kehoachgiaohang_nhanvien").data("kendoComboBox").value(editItem.idNhanVien)
                            }
                        });
                        // End create or reload combo nhân viên

                        // Create combo đơn hàng
                        kendo.ui.progress($(".k-window"), true);
                        console.log($("#kehoachgiaohang_donhang").data("kendoComboBox"));
                        $http({
                            method: 'GET',
                            url: urlApi + '/api/kehoachgiaohang/getalldonhang'
                        }).then(function successCallback(dsKhachHang) {
                            var dsKhachHangDataSource = new kendo.data.DataSource({
                                data: dsKhachHang.data
                            })
                            if ($("#kehoachgiaohang_donhang").data("kendoComboBox") == undefined) {
                                $("#kehoachgiaohang_donhang").kendoComboBox({
                                    dataTextField: "ten",
                                    dataValueField: "ma",
                                    dataSource: dsKhachHangDataSource,
                                    filter: "contains",
                                    suggest: true,
                                    delay: 1000
                                });
                            } else {
                                $("#kehoachgiaohang_donhang").data("kendoComboBox").setDataSource(dsKhachHangDataSource);
                            }
                            if (editItem != null) {
                                console.log(editItem.idDonHang);
                                $("#kehoachgiaohang_donhang").data("kendoComboBox").value(editItem.idDonHang)
                            }
                            kendo.ui.progress($(".k-window"), false);
                        });


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
            //dataBinding: scheduler_dataBinding,
            //dataBound: scheduler_dataBound,
            save: scheduler_save,
            remove: scheduler_remove,
            edit: scheduler_edit,
            add: scheduler_add,
            cancel: scheduler_cancel,
            //change: scheduler_change,
            //moveStart: scheduler_moveStart,
            //move: scheduler_move,
            //moveEnd: scheduler_moveEnd,
            //resizeStart: scheduler_resizeStart,
            //resize: scheduler_resize,
            //resizeEnd: scheduler_resizeEnd,
            //navigate: scheduler_navigate,
        }

    }

    $scope.init();

    function getquyen() {
        let path = $location.path();
        let url = path.replace('/', '')
        ComboboxDataService.getquyen(url).then(function (result) {
            $scope.permission = result.data;
            console.log($scope.permission);
            if ($scope.permission.iD_ChucNang <= 0) {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_khongcoquyentruycapchucnang') }, "error");
                $location.path('/home')
            }
            console.log($scope.schedulerOptions.editable);
            $scope.schedulerOptions.editable.destroy = $scope.permission.xoa == 1 ? true : false;
            $scope.scheduler.refresh();
        });
    }

    angular.element(document).ready(function () {
        $("#scheduler").kendoTooltip({
            autoHide: true,
            filter: "div.k-event",
            content: kendo.template($("#templateSchedulerTooltip").html()),
            position: "bottom"
        });

        var dropDown = $("<input style='width:150px;' id='kehoachgiaohang_idnv'/>");
        $($scope.scheduler.toolbar).prepend(dropDown);

        $http({
            method: 'GET',
            url: urlApi + '/api/kehoachnhanvien/getallnhanvien'
        }).then(function successCallback(dsNhanVien) {
            var data = [];
            data.push({ tenDayDu: $.i18n('label_tatca'), idnv: 0 });
            $.each(dsNhanVien.data, function (index, item) {
                data.push({ tenDayDu: item.tenDayDu, idnv: item.idnv })
            })
            var dsNhanVienDataSource = new kendo.data.DataSource({
                data: data
            })
            if ($("#kehoachgiaohang_idnv").data("kendoComboBox") == undefined) {
                $("#kehoachgiaohang_idnv").kendoComboBox({
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
                $("#kehoachgiaohang_idnv").data("kendoComboBox").value(0);
            }
            $scope.scheduler.dataSource.read();

        })
    });

    var editItem = null;


    function scheduler_edit(e) {
        if (e.event.taskId != "") {
            editItem = e.event;
            //e.sender.editEvent(e.event);
        }
    }

    function scheduler_save() {
        if ($scope.permission.sua != 1) {
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_bankhongcoquyenthuchienthaotac') }, 'error');
        } else {
            if (editItem != null) {
                console.log(editItem);
                if (editItem.start < new Date()) {
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_khongthesuakehoachtaithoidiemquakhu') }, 'error');
                    return;
                }
                var batdau = $("#kehoachgiaohang_giocheckin").data("kendoDateTimePicker").value();
                var batdaus = batdau.getFullYear() + "-" + parseInt(batdau.getMonth() + 1) + "-" + batdau.getDate() + " " + batdau.getHours() + ":" + batdau.getMinutes() + ":00";
                var data = {
                    ID: editItem.id,
                    ID_KhachHang: 0,
                    ID_NhanVien: $("#kehoachgiaohang_nhanvien").data("kendoComboBox").value(),
                    ID_DonHang: $("#kehoachgiaohang_donhang").data("kendoComboBox").value(),
                    Ngay: batdaus,
                    GhiChu: $("#kehoachgiaohang_ghichu").val()
                }



                $http({
                    method: 'POST',
                    url: urlApi + '/api/kehoachgiaohang/update',
                    data: $scope.permission.sua == 1 ? data : null
                }).then(function successCallback(response) {
                    $scope.scheduler.dataSource.read();
                });
            }
        }
    }

    function scheduler_remove(e) {
        if (e.event.taskId != "") {
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
})