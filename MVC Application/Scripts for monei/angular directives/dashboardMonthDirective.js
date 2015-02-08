
// Load the Visualization API and the charts package.
google.load('visualization', '1.0', { 'packages': ['corechart'] });

var _monthScope;
var x, y, z;
app.directive("dashboardMonth", function () {
	var directive = {};
	directive.restrict = "EA";
	directive.replace = true;
	directive.templateUrl = "/Scripts for monei/directive-templates/dashboard-month.html";
	directive.scope = {
		month: "=month"
	};
	directive.link = function ($scope, element, attribute) {
		//alert("link");
		_monthScope = $scope;

		// create chart for categories
		var chartContainer =
		//= element.find("#categories-chart");
		//element.find("#monthChart_" + $scope.month.index);
		element.find(".categories-chart")[0];

		var colors = ['#3366cc', '#dc3912', '#ff9900', '#109618', '#990099', '#0099c6', '#dd4477', '#66aa00', '#b82e2e', '#b87333', '#c0c0c0', '#ffd700'];
		var options = {
			legend: 'none',
			hAxis: { maxValue: 1000 },
			is3D: true//,
			//orientation: 'horizontal'
		};


		var data = ([
			['Category', 'Amount', { 'role': 'style' }]
		]);

		for (index = 0; index < $scope.month.categories.categories.length; index++) {
			var category = $scope.month.categories.categories[index];
			if (category.amount <= 0) // only expenses or empty category
				data.push([category.name, -category.amount, colors[index]]);
		}
		var dataTable = google.visualization.arrayToDataTable(data);
		var formatter = new google.visualization.NumberFormat(
			{ suffix: '€', negativeColor: 'red', negativeParens: true });
		formatter.format(dataTable, 1); // Apply formatter to second column

		//var chart = new google.visualization.PieChart(element[0]/*chartContainer*/);
		//var chart = new google.visualization.ColumnChart(element[0]/*chartContainer*/);
		var chart = new google.visualization.BarChart(chartContainer);

		chart.formatters = {
			number: [{
				columnNum: 0,
				pattern: "€ #,##0.00"
			}]
		};

		chart.draw(dataTable, options);

		//$(element).fadeIn();
	};

	//directive.require = "^ngModel";
	return directive;
});

