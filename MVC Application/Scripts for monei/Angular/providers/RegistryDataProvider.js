app.factory("RegistryDataProvider",
["$http", "apiToken", "utils",
function ($http, apiToken, utils) {
   
    var baseUrl = "/api/registry";
    var headers = { api_token: apiToken };
    var provider = {};

    provider.OPERATION_TYPE_TRANSFER = "Transfer";
        
    provider.search = function(filters, callback, errorCallback, finallyCallback) {
        $http.post(baseUrl + "/search", filters)
            .success(callback)
            .error(function(error, status) { errorCallback && errorCallback(error) })
            .finally(function() { finallyCallback && finallyCallback() });
    };

    provider.validateData = function (data)
    {
        if (!data) throw Error("Data is null");        
        if (!data.date) throw Error("Date must be specified");
        if (!data.categoryId) throw new Error("Category must be specified");
        if ((!data.amount || data.amount == 0) && data.operation != provider.OPERATION_TYPE_TRANSFER) throw Error("Amount must be specified");
        if (!data.description || data.description.length == 0) throw Error("Description cannot be empty");
    }

    provider.save = function (record, callback, errorCallback, finallyCallback) {
        $http.post(baseUrl, record, {"headers": headers})
            .success(function (data) { callback && callback(data); })
            .error(function (error, status) { errorCallback && errorCallback(error); })
            .finally(function () { finallyCallback && finallyCallback(); })
        ;
    };
    
    return provider;
}]);