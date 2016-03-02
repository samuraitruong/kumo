angular.module("serviceModule")
.factory("accountService", [
    '$http', 'ajaxService', function ($http, ajaxService) {

        //Create new record
        var post = function (account, callback) {
            ajaxService.ajaxImpl('POST', 'AccountsAPI/', account, callback);
        }

        //Get Single Records
        var get = function (id, callback) {
            ajaxService.ajaxImpl('GET', 'AccountsAPI/' + id, null, callback);
        }

        //Get All Accounts
        var getAccounts = function (callback) {
            ajaxService.ajaxImpl('GET', 'AccountsAPI', null, callback);
        }


        //Update the Record
        var put = function (id, account, callback) {
            ajaxService.ajaxImpl('PUT', 'AccountsAPI/' + id, account, callback);
        }

        //Delete the Record
        var remove = function (id, callback) {
            ajaxService.ajaxImpl('DELETE', 'AccountsAPI/' + id, null, callback);
        }

        var isAdmin = function (callback) {
            ajaxService.ajaxImpl('POST', 'UserAPI/', null, callback);
        }

        return {
            post: post,
            get: get,
            getAccounts: getAccounts,
            put: put,
            remove: remove,
            isAdmin: isAdmin
        }
    }]);