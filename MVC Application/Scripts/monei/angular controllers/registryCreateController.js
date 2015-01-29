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
				function (data) { $scope.showError(data, "Fail to get Categories.");}
				);
		};

		$scope.showError = function(error, context)
		{

			//$scope.error = error.exceptionMessage || error.message || error; //.message
			$scope.error = context + " " + error.exceptionMessage || error.message || error; //.message
		};

		$scope.initialize();

	}
]);