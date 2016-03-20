app.factory("CategoryDataProvider",
["$http",
function categoryDataProvider($http) {
	var baseUrl = "/api/category/";
	var provider = {};

	provider.getCategories = function(callback, errorCallback, finallyCallback) {
		$http.get(baseUrl + "list")
			.success(function(data) { callback(data); })
			.error(function(data, status) { errorCallback && errorCallback(data); })
			.finally(function() { finallyCallback && finallyCallback(); });
	};

	return provider;
}]);