(function () {
    'use strict';

    angular
        .module('portfolioApp')
        .controller('infoController', infoController);
    

    function infoController($http) {
        var vm = this;
        vm.title = 'Info';
        vm.getUrl = '/api/photographers';
        vm.postUrl = '/api/create-photographer/';
        vm.photographers = [];
        vm.pagePhotographers = [];
        vm.pages = [];
        vm.currentPage = 1;
        vm.maxPages = 1;
        
        

        activate();

        function activate() {
            vm.createPhotographer = createPhGrapher;
            vm.getData = getData;
            vm.goToPhotographer = goToPhotographer;
            vm.goToPage = goToPage;
            vm.getData();
            vm.maxPages = Math.ceil(vm.photographers.length / 3);
        }

        function createPhGrapher() {
            var data = {
                'name': vm.newName,
                'birthDate': vm.newBirthDate
            };
            
            $http.post(vm.postUrl,
                JSON.stringify(data),
                {
                    headers: {
                        'Content-Type': 'application/json'
                    }
                }
                 )
                .then(function (responce) {
                    //success
                    vm.photographers = responce.data.result;
                })
                .catch(function (responce) {
                    //failure
                    alert('post failure');
                })
                .finally(function () {
                    //finally
                    vm.maxPages = Math.ceil(vm.photographers.length / 3);
                    vm.goToPage(vm.currentPage);
                });
        }

        function getData() {

            
            $http.get(vm.getUrl)
                .then(function (responce) {
                    //success
                    
                    vm.photographers = responce.data.result;
                })
                .catch(function (responce) {
                    //failure
                    alert('get failure');
                })
                .finally(function () {
                    //finally
                    vm.maxPages = Math.ceil(vm.photographers.length / 3);
                    vm.goToPage(vm.currentPage);
                });
        }

        function goToPhotographer(id) {
            console.log('goToPhotographer(' + id + ')');
        }

        function constructPages() {
            
            if (vm.maxPages <= 7) {
                vm.pages = new Array(vm.maxPages);
                for (var i = 0; i < vm.pages.length; i++) {
                    vm.pages[i] = i+1;
                }
            } else {
                vm.pages = new Array(7);
                vm.pages[0] = 1;
                vm.pages[6] = vm.maxPages;
                if (vm.currentPage <= 4) {
                    for (var k = 1; k < 6; k++) {
                        vm.pages[k] = k+1;
                    }
                } else if (vm.currentPage >= vm.maxPages - 3) {
                    var counter = 1;
                    for (var j = - 5; j < 0; j++) {
                        vm.pages[counter++] = vm.maxPages + j;
                    }
                } else {
                    var count = 1;
                    for (var h = - 2; h < 3; h++) {
                        vm.pages[count++] = vm.currentPage + h;
                    }
                }
            }
            
        }

        function goToPage(page) {
            if (page > vm.maxPages) {
                vm.currentPage = 1;
            } else {
                vm.currentPage = page;
            }
            constructPages();
            var firstItem = (vm.currentPage - 1) * 3;
            if (firstItem + 3 > vm.photographers.length) {
                vm.pagePhotographers = vm.photographers.slice(firstItem, vm.photographers.length);
            } else {
                vm.pagePhotographers = vm.photographers.slice(firstItem, firstItem + 3);
                
            }
            console.log(page, vm.maxPages);
        }
    }
})();
