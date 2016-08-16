var _error;

app.directive("moneiRegistryCreatePanel",
["$timeout", "CategoryDataProvider", "RegistryDataProvider", "NotificationService", 
    function ($timeout, CategoryDataProvider, RegistryDataProvider, NotificationService) {

    var directive = {
        scope: {
            onRecordCreated: "&",
            me: "=",
        }
    };
    directive.restrict = "E";
    directive.replace = true;
    directive.templateUrl = "/Scripts for monei/directive-templates/RegistryCreatePanel.html";

    directive.link = function (scope, element, attrs) {
        
        scope.id = attrs.id;

        scope.selectedCategory = null;
        scope.selectedSubcategory = null;
        scope.noCategorySelectedText = "(select one)";
        scope.noSubategorySelectedText = "(select one)";

        $(element[0].querySelector('.datetimepicker-date')).datetimepicker(
            {
                format: "L",
                showTodayButton: true
            }
        );
        //$(element.find('.datetimepicker-date')).        

        scope.save = function () {

            alert("save");

            scope.error = null;
            try {
                var purchase = {};

                PurchaseDataProvider.save(
                    scope.savePurchaseSuccess, scope.savePurchaseFail, scope.savePurchaseFinish
                );
            }
            catch(error)
            {
                _error = error;
                scope.showError(error);
                return;
            }
            scope.onRecordCreated();
            scope.close();            
        };

        scope.showError = function(error){
            scope.error = error;
            $timeout(function () { scope.error = null; }, 3 * 1000);
        };

        scope.close = function () {
            element.modal("hide");
        };        
         
        scope.reset = function () {   
            scope.date = moment().format("L");
            scope.amount = 0;
            scope.category = null;
            scope.subcategory = null;
        };

        // expose the reset method to container. "me" is a property of the directive scope that exists for this specific need.        
        scope.me.reset = scope.reset;

        scope.setDate = function(days) {
            scope.date = moment().add(days, "days").format("L");            
        };

        // call reset
        scope.reset();

        scope.savePurchaseSuccess = function () {
            alert("success");
            NotificationService.info("Purchase saved");
        };

        scope.savePurchaseFail = function () {
            alert("fail");
            scope.showError("Create Purchase fail");
            NotificationService.error("Purchase saved");
        };

        scope.savePurchaseFinish = function () {
            scope.reset();
        };
    };

    return directive;
}]);
