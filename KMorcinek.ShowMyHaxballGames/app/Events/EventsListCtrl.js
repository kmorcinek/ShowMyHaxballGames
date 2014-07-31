angular.module('admin.events').controller('EventsListCtrl',
    function ($scope, events) {
        $scope.events = events;
    });