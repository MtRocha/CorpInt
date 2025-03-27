class FiltroDinamico {
    constructor(containerSelector, filtroSelector, filtrosConfig = {}, btnLimparSelector = "#btnLimparFiltros") {
        this.container = document.querySelector(containerSelector);
        this.filtros = document.querySelectorAll(filtroSelector);
        this.filtrosConfig = filtrosConfig;
        this.btnLimpar = document.querySelector(btnLimparSelector);

        if (!this.container) {
            console.error(`FiltroDinamico: O container "${containerSelector}" não foi encontrado.`);
            return;
        }

        this.inicializarFiltros();
        this.inicializarBotaoLimpar();
    }

    inicializarFiltros() {
        this.filtros.forEach(filtro => filtro.addEventListener("input", () => this.aplicarFiltro()));
    }

    inicializarBotaoLimpar() {
        if (this.btnLimpar) {
            this.btnLimpar.addEventListener("click", () => this.limparFiltros());
        } else {
            console.warn("⚠️ Botão de limpar filtros não encontrado.");
        }
    }

    aplicarFiltro() {
        const valoresFiltro = {};

        // Captura os valores preenchidos nos filtros
        this.filtros.forEach(filtro => {
            if (filtro.value.trim() !== "") {
                valoresFiltro[filtro.id] = filtro.value.toLowerCase().trim();
            }
        });

        this.container.querySelectorAll(".item-filtravel").forEach(item => {
            let visivel = false; // Assume que o item será visível até que um filtro o esconda
                
            for (const filtroId in valoresFiltro) {
                const atributo = (item.dataset[filtroId] || "").toLowerCase().trim();
                const filtroValor = valoresFiltro[filtroId];
                const comparacao = this.filtrosConfig[filtroId]?.comparacao || "igual";

                if (comparacao === "igual" && atributo == filtroValor) {
                    visivel = true;
                    break; // Se um filtro falhar, o item será ocultado
                }
                if (comparacao === "contem" && atributo.includes(filtroValor)) {
                    visivel = true;
                    break;
                }
            }

            if (visivel) {
                item.classList.remove("filtrado"); // Removemos a classe de ocultação
            } else {
                item.classList.add("filtrado"); // Aplicamos a classe de ocultação
            }
        });
    }

    limparFiltros() {
        this.filtros.forEach(filtro => {
            if (filtro.tagName.toLowerCase() === "select") {
                filtro.selectedIndex = 0; // Retorna o select para a primeira opção
            } else {
                filtro.value = "";
            }
        });

        this.container.querySelectorAll(".item-filtravel").forEach(item => {
            item.classList.remove("filtrado"); // Exibe todos os itens novamente
        });

        console.log("🔄 Filtros resetados!");
    }
}
