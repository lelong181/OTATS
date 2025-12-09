'use strict';
var app = angular.module('app', ['ngRoute', 'ngCookies', 'ui.router', 'oc.lazyLoad', 'kendo.directives', 'ngAnimate', 'chart.js', 'ngMap', 'ui-notification']);

app.config(['$stateProvider', '$urlRouterProvider', '$locationProvider', '$ocLazyLoadProvider', 'NotificationProvider',
    function ($stateProvider, $urlRouterProvider, $locationProvider, $ocLazyLoadProvider, NotificationProvider) {

        NotificationProvider.setOptions({
            delay: 10000,
            startTop: 20,
            startRight: 10,
            verticalSpacing: 20,
            horizontalSpacing: 20,
            positionX: 'right',
            positionY: 'bottom'
        });

        //Mọi đường dẫn không hợp lệ đều được chuyển đến state login  
        $urlRouterProvider.otherwise('/login');

        $stateProvider
            .state('login', {
                cache: false,
                url: '/login',
                views: {
                    'viewMain': {
                        templateUrl: '/app/components/login/loginView.html',
                        controller: 'loginController',
                    }
                },
                resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/login/loginController.js',
                                '/app/services/AuthenticationService.js'
                            ]
                        });
                    }]
                }
            })

            .state('layoutAdmin', {
                abstract: true,
                views: {
                    'viewMain': {
                        templateUrl: "/app/components/admin/navbar/layoutView.html",
                        controller: 'layoutController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/admin/navbar/layoutController.js',
                                '/app/components/admin/navbar/layoutService.js'
                            ]
                        });
                    }]
                }
            })

            .state('layout', {
                abstract: true,
                views: {
                    'viewMain': {
                        templateUrl: "/app/components/navbar/layoutView.html",
                        controller: 'layoutController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/navbar/layoutController.js',
                                '/app/components/navbar/layoutService.js'
                            ]
                        });
                    }]
                }
            });


        /*Admin state*/
        $stateProvider
            .state('homeAdmin', {
                parent: 'layoutAdmin',
                abstract: false,
                url: '/admin/home',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/admin/home/homeView.html',
                        controller: 'homeController'
                    }
                },
                resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/admin/home/homeController.js?v1=' + (new Date().getTime()),
                                '/app/components/admin/home/homeDataService.js'
                            ]
                        });
                    }]
                }
            })
        /*Admin state*/

        $stateProvider
            .state('home', {
                parent: 'layout',
                abstract: false,
                url: '/home',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/home/homeView.html',
                        controller: 'homeController'
                    }
                },
                resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/home/homeController.js?v1=' + (new Date().getTime()),
                                '/app/components/home/homeDataService.js'
                            ]
                        });
                    }]
                }
            });
        $stateProvider
            .state('newscreen', {
                parent: 'layout',
                abstract: false,
                url: '/abcde',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/newscreen/newscreen.html',
                        controller: 'newscreenController'
                    }
                },
                resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/newscreen/newscreenController.js'
                            ]
                        });
                    }]
                }
            });

        $stateProvider
            .state('khachhang', {
                parent: 'layout',
                abstract: false,
                url: '/khachhang',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/khachhang/khachHangView.html',
                        controller: 'khachHangController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/khachhang/khachHangController.js',
                                '/app/components/khachhang/khachHangDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('checkinmaqr', {
                parent: 'layout',
                abstract: false,
                url: '/checkinmaqr',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/checkinmaqr/checkInMaQrView.html',
                        controller: 'checkInMaQrController'
                    }
                },
                resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/checkinmaqr/checkInMaQrController.js',
                                '/app/components/checkinmaqr/checkInMaQrDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('editkhachhang', {
                parent: 'layout',
                abstract: false,
                url: '/editkhachhang/:idkhachhang',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/khachhang/editKhachHangView.html',
                        controller: 'editkhachHangController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/khachhang/editKhachHangController.js',
                                '/app/components/khachhang/khachHangDataService.js',
                                '/app/components/danhmuc/danhmucDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('duyetxoakhachhang', {
                parent: 'layout',
                abstract: false,
                url: '/duyetxoakhachhang',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/khachhang/duyetXoaKhachHangView.html',
                        controller: 'duyetXoaKhachHangController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/khachhang/duyetXoaKhachHangController.js',
                                '/app/components/khachhang/khachHangDataService.js'
                            ]
                        });
                    }]
                }
            })

        $stateProvider
            .state('nhanvien', {
                parent: 'layout',
                abstract: false,
                url: '/nhanvien?tructuyen',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/nhanvien/nhanVienView.html',
                        controller: 'nhanVienController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/nhanvien/nhanVienController.js',
                                '/app/components/nhanvien/nhanVienDataService.js'
                            ]
                        });
                    }]
                }
            })

        $stateProvider
            .state('huongdanvien', {
                parent: 'layout',
                abstract: false,
                url: '/huongdanvien',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/hdv/hdvView.html',
                        controller: 'hdvController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/hdv/hdvController.js',
                                '/app/components/hdv/hdvDataService.js',
                                '/app/components/nhanvien/nhanVienDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('dangkyfoc', {
                parent: 'layout',
                abstract: false,
                cache: false,
                url: '/dangkyfoc',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/hdv/dangkyfocView.html',
                        controller: 'dangkyfocController',
                    }
                },
                resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/hdv/dangkyfocController.js',
                                '/app/components/hdv/hdvDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('lichsuthaotac', {
                parent: 'layout',
                abstract: false,
                url: '/lichsuthaotac',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/nhanvien/baocaonhanvien/lichSuThaoTacView.html',
                        controller: 'lichSuThaoTacController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/nhanvien/baocaonhanvien/lichSuThaoTacController.js',
                                '/app/components/nhanvien/baocaonhanvien/baoCaoNhanVienDataServiceDetail.js'
                            ]
                        });
                    }]
                }
            })
            .state('vitrihientai', {
                parent: 'layout',
                abstract: false,
                url: '/vitrihientai',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/nhanvien/baocaonhanvien/viTriHienTaiView.html',
                        controller: 'viTriHienTaiController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load('/app/components/nhanvien/baocaonhanvien/viTriHienTaiController.js');
                    }]
                }
            })
            .state('lotrinhnhanvien', {
                parent: 'layout',
                abstract: false,
                url: '/lotrinhnhanvien?idnhanvien&tungay&denngay',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/nhanvien/baocaonhanvien/loTrinhNhanVienView.html',
                        controller: 'loTrinhNhanVienController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load('/app/components/nhanvien/baocaonhanvien/loTrinhNhanVienController.js');
                    }]
                }
            })
            .state('lichsuvaoradiem', {
                parent: 'layout',
                abstract: false,
                url: '/lichsuvaoradiem?idcheckin&idnhanvien&idkhachhang&from&to',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/nhanvien/baocaonhanvien/lichSuVaoRaDiemView.html',
                        controller: 'lichSuVaoRaDiemController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/nhanvien/baocaonhanvien/lichSuVaoRaDiemController.js',
                                '/app/components/nhanvien/baocaonhanvien/baoCaoNhanVienDataServiceDetail.js'
                            ]
                        });
                    }]
                }
            })
            .state('baocaophienlamviec', {
                parent: 'layout',
                abstract: false,
                url: '/baocaophienlamviec?idnhanvien&from&to',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/nhanvien/baocaonhanvien/phienLamViecView.html',
                        controller: 'phienLamViecController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/nhanvien/baocaonhanvien/phienLamViecController.js',
                                '/app/components/nhanvien/baocaonhanvien/baoCaoNhanVienDataServiceDetail.js'
                            ]
                        });
                    }]
                }
            })
            .state('baocaomatketnoi', {
                parent: 'layout',
                abstract: false,
                url: '/baocaomatketnoi',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/nhanvien/baocaonhanvien/baoCaoKhongKetNoiView.html',
                        controller: 'baoCaoKhongKetNoiController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/nhanvien/baocaonhanvien/baoCaoKhongKetNoiController.js',
                                '/app/components/nhanvien/baocaonhanvien/baoCaoNhanVienDataServiceDetail.js'
                            ]
                        });
                    }]
                }
            })
            .state('chuyenquyen', {
                parent: 'layout',
                abstract: false,
                url: '/chuyenquyen/:idnhanvien',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/nhanvien/chuyenKhachHangView.html',
                        controller: 'chuyenKhachHangController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/nhanvien/chuyenKhachHangController.js',
                                '/app/components/nhanvien/nhanVienDataService.js'
                            ]
                        });
                    }]
                }
            })

        $stateProvider
            .state('danhsachmathang', {
                parent: 'layout',
                abstract: false,
                url: '/danhsachmathang',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/hanghoa/hangHoaView.html',
                        controller: 'hangHoaController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/hanghoa/hangHoaController.js',
                                '/app/services/printService.js',
                                '/app/components/hanghoa/hangHoaDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('guibanmathang', {
                parent: 'layout',
                abstract: false,
                url: '/guibanmathang',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/hanghoa/guiBanMatHangView.html',
                        controller: 'guiBanMatHangController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/hanghoa/guiBanMatHangController.js',
                                '/app/components/hanghoa/hangHoaDataService.js'
                            ]
                        });
                    }]
                }
            })

        $stateProvider
            .state('danhsachdonhang', {
                parent: 'layout',
                abstract: false,
                url: '/danhsachdonhang?idnv&idkh&idmathang&idtrangthaixem&ttht&ttgh&tttt&from&to&donhangtaidiem',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/donhang/donHangView.html',
                        controller: 'donHangController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/donhang/donHangController.js?v1=' + (new Date().getTime()),
                            '/app/components/donhang/donHangDataService.js?v1=' + (new Date().getTime())
                            ]
                        });
                    }]
                }
            })
            .state('danhsachdonhangpchl', {
                parent: 'layout',
                abstract: false,
                url: '/danhsachdonhangpchl?idnv&idkh&idmathang&idtrangthaixem&ttht&ttgh&tttt&from&to&donhangtaidiem',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/donhang/donhangpchl/donHangPCHLView.html',
                        controller: 'donHangPCHLController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/donhang/donhangpchl/donHangPCHLController.js?v1=' + (new Date().getTime()),
                            '/app/components/donhang/donHangDataService.js?v1=' + (new Date().getTime())
                            ]
                        });
                    }]
                }
            })
            .state('danhsachdonhangota', {
                parent: 'layout',
                abstract: false,
                url: '/danhsachdonhangota?idnv&idkh&idmathang&idtrangthaixem&ttht&ttgh&tttt&from&to&donhangtaidiem',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/donhang/donhangota/donHangOTAView.html',
                        controller: 'donHangOTAController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/donhang/donhangota/donHangOTAController.js?v1=' + (new Date().getTime()),
                            '/app/components/donhang/donHangDataService.js?v1=' + (new Date().getTime())
                            ]
                        });
                    }]
                }
            })
            .state('danhsachdonhangpkd', {
                parent: 'layout',
                abstract: false,
                url: '/danhsachdonhangpkd?idnv&idkh&idmathang&idtrangthaixem&ttht&ttgh&tttt&from&to&donhangtaidiem',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/donhang/donhangpkd/donHangPKDView.html',
                        controller: 'donHangPKDController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/donhang/donhangpkd/donHangPKDController.js?v1=' + (new Date().getTime()),
                            '/app/components/donhang/donHangDataService.js?v1=' + (new Date().getTime())
                            ]
                        });
                    }]
                }
            })
            .state('danhsachdonhanghdv', {
                parent: 'layout',
                abstract: false,
                url: '/danhsachdonhanghdv',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/donhang/donhanghdv/donHangHDVView.html',
                        controller: 'donHangHDVController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/donhang/donhanghdv/donHanghdvController.js?v1=' + (new Date().getTime()),
                            '/app/components/donhang/donHangDataService.js?v1=' + (new Date().getTime())
                            ]
                        });
                    }]
                }
            })
            .state('editdonhang', {
                parent: 'layout',
                abstract: false,
                url: '/editdonhang/:iddonhang',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/donhang/editDonHangView.html',
                        controller: 'editDonHangController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/donhang/editDonHangController.js?v1=' + (new Date().getTime()),
                                '/app/services/printService.js',
                            '/app/components/donhang/donHangDataService.js?v1=' + (new Date().getTime())
                            ]
                        });
                    }]
                }
            })
            .state('editdonhangpchl', {
                parent: 'layout',
                abstract: false,
                url: '/editdonhangpchl/:iddonhang',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/donhang/donhangpchl/editDonHangPCHLView.html',
                        controller: 'editDonHangPCHLController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/donhang/donhangpchl/editDonHangPCHLController.js?v1=' + (new Date().getTime()),
                                '/app/services/printService.js',
                            '/app/components/donhang/donHangDataService.js?v1=' + (new Date().getTime())
                            ]
                        });
                    }]
                }
            })
            .state('editdonhangota', {
                parent: 'layout',
                abstract: false,
                url: '/editdonhangota/:iddonhang',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/donhang/donhangota/editDonHangOTAView.html',
                        controller: 'editDonHangOTAController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/donhang/donhangota/editDonHangOTAController.js?v1=' + (new Date().getTime()),
                                '/app/services/printService.js',
                                '/app/components/lspay/lspayDataService.js',
                            '/app/components/donhang/donHangDataService.js?v1=' + (new Date().getTime())
                            ]
                        });
                    }]
                }
            })
            .state('editdonhangpkd', {
                parent: 'layout',
                abstract: false,
                url: '/editdonhangpkd/:iddonhang',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/donhang/donhangpkd/editDonHangPKDView.html',
                        controller: 'editDonHangPKDController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/donhang/donhangpkd/editDonHangPKDController.js?v1=' + (new Date().getTime()),
                                '/app/services/printService.js',
                            '/app/components/donhang/donHangDataService.js?v1=' + (new Date().getTime())
                            ]
                        });
                    }]
                }
            })
            .state('editdonhanghdv', {
                parent: 'layout',
                abstract: false,
                url: '/editdonhanghdv/:iddonhang',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/donhang/donhanghdv/editDonHangHDVView.html',
                        controller: 'editDonHangHDVController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/donhang/donhanghdv/editDonHangHDVController.js?v1=' + (new Date().getTime()),
                                '/app/services/printService.js',
                            '/app/components/donhang/donHangDataService.js?v1=' + (new Date().getTime())
                            ]
                        });
                    }]
                }
            })
            .state('adddonhang', {
                parent: 'layout',
                abstract: false,
                url: '/adddonhang',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/donhang/addDonHangView.html',
                        controller: 'addDonHangController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/donhang/addDonHangController.js?v1=' + (new Date().getTime()),
                                '/app/services/printService.js',
                            '/app/components/donhang/donHangDataService.js?v1=' + (new Date().getTime())
                            ]
                        });
                    }]
                }
            })
            .state('adddonhangdv', {
                parent: 'layout',
                abstract: false,
                url: '/adddonhangdv',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/donhang/addDonHangDichVuView.html',
                        controller: 'addDonHangDichVuController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/donhang/addDonHangDichVuController.js?v1=' + (new Date().getTime()),
                                '/app/services/printService.js',
                            '/app/components/donhang/donHangDataService.js?v1=' + (new Date().getTime())
                            ]
                        });
                    }]
                }
            })
            .state('adddonhangpchl', {
                parent: 'layout',
                abstract: false,
                url: '/adddonhangpchl',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/donhang/donhangpchl/addDonHangPCHLView.html',
                        controller: 'addDonHangPCHLController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/donhang/donhangpchl/addDonHangPCHLController.js?v1=' + (new Date().getTime()),
                                '/app/services/printService.js',
                            '/app/components/donhang/donHangDataService.js?v1=' + (new Date().getTime())
                            ]
                        });
                    }]
                }
            })
            .state('adddonhangota', {
                parent: 'layout',
                abstract: false,
                url: '/adddonhangota',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/donhang/donhangota/addDonHangOTAView.html',
                        controller: 'addDonHangOTAController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/donhang/donhangota/addDonHangOTAController.js?v1=' + (new Date().getTime()),
                                '/app/services/printService.js',
                            '/app/components/donhang/donHangDataService.js?v1=' + (new Date().getTime())
                            ]
                        });
                    }]
                }
            })
            .state('adddonhangpkd', {
                parent: 'layout',
                abstract: false,
                url: '/adddonhangpkd',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/donhang/donhangpkd/addDonHangPKDView.html',
                        controller: 'addDonHangPKDController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/donhang/donhangpkd/addDonHangPKDController.js?v1=' + (new Date().getTime()),
                                '/app/services/printService.js',
                            '/app/components/donhang/donHangDataService.js?v1=' + (new Date().getTime())
                            ]
                        });
                    }]
                }
            })
            .state('adddonhangpos', {
                parent: 'layout',
                abstract: false,
                url: '/adddonhangpos',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/donhang/addDonHangViewPOS.html',
                        controller: 'addDonHangPOSController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/donhang/addDonHangPOSController.js',
                                '/app/services/printService.js',
                                '/app/components/donhang/donHangDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('posbanve', {
                parent: 'layout',
                abstract: false,
                url: '/posbanve',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/donhang/posbanve/posBanVeViewPOS.html',
                        controller: 'posBanVeController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/donhang/posbanve/posBanVeController.js?v1=' + (new Date().getTime()),
                                '/app/services/printService.js',
                                '/app/components/donhang/donHangDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('posbanvebd', {
                parent: 'layout',
                abstract: false,
                url: '/posbanvebd',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/donhang/posbanvebaidinh/posBanVeBDView.html',
                        controller: 'posBanVeBDController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/donhang/posbanvebaidinh/posBanVeBDController.js?v1=' + (new Date().getTime()),
                                '/app/services/printService.js',
                                '/app/components/donhang/donHangDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('posbanvetag', {
                parent: 'layout',
                abstract: false,
                url: '/posbanvetag',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/donhang/posbanvetag/posBanVeTAGView.html',
                        controller: 'posBanVeTAGController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/donhang/posbanvetag/posBanVeTAGController.js?v1=' + (new Date().getTime()),
                                '/app/services/printService.js',
                                '/app/components/donhang/donHangDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('posbanvepchl', {
                parent: 'layout',
                abstract: false,
                url: '/posbanvepchl',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/donhang/posbanvepchl/posBanVePCHLView.html',
                        controller: 'posBanVePCHLController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/donhang/posbanvepchl/posBanVePCHLController.js?v1=' + (new Date().getTime()),
                                '/app/services/printService.js',
                                '/app/components/donhang/donHangDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('baocaotonghopdonhang', {
                parent: 'layout',
                abstract: false,
                url: '/baocaotonghopdonhang',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/donhang/baocaodonhang/tongHopDonHangView.html',
                        controller: 'tongHopDonHangController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/donhang/baocaodonhang/tongHopDonHangController.js',
                                '/app/components/donhang/baocaodonhang/baoCaoDonHangDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('baocaodonhangtheonhanvien', {
                parent: 'layout',
                abstract: false,
                url: '/baocaodonhangtheonhanvien',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/donhang/baocaodonhang/donHangTheoNhanVienView.html',
                        controller: 'donHangTheoNhanVienController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/donhang/baocaodonhang/donHangTheoNhanVienController.js',
                                '/app/components/donhang/baocaodonhang/baoCaoDonHangDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('baocaodoanhthu', {
                parent: 'layout',
                abstract: false,
                url: '/baocaodoanhthu',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/donhang/baocaodonhang/baoCaoDoanhThuView.html',
                        controller: 'baoCaoDoanhThuController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/donhang/baocaodonhang/baoCaoDoanhThuController.js',
                                '/app/components/donhang/baocaodonhang/baoCaoDonHangDataService.js'
                            ]
                        });
                    }]
                }
            })

        $stateProvider
            .state('khuyenmai', {
                parent: 'layout',
                abstract: false,
                url: '/khuyenmai',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/khuyenmai/khuyenMaiView.html',
                        controller: 'khuyenMaiController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/khuyenmai/khuyenMaiController.js',
                                '/app/components/khuyenmai/khuyenMaiDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('editkhuyenmai', {
                parent: 'layout',
                abstract: false,
                url: '/editkhuyenmai/:idkhuyenmai',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/khuyenmai/editKhuyenMaiView.html',
                        controller: 'editKhuyenMaiController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/khuyenmai/editKhuyenMaiController.js',
                                '/app/components/khuyenmai/khuyenMaiDataService.js'
                            ]
                        });
                    }]
                }
            })

        $stateProvider
            .state('kehoachnhanvien', {
                parent: 'layout',
                abstract: false,
                url: '/kehoachnhanvien',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/kehoach/keHoachNhanVienView.html',
                        controller: 'keHoachNhanVienController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load('/app/components/kehoach/keHoachNhanVienController.js');
                    }]
                }
            })
            .state('kehoachnhanvienV2', {
                parent: 'layout',
                abstract: false,
                url: '/kehoachnhanvienV2',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/kehoach/keHoachNhanVienV2View.html',
                        controller: 'keHoachNhanVienController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load('/app/components/kehoach/keHoachNhanVienV2Controller.js');
                    }]
                }
            })
            .state('baocaokehoachnhanvien', {
                parent: 'layout',
                abstract: false,
                url: '/baocaokehoachnhanvien',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/kehoach/baoCaoKeHoachNhanVienView.html',
                        controller: 'baoCaoKeHoachNhanVienController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/kehoach/baoCaoKeHoachNhanVienController.js',
                                '/app/components/kehoach/baoCaoKeHoachDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('kehoachgiaohang', {
                parent: 'layout',
                abstract: false,
                url: '/kehoachgiaohang',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/kehoach/keHoachGiaoHangView.html',
                        controller: 'keHoachGiaoHangController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load('/app/components/kehoach/keHoachGiaoHangController.js');
                    }]
                }
            })
            .state('kehoachbaoduong', {
                parent: 'layout',
                abstract: false,
                url: '/kehoachbaoduong',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/kehoach/keHoachBaoDuongView.html',
                        controller: 'keHoachBaoDuongController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load('/app/components/kehoach/keHoachBaoDuongController.js');
                    }]
                }
            })
            .state('quanlytuyen', {
                parent: 'layout',
                abstract: false,
                url: '/quanlytuyen',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/kehoach/keHoachTuyenKhachHangView.html',
                        controller: 'keHoachTuyenKhachHangController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/kehoach/keHoachTuyenKhachHangController.js',
                                '/app/components/kehoach/keHoachTuyenKhachHangDataService.js'
                            ]
                        });
                    }]
                }
            })

        $stateProvider
            .state('danhmuckhohang', {
                parent: 'layout',
                abstract: false,
                url: '/danhmuckhohang',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/khohang/khoHangView.html',
                        controller: 'khoHangController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/khohang/khoHangController.js',
                                '/app/components/khohang/khoHangDataService.js',
                                '/app/components/nhanvien/nhanVienDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('tonkhobandau', {
                parent: 'layout',
                abstract: false,
                url: '/tonkhobandau',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/khohang/tonKhoBanDauView.html',
                        controller: 'tonKhoBanDauController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/khohang/tonKhoBanDauController.js',
                                '/app/components/khohang/khoHangDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('phieudieuchuyen', {
                parent: 'layout',
                abstract: false,
                url: '/phieudieuchuyen',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/khohang/phieuDieuChuyenView.html',
                        controller: 'phieuDieuChuyenController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/khohang/phieuDieuChuyenController.js',
                                '/app/components/khohang/khoHangDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('phieunhapkho', {
                parent: 'layout',
                abstract: false,
                url: '/phieunhapkho',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/khohang/phieuNhapView.html',
                        controller: 'phieuNhapController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/khohang/phieuNhapController.js',
                                '/app/components/khohang/khoHangDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('phieudieuchinh', {
                parent: 'layout',
                abstract: false,
                url: '/phieudieuchinh',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/khohang/phieuDieuChinhView.html',
                        controller: 'phieuDieuChinhController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/khohang/phieuDieuChinhController.js',
                                '/app/components/khohang/khoHangDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('baocaodieuchinh', {
                parent: 'layout',
                abstract: false,
                url: '/baocaodieuchinh',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/khohang/baoCaoDieuChinhView.html',
                        controller: 'baoCaoDieuChinhController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/khohang/baoCaoDieuChinhController.js',
                                '/app/components/khohang/khoHangDataService.js'
                            ]
                        })
                    }]
                }
            })

        $stateProvider
            .state('lotrinhdichuyen', {
                parent: 'layout',
                abstract: false,
                url: '/lotrinhdichuyen?idnhanvien&tungay&denngay',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/maps/loTrinhDiChuyenView.html',
                        controller: 'loTrinhDiChuyenController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/maps/loTrinhDiChuyenController.js',
                                '/app/components/maps/mapsDataService.js',
                                '/Scripts/markerclusterer.js'
                            ]
                        });
                    }]
                }
            })
            .state('vitrikhachhang', {
                parent: 'layout',
                abstract: false,
                url: '/vitrikhachhang?kinhdo&vido',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/maps/viTriKhachHangView.html',
                        controller: 'viTriKhachHangController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/maps/viTriKhachHangController.js',
                                '/app/components/maps/mapsDataService.js',
                                '/Scripts/markerclusterer.js'
                            ]
                        });
                    }]
                }
            })
            .state('vitrinhanvien', {
                parent: 'layout',
                abstract: false,
                url: '/vitrinhanvien',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/maps/viTriNhanVienView.html',
                        controller: 'viTriNhanVienController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/maps/viTriNhanVienController.js',
                                '/app/components/maps/mapsDataService.js',
                                '/Scripts/markerclusterer.js'
                            ]
                        });
                    }]
                }
            })
            .state('editnhanvien', {
                parent: 'layout',
                abstract: false,
                url: '/editnhanvien/:idnhanvien?idnhom',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/nhanvien/editNhanVienView.html',
                        controller: 'editNhanVienController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/nhanvien/editNhanVienController.js',
                                '/app/components/nhanvien/nhanVienDataService.js',
                                '/app/components/danhmuc/danhmucDataService.js'
                            ]
                        });
                    }]
                }
            })

        $stateProvider
            .state('cauhinh', {
                parent: 'layout',
                abstract: false,
                url: '/cauhinh',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/cauhinh/cauHinhView.html',
                        controller: 'cauHinhController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/cauhinh/cauHinhController.js'
                            ]
                        });
                    }]
                }
            })
            .state('doimatkhau', {
                parent: 'layout',
                abstract: false,
                url: '/doimatkhau',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/cauhinh/doiMatKhauView.html',
                        controller: 'doiMatKhauController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/cauhinh/doiMatKhauController.js',
                                '/app/services/AuthenticationService.js'
                            ]
                        });
                    }]
                }
            })
            .state('phanquyenchucnang', {
                parent: 'layout',
                abstract: false,
                url: '/phanquyenchucnang',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/quantri/phanQuyenChucNangView.html',
                        controller: 'phanQuyenChucNangController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/quantri/phanQuyenChucNangController.js',
                                '/app/components/quantri/phanQuyenDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('phanquyenkhachhangnhomnhanvien', {
                parent: 'layout',
                abstract: false,
                url: '/phanquyenkhachhangnhomnhanvien',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/quantri/phanQuyenKhachHangTheoNhomNhanVienView.html',
                        controller: 'phanQuyenKhachHangTheoNhomNhanVienController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/quantri/phanQuyenKhachHangTheoNhomNhanVienController.js',
                                '/app/components/quantri/phanQuyenDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('phanquyennganhhangnhomnhanvien', {
                parent: 'layout',
                abstract: false,
                url: '/phanquyennganhhangnhomnhanvien',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/quantri/phanQuyenNganhHangTheoNhomNhanVienView.html',
                        controller: 'phanQuyenNganhHangTheoNhomNhanVienController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/quantri/phanQuyenNganhHangTheoNhomNhanVienController.js',
                                '/app/components/quantri/phanQuyenDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('phanquyennhanvienkhanhhang', {
                parent: 'layout',
                abstract: false,
                url: '/phanquyennhanvienkhanhhang',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/quantri/phanQuyenNhanVienTheoKhachHangView.html',
                        controller: 'phanQuyenNhanVienTheoKhachHangController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/quantri/phanQuyenNhanVienTheoKhachHangController.js',
                                '/app/components/quantri/phanQuyenDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('phanquyenkhachhangnhanvien', {
                parent: 'layout',
                abstract: false,
                url: '/phanquyenkhachhangnhanvien',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/quantri/phanQuyenKhachHangTheoNhanVienView.html',
                        controller: 'phanQuyenKhachHangTheoNhanVienController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/quantri/phanQuyenKhachHangTheoNhanVienController.js',
                                '/app/components/quantri/phanQuyenDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('quanlyweb', {
                parent: 'layout',
                abstract: false,
                url: '/quanlyweb',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/quantri/quanLyView.html',
                        controller: 'quanLyController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/quantri/quanLyController.js',
                                '/app/components/quantri/quanTriDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('inthea6', {
                abstract: false,
                url: '/inthea6',
                views: {
                    'viewMain': {
                        templateUrl: '/app/components/quantri/inTheA6View.html',
                        controller: 'inTheA6Controller'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/quantri/inTheA6Controller.js',
                                '/app/components/quantri/quanTriDataService.js'
                            ]
                        });
                    }]
                }
            })

        $stateProvider
            .state('danhmuc', {
                parent: 'layout',
                abstract: false,
                url: '/danhmuc',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/danhmuc/danhMucChungView.html',
                        controller: 'danhMucChungController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/danhmuc/danhMucChungController.js',
                                '/app/components/danhmuc/danhMucDataService.js',
                                '/app/components/donhang/donHangDataService.js'
                            ]
                        });
                    }]
                }
            })

        $stateProvider
            .state('quanlyxe', {
                parent: 'layout',
                abstract: false,
                url: '/quanlyxe',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/quanlyxe/quanLyXeView.html',
                        controller: 'quanLyXeController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/quanlyxe/quanLyXeController.js',
                                '/app/components/quanlyxe/quanLyXeDataService.js'
                            ]
                        });
                    }]
                }
            })

        $stateProvider
            .state('album', {
                parent: 'layout',
                abstract: false,
                url: '/album?idalbum&idcheckin&idlichsubaoduong&idimage',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/album/albumView.html',
                        controller: 'albumController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/album/albumController.js',
                                '/app/components/album/albumDataService.js'
                            ]
                        });
                    }]
                }
            })

        $stateProvider
            .state('hanmuckhachhang', {
                parent: 'layout',
                abstract: false,
                url: '/hanmuckhachhang',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/congno/hanMucCongNoKhachHangView.html',
                        controller: 'hanMucCongNoKhachHangController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/congno/hanMucCongNoKhachHangController.js',
                                '/app/components/congno/hanMucDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('hanmucnhanvien', {
                parent: 'layout',
                abstract: false,
                url: '/hanmucnhanvien',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/congno/hanMucCongNoNhanVienView.html',
                        controller: 'hanMucCongNoNhanVienController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/congno/hanMucCongNoNhanVienController.js',
                                '/app/components/congno/hanMucDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('hanmucnhomnhanvien', {
                parent: 'layout',
                abstract: false,
                url: '/hanmucnhomnhanvien',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/congno/hanMucCongNoNhomNhanVienView.html',
                        controller: 'hanMucCongNoNhomNhanVienController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/congno/hanMucCongNoNhomNhanVienController.js',
                                '/app/components/congno/hanMucDataService.js'
                            ]
                        });
                    }]
                }
            })

            .state('mucchietkhaunhomnhanvien', {
                parent: 'layout',
                abstract: false,
                url: '/mucchietkhaunhomnhanvien',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/chietkhaudaily/cauHinhChietKhauDaiLyView.html',
                        controller: 'cauHinhChietKhauDaiLyController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/chietkhaudaily/cauHinhChietKhauDaiLyController.js',
                                '/app/components/chietkhaudaily/chietKhauDataService.js'
                            ]
                        });
                    }]
                }
            })

        $stateProvider
            .state('congviec', {
                parent: 'layout',
                abstract: false,
                url: '/congviec',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/congviec/congViecView.html',
                        controller: 'congViecController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/congviec/congViecController.js',
                                '/app/components/congviec/congViecDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('chitieukpi', {
                parent: 'layout',
                abstract: false,
                url: '/chitieukpi',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/congviec/chiTieuKPIView.html',
                        controller: 'chiTieuKPIController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/congviec/chiTieuKPIController.js',
                                '/app/components/congviec/congViecDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('guitinnhan', {
                parent: 'layout',
                abstract: false,
                url: '/guitinnhan',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/tinnhan/guiTinNhanView.html',
                        controller: 'guiTinNhanController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/tinnhan/guiTinNhanController.js',
                                '/app/components/tinnhan/tinNhanDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('baocaoguitinnhan_nv', {
                parent: 'layout',
                abstract: false,
                url: '/baocaoguitinnhan/:idnhanvien',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/tinnhan/baoCaoGuiTinNhanView.html',
                        controller: 'baoCaoGuiTinNhanController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/tinnhan/baoCaoGuiTinNhanController.js',
                                '/app/components/tinnhan/tinNhanDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('baocaoguitinnhan', {
                parent: 'layout',
                abstract: false,
                url: '/baocaoguitinnhan?chuadoc',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/tinnhan/baoCaoGuiTinNhanView.html',
                        controller: 'baoCaoGuiTinNhanController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/tinnhan/baoCaoGuiTinNhanController.js',
                                '/app/components/tinnhan/tinNhanDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('baocaotinnhannhanvien', {
                parent: 'layout',
                abstract: false,
                url: '/baocaotinnhannhanvien',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/tinnhan/baoCaoTinNhanNhanVienView.html',
                        controller: 'baoCaoTinNhanNhanVienController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/tinnhan/baoCaoTinNhanNhanVienController.js',
                                '/app/components/tinnhan/tinNhanDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('baocaochitiettinnhan', {
                parent: 'layout',
                abstract: false,
                url: '/baocaochitiettinnhan?idtinnhan',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/tinnhan/baoCaoChiTietTinNhanView.html',
                        controller: 'baoCaoChiTietTinNhanController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/tinnhan/baoCaoChiTietTinNhanController.js',
                                '/app/components/tinnhan/tinNhanDataService.js'
                            ]
                        });
                    }]
                }
            })

        $stateProvider
            .state('baocaotheodoicongno', {
                parent: 'layout',
                abstract: false,
                url: '/baocaotheodoicongno',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/baocaocongno/bm001_BaoCaoTheoDoiCongNoView.html',
                        controller: 'bm001_BaoCaoTheoDoiCongNoController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/baocaocongno/bm001_BaoCaoTheoDoiCongNoController.js',
                                '/app/components/baocao/baocaocongno/baoCaoCongNoDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('baocaothuhoicongno', {
                parent: 'layout',
                abstract: false,
                url: '/baocaothuhoicongno',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/baocaocongno/bm002_BaoCaoThuHoiCongNoView.html',
                        controller: 'bm002_BaoCaoThuHoiCongNoController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/baocaocongno/bm002_BaoCaoThuHoiCongNoController.js',
                                '/app/components/baocao/baocaocongno/baoCaoCongNoDataService.js'
                            ]
                        });
                    }]
                }
            })

        $stateProvider
            .state('baocaocackhonhapxuatton', {
                parent: 'layout',
                abstract: false,
                url: '/baocaocackhonhapxuatton',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/baocaokhohang/bm014_BaoCaoNhapXuatTonCacKhoView.html',
                        controller: 'bm014_BaoCaoNhapXuatTonCacKhoController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/baocaokhohang/bm014_BaoCaoNhapXuatTonCacKhoController.js',
                                '/app/components/baocao/baocaokhohang/baoCaoKhoHangDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('baocaonhapxuatton', {
                parent: 'layout',
                abstract: false,
                url: '/baocaonhapxuatton',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/baocaokhohang/bm015_BaoCaoNhapXuatTonView.html',
                        controller: 'bm015_BaoCaoNhapXuatTonController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/baocaokhohang/bm015_BaoCaoNhapXuatTonController.js',
                                '/app/components/baocao/baocaokhohang/baoCaoKhoHangDataService.js'
                            ]
                        });
                    }]
                }
            })

        $stateProvider
            .state('tonghopdoanhthuticketota', {
                parent: 'layout',
                abstract: false,
                url: '/tonghopdoanhthuticketota',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/bieudodoanhthu/tonghopdoanhthuticketotaView.html',
                        controller: 'tonghopdoanhthuticketotaController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/bieudodoanhthu/tonghopdoanhthuticketotaController.js',
                                '/app/components/baocao/bieudodoanhthu/bieuDoDoanhThuDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('tonghopdoanhthutheocaticketota', {
                parent: 'layout',
                abstract: false,
                url: '/tonghopdoanhthutheocaticketota',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/bieudodoanhthu/tonghopdoanhthutheocaticketotaView.html',
                        controller: 'tonghopdoanhthutheocaticketotaController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/bieudodoanhthu/tonghopdoanhthutheocaticketotaController.js',
                                '/app/components/baocao/bieudodoanhthu/bieuDoDoanhThuDataService.js'
                            ]
                        });
                    }]
                }
            })

            .state('tonghopthanhtoanthungan', {
                parent: 'layout',
                abstract: false,
                url: '/tonghopthanhtoanthungan',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/bieudodoanhthu/tonghopthanhtoanthunganView.html',
                        controller: 'tonghopthanhtoanthunganController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/bieudodoanhthu/tonghopthanhtoanthunganController.js',
                                '/app/components/baocao/bieudodoanhthu/bieuDoDoanhThuDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('tonghophinhthucthanhtoan', {
                parent: 'layout',
                abstract: false,
                url: '/tonghophinhthucthanhtoan',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/bieudodoanhthu/tonghophinhthucthanhtoanView.html',
                        controller: 'tonghophinhthucthanhtoanController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/bieudodoanhthu/tonghophinhthucthanhtoanController.js',
                                '/app/components/baocao/bieudodoanhthu/bieuDoDoanhThuDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('tonghophttt', {
                parent: 'layout',
                abstract: false,
                url: '/tonghophttt',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/bieudodoanhthu/tonghophtttView.html',
                        controller: 'tonghophtttController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/bieudodoanhthu/tonghophtttController.js',
                                '/app/components/baocao/bieudodoanhthu/bieuDoDoanhThuDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('tonghopdichvutheobooking', {
                parent: 'layout',
                abstract: false,
                url: '/tonghopdichvutheobooking',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/bieudodoanhthu/tonghopdichvutheobookingView.html',
                        controller: 'tonghopdichvutheobookingController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/bieudodoanhthu/tonghopdichvutheobookingController.js',
                                '/app/components/baocao/bieudodoanhthu/bieuDoDoanhThuDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('tonghopdichvutheobooking2', {
                parent: 'layout',
                abstract: false,
                url: '/tonghopdichvutheobookingnoticket',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/bieudodoanhthu/tonghopdichvutheobooking2View.html',
                        controller: 'tonghopdichvutheobooking2Controller'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/bieudodoanhthu/tonghopdichvutheobooking2Controller.js',
                                '/app/components/baocao/bieudodoanhthu/bieuDoDoanhThuDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('baocaodoisoatdichvu', {
                parent: 'layout',
                abstract: false,
                url: '/baocaodoisoatdichvu',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/baocaokhachhang/baocaodoisoatdichvuView.html',
                        controller: 'baocaodoisoatdichvuController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/baocaokhachhang/baocaodoisoatdichvuController.js',
                                '/app/components/baocao/baocaokhachhang/baoCaoKhachHangDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('bieudodoanhthuthang', {
                parent: 'layout',
                abstract: false,
                url: '/bieudodoanhthuthang',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/bieudodoanhthu/bm034_BieuDoDoanhThuThangView.html',
                        controller: 'bm034_BieuDoDoanhThuThangController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/bieudodoanhthu/bm034_BieuDoDoanhThuThangController.js',
                                '/app/components/baocao/bieudodoanhthu/bieuDoDoanhThuDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('bieudodoanhthutheokhuvuc', {
                parent: 'layout',
                abstract: false,
                url: '/bieudodoanhthutheokhuvuc',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/bieudodoanhthu/bm035_BieuDoDoanhThuTheoKhuVucView.html',
                        controller: 'bm035_BieuDoDoanhThuTheoKhuVucController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/bieudodoanhthu/bm035_BieuDoDoanhThuTheoKhuVucController.js',
                                '/app/components/baocao/bieudodoanhthu/bieuDoDoanhThuDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('bieudodoanhthutheonhanvien', {
                parent: 'layout',
                abstract: false,
                url: '/bieudodoanhthutheonhanvien',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/bieudodoanhthu/bm036_BieuDoDoanhThuTheoNhanVienView.html',
                        controller: 'bm036_BieuDoDoanhThuTheoNhanVienController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/bieudodoanhthu/bm036_BieuDoDoanhThuTheoNhanVienController.js',
                                '/app/components/baocao/bieudodoanhthu/bieuDoDoanhThuDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('bieudodoanhthutheonhomnhanvien', {
                parent: 'layout',
                abstract: false,
                url: '/bieudodoanhthutheonhomnhanvien',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/bieudodoanhthu/bm037_BieuDoDoanhThuTheoNhomNhanVienView.html',
                        controller: 'bm037_BieuDoDoanhThuTheoNhomNhanVienController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/bieudodoanhthu/bm037_BieuDoDoanhThuTheoNhomNhanVienController.js',
                                '/app/components/baocao/bieudodoanhthu/bieuDoDoanhThuDataService.js'
                            ]
                        });
                    }]
                }
            })

            .state('bieudochietkhaudoanhthutheonhomnhanvien', {
                parent: 'layout',
                abstract: false,
                url: '/bieudochietkhaudoanhthutheonhomnhanvien',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/bieudodoanhthu/bieuDoChietKhauDoanhThuTheoNhomNhanVienView.html',
                        controller: 'bieuDoChietKhauDoanhThuTheoNhomNhanVienController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/bieudodoanhthu/bieuDoChietKhauDoanhThuTheoNhomNhanVienController.js',
                                '/app/components/baocao/bieudodoanhthu/bieuDoDoanhThuDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('bieudotopdoanhthutheokhachhang', {
                parent: 'layout',
                abstract: false,
                url: '/bieudotopdoanhthutheokhachhang',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/bieudodoanhthu/bm038_BieuDoTop10DoanhThuTheoKhachHangView.html',
                        controller: 'bm038_BieuDoTop10DoanhThuTheoKhachHangController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/bieudodoanhthu/bm038_BieuDoTop10DoanhThuTheoKhachHangController.js',
                                '/app/components/baocao/bieudodoanhthu/bieuDoDoanhThuDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('bieudotopdoanhthutheonhanvien', {
                parent: 'layout',
                abstract: false,
                url: '/bieudotopdoanhthutheonhanvien',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/bieudodoanhthu/bm039_BieuDoTop10DoanhThuTheoNhanVienView.html',
                        controller: 'bm039_BieuDoTop10DoanhThuTheoNhanVienController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/bieudodoanhthu/bm039_BieuDoTop10DoanhThuTheoNhanVienController.js',
                                '/app/components/baocao/bieudodoanhthu/bieuDoDoanhThuDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('bieudotytrongdoanhthukhachhang', {
                parent: 'layout',
                abstract: false,
                url: '/bieudotytrongdoanhthukhachhang',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/bieudodoanhthu/bm040_BieuDoTyTrongDoanhThuTheoKhachHangView.html',
                        controller: 'bm040_BieuDoTyTrongDoanhThuTheoKhachHangController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/bieudodoanhthu/bm040_BieuDoTyTrongDoanhThuTheoKhachHangController.js',
                                '/app/components/baocao/bieudodoanhthu/bieuDoDoanhThuDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('baocaobanvecacdiem', {
                parent: 'layout',
                abstract: false,
                url: '/baocaobanvecacdiem',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/bieudodoanhthu/bm041_BaoCaoDoanhThuCacDiem.html',
                        controller: 'bm041_BaoCaoDoanhThuCacDiemController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/bieudodoanhthu/bm041_BaoCaoDoanhThuCacDiemController.js',
                                '/app/components/baocao/bieudodoanhthu/bieuDoDoanhThuDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('baocaobanveketnoidisan', {
                parent: 'layout',
                abstract: false,
                url: '/baocaobanveketnoidisan',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/bieudodoanhthu/bm042_BaoCaoDoanhThuCacDiemV2.html',
                        controller: 'bm042_BaoCaoDoanhThuCacDiemV2Controller'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/bieudodoanhthu/bm042_BaoCaoDoanhThuCacDiemV2Controller.js',
                                '/app/components/baocao/bieudodoanhthu/bieuDoDoanhThuDataService.js'
                            ]
                        });
                    }]
                }
            })

        $stateProvider
            .state('bieudodonhangtheokhuvuc', {
                parent: 'layout',
                abstract: false,
                url: '/bieudodonhangtheokhuvuc',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/bieudosanluong/bm041_BieuDoDonHangTheoKhuVucView.html',
                        controller: 'bm041_BieuDoDonHangTheoKhuVucController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/bieudosanluong/bm041_BieuDoDonHangTheoKhuVucController.js',
                                '/app/components/baocao/bieudosanluong/baoCaoSanLuongDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('bieudodonhangtheonhanvien', {
                parent: 'layout',
                abstract: false,
                url: '/bieudodonhangtheonhanvien',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/bieudosanluong/bm042_BieuDoDonHangTheoNhanVienView.html',
                        controller: 'bm042_BieuDoDonHangTheoNhanVienController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/bieudosanluong/bm042_BieuDoDonHangTheoNhanVienController.js',
                                '/app/components/baocao/bieudosanluong/baoCaoSanLuongDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('bieudotopdonhangtheokhachhang', {
                parent: 'layout',
                abstract: false,
                url: '/bieudotopdonhangtheokhachhang',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/bieudosanluong/bm043_BieuDoTop10DonHangTheoKhachHangView.html',
                        controller: 'bm043_BieuDoTop10DonHangTheoKhachHangController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/bieudosanluong/bm043_BieuDoTop10DonHangTheoKhachHangController.js',
                                '/app/components/baocao/bieudosanluong/baoCaoSanLuongDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('bieudotopdonhangtheonhanvien', {
                parent: 'layout',
                abstract: false,
                url: '/bieudotopdonhangtheonhanvien',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/bieudosanluong/bm044_BieuDoTop10DonHangTheoNhanVienView.html',
                        controller: 'bm044_BieuDoTop10DonHangTheoNhanVienController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/bieudosanluong/bm044_BieuDoTop10DonHangTheoNhanVienController.js',
                                '/app/components/baocao/bieudosanluong/baoCaoSanLuongDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('bieudotopnhanviendichuyen', {
                parent: 'layout',
                abstract: false,
                url: '/bieudotopnhanviendichuyen',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/bieudosanluong/bm045_BieuDoTop10NhanVienDiChuyenView.html',
                        controller: 'bm045_BieuDoTop10NhanVienDiChuyenController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/bieudosanluong/bm045_BieuDoTop10NhanVienDiChuyenController.js',
                                '/app/components/baocao/bieudosanluong/baoCaoSanLuongDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('bieudotopsanphambanchay', {
                parent: 'layout',
                abstract: false,
                url: '/bieudotopsanphambanchay',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/bieudosanluong/bm046_BieuDoTop10SanPhamBanChayView.html',
                        controller: 'bm046_BieuDoTop10SanPhamBanChayController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/bieudosanluong/bm046_BieuDoTop10SanPhamBanChayController.js',
                                '/app/components/baocao/bieudosanluong/baoCaoSanLuongDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('bieudotopsanphambanthap', {
                parent: 'layout',
                abstract: false,
                url: '/bieudotopsanphambanthap',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/bieudosanluong/bm047_BieuDoTop10SanPhamBanThapView.html',
                        controller: 'bm047_BieuDoTop10SanPhamBanThapController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/bieudosanluong/bm047_BieuDoTop10SanPhamBanThapController.js',
                                '/app/components/baocao/bieudosanluong/baoCaoSanLuongDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('bieudotopviengtham', {
                parent: 'layout',
                abstract: false,
                url: '/bieudotopviengtham',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/bieudosanluong/bm048_BieuDoTop10ViengThamView.html',
                        controller: 'bm048_BieuDoTop10ViengThamController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/bieudosanluong/bm048_BieuDoTop10ViengThamController.js',
                                '/app/components/baocao/bieudosanluong/baoCaoSanLuongDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('bieudokhachhangtheonganhhang', {
                parent: 'layout',
                abstract: false,
                url: '/bieudokhachhangtheonganhhang',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/bieudosanluong/bm049_BieuDoKhachHangTheoNganhHangView.html',
                        controller: 'bm049_BieuDoKhachHangTheoNganhHangController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/bieudosanluong/bm049_BieuDoKhachHangTheoNganhHangController.js',
                                '/app/components/baocao/bieudosanluong/baoCaoSanLuongDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('bieudokhachhangtheokhuvuc', {
                parent: 'layout',
                abstract: false,
                url: '/bieudokhachhangtheokhuvuc',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/bieudosanluong/bm050_BieuDoKhachHangTheoKhuVucView.html',
                        controller: 'bm050_BieuDoKhachHangTheoKhuVucController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/bieudosanluong/bm050_BieuDoKhachHangTheoKhuVucController.js',
                                '/app/components/baocao/bieudosanluong/baoCaoSanLuongDataService.js'
                            ]
                        });
                    }]
                }
            })


        $stateProvider
            .state('baocaoanhchup', {
                parent: 'layout',
                abstract: false,
                url: '/baocaoanhchup',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/baocaokhachhang/bm003_BaoCaoAnhChupView.html',
                        controller: 'bm003_BaoCaoAnhChupController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/baocaokhachhang/bm003_BaoCaoAnhChupController.js',
                                '/app/components/baocao/baocaokhachhang/baoCaoKhachHangDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('baocaodoanhthutheokhachhang', {
                parent: 'layout',
                abstract: false,
                url: '/baocaodoanhthutheokhachhang',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/baocaokhachhang/bm004_BaoCaoDoanhThuTheoKhachHangView.html',
                        controller: 'bm004_BaoCaoDoanhThuTheoKhachHangController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/baocaokhachhang/bm004_BaoCaoDoanhThuTheoKhachHangController.js',
                                '/app/components/baocao/baocaokhachhang/baoCaoKhachHangDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('baocaokhachhangmathangdonhang', {
                parent: 'layout',
                abstract: false,
                url: '/baocaokhachhangmathangdonhang',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/baocaokhachhang/bm005_BaoCaoKhachHangMatHangDonHangView.html',
                        controller: 'bm005_BaoCaoKhachHangMatHangDonHangController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/baocaokhachhang/bm005_BaoCaoKhachHangMatHangDonHangController.js',
                                '/app/components/baocao/baocaokhachhang/baoCaoKhachHangDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('baocaokhachhangmomoi', {
                parent: 'layout',
                abstract: false,
                url: '/baocaokhachhangmomoi',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/baocaokhachhang/bm006_BaoCaoKhachHangMoMoiView.html',
                        controller: 'bm006_BaoCaoKhachHangMoMoiController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/baocaokhachhang/bm006_BaoCaoKhachHangMoMoiController.js',
                                '/app/components/baocao/baocaokhachhang/baoCaoKhachHangDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('baocaokhachhangtheogiaodich', {
                parent: 'layout',
                abstract: false,
                url: '/baocaokhachhangtheogiaodich',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/baocaokhachhang/bm007_BaoCaoKhachHangTheoGiaoDichView.html',
                        controller: 'bm007_BaoCaoKhachHangTheoGiaoDichController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/baocaokhachhang/bm007_BaoCaoKhachHangTheoGiaoDichController.js',
                                '/app/components/baocao/baocaokhachhang/baoCaoKhachHangDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('baocaokhachhangtheokhuvuc', {
                parent: 'layout',
                abstract: false,
                url: '/baocaokhachhangtheokhuvuc',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/baocaokhachhang/bm008_BaoCaoKhachHangTheoKhuVucView.html',
                        controller: 'bm008_BaoCaoKhachHangTheoKhuVucController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/baocaokhachhang/bm008_BaoCaoKhachHangTheoKhuVucController.js',
                                '/app/components/baocao/baocaokhachhang/baoCaoKhachHangDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('baocaomathangkhachhang', {
                parent: 'layout',
                abstract: false,
                url: '/baocaomathangkhachhang',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/baocaokhachhang/bm009_BaoCaoMatHangKhachHangView.html',
                        controller: 'bm009_BaoCaoMatHangKhachHangController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/baocaokhachhang/bm009_BaoCaoMatHangKhachHangController.js',
                                '/app/components/baocao/baocaokhachhang/baoCaoKhachHangDataService.js',
                                '/app/components/donhang/donHangDataService.js?v1=' + (new Date().getTime())
                            ]
                        });
                    }]
                }
            })
            .state('baocaophanhoi', {
                parent: 'layout',
                abstract: false,
                url: '/baocaophanhoi',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/baocaokhachhang/bm010_BaoCaoPhanHoiView.html',
                        controller: 'bm010_BaoCaoPhanHoiController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/baocaokhachhang/bm010_BaoCaoPhanHoiController.js',
                                '/app/components/baocao/baocaokhachhang/baoCaoKhachHangDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('baocaotonghopdonhangtheokhachhang', {
                parent: 'layout',
                abstract: false,
                url: '/baocaotonghopdonhangtheokhachhang',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/baocaokhachhang/bm011_BaoCaoTongHopDonHangTheoKhachHangView.html',
                        controller: 'bm011_BaoCaoTongHopDonHangTheoKhachHangController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/baocaokhachhang/bm011_BaoCaoTongHopDonHangTheoKhachHangController.js',
                                '/app/services/ComboboxDataService.js',
                                '/app/components/baocao/baocaokhachhang/baoCaoKhachHangDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('baocaotonghopvaodiemtheokhachhang', {
                parent: 'layout',
                abstract: false,
                url: '/baocaotonghopvaodiemtheokhachhang',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/baocaokhachhang/bm012_BaoCaoTongHopVaoDiemTheoKhachHangView.html',
                        controller: 'bm012_BaoCaoTongHopVaoDiemTheoKhachHangController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/baocaokhachhang/bm012_BaoCaoTongHopVaoDiemTheoKhachHangController.js',
                                '/app/services/ComboboxDataService.js',
                                '/app/components/baocao/baocaokhachhang/baoCaoKhachHangDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('baocaoviengthamkhachhang', {
                parent: 'layout',
                abstract: false,
                url: '/baocaoviengthamkhachhang',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/baocaokhachhang/bm013_BaoCaoViengThamKhachHangView.html',
                        controller: 'bm013_BaoCaoViengThamKhachHangController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/baocaokhachhang/bm013_BaoCaoViengThamKhachHangController.js',
                                '/app/services/ComboboxDataService.js',
                                '/app/components/baocao/baocaokhachhang/baoCaoKhachHangDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('baocacsoatve', {
                parent: 'layout',
                abstract: false,
                url: '/baocacsoatve',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/baocaokhachhang/BaoCaoSoatVeKhachHang.html',
                        controller: 'BaoCaoSoatVeKhachHangController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/baocaokhachhang/BaoCaoSoatVeKhachHangController.js',
                                '/app/services/ComboboxDataService.js',
                                '/app/components/baocao/baocaokhachhang/baoCaoKhachHangDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('baocaocongsoatve', {
                parent: 'layout',
                abstract: false,
                url: '/baocaocongsoatve',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/baocaokhachhang/BaoCaoCongSoatVe.html',
                        controller: 'BaoCaoCongSoatVeController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/baocaokhachhang/BaoCaoCongSoatVeController.js',
                                '/app/services/ComboboxDataService.js',
                                '/app/components/baocao/baocaokhachhang/baoCaoKhachHangDataService.js'
                            ]
                        });
                    }]
                }
            })

        $stateProvider
            .state('baocaodonhangcobankpi', {
                parent: 'layout',
                abstract: false,
                url: '/baocaodonhangcobankpi',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/baocaonhanvien/bm016_BaoCaoDonHangCoBanKPINhanVienView.html',
                        controller: 'bm016_BaoCaoDonHangCoBanKPINhanVienController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/baocaonhanvien/bm016_BaoCaoDonHangCoBanKPINhanVienController.js',
                                '/app/components/baocao/baocaonhanvien/baoCaoNhanVienDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('baocaodonhangtheodiem', {
                parent: 'layout',
                abstract: false,
                url: '/baocaodonhangtheodiem',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/baocaonhanvien/bm017_BaoCaoDonHangTheoDiemView.html',
                        controller: 'bm017_BaoCaoDonHangTheoDiemController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/baocaonhanvien/bm017_BaoCaoDonHangTheoDiemController.js',
                                '/app/components/baocao/baocaonhanvien/baoCaoNhanVienDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('baocaodungdo', {
                parent: 'layout',
                abstract: false,
                url: '/baocaodungdo',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/baocaonhanvien/bm018_BaoCaoDungDoView.html',
                        controller: 'bm018_BaoCaoDungDoController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/baocaonhanvien/bm018_BaoCaoDungDoController.js',
                                '/app/components/baocao/baocaonhanvien/baoCaoNhanVienDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('baocaochuyendo', {
                parent: 'layout',
                abstract: false,
                url: '/baocaochuyendo',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/baocaonhanvien/bm018_BaoCaoChuyenDoView.html',
                        controller: 'bm018_BaoCaoChuyenDoController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/baocaonhanvien/bm018_BaoCaoChuyenDoController.js',
                                '/app/components/baocao/baocaonhanvien/baoCaoNhanVienDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('baocaokmdichuyen', {
                parent: 'layout',
                abstract: false,
                url: '/baocaokmdichuyen',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/baocaonhanvien/bm019_BaoCaoKMDiChuyenView.html',
                        controller: 'bm019_BaoCaoKMDiChuyenController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/baocaonhanvien/bm019_BaoCaoKMDiChuyenController.js',
                                '/app/components/baocao/baocaonhanvien/baoCaoNhanVienDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('baocaokpinhanvien', {
                parent: 'layout',
                abstract: false,
                url: '/baocaokpinhanvien',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/baocaonhanvien/bm020_BaoCaoKPINhanVienView.html',
                        controller: 'bm020_BaoCaoKPINhanVienController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/baocaonhanvien/bm020_BaoCaoKPINhanVienController.js',
                                '/app/components/baocao/baocaonhanvien/baoCaoNhanVienDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('baocaolichhen', {
                parent: 'layout',
                abstract: false,
                url: '/baocaolichhen',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/baocaonhanvien/bm021_BaoCaoLichHenView.html',
                        controller: 'bm021_BaoCaoLichHenController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/baocaonhanvien/bm021_BaoCaoLichHenController.js',
                                '/app/components/baocao/baocaonhanvien/baoCaoNhanVienDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('baocaolichsugiaohang', {
                parent: 'layout',
                abstract: false,
                url: '/baocaolichsugiaohang',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/baocaonhanvien/bm022_BaoCaoLichSuGiaoHangView.html',
                        controller: 'bm022_BaoCaoLichSuGiaoHangController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/baocaonhanvien/bm022_BaoCaoLichSuGiaoHangController.js',
                                '/app/components/baocao/baocaonhanvien/baoCaoNhanVienDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('baocaolichsumattinhieu', {
                parent: 'layout',
                abstract: false,
                url: '/baocaolichsumattinhieu',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/baocaonhanvien/bm023_BaoCaoLichSuMatTinHieuView.html',
                        controller: 'bm023_BaoCaoLichSuMatTinHieuController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/baocaonhanvien/bm023_BaoCaoLichSuMatTinHieuController.js',
                                '/app/components/baocao/baocaonhanvien/baoCaoNhanVienDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('baocaolichsusuachua', {
                parent: 'layout',
                abstract: false,
                url: '/baocaolichsusuachua',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/baocaonhanvien/bm024_BaoCaoLichSuSuaChuaView.html',
                        controller: 'bm024_BaoCaoLichSuSuaChuaController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/baocaonhanvien/bm024_BaoCaoLichSuSuaChuaController.js',
                                '/app/components/baocao/baocaonhanvien/baoCaoNhanVienDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('baocaofakegps', {
                parent: 'layout',
                abstract: false,
                url: '/baocaofakegps',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/baocaonhanvien/bm025_BaoCaoTinhTrangFakeGPSView.html',
                        controller: 'bm025_BaoCaoTinhTrangFakeGPSController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/baocaonhanvien/bm025_BaoCaoTinhTrangFakeGPSController.js',
                                '/app/components/baocao/baocaonhanvien/baoCaoNhanVienDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('baocaogps', {
                parent: 'layout',
                abstract: false,
                url: '/baocaogps',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/baocaonhanvien/bm026_BaoCaoTinhTrangGPSView.html',
                        controller: 'bm026_BaoCaoTinhTrangGPSController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/baocaonhanvien/bm026_BaoCaoTinhTrangGPSController.js',
                                '/app/components/baocao/baocaonhanvien/baoCaoNhanVienDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('baocaotonghopcongviec', {
                parent: 'layout',
                abstract: false,
                url: '/baocaotonghopcongviec',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/baocaonhanvien/bm027_BaoCaoTongHopCongViecNhanVienView.html',
                        controller: 'bm027_BaoCaoTongHopCongViecNhanVienController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/baocaonhanvien/bm027_BaoCaoTongHopCongViecNhanVienController.js',
                                '/app/components/baocao/baocaonhanvien/baoCaoNhanVienDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('baocaotonghopmathangtheonhanvien', {
                parent: 'layout',
                abstract: false,
                url: '/baocaotonghopmathangtheonhanvien',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/baocaonhanvien/bm028_BaoCaoTongHopMatHangTheoNhanVienView.html',
                        controller: 'bm028_BaoCaoTongHopMatHangTheoNhanVienController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/baocaonhanvien/bm028_BaoCaoTongHopMatHangTheoNhanVienController.js',
                                '/app/components/baocao/baocaonhanvien/baoCaoNhanVienDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('baocaotonghopravaodiem', {
                parent: 'layout',
                abstract: false,
                url: '/baocaotonghopravaodiem',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/baocaonhanvien/bm029_BaoCaoTongHopRaVaoDiemView.html',
                        controller: 'bm029_BaoCaoTongHopRaVaoDiemController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/baocaonhanvien/bm029_BaoCaoTongHopRaVaoDiemController.js',
                                '/app/components/baocao/baocaonhanvien/baoCaoNhanVienDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('baocaoviengthamkhachhangtheotuyen', {
                parent: 'layout',
                abstract: false,
                url: '/baocaoviengthamkhachhangtheotuyen',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/baocaonhanvien/bm030_BaoCaoViengThamKhachHangTheoTuyenView.html',
                        controller: 'bm030_BaoCaoViengThamKhachHangTheoTuyenController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/baocaonhanvien/bm030_BaoCaoViengThamKhachHangTheoTuyenController.js',
                                '/app/components/baocao/baocaonhanvien/baoCaoNhanVienDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('baocaoviengthamtheotuyen', {
                parent: 'layout',
                abstract: false,
                url: '/baocaoviengthamtheotuyen',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/baocaonhanvien/bm031_BaoCaoViengThamTheoTuyenView.html',
                        controller: 'bm031_BaoCaoViengThamTheoTuyenController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/baocaonhanvien/bm031_BaoCaoViengThamTheoTuyenController.js',
                                '/app/components/baocao/baocaonhanvien/baoCaoNhanVienDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('bieudokpichitieu', {
                parent: 'layout',
                abstract: false,
                url: '/bieudokpichitieu',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/baocaonhanvien/bm032_BieuDoKPITheoCacChiTieuView.html',
                        controller: 'bm032_BieuDoKPITheoCacChiTieuController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/baocaonhanvien/bm032_BieuDoKPITheoCacChiTieuController.js',
                                '/app/components/baocao/baocaonhanvien/baoCaoNhanVienDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('bieudokpinhanvien', {
                parent: 'layout',
                abstract: false,
                url: '/bieudokpinhanvien',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/baocaonhanvien/bm033_BieuDoKPITheoNhanVienView.html',
                        controller: 'bm033_BieuDoKPITheoNhanVienController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/baocaonhanvien/bm033_BieuDoKPITheoNhanVienController.js',
                                '/app/components/baocao/baocaonhanvien/baoCaoNhanVienDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('biendongsodutaikhoan', {
                parent: 'layout',
                abstract: false,
                url: '/biendongsodutaikhoan',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/baocaonhanvien/bm034_BienDongSoDuTaiKhoanView.html',
                        controller: 'bm034_BienDongSoDuTaiKhoanController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/baocaonhanvien/bm034_BienDongSoDuTaiKhoanController.js',
                                '/app/components/baocao/baocaonhanvien/baoCaoNhanVienDataService.js',
                                '/app/components/donhang/donHangDataService.js?v1=' + (new Date().getTime())
                            ]
                        });
                    }]
                }
            })


        $stateProvider
            .state('baocaotonghopchuongtrinhkhuyenmai', {
                parent: 'layout',
                abstract: false,
                url: '/baocaotonghopchuongtrinhkhuyenmai',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/baocaokhuyenmai/bm051_BaoCaoTongHopChuongTrinhKhuyenMaiView.html',
                        controller: 'bm051_BaoCaoTongHopChuongTrinhKhuyenMaiController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/baocaokhuyenmai/bm051_BaoCaoTongHopChuongTrinhKhuyenMaiController.js',
                                '/app/components/baocao/baocaokhuyenmai/baoCaoKhuyenMaiDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('baocaochitietchuongtrinhkhuyenmai', {
                parent: 'layout',
                abstract: false,
                url: '/baocaochitietchuongtrinhkhuyenmai',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/baocao/baocaokhuyenmai/bm052_BaoCaoChiTietChuongTrinhKhuyenMaiView.html',
                        controller: 'bm052_BaoCaoChiTietChuongTrinhKhuyenMaiController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/baocao/baocaokhuyenmai/bm052_BaoCaoChiTietChuongTrinhKhuyenMaiController.js',
                                '/app/components/baocao/baocaokhuyenmai/baoCaoKhuyenMaiDataService.js'
                            ]
                        });
                    }]
                }
            })

        $stateProvider
            .state('thongtinlienhe', {
                parent: 'layout',
                abstract: false,
                url: '/thongtinlienhe',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/trogiup/thongTinLienHe.html',
                        controller: 'thongTinLienHeController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/trogiup/thongTinLienHeController.js'
                                //,
                                //'/app/components/trogiup/troGiupDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('thongtinphanmem', {
                parent: 'layout',
                abstract: false,
                url: '/thongtinphanmem',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/trogiup/thongTinPhanMem.html',
                        controller: 'thongTinPhanMemController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/trogiup/thongTinPhanMemController.js'
                                //,
                                //'/app/components/trogiup/troGiupDataService.js'
                            ]
                        });
                    }]
                }
            })
            .state('napvilspay', {
                parent: 'layout',
                abstract: false,
                url: '/napvilspay',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/lspay/napViLspayView.html',
                        controller: 'napViLspayController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: [
                                '/app/components/lspay/napViLspayController.js',
                                '/app/components/lspay/lspayDataService.js',
                                '/app/components/donhang/donHangDataService.js?v1=' + (new Date().getTime())
                            ]
                        });
                    }]
                }
            })
            .state('doisoatonepay', {
                parent: 'layout',
                abstract: false,
                url: '/doisoatonepay',
                views: {
                    'mainContent@layout': {
                        templateUrl: '/app/components/lspay/doisoatonepayView.html',
                        controller: 'doisoatonepayController'
                    }
                }, resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            serie: true,
                            files: ['/app/components/lspay/doisoatonepayController.js',
                                '/app/components/lspay/lspayDataService.js'
                            ]
                        });
                    }]
                }
            })

    }
]);

app.directive('myEnter', function () {
    return function (scope, element, attrs) {
        element.bind("keydown keypress", function (event) {
            if (event.which === 13) {
                scope.$apply(function () {
                    scope.$eval(attrs.myEnter);
                });

                event.preventDefault();
            }
        });
    };
});

app.run(['$rootScope', '$location', '$http', '$cookies', function ($rootScope, $location, $http, $cookies) {
    //// keep user logged in after page refresh
    //$rootScope.UserInfo = $cookies.getObject('userinfo') || {};
    //$rootScope.Authorization = $cookies.getObject('authorization') || null;
    //$rootScope.isAdmin = $cookies.getObject('isadmin');
    //$rootScope.lastiddonhang = $cookies.getObject('lastiddonhang') || 0;    
    //$http.defaults.headers.post['Access-Control-Allow-Headers'] = 'Origin, X-Requested-Width, Content-Type, Accept';
    //$http.defaults.headers.post['Access-Control-Allow-Origin'] = '*';
    $rootScope.lang = $cookies.getObject('ksmartlang') || 'vi-vn';
    if ($rootScope.lang == 'vi-vn') {
        document.title = "LSCLOUD - HỆ THỐNG QUẢN LÝ NHÂN VIÊN KINH DOANH";

    } else {
        document.title = "LsCloud - Manager Sales Employees System";
    }
    $.i18n().destroy();
    $.i18n().locale = $rootScope.lang;
    $rootScope.changeLang = function (key) {
        $rootScope.lang = key;
        $.i18n().destroy();
        $.i18n().locale = key;
        if (key == 'vi-vn') {
            kendo.culture("vi-VN");
        } else {
            kendo.culture("en-US");
        }



        let cookieExp = new Date();
        cookieExp.setDate(cookieExp.getDate() + 7);
        $cookies.putObject('ksmartlang', key, { expires: cookieExp });

        if ($rootScope.Authorization != null && $rootScope.Authorization != '' && $rootScope.Authorization != undefined)
            $http.get(urlApi + '/api/userinfo/changelang?lang=' + $rootScope.lang.substring(0, 2)).then(function (response) {
                window.location.reload();
            });
    }
    $rootScope.bindTextI18n = function (key) {
        return $.i18n(key);
    }


    //if ($rootScope.Authorization != null && $rootScope.Authorization != '' && $rootScope.Authorization != undefined) {
    //    $http.defaults.headers.common['Authorization'] = $rootScope.Authorization;
    //}

    $rootScope.$on('$locationChangeStart', function (event, next, current) {
        $('#navbarNavDropdown').collapse('hide');
        $rootScope.UserInfo = $cookies.getObject('userinfo') || {};
        $rootScope.Authorization = $cookies.getObject('authorization') || null;
        $rootScope.isAdmin = $cookies.getObject('isadmin');
        $rootScope.lastiddonhang = $cookies.getObject('lastiddonhang') || 0;
        if ($rootScope.Authorization != null && $rootScope.Authorization != '' && $rootScope.Authorization != undefined) {
            $http.defaults.headers.common['Authorization'] = $rootScope.Authorization;
        }

        // redirect to login page if not logged in and trying to access a restricted page
        var restrictedPage = $.inArray($location.path(), ['/login', '/dangkyhdv']) === -1;
        var loggedIn = ($rootScope.Authorization != null && $rootScope.Authorization != '' && $rootScope.Authorization != undefined);
        if (restrictedPage && !loggedIn) {
            $location.path('/login');
        }
    });

    $rootScope.$on('$locationChangeSuccess', function () {
    });
}]);
