async function curtirPublicacao(id, btn) {
    try {
        shootFromButton(btn);
        btn.classList.add("btn-selecionado");
        const response = await fetch(`/Publicacao/Curtir/${id}`, {
            method: "POST",
            headers: { "Content-Type": "application/json" }
        });

        if (!response.ok) throw new Error("Erro ao curtir");

        const data = await response.json(); // Espera a resposta JSON
        jackpotAnimarNumero(`curtidas-${id}`, data.novaQuantidade);
        btn.disabled = true;
    } catch (error) {
        console.error("Erro ao curtir:", error);
    }
}

async function descurtirPublicacao(id,btn) {
    try {
        let btnPub = document.getElementById(`curtidas-${id}`).parentElement
        btn.disabled = true;
        btn.classList.add("btn-selecionado");
        btnPub.disabled = true

        const response = await fetch(`/Publicacao/Descurtir/${id}`, {
            method: "POST",
            headers: { "Content-Type": "application/json" }
        });

        if (!response.ok) throw new Error("Erro ao descurtir");

        const data = await response.json();

        jackpotAnimarNumero(`descurtidas-${id}`, data.novaQuantidade);
        document.getElementById(`descurtidas-${id}`).disabled = true;

    } catch (error) {
        console.error("Erro ao descurtir:", error);
    }
}

async function FavoritarDashboard(id, btn) {
    try {
        shootFromButton(btn);
        let icon = btn.querySelector('i')
        fetch(`/PowerBI/Favoritar?id=${id}`, {
            method: "POST",
        }).then(response => response.json())
          .then(data => {

            if (data.favorito === 1) {
                icon.classList.remove("far");
                icon.classList.add("fas");
            }
            else {
                icon.classList.remove("fas");
                icon.classList.add("far");
            }

        })
    }
    catch (error) {
        console.error("Erro ao favoritar dashboard:", error);
    }
}

async function EnviarComentario(id, btn) {
    const inputEl = document.getElementById(`input-comentar-${id}`);
    const erroEl = document.getElementById(`erro-comentar-${id}`);
    const conteudo = inputEl.value;

    btn.disabled = true;
    erroEl.innerHTML = ""; // limpa os erros anteriores

    try {
        const response = await fetch(`/Comentario/Comentar?id=${id}&conteudo=${encodeURIComponent(conteudo)}`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            }
        });

        if (response.ok) {
            let container = document.getElementById(`comentarios-container-${id}`)
            const previousScrollHeight = container.scrollHeight;
            container.scrollTop = container.scrollHeight - previousScrollHeight * 2;
            inputEl.value = "";
        } else {
            const erroData = await response.json();
            if (erroData.erros) {
                let html = "<ul class='mb-0'>";
                for (const mensagens of Object.values(erroData.erros)) {
                    mensagens.forEach(msg => {
                        html += `<li>${msg}</li>`;
                    });
                }
                html += "</ul>";
                erroEl.innerHTML = html;
            } else {
                erroEl.textContent = "Erro ao enviar comentário. Tente novamente.";
            }
        }
    } catch (error) {
        console.error("Erro ao comentar:", error);
        erroEl.textContent = "Erro inesperado. Verifique sua conexão.";
    } finally {
        btn.disabled = false;
    }
}




///////////////////////// ESCUTADORES PARA INTERAÇÃO EM TEMPO REAL ///////////////////////   

const connectionReactions = new signalR.HubConnectionBuilder()
    .withUrl("/reactionhub") // deve bater com o endpoint mapeado no `MapHub`
    .build();

// Evento para curtida em tempo real
connectionReactions.on("AtualizarCurtida", (idPublicacao, novaQuantidade) => {
    const curtidaSpan = document.getElementById(`curtidas-${idPublicacao}`);
    if (curtidaSpan) {
        jackpotAnimarNumero(`curtidas-${idPublicacao}`, novaQuantidade);
    }
});

// Evento para descurtida em tempo real
connectionReactions.on("AtualizarDescurtida", (idPublicacao, novaQuantidade) => {
    const descurtidaSpan = document.getElementById(`descurtidas-${idPublicacao}`);
    if (descurtidaSpan) {
        jackpotAnimarNumero(`descurtidas-${idPublicacao}`, novaQuantidade);
    }
});

connectionReactions.on("NovoComentario", (pubId, novaQuantidade, componenteHtml) => {
    // 1. Atualiza e anima o contador
    const contador = document.getElementById(`count-com-${pubId}`);
    if (contador) {
        jackpotAnimarNumero(`count-com-${pubId}`, novaQuantidade);
    }

    // 2. Adiciona o novo comentário no topo da lista
    const lista = document.getElementById(`comentarios-container-${pubId}`);
    if (lista) {
        const div = document.createElement("div");
        div.innerHTML = componenteHtml;
        lista.insertBefore(div, lista.firstChild);
    }
});


// Inicia a conexão
connectionReactions.start().catch(err => console.error(err.toString()));