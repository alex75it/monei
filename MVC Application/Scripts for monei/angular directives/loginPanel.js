var _moneiLoginPanel_scope;
app.directive("moneiLoginPanel", ["AccountProvider", "$window", function (AccountProvider, $window) {

	var directive = {};

	directive.restrict = "E";
	directive.replace = true;
	directive.templateUrl = "/Scripts for monei/directive-templates/login-panel.html"

	directive.scope = {
		//redirectUrl: "=redirectTo",
		onLoggedIn: "&",
		onLoggedin: "&",
		onLoginFail: "&",
	};

	directive.link = function ($scope, $element, $attrs) {
		_moneiLoginPanel_scope = $scope;
		$scope.username = null;
		$scope.password = null;
		$scope.rememberMe = false;
		$scope.errors = {};

		$scope.login = function () {
			$scope.errors = {};
			var data = { username: $scope.username, password: $scope.password, rememberMe: $scope.rememberMe };	

			// reverse order for focus
			if (!data.password) { $scope.errors.passwordError = "Password needed"; $element.find("#password").focus(); };
			if (!data.username) { $scope.errors.usernameError = "Username needed"; $element.find("#username").focus(); };		
			

			if (!$scope.errors.usernameError && !$scope.errors.passwordError) {

				AccountProvider.login(data, 
					/*success*/
					function (result) {	
						var loggedIn = false;
						switch (result)
						{
							case AccountProvider.loginResults.Ok:
								loggedIn = true;
								break;
							case AccountProvider.loginResults.UsernameNotFound:
								$scope.loginFail && $scope.loginFail("Username not found");
								//$scope.showError("Username not found");
								$scope.errors.usernameError = "Username not found";
								$element.find("#username").focus();
								break;
							case AccountProvider.loginResults.WrongPassword:
								$scope.loginFail && $scope.loginFail("Wrong password");
								//$scope.showError("Wrong password");
								$scope.errors.passwordError = "Wrong password";
								$element.find("#password").focus();
								break;

							default:
								$scope.showError(new Error("Unknown login result: " + result));
								break;
						}

						if (loggedIn) {
							$scope.onLoggedin && $scope.onLoggedin(data);
							//if ($attrs.redirectto) alert("go to: " + $attrs.redirectto);	
							if ($attrs.redirectto)
								$window.location.href = $attrs.redirectto;
						}
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