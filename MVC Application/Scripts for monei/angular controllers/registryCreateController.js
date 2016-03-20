var _scope;
app.controller("RegistryCreateController", [
	"$scope", "$element", "CategoryDataProvider",
	function ($scope, $element, CategoryDataProvider) {
		_scope = $scope;
		$scope.date = moment();
		$scope.category;
		$scope.categories = [];
		$scope.error = null;


		$scope.initialize = function () {			
		    CategoryDataProvider.getCategories(
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