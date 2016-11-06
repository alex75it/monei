app.controller("LoginController",
["$scope", "$location",
function ($scope, $location) {

	$scope.redirectTo = { name: "Home", url: "home" }

	var redirectToValue = monei.utils.QueryString["redirectTo"];	
	if (redirectToValue)
	{
		$scope.redirectTo = { name: redirectToValue, url: redirectToValue };
	}

	$scope.login = function () {
		//alert("login ok");
	};
}

]);