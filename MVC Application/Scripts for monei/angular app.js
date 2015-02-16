// Angular

var app = angular.module("monei", []);

// doesn't be called, I don't know why...
//app.config(function (utilsProvider) {
//	alert(3);
//	utilsProvider.setLanguage("it");

//});

// doesn't be called, I don't know why...
/*app.config(function ($routeProvider) {
	alert("config $routeProvider");
});*/

app.config(function($httpProvider){

});



var _scope; // for debug purpose