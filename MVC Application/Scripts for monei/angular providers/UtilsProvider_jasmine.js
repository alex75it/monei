"use strict";
    
describe("UtilsService", function() {
    var utils, httpBackend;
    var appName = app.name; // this "initialize" module, all other way give an error
    
    beforeEach(angular.mock.module("monei"));

    beforeEach(window.inject(function (_utils_) { 
        utils = _utils_;
    }));

    //var $injector = angular.injector(["monei"]);

    it("should be injected", function () {
        //expect(utils).not.toBeNull();
        expect(utils).toBeDefined();
    });

    it("when call toShortDate() with null should return empty string", function () {
        var date = null;
        expect(utils.toShortDate()).toEqual("");      
    });

    it("when call toShortDate() with moment() object should return shortDate format date", function () {
        var date = moment();
        var result = utils.toShortDate(date);
        expect(result).not.toEqual(null);
        expect(result).not.toEqual("");
        // todo... how to test culture dipendent format?     
    });

    it("when call getDate() with moment() object should return not null or empty", function() {
        var date = moment();
        var result = utils.getDate(date);
        expect(result).not.toEqual(null);
        expect(result).not.toEqual("");      
    });

    it("when call getDate() with Date() object should return not null or empty", function () {
        var date = Date();
        var result = utils.getDate(date);
        expect(result).not.toEqual(null);
        expect(result).not.toEqual("");
    });

});
