//Biến dùng chung********************************************
const formatDate = 'dd/MM/yyyy';
const formatMonth = 'MM/yyyy';
const formatDateTime = 'dd/MM/yyyy HH:mm:ss';
const formatTime = 'HH:mm:ss';
const formatDateFilter = 'yyyy/MM/dd';
const formatDateTimeFilter = 'yyyy/MM/dd HH:mm:ss';

const pageableShort = {
    input: true,
    numeric: false,
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

const pageableShort_vi = {
    input: true,
    numeric: false,
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

const pageableShort_en = {
    input: true,
    numeric: false,
    messages: {
        display: "Displays {0}-{1} of {2} records",
        empty:""
        //page: "Nhập số trang",
        //of: "/ {0}",
        //next: "Trang sau",
        //previous: "Trang trước",
        //first: "Trang đầu",
        //last: "Trang cuối"
    }
};

const pageableShort_vi_nodisplay = {
    input: true,
    numeric: false,
    
    messages: {
        display: "",
        empty:"",
        page: "Nhập số trang",
        of: "/ {0}",
        next: "Trang sau",
        previous: "Trang trước",
        first: "Trang đầu",
        last: "Trang cuối"
    }
};
const pageableShort_en_nodisplay = {
    input: true,
    numeric: false,
    messages: {
        display: "",
    }
};

const pageableShort_nodisplay = {
    input: true,
    numeric: false,
    messages: {
        display: "",
        page: "Nhập số trang",
        of: "/ {0}",
        next: "Trang sau",
        previous: "Trang trước",
        first: "Trang đầu",
        last: "Trang cuối"
    }
};
const filterable = {
    mode: "row",
    messages: {
        and: "và",
        or: "hoặc",
        filter: "Lọc",
        clear: "Xóa lọc"
    }
}

//filter grid
const defaultFilterableGrid = {
    cell: {
        operator: "contains",
        suggestionOperator: "contains",
        showOperators: false
    }
}
const FilterableGrid_eq = {
    cell: {
        operator: "eq",
        suggestionOperator: "eq",
        showOperators: false
    }
}
const defaultDateFilterableGrid = {
    cell: {
        operator: "gte",
        suggestionOperator: "gte",
        showOperators: false
    }
}
const filterableGrid_GreatestOrEqual = {
    cell: {
        operator: "gte",
        suggestionOperator: "gte",
        showOperators: false
    }
}
const filterableGrid_False = {
    cell: {
        enabled: false
    }
}

//Hàm dùng chung *********************************************
function CreateSiteMap() {
    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            var location = window.location.href;
            var cururl = location.substring(location.lastIndexOf("#!/") + 3, location.length);
            if (cururl.indexOf("/") > 0)
                cururl = cururl.substring(0, cururl.indexOf("/"));
            if (cururl.indexOf("?") > 0)
                cururl = cururl.substring(0, cururl.indexOf("?"));

            xmlDoc = $.parseXML(this.responseText);
            $title = $(xmlDoc).find("siteMapNode[url='" + cururl + "']");
            parentArr = $title.parents();
            html = '';
            for (var i = parentArr.length - 1; i >= 0; i--) {
                var title = $(parentArr[i]).attr('title'),
                    url = $(parentArr[i]).attr('url'),
                    description = $(parentArr[i]).attr('description');
                if (title != undefined) {
                    if (title == "Trang chủ") {
                        html += '<a href="/index.html#!/home"> <span class="fa fa-home"></span>' +  $.i18n("menu_trangchu") + '</a>'
                    } else {
                        if (url == undefined) {
                            var menu = title.replace(" ", "");
                            html += ' <span class="fa fa-caret-right"></span><a style="cursor:pointer" onclick="openMenu(\'' + menu + '\')"> ' + $.i18n(title)  + ' </a>';
                        } else {
                            html += ' <span class="fa fa-caret-right"></span><a href="../../' + url + '"> ' + $.i18n(title) + ' </a>';
                        }
                    }
                }
            }
            html += '<span class="fa fa-caret-right"></span><a> ' + $.i18n($title.attr("title")) + ' </a>';
            //$("#sitemap").html(html);
        }
    };
    xhttp.open("GET", "../sitemap.xml", true);
    xhttp.send();
}
function openMenu(id) {
    console.log("#" + id)
    console.log($("#" + id).addClass("open"))
}

function formatNumberInFooterGrid(nameField, digits) {
    return '#= kendo.toString(' + nameField + ', "' + digits + '")#';
}
function formatNumberInGrid(digits) {
    return "{0:" + digits + "}";
}

function formatNumberWithN3(nameField) {
    return '#= kendo.toString(' + nameField + ', "n3")#';
}

function hideLoadingPage() {
    setTimeout(function () {
        $('.loader').hide();
    }, 100)
}
function showLoadingPage() {
    $('.loader').show();
}

function commonDownFile(response) {
    let data = response.data;

    headers = response.headers();

    let filename = headers['x-filename'];
    let contentType = headers['content-type'];

    let linkElement = document.createElement('a');
    try {
        let blob = new Blob([data], { type: contentType });
        let url = window.URL.createObjectURL(blob);

        linkElement.setAttribute('href', url);
        linkElement.setAttribute("download", filename);

        let clickEvent = new MouseEvent("click", {
            "view": window,
            "bubbles": true,
            "cancelable": false
        });
        linkElement.dispatchEvent(clickEvent);
    } catch (ex) {
        console.log(ex);
        Notification({ title: $.i18n('label_thongbao'), message: 'Lỗi tải file, vui lòng load lại trang' }, 'warning');
    }
}
function commonGetRowSelected(gridid) {
    let listRowsSelected = [];
    var grid = $(gridid).data("kendoGrid");
    grid.select().each(function () {
        let dataItem = grid.dataItem(this);
        if (listRowsSelected.indexOf(dataItem) == -1) {
            listRowsSelected.push(dataItem);
        }
    });
    return listRowsSelected;
}

function commonOpenLoadingText(id) {
    setTimeout(function () {
        let $this = $(id);
        let loadingText = '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>' + $.i18n('label_dangxuly') + '...';
        if ($this.html() !== loadingText) {
            $this.data('original-text', $this.html());
            $this.html(loadingText);
        }
    }, 100);
}

function commonCloseLoadingText(id) {
    setTimeout(function () {
        let $this = $(id);
        $this.html($this.data('original-text'));
    },100)
}

function openConfirm(message, acceptAction, cancelAction, data) {
    var scope = angular.element("#mainContentId").scope();
    var textHuy = scope.lang == 'vi-vn' ? 'Hủy' : 'Cancel';
    var textDongY = scope.lang == 'vi-vn' ? 'Đồng ý' : 'Accept';
    $(" <div id='confirmDelete'></div>").appendTo("body").kendoDialog({
        width: "450px",
        closable: true,
        modal: true,
        title: scope.lang == 'vi-vn' ? 'Xác nhận!' : 'Comfirm',
        content: message,
        actions: [
            {
                text: textHuy, primary: false, action: function () {
                    if (cancelAction != null) {
                        scope[cancelAction](data);
                    }
                }
            },
            {
                text: textDongY, primary: true, action: function () {
                    scope[acceptAction](data);
                }
            }
        ],
    })
}


Date.prototype.addDays = function (days) {
    var date = new Date(this.valueOf());
    date.setDate(date.getDate() + days);
    return date;
}











