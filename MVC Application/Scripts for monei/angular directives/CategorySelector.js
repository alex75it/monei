app.directive("moneiCategorySelector",
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
		//var multipleAttr = attrs["multiple"];

		element.multiselect({
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
				}
				else if (options.length > 1) {
					return '<span class="pull-left">' + options.length + ' selected</span> <span class="caret"></span>';
				}
				else {
					var selected = '';
					options.each(function () {
						selected += $(this).text() + ', ';
					});
					return '<span class="pull-left">' + selected.substr(0, selected.length - 2) + '</span> <span class="caret"></span>';
				}
			}
		});

		
		//if (multipleAttr && (multipleAttr == "true" || multipleAttr == "1"))
		//	element.attr("multiple", "multiple");
		//else
		//	element.removeAttr("multiple");


		// Watch for any changes to the length of our select element
		/*
		scope.$watch(function () {
			return element[0].length;
		}, function () {
			console.log("Watch");
			element.multiselect('rebuild');
		});
		*/

		// Watch for any changes from outside the directive and refresh
		scope.$watch(attrs.ngModel, function () {
			console.log("Watch ngModel");
		//	console.log(attrs.ngModel);
			//element.multiselect('refresh');
			setTimeout(function() {
				element.multiselect('rebuild');
			}, 0.3 * 1000);
		});

		/*
		scope.$watch(attrs.ngModel, function () {
			console.log("Watch ngModel");
			console.log(attrs.ngModel);
			//element.multiselect('refresh');
			element.multiselect('rebuild');
		});
		*/

		categoryProvider.getCategories(
			function(data) {
				scope.categories = data;
				//scope.categories.splice(0, 0, { id: 0, name: '(all)' });
		
				setTimeout(function() { element.multiselect('rebuild'); }, 0.3*1000); // little delay
			},
			function(error) { throw Error("Fail to load categories. " + error); },
			function() { scope.loading = false; }
		);


	};

	return directive;
}]);

