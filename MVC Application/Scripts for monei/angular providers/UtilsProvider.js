// https://egghead.io/lessons/angularjs-providers

/*
  
Requires:
   moment.js
  
Usage:
app.config( function(utilsProvider){
    utilsProvider.language
});

*/


app.factory("utils", function () {
    
    var accountGuid = null;

    var provider = {};

    provider.setAccountGuid = function (guid) {
        accountGuid = guid;
    };

    provider.getAccountGuid = function () {
        if (!accountGuid)
            throw Error("AccountGuid not set");
        return accountGuid;
    };

    provider.toShortDate = function(date) {
        if (!date) return "";
        return moment(date).format('L');
    };

    provider.getDate = function(date) {
        if (!date) return null;
        return moment(date, "L").toDate();
    };

    return provider;
});

