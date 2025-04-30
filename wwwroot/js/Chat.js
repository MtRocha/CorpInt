class MensagensPaginadas {
    constructor(containerId, loadingId, filtros) {
        this.container = document.getElementById(containerId);
        this.loading = document.getElementById(loadingId);
        this.filtros = filtros || {}; // pode conter { termo, data }
        this.quantidade = 10;
        this.pagina = 0;
        this.carregando = false;
        this.scroll = false;

        this.inicializarEventos();
        this.carregarMensagens();

        this.container.addEventListener("scroll", () => this.verificarScroll());
    }

    inicializarEventos() {
        if (this.filtros.termo) {
            this.filtros.termo.addEventListener("input", () => this.filtrarMensagens());
        }
        if (this.filtros.data) {
            this.filtros.data.addEventListener("change", () => this.filtrarMensagens());
        }
    }

    verificarScroll() {
        if (this.carregando) return;

        const posicaoScroll = this.container.scrollTop + this.container.clientHeight;
        const alturaTotal = this.container.scrollHeight;

        if (posicaoScroll >= alturaTotal * 0.9) {
            this.scroll = true;
            this.carregarMensagens();
        }
    }

    filtrarMensagens() {
        this.pagina = 0;
        this.scroll = false;
        this.container.innerHTML = "";
        this.carregarMensagens();
    }

    carregarMensagens() {
        if (this.carregando) return;
        this.carregando = true;

        const termo = this.filtros.termo?.value || "";
        const data = this.filtros.data?.value || "";

        const url = `/Mensagens/ListaMensagens?quantidade=${this.quantidade}&pagina=${this.pagina}&conteudo=${termo}&data=${data}`;

        this.loading.style.display = "block";

        fetch(url)
            .then(response => response.json())
            .then(data => {
                if (data.length === 0 && this.pagina === 0) {
                    this.container.innerHTML = `<p class="no-messages">Nenhuma mensagem encontrada.</p>`;
                    return;
                }

                data.forEach(msg => {
                    const div = document.createElement("div");
                    div.innerHTML = msg;
                    div.classList.add("mensagem-renderizada");
                    this.container.appendChild(div);
                });

                this.pagina += this.quantidade;
            })
            .catch(error => console.error("Erro ao carregar mensagens:", error))
            .finally(() => {
                this.carregando = false;
                this.loading.style.display = "none";
                this.scroll = false;
            });
    }
}
