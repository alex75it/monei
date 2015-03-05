
app.controller("RegistryController", [
"$scope", "$http", "RegistryProvider", "utils",
function ($scope, $http, RegistryProvider, utils) {
	_scope = $scope;
	$scope.filters = {};
	$scope.records = [];
	$scope.runExport = false;
		
	$scope.openCreatePanel = function () {
		$("#modal_create").modal("show");
	};


	$scope.search = function () {
		if ($scope.loading)
			return;

		$scope.searchError = null;

		// check filters
		// date range or product must be set
		if ((!$scope.filters.from && !$scope.filters.to) && !$scope.filters.products) {
			//$scope.showWarning("At least a date range or product list must be set");
			$scope.searchError = "At least a date range or product list must be set";
			return;
		}
		
		$scope.loading = true;

		var filters = {
			fromDate: utils.getDate($scope.filters.from),
			toDate: utils.getDate($scope.filters.to),
			categories: $scope.filters.categories // ? $scope.filters.categories.id : null

		};

		$scope.error = null;

		RegistryProvider.search(
			filters,
			/*success*/
			function (data) { $scope.records = data; },
			/*error*/
			$scope.showError,
			/*finally*/
			function () { $scope.loading = false;/* window.setTimeout(function() { $scope.loading = false; } , .5*1000) */ }
			);
	};

	$scope.showError = function (error) {
		$scope.loading = false;
		$scope.error = error.exceptionMessage || error.message || error;
	};


	$scope.init = function () {

		var startDate = moment().startOf('month');

		$scope.filters = {
			from: monei.utils.toShortDate(startDate),
			to: null
		};
	}

	$scope.init();


	$scope.export = function () {


		var dateFrom = moment($scope.filters.dateFrom, 'MM/DD/YYYY').format('YYYY-MM-DD');
		var dateTo = moment($scope.filters.dateTo, 'MM/DD/YYYY').format('YYYY-MM-DD');
		var postData = {
			startDate: dateFrom,
			endDate: dateTo
		};

		if ($scope.runExport)
			return;

		$scope.runExport = true;
		try {
			dateFrom = '2014-11-01';
			dateTo = '2014-11-30';
			var $downloadForm = $("<form method='POST'>")
				.attr("action", "/api/registry/export")
				//.css('display', 'none')
				.append($("<input name='StartDate' type='text'>").val(dateFrom))
				.append($("<input name='EndDate' type='hidden'>").val(dateTo));
			$(body).append($downloadForm);
			$downloadForm.submit();
			$downloadForm.remove();
			return;

			var formId = "downloadForm";
			var downloadForm = "<div>Downloading..."
				+ "<form id='downloadForm' method=\"POST\" action='/api/registry/export' >"
				+ "<input name='StartDate' type='hidden' value='" + dateFrom + "' />"
				+ "<input name='EndDate' type=\"hidden\" value=\"" + dateTo + "\" />"
				+ "</form><script>try { document.forms[0].submit(); } catch (error) { alert(error);}</script></div>";

			//var w = window.open("_blank");
			$(body).append($("<div>").html = downloadForm);  // $(document).html(html);			
			$("#downloadForm").submit();
			$(document).remove("#downloadForm");

			return;
			w.document.write(html);
			setTimeout(function () { w.close(); }, 0.1 * 1000); // little delay is requied or POST not happens

			return;


			$http.post("/api/registry/list", postData)
				.success(function (data) {
					$scope.records = data;
				})
				.error(function (data) { alert(data.exceptionMessage || data.message); })
			;
		}
		finally {
			$scope.runExport = false;
		}
	};

}]);

$(function () {

	$('.datepicker').datepicker({ 'todayHighlight': true, 'autoclose': true })
		/*
		.on('changeDate', function (e) {
			$(this).val(moment(e.date).format('l'));
		})
		.on('hide', function (e) {
			$(this).val(moment(e.date).format('l'));
		})
		*/
	;

	$('i.taxDeductible').tooltip({ title: 'Tax deductible' });
	$('i.specialEvent').tooltip({ title: 'Special event' });


});