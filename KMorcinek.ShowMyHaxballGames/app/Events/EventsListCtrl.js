(function() {
    'use strict';
        angular.module('admin.events').controller('EventsListCtrl',
        function ($scope, events, Helpers, Events) {
            $scope.events = events;
            $scope.remove = remove;

            function remove(event) {
                if (!confirm('Are you sure?'))
                    return;

                Helpers.safeRemove($scope.events, event);
                Events.remove({ id: event.id }, function() {
                    console.log('delete');
                });
            }
        });
})();