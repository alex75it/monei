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

            //alert("save");
            TODO: implement method

            scope.error = null;
            try {
                var data = {};

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

        scope.showError = function (error) {  
            scope.error = error.message || error;
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

        scope.saveRecordSuccess = function () {
            NotificationService.info("Purchase saved");
            scope.onRecordCreated();
            scope.close();
        };

        scope.saveRecordFail = function (error) {
            scope.showError(error);
            NotificationService.error("Record saved");
        };

        scope.saveRecordFinish = function () {
            scope.reset();
        };
    };

    return directive;
}]);
