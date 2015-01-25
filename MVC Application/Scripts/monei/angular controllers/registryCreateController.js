var _scope;
app.controller("registryCreateController", [
	"$scope", "$element", "CategoryProvider",
	function ($scope, $element, CategoryProvider) {
		_scope = $scope;
		$scope.date = moment();
		$scope.category;
		$scope.categories = [];
		$scope.error = null;


		$scope.initialize = function () {			
			CategoryProvider.getCategories(
				/*success*/
				function (data) {  $scope.categories = data; },
				/*error*/
				function (data) { $scope.showError(data);}
				);
		};

		$scope.showError = function(error)
		{
			$scope.error = error.exceptionMessage || error.message || error; //.message
		};

		$scope.initialize();

	}
]);