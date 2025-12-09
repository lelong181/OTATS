
var dialogRoot = $("#dialogRoot").kendoDialog().data("kendoDialog").close();
// Hàm bật dialog thông báo
function openDialog(dialogRoot, message) {
    dialogRoot.setOptions({
        width: "450px",
        closable: true,
        modal: true,
        title: "Thông báo!",
        content: message,
        actions: []
    });
    dialogRoot.open();
}
// Hàm bật dialog confirm

//Kiểm tra thiết bị
var isMobile = {
    Android: function () {
        return navigator.userAgent.match(/Android/i);
    },
    BlackBerry: function () {
        return navigator.userAgent.match(/BlackBerry/i);
    },
    iOS: function () {
        return navigator.userAgent.match(/iPhone|iPad|iPod/i);
    },
    Opera: function () {
        return navigator.userAgent.match(/Opera Mini/i);
    },
    Windows: function () {
        return navigator.userAgent.match(/IEMobile/i);
    },
    any: function () {
        return (isMobile.Android() || isMobile.BlackBerry() || isMobile.iOS() || isMobile.Opera() || isMobile.Windows());
    }
};


function openConfirm(dialogRoot, message, acceptAction, cancelAction) {
    var result;
    dialogRoot.setOptions({
        width: "450px",
        closable: true,
        modal: true,
        title: "Xác nhận!",
        content: message,
        actions: [
            {
                text: 'Đồng ý', action: function () {
                    acceptAction();
                    result = true;
                }
            },
            {
                text: 'Hủy', action: function () {
                    cancelAction();
                    result = false;
                }
            }

        ],
    });
    dialogRoot.open();
    return result;
}
var notification;
function notify() {
    //#notify
    var wWidth = $(window).width(),
        newLeft = Math.floor(wWidth / 2 - 400 / 2);
    notification = $("<div />").kendoNotification({
        position: {
            //pinned: true,
            top: 50,
            right: newLeft
        },
        templates: [{
            type: "error",
            template: $("#errorTemplate").html()
        }, {
            type: "success",
            template: $("#successTemplate").html()
        }]
    }).data("kendoNotification");
    //console.log(notification);
}
notify();

// Hàm set validate input
function SetValidate(input) {
    var valid = true;
    var inputtag = $("#" + input);
    //console.log(input + " = " + inputtag.val());

    inputtag.keyup(function () {
        if (inputtag.val() == "" || inputtag.val().trim() == "") {
            inputtag.parents().find("div[toggle-valid='" + input + "']").removeClass("valid").addClass("invalid");
        } else {
            inputtag.parents().find("div[toggle-valid='" + input + "']").removeClass("invalid").addClass("valid");
        }
    })
    inputtag.change(function () {
        if (inputtag.val() == "" || inputtag.val().trim() == "") {
            inputtag.parents().find("div[toggle-valid='" + input + "']").removeClass("valid").addClass("invalid");
        } else {
            inputtag.parents().find("div[toggle-valid='" + input + "']").removeClass("invalid").addClass("valid");
        }
    })
    if (inputtag.val() == "" || inputtag.val().trim() == "") {
        valid = false;
        inputtag.parents().find("div[toggle-valid='" + input + "']").removeClass("valid").addClass("invalid");
    } else {
        inputtag.parents().find("div[toggle-valid='" + input + "']").removeClass("invalid").addClass("valid");
    }
    if (inputtag.data("kendoDropDownTree") != undefined) {
        if (inputtag.data("kendoDropDownTree").value() == "") {
            valid = false;
            inputtag.parents().find("div[toggle-valid='" + input + "']").removeClass("valid").addClass("invalid");
        } else {
            inputtag.parents().find("div[toggle-valid='" + input + "']").removeClass("invalid").addClass("valid");
        }
        return valid;
    }


    return valid;
}
//Hàm disable validate
function DisableValidate(input) {
    var valid = true;
    var inputtag = $("#" + input);
    //console.log(input + " = " + inputtag.val());
    inputtag.keyup(function () {

    })
    inputtag.change(function () {

    })
    inputtag.parents().find("div[toggle-valid='" + input + "']").removeClass("invalid").addClass("valid");
}

// Hàm tạo sitemap


var defaultFilterableGrid = {
    cell: {
        operator: "contains",
        suggestionOperator: "contains",
        showOperators: false
    }
}

// cofig cột với checkbox
var columnCheckBox = {
    locked: true,
    lockable: false,
    selectable: true,
    width: 40
}

// config cột với button delete trên dòng
var columnDelete = function (idGrid) {
    var template = '<button ng-click="deleteRowInGridView($event,\'' + idGrid + '\')" class="k-link k-button k-button-menu k-button-menu-row" ><img src="img/delete.svg" class="imgToolbar" /></button>';
    var obj = {
        template: template,
        title: "", width: 45,
        locked: true,
        lockable: false,
    }
    return obj;
}

function setColumStt(idGrid) {
    var obj = {
        //locked: true,
        lockable: false,
        title: $.i18n("label_khachhang_34"),
        template: dataItem => ($(idGrid).data("kendoGrid").dataSource.page() - 1) *
            $(idGrid).data("kendoGrid").dataSource.pageSize() +
            $(idGrid).data("kendoGrid").dataSource.indexOf(dataItem) + 1,
        width: 45,
        headerAttributes: { style: "text-align:center" },
        attributes: { class: "text-center" },
    }
    return obj;
}
function setColumStt2(idGrid) {
    var obj = {
        //locked: true,
        lockable: false,
        title: $.i18n("label_donHang_119"),
        template: "#= ++RecordNumber #",
        width: 45,
        headerAttributes: { style: "text-align:center" },
        attributes: { class: "text-center" },
    }
    return obj;
}


var pageableShort = {
    input: true,
    numeric: false,
    pageSizes: ["All", 5, 10, 20, 30, 50],
    //refresh: true,
    //pageSizes: true,
    //buttonCount: 5,
    messages: {
        itemsPerPage: "Bản ghi trên trang",
        display: "Hiển thị {0}-{1} trong số {2} bản ghi",
        page: "Trang",
        of: "/ {0}",
        next: "Trang sau",
        previous: "Trang trước",
        first: "Trang đầu",
        last: "Trang cuối"
    }
};
var filterable = {
    mode: "row",
    messages: {
        and: "và",
        or: "hoặc",
        filter: "Lọc",
        clear: "Xóa lọc"
    }
}


// sự kiện autoFit Column
function setGridWidth(e) {
    for (var i = 1; i < e.sender.columns.length; i++) {
        e.sender.autoFitColumn(i);
    }
}

var pageableDefault = {
    input: true,
    numeric: false,
    //refresh: true,
    messages: {
        display: "Hiển thị {0}-{1} trong số {2} bản ghi",
        page: "Nhập số trang",
        of: "/ {0}",
        next: "Trang sau",
        previous: "Trang trước",
        first: "Trang đầu",
        last: "Trang cuối"
    }
};

function mouseenterGrid(e, idGrid) {
    var row = $(e.target).closest("tr");
    var uid = row.data().uid;
    var tr = $(idGrid).data().kendoGrid.element.find("tbody tr[data-uid=" + uid + "]");
    $(tr).addClass('k-state-hover');
}

function mouseleaveGrid(e, idGrid) {
    var row = $(e.target).closest("tr");
    var uid = row.data().uid;
    var tr = $(idGrid).data().kendoGrid.element.find("tbody tr[data-uid=" + uid + "]");
    $(tr).removeClass('k-state-hover');
}
/**
 * show loading in grid
 * @param {any} gridElement định dạng "#id_grid"
 * @author VTLan
 */
function showLoading(gridElement) {
    kendo.ui.progress($(gridElement), true);
}

/**
 * hide loading in grid
 * @param {any} gridElement định dạng "#id_grid"
 * @author VTLan
 */
function hideLoading(gridElement) {
    setTimeout(function () {
        kendo.ui.progress($(gridElement), false);
    }, 10)
}
/**
 * resize grid 
 * @param {any} gridElement định dạng "#id_grid"
 * @author VTLan
 */
function resizeGrid(idGrid) {
    var heightNavbar = $('#navbar').innerHeight();
    var paddingTop = heightNavbar + 130;
    var newWidth = $('body').innerWidth() - 20;
    var oldWidth = $(".k-grid-content table").width() - 20;
    $(idGrid).css("height", "calc(100vh - " + paddingTop + "px)");
    if (oldWidth < newWidth) {
        $(".k-grid-content").css("padding-right", "18px");
        $(".k-grid-content table").css("width", newWidth + "px");
        $(".k-grid-header-wrap table").css("width", newWidth + "px");
    }
}
/**
 * hide loading in page
 * @param {any} gridElement định dạng "#id_grid"
 * @author VTLan
 */
function hideLoadingPage() {
    setTimeout(function () {
        $('.loader').hide();
    }, 100)
}
/**
 * show loading in page
 * @param {any} gridElement định dạng "#id_grid"
 * @author VTLan
 */
function showLoadingPage() {
    $('.loader').show();
}


/**vuongtm*************************************************************
*
*/
//config grid options angular kendo
function setOptionsGridPagingServer(idGrid, columns, pageable, editable, group, filterable, titleExcel) {
    if (group != null) {
        dataSources.group(group);
    }

    // hàm config grid
    var config = {
        height: function () {
            var heightGrid = $(document).height() - ($("#navbar").height() + $(".toolBarMenu").height() + $(".toolbar").height() + $(".sitemap").height());
            return heightGrid - 10;
        },
        sortable: true,
        persistSelection: true,
        property: true,
        excelExport: function (e) {
            if (titleExcel != null) {
                excelExport(e, titleExcel);
            }
        },
        pageable: pageable,
        //autoFitColumn: true,
        //dataBinding: function () {
        //    RecordNumber = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        //},
        resizable: true,
        autoSync: true,
        autoBind: true,
        dataBound: function (e) {
            var scope = angular.element("#mainContentId").scope();
            scope.dataBoundGrid(e, idGrid);
        },
        //columnShow: function (e) {
        //    setGridWidth(e);
        //},
        //columnHide: function (e) {
        //    setGridWidth(e);
        //},
        columns: columns,
        editable: editable
    };

    if (filterable) {
        config.filterable = {
            mode: "row",
            messages: {
                and: "và",
                or: "hoặc",
                filter: "Lọc",
                clear: "Xóa lọc"
            }
        }
    }

    return config;
}

//END vuongtm*************************************************************

function OpenLayoutWindow(page, windowOptions) {
    if (windowOptions == null) {
        windowOptions = {
            width: "20%",
            height: "20%",
            title: ""
        }
    }
    if ($("#LayoutWindow").data("kendoWindow") == undefined) {
        $("#LayoutWindow").kendoWindow({
            width: windowOptions.width,
            height: windowOptions.height,
            title: windowOptions.title,
            visible: false,
            modal: true,
            resizable: false,
            close: function (e) {
                $('#LayoutWindowContent').empty();
            },
            actions: [
                "Close"
            ]
        })
    }
    else {
        var dialog1 = $("#LayoutWindow").data("kendoWindow");
        dialog1.setOptions({
            width: windowOptions.width,
            height: windowOptions.height,
            title: windowOptions.title
        });
    }

    $("#LayoutWindowContent").load(page);

    if ($(window).width() > 800) {
        $("#LayoutWindow").data("kendoWindow").center().open();
    } else {
        $("#LayoutWindow").data("kendoWindow").center().maximize().open();
    }
}

function CloseLayoutWindow() {
    if ($("#LayoutWindow").data("kendoWindow") != undefined) {
        $("#LayoutWindow").data("kendoWindow").close();
    }
}


