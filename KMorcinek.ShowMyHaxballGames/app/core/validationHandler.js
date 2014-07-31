angular.module('app')
    .factory('ValidationHandler', function () {
        return function (scope, saveCallback) {
            if (scope.form.$invalid) {
                scope.didTrySubmit = true;
                return;
            }

            saveCallback();
        }
    });

