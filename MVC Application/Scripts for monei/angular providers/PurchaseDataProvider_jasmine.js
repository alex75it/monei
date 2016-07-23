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

    it("has \"save\" function", function () {
        inject(function (PurchaseDataProvider) {
            expect(PurchaseDataProvider.save).toBeDefined();
        });
    })

    describe("save()", function () {
        it("should call /api/purcase/ with POST HTTP method", function () {
            var url = baseUrl;
            httpBackend.expectPOST(baseUrl).respond(null);

            httpBackend.verifyNoOutstandingExpectation();
            httpBackend.verifyNoOutstandingRequest();
        });

        describe("when call return an error", function () {
            it("should call error and finish callbacks", function () {
                inject(function (PurchaseDataProvider) {
                    var url = baseUrl;
                    httpBackend.expectPOST(baseUrl).respond(null);

                    var callbackSpy = {
                        error: function () { },
                        finish: function () { }
                    }
                    spyOn(callbackSpy, "error");
                    spyOn(callbackSpy, "finish");

                    PurchaseDataProvider.save({}, null, callbackSpy.error, callbackSpy.finish);

                    expect(callbackSpy.error).toHaveBeenCalled();
                    expect(callbackSpy.finish).toHaveBeenCalled();
                });
            });
        });

        it("should call finish callback", function () {
            inject(function (PurchaseDataProvider) { 
                var url = baseUrl;
                httpBackend.expectPOST(baseUrl).respond(null);

                var callCheck = false;
                PurchaseDataProvider.save({}, null, null, function() { callCheck = true; });

                expect(callCheck).toBe(true);
            });
        });
    });
});
