"use strict";

/// Jasmine tests
describe("RegistryCreatePanel directive", function () {

    var $compile;
    var $rootScope;
    var directiveElement;

    var DIRECTIVE_ID = "registryCreatePanel";
    
    beforeEach( function(){
        //angular.mock.module("monei");
        //angular.module(app.name, []);
        
        inject(function (_$compile_, _$rootScope_) {
            $compile = _$compile_;
            $rootScope = _$rootScope_;
            directiveElement = getDirectiveElement();
        });
    });

    function getDirectiveElement() {
        var element = angular.element('<moneiRegistryCreatePanel id="' + DIRECTIVE_ID + '"></moneiRegistryCreatePanel>');

        var compiledElement = $compile(element)($rootScope);
        $rootScope.$digest();
        return compiledElement;
    };

    function getErrorContainer()
    {
        var errorContainer = directiveElement.find("#" + DIRECTIVE_ID + "_error");
        if (!errorContainer)
            throw Error("fail to find error contaginer");

        return errorContainer;
    }

    
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

            beforeEach(function () {
                //$(saveButton).click.apply($(saveButton)); // can be done without jQuery ?
                //browserTrigger(saveButton, "click");

                //alert(saveButton.triggerHandler);
                saveButton.triggerHandler("click");

                //$rootScope.save();

                //$rootScope.$digest();
                // todo: I don't know how to test this. 
            });

            it("should call Provider save function", function () {
                //controllerSpy.
                expect(true).toBeTruthy();
            });
        });
    });

    describe("showError()", function () {
        var errorContainer = null;

        beforeEach(function () {
            errorContainer = getErrorContainer();
        });

        it("is defined", function () {
            expect(errorContainer).toBeDefined();
        });

        describe("whne called with 'test error'", function () {
            xit("set text in error container and set it visible", function () {
                var errorContainer = getErrorContainer();
                // todo: I don't know how to test this
                //directive.showError("test error");

                expect(errorContainer.text()).toEquual("test error");
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