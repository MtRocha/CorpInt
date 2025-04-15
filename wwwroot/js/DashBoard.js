class PowerBI {
    constructor(containerId, loadingId) {
        this.container = document.getElementById(containerId);
        this.loading = document.getElementById(loadingId);
        this.acesso = document.getElementById('tipoAcesso').value;
        this.formEdicao = document.getElementById('editForm');
        if (this.container) {
            this.init();
        }
    }

    init() {
        this.ListaDashboards();
    }

    PrepararParaEdicao(id) {
        fetch(`/PowerBI/PrepararParaEdicao?id=${id}`)
            .then(response => response.json())
            .then(data => {
                if (!data) return;
                console.log(data)
                // Preenche os campos do formulário de edição
                document.getElementById("edit-id").value = data.model.id;
                document.getElementById("titulo").value = data.model.titulo;
                document.getElementById("link").value = data.model.link;
                document.getElementById("descricao").value = data.model.descricao;
                document.getElementById("carteiraSelect").value = data.model.tipoAcesso;
                document.getElementById("intAtt").value = data.model.intervaloAtualizacao;


                // Mostra o modal de edição
                $('#editModal').modal('show'); 
            })
            .catch(error => console.error("Erro ao buscar dados para edição:", error));
    }



    ListaDashboards() {
        let url = `/PowerBI/ListarDashBoards`;
        this.loading.style.display = "block";

        fetch(url)
            .then(response => response.json())
            .then(data => {
                this.loading.style.display = "none";

                if (data.length === 0) {
                    this.container.innerHTML = `
                        <div class="card-sem-publicacoes">
                            <i class="fas fa-exclamation-circle"></i>
                            <p>Você não possui Dashboards Liberados.</p>
                        </div>`;
                    return;
                }

                data.forEach(dash => {
                    let div = document.createElement("div");
                    div.style.width = "100%";
                    div.innerHTML = dash;
                    div.classList.add("animated-dashboard");

                    div.querySelector(".btn-editar").addEventListener("click", () => {
                        this.PrepararParaEdicao(div.querySelector(".btn-editar").dataset.id);
                    });

                    this.container.appendChild(div);
                });

                this.animarDashboards();
            })
            .catch(error => {
                this.loading.style.display = "none";
                console.error("Erro ao carregar DashBoards:", error);
            });
    }
}
