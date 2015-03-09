app.directive("moneiSubcategorySelector",
["subcategoryDataProvider",
function (subcategoryDataProvider) {

	var directive = {
		restrict: "E",
		templateUrl: "/Scripts for monei/directive-templates/SubcategorySelector.html",
		replace: true,

	};

	directive.scope = {
		categories: "=categories"
	};

	directive.link = function(scope, element, attrs) {


		element.find("select").multiselect({
			templates: {
				button: '<button type="button" class="multiselect dropdown-toggle form-control" data-toggle="dropdown"></button>'
			},
			buttonClass: 'btn btn-default',
			buttonWidth: 'auto',
			buttonContainer: '<div class="btn-group bootstrap-select" />',
			maxHeight: false,
			buttonText: function (options) {
				if (options.length == 0) {
					return '<span class="pull-left">(all)</span> <span class="caret"></span>';
				} else if (options.length > 1) {
					return '<span class="pull-left">' + options.length + ' selected</span> <span class="caret"></span>';
				} else {
					var selected = '';
					options.each(function () {
						selected += $(this).text() + ', ';
					});
					return '<span class="pull-left">' + selected.substr(0, selected.length - 2) + '</span> <span class="caret"></span>';
				}
			}
		});

		scope.$watch(
			function () { return scope.categories; },
			function () {
				//console.log("categories: " + scope.categories);
				scope.subcategories = [];
				scope.loading = true;
				if (scope.categories && scope.categories.length == 1) {
					var category = scope.categories[0];
					subcategoryDataProvider.getSubcategories(category,
						function(data) {
							scope.subcategories = data;
							setTimeout(function () {
								element.find("select").multiselect('rebuild');
							}, 0.5 * 1000); // little delay is needed
						},
						function(error) { alert(error); },
						function() {scope.loading = false;}
					);
				}
			}
		);

	};

	return directive;

}]);