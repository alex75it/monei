"use strict";

/// Jasmine tests
describe("Directive: RegistryCreatePanel", function () {

    var $compile;
    var $rootScope;
    var directiveElement;
    
    beforeEach( function(){
        //angular.mock.module("monei");
        angular.module(app.name);

        inject(function (_$compile_, _$rootScope_) {
            $compile = _$compile_;
            $rootScope = _$rootScope_;
            directiveElement = getDirectiveElement();
        });
    });

    function getDirectiveElement() {
        var element = angular.element("<moneiRegistryCreatePanel></moneiRegistryCreatePanel>");
        var compiledElement = $compile(element)($rootScope);
        $rootScope.$digest();
        return compiledElement;
    };


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

    it("is compiled", function () {
        expect(directiveElement).toBeDefined();
    });

    describe("save()", function () {
        it("is defined", function () {
            expect(directiveElement.save).toBeDefined();
        });
    });

});