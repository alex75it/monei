var _moneiLoginPanel_scope;
app.directive("moneiLoginPanel", ["AccountProvider", "$window", function (AccountProvider, $window) {

	var directive = {};

	directive.restrict = "E";
	directive.replace = true;
	directive.templateUrl = "/Scripts for monei/directive-templates/login-panel.html"

	directive.scope = {
		//redirectUrl: "=redirectTo",
		onLoggedin: "&",
	};

	directive.link = function ($scope, $element, $attrs) {
		_moneiLoginPanel_scope = $scope;
		$scope.username = null;
		$scope.password = null;
		$scope.rememberMe = false;
		$scope.errors = {};

		$scope.login = function () {
			$scope.errors = {};
			//var data = { username: $scope.username, password: $scope.password, rememberMe: $scope.rememberMe };
			var data = { username: $scope.username, password: $scope.password };
			//$scope.data = data;			

			if (!data.username) $scope.errors.usernameError = "Username needed";
			if (!data.password) $scope.errors.passwordError = "Password needed";
			

			if (!$scope.errors.usernameError && !$scope.errors.passwordError) {

				//alert("login API");
				AccountProvider.login(data, 
					/*success*/
					function (result) {
						alert("login...result: " + result);
						$scope.login_result = result;

						$scope.onLoggedin && $scope.onLoggedin(data);

						if ($attrs.redirectto)
							$window.location.href = $attrs.redirectto;
						//alert("go to: " + $attrs.redirectto);
					},
					/*error*/
					$scope.showError
					);

			}
		};

		$scope.showError = function (error, status) {
			$scope.error = error.exceptionMessage || error.message || error;
		};

	};

	return directive;

}]);