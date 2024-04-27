$(document).ready(function () {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("https://tu-servidor/hardwareHub") // Reemplaza con la URL de tu servidor y el nombre del hub
        .build();

    connection.on("ReceiveHardwareInfo", function (data) {
        // Aquí recibes los datos del hardware y los muestras en la página
        $("#hardwareInfo").html(`<pre>${JSON.stringify(data, null, 2)}</pre>`);
    });

    connection.start()
        .then(function () {
            console.log("Conexión establecida con el servidor SignalR");
        })
        .catch(function (err) {
            console.error("Error al conectar con el servidor SignalR:", err);
        });
});

