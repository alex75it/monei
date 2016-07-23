"use strict";
/// Jasmine tests
describe("registryDataProvider", function () {

	var homeCategoryId = 3;

	var appName = app.name; // this "initialize" module, all other way give an error
	beforeEach(angular.mock.module("monei")); // this is needed to make injection of providers works

	var baseUrl = "/api/registry/";

	// mock the Angular $http service
	var requestHandler;
	var httpBackend;
	beforeEach(inject(function ($injector) {
		httpBackend = $injector.get("$httpBackend");
	}));

	it("has \"search\" function", function () {
		inject(function (registryDataProvider) {
			expect(registryDataProvider.search).toBeDefined();
		});
	});

	it("when call \"search\" function the \"callback\" function is called", function () {
		inject(function (registryDataProvider) {
			// Arrange
			var url = baseUrl + "search";
			httpBackend.expectPOST(url).respond([{ id: 123, name: "Home" }]);

			var callbackSpy = {
				success: function (data) { }
			};
			spyOn(callbackSpy, "success");

			// Act
			var postData = {};
			registryDataProvider.search(postData, callbackSpy.success);
			httpBackend.flush();

			// Assert
			expect(callbackSpy.success).toHaveBeenCalled();
		});
	});

	it("when call \"search\" returns data", function () {
		inject(function (registryDataProvider) {
			// Arrange
			var url = baseUrl + "search";
			httpBackend.expectPOST(url).respond([{ id: 123, date: "2015-03-08" }]);

			var returnData = "null";
			var success = function (data) { returnData = data; };

			// Act
			var postData = { categories: [homeCategoryId] };
			registryDataProvider.search(postData, success);
			httpBackend.flush();

			// Assert
			expect(returnData).not.toBeUndefined();
			expect(returnData).not.toBeNull();
			expect(returnData.length).toBe(1);
			expect(returnData[0].date).toEqual("2015-03-08");
		});
	});


});