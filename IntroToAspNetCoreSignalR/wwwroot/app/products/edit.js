var initSignalR = function () {
    var connection = new signalR.HubConnectionBuilder().withUrl("/NotifyHub").build();
    connection.on("ReceiveUpdateNotification", function (message, type, token) {
        //Type
        //1 = Add
        //2 = Update
        //3 = Delete
        var existingToken = $("#Token").val();
        if (existingToken != token) {
            if (parseInt(type) == 2) {
                alert(message);
            } else if (parseInt(type) == 3) {
                alert(message);
            }
            console.log(message);

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