// Jasmine test
"use strict";


describe("RegistryController", function () {

	var scope;
	var controller;
	var utils;

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
	

	xit("search emthod works", function() {
		

	});

	it("export call should works", function() {
		

	});

	it("showError function works", function() {
		

	});

});
