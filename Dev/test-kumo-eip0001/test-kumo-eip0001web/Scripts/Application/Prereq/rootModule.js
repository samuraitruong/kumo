var ns = sfe.ns("sfe.Modules");
angular.module("serviceModule", []);
angular.module("controllerModule", []);
angular.module("directivesModule", []);
angular.module("filtersModule", []);

ns.rootModule = angular.module("rootModule",
[
    "ui.bootstrap",
    "ui.router",
    "ngRoute",
    "serviceModule",
    "controllerModule",
    "directivesModule",
    "toaster",
]);
