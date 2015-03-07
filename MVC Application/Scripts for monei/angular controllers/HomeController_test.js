"use strict";
/// Jasmine tests
//describe("monei module", function() {

	var scope, controller;
	beforeEach(function () {
		angular.module('monei');
	});

	describe('HomeController', function () {

		var appName = app.name; // this "initialize" module, all other way give an error
		beforeEach(angular.mock.module(appName)); // this is needed to make injection of providers works
		
		beforeEach(inject(function ($rootScope, $controller) {
			scope = $rootScope.$new();
			controller = $controller('HomeController', {
				'$scope': scope
			});
		}));

		it("loginFailCounter must be 0 and increase after login fail", function() {
			expect(scope.loginFailCounter).toBe(0);
			scope.loginFail("some error occurs");
			expect(scope.loginFailCounter).toBe(1);
		});

		it("loggedIn function exists", function() {
			expect(scope.loggedIn).toBeDefined();
		});

		//it('watches the name and updates the counter', function () {
		//	expect(scope.counter).toBe(0);
		//	scope.name = 'Batman';
		//	scope.$digest();
		//	expect(scope.counter).toBe(1);
		//});
	});


//});