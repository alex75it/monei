"use strict";
/// Jasmine tests
describe("categoryDataProvider", function() {

	var appName = app.name; // this "initialize" module, all other way give an error
	beforeEach(angular.mock.module("monei")); // this is needed to make injection of providers works

	it("contains \"getCategories\" function", function () {
		inject(function (categoryDataProvider) {
			expect(categoryDataProvider.getCategories).toBeDefined();
		});
	});

});