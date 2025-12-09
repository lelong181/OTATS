angular.module('app', []).controller('cauHinhController', cauHinhController);
cauHinhController.$inject = ['$scope', '$http', '$location', '$timeout', 'Notification'];

function cauHinhController($scope, $http, $location, $timeout, Notification) {
    CreateSiteMap();

    let image_url = '';

    function init() {
        loadDinhDangSoCBB();
        loadCauHinhChung();
    }

    function loadDinhDangSoCBB() {
        let arr = [
            { value: $.i18n("label_songuyen"), id: 0 },
            { value: $.i18n("label_sothapphan"), id: 1 },
        ]
        $scope.dinhDangDataSource = arr;
    }
    function loadCauHinhChung() {
        $http({
            method: 'GET',
            url: urlApi + '/api/cauhinh/get'
        }).then(function successCallback(response) {
            $scope.dataCauHinh = {
                tenCongTy: response.data.tenCongTy,
                diaChi: response.data.diaChi,
                dienThoai: response.data.dienThoai,
                email: response.data.email,
                soPhutVaoDiemToiThieu: response.data.soPhutVaoDiemToiThieu,
                thoiGianThongBaoGiaMoi: response.data.thoiGianThongBaoGiaMoi,
                dinhDangSo: response.data.dinhDangSo,
                iconPath: response.data.iconPath,
                suDungBangGiaLoaiKhachHang: response.data.suDungBangGiaLoaiKhachHang
            };

            if (response.data.iconPath != '') {
                $("#previewnhanvien").html('<div class="imgprevew"><img src="' + SERVERIMAGE + response.data.iconPath + '" style="width:154px;height:179px;max-height:179px;" /></div>')
            }
            else {
                $("#previewnhanvien").html('')
            }

            $timeout(function () {
                $("#dinhDangSo").data("kendoComboBox").value($scope.dataCauHinh.dinhDangSo);
            }, 10)

        }, function errorCallback(response) {
                Notification({ message: $.i18n("warning_loigiatrivuilongtailaitrang") }, 'warning');
        });
    }
    
    function validate() {
        let flag = true;

        if ($scope.dinhDangselect != undefined)
            $scope.dataCauHinh.dinhDangSo = ($scope.dinhDangselect.id < 0) ? 0 : $scope.dinhDangselect.id;

        //if (flag && !$scope.dataCauHinh.tenCongTy) {
        //    flag = false;
        //    $('#InputName1').focus();
        //    Notification({ message: $.i18n("warning_nhaptencongty") }, 'warning');
        //}

        if (flag && +$scope.dataCauHinh.soPhutVaoDiemToiThieu != +$scope.dataCauHinh.soPhutVaoDiemToiThieu) {
            flag = false;
            $('#minute').focus();
            Notification({ message: $.i18n("warning_nhapsophutvaodiemtoithieu") }, 'warning');
        }

        if (flag && !$scope.dataCauHinh.thoiGianThongBaoGiaMoi) {
            flag = false;
            $('#Day').focus();
            Notification({ message: $.i18n("warning_nhapthoigianthongbaogiamoi") }, 'warning');
        }

        if (flag && $scope.dataCauHinh.dinhDangSo < 0) {
            flag = false;
            Notification({ message: $.i18n("warning_chuachondinhdangso") }, 'warning');
        }

        return flag;
    }
    function openConfirm(message, acceptAction, cancelAction, data) {
        var scope = angular.element("#mainContentId").scope();
        $(" <div id='confirmUpdate'></div>").appendTo("body").kendoDialog({
            width: "450px",
            closable: true,
            modal: true,
            title: $.i18n("label_xacnhan"),
            content: message,
            actions: [
                {
                    text: $.i18n("button_huy"), primary: false, action: function () {
                        if (cancelAction != null) {
                            scope[cancelAction](data);
                        }
                    }
                },
                {
                    text: $.i18n("button_dongy"), primary: true, action: function () {
                        scope[acceptAction](data);
                    }
                }
            ],
        })
    }

    $("#files").kendoUpload({
        multiple: false,
        select: onUploadImageSuccess,
        validation: {
            allowedExtensions: [".jpg", ".jpeg", ".png"]
        },
        showFileList: false
    });
    $("#files").closest(".k-upload").find("span").text($.i18n("button_chonlogo"));
    function onUploadImageSuccess(e) {
        let data = new FormData();
        data.append('file', e.files[0].rawFile);
        let files = e.files[0];
        if (files.extension.toLowerCase() != ".jpg" && files.extension.toLowerCase() != ".png" && files.extension.toLowerCase() != ".jpeg") {
            e.preventDefault();
            Notification({ title: $.i18n('label_thongbao'), message: $.i18n("label_vuilongchonfileanhjpgpngjpeg") }, 'warning');
        } else {
            $http.post(urlApi + '/api/uploadfile/savefile', data,
                {
                    transformRequest: angular.identity,
                    headers: { 'Content-Type': undefined }
                }).then(function (response) {
                    if (response != null) {
                        $("#previewnhanvien").html('<div class="imgprevew"><img src="' + urlApi + response.data + '" style="width:154px;height:179px;max-height:179px;" /></div>')
                        image_url = response.data;
                    } else {
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_taianhthatbaixinvuilongthulai') }, 'warning');
                    }
                }, function (response) {
                        Notification({ title: $.i18n('label_thongbao'), message: $.i18n('label_taianhthatbaixinvuilongthulai') }, 'warning');
                });
        }
    }

    //event
    $scope.dinhDangOnChange = function () {
        $scope.dinhDangselect = this.dinhDangselect;
    }

    $scope.backhome = function () {
        $location.path('/home');
    }
    $scope.savechange = function () {
        if (validate()) {
            openConfirm($.i18n('label_saukhicapnhatcandangxuatvadangnhaplaibancochacchancapnhatkhong'), 'CapNhatCauHinh');
        }
    }
    $scope.CapNhatCauHinh = function () {
        $scope.dataCauHinh.iconPath = image_url;
        $http({
            method: 'POST',
            url: urlApi + '/api/cauhinh/update',
            data: $scope.dataCauHinh
        }).then(function successCallback(response) {
            if (response.status == 200) {
                Notification({ message: $.i18n('label_capnhatcauhinhthanhcong') }, 'success');
                $location.path('/login');
            } else {
                Notification({ message: $.i18n('label_capnhatcauhinhkhongthanhcong') }, 'warning');
            }
        }, function errorCallback(response) {
                Notification({ message: $.i18n('label_capnhatcauhinhkhongthanhcong') }, 'warning');
        });
    }

    init();
}