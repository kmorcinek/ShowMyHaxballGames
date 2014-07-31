angular.module('app')
    .factory('Events', function ($resource, $http) {
        return $resource('/api/events/:id', { id: '@id' });
    });