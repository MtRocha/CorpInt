﻿class FeedPaginado {
    constructor(containerId, loadingId, filtros) {
        this.container = document.getElementById(containerId);
        this.loading = document.getElementById(loadingId);
        this.filtros = filtros; // Objeto com os filtros { tipo, termo, data }
        this.quantidade = 5;
        this.pagina = 0;
        this.carregando = false;

        this.inicializarEventos();
        this.ListaFeedPaginado();

        this.container.addEventListener("scroll", () => this.verificarScroll());
    }

    inicializarEventos() {
        if (!this.container || !this.loading) {
            console.error("Elementos necessários não encontrados.");
            return;
        }

        if (!this.filtros) return;

        this.filtros.tipo?.addEventListener("change", () => this.listar(false)); // Mantém a página
        this.filtros.termo?.addEventListener("input", () => this.verificarFiltroTermo());
        this.filtros.data?.addEventListener("change", () => this.verificarFiltroData());
        this.filtros.limpar?.addEventListener("click", () => this.limparFiltros());
    }

    verificarScroll() {
        if (this.carregando) return;

        let posicaoScroll = this.container.scrollTop + this.container.clientHeight;
        let alturaTotal = this.container.scrollHeight;

        if (posicaoScroll >= alturaTotal * 0.9) {
            requestAnimationFrame(() => {
                setTimeout(() => {
                    this.ListaFeedPaginado();
                }, 300);
            });
        }
    }

    verificarFiltroTermo() {
        if (this.filtros.termo.value.trim() === "") {
            this.listar(true); // Reseta página se termo for apagado
        } else {
            this.listar(false);
        }
    }

    verificarFiltroData() {
        if (!this.filtros.data.value) {
            this.listar(true); // Reseta página se data for apagada
        } else {
            this.listar(false);
        }
    }

    listar(resetPagina = false) {
        if (resetPagina) {
            this.pagina = 0; // Resetar página quando necessário
            this.container.innerHTML = ""; // Limpar conteúdo
        }
        this.ListaFeedPaginado();
    }

    ListaFeedPaginado() {
        if (this.carregando) return;
        this.carregando = true;

        if (!this.container || !this.loading) return;

        // Obtendo valores dos filtros
        let tipo = this.filtros.tipo?.value.trim() || "";
        let termo = this.filtros.termo?.value || "";
        let data = this.filtros.data?.value || "";

        // Construindo a URL da requisição
        let url = `/Publicacao/ListaFeed?quantidade=${this.quantidade}&pagina=${this.pagina}`;
        url += `&tipo=${tipo}&conteudo=${termo}&data=${data}`;

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
                    div.classList.add("Hidden-Frame");
                    this.container.appendChild(div);
                });

                this.pagina += 5; // Continua de onde parou

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

        this.listar(true); // Agora reseta corretamente
    }
}
