(function () {
    'use strict';

    angular
        .module('portfolioApp')
        .controller('infoController', infoController);
    

    function infoController($http) {
        var vm = this;
        vm.title = 'Info';
        vm.url = '';

        activate();

        function activate() {
            vm.getData();
            vm.createPhotographer = createPhGrapher;
        }

        function createPhGrapher() {
            
            
            $http.post(vm.url + '/' + vm.newName + '/' + vm.newBirthDate)
                .success(function (responce) {
                    //success
                })
                .error(function (responce) {
                    //failure
                })
                .finally(function () {
                    //finally
                });
        }

        function getData() {
            $http.get(vm.url)
                .success(function (responce) {
                    //success
                })
                .error(function (responce) {
                    //failure
                })
                .finally(function () {
                    //finally
                });
        }
    }
})();
