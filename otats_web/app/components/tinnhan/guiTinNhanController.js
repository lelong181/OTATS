angular.module('app').controller('guiTinNhanController', function ($state, $scope, $location, Notification, NgMap, tinNhanDataService) {
    CreateSiteMap();

    let geocoder = new google.maps.Geocoder();
    $scope.urlApi = urlApi;

    function init() {
        initMap();

        $scope.mes = {
            noiDung: "",
            ids: []
        }
        $scope.TinNhanForNV = "";
        $scope.trangThaiData = [
            {
                text: $.i18n("label_tatca"), value: 100
            },
            {
                text: $.i18n("label_ngoaituyen"), value: 0
            },
            {
                text: $.i18n("label_tructuyen"), value: 1
            },
            {
                text: $.i18n("label_matketnoi"), value: 2
            }
        ];
        $scope.trangThaiSelect = {
            text: $.i18n("label_tatca"), value: 100
        }
        $scope.khoangCach = 0;
    }

    function initMap() {
        NgMap.getMap().then(function (map) {
            $scope.map = map;
            google.maps.event.addListener($scope.map, 'click', function (event) {
                placeMarker(event.latLng);
            });
            inittreeview();
        });
    }

    function inittreeview() {
        let dataSource = new kendo.data.HierarchicalDataSource({
            data: [],
            schema: {
                model: {
                    children: "childs"
                }
            }
        })

        $("#treeview").kendoTreeView({
            dataSource: dataSource,
            dataTextField: "tenNhom",
            dataValueField: "iD_Nhom",
            select: onSelectNhom,
        });

        loadtreeView();
    }
    function loadtreeView() {
        tinNhanDataService.getListNhomNhanVien().then(function (result) {
            setDataTreeview(result.data);
        });
    }
    function setDataTreeview(data) {
        let dataSource = new kendo.data.HierarchicalDataSource({
            data: data,
            schema: {
                model: {
                    children: "childs"
                }
            }
        })

        let tree = $("#treeview").data("kendoTreeView");
        tree.setDataSource(dataSource);
        tree.expand(".k-item");
        tree.select(".k-first");

        let selectedNode = tree.select();
        $scope.idNhom = tree.dataItem(selectedNode).iD_Nhom;
        $scope.loadgrid();
    }
    function onSelectNhom(e) {
        $scope.idNhom = $("#treeview").getKendoTreeView().dataItem(e.node).iD_Nhom;
        $scope.loadgrid();
    }
    function placeMarker(location) {

        if ($scope.khachhang == undefined) {
            $scope.khachhang = new google.maps.Marker({
                position: location,
                map: $scope.map,
                animation: google.maps.Animation.DROP,
            });
            google.maps.event.addListener($scope.khachhang, 'dragend', function () {
                geocoder.geocode
                    ({
                        latLng: $scope.khachhang.getPosition()
                    },
                        function (results, status) {
                            if (status == google.maps.GeocoderStatus.OK) {
                                document.getElementById('txtDiaChi').value = results[0].formatted_address;
                            }

                        }
                    );
            });
        }
        else {
            $scope.khachhang.setPosition(location);
        }
        geocoder.geocode
            ({
                latLng: $scope.khachhang.getPosition()
            },
                function (results, status) {
                    if (status == google.maps.GeocoderStatus.OK) {
                        document.getElementById('txtDiaChi').value = results[0].formatted_address;
                    }

                }
            );
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
        dataList.push({ field: "tennhanvien", title: $.i18n('header_tendaydu'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "175px" });
        dataList.push({ field: "trangthai", title: $.i18n('header_trangthaitructuyen'), headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid, width: "100px" });
        dataList.push({
            field: "thoigiancapnhat", title: $.i18n('header_thoigiancapnhat'),
            attributes: { style: "text-align: left" },
            //template: function (dataItem) {
            //    console.log(dataItem.thoigiancapnhat);
            //    if (dataItem.thoigiancapnhat != null) {
            //        if (dataItem.thoigiancapnhat.getFullYear() > 1900)
            //            return kendo.htmlEncode(kendo.toString(dataItem.thoigiancapnhat, formatDateTime));
            //        else
            //            return '';
            //    } else
            //        return '';
            //},
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
            filterable: false, width: "150px"
        });
        dataList.push({
            title: $.i18n('header_tacvu'),
            template: function (e) {
                return '<button class="btn btn-link btn-menubar" ng-click="xemchitiet(' + e.idnhanvien + ')"><i class="fas fa-file fas-sm color-infor"></i></button>';
            },
            headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, filterable: defaultFilterableGrid,
            width: "60px"
        });
        return dataList;
    }

    //envent
    $scope.guiTinNhan = function () {
        if ($scope.mes.noiDung == undefined || $scope.mes.noiDung == "") {
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n("label_chuanhapnoidungtinnhan") }, 'error');
        } else {
            tinNhanDataService.guiTinNhan($scope.mes).then(function (result) {
                if (result.flag) {
                    if (result.data.success) {
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.data.msg) }, 'success');
                    } else {
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.data.msg) }, 'error');
                    }
                } else {
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.data.msg) }, 'error');
                }
            });
        }
    }
    $scope.guiTinNhanForNV = function (idnv,mess) {
        tinNhanDataService.guiTinNhan({
            noiDung: mess,
            ids: [idnv]
        }).then(function (result) {
            if (result.flag) {
                if (result.data.success) {
                    $scope.TinNhanForNV = "";
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.data.msg) }, 'success');
                } else {
                    Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.data.msg) }, 'error');
                }
            } else {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n(result.data.msg) }, 'error');
            }
        });
    }
    $scope.xemchitiet = function (idnhanvien) {
        $state.go("baocaoguitinnhan_nv", { idnhanvien: idnhanvien });
    }
    $scope.loadgrid = function () {
        kendo.ui.progress($("#grid"), true);
        $scope.gridOptions = {
            sortable: true,
            height: function () {
                var heightGrid = $(window).height() - $(".sitemap").height();
                console.log(heightGrid - 330);
                return heightGrid - 330;
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
            change: function (e) {
                var rows = e.sender.select();
                var selectedID = [];
                rows.each(function (e) {
                    var grid = $("#grid").data("kendoGrid");
                    var dataItem = grid.dataItem(this);

                    selectedID.push(dataItem.idnhanvien)
                })
                $scope.mes.ids = selectedID;
            }
        };
        let center = $scope.map.getCenter();
        tinNhanDataService.getlist(center.lng(), center.lat(), $scope.khoangCach, $scope.trangThaiSelect.value, $scope.idNhom).then(function (result) {
            $scope.stores = result.data;
            $scope.gridData = {
                data: result.data,
                schema: {
                    model: {
                        fields: {
                            //thoigiancapnhat: {
                            //    type: "date"
                            //}
                        }
                    }
                },
                pageSize: 20
            };
            kendo.ui.progress($("#grid"), false);
        });
    }
    $scope.movetomap = function () {
        if ($scope.timkiemdiachi != "") {            
            geocoder.geocode({ 'address': $scope.timkiemdiachi }, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    $scope.map.setCenter(results[0].geometry.location);
                    iconurl = "images/location.png";
                    if ($scope.khachhang == undefined) {
                        $scope.khachhang = new google.maps.Marker({
                            map: $scope.map,
                            position: results[0].geometry.location,
                            //icon: iconurl,
                            draggable: true,
                            animation: google.maps.Animation.DROP
                        });
                        google.maps.event.addListener($scope.khachhang, 'dragend', function () {
                            geocoder.geocode
                                ({
                                    latLng: $scope.khachhang.getPosition()
                                },
                                    function (results, status) {
                                        if (status == google.maps.GeocoderStatus.OK) {
                                            document.getElementById('txtDiaChi').value = results[0].formatted_address;
                                        }

                                    }
                                );
                        });
                    } else {
                        $scope.khachhang.setPosition(results[0].geometry.location);
                    }
                } else {
                    alert($.i18n("label_khongxacdinhduocdiachinay"));
                }
            });
        }
    }

    init();
})