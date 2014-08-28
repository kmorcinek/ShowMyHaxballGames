(function() {
    'use strict';
        angular.module('app', ['ngRoute', 'admin.events'])
        .config(function ($routeProvider, $locationProvider) {
            $routeProvider
                .when('/admin/configuration', {
                    controller: 'ConfigurationCtrl',
                    templateUrl: '/app/Configuration/configuration-details.html',
                    resolve: {
                        configuration: function (Configurations) {
                            return Configurations.get({ id: 1 }).$promise;
                        }
                    },
                })
                .otherwise({
                    redirectTo: '/admin/events/'
                });

            $locationProvider.html5Mode(true);
        });
})();