var ns = sfe.ns("sfe.Constants");

ns.errorTypes = function () {
	var permissionError = "permissionError";
	var queryError = "queryError";
	var saveError = "saveError";
	var other = "Other";

	return {
		PermissionError: permissionError,
		QueryError: queryError,
		SaveError: saveError,
		Other: other
	};
}();