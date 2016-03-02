angular.module("controllerModule")
.controller("accountsController",
function ($scope, $log, accountService, notificationService) {

    $scope.IsNewRecord = 1;

    loadRecords();

    function loadRecords() {
        accountService.getAccounts(function(result){
            $scope.Accounts = result;
        });
    }

    $scope.delete = function (id) {
        if (id) {
            notificationService.note('In progress', 'Account is deleting');
            accountService.remove(id, function () {
                notificationService.clear();
                notificationService.success('Success', 'Account is deleted');
                loadRecords();
            });
        }
    }
});