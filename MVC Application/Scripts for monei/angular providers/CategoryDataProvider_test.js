"use strict";
/// Jasmine tests
describe("categoryDataProvider", function() {

	var appName = app.name; // this "initialize" module, all other way give an error
	beforeEach(angular.mock.module("monei")); // this is needed to make injection of providers works

	var baseUrl = "/api/category/";

	// mock the Angular $http service
	var requestHandler;
	var httpBackend;
	beforeEach(inject(function($injector) {
		httpBackend = $injector.get("$httpBackend");
		// default response
		//requestHandler = 
	}));

	it("contains \"getCategories\" function", function () {
		inject(function (categoryDataProvider) {
			expect(categoryDataProvider.getCategories).toBeDefined();
		});
	});

	it("when call \"getCategories\" function the \"callback\" function is called", function() {
		inject(function (categoryDataProvider) {
			// Arrange
			var url = baseUrl + "list";
			httpBackend.expectGET(url).respond([{ id: 123, name: "Home" }]);

			var callbackSpy = {
				success: function(data) {}
			};
			spyOn(callbackSpy, "success");

			// Act
			categoryDataProvider.getCategories(callbackSpy.success);
			httpBackend.flush();

			// Assert
			expect(callbackSpy.success).toHaveBeenCalled();
		});
	});

	it("when call \"getCategories\" returns data", function () {
		inject(function (categoryDataProvider) {
			// Arrange
			var url = baseUrl + "list";
			httpBackend.expectGET(url).respond([{ id: 123, name: "Home" }]);

			var returnData = "null";
			var success = function(data) { returnData = data; };

			// Act
			categoryDataProvider.getCategories(success);
			httpBackend.flush();

			// Assert
			expect(returnData).not.toBeUndefined();
			expect(returnData).not.toBeNull();
			expect(returnData.length).toBe(1);
			expect(returnData[0].name).toEqual("Home");
		});
	});


});