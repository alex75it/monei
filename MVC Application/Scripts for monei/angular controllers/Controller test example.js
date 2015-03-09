// Jasmine test
"use strict";


// example of controller test
app.controller('NavCtrl', function ($scope, $location) {
	$scope.isActive = function (route) {
		return route === $location.path();
	};
});

describe('NavCtrl', function () {
	var scope, $location, createController;

	beforeEach(module(app.name)); // it not works without "beforeEach", simply call "module(app.name);" does not works.

	beforeEach(inject(function ($rootScope, $controller) {
		$location = _$location_;
		scope = $rootScope.$new();
		
		createController = function () {
			return $controller('NavCtrl', {
				'$scope': scope
			});
		};
	}));

	it("must have been defined", function () {
		expect(createController).toBeDefined();
	});

	it('should have a method to check if the path is active', function () {
		var controller = createController();
		$location.path('/about');
		expect($location.path()).toBe('/about');
		expect(scope.isActive('/about')).toBe(true);
		expect(scope.isActive('/contact')).toBe(false);
	});
});