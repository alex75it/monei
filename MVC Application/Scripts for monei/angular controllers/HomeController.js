﻿app.controller("HomeController",
["$scope",
function ($scope) {
	
	$scope.loginFailCounter = 0;

	$scope.loggedIn = function () {
		console.info("loggedin");
	};

	$scope.loginFail = function (cause) {
		console.info("loginFail for " + cause);
		$scope.loginFailCounter += 1;
		if ($scope.loginFailCounter == 10)
		{
			$scope.loginFailCounter = 0;
		}

	};
}

]);