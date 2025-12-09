/// <reference path="../libs/angularjs/angular.min1.7.2.js" />

'use strict';

angular
    .module('app', [])
    .controller('loginController', loginController);

loginController.$inject = ['$rootScope', '$scope', '$http', '$location', 'AuthenticationService', 'Notification'];
function loginController($rootScope, $scope, $http, $location, AuthenticationService, Notification) {

    $('#login-radiobtn a').on('click', function () {
        var sel = $(this).data('title');
        var tog = $(this).data('toggle');
        $('#' + tog).prop('value', sel);

        $('a[data-toggle="' + tog + '"]').not('[data-title="' + sel + '"]').removeClass('active').addClass('notActive');
        $('a[data-toggle="' + tog + '"][data-title="' + sel + '"]').removeClass('notActive').addClass('active');
    });

    (function initController() {
        hideLoadingPage();
        $scope.flag = false;
        $scope.message = $.i18n('label_taikhoanhoacmatkhaukhongdung');
        $('#macongty').focus();
        $scope.macongty = 'TRANGANGROUP'
        AuthenticationService.ClearCredentials();

    })();

    $scope.login = function () {

        let isnhanvien = $("input[name=loailogin]:checked").val();
        AuthenticationService.Login($scope.username, $scope.password, $scope.macongty, isnhanvien).then(function (result) {
            if (result !== null && result) {
                AuthenticationService.SetCredentials($scope.username, $scope.macongty, isnhanvien).then(function (res) {
                    if (res !== null && res.flag) {
                        $location.path('/home');
                        AuthenticationService.changelang($rootScope.lang.substring(0, 2));
                    } else {
                        $scope.message = $.i18n('label_taikhoanhoacmatkhaukhongdunghoac');
                        $scope.flag = true;
                        //$scope.dataLoading = false;
                        Notification.success('Success notification');
                    }
                });
            } else {
                $scope.message = $.i18n('label_taikhoanhoacmatkhaukhongdunghoac');
                $scope.flag = true;
                //$scope.dataLoading = false;
            }
        });
    }
}