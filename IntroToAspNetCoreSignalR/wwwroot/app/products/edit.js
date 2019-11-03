var initSignalR = function () {
    var connection = new signalR.HubConnectionBuilder().withUrl("/NotifyHub").build();
    connection.on("ReceiveUpdateNotification", function (message,token, productId) {
        var existingToken = $("#Token").val();
        var existingProductId = $("#Id").val();
        if (existingToken != token && existingProductId == productId.toString()) {
            alert(message);
            console.log(message);
            location.reload();
        }
    });
    connection.on("ReceiveDeleteNotification", function (message, token, productId) {
        var existingToken = $("#Token").val();
        var existingProductId = $("#Id").val();

        if (existingToken != token && existingProductId == productId.toString()) {
            alert(message);
            console.log(message);
            location.reload();
        }


    });
    connection.onclose(function () {
        setTimeout(function () {
            console.log("Reconnecting in 3 seconds...");
            connection.start().catch(function (err) {
                alert("An error occured. Cannot connect to web socket server");
                console.log(err);
            });

        }, 2000);
    });
    connection.start().catch(function (err) {
        alert("An error occured. Cannot connect to web socket server");
        console.log(err);
    });
};
$(document).ready(function () {
    initSignalR();
});