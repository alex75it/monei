"use strict";
/// Jasmine tests
describe("RegistryDataProvider", function () {

    var homeCategoryId = 3;

    var appName = app.name; // this "initialize" module, all other ways return an error
    beforeEach(angular.mock.module("monei")); // this is needed to make injection of providers works

    var baseUrl = "/api/registry";

    // mock the Angular $http service
    var requestHandler;
    var httpBackend;
    var registryDataProvider;

    beforeEach(inject(function (_RegistryDataProvider_, _utils_, $injector) { // uderscore are removed by Angular
        httpBackend = $injector.get("$httpBackend");
        registryDataProvider = _RegistryDataProvider_;
        _utils_.setAccountGuid("0000");
    }));

    afterEach(function () {
        httpBackend.verifyNoOutstandingExpectation();
        httpBackend.verifyNoOutstandingRequest();
    });

    describe("search()", function () {

        it("should exists", function () {            
            expect(registryDataProvider.search).toBeDefined();            
        });

        describe("when call to Web API returns", function() {
            it('the \"callback\" function is called', function () {
                // Arrange
                var url = baseUrl + "/search";
                httpBackend.expectPOST(url).respond([{ id: 123, name: "Home" }]);

                var callbackSpy = {
                    success: function (data) { }
                };
                spyOn(callbackSpy, "success");

                // Act
                var postData = {};
                registryDataProvider.search(postData, callbackSpy.success);
                httpBackend.flush();

                // Assert
                expect(callbackSpy.success).toHaveBeenCalled();
            });
        });

        describe("when call to Web API returns data", function () {
            it("returned data is defined", function () {
                // Arrange
                var url = baseUrl + "/search";
                httpBackend.expectPOST(url).respond([{ id: 123, date: "2015-03-08" }]);

                var returnData = "null";
                var success = function (data) { returnData = data; };

                // Act
                var postData = { categories: [homeCategoryId] };
                registryDataProvider.search(postData, success);
                httpBackend.flush();

                // Assert
                expect(returnData).not.toBeUndefined();
                expect(returnData).not.toBeNull();
                expect(returnData.length).toBe(1);
                expect(returnData[0].date).toEqual("2015-03-08");
            });
        });

    });
    
    describe("save()", function () {

        it("should exists", function () {
            expect(registryDataProvider.save).toBeDefined();
        })

        it('should call "/api/registry" with POST HTTP method', function () {

            var url = baseUrl;
            httpBackend.expectPOST(baseUrl, {}).respond(null);

            registryDataProvider.save({});
            httpBackend.flush();

            expect(true).toBe(true);          
        });

        describe("when call to Web API returns an error", function () {
            it('should call "error" and "finish" callbacks', function () {
                httpBackend.expectPOST(baseUrl).respond(500, ""); // respond(null);

                var callbackSpy = {
                    error: function () { },
                    finish: function () { }
                }
                spyOn(callbackSpy, "error");
                spyOn(callbackSpy, "finish");

                registryDataProvider.save({}, null, callbackSpy.error, callbackSpy.finish);
                httpBackend.flush();

                expect(callbackSpy.error).toHaveBeenCalled();
                expect(callbackSpy.finish).toHaveBeenCalled();               
            });
        });

        describe("when call to Web API returns data", function () {
            it('should call "success" callback with data', function () {
                var expectedData = 1;
                httpBackend.whenPOST(baseUrl).respond(200, expectedData);

                var returnedData = null;
                registryDataProvider.save(null, function (newId) { returnedData = newId; });
                httpBackend.flush();

                expect(returnedData).toEqual(expectedData);               
            });
        });

        it("should call finish callback", function () {
            var url = baseUrl;
            httpBackend.expectPOST(baseUrl).respond(null);

            var callCheck = false;
            registryDataProvider.save({}, null, null, function () { callCheck = true; });
            httpBackend.flush();

            expect(callCheck).toBe(true);           
        });
    });

});