app.controller("LoginController",
["$scope",
function ($scope) {
	
	//$scope.redirectTo = "aaa";
	$scope.aaa = "oooo";
	$scope.redirectTo = {name:"Registry", url:"/registry"};

	$scope.login = function (data) {
		alert("username: " + data.username);
	};
}

]);