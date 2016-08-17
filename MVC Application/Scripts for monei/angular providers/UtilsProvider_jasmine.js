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

    describe("setAccountGuid()", function () {
        it("should exists", function () {
            expect(utils.setAccountGuid).toBeDefined();
        });

        it("should set AccountGuid", function () {
            var accountGuid = "1";
            utils.setAccountGuid(accountGuid);
            var value = utils.getAccountGuid();
            expect(value).toEqual(accountGuid);
        });
    });

    describe("getAccountGuid()", function () {
        it("should exists", function () {
            expect(utils.getAccountGuid).toBeDefined();
        });

        describe("when AccountGuid is set", function () {
            var accountGuid = "2";
            beforeEach(function () { utils.setAccountGuid(accountGuid); });
            it("should return the value", function () {
                var value = utils.getAccountGuid();
                expect(value).toEqual(accountGuid);
            });
        });

        describe("when AccountGuid is NOT set", function () {
            it("should throw error", function () {
                expect(utils.getAccountGuid).toThrow();
            });
        });
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
