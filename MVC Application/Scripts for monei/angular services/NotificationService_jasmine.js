"use strict";
/// Jasmine tests
describe("NotificationService", function () {

    var appName = app.name; // this "initialize" module, all other way give an error

    beforeEach(angular.mock.module("monei"));

    it("injection should work", function () {
        inject(function (NotificationService) {
            expect(NotificationService).toBeDefined();
        });
    });

    it("info() should exists", function () {
        inject(function (NotificationService) {
            expect(NotificationService.info).toBeDefined();
        });
    });

    it("warn() should exists", function () {
        inject(function (NotificationService) {
            expect(NotificationService.warn).toBeDefined();
        });
    });

    it("error() should exists", function () {
        inject(function (NotificationService) {
            expect(NotificationService.error).toBeDefined();
        });
    });

});
