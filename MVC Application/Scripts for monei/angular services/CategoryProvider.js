app.service("CategoryProvider",
	["$http",
	function ($http) {
		return {
			getCategories: function (callback, errorCallback, finallyCallback) {
				$http.get("/api/category/list")
					.success(function (data) { callback(data); })
					.error(function (data, status) { errorCallback && errorCallback(data); })
					.finally(function () { finallyCallback && finallyCallback(); })
				;
			}
		};
	}
]);