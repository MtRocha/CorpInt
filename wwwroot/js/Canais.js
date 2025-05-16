const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub") // certifique-se de que a rota está correta
    .build();



connection.on("Receber", (user, messageHtml) => {
    const container = document.getElementById("chat-messages-container");

    if (container) {
        const div = document.createElement("div");
        div.innerHTML = messageHtml;
        container.appendChild(div);
        container.scrollTop = container.scrollHeight; 
    }
});

connection.start()
    .then(() => console.log("Conectado ao SignalR"))
    .catch(err => console.error("Erro ao conectar ao SignalR:", err));

