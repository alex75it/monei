app.factory("SubcategoryDataProvider",
["$http",
function($http) {

	var baseUrl = "/api/subcategory/";

	var provider = {};

	provider.getSubcategories = function (categoryId, callback, errorCallback, finallyCallback) {
		$http.get(baseUrl + "category/" + categoryId)
			.success(function(data, status) { callback(data) })
			.error(function(error, status) {errorCallback && errorCallback(error)})
			.finally(function() { finallyCallback && finallyCallback() })
		;
	};

	return provider;

}]);