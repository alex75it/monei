app.directive("moneiLoginPanel", ["AccountProvider", "$window", function (AccountProvider, $window) {

	var directive = {};

	directive.restrict = "E";
	directive.replace = true;
	directive.templateUrl = "/Scripts for monei/Angular/directive-templates/login-panel.html"

	directive.scope = {
		//redirectUrl: "=redirectTo",
		onLoggedIn: "&",
		onLoginFail: "&"
	};

	directive.link = function ($scope, $element, $attrs) {
		$scope.username = null;
		$scope.password = null;
		$scope.rememberMe = false;
		$scope.errors = {};

		$element.find("#username").focus();

		$scope.login = function () {
			
			if (!$attrs.redirectToUrl && !onLoggedIn)
				showError("No action after login?");

			$scope.errors = {};
			var data = { username: $scope.username, password: $scope.password, rememberMe: $scope.rememberMe };	

			// reverse order for focus
			if (!data.password) { $scope.errors.passwordError = "Password needed"; $element.find("#password").focus(); };
			if (!data.username) { $scope.errors.usernameError = "Username needed"; $element.find("#username").focus(); };		
			

			if (!$scope.errors.usernameError && !$scope.errors.passwordError) {
				$scope.isLoading = true;
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
								$scope.onLoginFail && $scope.onLoginFail({ cause: "Username not found" });
								//$scope.showError("Username not found");
								$scope.errors.usernameError = "Username not found";
								$element.find("#username").focus();
								break;
							case AccountProvider.loginResults.WrongPassword:
								$scope.onLoginFail && $scope.onLoginFail({ cause: "Wrong password" });
								//$scope.showError("Wrong password");
								$scope.errors.passwordError = "Wrong password";
								$element.find("#password").focus();
								break;

							default:
								$scope.showError(new Error("Unknown login result: " + result));
								break;
						}

						if (loggedIn) {
							console.log("login: OK");
							$scope.onLoggedIn && $scope.onLoggedIn();
							//if ($attrs.redirectTo) alert("go to: " + $attrs.redirectTo);	
							$element.hide();
							if ($attrs.redirectToUrl)
								$window.location.href = "/" + $attrs.redirectToUrl;
						}
					},
					/*error*/
					$scope.showError,
					/*finally*/
					function () { $scope.isLoading = false; }
					);

			}
		};

		$scope.showError = function (error, status) {
			$scope.error = error.exceptionMessage || error.message || error;
		};

	};

	return directive;

}]);