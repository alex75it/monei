app.factory("subcategoryDataProvider",
["$http",
function($http) {

	var baseUrl = "/api/subcategory/";

	var provider = {};

	provider.getSubcategories = function (callback, errorCallback, finallyCallback) {
		$http.get(baseUrl)
			.success(function(data, status) { callback(data) })
			.error(function(error, status) {errorCallback && errorCallback(error)})
			.finally(function() { finallyCallback && finallyCallback() })
		;
	};

	return provider;

}]);