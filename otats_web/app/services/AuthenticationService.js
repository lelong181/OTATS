(function () {
    'use strict';

    angular
        .module('app')
        .factory('AuthenticationService', AuthenticationService);

    AuthenticationService.$inject = ['$http', '$cookies', '$rootScope', '$timeout'];
    function AuthenticationService($http, $cookies, $rootScope, $timeout) {
        var service = {};

        service.Login = Login;
        service.SetCredentials = SetCredentials;
        service.ClearCredentials = ClearCredentials;
        service.ChangePass = ChangePass;
        service.changelang = changelang;

        return service;

        function changelang(lang) {
            $http.get(urlApi + '/api/userinfo/changelang?lang=' + lang).then(function (response) {
                return { flag: true, data: response }
            });
        }

        function Login(user, password, macongty, isnhanvien) {
            var data = {
                Username: user,
                MaCongty: macongty,
                PassWord: password,
                IsNhanVien: isnhanvien
            };


            return $http.post(URL_AUTHENTICATION_CHECKLOGIN, data).then(function (response) {
                return response.data;
            }, function (response) {
                return response.data;
            });
        }

        function SetCredentials(username, macongty, isnhanvien) {
            var token = window.localStorage.getItem('OTA_FCM_TOKEN');
            let data = {
                Username: username,
                MaCongTy: macongty,
                IsNhanVien: isnhanvien,
                IDPUSH: token
            };
            
            let isadmin = (isnhanvien == 1) ? 0 : 1;
            console.log(data);
            return $http.post(URL_AUTHENTICATION_LOGIN, data).then(function (response) {
                if (response.data !== null && response.data) {
                    SetCredentialsDetail(macongty, isadmin, response.data);
                    return { flag: true }
                }
            }, function (response) {
                ClearCredentials();
                    return { flag: false, message: $.i18n('label_taikhoanhoacmatkhaukhongdung') }
            });
        }

        function ClearCredentials() {
            $rootScope.UserInfo = {};
            $rootScope.Authorization = '';
            $cookies.remove('userinfo');
            $cookies.remove('authorization');
            delete $http.defaults.headers.common.Authorization;
        }

        function SetCredentialsDetail(_macongty, _isadmin, _result) {

            $rootScope.isAdmin = _isadmin;
            $rootScope.Authorization = _result.token;
            $rootScope.UserInfo = _result.userinfo;
            $rootScope.UserInfo.macongty = _macongty;
            if ($rootScope.UserInfo.dinhDangTienSoThapPhan == 1)
                $rootScope.UserInfo.dinhDangSo = "n" + _result.cauhinh.soChuSoThapPhan;
            else
                $rootScope.UserInfo.dinhDangSo = "n0";

            $rootScope.UserInfo.tencongty = _result.congty.tencongty;
            $rootScope.CongTy = _result.congty;
            $rootScope.PhanMem = _result.phanmem;

            // set default auth header for http requests
            $http.defaults.headers.common['Authorization'] = $rootScope.Authorization;

            // store user details in globals cookie that keeps user logged in for 1 week (or until they logout)
            let cookieExp = new Date();
            cookieExp.setDate(cookieExp.getDate() + 7);
            $cookies.putObject('userinfo', $rootScope.UserInfo, { expires: cookieExp });
            $cookies.putObject('authorization', $rootScope.Authorization, { expires: cookieExp });
            $cookies.putObject('isadmin', $rootScope.isAdmin, { expires: cookieExp });

        }

        function ChangePass(username, password, newpass) {
            var data = {
                Username: $rootScope.UserInfo.username,
                Password: password,
                NewPass: newpass
            };

            return $http.post(URL_AUTHENTICATION_CHANGEPASS, data).then(function (response) {
                if (response.data !== null && response.data) {
                    if (response.data.success)
                        return { flag: true };
                    else
                        return { flag: false, message: response.data.msg };
                } else {
                    return { flag: false, message: $.i18n('label_doimatkhaukhongthanhcongvuilongthuchienlai') };
                }
            }, function (response) {
                ClearCredentials();
                    return { flag: false, message: $.i18n('label_doimatkhaukhongthanhcongvuilongthuchienlai') };
            });
        }
    }

})();