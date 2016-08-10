"use strict";
/// Jasmine tests
describe("CategoryDataProvider", function() {

    var appName = app.name; // this "initialize" module, all other way give an error
    beforeEach(angular.mock.module("monei")); // this is needed to make injection of providers works

    var baseUrl = "/api/category/";

    // mock the Angular $http service
    var requestHandler;
    var httpBackend;
    var CategoryDataProvider;

    beforeEach(inject(function ($injector, _CategoryDataProvider_) {
        httpBackend = $injector.get("$httpBackend");
        // default response
        CategoryDataProvider = _CategoryDataProvider_;
    }));

    it("is injected", function() {
        expect(CategoryDataProvider).toBeDefined();
    });

    it("getCategories() is defined", function () {
        expect(CategoryDataProvider.getCategories).toBeDefined();		
    });

    it("when call \"getCategories\" function the \"callback\" function is called", function() {
        // Arrange
        var url = baseUrl + "list";
        httpBackend.expectGET(url).respond([{ id: 123, name: "Home" }]);

        var callbackSpy = {
            success: function(data) {}
        };
        spyOn(callbackSpy, "success");

        // Act
        CategoryDataProvider.getCategories(callbackSpy.success);
        httpBackend.flush();

        // Assert
        expect(callbackSpy.success).toHaveBeenCalled();	
    });

    it("when call \"getCategories\" returns data", function () {
        // Arrange
        var url = baseUrl + "list";
        httpBackend.expectGET(url).respond([{ id: 123, name: "Home" }]);

        var returnData = "null";
        var success = function(data) { returnData = data; };

        // Act
        CategoryDataProvider.getCategories(success);
        httpBackend.flush();

        // Assert
        expect(returnData).not.toBeUndefined();
        expect(returnData).not.toBeNull();
        expect(returnData.length).toBe(1);
        expect(returnData[0].name).toEqual("Home");	
    });
    
});