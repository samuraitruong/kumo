'use strict';

angular.module('KumoApp.directives', []).
  directive('appVersion', ['version', function (version)
  {
      return function (scope, elm, attrs)
      {
          elm.text(version);
      };
  }]);