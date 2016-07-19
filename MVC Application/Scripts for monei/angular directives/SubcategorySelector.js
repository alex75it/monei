"use strict";

app.directive("moneiSubcategorySelector",
["subcategoryDataProvider",
function (subcategoryDataProvider) {

    var directive = {
        restrict: "E",
        templateUrl: "/Scripts for monei/directive-templates/SubcategorySelector.html",
        replace: true
    };

    directive.scope = {
        id: "=",
        selectedCategory: "=category"    // atribute categories must exists and bind to selectedCategory
        , selectedSubcategories: "=" // attribute "selected-subcategories" must exists
    };

    directive.link = function(scope, element, attrs) {

        scope.loadingText = "(none category selected)"; // todo: pass as attribute

        var selectElement = element.find("select");  // if none container is used use just "element"
        var loadingPanel = element.find("#loadingPanel_" + scope.id);

        // pass class attribute from container to select
        selectElement.attr("class", element.attr("class"));
        element.attr("class", ""); // remove "form-control" class
        
        function createMultiselect() {
            // var selectElement = element; // if a container is used use element.find("select")

            selectElement.multiselect({
                templates: {
                    button: '<button type="button" class="multiselect dropdown-toggle form-control" data-toggle="dropdown"></button>'
                },
                buttonClass: 'btn btn-default',
                buttonWidth: 'auto',
                buttonContainer: '<div class="btn-group" />',
                maxHeight: false,
                buttonText: function (options) {
                    if (options.length == 0) {
                        return '<span class="pull-left">(all)</span> <span class="caret"></span>';
                    } else if (options.length > 1) {
                        return '<span class="pull-left">' + options.length + ' selected</span> <span class="caret"></span>';
                    } else {
                        var selected = '';
                        options.each(function () {
                            selected += $(this).text() + ', ';
                        });
                        return '<span class="pull-left">' + selected.substr(0, selected.length - 2) + '</span> <span class="caret"></span>';
                    }
                }
            });
        }

        scope.$watch(
            function () { return scope.selectedCategory; },
            function () {
                scope.subcategories = [];
                scope.loading = true;
                if (scope.selectedCategory) {
                    subcategoryDataProvider.getSubcategories(scope.selectedCategory,
                        function(data) {
                            scope.subcategories = data;
                            if (element.attr("multiple")) {
                                setTimeout(function () {
                                    createMultiselect()
                                }, 0.5 * 1000); // little delay is needed
                            }
                        },
                        function(error) { alert(error); },
                        function() {scope.loading = false;}
                    );
                }
            }
        );

    };

    return directive;

}]);