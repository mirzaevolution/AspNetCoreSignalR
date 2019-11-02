/// <reference path="../../lib/signalr/dist/browser/signalr.min.js" />
var connection = new signalR.HubConnectionBuilder().withUrl("/ChatHub").build();

var signalRBuilder = function () {

    //because the server keep alive interval is 1 minute
    //so the client server timeout in ms must be set to 2 minutes
    connection.serverTimeoutInMilliseconds = 120000;

    connection.on("ReceiveMessage", function (user, message) {
        var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
        var loggedUser = $("#loggedUser").val();
        var boxColor = "bg-primary";
        if (user.toLowerCase() == loggedUser.toLowerCase()) {
            boxColor = "bg-danger";
        }
        var userItem = "<span class='font-weight-bold text-warning'>" + user + ": </span><br>";
        var msgItem = "<span class='text-light'>" + msg + "</span>";
        var element = "<p class='p-2 text-wrap " + boxColor + "'>" + userItem + msgItem + "</p>";
        $("#chatBox").append(element);
    });
    connection.onclose(function () {
        $("#buttonSend").prop("disabled", true);
        $("#buttonSendTimeNotif").prop("disabled", true);
        console.log("Reconnecting in 3 seconds...");
        setTimeout(function () {

            connection.start().then(function () {
                $("#buttonSend").prop("disabled", false);
                $("#buttonSendTimeNotif").prop("disabled", false);
            }).catch(function (err) {
                alert("An error occured. Cannot connect to web socket server");
                console.log(err);
            });

        }, 2000);
    });
    connection.start().then(function () {
        $("#buttonSend").prop("disabled", false);
        $("#buttonSendTimeNotif").prop("disabled", false);
    }).catch(function (err) {
        alert("An error occured. Cannot connect to web socket server");
        console.log(err);
    });
    $("#buttonSend").click(function () {
        var msg = $("#messageBox").val();
        if (msg == "" || msg == null) {
            alert("Message cannot be empty!");
        } else {
            var loggedUser = $("#loggedUser").val();
            var message = $("#messageBox").val();
            $("#messageBox").val("");
            connection.invoke("SendMessage", loggedUser, message).catch(function (err) {
                alert("An error occured while sending the message");
                console.log(err);
            });
        }
    });
    $("#buttonSendTimeNotif").click(function () {
        var url = "/SignalR/InvokeTimeNotification";
        $.ajax({
            url: url,
            type: "POST",
            success: function () {
                console.log(url + " was invoked successfully");
            },
            error: function () {
                console.log("Error invoking " + url);
            }
        })
    });
};

var userPromptDialog = function () {
    var userName = "";
    do {
        userName = prompt("Input your name:");
    } while (userName === "" || userName == null);
    $("#loggedUser").val(userName);
    $("#userName").text(userName);
};
$(document).ready(function () {
    signalRBuilder();
    userPromptDialog();
});