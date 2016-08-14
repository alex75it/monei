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
        var element = angular.element('<moneiRegistryCreatePanel id="registryCreatePanel"></moneiRegistryCreatePanel>');
        var compiledElement = $compile(element)($rootScope);
        $rootScope.$digest();
        return compiledElement;
    };

    
    it("is compiled", function () {
        expect(directiveElement).toBeDefined();
    });

    describe('"save" button', function () {
        var saveButton;
        beforeEach(function () {
            saveButton = directiveElement.find("#registryCreatePanel_saveButton");
        });
        
        it("should exists", function () { 
            expect(saveButton).toBeDefined();
        });

        describe("when clicked", function () {
            xit("should call Proder save function", function () {

            });
        });

    });

    xdescribe("save()", function () {
        it("is defined", function () {
            expect(directiveElement.save).toBeDefined();
        });
    });

    xdescribe("reset()", function () {
        it("is defined", function () {
            expect(directiveElement.reset).toBeDefined();
        });
    });
});