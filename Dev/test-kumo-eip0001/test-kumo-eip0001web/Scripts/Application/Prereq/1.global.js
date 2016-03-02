var sfe = sfe || {};

// Fix 'console is undefined' error in IE9 and below
if (!(window.console && console.log)) {
    console = {
        log: function () { },
        debug: function () { },
        info: function () { },
        warn: function () { },
        error: function () { }
    };
}

sfe.ns = function (namespace) {
    var parts = namespace.split('.'),
               parent = sfe,
               i,
               max;

    if (parts[0] === "sfe") {
        parts = parts.slice(1);
    }
    for (i = 0, max = parts.length; i < max; i++) {
        if (typeof parent[parts[i]] === "undefined") {
            parent[parts[i]] = {};
        }
        parent = parent[parts[i]];
    }
    return parent;
};

sfe.isNull = function (obj) {
    //0 and false are not null
    if ((obj === 0) || (obj === false)) {
        return false;
    }
    return (!obj || typeof obj === 'undefined' || obj === null);
};
sfe.isNullOrEmpty = function (obj) {
    return sfe.isNull(obj) || obj === '';
};
sfe.isFunction = function (obj) {
    return !sfe.isNullOrEmpty(obj) && typeof obj === "function";
}
sfe.safeApply = function ($scope, callback) {
    if (sfe.isNull($scope) || sfe.isNull(callback))
        return;

    var phase = $scope.$root.$$phase;
    if (phase == '$apply' || phase == '$digest') {
        if (callback && (typeof (callback) === 'function')) {
            callback();
        }
    } else {
        $scope.$apply(callback);
    }
}

sfe.getPropertyRecursive = function (obj, propNames) {
    if (propNames.length === 1)
        return obj[propNames[0]];
    else {
        var propName = propNames.splice(0, 1)[0];
        return sfe.getPropertyRecursive(obj[propName], propNames);
    }
}

Array.prototype.getElementsWith = function (propertyName, value) {
    return this.filter(function (item) {
        var propChain = propertyName.split('.');

        return sfe.getPropertyRecursive(item, propChain) === value;
    });
};

Array.prototype.getElementWith = function (propertyName, value) {
    var arr = this.getElementsWith(propertyName, value);

    return arr[0];
};

Array.prototype.hasElementWith = function (propertyName, value) {
    return this.filter(function (item) {
        var propChain = propertyName.split('.');

        return sfe.getPropertyRecursive(item, propChain) === value;
    }).length > 0;
};

Array.prototype.hasTextElement = function (value) {
    return this.filter(function (item) {
        return item === value;
    }).length > 0;
}

Array.prototype.hasAtLeastElementContains = function (propertyName, value, limit) {
    return this.filter(function (item) {
        return item[propertyName].toLowerCase().indexOf(value.toLowerCase()) > -1;
    }).length >= limit;
};

Array.prototype.removeElement = function (element) {
    this.splice(this.indexOf(element), 1);
};

Array.prototype.removeElementWith = function (propertyName, value) {
    var index = -1;
    for (var i = 0; i < this.length; i++) {
        if (this[i][propertyName] === value) {
            index = i;
            break;
        }
    }

    if (index > -1) {
        this.splice(index, 1);
    }
};
Array.prototype.indexOfObject = function (propertyName, value) {
    for (var i = 0; i < this.length; i++) {
        if (this[i][propertyName] === value) {
            return i;
        }
    }
    return -1;
};