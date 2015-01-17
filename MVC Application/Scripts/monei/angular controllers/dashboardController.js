app = angular.module("monei", ["googlechart"]);

app.controller("DashboardController", ["$scope", "$http", function ($scope, $http) {
//app.controller("DashboardController", function ($scope, $http) {

	_scope = $scope;

	alert("DashboardController");

	$scope.userCurrency = "€";
	$scope.year = 2015;
	$scope.data = null;

	var in_out_colors = ["#088A08", "#DF0101"];

	$scope.initialize = function () {
		$scope.loadData();
	};
			
	$scope.loadData = function () {

		var year = $scope.year;
		// todo: Account Guid
		$http.get("/api/dashboard/" + year, { headers: { "account-guid": "9CED8C90-4EE8-420D-8D6E-9D020889B06B" } })
			.success(function (data) {
				$scope.data = data;
				$("#monthsContainer").fadeIn();

				showYearData();						
			});

		function showYearData() {

			var resumeChart = {};
			resumeChart.type = "PieChart";
			resumeChart.data = [
				["Voice", "Amount"],
				["Income", $scope.data.income],
				["Outcome", $scope.data.outcome]
			];
			//chart.data.push(["Movements", 10])

			resumeChart.options = {
				displayExactValues: true,
				width: 000, // trick to make background tranparent
				height: 150,
				colors: in_out_colors,
				is3D: true,
				//chartArea: { left: 10, top: 10, bottom: 10, height: "100%" }
			};

			resumeChart.formatters = {
				number: [{
					columnNum: 1,
					pattern: "€ #,##0.00"
				}]
			};

			$scope.yearResumeChart = resumeChart;


			var monthsChart = {};
			monthsChart.type = "ColumnChart";

			monthsChart.data = [
				["Month", "Income", "Outcome"]
			];

			//$scope.data.months.each(function (item, index) {
			//});
			//for (index = 0; index < $scope.data.months.length; index++) {
			//	var month = $scope.data.months[index];
			//	monthsChart.data.push([$scope.monthName(month.index), month.income, month.outcome]);
			//}

			monthsChart.data = {
				cols: [
					{ label: "Month", type:"string" }, { label: "Income", type:"number" }, { label: "Outcome", type:"number" }
				],
				rows: []
			};

			for (index = 0; index < $scope.data.months.length; index++) {
				var month = $scope.data.months[index];
				monthsChart.data.rows.push(
					{
						c: [
							{ v: $scope.monthName(month.index) },
							{ v: month.income },
							{ v: month.outcome }
						]
					}
				);
			}

			monthsChart.options = {
				displayExactValues: true,
				isStacked: false,
				displayed: true,
				width: 000, // trick to make background tranparent
				height: 150,
				is3D: true,
				fill: 50,
				//colors: ['#e0440e', '#e6693e', '#ec8f6e', '#f3b49f', '#f6c7b6'],
				colors: in_out_colors,
				//chartArea: { left: 10, top: 10, bottom: 100, height: "100%" },
				//chartArea: { left:10, right:0 },
				//hAxis: {title:"Months"}
				comma_item:null
			};

			$scope.yearMonthsChart = monthsChart;

			$("#yearContainer").fadeIn();
		};

	};

	$scope.yearChanged = function () {
		$("#yearContainer").fadeOut();
		$("#monthsContainer").fadeOut();
		$scope.loadData();
	};		

	// todo: create generic provider
	$scope.monthName = function (monthIndex) {
		return moment([1, monthIndex-1, 1]).format("MMMM");
	};

	$scope.initialize();
}]);

// Load the Visualization API and the charts package.
google.load('visualization', '1.0', { 'packages': ['corechart'] });

var _monthScope;
var x, y, z;
app.directive("dashboardMonth", function () {
	var directive = {};
	directive.restrict = "EA";
	directive.replace = true;
	directive.templateUrl = "/Scripts/monei/directive-templates/dashboard-month.html";
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
			hAxis: {maxValue: 1000},
			is3D: true//,
			//orientation: 'horizontal'
		};


		var data = ([
			['Category', 'Amount', {'role':'style'}]
		]);

		for (index = 0; index < $scope.month.categories.categories.length; index++) {
			var category = $scope.month.categories.categories[index];
			if (category.amount <= 0) // only expenses or empty category
				data.push([category.name, -category.amount, colors[index]]);
		}
		var dataTable = google.visualization.arrayToDataTable(data);
		var formatter = new google.visualization.NumberFormat(
			{ suffux: '€', negativeColor: 'red', negativeParens: true });
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

