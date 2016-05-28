app = angular.module("monei", ["googlechart"]);

app.controller("DashboardController", ["$scope", "$http", function ($scope, $http) {

	_scope = $scope;

	$scope.accountGuid = "9CED8C90-4EE8-420D-8D6E-9D020889B06B";
	$scope.userCurrency = "€";
	$scope.years = [];
	$scope.year = 2016;
	$scope.data = null;

	var in_out_colors = ["#088A08", "#DF0101"];

	$scope.initialize = function () {
		$scope.loadData();
	};
			
	$scope.loadData = function () {

		var year = $scope.year;
		// todo: Account Guid
		$http.get("/api/dashboard/" + year, { headers: { "account-guid":$scope.accountGuid } })
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
