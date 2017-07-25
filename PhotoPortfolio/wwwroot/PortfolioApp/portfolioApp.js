(function () {
    'use strict';

    angular.module('portfolioApp', [
        'ngRoute'
    ])
        .config(function ($routeProvider) {
            $routeProvider
                .when("/", {
                    templateUrl: "PortfolioApp/Templates/Info.Controller.Template.html",
                    controller: 'infoController',
                    controllerAs: 'vm'
                });
        });
})();