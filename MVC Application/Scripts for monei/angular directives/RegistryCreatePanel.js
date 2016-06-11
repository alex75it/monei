app.directive("moneiRegistryCreatePanel",
["$timeout", "CategoryDataProvider", function ($timeout, CategoryDataProvider) {

    var directive = {
        scope: {
            onRecordCreated: "&"
        }
    };
    directive.restrict = "E";
    directive.replace = true;
    directive.templateUrl = "/Scripts for monei/directive-templates/RegistryCreatePanel.html";

    directive.link = function (scope, element, attrs) {
        scope.date = moment().toDate();
        scope.amount = 0;

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
        
    };

    return directive;
}]);
