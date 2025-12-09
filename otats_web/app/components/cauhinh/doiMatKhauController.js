'use strict';

angular
    .module('app', [])
    .controller('doiMatKhauController', doiMatKhauController);

doiMatKhauController.$inject = ['$scope', '$http', '$location', 'AuthenticationService', 'Notification'];

function doiMatKhauController($scope, $http, $location, AuthenticationService, Notification) {
    CreateSiteMap();
    hideLoadingPage();

    $scope.passold = '';
    $scope.pass = '';
    $scope.passrep = '';

    $scope.changpass = function () {
        if ($scope.validate()) {
            AuthenticationService.ChangePass('',$scope.passold, $scope.pass).then(function (result) {
                if (result !== null && result.flag) {
                    $location.path('/login');
                } else {
                    $('#passold').focus();
                    Notification({ message: result.message }, 'warning');
                }
            });
        }
    }

    $scope.validate = function () {
        var flag = true;

        if (flag && !$scope.passold) {
            flag = false;
            $('#passold').focus();
            Notification({ message: $.i18n("label_nhapmatkhaucu") }, 'warning');
            
        }

        if (flag && !$scope.pass) {
            flag = false;
            $('#pass').focus();
            Notification({ message: $.i18n("label_nhapmatkhaumoi") }, 'warning');
            
        }

        if (flag && !$scope.passrep) {
            flag = false;
            $('#passrep').focus();
            Notification({ message: $.i18n("label_nhaplaimatkhaumoi") }, 'warning');
            
        }
        if (flag && ($scope.pass != $scope.passrep)) {
            flag = false;
            $('#passrep').focus();
            
            Notification({ message: $.i18n("label_xacnhanmatkhaumoikhongdung") }, 'warning');
            
        }

        return flag;
    }
    
}