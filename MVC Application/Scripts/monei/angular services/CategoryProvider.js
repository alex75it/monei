app.service("CategoryProvider",
	["$http",
	function ($http) {
		return {
			getCategories: function(callback, errorCallback, finallyCallback) {
				$http.get("/api/category")
					.success(function () { callback(data); })
					.error(function (data, status) { errorCallback && errorCallback(data); })
					.finally(function (data, status) { finallyCallback && finallyCallback(data); })
				;
			}
		};
	}
]);