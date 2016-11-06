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
    }));

    afterEach(function () {
        httpBackend.verifyNoOutstandingExpectation();
        httpBackend.verifyNoOutstandingRequest();
    });

    it("is injected", function () {
        expect(registryDataProvider).toBeDefined();
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
    
    describe("validateData()", function () {
        it("should exists", function () {
            expect(registryDataProvider.validateData).toBeDefined();
            expect(typeof(registryDataProvider.validateData)).toBe("function");
        });

        describe("when date is undefined", function () {
            it('should raise Error("Date must be specified")', function () {
                var data = {};
                data.categoryId = 1;
                data.amount = 1;
                data.description = "aaa";
                expect(function () { registryDataProvider.validateData(data) }).toThrowError("Date must be specified");
            });
        });

        describe("when categoryId is not specified", function () {
            it('should raise Error("Category must be specified")', function () {
                var data = {};
                data.date = moment().toDate();
                data.amount = 1;
                data.description = "aaa";
                expect(function () { registryDataProvider.validateData(data) }).toThrow();
                expect(function () { registryDataProvider.validateData(data) }).toThrowError("Category must be specified");
            });
        });

        describe("when amount is undefined", function () {            
            it('should raise Error("Amount must be specified")', function(){
                var data = {};
                data.date = moment().toDate();
                data.categoryId = 1;
                data.description = "aaa";
                expect(function () { registryDataProvider.validateData(data) }).toThrowError("Amount must be specified");
            });
        });

        describe("when amount is zero", function () {
            describe("and operation is Transfer", function () {
                it("should not raise an error", function () {
                    var data = {};
                    data.date = moment().toDate();
                    data.categoryId = 1;
                    data.operation = registryDataProvider.OPERATION_TYPE_TRANSFER;
                    data.amount = 0;
                    data.description = "aaa";
                    expect(function () { registryDataProvider.validateData(data) }).not.toThrow();
                });
            });

            describe("and operation is not Transfer", function () {
                it("should raise an error", function () {
                    var data = {};
                    data.date = moment().toDate();
                    data.categoryId = 1;
                    data.operation = registryDataProvider.OPERATION_TYPE_INBOUND;
                    data.amount = 0;
                    data.description = "aaa";
                    expect(function () { registryDataProvider.validateData(data) }).toThrow();
                    expect(function () { registryDataProvider.validateData(data) }).toThrowError("Amount must be specified");
                });
            });
        });

        describe("when description is empty", function () {
            it('should raise Error("Description cannot be empty")', function () {
                var data = {};
                data.date = moment().toDate();
                data.categoryId = 1;
                data.operation = registryDataProvider.OPERATION_TYPE_INBOUND;
                data.amount = 1;
                expect(function () { registryDataProvider.validateData(data) }).toThrow();
                expect(function () { registryDataProvider.validateData(data) }).toThrowError("Description cannot be empty");
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