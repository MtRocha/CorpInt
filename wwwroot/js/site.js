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

function formatarData(dataString) {
    const data = new Date(dataString);

    const dia = String(data.getDate()).padStart(2, '0');
    const mes = String(data.getMonth() + 1).padStart(2, '0'); // Janeiro é 0
    const ano = data.getFullYear();

    const horas = String(data.getHours()).padStart(2, '0');
    const minutos = String(data.getMinutes()).padStart(2, '0');

    return `${dia}/${mes}/${ano} ${horas}:${minutos}`;
}