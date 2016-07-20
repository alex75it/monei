app.directive("moneiRegistryCreatePanel",
["$timeout", "CategoryDataProvider", function ($timeout, CategoryDataProvider) {

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

        scope.save = function () {
            scope.error = null;
            try {
                throw "Not yet implemented";
            }
            catch(error)
            {
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

        scope.me.reset = scope.reset;

        scope.setDate = function(days) {
            scope.date = moment().add(days, "days").format("L");            
        };

        scope.$watch(function () { return scope.selectedCategory; }, function () {
            //alert("category is changed");
        });

        // call reset
        scope.reset();        
    };

    return directive;
}]);
