(function() {
    'use strict';

    angular.module("app")
        .service('Helpers', function() {
            this.safeRemove = function(array, element) {
                var index = array.indexOf(element);
                if(index > -1){
                    array.splice(index, 1);
                }
            }
        });
})();