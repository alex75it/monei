app.service("RegistryProvider",
["$http",
function($http) {
	return {
	search: function(filters, callback, errorCallback, finallyCallback) {
		$http.post("/api/registry/search", filters)
			.success(callback)
			.error(function(error, status) {errorCallback && errorCallback(error) })
			.finally(function() { finallyCallback && finallyCallback() });

	}
}
}]);