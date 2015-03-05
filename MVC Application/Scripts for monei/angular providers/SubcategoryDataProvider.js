app.factory("subcategoryDataProvider",
["$http",
function($http) {

	return {
		getSubcategories: function(callback, erroCallback) {
			var data = [{id:1, name:"Test"}];
			callback(data);
		}

	};

}]);