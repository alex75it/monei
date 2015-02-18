"use strict";

/// Jasmine tests
describe("subcategoryProvider", function () {

	//var subcategoryProvider;
	var appName = app.name; // this "initialize" module, all other way give an error
	var $httpBackend;
	var requestHandler;

	beforeEach(angular.mock.module("monei"));
	beforeEach(inject(function ($injector) {
		$httpBackend = $injector.get("$httpBackend");
		requestHandler = $httpBackend.when("GET", "/api/subcategory/").respond([]);
	}));

	it("contains getSubcategories function", function () {
		inject(function (subcategoryProvider) {
			expect(subcategoryProvider.getSubcategories).toBeDefined();
		});
	});

	it("when getSubcategories is called it call success callback", function () {
		inject(function (subcategoryProvider) {
			// Prepare
			requestHandler.respond([{ name: "subcategory A" }]);
			$httpBackend.expectGET("/api/subcategory/");

			var service = {
				success: function(data) {}
			};
			spyOn(service, "success");
			// Act
			subcategoryProvider.getSubcategories(service.success);

			$httpBackend.flush();
			// Verify
			expect(service.success).toHaveBeenCalled();
		});
	});


	it("when getSubcategories is called success return expected data", function () {
		inject(function (subcategoryProvider) {
			requestHandler.respond([{ name: "subcategory A" }]);
			$httpBackend.expectGET("/api/subcategory/");

			var result;
			var success = function (data) { result = data; };
			subcategoryProvider.getSubcategories(success);
			$httpBackend.flush();

			expect(result).not.toBeUndefined();
			expect(result).not.toBeNull();
			expect(result.length).toBe(1);
			expect(result[0].name).toEqual("subcategory A");
		});
	});

});