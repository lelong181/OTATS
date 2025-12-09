angular.module('app').controller('loTrinhDiChuyenController', function ($rootScope, $scope, $stateParams, $location, $timeout, $interval, $compile, NgMap, Notification, ComboboxDataService, mapsDataService) {
    CreateSiteMap();

    let BanDo;

    let $lotrinharr = [];
    let $lotrinh = {};
    let $arrlotrinh_map = [];

    let Marker_LoTrinh_TrucTuyen = {};
    let Marker_LoTrinh_VaoDiem = {};
    let Marker_LoTrinh_RaDiem = {};
    let Marker_KeHoachDiChuyen = {};
    let MatTinHieu = {};
    let CoTinHieu = {};

    let $nhanvienlotrinh;
    let $chaylotrinh = false;
    let $counter = 0;
    let $scroll = 0;
    let $vitrilotrinh = 0;

    let $circle;
    let $thisInterval;

    let infowindow = new google.maps.InfoWindow;
    let activeInfoWindow;

    let infoDiemBatDau = null;
    let infoDiemKetThuc = null;

    let lineSymbol = {
        path: google.maps.SymbolPath.CIRCLE,
        scale: 5,
        strokeColor: '#005db5',
        strokeWidth: '#005db5'
    };

    let $loaiLoTrinh = 2;
    let __idnhanvien = -1;
    let __idnhom = 0;

    let param_from = '';
    let param_to = '';

    function init() {
        getquyen();
        initparam();
        $scope.phienlamviec = false;
        $scope.loai = '2';
        initmap();
        initdate();
        initcombo();
    }
    function initparam() {
        param_from = ($stateParams.tungay == undefined) ? '' : $stateParams.tungay;
        param_to = ($stateParams.denngay == undefined) ? '' : $stateParams.denngay;
        __idnhanvien = ($stateParams.idnhanvien == undefined) ? -1 : $stateParams.idnhanvien;
    }
    function getquyen() {
        let path = $location.path();
        let url = path.replace('/', '')
        ComboboxDataService.getquyen(url).then(function (result) {
            $scope.permission = result.data;
            if ($scope.permission.iD_ChucNang <= 0) {
                Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_khongcoquyentruycapchucnang') }, "error");
                $location.path('/home')
            }
        });
    }

    function initmap() {
        NgMap.getMap().then(function (map) {
            BanDo = map;
            //if (__idnhanvien > 0)
            loaddatamap();
        });
    }
    function initdate() {
        let dateNow = new Date();

        if (param_from != '') {
            $scope.obj_TuNgay = new Date(param_from);
        } else
            $scope.obj_TuNgay = new Date(dateNow.setHours(0, 0, 0));

        if (param_to != '') {
            $scope.obj_DenNgay = new Date(param_to);
        } else
            $scope.obj_DenNgay = new Date(dateNow.setHours(23, 59, 59));

        $scope.maxDate = $scope.obj_DenNgay;
        $scope.minDate = $scope.obj_TuNgay;
    }
    function initcombo() {
        ComboboxDataService.getDataNhomNhanVien().then(function (result) {
            $scope.nhomnhanvienData = result.data;

            if (__idnhanvien > 0)
                $timeout(function () { $("#nhanvien").data("kendoComboBox").value(__idnhanvien); }, 100);
        });
        ComboboxDataService.getDataNhanVienDangNhapPhanMem(-2).then(function (result) {
            if (result.flag) {
                $scope.nhanvienData = result.data;
            }
            else
                $scope.nhanvienData = [];
        });
    }

    function animateCircle(polyline) {
        var count = 0;
        // fallback icon if the poly has no icon to animate
        var defaultIcon = [
            {
                icon: lineSymbol,
                offset: '100%'
            }
        ];
        window.setInterval(function () {
            count = (count + 1) % 200;
            var icons = polyline.get('icons') || defaultIcon;
            icons[0].offset = (count / 2) + '%';
            polyline.set('icons', icons);
        }, 100);
    }
    function scrollUL(li) {
        // scroll UL to make li visible
        // li can be the li element or its id

        var ul = li.parentNode;
        // fudge adjustment for borders effect on offsetHeight
        var fudge = 4;
        // bottom most position needed for viewing
        var bottom = (ul.scrollTop + (ul.offsetHeight - fudge) - li.offsetHeight);
        // top most position needed for viewing
        var top = ul.scrollTop + fudge;
        if (li.offsetTop <= top) {
            // move to top position if LI above it
            // use algebra to subtract fudge from both sides to solve for ul.scrollTop
            ul.scrollTop = li.offsetTop - fudge;
        } else if (li.offsetTop >= bottom) {
            // move to bottom position if LI below it
            // use algebra to subtract ((ul.offsetHeight - fudge) - li.offsetHeight) from both sides to solve for ul.scrollTop
            ul.scrollTop = li.offsetTop - ((ul.offsetHeight - fudge) - li.offsetHeight);
        }
    };

    function loaddatamap() {
        let _loainoi = $scope.loai;

        if (_loainoi == -1) {
            loadmaplotrinh(_loainoi);
        }
        else if (_loainoi == 1 || _loainoi == 3)//lo trinh suy dien
        {
            if ($scope.phienlamviec) {
                loadmaplotrinhphienlamviec(_loainoi);
            }
            else {
                loadmaplotrinh(_loainoi);
            }
        }
        else if (_loainoi == 2)//lo trinh noi diem gps
            if ($scope.phienlamviec) {
                loadmaplotrinhphienlamviec(0);
            }
            else {
                loadmaplotrinh(0);
            }
        else {
            loadmaplotrinhkhongnoidiem(1);
        }
    }

    function loadmaplotrinh(_loainoi) {
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        $loaiLoTrinh = _loainoi;

        //let lb = new $.LoadingBox({ loadingImageSrc: "../../../assets/img/default.gif", });
        mapsDataService.getdatalotrinhnhanvien(__idnhanvien, 0, $loaiLoTrinh, fromdate, todate).then(function (response) {
            if (response.flag) {
                let $data = response.data;

                clearInterval($thisInterval);
                $counter = 0;
                let $arrlotrinh_vaodiem = $data.datavaodiem;
                let $arrlotrinh_radiem = $data.dataradiem;
                let $arrlotrinh_suydien = $data.datalotrinh_suydien;
                let $arrbanglotrinh = $data.databanglotrinh;
                let $arrlotrinh = $data.datalotrinh;
                let $arrmattinhieu = $data.datamattinhieu;
                let $arrKeHoach = $data.dataKeHoachDiChuyen;


                if (!$.isEmptyObject(Marker_KeHoachDiChuyen)) {
                    $.each(Marker_KeHoachDiChuyen, function (i, v) {
                        Marker_KeHoachDiChuyen[i].setMap(null);
                    });
                    Marker_KeHoachDiChuyen = {};
                }
                if (!$.isEmptyObject(MatTinHieu)) {
                    $.each(MatTinHieu, function (i, v) {
                        MatTinHieu[i].setMap(null);
                    });
                    MatTinHieu = {};
                }
                if (!$.isEmptyObject(CoTinHieu)) {
                    $.each(CoTinHieu, function (i, v) {
                        CoTinHieu[i].setMap(null);
                    });
                    CoTinHieu = {};
                }

                try {
                    if ($lotrinh != null) {
                        $lotrinh.setMap(null);
                    }
                    if ($nhanvienlotrinh != null) {
                        $nhanvienlotrinh.setMap(null);
                        $lotrinharr = [];
                    }
                } catch (e) { }

                try {
                    if (!$.isEmptyObject($arrlotrinh_map)) {
                        $.each($arrlotrinh_map, function (i, v) {
                            $arrlotrinh_map[i].setMap(null);
                        });
                    }
                } catch (e) { }

                if (!$.isEmptyObject(Marker_LoTrinh_VaoDiem)) {
                    $.each(Marker_LoTrinh_VaoDiem, function (i, v) {
                        Marker_LoTrinh_VaoDiem[i].setMap(null);
                    });
                    Marker_LoTrinh_VaoDiem = {};
                }
                if (!$.isEmptyObject(Marker_LoTrinh_RaDiem)) {
                    $.each(Marker_LoTrinh_RaDiem, function (i, v) {
                        Marker_LoTrinh_RaDiem[i].setMap(null);
                    });
                    Marker_LoTrinh_RaDiem = {};
                }

                if ($arrKeHoach != null) {
                    for (i = 0; i < $arrKeHoach.length; i++) {
                        //let daily_icon = boykatty.utilities.labelDaiLyColor($arrKeHoach[i].text_color, $arrKeHoach[i].TenKhachHang, "../../../assets/img/cuahang.png");
                        let daily_icon = {
                            url: 'assets/img/cuahang.png',
                            // This marker is 20 pixels wide by 32 pixels high.
                            size: new google.maps.Size(20, 32),
                            // The origin for this image is (0, 0).
                            origin: new google.maps.Point(0, 0),
                            // The anchor for this image is the base of the flagpole at (0, 32).
                            anchor: new google.maps.Point(0, 32),
                            labelOrigin: new google.maps.Point(8, -8)
                        }
                        let d = new Date($arrKeHoach[i].ThoiGianCheckInDuKien)
                        Marker_KeHoachDiChuyen[i] = new google.maps.Marker({
                            position: new google.maps.LatLng($arrKeHoach[i].ViDo, $arrKeHoach[i].KinhDo),
                            icon: daily_icon,
                            map: BanDo,
                            lable: $arrKeHoach[i].TenKhachHang,
                            content: "Khách hàng:" + $arrKeHoach[i].TenKhachHang + " <br/>- Địa chỉ:" + $arrKeHoach[i].DiaChi + " <br/>- Dự kiến:" + kendo.toString(d, 'dd/MM/yyyy HH:mm') + "<br/>- " + $arrKeHoach[i].text_color_mota
                        });

                        infowindow = new google.maps.InfoWindow();
                        google.maps.event.addListener(Marker_KeHoachDiChuyen[i], 'click', (function (marker, i, infowindow) {
                            return function () {
                                if (activeInfoWindow)
                                    activeInfoWindow.close();

                                infowindow.setContent(this.content);

                                infowindow.open(BanDo, this);
                                activeInfoWindow = infowindow;
                            };
                        })(Marker_LoTrinh_TrucTuyen[i], i, infowindow));
                    }
                }
                //set hiển thị điểm đầu và cuối
                let $diemdautien = $data.loTrinhDauTien;
                let $diemcuoi = $data.loTrinhCuoiCung;

                if ($diemdautien != null) {
                    $maker_diemdautien = new google.maps.Marker({
                        position: new google.maps.LatLng($diemdautien.vido, $diemdautien.kinhdo),
                        map: BanDo,
                        lable: $diemdautien.accuracy,
                        content: $.i18n('label_batdau') + " - " + $diemdautien.thoigian
                    });
                    infoDiemBatDau = new google.maps.InfoWindow({
                        content: $maker_diemdautien.content
                    });

                    infoDiemBatDau.open(BanDo, $maker_diemdautien);
                }

                if ($diemcuoi != null) {
                    $maker_diemcuoicung = new google.maps.Marker({
                        position: new google.maps.LatLng($diemcuoi.vido, $diemcuoi.kinhdo),
                        icon: "../../../assets/img/dungdo.png",
                        map: BanDo,
                        lable: $diemcuoi.accuracy,
                        content: $.i18n('label_ketthuc') + " - " + $diemcuoi.thoigian
                    });
                    infoDiemKetThuc = new google.maps.InfoWindow({
                        content: $maker_diemcuoicung.content
                    });

                    infoDiemKetThuc.open(BanDo, $maker_diemcuoicung);
                }
                $lotrinh_line = [];

                if ($arrlotrinh.length > 0) {
                    if ($loaiLoTrinh == 1 || $loaiLoTrinh == 3) {
                        //lo trinh suy dien
                        $.each($arrlotrinh_suydien, function ($i, $e) {
                            //ve lộ trình
                            $lotrinh_line.push(new google.maps.LatLng($e.vido, $e.kinhdo));
                        });

                        //duyệt qua mảng của lộ trình theo thiết bị để lấy accuracy map với bảng lộ trình ( vì bảng lộ trình không khớp với lộ trình suy diễn)
                        $.each($arrlotrinh, function ($i, $e) {
                            $lotrinharr.push($e);
                        });
                    }
                    else {
                        //lo trinh gps
                        if ($arrlotrinh.length > 0) {
                            $.each($arrlotrinh, function ($i, $e) {
                                $lotrinh_line.push(new google.maps.LatLng($e.vido, $e.kinhdo));
                                $lotrinharr.push($e);
                            });
                        }
                    }

                    try {
                        //set vi tri cho nhan vien trong lo trinh
                        $nhanvienlotrinh = new google.maps.Marker({
                            position: $lotrinh_line[0],
                            map: BanDo
                        });
                        BanDo.panTo($lotrinh_line[0]);
                        //ket thuc set vitri
                    } catch (e) { }

                    //Vẽ lộ trình online
                    $lotrinh = new google.maps.Polyline({
                        path: $lotrinh_line,
                        geodesic: true,
                        strokeColor: '#005db5',
                        strokeOpacity: 1,
                        strokeWeight: 2.5,
                        icons: [{
                            icon: lineSymbol,
                            offset: '100%'
                        }]
                    });

                    $lotrinh.setMap(BanDo);
                    animateCircle($lotrinh);

                    //Vẽ lộ trình offline
                    if ($loaiLoTrinh != 1 && $loaiLoTrinh != 3) {
                        if ($arrmattinhieu.length > 0 && $.isEmptyObject(MatTinHieu)) { //ve lo trinh mat tin hieu
                            $.each($arrmattinhieu, function ($i, $e) {
                                MatTinHieu[$i] = new google.maps.Polyline({
                                    path: [new google.maps.LatLng($e.diemdau.vido, $e.diemdau.kinhdo), new google.maps.LatLng($e.diemcuoi.vido, $e.diemcuoi.kinhdo)],
                                    geodesic: true,
                                    strokeColor: '#E7453D',
                                    strokeOpacity: 1,
                                    strokeWeight: 2.5
                                });
                                MatTinHieu[$i].setMap(BanDo);
                            });
                        }
                    }

                    loadbanglotrinh($arrbanglotrinh);

                    if (!$.isEmptyObject(Marker_LoTrinh_TrucTuyen)) {
                        $.each(Marker_LoTrinh_TrucTuyen, function (i, v) {
                            Marker_LoTrinh_TrucTuyen[i].setMap(null);
                        });
                        Marker_LoTrinh_TrucTuyen = {};
                    }

                    if ($loaiLoTrinh != 3) {

                        for (i = 0; i < $arrbanglotrinh.length; i++) {

                            if ($arrbanglotrinh[i].ghichu == "label_ngoaituyen") {
                                if ($arrbanglotrinh[i].thoiGianDungDo_Giay > 0) {
                                    Marker_LoTrinh_TrucTuyen[i] = new google.maps.Marker({
                                        position: new google.maps.LatLng($arrbanglotrinh[i].vido, $arrbanglotrinh[i].kinhdo),
                                        icon: "../../../assets/img/dungdo.png",
                                        map: BanDo,
                                        lable: $arrbanglotrinh[i].accuracy,
                                        content: $.i18n("label_dungdo") + $.i18n($arrbanglotrinh[i].ghichu) + " - " + $arrbanglotrinh[i].thoigian + "(" + $.i18n("label_ketthucdung") + ": " + $arrbanglotrinh[i].thoigianketthuc + ")"
                                    });
                                }
                                else {
                                    Marker_LoTrinh_TrucTuyen[i] = new google.maps.Marker({
                                        position: new google.maps.LatLng($arrbanglotrinh[i].vido, $arrbanglotrinh[i].kinhdo),
                                        icon: "../../../assets/img/FFFFFF-0.8.png",
                                        map: BanDo,
                                        lable: $arrbanglotrinh[i].accuracy,
                                        content: $.i18n($arrbanglotrinh[i].ghichu) + " - " + $arrbanglotrinh[i].thoigian + "(" + $.i18n('label_bankinh') + ": " + $arrbanglotrinh[i].accuracy + "," + $.i18n('label_vantoc') + ": " + $arrbanglotrinh[i].speed.toFixed(2) + ", " + $.i18n('header_tinhtrangpin') + ": " + $arrbanglotrinh[i].tinhtrangpin + ")"
                                    });
                                }
                                infowindow = new google.maps.InfoWindow();
                                google.maps.event.addListener(Marker_LoTrinh_TrucTuyen[i], 'click', (function (marker, i, infowindow) {
                                    return function () {
                                        if (activeInfoWindow) {
                                            activeInfoWindow.close();
                                        }

                                        if ($arrbanglotrinh[i].thoiGianDungDo_Giay > 0) {
                                            infowindow = new google.maps.InfoWindow({ content: "<div id='infowin'>" + $.i18n("label_thongtin") + "</div>" });

                                            mapsDataService.chitietdungdo($arrbanglotrinh[i].idnhanvien, $arrbanglotrinh[i].kinhdo, $arrbanglotrinh[i].vido, $arrbanglotrinh[i].thoigian, $arrbanglotrinh[i].thoigianketthuc).then(function (response) {
                                                infowindow.setContent(response.data);
                                            })
                                            //$.post('ajaxdata/chitietdungdo.aspx', { 'idnhanvien': $arrbanglotrinh[i].idnhanvien, 'kinhdo': $arrbanglotrinh[i].kinhdo, 'vido': $arrbanglotrinh[i].vido, 'thoigianbatdau': $arrbanglotrinh[i].thoigian, 'thoigianketthuc': $arrbanglotrinh[i].thoigianketthuc }, function (dt) {//truy xuat du lay du lieu tu server qua phuong thuc POST

                                            //    document.getElementById('infowin').innerHTML = dt;

                                            //});
                                        } else {
                                            infowindow.setContent(this.content);
                                        }

                                        infowindow.open(BanDo, this);
                                        activeInfoWindow = infowindow;

                                        $('#timelineContent li').removeClass('active');
                                        let ul = document.getElementById("timelineContent");
                                        let liArray = ul.getElementsByTagName("li");

                                        for (let k = 0; k < liArray.length; k++) {
                                            if (liArray[k].attributes["id"].value == "banglotrinh_" + $arrbanglotrinh[i].thoigian) {
                                                $('#timelineContent li').eq(k).addClass('active');
                                                ul.scrollTop = (liArray[k].offsetTop - 20);
                                            }
                                        }

                                        if ($circle != null) {
                                            $circle.setMap(null);
                                        }
                                        $circle = new google.maps.Circle({
                                            center: marker.getPosition(),
                                            radius: marker.lable,
                                            map: BanDo,
                                            fillColor: '#0000FF',
                                            strokeOpacity: 0.8,
                                            strokeWeight: 2,
                                            strokeColor: '#0000FF',
                                            fillOpacity: 0.35
                                        });
                                    };
                                })(Marker_LoTrinh_TrucTuyen[i], i, infowindow));
                            }
                            else if ($arrbanglotrinh[i].ghichu == "label_tructuyen") {
                                if ($arrbanglotrinh[i].thoiGianDungDo_Giay > 0) {
                                    Marker_LoTrinh_TrucTuyen[i] = new google.maps.Marker({
                                        position: new google.maps.LatLng($arrbanglotrinh[i].vido, $arrbanglotrinh[i].kinhdo),
                                        icon: "../../../assets/img/dungdo.png",
                                        map: BanDo,
                                        lable: $arrbanglotrinh[i].accuracy,
                                        content: $.i18n("label_dungdo") + $.i18n($arrbanglotrinh[i].ghichu) + " - " + $arrbanglotrinh[i].thoigian + "(" + $.i18n("label_ketthucdung") + ": " + $arrbanglotrinh[i].thoigianketthuc + ")"
                                    });
                                }
                                else {
                                    Marker_LoTrinh_TrucTuyen[i] = new google.maps.Marker({
                                        position: new google.maps.LatLng($arrbanglotrinh[i].vido, $arrbanglotrinh[i].kinhdo),
                                        icon: "../../../assets/img/FFFFFF-0.8.png",
                                        map: BanDo,
                                        lable: $arrbanglotrinh[i].accuracy,
                                        content: $.i18n($arrbanglotrinh[i].ghichu) + " - " + $arrbanglotrinh[i].thoigian + "(" + $.i18n('label_bankinh') + ": " + $arrbanglotrinh[i].accuracy + "," + $.i18n('label_vantoc') + ": " + $arrbanglotrinh[i].speed.toFixed(2) + ", " + $.i18n('header_tinhtrangpin') + ": " + $arrbanglotrinh[i].tinhtrangpin + ")"
                                    });
                                }
                                infowindow = new google.maps.InfoWindow();
                                google.maps.event.addListener(Marker_LoTrinh_TrucTuyen[i], 'click', (function (marker, i, infowindow) {
                                    return function () {
                                        if (activeInfoWindow) {
                                            activeInfoWindow.close();
                                        }

                                        if ($arrbanglotrinh[i].thoiGianDungDo_Giay > 0) {
                                            infowindow = new google.maps.InfoWindow({ content: "<div id='infowin'>" + $.i18n("label_thongtin") + "</div>" });

                                            mapsDataService.chitietdungdo($arrbanglotrinh[i].idnhanvien, $arrbanglotrinh[i].kinhdo, $arrbanglotrinh[i].vido, $arrbanglotrinh[i].thoigian, $arrbanglotrinh[i].thoigianketthuc).then(function (response) {
                                                infowindow.setContent(response.data);
                                            })

                                            //$.post('ajaxdata/chitietdungdo.aspx', { 'idnhanvien': $arrbanglotrinh[i].idnhanvien, 'kinhdo': $arrbanglotrinh[i].kinhdo, 'vido': $arrbanglotrinh[i].vido, 'thoigianbatdau': $arrbanglotrinh[i].thoigian, 'thoigianketthuc': $arrbanglotrinh[i].thoigianketthuc }, function (dt) {//truy xuat du lay du lieu tu server qua phuong thuc POST

                                            //    document.getElementById('infowin').innerHTML = dt;

                                            //});
                                        } else {
                                            infowindow.setContent(this.content);
                                        }

                                        infowindow.open(BanDo, this);
                                        activeInfoWindow = infowindow;

                                        //set active bang lo trinh khi click vao marker
                                        $('#timelineContent li').removeClass('active');
                                        let ul = document.getElementById("timelineContent");
                                        let liArray = ul.getElementsByTagName("li");

                                        for (let k = 0; k < liArray.length; k++) {
                                            if (liArray[k].attributes["id"].value == "banglotrinh_" + $arrbanglotrinh[i].thoigian) {
                                                $('#timelineContent li').eq(k).addClass('active');
                                                ul.scrollTop = (liArray[k].offsetTop - 20);
                                            }
                                        }

                                        if ($circle != null) {
                                            $circle.setMap(null);
                                        }
                                        $circle = new google.maps.Circle({
                                            center: marker.getPosition(),
                                            radius: marker.lable,
                                            map: BanDo,
                                            fillColor: '#0000FF',
                                            strokeOpacity: 0.8,
                                            strokeWeight: 2,
                                            strokeColor: '#0000FF',
                                            fillOpacity: 0.35
                                        });
                                    };
                                })(Marker_LoTrinh_TrucTuyen[i], i, infowindow));
                            }
                        }
                    }

                    //paint vao ra diem
                    for (i = 0; i < $arrlotrinh_vaodiem.length; i++) {
                        Marker_LoTrinh_VaoDiem[i] = new google.maps.Marker({
                            position: new google.maps.LatLng($arrlotrinh_vaodiem[i].vido, $arrlotrinh_vaodiem[i].kinhdo),
                            icon: "../../../assets/img/vaodiem.png",
                            map: BanDo,
                            lable: $arrlotrinh_vaodiem[i].accuracy,
                            content: $.i18n('header_vaodiem') + " - " + $.i18n('label_tenkhachhang') + ": " + $arrlotrinh_vaodiem[i].tenkhachhang
                                + " - " + $.i18n('label_thoigianvao') + ": " + $arrlotrinh_vaodiem[i].thoigianvaodiem
                                + " / " + $.i18n('label_thoigianra') + ": " + $arrlotrinh_vaodiem[i].thoigianradiem
                        });
                        infowindow = new google.maps.InfoWindow();
                        google.maps.event.addListener(Marker_LoTrinh_VaoDiem[i], 'click', (function (marker, i, infowindow) {
                            return function () {
                                if (activeInfoWindow) {
                                    activeInfoWindow.close();
                                }
                                infowindow.setContent(this.content);
                                infowindow.open(BanDo, this);
                                activeInfoWindow = infowindow;

                                //set active bang lo trinh khi click vao marker
                                $('#timelineContent li').removeClass('active');
                                let ul = document.getElementById("timelineContent");
                                let liArray = ul.getElementsByTagName("li");

                                for (let k = 0; k < liArray.length; k++) {
                                    if (liArray[k].attributes["id"].value == "banglotrinh_" + $arrlotrinh_vaodiem[i].thoigian) {
                                        $('#timelineContent li').eq(k).addClass('active');
                                        ul.scrollTop = (liArray[k].offsetTop - 20);
                                    }
                                }

                                if ($circle != null) {
                                    $circle.setMap(null);
                                }
                                $circle = new google.maps.Circle({
                                    center: marker.getPosition(),
                                    radius: marker.lable,
                                    map: BanDo,
                                    fillColor: '#0000FF',
                                    strokeOpacity: 0.8,
                                    strokeWeight: 2,
                                    strokeColor: '#0000FF',
                                    fillOpacity: 0.35
                                });
                            };
                        })(Marker_LoTrinh_VaoDiem[i], i, infowindow));
                    }

                    for (j = 0; j < $arrlotrinh_radiem.length; j++) {
                        Marker_LoTrinh_RaDiem[j] = new google.maps.Marker({
                            position: new google.maps.LatLng($arrlotrinh_radiem[j].vido, $arrlotrinh_radiem[j].kinhdo),
                            icon: "../../../assets/img/radiem.png",
                            map: BanDo,
                            lable: $arrlotrinh_radiem[j].accuracy,
                            content: $.i18n('label_radiem') + " - " + $.i18n('label_tenkhachhang') + ": " + $arrlotrinh_radiem[j].tenkhachhang
                                + " - " + $.i18n('label_thoigianvao') + ": " + $arrlotrinh_radiem[j].thoigianvaodiem
                                + " / " + $.i18n('label_thoigianra') + ": " + $arrlotrinh_radiem[j].thoigianradiem
                        });
                        infowindow = new google.maps.InfoWindow();
                        google.maps.event.addListener(Marker_LoTrinh_RaDiem[j], 'click', (function (marker, j, infowindow) {
                            return function () {
                                if (activeInfoWindow == infowindow) {
                                    return;
                                }
                                if (activeInfoWindow) {
                                    activeInfoWindow.close();
                                }
                                infowindow.setContent(this.content);
                                infowindow.open(BanDo, this);
                                activeInfoWindow = infowindow;

                                if ($circle != null) {
                                    $circle.setMap(null);
                                }
                                $circle = new google.maps.Circle({
                                    center: marker.getPosition(),
                                    radius: marker.lable,
                                    map: BanDo,
                                    fillColor: '#0000FF',
                                    strokeOpacity: 0.8,
                                    strokeWeight: 2,
                                    strokeColor: '#0000FF',
                                    fillOpacity: 0.35
                                });
                            };
                        })(Marker_LoTrinh_RaDiem[j], j, infowindow));
                    }

                    //lb.close(); LoadingBox
                } else {
                    if (!$.isEmptyObject(Marker_LoTrinh_TrucTuyen)) {
                        $.each(Marker_LoTrinh_TrucTuyen, function (i, v) {
                            Marker_LoTrinh_TrucTuyen[i].setMap(null);
                        });
                        Marker_LoTrinh_TrucTuyen = {};
                    }
                    if (!$.isEmptyObject(Marker_LoTrinh_RaDiem)) {
                        $.each(Marker_LoTrinh_RaDiem, function (i, v) {
                            Marker_LoTrinh_RaDiem[i].setMap(null);
                        });
                        Marker_LoTrinh_RaDiem = {};
                    }
                    if (!$.isEmptyObject(Marker_LoTrinh_VaoDiem)) {
                        $.each(Marker_LoTrinh_VaoDiem, function (i, v) {
                            Marker_LoTrinh_VaoDiem[i].setMap(null);
                        });
                        Marker_LoTrinh_VaoDiem = {};
                    }
                    if (!$.isEmptyObject(MatTinHieu)) {
                        $.each(MatTinHieu, function (i, v) {
                            MatTinHieu[i].setMap(null);
                        });
                        MatTinHieu = {};
                    }
                    if (!$.isEmptyObject(CoTinHieu)) {
                        $.each(CoTinHieu, function (i, v) {
                            CoTinHieu[i].setMap(null);
                        });
                        CoTinHieu = {};
                    }
                    if ($nhanvienlotrinh != null) {
                        $nhanvienlotrinh.setMap(null);
                        $lotrinharr = [];
                        try {
                            $lotrinh.setMap(null);
                        } catch (e) { }
                        if (!$.isEmptyObject(MatTinHieu)) {
                            $.each(MatTinHieu, function (i, v) {
                                MatTinHieu[i].setMap(null);
                            });
                            MatTinHieu = {};
                        }
                    }

                    loadbanglotrinh($arrbanglotrinh);
                    //lb.close();
                    alert($.i18n('label_khongcodulieulotrinh'));
                }
            } else {
                if (!$.isEmptyObject(Marker_LoTrinh_TrucTuyen)) {
                    $.each(Marker_LoTrinh_TrucTuyen, function (i, v) {
                        Marker_LoTrinh_TrucTuyen[i].setMap(null);
                    });
                    Marker_LoTrinh_TrucTuyen = {};
                }
                if (!$.isEmptyObject(Marker_LoTrinh_RaDiem)) {
                    $.each(Marker_LoTrinh_RaDiem, function (i, v) {
                        Marker_LoTrinh_RaDiem[i].setMap(null);
                    });
                    Marker_LoTrinh_RaDiem = {};
                }
                if (!$.isEmptyObject(Marker_LoTrinh_VaoDiem)) {
                    $.each(Marker_LoTrinh_VaoDiem, function (i, v) {
                        Marker_LoTrinh_VaoDiem[i].setMap(null);
                    });
                    Marker_LoTrinh_VaoDiem = {};
                }
                if (!$.isEmptyObject(MatTinHieu)) {
                    $.each(MatTinHieu, function (i, v) {
                        MatTinHieu[i].setMap(null);
                    });
                    MatTinHieu = {};
                }
                if (!$.isEmptyObject(CoTinHieu)) {
                    $.each(CoTinHieu, function (i, v) {
                        CoTinHieu[i].setMap(null);
                    });
                    CoTinHieu = {};
                }
                if ($nhanvienlotrinh != null) {
                    $nhanvienlotrinh.setMap(null);
                    $lotrinharr = [];
                    try {
                        $lotrinh.setMap(null);
                    } catch (e) { }
                    if (!$.isEmptyObject(MatTinHieu)) {
                        $.each(MatTinHieu, function (i, v) {
                            MatTinHieu[i].setMap(null);
                        });
                        MatTinHieu = {};
                    }
                }

                loadbanglotrinh([]);
                //lb.close();
                alert($.i18n(response.message));
            }
            //lb.close();
        })
    }
    function loadmaplotrinhkhongnoidiem(_loainoi) {
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        $loaiLoTrinh = _loainoi;

        let lb = new $.LoadingBox({ loadingImageSrc: "../../../assets/img/default.gif", });
        mapsDataService.getdatalotrinhnhanvien(__idnhanvien, 1, $loaiLoTrinh, fromdate, todate).then(function (response) {
            if (response.flag) {
                let $data = response.data;

                clearInterval($thisInterval);
                $counter = 0;
                var $arrbanglotrinh_suydien = $data.databanglotrinh_suydien;
                var $arrbanglotrinh = $data.databanglotrinh;
                var $arrlotrinh = $data.datalotrinh;
                var $arrmattinhieu = $data.datamattinhieu;

                if ($nhanvienlotrinh != null) {
                    $nhanvienlotrinh.setMap(null);
                    $lotrinharr = [];
                    try {
                        $lotrinh.setMap(null);
                    } catch (e) {

                    }
                    if (!$.isEmptyObject(CoTinHieu)) {
                        $.each(CoTinHieu, function (i, v) {
                            CoTinHieu[i].setMap(null);
                        });
                        CoTinHieu = {};
                    }
                    if (!$.isEmptyObject(MatTinHieu)) {
                        $.each(MatTinHieu, function (i, v) {
                            MatTinHieu[i].setMap(null);
                        });
                        MatTinHieu = {};
                    }
                }
                //xoa các marker đã vẽ của phần bản đồ nối điểm
                if (!$.isEmptyObject(Marker_LoTrinh_TrucTuyen)) {
                    $.each(Marker_LoTrinh_TrucTuyen, function (i, v) {
                        Marker_LoTrinh_TrucTuyen[i].setMap(null);
                    });
                    Marker_LoTrinh_TrucTuyen = {};
                }
                $lotrinh_line = [];

                if ($arrlotrinh.length > 0) {
                    $.each($arrlotrinh, function ($i, $e) {
                        $lotrinh_line.push(new google.maps.LatLng($e.vido, $e.kinhdo));

                        $lotrinharr.push($e);
                    });

                    //set vi tri cho nhan vien trong lo trinh
                    $nhanvienlotrinh = new google.maps.Marker({
                        icon: 'https://raw.githubusercontent.com/Concept211/Google-Maps-Markers/master/images/marker_green.png',
                        position: $lotrinh_line[0],
                        map: BanDo
                    });

                    BanDo.panTo($lotrinh_line[0]);
                    //ket thuc set vitri

                    if ($arrlotrinh.length > 0) {
                        $.each($arrlotrinh, function ($i, $e) {
                            var label = "0";
                            if ($e.ghichu == "label_ngoaituyen") {
                                CoTinHieu[$i] = new google.maps.Marker({
                                    position: new google.maps.LatLng($e.vido, $e.kinhdo),
                                    lable: $e.accuracy,
                                    content: $.i18n($e.ghichu) + " - " + $e.thoigian,
                                    icon: 'https://chart.googleapis.com/chart?chst=d_map_spin&chld=0.70|0|EA4335|13|b|' + ($i + 1),
                                });
                            } else {
                                CoTinHieu[$i] = new google.maps.Marker({
                                    position: new google.maps.LatLng($e.vido, $e.kinhdo),
                                    lable: $e.accuracy,
                                    content: $.i18n($e.ghichu) + " - " + $e.thoigian,
                                    icon: 'https://chart.googleapis.com/chart?chst=d_map_spin&chld=0.70|0|4285F4|13|b|' + ($i + 1),
                                });
                            }
                            infowindow = new google.maps.InfoWindow();
                            google.maps.event.addListener(CoTinHieu[$i], 'click', (function (marker, i, infowindow) {
                                return function () {
                                    if (activeInfoWindow == infowindow) {
                                        return;
                                    }
                                    if (activeInfoWindow) {
                                        activeInfoWindow.close();
                                    }
                                    infowindow.setContent(this.content);
                                    infowindow.open(BanDo, this);
                                    activeInfoWindow = infowindow;

                                    //set active bang lo trinh khi click vao marker
                                    $('#timelineContent li').removeClass('active');
                                    var ul = document.getElementById("timelineContent");
                                    var liArray = ul.getElementsByTagName("li");

                                    for (var k = 0; k < liArray.length; k++) {

                                        if (liArray[k].attributes["id"].value == "banglotrinh_" + $e.thoigian) {
                                            $('#timelineContent li').eq(k).addClass('active');
                                            ul.scrollTop = (liArray[k].offsetTop - 20);
                                        }
                                    }

                                    if ($circle != null) {
                                        $circle.setMap(null);
                                    }

                                    $circle = new google.maps.Circle({
                                        center: marker.getPosition(),
                                        radius: marker.lable,
                                        map: BanDo,
                                        fillColor: '#0000FF',
                                        strokeOpacity: 0.8,
                                        strokeWeight: 2,
                                        strokeColor: '#0000FF',
                                        fillOpacity: 0.35
                                    });

                                };
                            })(CoTinHieu[$i], i, infowindow));

                            CoTinHieu[$i].setMap(BanDo);

                        });
                    }

                    if ($arrmattinhieu.length > 0 && $.isEmptyObject(MatTinHieu)) { //ve lo trinh mat tin hieu
                        $.each($arrmattinhieu, function ($i, $e) {
                            MatTinHieu[$i] = new google.maps.Marker({
                                position: new google.maps.LatLng($e.vido, $e.kinhdo),
                                icon: 'https://raw.githubusercontent.com/Concept211/Google-Maps-Markers/master/images/marker_red' + ($i + 1) + '.png',
                                title: $i
                            });
                            MatTinHieu[$i].setMap(BanDo);
                        });
                    }

                    loadbanglotrinh($arrbanglotrinh);
                    //lb.close();
                } else {
                    //lb.close();
                    alert($.i18n('label_khongcodulieulotrinh'));
                }
            }

            lb.close();
        })
    }

    function loadmaplotrinhphienlamviec(_loainoi) {
        let fromdate = kendo.toString($scope.obj_TuNgay, formatDateTimeFilter);
        let todate = kendo.toString($scope.obj_DenNgay, formatDateTimeFilter);
        $loaiLoTrinh = _loainoi;

        let lb = new $.LoadingBox({ loadingImageSrc: "../../../assets/img/default.gif", });
        mapsDataService.getdatalotrinhnhanvienphienlamviec(__idnhanvien, 0, $loaiLoTrinh, fromdate, todate).then(function (response) {
            if (response.flag) {
                let $data = response.data;

                clearInterval($thisInterval);
                $counter = 0;

                var $arrlotrinh_vaodiem = $data.datavaodiem;
                var $arrlotrinh_radiem = $data.dataradiem;
                var $arrlotrinh_suydien = $data.datalotrinh_suydien;
                var $arrbanglotrinh = $data.databanglotrinh;
                var $arrlotrinh = $data.datalotrinh;
                var $arrmattinhieu = $data.datamattinhieu;

                //set hiển thị điểm đầu và cuối
                var $diemdautien = $data.loTrinhDauTien;
                var $diemcuoi = $data.loTrinhCuoiCung;
                if ($diemdautien != null) {
                    $maker_diemdautien = new google.maps.Marker({
                        position: new google.maps.LatLng($diemdautien.vido, $diemdautien.kinhdo),
                        map: BanDo,
                        lable: $diemdautien.accuracy,
                        content: $.i18n('label_batdau') + " - " + $diemdautien.thoigian
                    });
                    infoDiemBatDau = new google.maps.InfoWindow({
                        content: $maker_diemdautien.content
                    });

                    infoDiemBatDau.open(BanDo, $maker_diemdautien);
                }
                if ($diemcuoi != null) {
                    $maker_diemcuoicung = new google.maps.Marker({
                        position: new google.maps.LatLng($diemcuoi.vido, $diemcuoi.kinhdo),
                        map: BanDo,
                        lable: $diemcuoi.accuracy,
                        content: $.i18n('label_ketthuc') + " - " + $diemcuoi.thoigian
                    });
                    infoDiemKetThuc = new google.maps.InfoWindow({
                        content: $maker_diemcuoicung.content
                    });

                    infoDiemKetThuc.open(BanDo, $maker_diemcuoicung);
                }
                if (!$.isEmptyObject(MatTinHieu)) {
                    $.each(MatTinHieu, function (i, v) {
                        MatTinHieu[i].setMap(null);
                    });
                    MatTinHieu = {};
                }
                if (!$.isEmptyObject(CoTinHieu)) {
                    $.each(CoTinHieu, function (i, v) {
                        CoTinHieu[i].setMap(null);
                    });
                    CoTinHieu = {};
                }

                if ($nhanvienlotrinh != null) {
                    $nhanvienlotrinh.setMap(null);

                    $lotrinharr = [];
                    try {
                        $lotrinh.setMap(null);
                    } catch (e) { }
                }
                try {
                    if (!$.isEmptyObject($arrlotrinh_map)) {
                        $.each($arrlotrinh_map, function (i, v) {
                            $arrlotrinh_map[i].setMap(null);
                        });
                    }
                } catch (e) { }

                if (!$.isEmptyObject(Marker_LoTrinh_TrucTuyen)) {
                    $.each(Marker_LoTrinh_TrucTuyen, function (i, v) {
                        Marker_LoTrinh_TrucTuyen[i].setMap(null);
                    });
                    Marker_LoTrinh_TrucTuyen = {};
                }

                if (!$.isEmptyObject(Marker_LoTrinh_RaDiem)) {
                    $.each(Marker_LoTrinh_RaDiem, function (i, v) {
                        Marker_LoTrinh_RaDiem[i].setMap(null);
                    });
                    Marker_LoTrinh_RaDiem = {};
                }
                if (!$.isEmptyObject(Marker_LoTrinh_VaoDiem)) {

                    $.each(Marker_LoTrinh_VaoDiem, function (i, v) {
                        Marker_LoTrinh_VaoDiem[i].setMap(null);
                    });
                    Marker_LoTrinh_VaoDiem = {};
                }


                $arrlotrinh_line = [];
                if (!$.isEmptyObject($lotrinh)) {

                    $lotrinh.setMap(null);
                }
                if ($loaiLoTrinh == 1 || $loaiLoTrinh == 3) {
                    //lo trinh suy dien
                    $.each($arrlotrinh_suydien, function ($z, $k) {
                        $lotrinh_line = [];
                        $.each($arrlotrinh_suydien[$z], function ($i, $e) {
                            //ve lộ trình
                            $lotrinh_line.push(new google.maps.LatLng($e.vido, $e.kinhdo));
                        });
                        $arrlotrinh_line.push($lotrinh_line);
                    });

                    $.each($arrlotrinh_line, function (h, e) {
                        $lotrinh = new google.maps.Polyline({
                            path: $arrlotrinh_line[h],
                            geodesic: true,
                            strokeColor: '#005db5',
                            strokeOpacity: 1,
                            strokeWeight: 2.5,
                        });
                        $lotrinh.setMap(BanDo);
                        $arrlotrinh_map.push($lotrinh);
                    });
                }

                if ($arrlotrinh.length > 0) {
                    if ($loaiLoTrinh == 1 || $loaiLoTrinh == 3) {
                        $.each($arrlotrinh, function ($i, $e) {
                            $lotrinharr.push($e);
                        });
                    }
                    else {
                        //lo trinh gps
                        if ($arrlotrinh.length > 0) {
                            $.each($arrlotrinh, function ($i, $e) {
                                $lotrinharr.push($e);
                            });
                        }
                    }

                    //set vi tri cho nhan vien trong lo trinh
                    try {
                        $nhanvienlotrinh = new google.maps.Marker({
                            position: $lotrinh_line[0],
                            map: BanDo
                        });
                        BanDo.panTo($lotrinh_line[0]);
                        //ket thuc set vitri
                    } catch (e) { }

                    if ($loaiLoTrinh != 1 && $loaiLoTrinh != 3) {
                        if ($arrmattinhieu.length > 0 && $.isEmptyObject(MatTinHieu)) { //ve lo trinh mat tin hieu
                            $.each($arrmattinhieu, function ($i, $e) {
                                MatTinHieu[$i] = new google.maps.Polyline({
                                    path: [new google.maps.LatLng($e.diemdau.vido, $e.diemdau.kinhdo), new google.maps.LatLng($e.diemcuoi.vido, $e.diemcuoi.kinhdo)],
                                    geodesic: true,
                                    strokeColor: '#E7453D',
                                    strokeOpacity: 1,
                                    strokeWeight: 2.5
                                });
                                MatTinHieu[$i].setMap(BanDo);
                            });
                        }
                    }

                    loadbanglotrinh($arrbanglotrinh);

                    if (!$.isEmptyObject(Marker_LoTrinh_TrucTuyen)) {
                        $.each(Marker_LoTrinh_TrucTuyen, function (i, v) {
                            Marker_LoTrinh_TrucTuyen[i].setMap(null);
                        });
                        Marker_LoTrinh_TrucTuyen = {};
                    }

                    if ($loaiLoTrinh != 3) {
                        for (i = 0; i < $arrbanglotrinh.length; i++) {
                            if ($arrbanglotrinh[i].ghichu == "label_ngoaituyen") {
                                Marker_LoTrinh_TrucTuyen[i] = new google.maps.Marker({
                                    position: new google.maps.LatLng($arrbanglotrinh[i].vido, $arrbanglotrinh[i].kinhdo),
                                    icon: "../../../assets/img/offline_location.png",
                                    map: BanDo,
                                    lable: $arrbanglotrinh[i].accuracy,
                                    content: $.i18n($arrbanglotrinh[i].ghichu) + " - " + $arrbanglotrinh[i].thoigian + "(" + $arrbanglotrinh[i].accuracy + "," + $arrbanglotrinh[i].speed.toFixed(2) + ")"
                                });
                                infowindow = new google.maps.InfoWindow();
                                google.maps.event.addListener(Marker_LoTrinh_TrucTuyen[i], 'click', (function (marker, i, infowindow) {
                                    return function () {
                                        if (activeInfoWindow) {
                                            activeInfoWindow.close();
                                        }
                                        infowindow.setContent(this.content);
                                        infowindow.open(BanDo, this);
                                        activeInfoWindow = infowindow;

                                        //set active bang lo trinh khi click vao marker
                                        $('#timelineContent li').removeClass('active');
                                        var ul = document.getElementById("timelineContent");
                                        var liArray = ul.getElementsByTagName("li");

                                        for (var k = 0; k < liArray.length; k++) {
                                            if (liArray[k].attributes["id"].value == "banglotrinh_" + $arrbanglotrinh[i].thoigian) {
                                                $('#timelineContent li').eq(k).addClass('active');
                                                ul.scrollTop = (liArray[k].offsetTop - 20);
                                            }
                                        }

                                        if ($circle != null) {
                                            $circle.setMap(null);
                                        }
                                        $circle = new google.maps.Circle({
                                            center: marker.getPosition(),
                                            radius: marker.lable,
                                            map: BanDo,
                                            fillColor: '#0000FF',
                                            strokeOpacity: 0.8,
                                            strokeWeight: 2,
                                            strokeColor: '#0000FF',
                                            fillOpacity: 0.35
                                        });
                                    };
                                })(Marker_LoTrinh_TrucTuyen[i], i, infowindow));
                            }
                            else if ($arrbanglotrinh[i].ghichu == "label_tructuyen") {
                                Marker_LoTrinh_TrucTuyen[i] = new google.maps.Marker({
                                    position: new google.maps.LatLng($arrbanglotrinh[i].vido, $arrbanglotrinh[i].kinhdo),
                                    icon: "../../../assets/img/online_location.png",
                                    map: BanDo,
                                    lable: $arrbanglotrinh[i].accuracy,
                                    content: $.i18n($arrbanglotrinh[i].ghichu) + " - " + $arrbanglotrinh[i].thoigian + "(" + $arrbanglotrinh[i].accuracy + "," + $arrbanglotrinh[i].speed.toFixed(2) + ")"

                                });
                                infowindow = new google.maps.InfoWindow();
                                google.maps.event.addListener(Marker_LoTrinh_TrucTuyen[i], 'click', (function (marker, i, infowindow) {
                                    return function () {
                                        if (activeInfoWindow) {
                                            activeInfoWindow.close();
                                        }
                                        infowindow.setContent(this.content);
                                        infowindow.open(BanDo, this);
                                        activeInfoWindow = infowindow;

                                        //set active bang lo trinh khi click vao marker
                                        $('#timelineContent li').removeClass('active');
                                        var ul = document.getElementById("timelineContent");
                                        var liArray = ul.getElementsByTagName("li");

                                        for (var k = 0; k < liArray.length; k++) {
                                            if (liArray[k].attributes["id"].value == "banglotrinh_" + $arrbanglotrinh[i].thoigian) {
                                                $('#timelineContent li').eq(k).addClass('active');
                                                ul.scrollTop = (liArray[k].offsetTop - 20);
                                            }
                                        }

                                        if ($circle != null) {
                                            $circle.setMap(null);
                                        }
                                        $circle = new google.maps.Circle({
                                            center: marker.getPosition(),
                                            radius: marker.lable,
                                            map: BanDo,
                                            fillColor: '#0000FF',
                                            strokeOpacity: 0.8,
                                            strokeWeight: 2,
                                            strokeColor: '#0000FF',
                                            fillOpacity: 0.35
                                        });
                                    };
                                })(Marker_LoTrinh_TrucTuyen[i], i, infowindow));
                            }
                        }
                    }
                    //paint vao ra diem

                    for (i = 0; i < $arrlotrinh_vaodiem.length; i++) {
                        Marker_LoTrinh_VaoDiem[i] = new google.maps.Marker({
                            position: new google.maps.LatLng($arrlotrinh_vaodiem[i].vido, $arrlotrinh_vaodiem[i].kinhdo),
                            icon: "../../../assets/img/vaodiem.png",
                            map: BanDo,
                            lable: $arrlotrinh_vaodiem[i].accuracy,
                            content: $.i18n('header_vaodiem') + " - " + $.i18n('label_tenkhachhang') + ": " + $arrlotrinh_vaodiem[i].tenkhachhang
                                + " - " + $.i18n('label_thoigianvao') + ": " + $arrlotrinh_vaodiem[i].thoigianvaodiem
                                + " / " + $.i18n('label_thoigianra') + ": " + $arrlotrinh_vaodiem[i].thoigianradiem
                        });
                        infowindow = new google.maps.InfoWindow();
                        google.maps.event.addListener(Marker_LoTrinh_VaoDiem[i], 'click', (function (marker, i, infowindow) {
                            return function () {
                                if (activeInfoWindow) {
                                    activeInfoWindow.close();
                                }
                                infowindow.setContent(this.content);
                                infowindow.open(BanDo, this);
                                activeInfoWindow = infowindow;

                                //set active bang lo trinh khi click vao marker
                                $('#timelineContent li').removeClass('active');
                                var ul = document.getElementById("timelineContent");
                                var liArray = ul.getElementsByTagName("li");

                                for (var k = 0; k < liArray.length; k++) {
                                    if (liArray[k].attributes["id"].value == "banglotrinh_" + $arrlotrinh_vaodiem[i].thoigian) {
                                        $('#timelineContent li').eq(k).addClass('active');
                                        ul.scrollTop = (liArray[k].offsetTop - 20);
                                    }
                                }

                                if ($circle != null) {
                                    $circle.setMap(null);
                                }
                                $circle = new google.maps.Circle({
                                    center: marker.getPosition(),
                                    radius: marker.lable,
                                    map: BanDo,
                                    fillColor: '#0000FF',
                                    strokeOpacity: 0.8,
                                    strokeWeight: 2,
                                    strokeColor: '#0000FF',
                                    fillOpacity: 0.35
                                });

                            };
                        })(Marker_LoTrinh_VaoDiem[i], i, infowindow));
                    }

                    for (j = 0; j < $arrlotrinh_radiem.length; j++) {
                        Marker_LoTrinh_RaDiem[j] = new google.maps.Marker({
                            position: new google.maps.LatLng($arrlotrinh_radiem[j].vido, $arrlotrinh_radiem[j].kinhdo),
                            icon: "../../../assets/img/radiem.png",
                            map: BanDo,
                            lable: $arrlotrinh_radiem[j].accuracy,
                            content: $.i18n('label_radiem') + " - " + $.i18n('label_tenkhachhang') + ": " + $arrlotrinh_radiem[j].tenkhachhang
                                + " - " + $.i18n('label_thoigianvao') + ": " + $arrlotrinh_radiem[j].thoigianvaodiem
                                + " / " + $.i18n('label_thoigianra') + ": " + $arrlotrinh_radiem[j].thoigianradiem
                        });
                        infowindow = new google.maps.InfoWindow();
                        google.maps.event.addListener(Marker_LoTrinh_RaDiem[j], 'click', (function (marker, j, infowindow) {
                            return function () {
                                if (activeInfoWindow == infowindow) {
                                    return;
                                }
                                if (activeInfoWindow) {
                                    activeInfoWindow.close();
                                }
                                infowindow.setContent(this.content);
                                infowindow.open(BanDo, this);
                                activeInfoWindow = infowindow;

                                if ($circle != null) {
                                    $circle.setMap(null);
                                }
                                $circle = new google.maps.Circle({
                                    center: marker.getPosition(),
                                    radius: marker.lable,
                                    map: BanDo,
                                    fillColor: '#0000FF',
                                    strokeOpacity: 0.8,
                                    strokeWeight: 2,
                                    strokeColor: '#0000FF',
                                    fillOpacity: 0.35
                                });

                            };
                        })(Marker_LoTrinh_RaDiem[j], j, infowindow));
                    }

                    //lb.close();
                } else {
                    if (!$.isEmptyObject(Marker_LoTrinh_TrucTuyen)) {
                        $.each(Marker_LoTrinh_TrucTuyen, function (i, v) {
                            Marker_LoTrinh_TrucTuyen[i].setMap(null);
                        });
                        Marker_LoTrinh_TrucTuyen = {};
                    }

                    if (!$.isEmptyObject(Marker_LoTrinh_RaDiem)) {
                        $.each(Marker_LoTrinh_RaDiem, function (i, v) {
                            Marker_LoTrinh_RaDiem[i].setMap(null);
                        });
                        Marker_LoTrinh_RaDiem = {};
                    }
                    if (!$.isEmptyObject(Marker_LoTrinh_VaoDiem)) {
                        $.each(Marker_LoTrinh_VaoDiem, function (i, v) {
                            Marker_LoTrinh_VaoDiem[i].setMap(null);
                        });
                        Marker_LoTrinh_VaoDiem = {};
                    }
                    if (!$.isEmptyObject(MatTinHieu)) {
                        $.each(MatTinHieu, function (i, v) {
                            MatTinHieu[i].setMap(null);
                        });
                        MatTinHieu = {};
                    }
                    if (!$.isEmptyObject(CoTinHieu)) {
                        $.each(CoTinHieu, function (i, v) {
                            CoTinHieu[i].setMap(null);
                        });
                        CoTinHieu = {};
                    }
                    if ($nhanvienlotrinh != null) {
                        $nhanvienlotrinh.setMap(null);
                        $lotrinharr = [];
                        try {
                            $lotrinh.setMap(null);
                        } catch (e) { }

                        if (!$.isEmptyObject(MatTinHieu)) {
                            $.each(MatTinHieu, function (i, v) {
                                MatTinHieu[i].setMap(null);
                            });
                            MatTinHieu = {};
                        }
                    }

                    loadbanglotrinh($arrbanglotrinh);
                    //lb.close();
                    alert($.i18n('label_khongcodulieulotrinh'));
                }
            } else {
                if (!$.isEmptyObject(Marker_LoTrinh_TrucTuyen)) {
                    $.each(Marker_LoTrinh_TrucTuyen, function (i, v) {
                        Marker_LoTrinh_TrucTuyen[i].setMap(null);
                    });
                    Marker_LoTrinh_TrucTuyen = {};
                }

                if (!$.isEmptyObject(Marker_LoTrinh_RaDiem)) {
                    $.each(Marker_LoTrinh_RaDiem, function (i, v) {
                        Marker_LoTrinh_RaDiem[i].setMap(null);
                    });
                    Marker_LoTrinh_RaDiem = {};
                }
                if (!$.isEmptyObject(Marker_LoTrinh_VaoDiem)) {
                    $.each(Marker_LoTrinh_VaoDiem, function (i, v) {
                        Marker_LoTrinh_VaoDiem[i].setMap(null);
                    });
                    Marker_LoTrinh_VaoDiem = {};
                }
                if (!$.isEmptyObject(MatTinHieu)) {
                    $.each(MatTinHieu, function (i, v) {
                        MatTinHieu[i].setMap(null);
                    });
                    MatTinHieu = {};
                }
                if (!$.isEmptyObject(CoTinHieu)) {
                    $.each(CoTinHieu, function (i, v) {
                        CoTinHieu[i].setMap(null);
                    });
                    CoTinHieu = {};
                }
                if ($nhanvienlotrinh != null) {
                    $nhanvienlotrinh.setMap(null);
                    $lotrinharr = [];
                    try {
                        $lotrinh.setMap(null);

                    } catch (e) { }
                    if (!$.isEmptyObject(MatTinHieu)) {
                        $.each(MatTinHieu, function (i, v) {
                            MatTinHieu[i].setMap(null);
                        });
                        MatTinHieu = {};
                    }
                }
                try {
                    if (!$.isEmptyObject($arrlotrinh_map)) {
                        $.each($arrlotrinh_map, function (i, v) {
                            $arrlotrinh_map[i].setMap(null);
                        });
                    }
                } catch (e) { }
                loadbanglotrinh($arrbanglotrinh);
                //lb.close();
                alert(response.message);
            }

            lb.close();
        })
    }

    function loadbanglotrinh($e) {
        if ($e.length > 0) {
            let $cbanglotrinh = 0;
            let $activecss = "";
            $('.wrapper .timelineBlock .timelineControl #show-hide').hide();
            $('.wrapper .timelineBlock .timelineControl .loading').show();
            $('#timelineContent').html('');
            let $thisIntervalBangLoTRinh = setInterval(function () {
                if ($cbanglotrinh == 0) {
                    $activecss = 'class="active"';
                } else {
                    $activecss = '';
                }

                if ($cbanglotrinh >= parseInt($e.length)) {
                    clearInterval($thisIntervalBangLoTRinh);
                    $cbanglotrinh = 0;
                    $('.wrapper .timelineBlock .timelineControl #show-hide').show();
                    $('.wrapper .timelineBlock .timelineControl #show-hide').attr('title', 'Thu gọn');
                    $('.wrapper .timelineBlock .timelineControl .loading').hide();
                    $('#timelineContent').slideDown();
                    $('.timelineControl #show-hide').removeClass();
                    $('.timelineControl #show-hide').addClass('hides');
                    //$('.timelineBlock').append("<script> $(document).ready(function(){$('#timelineContent li').click(function () {$index = $(this).index(); LACHONG.denvitrilotrinh($index); });});</script>");
                } else {
                    if ($e[$cbanglotrinh].thoiGianDungDo_Giay > 0) {
                        let html = "<li ng-click='denvitrilotrinh(" + $cbanglotrinh + ")' id='banglotrinh_" + $e[$cbanglotrinh].thoigian + "' " + $activecss + " >" + $e[$cbanglotrinh].thoigian + " - Dừng đỗ (" + $e[$cbanglotrinh].tinhtrangpin + "% )" + $.i18n('label_pin') + " </li>";
                        angular.element($('#timelineContent')).append($compile(html)($scope))
                    }
                    else {
                        if ($e[$cbanglotrinh].ghichu != "label_vaodiem" && $e[$cbanglotrinh].ghichu != "label_radiem") {
                            let html = "<li ng-click='denvitrilotrinh(" + $cbanglotrinh + ")' id='banglotrinh_" + $e[$cbanglotrinh].thoigian + "' " + $activecss + " >" + $e[$cbanglotrinh].thoigian + " - " + $.i18n($e[$cbanglotrinh].ghichu) + " (" + $e[$cbanglotrinh].tinhtrangpin + "% " + $.i18n('label_pin') + ")</li>"
                            angular.element($('#timelineContent')).append($compile(html)($scope))
                        } else {
                            let html = "<li ng-click='denvitrilotrinh(" + $cbanglotrinh + ")' id='banglotrinh_" + $e[$cbanglotrinh].thoigian + "' " + $activecss + " >" + $e[$cbanglotrinh].thoigian + " - " + $.i18n($e[$cbanglotrinh].ghichu) + "</li>"
                            angular.element($('#timelineContent')).append($compile(html)($scope))
                        }
                    }
                }
                $cbanglotrinh++;
            }, 0);
        } else {
            $('#timelineContent').html('');
            $('#timelineContent').hide();
        }
    }

    //event
    $scope.fromDateChanged = function () {
        $scope.minDate = $scope.obj_TuNgay;
    }
    $scope.toDateChanged = function () {
        $scope.maxDate = $scope.obj_DenNgay;
    }
    $scope.nhanvienOnChange = function () {
        $scope.nhanvienselect = this.nhanvienselect;

        if ($scope.nhanvienselect != undefined)
            __idnhanvien = ($scope.nhanvienselect.iD_NhanVien < 0) ? 0 : $scope.nhanvienselect.iD_NhanVien;
        else
            __idnhanvien = 0;
    }
    $scope.nhomnhanvienOnChange = function () {
        $scope.nhomnhanvienselect = this.nhomnhanvienselect;

        if ($scope.nhomnhanvienselect != undefined)
            __idnhom = ($scope.nhomnhanvienselect.iD_Nhom <= 0) ? 0 : $scope.nhomnhanvienselect.iD_Nhom;
        else
            __idnhom = 0;

        ComboboxDataService.getDataNhanVienDangNhapPhanMem(__idnhom).then(function (result) {
            if (result.flag) {
                $scope.nhanvienData = result.data;
            }
            else
                $scope.nhanvienData = [];
            $("#nhanvien").data("kendoComboBox").value("")
        });
    }

    $scope.xemBaoCao = function () {
        if (__idnhanvien <= 0) {
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_canchonthongtinnhanvien') }, 'warning');
        } else {
            loaddatamap();
        }
    }

    $scope.locduongdi = function () {
        if (__idnhanvien > 0) {
            loaddatamap();
        }
    }

    $scope.backC = function () {
        if ($nhanvienlotrinh != null) {
            $counter = (($counter - 1) > 0) ? $counter - 1 : 0;
            $('#timelineContent li').removeClass('active');
            $('#timelineContent li').eq($counter).addClass('active');
            let $vitri = new google.maps.LatLng($lotrinharr[$counter].vido, $lotrinharr[$counter].kinhdo);
            let $accuracy = $lotrinharr[$counter].accuracy;
            if ($accuracy == 0)
                $accuracy = 100;

            $nhanvienlotrinh.setPosition($vitri);
            if ($circle != null) {
                $circle.setMap(null);
            }
            $circle = new google.maps.Circle({
                center: $vitri,
                radius: $accuracy,
                map: BanDo,
                fillColor: '#0000FF',
                strokeOpacity: 0.8,
                strokeWeight: 2,
                strokeColor: '#0000FF',
                fillOpacity: 0.35
            });
            BanDo.panTo($vitri);
        }
    }
    $scope.pauseC = function () {
        if ($nhanvienlotrinh != null) {
            clearInterval($thisInterval);
            $chaylotrinh = false;
        }
    }
    $scope.playC = function () {
        if ($nhanvienlotrinh != null && !$chaylotrinh) {

            let $gioihandong = $('#timelineContent').height();
            $chaylotrinh = true;
            $thisInterval = setInterval(function () {
                if ($counter >= parseInt($lotrinharr.length)) {
                    clearInterval($thisInterval);
                    $counter = 0;
                    $scroll = 0;
                    $chaylotrinh = false;
                } else {
                    $('#timelineContent li').removeClass('active');
                    $('#timelineContent li').eq($counter).addClass('active');
                    let $vitri = new google.maps.LatLng($lotrinharr[$counter].vido, $lotrinharr[$counter].kinhdo);
                    let $accuracy = $lotrinharr[$counter].accuracy;
                    if ($accuracy == 0)
                        $accuracy = 100;

                    $nhanvienlotrinh.setPosition($vitri);
                    $nhanvienlotrinh.setZIndex(1000);
                    BanDo.panTo($vitri);
                    if ($circle != null) {
                        $circle.setMap(null);
                    }
                    $circle = new google.maps.Circle({
                        center: $vitri,
                        radius: $accuracy,
                        map: BanDo,

                        fillColor: '#0000FF',
                        strokeOpacity: 0.8,
                        strokeWeight: 2,
                        strokeColor: '#0000FF',
                        fillOpacity: 0.35

                    });
                    if ($vitrilotrinh >= parseInt($gioihandong / 39)) {
                        $scroll += ($gioihandong);
                        $vitrilotrinh = 0;
                        $('#timelineContent').animate({ scrollTop: $scroll }, 500);
                    }
                }
                $vitrilotrinh++;
                $counter++;
            }, 1000);
        }
    }
    $scope.stopC = function () {
        if ($nhanvienlotrinh != null) {
            clearInterval($thisInterval);
            $counter = 0;
            $scroll = 1;
            $chaylotrinh = false;
            $('#timelineContent li').removeClass('active');
            $('#timelineContent li').eq($counter).addClass('active');
            let $vitri = new google.maps.LatLng($lotrinharr[$counter].vido, $lotrinharr[$counter].kinhdo);
            //let $accuracy = $lotrinharr[$e].accuracy;
            //if ($accuracy == 0)
            let $accuracy = 100;
            $nhanvienlotrinh.setPosition($vitri);
            BanDo.panTo($vitri);
            if ($circle != null) {
                $circle.setMap(null);
            }
            $circle = new google.maps.Circle({
                center: $vitri,
                radius: $accuracy,
                map: BanDo,
                fillColor: '#0000FF',
                strokeOpacity: 0.8,
                strokeWeight: 2,
                strokeColor: '#0000FF',
                fillOpacity: 0.35
            });
        }
    }
    $scope.nextC = function () {
        if ($nhanvienlotrinh != null) {
            $counter = (($counter + 1) > parseInt($lotrinharr.length)) ? parseInt($lotrinharr.length) : $counter + 1;
            $('#timelineContent li').removeClass('active');
            $('#timelineContent li').eq($counter).addClass('active');

            if ($('#timelineContent li').get($counter) != undefined) {
                scrollUL($('#timelineContent li').get($counter));
            }
            if ($lotrinharr[$counter] != undefined) {
                let $vitri = new google.maps.LatLng($lotrinharr[$counter].vido, $lotrinharr[$counter].kinhdo);
                let $accuracy = $lotrinharr[$counter].accuracy;
                if ($accuracy == 0)
                    $accuracy = 100;
                $nhanvienlotrinh.setPosition($vitri);
                BanDo.panTo($vitri);
                if ($circle != null) {
                    $circle.setMap(null);
                }
                $circle = new google.maps.Circle({
                    center: $vitri,
                    radius: $accuracy,
                    map: BanDo,
                    fillColor: '#0000FF',
                    strokeOpacity: 0.8,
                    strokeWeight: 2,
                    strokeColor: '#0000FF',
                    fillOpacity: 0.35
                });
            }
        }
    }
    $scope.morong = function () {

    }

    $scope.denvitrilotrinh = function ($e) {
        //console.log($e);
        if ($nhanvienlotrinh != null) {
            $counter = $e;
            $scroll = $e;
            $vitrilotrinh = $e;

            $('#timelineContent li').removeClass('active');
            $('#timelineContent li').eq($e).addClass('active');
            let $vitri = new google.maps.LatLng($lotrinharr[$e].vido, $lotrinharr[$e].kinhdo);

            infowindow = new google.maps.InfoWindow({ content: "<div id='infowin'>" + $.i18n("label_thongtin") + "</div>" });

            if ($lotrinharr[$e].thoiGianDungDo_Giay > 0) {
                mapsDataService.chitietdungdo($lotrinharr[$e].idnhanvien, $lotrinharr[$e].kinhdo, $lotrinharr[$e].vido, $lotrinharr[$e].thoigian, $lotrinharr[$e].thoigianketthuc).then(function (response) {
                    infowindow.setContent(response.data);
                })
                //$.post('ajaxdata/chitietdungdo.aspx', { 'idnhanvien': $lotrinharr[$e].idnhanvien, 'kinhdo': $lotrinharr[$e].kinhdo, 'vido': $lotrinharr[$e].vido, 'thoigianbatdau': $lotrinharr[$e].thoigian, 'thoigianketthuc': $lotrinharr[$e].thoigianketthuc }, function (dt) {//truy xuat du lay du lieu tu server qua phuong thuc POST
                //    document.getElementById('infowin').innerHTML = dt;
                //});
            } else {
                infowindow.setContent($.i18n($lotrinharr[$e].ghichu) + " - " + $lotrinharr[$e].thoigian + "(" + $.i18n('label_bankinh') + ": " + $lotrinharr[$e].accuracy + "," + $.i18n('label_vantoc') + ": " + $lotrinharr[$e].speed.toFixed(2) + ", " + $.i18n('label_tinhtrangpin') + ":" + $lotrinharr[$e].tinhtrangpin + ")");//,vận tốcvận tốc:" + $arrbanglotrinh[i].speed + ", tình trạng pin:"+$arrbanglotrinh[i].tinhtrangpin +")"
            }

            if (activeInfoWindow) {
                activeInfoWindow.close();
            }

            infowindow.setPosition($vitri);
            infowindow.open(BanDo);

            activeInfoWindow = infowindow;

            let $accuracy = $lotrinharr[$e].accuracy;
            if ($accuracy == 0)
                $accuracy = 100;
            $nhanvienlotrinh.setPosition($vitri);
            $nhanvienlotrinh.setZIndex(1000);
            $nhanvienlotrinh.setIcon('https://chart.googleapis.com/chart?chst=d_map_spin&chld=0.75|0|34A853|13|b|');

            BanDo.panTo($vitri);
            if ($circle != null) {
                $circle.setMap(null);
            }
            $circle = new google.maps.Circle({
                center: $vitri,
                radius: $accuracy,

                map: BanDo,
                fillColor: '#0000FF',
                strokeOpacity: 0.8,
                strokeWeight: 2,
                strokeColor: '#0000FF',
                fillOpacity: 0.35
            });
            //BanDo.fitBounds($circle.getBounds());
        }
    }

    init();

})