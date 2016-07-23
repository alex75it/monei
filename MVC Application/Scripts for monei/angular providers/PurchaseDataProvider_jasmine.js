"use strict";
/// Jasmine tests
describe("PurchaseDataProvider", function () {

    var appName = app.name; // this "initialize" module, all other ways return an error
    beforeEach(angular.mock.module("monei"));   // this is needed to make injection of providers works

    it("has \"save\" function", function () {
        inject(function (PurchaseDataProvider) {
            expect(PurchaseDataProvider.save).toBeDefined();
        });
    })
});
