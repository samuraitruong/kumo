'use strict';

angular.module('KumoApp.filters', []).
  filter('interpolate', ['version', function (version)
  {
      return function (text)
      {
          return String(text).replace(/\%VERSION\%/mg, version);
      };
  }]);