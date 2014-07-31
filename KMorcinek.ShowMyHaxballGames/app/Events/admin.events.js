angular.module('admin.events', ['ngRoute', 'ngResource', 'ngMessages'])
    .config(function ($routeProvider) {
        $routeProvider
            .when('/admin/events', {
                controller: 'EventsListCtrl',
                templateUrl: '/app/Events/events-list.html',
                resolve: {
                    events: function (Events) {
                        return Events.query().$promise;
                    }
                },
            })
            .when('/admin/events/new', {
                controller: 'EventsNewCtrl',
                templateUrl: '/app/Events/events-details.html',
            })
            .when('/admin/events/:id', {
                controller: 'EventsEditCtrl',
                templateUrl: '/app/Events/events-details.html',
                resolve: {
                    event: function ($q, $route, Events) {
                        return Events.get({ id: $route.current.params.id }).$promise;
                    }
                },
            })
    });