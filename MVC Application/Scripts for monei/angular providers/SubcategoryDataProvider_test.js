"use strict";
/// Jasmine tests
describe("subcategoryDataProvider", function () {

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
		inject(function (subcategoryDataProvider) {
			expect(subcategoryDataProvider.getSubcategories).toBeDefined();
		});
	});

	it("when getSubcategories is called it call success callback", function () {
		inject(function (subcategoryDataProvider) {
			// Arrange
			requestHandler.respond([{ name: "subcategory A" }]);
			$httpBackend.expectGET("/api/subcategory/");

			var service = {
				success: function (data) { }
			};
			spyOn(service, "success");
			// Act
			subcategoryDataProvider.getSubcategories(service.success);

			$httpBackend.flush();
			// Assert
			expect(service.success).toHaveBeenCalled();
		});
	});


	it("when getSubcategories is called success return expected data", function () {
		inject(function (subcategoryDataProvider) {
			requestHandler.respond([{ name: "subcategory A" }]);
			$httpBackend.expectGET("/api/subcategory/");

			var result;
			var success = function (data) { result = data; };
			subcategoryDataProvider.getSubcategories(success);
			$httpBackend.flush();

			expect(result).not.toBeUndefined();
			expect(result).not.toBeNull();
			expect(result.length).toBe(1);
			expect(result[0].name).toEqual("subcategory A");
		});
	});

});