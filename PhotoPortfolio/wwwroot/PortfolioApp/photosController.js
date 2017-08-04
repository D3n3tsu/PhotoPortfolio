(function () {
    'use strict';

    angular
        .module('portfolioApp')
        .controller('photosController', photosController);

    function photosController(FileUploader) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'photosController';


        activate();

        function activate() {
            vm.uploader = new FileUploader({
                url: '/api/upload-photo'
            });
        }
    }
})();
