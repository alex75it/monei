app.factory("subcategoryDataProvider",
["$http",
function($http) {

	var baseUrl = "/api/subcategory/";

	var provider = {};

	provider.getSubcategories = function (category, callback, errorCallback, finallyCallback) {
		$http.get(baseUrl + "category/" + category)
			.success(function(data, status) { callback(data) })
			.error(function(error, status) {errorCallback && errorCallback(error)})
			.finally(function() { finallyCallback && finallyCallback() })
		;
	};

	return provider;

}]);