(function() {
    'use strict';
        angular.module('app')
        .factory('Configurations', function ($resource) {
            return $resource('/api/configurations/:id', { id: '@id' });
        });
})();