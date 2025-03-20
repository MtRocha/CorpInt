function createEmojiImage(emoji, size = 50) {
    const canvas = document.createElement("canvas");
    const ctx = canvas.getContext("2d");
    canvas.width = size;
    canvas.height = size;

    // Define o tamanho e o alinhamento do emoji
    ctx.font = `${size * 0.8}px serif`;
    ctx.textAlign = "center";
    ctx.textBaseline = "middle";
    ctx.fillText(emoji, size / 2, size / 2);

    // Retorna uma função que gera a imagem
    return function () {
        const img = new Image();
        img.src = canvas.toDataURL();
        return img;
    };
}

// Criando o emoji 🦄 como textura de confete
const unicornShape = createEmojiImage("🦄");

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
