"use strict";
/// Jasmine tests
describe("RegistryCreatePanel", function () {

    var appName = app.name; // this "initialize" module, all other way give an error
    beforeEach(angular.mock.module("monei")); // this is needed to make injection of providers works

    http://stackoverflow.com/questions/14238490/injecting-dependent-services-when-unit-testing-angularjs-services
    http://stackoverflow.com/questions/14773269/injecting-a-mock-into-an-angularjs-service

    var panel;

    beforeEach(inject(function (_RegistryCreatePanel_) {

        alert(_RegistryCreatePanel_);
        panel = _RegistryCreatePanel_;
    }));

    it("is injected", function () {
        expect(panel).toBeDefined();
    });

    describe("save()", function () {
        xit("is defined", function () {
            expect(panel.save).toBeDefined();
        });
    });

});