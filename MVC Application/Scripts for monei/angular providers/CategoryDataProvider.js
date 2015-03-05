app.factory("categoryDataProvider",
["$http",
function categoryDataProvider($http) {
	var provider = {};

	provider.getCategories = function(callback, errorCallback, finallyCallback) {
		$http.get("/api/category/list")
			.success(function(data) { callback(data); })
			.error(function(data, status) { errorCallback && errorCallback(data); })
			.finally(function() { finallyCallback && finallyCallback(); });
	};

	return provider;
}]);