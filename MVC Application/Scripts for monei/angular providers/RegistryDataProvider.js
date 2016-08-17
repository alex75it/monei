app.factory("RegistryDataProvider",
["$http", "utils",
function ($http, utils) {

    var baseUrl = "/api/registry";
    var provider = {};

    var getHttpData = function () {
        var data = {};
        data.headers = {};
        data.headers["account-guid"] = utils.getAccountGuid();
        return data;
    };

    provider.search = function(filters, callback, errorCallback, finallyCallback) {
        $http.post(baseUrl + "/search", filters)
            .success(callback)
            .error(function(error, status) { errorCallback && errorCallback(error) })
            .finally(function() { finallyCallback && finallyCallback() });
    };

    provider.save = function (record, callback, errorCallback, finallyCallback) {
        $http.post(baseUrl, record, getHttpData())
            .success(function (data) { callback && callback(data); })
            .error(function (error, status) { errorCallback && errorCallback(error); })
            .finally(function () { finallyCallback && finallyCallback(); })
        ;
    };
    
    return provider;
}]);