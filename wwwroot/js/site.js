function activateButton(button) {

    document.querySelectorAll(".menu-btn").forEach(btn => btn.classList.remove("active"));
    button.classList.add("active");
}


function limparFormulario(id) {
    const form = document.getElementById(id);
    if (!form) return;

    // Limpar inputs de texto e arquivos
    form.querySelectorAll('input').forEach(input => {
        if (input.type === 'file' || input.type === 'text') {
            input.value = '';
        }
    });

    // Limpar selects
    form.querySelectorAll('select').forEach(select => {
        select.selectedIndex = 0;
    });

    // Limpar textareas
    form.querySelectorAll('textarea').forEach(textarea => {
        textarea.value = '';
    });
}

