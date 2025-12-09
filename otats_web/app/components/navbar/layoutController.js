
angular.module('app').controller('layoutController', function ($scope, $http, $rootScope, $interval, $cookies, $state, $location, Notification, layoutService) {
    let stopTime;

    function init() {
        $scope.count = {
            counttinnhan: 0,
            counthoatdong: 0,
            countdonhang: 0,
        }

        generate_navbar();

        LoadNotiDonHang();
        LoadNotiHoatDong();
        LoadNotiTinNhan();
    }

    function generate_navbar() {
        $scope.tenadmin = $rootScope.UserInfo.tenAdmin;
        console.log($rootScope.UserInfo);
        if ($rootScope.isAdmin == 1) {
            $http({
                method: 'GET',
                url: urlApi + '/api/chucnang/tree',
                beforeSend: function () {
                    if ($rootScope.Authorization == undefined || $rootScope.Authorization == null || $rootScope.Authorization == "") {
                        $location.path('/login');
                    }
                }
            }).then(function successCallback(response) {
                $scope.itemCollection = response.data;

            });
        } else if ($rootScope.UserInfo.isHDV) {
            $scope.itemCollection = [
                {
                    "$id": "1",
                    "nodes": [
                        {
                            "$id": "2",
                            "parent": {
                                "$ref": "1"
                            },
                            "id": 696992,
                            "parentId": 3,
                            "text": "Danh sách đoàn",
                            "icon": "fa fa-money",
                            "url": "danhsachdonhanghdv",
                            "urL_NEW": "danhsachdonhanghdv"
                        },
                        {
                            "$id": "3",
                            "parent": {
                                "$ref": "1"
                            },
                            "id": 696993,
                            "parentId": 3,
                            "text": "Đăng ký vé FOC cho HDV",
                            "icon": "fa fa-money",
                            "url": "dangkyfoc",
                            "urL_NEW": "dangkyfoc"
                        }
                    ],
                    "parent": null,
                    "id": 3,
                    "parentId": 0,
                    "text": "Hỗ trợ đoàn khách",
                    "icon": "fa fa-money",
                    "url": "#",
                    "urL_NEW": "#"
                }
            ]
        }
        else {
            $scope.itemCollection = [
                {
                    "$id": "1",
                    "nodes": [
                        {
                            "$id": "2",
                            "parent": {
                                "$ref": "1"
                            },
                            "id": 696992,
                            "parentId": 3,
                            "text": $.i18n('menu_danhsachdonhang'),
                            "icon": "fa fa-money",
                            "url": "DSDonHang.aspx",
                            "urL_NEW": "danhsachdonhangpkd"
                        },
                        {
                            "$id": "2",
                            "parent": {
                                "$ref": "1"
                            },
                            "id": 696993,
                            "parentId": 3,
                            "text": "3.2 POS",
                            "icon": "fa fa-money",
                            "url": "DSDonHang.aspx",
                            "urL_NEW": "posbanve"
                        }
                    ],
                    "parent": null,
                    "id": 3,
                    "parentId": 0,
                    "text": $.i18n('menu_donhang'),
                    "icon": "fa fa-money",
                    "url": "#",
                    "urL_NEW": "#"
                }
            ]
        }
    }

    //Không dùng hàm này nữa
    function init_signalr() {
        $.connection.hub.url = urlApi + "/signalr";

        let notificationsdonhang = $.connection.notiDonHangHub;
        console.log(notificationsdonhang);
        notificationsdonhang.client.callMessage = function () {
            console.log(123);
            LoadNotiDonHang()
        };

        $.connection.hub.start().done(function () {
            //LoadNotiDonHang();
            notificationsdonhang.server.Send();
            console.log(12);
        }).fail(function (e) {
            console.log(e);
        });


    }

    function loadnoti() {
        LoadNotiDonHang();
        //LoadNotiHoatDong();
        //LoadNotiTinNhan();
    }
    function LoadNotiDonHang() {
        layoutService.LoadNotiDonHang().then(function (result) {
            $scope.count.countdonhang = result.data.length;
            $scope.donHangCollection = result.data;

            if ($scope.count.countdonhang > 0) {
                let lastitem = result.data[0];
                if ($rootScope.lastiddonhang != lastitem.iD_DonHang && lastitem.iD_DonHang > 0) {

                    $rootScope.lastiddonhang = lastitem.iD_DonHang

                    let cookieExp = new Date();
                    cookieExp.setDate(cookieExp.getDate() + 1);
                    $cookies.putObject('lastiddonhang', $rootScope.lastiddonhang, { expires: cookieExp });

                    $scope.noti = {
                        nTitle: $.i18n('label_canhbaodonhangmoi'),
                        tenKhachHang: lastitem.tenKhachHang,
                        thoiGian: lastitem.thoiGian,
                        soTien: lastitem.soTien,
                        length: result.data.length,
                        iD_DonHang: lastitem.iD_DonHang
                    }

                    Notification.info({ templateUrl: "custom_template.html", scope: $scope });
                }
            }
        });
    }
    function LoadNotiHoatDong() {
        layoutService.LoadNotiHoatDong().then(function (result) {
            $scope.count.counthoatdong = result.data.length;
            $scope.hoatDongCollection = result.data;
        });
    }
    function LoadNotiTinNhan() {
        layoutService.LoadNotiTinNhan().then(function (result) {
            $scope.count.counttinnhan = result.data.length;
            $scope.tinNhanCollection = result.data;
        });
    }

    //event
    init();
    //stopTime = $interval(loadnoti, 60000);

    $scope.fomartnumber = function (data) {
        return kendo.toString(data, $rootScope.UserInfo.dinhDangSo);
    }

    $scope.nClick = function (_id) {
        let url = $state.href('editdonhang', { iddonhang: _id });
        window.open(url, '_blank');
        //$state.go('editdonhang', { iddonhang: _id });
    }
    $scope.openpage = function (_url) {
        if (_url == '/assets/files/HuongDanSuDung_Ksmart.pdf') {
            let hdsd = '/assets/files/HuongDanSuDung_Ksmart.pdf';
            if (!($rootScope.lang == 'vi-vn'))
                hdsd = '/assets/files/HuongDanSuDung_Ksmart_en.pdf';

            window.open("../../" + hdsd, "_blank");
        }
        else if (_url == '/assets/files/HuongDanSuDung_OTA.pdf') {
            let hdsd = '/assets/files/HuongDanSuDung_OTA.pdf';
            window.open("../../" + hdsd, "_blank");
        }
        else
            $state.go(_url);
    }

    $scope.imgurl = function (_url) {
        return (_url == '') ? 'assets/img/noimage.png' : _url
    }

    $scope.xemtatcahoatdong = function () {
        $state.go('lichsuthaotac');
    }
    $scope.xemchitiethoatdong = function (_item) {
        if (_item.loai == 3 || _item.loai == 4)
            $state.go('lichsuvaoradiem');
        if (_item.loai == 5)
            $state.go('album', { idalbum: _item.id });
        if (_item.loai == 6)
            $state.go('editdonhang', { iddonhang: _item.id });

    }

    $scope.xemtatcatinnhan = function () {
        $state.go('baocaoguitinnhan', { chuadoc: 1 });
    }
    $scope.xoatatcatinnhan = function () {
        layoutService.clearnoti(2).then(function (result) {
            $scope.count.counttinnhan = result.data.length;
            $scope.tinNhanCollection = result.data;
        });
    }
    $scope.xemchitiettinnhan = function (_id) {
        let arr_tinnhanall = $scope.tinNhanCollection;
        let arr_tinnhanchuachon = arr_tinnhanall.filter((item) => {
            return (item.iD_TINNHAN != _id)
        })

        $scope.count.counttinnhan = arr_tinnhanchuachon.length;
        $scope.tinNhanCollection = arr_tinnhanchuachon;

        $state.go('baocaochitiettinnhan', { idtinnhan: _id });
    }

    $scope.xemtatcadonhang = function () {
        $state.go('danhsachdonhang', { idtrangthaixem: 0 });
    }
    $scope.xoatatcadonhang = function () {
        layoutService.clearnoti(1).then(function (result) {
            $scope.count.countdonhang = result.data.length;
            $scope.donHangCollection = result.data;
        });
    }
    $scope.xemchitietdonhang = function (_id) {
        let arr_donhangall = $scope.donHangCollection;
        let arr_donhangchuaxem = arr_donhangall.filter((item) => {
            return (item.iD_DonHang != _id)
        })

        $scope.count.countdonhang = arr_donhangchuaxem.length;
        $scope.donHangCollection = arr_donhangchuaxem;

        $state.go('editdonhang', { iddonhang: _id });
    }

});
