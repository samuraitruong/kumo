angular.module("serviceModule").factory("notificationService", [
"toaster", function (toaster) {
    var success = function (title, text, timeout) {
        pop("success", title, text, timeout);
    };

    var error = function (title, text) {
        pop("error", title, text);
    };

    var warning = function (title, text) {
        pop("warning", title, text);
    };

    var note = function (title, text) {
        pop("note", title, text);
    };

    var clear = function () {
        toaster.clear();
    };

    var pop = function (type, title, text, timeout) {
        toaster.pop(type, title, text, timeout, "trustedHtml");
    };

    return {
        success: success,
        error: error,
        note: note,
        warning: warning,
        clear: clear,
    };
}
]);