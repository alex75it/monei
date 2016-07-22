"use strict";
/// Jasmine tests
describe("SubcategoryDataProvider", function () {

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
		inject(function (SubcategoryDataProvider) {
			expect(SubcategoryDataProvider.getSubcategories).toBeDefined();
		});
	});

	it("when getSubcategories is called it call success callback", function () {
		inject(function (SubcategoryDataProvider) {
			// Arrange
		    var categoryId = 1;
			$httpBackend.expectGET("/api/subcategory/category/" + categoryId).respond(true);

			var service = {
				success: function (data) { }
			};
			spyOn(service, "success");
		    // Act
			SubcategoryDataProvider.getSubcategories(categoryId, service.success);

			$httpBackend.flush();
			// Assert
			expect(service.success).toHaveBeenCalled();
		});
	});


	it("when getSubcategories is called success return expected data", function () {
		inject(function (SubcategoryDataProvider) {
		    var categoryId = 1;
		    var returnedSubcategories = [{ "id": 1, "name": "subcategory A" }, {"id":2}];
			$httpBackend.expectGET("/api/subcategory/category/" + categoryId).respond(returnedSubcategories);

			var result;
			var success = function (data) { result = data; };
			SubcategoryDataProvider.getSubcategories(categoryId, success);
			$httpBackend.flush();

			expect(result).not.toBeUndefined();
			expect(result).not.toBeNull();
			expect(result.length).toBe(2);
			expect(result[0].name).toEqual("subcategory A");
		});
	});

});