app.controller("LoginController",
["$scope",
function ($scope) {
	
	$scope.redirectTo = {name:"Registry", url:"/registry"};

	$scope.login = function (data) {
		alert("username: " + data.username);
	};
}

]);