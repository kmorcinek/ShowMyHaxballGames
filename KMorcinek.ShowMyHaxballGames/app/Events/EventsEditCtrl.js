(function () {
    'use strict';
        angular.module('admin.events').controller('EventsEditCtrl',
        function ($scope, event, Events, ValidationHandler, $location) {
            $scope.event = event;

            $scope.save = function () {
                ValidationHandler($scope, function () {
                    Events.save({}, $scope.event, function () {
                        $location.path('/admin/events');
                    });
                });
            };
        });
})();