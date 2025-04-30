class CommentsPaginado {
    constructor(sectionId, containerId, loadingId, pubId) {
        this.section = document.getElementById(sectionId);
        this.container = document.getElementById(containerId);
        this.loading = document.getElementById(loadingId);
        this.quantidade = 5;
        this.pagina = 0;
        this.carregando = false;
        this.pubId = pubId;
        this.commentView = 0;
        this.scroll = false;

        this.verificarScroll = this.verificarScroll.bind(this);
    }

    attachButton(btn) {
        if (btn) {
            btn.addEventListener('click', () => {
                if (this.commentView === 0) {
                    this.load();
                } else {
                    this.commentView = 0;
                    this.section.style.display = 'none';
                    this.container.removeEventListener("scroll", this.verificarScroll);
                }
            });
        }
    }

    verificarScroll() {
        if (this.carregando) return;

        let posicaoScroll = this.container.scrollTop + this.container.clientHeight;
        let alturaTotal = this.container.scrollHeight;

        if (posicaoScroll >= alturaTotal * 0.7) {
            requestAnimationFrame(() => {
                setTimeout(() => {
                    this.scroll = true;
                    this.load(); // chama o load incremental
                }, 300);
            });
        }
    }

    async load() {
        this.section.style.display = 'block';
        this.commentView = 1;
        if (!this.scroll) {
            this.pagina = 0;
            this.container.innerHTML = '';
        }

        if (this.carregando) return;
        this.carregando = true;
        this.loading.style.display = 'block';

        const url = `/Comentario/ListaComentarios?pubId=${this.pubId}&quantidade=${this.quantidade}&pagina=${this.pagina}`;
        try {
            this.container.style.display = 'flex';

            const resp = await fetch(url);
            if (!resp.ok) throw new Error('Erro ao carregar comentários');
            const htmlArray = await resp.json();

            if (htmlArray.length === 0 && this.pagina === 0) {
                this.container.innerHTML = `
                    <div class="card-sem-publicacoes">
                        <p>Nenhum comentário ainda.</p>
                    </div>`;
            } else {
                htmlArray.forEach(html => {
                    const div = document.createElement('div');
                    div.innerHTML = html;
                    this.container.appendChild(div);
                });

                this.pagina += 5;

                // Inicia o listener de scroll após o primeiro load
                if (!this.scroll) {
                    this.container.addEventListener('scroll', this.verificarScroll);
                }
            }
        } catch (e) {
            console.error(e);
            if (this.pagina === 0) {
                this.container.innerHTML = `<p class="text-danger">Falha ao carregar comentários.</p>`;
            }
        } finally {
            this.carregando = false;
            this.scroll = false;
            this.loading.style.display = 'none';
        }
    }
}
