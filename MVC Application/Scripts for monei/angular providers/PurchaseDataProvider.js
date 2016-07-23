app.factory("PurchaseDataProvider",
["$http",
function PurchaseDataProvider($http) {
    var baseUrl = "/api/purchase";
    var provider = {};


    provider.save = function (data, callback, errorCallback, finallyCallback) {
        $http.post(baseUrl, data)
            .success(function (data) { callback && callback(data); })
            .error(function (data, status) { errorCallback && errorCallback(data); })
            .finally(function () { finallyCallback && finallyCallback(); })
        ;
    };

    return provider;
}]);
