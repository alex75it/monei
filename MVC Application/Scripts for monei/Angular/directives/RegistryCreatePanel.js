app.directive("moneiRegistryCreatePanel",
["$timeout", "CategoryDataProvider", "RegistryDataProvider", "NotificationService", 
    function ($timeout, CategoryDataProvider, RegistryDataProvider, NotificationService) {

    var directive = {
        scope: {            
            open: "=",            
            close: "=",
            onRecordCreated: "&"
        }
    };
    directive.restrict = "E";
    directive.replace = true;
    directive.templateUrl = "/Scripts for monei/Angular/directive-templates/RegistryCreatePanel.html";

    directive.link = function (scope, element, attrs) {
        
        scope.id = attrs.id;

        scope.selectedCategory = null;
        scope.selectedSubcategory = null;
        scope.amount = 0.0;
        scope.noCategorySelectedText = "(select one)";
        scope.noSubategorySelectedText = "(select one)";

        scope.operationTypes = [
            { name: "Income", value: +1 },
            { name: "Outcome", value: -1 },
            { name: "Transfer", value: 0 }
        ];

        scope.selectedOperationType = scope.operationTypes[1].value; // outcome selected by default


        $(element[0].querySelector('.datetimepicker-date')).datetimepicker(
            {
                format: "L",
                showTodayButton: true
            }
        );
        //$(element.find('.datetimepicker-date')).    

        scope.open = function () {
            scope.reset();
            element.modal("show");
        };

        scope.close = function () {
            element.modal("hide");
        };
            var operationType = scope.selectedOperationType;
            var isSpecialEvent = scope.specialEvent;
            var isTaxDeductible = scope.taxDeductible;

        scope.reset = function () {
            scope.date = moment().format("L");
            scope.amount = 0;
            scope.category = null;
            scope.subcategory = null;
            scope.note = "",
            scope.specialEvent = false,
            scope.taxDeductible = false
        };

        scope.save = function () {
            scope.error = null;
            try {
                var data = {
                    date: scope.date,
                    categoryId: scope.selectedCategory,
                    subcategoryId: scope.selectedSubcategory,
                    amount: scope.amount,
                    note: scope.note,                
                    isSpecialEvent: scope.specialEvent,
                    isTaxDeductible: scope.taxDeductible
                };

                RegistryDataProvider.save( data,
                    scope.saveRecordSuccess, scope.saveRecordFail, scope.saveRecordFinish
                );
            }
            catch(error)
            {
                scope.showError(error);
                return;
            }           
        };

        scope.setDate = function (days) {
            scope.date = moment().add(days, "days").format("L");
        };

        scope.showError = function (error) {  
            scope.error = error.exceptionMessage || error.message || error;
            $timeout(function () { scope.error = null; }, 3 * 1000);
        };

        // call reset
        scope.reset();

        scope.saveRecordSuccess = function () {
            alert("saveRecordSuccess");
            NotificationService.info("Record saved");
            scope.onRecordCreated();
            scope.close();
        };

        scope.saveRecordFail = function (error) {
            scope.showError(error);
            NotificationService.error("Record NOT saved");
        };

        scope.saveRecordFinish = function () {
            alert('finish');
        };
    };

    return directive;
}]);
