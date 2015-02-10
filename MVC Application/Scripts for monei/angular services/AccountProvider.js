app.service("AccountProvider",
	["$http",
	function ($http) {
		return {
			login: function (data, callback, errorCallback, finallyCallback) {
				$http.post("/api/account/login", data)
					.success(function (data) { callback(data); })
					.error(function (data, status) { errorCallback && errorCallback(data, status); })
					.finally(function () { finallyCallback && finallyCallback(); })
				;
			}
		};
	}
	]);