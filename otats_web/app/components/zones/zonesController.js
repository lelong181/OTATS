angular.module('app').controller('zonesController', function ($scope, $rootScope, $timeout, Notification, zonesDataService) {
    $scope.title = "Quản lý Phân khu";
    // Ensure safe access to UserInfo
    $scope.hotelId = ($rootScope.UserInfo && $rootScope.UserInfo.macongty) ? $rootScope.UserInfo.macongty : 0;

    $scope.zones = [];
    $scope.isEdit = false;
    $scope.currentZone = {};

    window.record = 0; // Global variable for grid numbering (legacy Kendo pattern)

    function init() {
        loadgrid();
    }

    function loadgrid() {
        // Grid Configuration
        $scope.gridOptions = {
            dataSource: {
                transport: {
                    read: function (e) {
                        zonesDataService.getByHotel($scope.hotelId).then(function (response) {
                            if (response.flag) {
                                e.success(response.data);
                            } else {
                                e.success([]);
                                // Only show error if messsage is meaningful, otherwise silent empty list or warning
                                if (response.message) {
                                    Notification.warning({ message: response.message });
                                }
                            }
                        });
                    }
                },
                pageSize: 20,
                schema: {
                    model: {
                        id: "zoneId",
                        fields: {
                            zoneId: { type: "number" },
                            hotelId: { type: "number" },
                            zoneName: { type: "string" },
                            description: { type: "string" },
                            distanceToReception: { type: "number" },
                            id_QLLH: { type: "number" }
                        }
                    }
                }
            },
            sortable: true,
            pageable: true,
            resizable: true,
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".toolbarmenu").height());
                return heightGrid - 40;
            },
            dataBinding: function () {
                record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
            },
            columns: [
                {
                    title: "#", template: "#= ++record #", width: "50px",
                    headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
                    attributes: { class: "text-center" }
                },
                {
                    field: "zoneName", title: "Tên Phân Khu", width: "200px",
                    headerAttributes: { "class": "table-header-cell", style: "text-align: center" }
                },
                {
                    field: "description", title: "Mô tả", width: "300px",
                    headerAttributes: { "class": "table-header-cell", style: "text-align: center" }
                },
                {
                    field: "distanceToReception", title: "Khoảng cách đến Lễ tân (m)", width: "150px",
                    headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
                    attributes: { class: "text-center" }
                },
                {
                    title: "Thao tác",
                    width: "120px",
                    headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
                    attributes: { class: "text-center" },
                    template: '<button class="btn btn-link btn-menubar" ng-click="openEdit(dataItem)" title="Sửa"><i class="fas fa-edit fas-sm color-infor"></i></button>' +
                        '<button class="btn btn-link btn-menubar" ng-click="confirmDelete(dataItem)" title="Xóa"><i class="fas fa-trash fas-sm color-danger"></i></button>'
                }
            ]
        };
    }

    // Modal Functions
    $scope.openAdd = function () {
        $scope.isEdit = false;
        $scope.currentZone = {
            hotelId: $scope.hotelId,
            zoneName: "",
            description: "",
            distanceToReception: 0
        };
        $('#modalZone').modal('show');
    };

    $scope.openEdit = function (dataItem) {
        $scope.isEdit = true;
        $scope.currentZone = angular.copy(dataItem);
        $scope.currentZone.hotelId = $scope.currentZone.hotelId || $scope.hotelId;
        $('#modalZone').modal('show');
    };

    $scope.saveZone = function () {
        if (!$scope.currentZone.zoneName) {
            Notification.warning({ message: 'Vui lòng nhập tên phân khu' });
            return;
        }

        if ($scope.isEdit) {
            zonesDataService.update($scope.currentZone).then(function (response) {
                if (response.flag) {
                    Notification.success({ message: response.message });
                    $('#modalZone').modal('hide');
                    $scope.refreshGrid();
                } else {
                    Notification.error({ message: response.message });
                }
            });
        } else {
            zonesDataService.insert($scope.currentZone).then(function (response) {
                if (response.flag) {
                    Notification.success({ message: response.message });
                    $('#modalZone').modal('hide');
                    $scope.refreshGrid();
                } else {
                    Notification.error({ message: response.message });
                }
            });
        }
    };

    $scope.confirmDelete = function (dataItem) {
        // Use standard confirm or custom modal if available. Using standard confirm for now as per previous code.
        // Or better, use the commonOpenConfirm if it's a global pattern, but I don't refer to it clearly. 
        // khachHangController uses 'openConfirm'. Let's stick to standard confirm for safety unless I find openConfirm definintion.
        // khachHangController.js: openConfirm($.i18n('label_...'), 'apDungXoaKhachHang', null, data);
        // I will use standard confirm to be safe and self-contained, as openConfirm assumes global scope availability.

        if (confirm("Bạn có chắc chắn muốn xóa phân khu này không?")) {
            zonesDataService.deleteZone(dataItem.zoneId).then(function (response) {
                if (response.flag) {
                    Notification.success({ message: response.message });
                    $scope.refreshGrid();
                } else {
                    Notification.error({ message: response.message });
                }
            });
        }
    };

    $scope.refreshGrid = function () {
        var grid = $("#gridZones").data("kendoGrid");
        if (grid) {
            grid.dataSource.read();
        }
    };

    init();
});
