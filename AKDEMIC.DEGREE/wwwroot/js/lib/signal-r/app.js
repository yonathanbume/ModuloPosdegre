var connection = new signalR
    .HubConnectionBuilder()
    .withUrl(_app.constants.hubs.akdemic.clientProxy.url)
    .build();

connection.on(_app.constants.hubs.akdemic.clientProxy.method, function (text, stateText, readDate, isRead, id, backgroundStateClass, url, sound) {
    Notifications.push({ text, stateText, readDate, isRead, id, backgroundStateClass, url, sound });
    Notifications.load_slopes();
});

connection
    .start()
    .then(function () { });
