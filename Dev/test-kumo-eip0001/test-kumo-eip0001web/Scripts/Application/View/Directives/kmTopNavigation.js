angular.module("directivesModule")
.directive('kmTopNavigation',
function ($state,
        $timeout,
        $rootScope,
        navigationService,
        accountService) {
    var ibTopNavigationController = function ($scope) {

        $scope.navshowitems = [];
        $scope.navitems = [];
        $scope.isAdminPerm = false;

        $scope.isShowManageUser = function (item) {
            if(item.state === "usermanagement")
                return $scope.isAdminPerm;
            return true;
        }
        
        accountService.isAdmin(function (result) {
            $scope.isAdminPerm = result;
        });

        var isExisted = function (navItem, name) {
            if (navItem.childNodes && navItem.childNodes.length > 0) {
                return navItem.childNodes.hasElementWith("route", name) ||
                    navItem.childNodes.hasElementWith("parentState", name);
            }
            return false;
        }

        var setTabActive = function (navitems, navshowitems, name) {
            navitems.forEach(function (item) {
                if (name.indexOf(item.route) > -1 || isExisted(item, name)) {
                    item.isActive = true;
                }
                else {
                    item.isActive = false;
                }
            });

            var activeItem = navitems.getElementsWith("isActive", true);

            if (activeItem && activeItem.length > 0) {
                navshowitems.forEach(function (showItem) {
                    if (showItem.state === activeItem[0].state) {
                        showItem.isActive = true;
                    }
                    else {
                        showItem.isActive = false;
                    }
                });
            }
        }

        var resetTopNavigation = function () {
            var isShowInTop = true;
            var showItems = navigationService.getTopnavigationNodes(isShowInTop);
            angular.copy(showItems, $scope.navshowitems);

            var allItems = navigationService.getTopnavigationNodes(!isShowInTop);
            angular.copy(allItems, $scope.navitems);

            var pathName = window.location.pathname;
            setTabActive($scope.navitems, $scope.navshowitems, pathName);
        }

        resetTopNavigation();
    }

    return {
        restrict: "E",
        replace: true,
        scope: {},
        templateUrl: '/Scripts/Application/View/Directives/kmTopNavigation.html',
        link: ibTopNavigationController
    };
});