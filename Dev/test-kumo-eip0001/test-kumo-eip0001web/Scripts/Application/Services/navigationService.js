angular.module("serviceModule")
.factory("navigationService", function ($rootScope, $state) {
    var getTopnavigationNodes = function (isInTopNav) {
        var retval = [];
        var states = sfe.routes.routeConfiguration;

        states.forEach(function (state) {
            if (state.route !== undefined &&
                (isInTopNav == undefined || !isInTopNav || (isInTopNav && state.inTopnav))) {
                state.childNodes = getChildNodes(states, state.state, isInTopNav);
                if (state.parentState == undefined) {
                    retval.push(state);
                }
            }
        });

        return retval;
    };

    var getChildNodes = function (states, parentStateName, isInTopNav) {
        var retval = [];

        states.forEach(function (state) {
            if (state.route !== undefined && state.parentState === parentStateName &&
                (isInTopNav == undefined || !isInTopNav || (isInTopNav && state.inTopnav))) {
                retval.push(state);
            }
        });

        return retval;
    }

    return {
        getTopnavigationNodes: getTopnavigationNodes,
    }
});