// https://egghead.io/lessons/angularjs-providers

/*
  
Requires:
   moment.js
  
Usage:
app.config( function(utilsProvider){
	utilsProvider.language
});

 */


app.factory("utils", function utilsFactory () {
	console.log("set utils provider");
	var provider = {};

	provider.toShortDate = function(date) {
		if (!date) return "";
		return moment(date).format('L');
	};

	provider.getDate = function(date) {
		if (!date) return null;
		return moment(date, "L").toDate();
	};

	//provider.$get: function() {
	//			culture: _culture,
	//			toShortDate: function(date) {
	//				if (!date) return "";
	//				return moment(date).format('L');
	//			},
	//			getDate: function(date) {
	//				if (!date) return null;
	//				return moment(date, "L").toDate();
	//			}
	//		};
	//	}

	//};

	return provider;

});

