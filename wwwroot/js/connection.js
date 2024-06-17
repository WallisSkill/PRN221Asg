const connection = new signalR.HubConnectionBuilder()
    .withUrl("/hub")
    .withAutomaticReconnect([0, 0, 10000])
    .build();
connection.start().catch(err => console.error(err.toString()));
