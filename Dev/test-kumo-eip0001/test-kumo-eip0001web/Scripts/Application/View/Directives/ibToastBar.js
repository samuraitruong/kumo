'use strict';

angular.module('directivesModule')
.service('toastBar', [
    '$rootScope', function ($rootScope) {
        this.pop = function (type, body, clickHandler) {
            this.toast = {
                type: type,
                body: body,
                clickHandler: clickHandler
            };
            $rootScope.$broadcast('toastbar-newToast');
        };

        this.clear = function () {
            $rootScope.$broadcast('toastbar-clearToasts');
        };
    }
])
.constant('toastBarConfig', {
    'icon-classes': {
        error: 'icon-close',
        info: 'icon-infosearch',
        warning: 'icon-warning',
        redWarning: 'icon-warning'
    },
    'icon-class': 'icon-warning',
    'label-classes': {
        error: 'label-danger',
        info: 'label-info',
        warning: 'label-warning',
        redWarning: 'label-danger'
    },
    'label-class': 'label-warning',
})
.directive('ibToastBar', [
    'toastBarConfig', 'toastBar',
    function ( toastBarConfig, toastBar) {
        return {
            replace: true,
            restrict: 'EA',
            scope: true,
            link: function (scope, elm, attrs) {

                var id = 0,
                    mergedConfig;

                mergedConfig = angular.extend({}, toastBarConfig, scope.$eval(attrs.toasterOptions));


                function addToast(toast) {

                    var shouldAdded = true;
                    scope.toastbars.forEach(function (entry) {
                        if (entry.body == toast.body) {
                            shouldAdded = false;
                        }
                        return;
                    });
                    if (!shouldAdded) return;

                    toast.icontype = mergedConfig['icon-classes'][toast.type];
                    if (!toast.icontype)
                        toast.icontype = mergedConfig['icon-class'];

                    toast.labeltype = mergedConfig['label-classes'][toast.type];
                    if (!toast.labeltype)
                        toast.labeltype = mergedConfig['label-class'];

                    id++;
                    angular.extend(toast, { id: id });

                    scope.toastbars.push(toast);
                }

                scope.toastbars = [];
                scope.$on('toastbar-newToast', function () {
                    addToast(toastBar.toast);
                });


                scope.$on('toastbar-clearToasts', function () {
                    scope.toastbars.splice(0, scope.toastbars.length);
                });

            },
            template:
                '<div id="toastbar-container" class="ib-toastbar-container"><div ng-repeat="bar in toastbars track by $index" class="label {{bar.labeltype}} ib-toastbar" id="toastbar-{{bar.id}}" ><a href="javascript:void(0)"><i class="{{bar.icontype}}"></i></a> <a title="{{bar.body}}" >{{bar.body}}</a></div></div>'
        };
    }
]);