app.directive("moneiCategorySelector",
["utils", "CategoryDataProvider",
function (utils, CategoryDataProvider) {

    var directive = {};
    directive.restrict = "E";
    directive.replace = true;
    directive.templateUrl =  "/Scripts for monei/Angular/directive-templates/CategorySelector.html";

    directive.scope = {
        selectedCategories: "=" // the container should use the property "selected-categories"
        , noSelectionText: "="  // text to show when there isn't a selection         
    };

    directive.link = function(scope, element, attrs) {
        scope.loading = true;

        scope.selectElement = element; // if a container is used use element.find("select")

        function createMultiselect() {            

            // remove unknown empty option cretaed by Angular because "multiple" prop is not set at angular compile time.
            // $('option[value="?"]', selectElement).remove();

            scope.selectElement.multiselect({
                templates: {
                    button: '<button type="button" class="multiselect dropdown-toggle form-control" data-toggle="dropdown" style="min-width:100px"></button>'
                },
                buttonClass: 'btn btn-default',
                buttonWidth: 'auto',
                buttonContainer: '<div class="btn-group" />',
                maxHeight: false,
                //onChange:
                buttonText: function (selectedOptions) {
                    if (selectedOptions.length == 0) {
                        return '<span>(all)</span> <span class="caret"></span>';
                    } else if (selectedOptions.length > 3) {
                        return '<span>' + selectedOptions.length + ' selected</span> <span class="caret"></span>';
                    } else {
                        var text = '';

                        selectedOptions.each(function () {
                            text += $(this).text() + ', ';
                        });
                        return '<span>' + text.substr(0, text.length - 2) + '</span> <span class="caret"></span>';
                    }
                }
            });            
        };

        CategoryDataProvider.getCategories(
            function(data) {
                scope.categories = data;

                if (attrs.multiple) {
                    setTimeout(function () {
                        createMultiselect();
                    }, 0.5 * 1000); // a little delay is needed
                }
                else {
                    scope.selectElement.find("option")[0].innerHTML = scope.noSelectionText || "";
                }
            },
            function(error) { throw Error("Fail to load categories. " + error); },
            function() { scope.loading = false; }
        );
        
    };

    return directive;
}]);

