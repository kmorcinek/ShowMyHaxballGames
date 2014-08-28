(function() {
    'use strict';
    angular.module('app').controller('ConfigurationCtrl',
        function($scope, configuration, Configurations) {
            $scope.configuration = configuration;

            $scope.save = function() {
                Configurations.save({}, $scope.configuration, function() {
                    alert('submitted');
                });
            };
        });
})();