class Chat {
    constructor(containerId, loadingId, canaisSelector, inputEnvioId, btnEnvioId) {
        this.container = document.getElementById(containerId);
        this.loading = document.getElementById(loadingId);
        this.canais = document.querySelectorAll(canaisSelector);
        this.inputEnvio = document.getElementById(inputEnvioId);
        this.btnEnvio = document.getElementById(btnEnvioId);
        this.userId = document.getElementById('userId');
        this.carteiraId = document.getElementById('carteiraId');
        this._grupoAtual = null;
        this.quantidade = 6;
        this.pagina = 0;
        this.carregando = false;
        this.scroll = false;
        this.carteiraSelect = document.getElementById("carteiraSelect");
        this.usuarioQualidade = document.getElementById("usuarioQualidade").value;
        this.nomeCanalAtual = "";


        this.inicializarSignalR();
        this.inicializarEventos();
        this.container.addEventListener("scroll", () => this.verificarScroll());

    }

    inicializarSignalR() {
        const connection = new signalR.HubConnectionBuilder()
            .withUrl("/chathub")
            .build();

        connection.on("Receber", (grupoMensagem, componenteHtml, model) => {

            if (model.carteira !== this.carteiraId.value && this.usuarioQualidade == 0) return;

            if (this._grupoAtual === grupoMensagem) {
                const wrapper = document.createElement("div");
                wrapper.innerHTML = componenteHtml;
                wrapper.classList.add("mensagem-whatsapp");

                const novaMensagem = wrapper.firstElementChild;

                if (model.idRemetente != this.userId.value) {
                    novaMensagem.classList.remove("enviada");
                    novaMensagem.classList.add("recebida");
                }

                this.container.appendChild(novaMensagem);
                this.container.scrollTop = this.container.scrollHeight;

            } else {
                const canalBtn = document.getElementById(grupoMensagem);
                if (canalBtn) {
                    let flag = canalBtn.querySelector('.nova-msg-flag');
                    if (flag) {
                        let qtd = parseInt(flag.innerText);
                        flag.innerText = qtd + 1;
                    } else {
                        const span = document.createElement('span');
                        span.classList.add('nova-msg-flag');
                        span.textContent = "1";
                        const nomeDiv = canalBtn.querySelector('.channel-name');
                        nomeDiv.appendChild(span);
                    }
                    const ultimaMensagem = canalBtn.querySelector('.channel-preview');
                    const horarioMensagem = canalBtn.querySelector('.channel-time');
                    ultimaMensagem.style.fontWeight = 'bold'
                    horarioMensagem.style.fontWeight = 'bold'
                    ultimaMensagem.textContent = model.mensagem;
                    horarioMensagem.textContent = formatarData(model.dataEnvio);
                }
            }
        });


        connection.on("MensagemPrivada", (user, messageHtml) => this.receberMensagem(messageHtml));

        connection.start()
            .then(() => console.log("Conectado ao SignalR"))
            .catch(err => console.error("Erro ao conectar ao SignalR:", err));
    }

    set grupoAtual(novoGrupo) {
        if (this._grupoAtual !== novoGrupo) {
            this._grupoAtual = novoGrupo;
            this.nomeCanalAtual = novoGrupo; 

            this.pagina = 0;
            this.scroll = false;
            this.container.innerHTML = "";

            this.carregarMensagens();
        }
    }
    set grupoAtual(novoGrupo) {
        if (this._grupoAtual !== novoGrupo) {
            this._grupoAtual = novoGrupo;

            this.pagina = 0;
            this.scroll = false;
            this.container.innerHTML = "";
            this.carregarMensagens();
        }
    }

    inicializarEventos() {
        this.canais.forEach(canal => {
            canal.addEventListener("click", () => {
                const ultimaMensagem = canal.querySelector('.channel-preview');
                const horarioMensagem = canal.querySelector('.channel-time');
                let canalNome = canal.querySelector('#chat-name-view').textContent;
                const flag = canal.querySelector('.nova-msg-flag');
                const grupoId = canal.dataset.group;
                this.grupoAtual = grupoId;
                let header = document.getElementById('chat-name');
                this.pagina = 0;
                ultimaMensagem.style.fontWeight = 'normal'
                horarioMensagem.style.fontWeight = 'normal'

                if (this.usuarioQualidade == 1 && this._grupoAtual === "Qualidade") {
                    this.carteiraSelect?.parentElement?.classList.remove("d-none");
                } else {
                    this.carteiraSelect?.parentElement?.classList.add("d-none");
                }

                if (flag) flag.remove();

                header.innerText = canalNome
                if (grupoId != this.grupoAtual) {
                    this.carregarMensagens();
                }
                this.container.scrollTop = this.container.scrollHeight;
            });
            if (this.carteiraSelect) {
                this.carteiraSelect.addEventListener("change", () => {
                    if (this._grupoAtual === "Qualidade" && this.usuarioQualidade) {
                        this.pagina = 0;
                        this.scroll = false;
                        this.container.innerHTML = "";
                        this.carregarMensagens();
                    }
                });
            }

        });

        if (this.btnEnvio) {
            this.btnEnvio.addEventListener("click", () => this.EnviarMensagem());
        }

        if (this.inputEnvio) {
            this.inputEnvio.addEventListener("keypress", (event) => {
                if (event.key === "Enter") {
                    event.preventDefault();
                    this.EnviarMensagem();
                }
            });
        }
    }

    async EnviarMensagem() {
        if (!this._grupoAtual) return;

        const conteudo = this.inputEnvio.value.trim();
        if (conteudo === "") return;

        let url = `/Mensagens/EnviarMensagem?grupo=${this._grupoAtual}&conteudo=${encodeURIComponent(conteudo)}`;

        if (this._grupoAtual === "Qualidade" && this.usuarioQualidade && this.carteiraSelect?.value) {
            url += `&carteira=${this.carteiraSelect.options[carteiraSelect.selectedIndex].text}`;
        }

        try {
            const response = await fetch(url, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                }
            });

            if (response.ok) {
                this.inputEnvio.value = "";
            }
        } catch (error) {
            console.error("Erro ao enviar mensagem:", error);
        }
    }

    async carregarMensagens(grupo = this._grupoAtual) {
        if (this.carregando) return;
        this.carregando = true;

        let url = `/Mensagens/ListarMensagens?quantidade=${this.quantidade}&pagina=${this.pagina}&grupo=${grupo}`;

        if (this._grupoAtual === "Qualidade" && this.usuarioQualidade && this.carteiraSelect?.value) {
            url += `&carteira=${this.carteiraSelect.value}`;
        }

        this.loading.style.display = "block";
        const scrollAntes = this.container.scrollHeight;

        try {

            if (!this.scroll) {
                this.container.innerHTML = ``;
            }

            const response = await fetch(url);
            const data = await response.json();

            if (data.length === 0 && this.pagina === 0) {
                this.container.innerHTML = `<div class="card-sem-publicacoes"><p>Nenhuma Mensagem ainda.</p></div>`;
                return;
            }

            data.forEach(msg => {
                const div = document.createElement("div");
                div.innerHTML = msg;
                div.classList.add("mensagem-renderizada");
                this.container.insertBefore(div, this.container.firstChild);
            });

            this.pagina += 6;

            const scrollDepois = this.container.scrollHeight;
            this.container.scrollTop += (scrollDepois - scrollAntes);

        } catch (error) {
            console.error("Erro ao carregar mensagens:", error);
        } finally {
            this.carregando = false;
            this.loading.style.display = "none";
            this.scroll = false;
        }
    }



    verificarScroll() {
        if (this.carregando || this.container.scrollTop !== 0) return;

        this.scroll = true;
        this.carregarMensagens(this._grupoAtual);
    }

}




