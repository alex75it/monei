﻿app.directive("moneiRegistryCreatePanel",
["$timeout", "CategoryDataProvider", function ($timeout, CategoryDataProvider) {

    var directive = {
        scope: {
            onRecordCreated: "&",
            me: "="
        }
    };
    directive.restrict = "E";
    directive.replace = true;
    directive.templateUrl = "/Scripts for monei/directive-templates/RegistryCreatePanel.html";

    directive.link = function (scope, element, attrs) {
        //scope.date = moment().add(1, "days");
        scope.amount = 0;

    
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
        
        scope.me.reset = 
        scope.reset = function () {
            alert("reset");
            scope.date = moment();
        };
        
    };

    return directive;
}]);
