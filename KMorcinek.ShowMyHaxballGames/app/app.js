angular.module('app', ['ngRoute', 'admin.events'])
    .config(function ($routeProvider, $locationProvider) {
        //$routeProvider
        //    .otherwise({
        //        redirectTo: '/admin/events/new'
        //    });

        $locationProvider.html5Mode(true);
    });