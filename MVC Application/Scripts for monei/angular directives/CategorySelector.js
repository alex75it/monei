app.directive("moneiCategorySelector",
["utils", "CategoryDataProvider",
function (utils, CategoryDataProvider) {

    var directive = {};
    directive.restrict = "E";
    directive.replace = true;
    directive.templateUrl = "/Scripts for monei/directive-templates/CategorySelector.html";

    directive.scope = {
        selectedCategories: "=" // the container should use the property "selected-categories"
    };

    directive.link = function(scope, element, attrs) {
        scope.loading = true;
        
        var select = element; // if a container is used use element.find("select")
        //scope.select = select;

        function createMultiselect() {
            alert("createMultiselect");
            if (attrs.multiselect == 'true') {

                element.attr("multiple", "multiple");

                removeEmptyOption(select);

                select.multiselect({
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
                            var selected = '';

                            selectedOptions.each(function () {
                                selected += $(this).text() + ', ';
                            });
                            return '<span>' + selected.substr(0, selected.length - 2) + '</span> <span class="caret"></span>';
                        }
                    }
                });
            }
        };

        function removeEmptyOption(selectElement) {
            // remove unknown empty option cretaed by Angular because "multiple" prop is not set at angular compile time.
            $('option[value="?"]', selectElement).remove();
        }

        CategoryDataProvider.getCategories(
            function(data) {
                scope.categories = data;

                setTimeout(function () {
                    createMultiselect();
                }, 0.5 * 1000); // a little delay is needed
            },
            function(error) { throw Error("Fail to load categories. " + error); },
            function() { scope.loading = false; }
        );
        
    };

    return directive;
}]);

