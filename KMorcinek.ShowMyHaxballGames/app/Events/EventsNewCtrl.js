angular.module('admin.events').controller('EventsNewCtrl',
    function ($scope, Events, ValidationHandler, $location) {
        $scope.event = {
            isFromHaxball: true,
        };

        $scope.isNewEntry = true;

        $scope.$watch('event.isFromHaxball', function (newValue) {
            if (newValue) {
                $scope.event.status = undefined;
                $scope.event.url = undefined;
            } else {
                $scope.event.leagueNumber = undefined;
                $scope.event.title = undefined;
            }
        });

        $scope.save = function () {
            ValidationHandler($scope, function () {
                Events.save({}, $scope.event, function () {
                    $location.path('/admin/events');
                });
            });
        };
    });