(function () {
    'use strict';

    angular.module('portfolioApp', [
        'ngRoute',
        'angularFileUpload'
    ])
        .config(function ($routeProvider) {
            $routeProvider
                .when("/", {
                    templateUrl: "PortfolioApp/Templates/Info.Controller.Template.html",
                    controller: 'infoController',
                    controllerAs: 'vm'
                })
                .when("/photographer", {
                    templateUrl: "PortfolioApp/Templates/Photos.Controller.Template.html",
                    controller: "photosController",
                    controllerAs: "vm"
                });
        });
})();