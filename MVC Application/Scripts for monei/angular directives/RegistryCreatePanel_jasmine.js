"use strict";
/// Jasmine tests
describe("RegistryCreatePanel", function () {

    var appName = app.name; // this "initialize" module, all other way give an error
    beforeEach(angular.mock.module("monei")); // this is needed to make injection of providers works

    /**
    beforeEach(function () {
        var mockModule = angular.module("mockServices", [])
        //["$timeout", "CategoryDataProvider", "RegistryDataProvider", "NotificationService", 
        mockModule.service("CategoryDataProvider", function () {
            
        });

        mockModule.service("RegistryDataProvider", function () {

        });

        mockModule.service("NotificationService", function () {

        });
        var app = angular.module("monei", [mockModule]);

        alert("mocked");
    });
    */
    

    

    //http://stackoverflow.com/questions/14238490/injecting-dependent-services-when-unit-testing-angularjs-services
    //http://stackoverflow.com/questions/14773269/injecting-a-mock-into-an-angularjs-service

    var panel;

    //beforeEach(inject(function (/*RegistryCreatePanel*/RegistryDataProvider) {
    beforeEach(function () {

        

        //alert(RegistryCreatePanel);
        //panel = RegistryCreatePanel;

        //alert("RegistryDataProvider: " + RegistryDataProvider);
    });

    it("is injected", function () {
        expect(panel).toBeDefined();
    });

    describe("save()", function () {
        it("is defined", function () {
            expect(panel.save).toBeDefined();
        });
    });

});