"use strict";
/// Jasmine tests
describe("PurchaseDataProvider", function () {
    it("has \"save\" function", function () {
        inject(function(PurchaseDataProvider){
            expect(PurchaseDataProvider.save).toBeDefined();
        });
    })
});
