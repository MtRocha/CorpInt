

const defaults = {
    spread: 270,
    ticks: 60,
    gravity: 0.01,
    decay: 0.96,
    startVelocity: 5,
    shapes: ['star'], // Usa o formato padrão para evitar erros
    colors: ['ff6600'],
    scalar: 1
};

function shootFromButton(button) {
    const rect = button.getBoundingClientRect();
    const originX = (rect.left + rect.width / 2) / window.innerWidth;
    const originY = (rect.top + rect.height / 2) / window.innerHeight;

    // Dispara confetes normais
    confetti({
        ...defaults,
        particleCount: 30,
        origin: { x: originX, y: originY }
    });

    // Dispara confetes pequenos
    confetti({
        ...defaults,
        particleCount: 10,
        scalar: 1,
        spread: 180,
        shapes: ["star"],
        origin: { x: originX, y: originY }
    });

}


/**
 * Anima um elemento em estilo caça-níquel (slot machine),
 * girando verticalmente até parar em `novoValor`.
 */
function jackpotAnimarNumero(id, novoValor) {
    const wrapper = document.getElementById(id);
    if (!wrapper) return;

    // Valor antigo e criação do rolo de números
    const container = document.createElement('span');
    container.style.display = 'block';

    // Sequência de números aleatórios + valor final
    let html = '';
    for (let i = 0; i < 12; i++) {
        html += `<span>${Math.floor(Math.random() * ((novoValor * 5) - novoValor)) + novoValor}</span>`;
    }
    html += `<span>${novoValor}</span>`;
    container.innerHTML = html;

    // Substitui o conteúdo e insere o rolo
    wrapper.innerHTML = '';
    wrapper.appendChild(container);

    // Calcula o deslocamento vertical até o último valor
    const altura = wrapper.offsetHeight;
    const total = container.children.length;
    const desloc = -(altura * (total - 1));

    // Animação com velocidade constante e duração menor
    anime({
        targets: container,
        translateY: desloc,
        duration: 300,       // velocidade mais rápida
        easing: 'linear',    // sem aceleração ou desaceleração
        complete: () => {
            // Ao final, escreve só o valor exato
            wrapper.innerText = novoValor;
        }
    });
}


