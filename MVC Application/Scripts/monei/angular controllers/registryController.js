
app.controller("registryController", function ($scope, $http) {
	_scope = $scope;
	$scope.filters = {};
	$scope.records;
	$scope.runExport = false;
		
	$scope.export = function () {

		var dateTo = moment($scope.filters.dateTo, 'MM/DD/YYYY').format('YYYY-MM-DD');
		var dateFrom = moment($scope.filters.dateFrom, 'MM/DD/YYYY').format('YYYY-MM-DD');
		var postData = {
			startDate: dateFrom,
			endDate: dateTo
		};

		if ($scope.runExport)
			return;
				
		$scope.runExport = true;
		try {
			
			//var html = "<html><body><div>Downloading..."
			//	+ "<form method=\"POST\" action='/api/registry/export' >"
			//	+ "<input name='StartDate' type='hidden' value='"+dateFrom+"' />"
			//	+ "<input name='EndDate' type=\"hidden\" value=\""+dateTo+"\" />"				
			//	+ "</form><script>try { document.forms[0].submit(); } catch (error) { alert(error);}</script></div></body></html>";
			//var f = $(body).append($("<form>aaa</form>").attr("action", "/api/registry/export").append($("<input type='text'>").val("23rrr4")))
			var dateFrom = '2014-11-01'
			var dateTo = '2014-11-30'
			var $downloadForm = $("<form method='POST'>")
					.attr("action", "/api/registry/export")
					//.css('display', 'none')
					.append($("<input name='StartDate' type='text'>").val(dateFrom))
					.append($("<input name='EndDate' type='hidden'>").val(dateTo))
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
			$(document).remove("#downloadForm")

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
		finally
		{
			$scope.runExport = false;
		}
	};

});

$(function () {

	$('.datepicker').datepicker({ 'todayHighlight': true, 'autoclose': true })
		.on('changeDate', function (e) {
			$(this).val(moment(e.date).format('l'));
		})
		.on('hide', function (e) {
			$(this).val(moment(e.date).format('l'));
		})
	;

	$('i.taxDeductible').tooltip({ title: 'Tax deductible' });
	$('i.specialEvent').tooltip({ title: 'Special event' });


});