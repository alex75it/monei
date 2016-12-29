// Jasmine test
"use strict";

describe("RegistryController", function () {

    var scope;
    var controller;
    beforeEach(module(app.name));

    beforeEach(inject(function ($rootScope, $controller, utils, RegistryDataProvider) {
        scope = $rootScope.$new();
        controller = $controller("RegistryController", {
            $scope: scope,
            utils: utils,
            RegistryDataProvider: RegistryDataProvider
        });
    }));
    
    it("must have been defined", function () {
        expect(controller).toBeDefined();
    });
    
    it("contains \"init\" function", function() {
        expect(scope.init).toBeDefined();
    });
    

    describe("search function", function() {
        
    });

    it("search method works", function() {
        expect(scope.search).toBeDefined();
        scope.search();
        // it is true immediately after the call
        expect(scope.loading).toBe(true);
    });

    xit("export call should works", function() {        

    });

    xit("showError function works", function() {        

    });

});
