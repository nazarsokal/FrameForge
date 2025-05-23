const connection = new signalR.HubConnectionBuilder()
    .withUrl("/battleHub")
    .withAutomaticReconnect()
    .build();
document.addEventListener('DOMContentLoaded', () => {
    
    connection.start()
        .then(() => {
            console.log("Connected to hub");
        })
        .catch(err => console.error(err));

    try {
        await connection.invoke("GetStudent", roomId);


    } catch (err) {
        console.error(err);
    }


});