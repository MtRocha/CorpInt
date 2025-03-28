class FeedPaginado {
    constructor(containerId, loadingId, filtros) {
        this.container = document.getElementById(containerId);
        this.loading = document.getElementById(loadingId);
        this.filtros = filtros; // Objeto com os filtros { tipo, termo, data }
        this.quantidade = 5;
        this.pagina = 0;
        this.carregando = false;

        this.inicializarEventos();
        // Chama a primeira carga de publicações
        this.ListaFeedPaginado();

        // Adiciona o evento de scroll ao container
        this.container.addEventListener("scroll", () => this.verificarScroll());
    }

    inicializarEventos() {
        if (!this.container || !this.loading) {
            console.error("Elementos necessários não encontrados.");
            return;
        }

        if (!this.filtros) return;

        // Ativa a filtragem automática
        this.filtros.tipo?.addEventListener("change", () => this.listar(true));
        this.filtros.termo?.addEventListener("input", () => this.verificarFiltroTermo());
        this.filtros.data?.addEventListener("change", () => this.verificarFiltroData());
        this.filtros.limpar?.addEventListener("click", () => this.limparFiltros());
    }

    verificarScroll() {
        if (this.carregando) return; // Evita múltiplas chamadas

        let posicaoScroll = this.container.scrollTop + this.container.clientHeight;
        let alturaTotal = this.container.scrollHeight;

        // Quando o scroll estiver a 90% da altura total, carrega mais posts
        if (posicaoScroll >= alturaTotal * 0.9) {
            requestAnimationFrame(() => {
                setTimeout(() => {
                    this.ListaFeedPaginado(false);
                }, 300); // Aguarda 300ms para evitar múltiplas chamadas
            });
        }
    }

    verificarFiltroTermo() {
        if (this.filtros.termo.value.trim() === "") {
            this.listar(false);
        }
        else {
            this.listar(true);
        }
    }

    verificarFiltroData() {
        if (!this.filtros.data.value) {
            this.listar(false);
        }
        else {
            this.listar(true);
        }
    }

    listar(resetPagina = false) {
        if (resetPagina) {
            this.pagina = 0; // Resetar a página para 0
            this.container.innerHTML = ""; // Limpar o conteúdo atual
        }
        this.ListaFeedPaginado(resetPagina);
    }

    ListaFeedPaginado(resetPagina = false) {
        if (this.carregando) return;
        this.carregando = true;

        if (!this.container || !this.loading) return;

        if (resetPagina) {
            this.pagina = 0;
            this.container.innerHTML = "";
        }

        // Obtendo valores dos filtros
        let tipo = this.filtros.tipo?.value.trim() || "";
        let termo = this.filtros.termo?.value || "";
        let data = this.filtros.data?.value || "";

        // Construindo a URL da requisição
        let url = `/Publicacao/ListaFeed?quantidade=${this.quantidade}&pagina=${this.pagina}`;
        if (tipo || termo || data) {
            url += `&tipo=${tipo}&conteudo=${termo}&data=${data}`;
        }

        // Exibindo o loader
        this.loading.style.display = "block";

        fetch(url)
            .then(response => response.json())
            .then(data => {
                if (data.length === 0 && this.pagina === 0) {
                    console.log("Nenhuma publicação encontrada.");
                    this.container.innerHTML = "<p>Nenhuma publicação encontrada.</p>";
                    return;
                }

                data.forEach(publicacao => {
                    let div = document.createElement("div");
                    div.style.width = "100%";
                    div.innerHTML = publicacao;
                    div.classList.add("Hidden-Frame"); // Animação nos novos elementos
                    this.container.appendChild(div);
                });

                if (!resetPagina) {
                    this.pagina += 5;
                }
                this.carregando = false;
            })
            .catch(error => console.error("Erro ao carregar publicações:", error))
            .finally(() => {
                this.carregando = false;
                this.loading.style.display = "none";
            });
    }

    limparFiltros() {
        if (this.filtros.tipo) this.filtros.tipo.selectedIndex = 0;
        if (this.filtros.termo) this.filtros.termo.value = "";
        if (this.filtros.data) this.filtros.data.value = "";

        this.listar(true);
    }
}
