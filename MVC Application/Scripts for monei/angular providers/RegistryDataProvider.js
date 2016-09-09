app.factory("RegistryDataProvider",
["$http",
function ($http) {

    var baseUrl = "/api/registry";
    var headers = { "account-guid": "00000000-0000-0000-0000-000000000000" };
    var provider = {};

    provider.search = function(filters, callback, errorCallback, finallyCallback) {
        $http.post(baseUrl + "/search", filters)
            .success(callback)
            .error(function(error, status) { errorCallback && errorCallback(error) })
            .finally(function() { finallyCallback && finallyCallback() });
    };

    provider.save = function (record, callback, errorCallback, finallyCallback) {
        $http.post(baseUrl, record, {"headers": headers})
            .success(function (data) { callback && callback(data); })
            .error(function (error, status) { errorCallback && errorCallback(error); })
            .finally(function () { finallyCallback && finallyCallback(); })
        ;
    };
    
    return provider;
}]);