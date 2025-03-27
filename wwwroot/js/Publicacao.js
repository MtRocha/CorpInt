



let quantidade = 20
let pagina = 0
let paginaIndice = 0
let carregando = false; // Flag para evitar múltiplas chamadas
function ListaFeedPaginado(tipo) {
    let container = document.getElementById("pub-container");
    let loading = document.getElementById("loading");
 
    if (!container || !loading) return;
    // Exibe o loader
    loading.style.display = "block";
    fetch(`/Publicacao/ListaFeed?quantidade=${quantidade}&pagina=${pagina}&tipo=${tipo}`)
        .then(response => response.json())
        .then(data => {
                data.forEach(publicacao => {
                    let div = document.createElement("div");
                    div.style.width = '100%'
                    div.innerHTML = publicacao;

                    // Aplica animação apenas nos novos elementos
                    div.classList.add("Hidden-Frame");

                    container.appendChild(div);
                });

            carregando = false;

            pagina += 20; // Incrementa a página para carregar a próxima
            loading.style.display = "none";

        })
        .catch(error => {
            console.error("Erro ao carregar publicações:", error);
            loading.style.display = "none";
        });


}

let idParaExcluir = null;
function apagarPublicacao(id) {
    window.location.href = `\Excluir?pubId=${id}`;
}

function confirmarExclusao(id) {
    idParaExcluir = id;
    let modal = new bootstrap.Modal(document.getElementById('confirmDeleteModal'));
    modal.show();
}