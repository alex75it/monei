app.directive("moneiRegistryCreatePanel",
["CategoryDataProvider", function (CategoryDataProvider) {

	var directive = {};
	directive.restrict = "E";
	directive.replace = true;
	directive.templateUrl = "/Scripts for monei/directive-templates/RegistryCreatePanel.html";

	directive.link = function ($scope, $element, $attrs) {
		$scope.date = moment().toDate();
		$scope.amount = 0;
	};

	return directive;
}]);
