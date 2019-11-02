var initDataTables = {
    init: function () {
        var table = $("#productTable").DataTable({
            ajax: "/Products/GetAll",
            columns: [
                { data: "name" },
                { data: "description" },
                {
                    data: "price", render: function (data, type, row) {
                        return "<span class='text-danger'>$" + data + "</span>";
                    }
                },
                {
                    data: "id",
                    render: function (data, type, row) {
                        return "<a class='btn btn-primary btn-xs' href='/Products/Edit/" + data + "'>Edit</a>";
                    }
                }
            ]
        });

    },
    refresh: function () {
        var table = $('#productTable').DataTable();
        table.destroy();
        this.init();
    }
};
var initSignalR = function () {
    var connection = new signalR.HubConnectionBuilder().withUrl("/NotifyHub").build();
    connection.on("ReceiveNotification", function (message, type) {
        //Type
        //1 = Add
        //2 = Update
        //3 = Delete
        console.log(message);
        initDataTables.refresh();

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
    initDataTables.init();
    initSignalR();
});