"use strict";
/// Jasmine tests
describe("PurchaseDataProvider", function () {

    var appName = app.name; // this "initialize" module, all other ways return an error
    beforeEach(angular.mock.module("monei"));   // this is needed to make injection of providers works

    // mock the Angular $http service
    var requestHandler;
    var httpBackend;
    var baseUrl = "/api/purchase";

    beforeEach(inject(function ($injector) {
        httpBackend = $injector.get("$httpBackend");
    }));

    afterEach(function(){
        httpBackend.verifyNoOutstandingExpectation();
        httpBackend.verifyNoOutstandingRequest();
    });

    it("has \"save\" function", function () {
        inject(function (PurchaseDataProvider) {
            expect(PurchaseDataProvider.save).toBeDefined();
        });
    })

    describe("save()", function () {
        it("should call /api/purcase/ with POST HTTP method", function () {
            inject(function (PurchaseDataProvider) {
                var url = baseUrl;
                httpBackend.expectPOST(baseUrl, {}).respond(null);

                PurchaseDataProvider.save({});
                httpBackend.flush();

                expect(true).toBe(true);
            });
        });

        describe("when call return an error", function () {
            it("should call error and finish callbacks", function () {
                inject(function (PurchaseDataProvider) {                    
                    httpBackend.expectPOST(baseUrl).respond(500, ""); // respond(null);

                    var callbackSpy = {
                        error: function () { },
                        finish: function () { }
                    }
                    spyOn(callbackSpy, "error");
                    spyOn(callbackSpy, "finish");

                    PurchaseDataProvider.save({}, null, callbackSpy.error, callbackSpy.finish);
                    httpBackend.flush();

                    expect(callbackSpy.error).toHaveBeenCalled();
                    expect(callbackSpy.finish).toHaveBeenCalled();
                });
            });
        });

        describe("when call return data", function () {
            it("should call success callback with data", function () {
                inject(function (PurchaseDataProvider) {
                    var expectedData = 1;
                    httpBackend.whenPOST(baseUrl).respond(expectedData);

                    var returnedData = null;
                    PurchaseDataProvider.save(null, function (newId) { returnedData = newId; });
                    httpBackend.flush();

                    expect(returnedData).toEqual(expectedData);
                });
            });
        });

        it("should call finish callback", function () {
            inject(function (PurchaseDataProvider) { 
                var url = baseUrl;
                httpBackend.expectPOST(baseUrl).respond(null);

                var callCheck = false;
                PurchaseDataProvider.save({}, null, null, function() { callCheck = true; });
                httpBackend.flush();

                expect(callCheck).toBe(true);
            });
        });
    });
});
