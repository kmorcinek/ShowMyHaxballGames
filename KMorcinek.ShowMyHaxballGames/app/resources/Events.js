(function() {
    'use strict';
        angular.module('app')
        .factory('Events', function ($resource) {
            return $resource('/api/events/:id', { id: '@id' });
        });
})();