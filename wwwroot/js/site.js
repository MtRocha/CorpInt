function activateButton(button) {
    // Remove a classe "active" de todos os botões
    document.querySelectorAll(".menu-btn").forEach(btn => btn.classList.remove("active"));

    // Adiciona a classe "active" no botão clicado
    button.classList.add("active");
}