var _scope;
app.controller("registryCreateController", [
	"$scope", "$element", "CategoryProvider",
	function ($scope, $element, CategoryProvider) {
		_scope = $scope;
		$scope.date = moment();
		$scope.categories = [];
		$scope.error = null;


		$scope.initialize = function () {
			CategoryProvider.getCategories(
				/*success*/
				function (data) { alert(data.length); $scope.categories = data; },
				/*error*/
				function (data) { $scope.showError(data);}
				// todo
				);
		};

		$scope.showError = function(error)
		{
			$scope.error = error; //.message
		};

		$scope.initialize();

	}
]);