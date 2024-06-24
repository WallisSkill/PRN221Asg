const connection = new signalR.HubConnectionBuilder()
    .withUrl("/hub")
    .withAutomaticReconnect([0, 0, 10000])
    .build();
connection.start().then(function () {
    connection.invoke("GetOnlineUsers");
}).catch(err => console.error(err.toString()));
