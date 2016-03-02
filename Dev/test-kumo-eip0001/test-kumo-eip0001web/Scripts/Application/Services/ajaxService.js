angular.module('serviceModule')
.factory('ajaxService',  function ($http, $q, $filter, errorService) {
    var dateTimeRegex = /^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}[\.\d{3}]*$/;
    var serviceRoot = window.location.protocol + '//' + window.location.host + '/';

    var ajaxImpl = function (method, serviceUrl, params, callback, errorCallback) {
        var requestInfo = {
            method: method,
            url: serviceRoot + '_api/' + serviceUrl,
            data: params,
        };

        var request = $http(requestInfo);

        request.then(handleSuccess, handleError);

        function handleSuccess(response) {
            if (angular.isArray(response.data)) {
                response.data.forEach(function (obj) {
                    if (typeof obj !== "string") {
                        Object.getOwnPropertyNames(obj).forEach(function (key) {
                            if (typeof obj[key] === 'string' && dateTimeRegex.test(obj[key])) {
                                //console.log("obj[key]", obj[key]);
                                obj[key] = parseDateFromServer(obj[key]);
                                //console.log("obj[key]1", obj[key]);
                            }
                        });
                    }
                });
            }

            callback(response.data);
        }

        function handleError(response) {
            var msg = '';
            if (!angular.isObject(response.data) ||
                (!response.data.message && !response.data.Message && !response.data.ExceptionMessage && !response.data.exceptionMessage)) {

                if (response.status === 500) {
                    msg = response.data;
                } else {
                    msg = {};
                }
            } else {
                msg = response.data.ExceptionMessage || response.data.exceptionMessage || response.data.Message || response.data.message;
            }

            errorService.raiseErrorNotification(msg);

            if (typeof errorCallback == 'function') {
                errorCallback(msg);
            }
        }

        function parseDateFromServer(src) {
            //console.log("original", src);
            var val = src + 'Z';
            var date = new Date(Date.parse(val));
            if (date == 'Invalid Date') {
                //console.log("Invalid Date", src);
                date = parseDate(src);
            }

            return date;
        }

        function parseDate(input) {
            try {
                var parts = input.split('T');
                var dateParts = parts[0].split('-');
                var timeParts = parts[1].split(':');
                // new Date(year, month [, day [, hours[, minutes[, seconds[, ms]]]]])
                var utcDate = new Date(dateParts[0], dateParts[1] - 1, dateParts[2], timeParts[0], timeParts[1], timeParts[2]); // Note: months are 0-based
                var currentDate = new Date();
                var offset = -(currentDate.getTimezoneOffset() / 60);

                var hours = utcDate.getHours();
                hours += offset;
                utcDate.setHours(hours);

                return utcDate;
            } catch (ex) {
                return input;
            }

        }
    }

    return {
        ajaxImpl: ajaxImpl,
    }

});