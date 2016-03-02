(function () {
    'use strict';
    angular.module("serviceModule").factory("errorService", errorService);

    errorService.$inject = ['$state', '$window', 'notificationService'];

    function errorService($state, $window, notificationService, localStorageService) {
        var service = {
            raiseErrorNotification: raiseErrorNotification,
        };

        return service;

        function raiseErrorNotification(msg) {
            notificationService.error("Error", getErrorMessage(msg));
        };

        function getErrorMessage(msg) {
            if (typeof msg === "object") {
                return msg.Message;
            } else {
                return msg;
            }
        }
    }
})();