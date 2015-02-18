﻿app.directive("moneiCategorySelector",
["utils", "category",
function (utils, categoryProvider) {

	var directive = {};
	directive.restrict = "E";
	directive.replace = true;
	directive.templateUrl = "/Scripts for monei/directive-templates/CategorySelector.html";

	directive.scope = {
		

	};

	directive.link = function(scope, element, attrs) {
		scope.loading = true;

		categoryProvider.getCategories(function (data) { scope.categories = data; },
			function(error) { throw Error("Fail to load categories. " + error); },
			function() { scope.loading = false; }
		);

	};

	return directive;
}]);