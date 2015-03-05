app.directive("moneiSubcategorySelector",
["subcategoryDataProvider",
function (subcategoryDataProvider) {

	var directive = {
		restrict: "E",
		templateUrl: "/Scripts for monei/directive-templates/SubcategorySelector.html",
		replace: true
	};

	directive.link = function(scope, element, attrs) {

		subcategoryDataProvider.getSubcategories(
			/*success*/
			function (data) {
				alert(13);
				scope.subcategories = data;
			}
		);

	};

	return directive;

}]);