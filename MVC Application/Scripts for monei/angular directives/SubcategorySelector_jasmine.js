"use strict";
/// Jasmine tests
describe("Subcategory Selector", function () {

    var scope;
    var directive;
    beforeEach(module(app.name));

    beforeEach(inject(function ($rootScope, $directive, SubcategoryDataProvider) {
        //scope = $rootScope.$new();
        directive = app.directive("moneiSubcategorySelector");
        alert(directive);

    }));

    it("should can be created", function () {
        expect(directive).toBeDefined();
    });

});