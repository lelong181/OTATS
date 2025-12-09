angular.module('app').controller('viTriKhachHangController', function ($rootScope, $compile, $scope, $state, $stateParams, NgMap, ComboboxDataService, mapsDataService) {
    CreateSiteMap();

    let kinhdo = 0;
    let vido = 0;

    let __map = null;
    let __arrid = [];

    let dynMarkers = [];
    let image = {
        url: 'assets/img/cuahang.png',
        // This marker is 20 pixels wide by 32 pixels high.
        size: new google.maps.Size(20, 32),
        // The origin for this image is (0, 0).
        origin: new google.maps.Point(0, 0),
        // The anchor for this image is the base of the flagpole at (0, 32).
        anchor: new google.maps.Point(0, 32),
        labelOrigin: new google.maps.Point(8, -8)
    };
    let options = {
        gridSize: 50,
        maxZoom: 20,
        imagePath: 'assets/img/m'
    };

    function init() {
        initparam();

        initcombo();
        loadmap(0, 0, 0, kinhdo, vido);
    }

    function initparam() {
        kinhdo = ($stateParams.kinhdo == undefined) ? 0 : $stateParams.kinhdo;
        vido = ($stateParams.vido == undefined) ? '' : $stateParams.vido;
    }

    function getmarkerlabel(txt) {
        return {
            color: '#343a40',
            fontWeight: '500',
            text: txt
        }
    }

    function openinfowindow(map, marker, idkhachhang) {
        mapsDataService.getchitietdaily(idkhachhang).then(function (response) {
            //console.log(response.data);
            var printScope = $rootScope.$new();
            var tag = $compile(response.data)(printScope);
            setTimeout(function () {
                //console.log(tag.html());
                if (response.flag) {
                    let infowindow = new google.maps.InfoWindow({
                        content: tag.html()
                    });

                    infowindow.open(map, marker);
                }
            })
        });
    }

    function loadmap(tinh, quan, loaikhachhang, kinhdo, vido) {
        NgMap.getMap().then(function (map) {
            __map = map;
            mapsDataService.getlistdaily(kinhdo, vido, tinh, quan, loaikhachhang).then(function (response) {
                let bounds = new google.maps.LatLngBounds();
                let locations = response.data;
                for (let i = 0; i < locations.length; i++) {
                    let latLng = new google.maps.LatLng(locations[i].vido, locations[i].kinhdo);
                    let marker = new google.maps.Marker({
                        position: latLng,
                        label: getmarkerlabel(locations[i].ten),
                        title: locations[i].ten,
                        icon: image,
                        map: map,
                    });
                    marker.addListener('click', function (e) {
                        openinfowindow(map, marker, locations[i].id);
                    });

                    dynMarkers.push(marker);

                    bounds.extend(marker.getPosition());

                    __arrid.push(locations[i].id);
                }

                $scope.markerClusterer = new MarkerClusterer(map, dynMarkers, options);

                map.setCenter(bounds.getCenter());
                map.fitBounds(bounds);
            })
        });
    }

    function initcombo() {
        ComboboxDataService.getDataKhachHang().then(function (result) {
            $scope.khachhangData = result.data;
        });
        ComboboxDataService.getTinhThanh().then(function (result) {
            $scope.tinhthanhData = result.data;

        });
        ComboboxDataService.getQuanHuyen(0).then(function (result) {
            $scope.quanhuyenData = result.data;

        });
        ComboboxDataService.getLoaiKhachHang().then(function (result) {
            $scope.loaikhachhangData = result.data;
        });
    }

    //event
    $scope.khachhangOnChange = function () {
        $scope.khachhangselect = this.khachhangselect;

        let idkhachhang = 0;
        if ($scope.khachhangselect != undefined)
            idkhachhang = ($scope.khachhangselect.iD_KhachHang < 0) ? 0 : $scope.khachhangselect.iD_KhachHang;

        let index = __arrid.indexOf(idkhachhang);

        mapsDataService.getvitridaily(idkhachhang).then(function (response) {
            if (response.flag) {
                let data = response.data;
                var vt = new google.maps.LatLng(data.toado.vido, data.toado.kinhdo);
                __map.panTo(vt);
                __map.setZoom(20);

                var printScope = $rootScope.$new();
                var tag = $compile(data.info)(printScope);
                setTimeout(function () {
                    //console.log(tag.html());
                    let infowindow = new google.maps.InfoWindow({
                        content: tag.html()
                    });

                    //infowindow.open(map, marker);
                    //let infowindow = new google.maps.InfoWindow({
                    //    content: data.info
                    infowindow.open(__map, dynMarkers[index]);

                });

            }
        });
    }
    $scope.tinhthanhOnChange = function () {
        $scope.tinhthanhselect = this.tinhthanhselect;

        $("#quanhuyen").data("kendoComboBox").value("");
        var idtinhthanh = 0;
        if ($scope.tinhthanhselect != undefined)
            idtinhthanh = ($scope.tinhthanhselect.iD_Tinh < 0) ? 0 : $scope.tinhthanhselect.iD_Tinh;

        ComboboxDataService.getQuanHuyen(idtinhthanh).then(function (result) {
            $scope.quanhuyenData = result.data;
        });

    }
    $scope.quanhuyenOnChange = function () {
        $scope.quanhuyenselect = this.quanhuyenselect;
    }
    $scope.loaikhachhangOnChange = function () {
        $scope.loaikhachhangselect = this.loaikhachhangselect;
    }

    $scope.timkiem = function () {
        let geocoder = new google.maps.Geocoder();

        geocoder.geocode({ 'address': $scope.diachi }, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
                __map.setCenter(results[0].geometry.location);
                __map.setZoom(15);

                marker = new google.maps.Marker({
                    map: __map,
                    draggable: true,
                    position: results[0].geometry.location
                });
            }
        });
    }
    $scope.lockhachhang = function () {
        let idtinh = 0;
        let idquan = 0;
        let idloaikhachhang = 0;

        if ($scope.tinhthanhselect != undefined)
            idtinh = ($scope.tinhthanhselect.iD_Tinh < 0) ? 0 : $scope.tinhthanhselect.iD_Tinh;
        if ($scope.quanhuyenselect != undefined)
            idquan = ($scope.quanhuyenselect.iD_Quan < 0) ? 0 : $scope.quanhuyenselect.iD_Quan;
        if ($scope.loaikhachhangselect != undefined)
            idloaikhachhang = ($scope.loaikhachhangselect.iD_LoaiKhachHang < 0) ? 0 : $scope.loaikhachhangselect.iD_LoaiKhachHang;

        $scope.markerClusterer.clearMarkers();
        dynMarkers = [];
        __arrid = [];

        loadmap(idtinh, idquan, idloaikhachhang);
    }

    init();

})