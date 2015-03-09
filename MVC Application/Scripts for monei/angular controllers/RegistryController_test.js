// Jasmine test
"use strict";


describe("RegistryController", function () {

	var scope;
	var controller;
	beforeEach(module(app.name));

	beforeEach(inject(function ($rootScope, $controller, utils, registryDataProvider) {
		scope = $rootScope.$new();
		controller = $controller("RegistryController", {
			$scope: scope,
			utils: utils,
			registryDataProvider: registryDataProvider
		});
	}));
	
	it("must have been defined", function () {
		expect(controller).toBeDefined();
	});
	
	it("contains \"init\" function", function() {
			expect(scope.init).toBeDefined();
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
