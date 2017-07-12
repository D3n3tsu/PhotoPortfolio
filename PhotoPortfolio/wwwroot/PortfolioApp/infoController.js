(function () {
    'use strict';

    angular
        .module('portfolioApp')
        .controller('infoController', infoController);
    

    function infoController() {
        var vm = this;
        vm.title = 'Info';

        activate();

        function activate() { }
    }
})();
