angular.module('app').controller('hotelsController', function ($scope, $rootScope, $timeout, Notification, hotelsDataService) {
    $scope.title = "Quản lý Khách sạn";
    // Ensure safe access to UserInfo
    $scope.idQLLH = ($rootScope.UserInfo && $rootScope.UserInfo.id) ? $rootScope.UserInfo.id : 0;

    $scope.hotels = [];
    $scope.isEdit = false;
    $scope.currentHotel = {};

    window.record = 0;

    function init() {
        loadgrid();
    }

    function loadgrid() {
        $scope.gridOptions = {
            dataSource: {
                transport: {
                    read: function (e) {
                        hotelsDataService.getAll($scope.idQLLH).then(function (response) {
                            if (response.flag) {
                                e.success(response.data);
                            } else {
                                e.success([]);
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
                        id: "hotelId",
                        fields: {
                            hotelId: { type: "number" },
                            hotelName: { type: "string" },
                            code: { type: "string" },
                            address: { type: "string" },
                            phone: { type: "string" },
                            email: { type: "string" },
                            website: { type: "string" },
                            logoUrl: { type: "string" },
                            isActive: { type: "boolean" },
                            id_QLLH: { type: "number" }
                        }
                    }
                }
            },
            sortable: true,
            pageable: {
                refresh: true,
                pageSizes: [10, 20, 50, 100],
                messages: {
                    display: "{0} - {1} của {2} dòng",
                    empty: "Không có dữ liệu",
                    page: "Trang",
                    of: "của {0}",
                    itemsPerPage: "dòng/trang",
                    first: "Trang đầu",
                    previous: "Trang trước",
                    next: "Trang sau",
                    last: "Trang cuối"
                }
            },
            resizable: true,
            selectable: "multiple, row",
            filterable: {
                mode: "row"
            },
            height: function () {
                var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height() + $(".toolbarmenu").height());
                return heightGrid - 40;
            },
            dataBinding: function () {
                window.record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
            },
            columns: [
                { headerAttributes: { "class": "table-header-cell", style: "text-align: center" }, selectable: true, width: "40px" },
                {
                    title: "#", template: "#= ++window.record #", width: "50px",
                    headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
                    attributes: { class: "text-center" }
                },
                {
                    field: "hotelName", title: "Tên Khách sạn",
                    headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
                    filterable: { cell: { operator: "contains" } }
                },
                {
                    field: "code", title: "Mã", width: "120px",
                    headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
                    attributes: { class: "text-center" },
                    filterable: { cell: { operator: "contains" } }
                },
                {
                    field: "address", title: "Địa chỉ",
                    headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
                    filterable: { cell: { operator: "contains" } }
                },
                {
                    field: "phone", title: "Điện thoại", width: "120px",
                    headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
                    attributes: { class: "text-center" },
                    filterable: { cell: { operator: "contains" } }
                },
                {
                    field: "isActive", title: "Trạng thái", width: "120px",
                    headerAttributes: { "class": "table-header-cell", style: "text-align: center" },
                    attributes: { class: "text-center" },
                    template: '<span class="#= isActive ? "color-success" : "color-danger" #">#= isActive ? "Hoạt động" : "Ngừng" #</span>',
                    filterable: false
                }
            ]
        };

        // Add double click event to open edit
        $("#gridHotels").on("dblclick", "tr.k-state-selected", function () {
            $scope.$apply(function () {
                $scope.openEdit();
            });
        });

        // Ensure row selection on click (if not handled by selectable: true on column)
        // Actually, with selectable: true on column, Kendo usually handles checkbox click.
        // To handle row click -> select checkbox, we can simply add selectable: "row" or handle click.
        // Let's try explicit handler to be safe as user requested "click row also selects".
        $("#gridHotels").on("click", "tr[role='row']", function () {
            var grid = $("#gridHotels").data("kendoGrid");
            var row = $(this);
            // Toggle selection is tricky if we don't want to interfere with checkbox. 
            // Most reliable is standard Kendo "selectable" option. 
            // But let's assume Kendo configuration needs to be:
            // configurable via 'selectable' option in grid declaration.
        });
    }

    $scope.openAdd = function () {
        $scope.isEdit = false;
        $scope.currentHotel = {
            id_QLLH: $scope.idQLLH,
            hotelName: "",
            code: "",
            address: "",
            phone: "",
            email: "",
            website: "",
            logoUrl: "",
            isActive: true
        };
        // Use Kendo Window
        if ($scope.hotelWindow) {
            $scope.hotelWindow.center().open();
        }
    };

    $scope.openEdit = function () {
        var grid = $("#gridHotels").data("kendoGrid");
        var selectedRows = grid.select();
        if (selectedRows.length === 0) {
            Notification.warning({ message: 'Vui lòng chọn khách sạn để sửa' });
            return;
        }
        if (selectedRows.length > 1) {
            Notification.warning({ message: 'Vui lòng chỉ chọn một khách sạn để sửa' });
            return;
        }

        var dataItem = grid.dataItem(selectedRows[0]);
        $scope.isEdit = true;
        $scope.currentHotel = angular.copy(dataItem);
        $scope.currentHotel.id_QLLH = $scope.currentHotel.id_QLLH || $scope.idQLLH;

        // Use Kendo Window
        if ($scope.hotelWindow) {
            $scope.hotelWindow.center().open();
        }
    };

    $scope.saveHotel = function () {
        if (!$scope.currentHotel.hotelName) {
            Notification.warning({ message: 'Vui lòng nhập tên khách sạn' });
            return;
        }

        if ($scope.isEdit) {
            hotelsDataService.update($scope.currentHotel).then(function (response) {
                if (response.flag) {
                    Notification.success({ message: response.message });
                    if ($scope.hotelWindow) $scope.hotelWindow.close();
                    $scope.refreshGrid();
                } else {
                    Notification.error({ message: response.message });
                }
            });
        } else {
            hotelsDataService.insert($scope.currentHotel).then(function (response) {
                if (response.flag) {
                    Notification.success({ message: response.message });
                    if ($scope.hotelWindow) $scope.hotelWindow.close();
                    $scope.refreshGrid();
                } else {
                    Notification.error({ message: response.message });
                }
            });
        }
    };

    $scope.confirmDelete = function () {
        var grid = $("#gridHotels").data("kendoGrid");
        var selectedRows = grid.select();
        if (selectedRows.length === 0) {
            Notification.warning({ message: 'Vui lòng chọn khách sạn để xóa' });
            return;
        }

        if (confirm("Bạn có chắc chắn muốn xóa " + selectedRows.length + " khách sạn đã chọn không?")) {
            // Loop through selected rows and delete (or generic multi-delete if API supported, but spec only mentions delete by ID)
            // Ideally we should use a Promise.all or chain deletions. For now, let's delete individually or ask user if backend supports list.
            // Spec: HotelsModel object.
            // Let's assume one by one or last one selected for simplicity unless I see bulk delete.
            // KhachHang uses `del(data)` which takes an array. Hotels spec 2.5 `POST /api/hotels/delete?id={id}` suggests single.
            // I will iterate and delete. 

            var promises = [];
            selectedRows.each(function () {
                var dataItem = grid.dataItem(this);
                promises.push(hotelsDataService.deleteHotel(dataItem.hotelId));
            });

            Promise.all(promises).then(function (results) {
                var failed = results.filter(function (r) { return !r.flag; });
                if (failed.length === 0) {
                    Notification.success({ message: 'Xóa thành công' });
                } else {
                    Notification.warning({ message: 'Có ' + failed.length + ' mục xóa thất bại' });
                }
                $scope.refreshGrid();
            });
        }
    };

    $scope.refreshGrid = function () {
        var grid = $("#gridHotels").data("kendoGrid");
        if (grid) {
            grid.dataSource.read();
        }
    };

    init();
});
