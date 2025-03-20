async function curtirPublicacao(id,btn) {
    try {
        shootFromButton(btn);
        let btnPub = document.getElementById(`descurtidas-${id}`).parentElement
        btn.disabled = true;
        btn.classList.add("btn-selecionado");
        btnPub.disabled = true
        const response = await fetch(`/Publicacao/Curtir/${id}`, {
            method: "POST",
            headers: { "Content-Type": "application/json" }
        });

        if (!response.ok) throw new Error("Erro ao curtir");

        const data = await response.json(); // Espera a resposta JSON
        document.getElementById(`curtidas-${id}`).innerText = data.novaQuantidade;
        document.getElementById(`curtidas-${id}`).disabled = true;
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
        document.getElementById(`descurtidas-${id}`).innerText = data.novaQuantidade;
        document.getElementById(`descurtidas-${id}`).disabled = true;

    } catch (error) {
        console.error("Erro ao descurtir:", error);
    }
}
