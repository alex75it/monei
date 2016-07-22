app.factory("PurchaseProvider",
["$http",
function PurchaseProvider($http) {
    var baseUrl = "/api/purchase/";
    var provider = {};


    provider.save = function (data, callback, errorCallback, finallyCallback) {
        $http.get(baseUrl + "list")
            .success(function (data) { callback(data); })
            .error(function (data, status) { errorCallback && errorCallback(data); })
            .finally(function () { finallyCallback && finallyCallback(); });
    };

    return provider;
}]);