"use strict";
/// Jasmine tests
describe("Subcategory Selector", function () {

    var compile;
    var scope;
    var directiveElement;
    

    beforeEach( function(){
        module(app.name);
        inject(function ($compile, $rootScope) {
            compile = $compile;
            scope = $rootScope.$new();

            directiveElement = getDirectiveElement();
        });
    });

    function getDirectiveElement() {
        var element = angular.element("<moneiSubcategorySelector></moneiSubcategorySelector>")
        var compiledElement = compile(element)(scope);
        scope.$digest();
        return compiledElement;
    };

    //https://docs.angularjs.org/guide/unit-testing
    //https://www.sitepoint.com/angular-testing-tips-testing-directives/
       

    it("should render a <select> element", function () {
        var select = directiveElement.find("select");
        expect(select).toBeDefined();
    });

});