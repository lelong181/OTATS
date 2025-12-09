angular.module('app').controller('keHoachBaoDuongController', function (ComboboxDataService, $rootScope, $scope, $http, $location, Notification, ComboboxDataService) {
    CreateSiteMap();
    hideLoadingPage();

    $scope.init = function () {
        $scope.loadscheduler();
        getquyen();
    }

    $scope.loadscheduler = function () {
        $scope.schedulerOptions = {
            date: new Date(),
            startTime: new Date("2013/1/1 00:00:00 AM"),
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
            eventTemplate: $("#eventschedule_baoduong_template").html(),
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
                        url: urlApi + '/api/kehoachbaoduong/getkehoachbaoduongbynhanvien',
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
                        url: urlApi + '/api/kehoachbaoduong/update',
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
                        url: urlApi + '/api/kehoachbaoduong/create',
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
                        url: urlApi + '/api/kehoachbaoduong/delete',
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
                            var batdau = $("#kehoachbaoduong_giocheckin").data("kendoDatePicker").value();
                            var batdaus = batdau.getFullYear() + "-" + parseInt(batdau.getMonth() + 1) + "-" + batdau.getDate() + " " + batdau.getHours() + ":" + batdau.getMinutes() + ":00";
                            var data = {
                                ID: 0,
                                ID_Xe: $("#kehoachbaoduong_xe").data("kendoComboBox").value(),
                                Ngay: batdaus,
                            }

                            console.log(batdau);
                            var newdate = new Date();
                            newdate.setHours(0);
                            console.log(newdate);
                            if (batdau >= newdate) {
                                return $scope.permission.them == 1 ? kendo.stringify(data) : null;

                            } else {
                                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_khongthexoakehoachtaithoidiemquakhu') }, 'error');

                                return null;
                            }
                        }
                        else if (operation == "destroy") {
                            if ($scope.permission.xoa != 1) {
                                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_bankhongcoquyenthuchienthaotac') }, 'error');
                            } else {
                                var data = {
                                    ID: editItem.taskId,
                                    ID_Xe: 0,
                                    Ngay: new Date(),
                                }
                                if (editItem.start < new Date()) {
                                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_khongthexoakehoachtaithoidiemquakhu') }, 'error');
                                    data.ID = 0;
                                }
                            }
                            return $scope.permission.xoa == 1 ? kendo.stringify(data) : null;
                        }
                        else if (operation == "read") {
                            var view = $scope.scheduler.view();
                            var idnhanvien = 0;
                            if ($("#kehoachbaoduong_idnv").data("kendoComboBox") != undefined) {
                                idnhanvien = $("#kehoachbaoduong_idnv").data("kendoComboBox").value();
                                if (idnhanvien < 0) {
                                    idnhanvien = 0;
                                }
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
                    if (e.type === "create" && e.response) {
                        console.log(e.response);
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
                            taskId: { from: "iD_Xe_KeHoachBaoDuong", type: "number" },
                            title: { from: "bienKiemSoat", defaultValue: "" },
                            start: { type: "date", from: "ngayBaoDuongDuKien" },
                            end: { type: "date", from: "ngayBDTiepTheo" },
                            idNhanVien: { from: "iD_NhanVien" },
                            idXe: { from: "iD_Xe" },
                            loaixe: { from: "loaiXe" },
                            mota: { from: "text_color_mota" },
                            tenNhanVien: { from: "tenNhanVien" },
                            text_color: { from: "text_color" }
                        }
                    },
                    //parse: function (response) {
                    //    var tasks = [];
                    //    for (var i = 0; i < response.length; i++) {
                    //        var task = {
                    //            taskId: response[i].iD_Xe_KeHoachBaoDuong,
                    //            title: response[i].bienKiemSoat,
                    //            start: response[i].ngayBaoDuongDuKien,
                    //            end: response[i].ngayBDTiepTheo,
                    //            idNhanVien: response[i].iD_NhanVien,
                    //            idXe: response[i].iD_Xe,
                    //            loaixe: response[i].loaiXe,
                    //            mota: $.i18n(response[i].text_color_mota),
                    //            tenNhanVien: response[i].tenNhanVien,
                    //            text_color: response[i].text_color,
                    //        };
                    //        tasks.push(task);
                    //    }
                    //    console.log(tasks);
                    //    return tasks;
                    //}
                },
            },
            editable: {
                template: $("#scheduler_editkehoachbaoduong").html(),
                window: {
                    title: $.i18n('label_thongtinkehoachbaoduong'),
                    open: function (e) {
                        console.log(editItem);
                        if ($("#kehoachbaoduong_giocheckin").data("kendoDatePicker") == undefined) {
                            if (editItem == null) {
                                $("#kehoachbaoduong_giocheckin").kendoDatePicker({
                                    value: new Date(),
                                    dateInput: false
                                });
                            } else {
                                $("#kehoachbaoduong_giocheckin").kendoDatePicker({
                                    value: editItem.start,
                                    format: "dd/MM/yyyy",
                                    dateInput: false
                                });
                            }

                        }

                        // Create or reload combo xe
                        $http({
                            method: 'GET',
                            url: urlApi + '/api/kehoachbaoduong/getallxe'
                        }).then(function successCallback(dsXe) {
                            var dsXeDataSource = new kendo.data.DataSource({
                                data: dsXe.data
                            })
                            if ($("#kehoachbaoduong_xe").data("kendoComboBox") == undefined) {
                                $("#kehoachbaoduong_xe").kendoComboBox({
                                    dataTextField: "bienKiemSoat",
                                    dataValueField: "iD_Xe",
                                    dataSource: dsXeDataSource,
                                    filter: "contains",
                                    suggest: true,
                                    delay: 1000
                                });
                            } else {
                                $("#kehoachbaoduong_xe").data("kendoComboBox").setDataSource(dsXeDataSource);
                            }
                            if (editItem != null) {
                                $("#kehoachbaoduong_xe").data("kendoComboBox").value(editItem.idXe)
                            }
                        });
                        // End create or reload combo xe

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
        }
    }

    $scope.init();

    angular.element(document).ready(function () {
        $("#scheduler").kendoTooltip({
            autoHide: true,
            filter: "div.k-event",
            content: kendo.template($("#templateSchedulerTooltip").html()),
            position: "bottom",
            requestStart: function (e) {
                console.log(e);
            },
            contentLoad: function (e) {
                console.log(e);
            }
        });

        var dropDown = $("<input style='width:400px;' id='kehoachbaoduong_idnv'/>");
        $($scope.scheduler.toolbar).prepend(dropDown);

        $http({
            method: 'GET',
            url: urlApi + '/api/baocao/getallnhanvien'
        }).then(function successCallback(dsNhanVien) {
            var dsNhanVienDataSource = new kendo.data.DataSource({
                data: dsNhanVien.data
            })
            if ($("#kehoachbaoduong_idnv").data("kendoComboBox") == undefined) {
                $("#kehoachbaoduong_idnv").kendoComboBox({
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
                $("#kehoachbaoduong_idnv").data("kendoComboBox").value(-1);
            }
            $scope.scheduler.dataSource.read();

        })
    })

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
        });
    }
    function validatethemsua() {
        let flag = true;
        let msg = '';

        if ($("#kehoachbaoduong_xe").data("kendoComboBox").value() == '' || $("#kehoachbaoduong_xe").data("kendoComboBox").value() == undefined || $("#kehoachbaoduong_xe").data("kendoComboBox").value() == 0) {
            flag = false;
            msg = 'label_chuachonxenao';
            //$("#kehoachbaoduong_xe").data("kendoComboBox").focus();
        }

        if (!flag)
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n(msg) }, 'warning');

        return flag;
    }


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
            if (validatethemsua()) {
                if (editItem != null) {
                    console.log(editItem);
                    if (editItem.start < new Date()) {
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_khongthesuakehoachtrongquakhu') }, 'error');
                        return;
                    }
                    var batdau = $("#kehoachbaoduong_giocheckin").data("kendoDatePicker").value();
                    var batdaus = batdau.getFullYear() + "-" + parseInt(batdau.getMonth() + 1) + "-" + batdau.getDate() + " " + batdau.getHours() + ":" + batdau.getMinutes() + ":00";
                    var data = {
                        ID: editItem.id,
                        ID_Xe: $("#kehoachbaoduong_xe").data("kendoComboBox").value(),
                        Ngay: batdaus,
                    }
                    if (batdau < new Date) {
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_suakehoachkhongthanhcongthoigiankhongphuhop') }, 'error');
                        return;
                    }
                    $http({
                        method: 'POST',
                        url: urlApi + '/api/kehoachbaoduong/update',
                        data: $scope.permission.sua == 1 ? data : null
                    }).then(function successCallback(response) {
                        $scope.scheduler.dataSource.read();
                    });
                }
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

