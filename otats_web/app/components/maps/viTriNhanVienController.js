angular.module('app').controller('viTriNhanVienController', function ($rootScope, $compile, $scope, $state, $timeout, $interval, NgMap, ComboboxDataService, mapsDataService) {
    CreateSiteMap();

    let __map = undefined;
    let __arridnhanvien = [];
    let __trangthai = 100;
    let idnhom = 0;
    let dynMarkers = [];
    let options = {
        gridSize: 50,
        maxZoom: 20,
        imagePath: 'assets/img/m'
    };
    let imagekh = {
        url: 'assets/img/cuahang.png',
        // This marker is 20 pixels wide by 32 pixels high.
        size: new google.maps.Size(20, 32),
        // The origin for this image is (0, 0).
        origin: new google.maps.Point(0, 0),
        // The anchor for this image is the base of the flagpole at (0, 32).
        anchor: new google.maps.Point(0, 32),
        labelOrigin: new google.maps.Point(8, -8)
    };

    $scope.thongke = {
        tructuyen: 0,
        chuadangnhapduoi60: 0,
        chuadangnhaptren60: 0,
        mattinhieuduoi60: 0,
        mattinhieutren60: 0,
        tong: 0,
    }

    function init() {
        initcombo();
        loadthongketructuyen();
        loadmap();
    }

    stopTime = $interval(loadthongketructuyen, 60000);

    function getimage(trangthai) {
        return image = {
            url: 'assets/img/lo' + trangthai + '.png',
            // This marker is 20 pixels wide by 32 pixels high.
            size: new google.maps.Size(20, 32),
            // The origin for this image is (0, 0).
            origin: new google.maps.Point(0, 0),
            // The anchor for this image is the base of the flagpole at (0, 32).
            anchor: new google.maps.Point(0, 32),
            labelOrigin: new google.maps.Point(8, -8)
        }
    }
    function getmarkerlabel(txt, trangthai) {
        let color = '#6c757d';
        if (trangthai == 1)
            color = '#007bff';
        if (trangthai == 2)
            color = '#dc3545';

        return {
            color: color,
            fontWeight: '500',
            text: txt
        }
    }

    function openinfowindow(map, marker, idnhanvien) {
        mapsDataService.getchitietnhanvien(idnhanvien).then(function (response) {
            var printScope = $rootScope.$new();
            var tag = $compile(response.data)(printScope);
            setTimeout(function () {
                if (response.flag) {
                    //console.log(response.data);
                    let infowindow = new google.maps.InfoWindow({
                        content: tag.html()
                    });

                    infowindow.open(map, marker);
                }
            })
        });
    }

    function openinfowindowkhachhang(map, marker, idkhachhang) {
        mapsDataService.getchitietdaily(idkhachhang).then(function (response) {
            if (response.flag) {
                let infowindow = new google.maps.InfoWindow({
                    content: response.data
                });

                infowindow.open(map, marker);
            }
        });
    }

    function loadmap() {
        dynMarkers = [];
        __arridnhanvien = [];

        NgMap.getMap().then(function (map) {
            __map = map;
            mapsDataService.getlistnhanvien(idnhom, __trangthai).then(function (response) {
                let bounds = new google.maps.LatLngBounds();

                let locations = response.data;
                for (let i = 0; i < locations.length; i++) {
                    let latLng = new google.maps.LatLng(locations[i].viDo, locations[i].kinhDo);
                    let marker = new google.maps.Marker({
                        position: latLng,
                        label: getmarkerlabel(locations[i].tennhanvien, locations[i].dangtructuyen),
                        title: locations[i].tennhanvien,
                        icon: getimage(locations[i].dangtructuyen),
                        map: map,
                    });
                    marker.addListener('click', function (e) {
                        openinfowindow(map, marker, locations[i].idnhanvien);
                    });

                    dynMarkers.push(marker);

                    bounds.extend(marker.getPosition());

                    __arridnhanvien.push(locations[i].idnhanvien);
                }

                $scope.markerClusterer = new MarkerClusterer(map, dynMarkers, options);

                map.setCenter(bounds.getCenter());
                map.fitBounds(bounds);
            })

            //khách hàng
            //mapsDataService.getlistdaily(0, 0, 0, 0, 0).then(function (response) {
            //    let locations = response.data;
            //    for (let i = 0; i < locations.length; i++) {
            //        let latLng = new google.maps.LatLng(locations[i].vido, locations[i].kinhdo);
            //        let marker = new google.maps.Marker({
            //            position: latLng,
            //            label: getmarkerlabel(locations[i].ten),
            //            title: locations[i].ten,
            //            icon: imagekh,
            //            map: map,
            //        });
            //        marker.addListener('click', function (e) {
            //            openinfowindowkhachhang(map, marker, locations[i].id);
            //        });

            //        dynMarkers.push(marker);
            //    }

            //    $scope.markerClusterer = new MarkerClusterer(map, dynMarkers, options);
            //})

        });
    }

    function loadthongketructuyen() {
        mapsDataService.getthongketructuyen(idnhom).then(function (result) {
            $scope.thongke = result.data;
        });
    }

    function initcombo() {
        ComboboxDataService.getDataNhomNhanVien().then(function (result) {
            $scope.nhomnhanvienData = result.data;
        });
        ComboboxDataService.getDataNhanVienDangNhapPhanMem(-2).then(function (result) {
            if (result.flag) {
                $scope.nhanvienData = result.data;
            }
            else
                $scope.nhanvienData = [];
        });

        let data = [
            { id: 1, name: $.i18n('label_nhanvien') },
            { id: 2, name: $.i18n('label_nhanvienvadaily') }
        ]

        $scope.kieuhienthiDataSource = data;

        $timeout(function () {
            $("#kieuhienthi").data("kendoDropDownList").value(1);
        }, 500);

    }

    //event
    $scope.nhanvienOnChange = function () {
        $scope.nhanvienselect = this.nhanvienselect;

        let iD_NhanVien = 0;
        if ($scope.nhanvienselect != undefined)
            iD_NhanVien = ($scope.nhanvienselect.iD_NhanVien < 0) ? 0 : $scope.nhanvienselect.iD_NhanVien;

        let index = __arridnhanvien.indexOf(iD_NhanVien);

        mapsDataService.getvitrihientainhanvien(iD_NhanVien).then(function (response) {
            if (response.flag) {
                let data = response.data;
                var vt = new google.maps.LatLng(data.toado.viDo, data.toado.kinhDo);
                __map.panTo(vt);
                __map.setZoom(25);

                var printScope = $rootScope.$new();
                var tag = $compile(data.info)(printScope);
                setTimeout(function () {
                    //console.log(tag.html());
                    let infowindow = new google.maps.InfoWindow({
                        content: tag.html()
                    });

                    //infowindow.open(map, marker);
                    infowindow.open(__map, dynMarkers[index]);

                })
                //let infowindow = new google.maps.InfoWindow({
                //    content: data.info
                //});

                //infowindow.open(__map, dynMarkers[index]);
            }
        });
    }
    $scope.nhomnhanvienOnChange = function () {
        $scope.nhomnhanvienselect = this.nhomnhanvienselect;

        if ($scope.nhomnhanvienselect != undefined)
            idnhom = ($scope.nhomnhanvienselect.iD_Nhom <= 0) ? 0 : $scope.nhomnhanvienselect.iD_Nhom;
        else
            idnhom = 0;

        ComboboxDataService.getDataNhanVienDangNhapPhanMem(idnhom).then(function (result) {
            if (result.flag) {
                $scope.nhanvienData = result.data;
            }
            else
                $scope.nhanvienData = [];
            $("#nhanvien").data("kendoComboBox").value("")
        });


        $scope.markerClusterer.clearMarkers();

        loadthongketructuyen();
        loadmap();
    }

    $scope.timkiemnhanvien = function (_trangthai) {
        __trangthai = _trangthai;
        $scope.markerClusterer.clearMarkers();

        loadmap();
    }

    init();

})