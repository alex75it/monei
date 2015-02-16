app.directive("moneiRegistryCreatePanel",
["CategoryProvider", function (CategoryProvider) {

	var directive = {};
	directive.restrict = "E";
	directive.replace = true;
	directive.templateUrl = "/Scripts for monei/angular templates/RegistryCreatePanel.html";

	directive.link = function ($scope, $element, $attrs) {
		$scope.date = moment().toDate();
		$scope.amount = 0;

	};

	return directive;

}]);