class PowerBI {
    constructor(containerId,containerFavId, loadingId, filtros) {
        this.container = document.getElementById(containerId);
        this.containerFav = document.getElementById(containerFavId);
        this.loading = document.getElementById(loadingId);
        this.filtros = filtros; 
        this.acesso = document.getElementById('tipoAcesso').value;
        this.formEdicao = document.getElementById('editForm');
        if (this.container) {
            this.init();
        }
    }

    init() {
        this.inicializarEventos();
        this.ListaDashboards();
    }

    inicializarEventos() {
        if (!this.filtros) return;

        this.filtros.termo?.addEventListener("input", () => this.verificarFiltroTermo(true));
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

    verificarFiltroTermo() {
        if (this.filtros.termo.value.trim() === "") {
            this.ListaDashboards(true);
        } else {
            this.ListaDashboards(false);
        }
    }

    ListaDashboards() {

        let termo = this.filtros.termo?.value || "";

        let url = `/PowerBI/ListarDashBoards?conteudo=${termo}`;;
        
        this.loading.style.display = "block";

        fetch(url)
            .then(response => response.json())
            .then(data => {
                this.loading.style.display = "none";
                this.container.innerHTML = ''
                this.containerFav.innerHTML = ''

                if (data.length === 0 && termo === "") {
                    this.container.innerHTML = `
                        <div class="card-sem-publicacoes">
                            <i class="fas fa-exclamation-circle"></i>
                            <p>Você não possui Dashboards Liberados.</p>
                        </div>`;
                    return;
                }

                if (data.length === 0 && termo != "") {
                    this.containerFav.style.display = 'none'
                    this.container.innerHTML = `
                        <div class="card-sem-publicacoes">
                            <i class="fas fa-exclamation-circle"></i>
                            <p>Nenhum Dashboard Encontrado.</p>
                        </div>`;
                    return;
                }

                data.forEach(dash => {
                    let div = document.createElement("div");
                    div.innerHTML = dash;

                    let btn = div.querySelector(".btn-editar")
                    if (btn != null) {

                        div.querySelector(".btn-editar").addEventListener("click", () => {
                            this.PrepararParaEdicao(div.querySelector(".btn-editar").dataset.id);
                        });

                    }

                    let favorito = div.querySelector("p")
                    let section = document.getElementById("fav-section")

                    if (favorito.textContent === "1")
                    {
                        this.containerFav.appendChild(div);
                    }
                    else
                    {
                        this.container.appendChild(div);
                    }
                });

                let items = this.containerFav.querySelectorAll(".dashboard-card");
                if (items.length === 0) {
                    this.containerFav.style.justifyContent = "center";
                    this.containerFav.innerHTML = `
                        <div class="card-sem-publicacoes">
                            <p>Você não possui Dashboards Favoritados.</p>
                        </div>`;
                } 

            })
            .catch(error => {
                this.loading.style.display = "none";
                console.error("Erro ao carregar DashBoards:", error);
            });
     
    }
}
