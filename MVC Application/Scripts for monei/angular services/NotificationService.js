app.factory("NotificationService", function () {

    var service = {};

    service.info = function (message) {
        console.info(message);
    };

    service.warn = function (message) {
        console.warn(message);
    };

    service.error = function (message) {
        console.error(message);
    };

    return service;
});