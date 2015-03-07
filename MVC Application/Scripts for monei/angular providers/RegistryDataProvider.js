app.factory("registryDataProvider",
["$http",
function ($http) {
	var baseUrl = "/api/registry/";
	var provider = {};

	provider.search = function(filters, callback, errorCallback, finallyCallback) {
		$http.post(baseUrl + "search", filters)
			.success(callback)
			.error(function(error, status) { errorCallback && errorCallback(error) })
			.finally(function() { finallyCallback && finallyCallback() });
	};
	
	return provider;
}]);