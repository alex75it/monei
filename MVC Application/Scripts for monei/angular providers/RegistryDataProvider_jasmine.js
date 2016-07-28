"use strict";
/// Jasmine tests
describe("registryDataProvider", function () {

	var homeCategoryId = 3;

	var appName = app.name; // this "initialize" module, all other ways return an error
	beforeEach(angular.mock.module("monei")); // this is needed to make injection of providers works

	var baseUrl = "/api/registry";

	// mock the Angular $http service
	var requestHandler;
	var httpBackend;

	beforeEach(inject(function ($injector) {
		httpBackend = $injector.get("$httpBackend");
	}));

	afterEach(function () {
	    httpBackend.verifyNoOutstandingExpectation();
	    httpBackend.verifyNoOutstandingRequest();
	});

	it("has \"search\" function", function () {
		inject(function (registryDataProvider) {
			expect(registryDataProvider.search).toBeDefined();
		});
	});

	it("when call \"search\" function the \"callback\" function is called", function () {
		inject(function (registryDataProvider) {
			// Arrange
			var url = baseUrl + "/search";
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
			var url = baseUrl + "/search";
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

	it("has \"save\" function", function () {
	    inject(function (registryDataProvider) {
	        expect(registryDataProvider.save).toBeDefined();
	    });
	})

	describe("save()", function () {
	    it("should call /api/registry with POST HTTP method", function () {
	        inject(function (registryDataProvider) {
	            var url = baseUrl;
	            httpBackend.expectPOST(baseUrl, {}).respond(null);

	            registryDataProvider.save({});
	            httpBackend.flush();

	            expect(true).toBe(true);
	        });
	    });

	    describe("when call return an error", function () {
	        it("should call error and finish callbacks", function () {
	            inject(function (registryDataProvider) {
	                httpBackend.expectPOST(baseUrl).respond(500, ""); // respond(null);

	                var callbackSpy = {
	                    error: function () { },
	                    finish: function () { }
	                }
	                spyOn(callbackSpy, "error");
	                spyOn(callbackSpy, "finish");

	                registryDataProvider.save({}, null, callbackSpy.error, callbackSpy.finish);
	                httpBackend.flush();

	                expect(callbackSpy.error).toHaveBeenCalled();
	                expect(callbackSpy.finish).toHaveBeenCalled();
	            });
	        });
	    });

	    describe("when call return data", function () {
	        it("should call success callback with data", function () {
	            inject(function (registryDataProvider) {
	                var expectedData = 1;
	                httpBackend.whenPOST(baseUrl).respond(expectedData);

	                var returnedData = null;
	                registryDataProvider.save(null, function (newId) { returnedData = newId; });
	                httpBackend.flush();

	                expect(returnedData).toEqual(expectedData);
	            });
	        });
	    });

	    it("should call finish callback", function () {
	        inject(function (registryDataProvider) {
	            var url = baseUrl;
	            httpBackend.expectPOST(baseUrl).respond(null);

	            var callCheck = false;
	            registryDataProvider.save({}, null, null, function () { callCheck = true; });
	            httpBackend.flush();

	            expect(callCheck).toBe(true);
	        });
	    });
	});

});