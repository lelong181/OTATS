angular.module('app', []).controller('albumController', albumController);
function albumController($scope, $state, $stateParams, $timeout, Notification, albumDataService) {
    CreateSiteMap();

    let param_idalbum = 0;
    let param_idcheckin = 0;
    let param_idlichsubaoduong = 0;
    let param_idimage = 0;
    let slideIndex = 1;

    let __arrimg = [];

    function init() {
        initparam();

        loadimages();
    }
    function initparam() {
        param_idalbum = ($stateParams.idalbum == undefined) ? 0 : $stateParams.idalbum;
        param_idcheckin = ($stateParams.idcheckin == undefined) ? 0 : $stateParams.idcheckin;
        param_idlichsubaoduong = ($stateParams.idlichsubaoduong == undefined) ? 0 : $stateParams.idlichsubaoduong;
        param_idimage = ($stateParams.idimage == undefined) ? 0 : $stateParams.idimage;
    }
    function loadimages() {
        albumDataService.getalbum(param_idalbum, param_idimage, param_idcheckin, param_idlichsubaoduong).then(function (result) {
            let data = result.data;
            let arr = [];
            if (param_idalbum > 0) {
                $scope.tenNhanVien = data.tennhanvien;
                $scope.tenKhachHang = (data.tenkhachhang == '') ? $.i18n("label_vaodiemtudo") : data.tenkhachhang;
                $scope.diaChi = data.diachi;
                if (data.danhsachanh != null)
                    arr = data.danhsachanh;
            } else if (param_idimage > 0) {
                $scope.tenNhanVien = data.tennhanvien;
                $scope.tenKhachHang = $.i18n("label_vaodiemtudo");
                $scope.diaChi = data.diachi;

                let list = [];
                list.push(data);
                arr = list;
            } else if (param_idcheckin > 0 || param_idlichsubaoduong > 0) {
                let obj = data[0];
                $scope.tenNhanVien = obj.tennhanvien;
                $scope.tenKhachHang = $.i18n("label_vaodiemtudo");
                $scope.diaChi = obj.diachi;

                arr = data;
            }

            $scope.listimages = arr;

            __arrimg = arr.map(function (item, index, ar) {
                return item.imageid
            })
        });
    }

    function showSlides(n) {
        let i;
        let slides = document.getElementsByClassName("mySlides");

        if (n > slides.length) { slideIndex = 1 }
        if (n < 1) { slideIndex = slides.length }
        for (i = 0; i < slides.length; i++) {
            slides[i].style.display = "none";
        }

        slides[slideIndex - 1].style.display = "block";
    }

    //event
    $scope.gettime = function (thoigian) {
        let d = new Date(thoigian);
        return kendo.toString(d, formatDateTime);
    }
    $scope.getPathImage = function (path) {
        return SERVERIMAGE + path;
    }
    $scope.opendiachi = function (kinhdo, vido) {
        let url = 'https://www.google.com/maps/dir/' + vido.toString() + ',' + kinhdo.toString();
        window.open(url, '_blank');
    }
    $scope.xoayanh = function (idinmage) {
        let url = SERVERIMAGE + '/AppUpload_v2.aspx?rotate=90&imageid=' + idinmage

        $.ajax({
            type: "GET",
            url: url,
            dataType: 'jsonp',
            crossDomain: true
        }).always(function (jqXHR, textStatus, errorThrown) {
            location.reload();
        });
    }

    $scope.openmodal = function (_id) {
        let n = 1
        for (i = 0; i < __arrimg.length; i++) {
            if (_id == __arrimg[i]) {
                n = i + 1;
                break;
            }
        }

        $("#myModal").css("display", "block");
        showSlides(slideIndex = n);
    }
    $scope.closemodal = function () {
        $("#myModal").css("display", "none")
    }

    $scope.plusSlides = function (n) {
        showSlides(slideIndex += n);
    }

    init();
}