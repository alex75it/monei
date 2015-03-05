app.directive("moneiCategorySelector",
["utils", "category",
function (utils, categoryProvider) {

	var directive = {};
	directive.restrict = "E";
	directive.replace = true;
	directive.templateUrl = "/Scripts for monei/directive-templates/CategorySelector.html";
	/*
	directive.scope = {

	};*/
	/*
	directive.controller = function($scope, $element) {
		//$scope.clicked = 0;
		//$scope.click = function() {
		//	$scope.clicked++;
		//}

		$scope.reload = function () {
			categoryProvider.getCategories(
				function (data) {
					$scope.categories = data;

					//setTimeout(function() {
					$element.multiselect('rebuild');
					//}, 0.3*1000); // little delay
				},
				function (error) { throw Error("Fail to load categories. " + error); },
				function () { $scope.loading = false; }
			);

		};
	};*/

	directive.link = function(scope, element, attrs) {
		scope.loading = true;

		element.find("select").multiselect({
			templates: {
				button: '<button type="button" class="multiselect dropdown-toggle form-control" data-toggle="dropdown"></button>'
			},
			buttonClass: 'btn btn-default',
			buttonWidth: 'auto',
			buttonContainer: '<div class="btn-group bootstrap-select" />',
			maxHeight: false,
			buttonText: function(options) {
				if (options.length == 0) {
					return '<span class="pull-left">(all)</span> <span class="caret"></span>';
				} else if (options.length > 1) {
					return '<span class="pull-left">' + options.length + ' selected</span> <span class="caret"></span>';
				} else {
					var selected = '';
					options.each(function() {
						selected += $(this).text() + ', ';
					});
					return '<span class="pull-left">' + selected.substr(0, selected.length - 2) + '</span> <span class="caret"></span>';
				}
			}
		});


		//scope.$watch(
		//	function () { return element.find("select option").length; },
		//	function () {
		//		console.log("options: " + element.find("select option").length);
		//		//element.find("select").multiselect('rebuild');
		//		//setTimeout(function() {
		//		//	 element.find("select").multiselect('rebuild');
		//		//}, 0.3*1000); // little delay
		//	}
		//);

		categoryProvider.getCategories(
			function(data) {
				scope.categories = data;
				//console.log("success");
				setTimeout(function() {
					 element.find("select").multiselect('rebuild');
				}, 0.5*1000); // little delay is needed
			},
			function(error) { throw Error("Fail to load categories. " + error); },
			function() { scope.loading = false; }
		);

	};

	return directive;
}]);

