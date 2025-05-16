document.addEventListener('DOMContentLoaded', function () {
    // Inicializar máscaras
    // Import jQuery if it's not already available
    if (typeof jQuery == 'undefined') {
        var script = document.createElement('script');
        script.src = "https://ajax.googleapis.com/ajax/libs/jquery/3.7.1/jquery.min.js";
        script.type = 'text/javascript';
        script.onload = function () {
            // Initialize masks after jQuery is loaded
            jQuery('#cpf').mask('000.000.000-00');
            jQuery('#recoveryCpf').mask('000.000.000-00');
        };
        document.getElementsByTagName('head')[0].appendChild(script);
    } else {
        jQuery('#cpf').mask('000.000.000-00');
        jQuery('#recoveryCpf').mask('000.000.000-00');
    }

    // Animações de entrada
    const anime = window.anime;

    // Animação do texto vertical
    anime({
        targets: '.vertical-text.roveri',
        opacity: [0, 1],
        translateY: [20, 0],
        easing: 'easeOutExpo',
        duration: 1000,
        delay: 300
    });

    anime({
        targets: '.vertical-text.news',
        opacity: [0, 1],
        translateY: [20, 0],
        easing: 'easeOutExpo',
        duration: 1000,
        delay: 500
    });

    // Animação do logo
    anime({
        targets: '.logo-container',
        opacity: [0, 1],
        scale: [0.9, 1],
        easing: 'easeOutExpo',
        duration: 1200,
        delay: 700
    });

    // Animação do mascote
    anime({
        targets: '.mascot-container',
        opacity: [0, 1],
        translateX: [50, 0],
        easing: 'easeOutExpo',
        duration: 1200,
        delay: 900
    });

    // Animação do formulário (FadeRight)
    anime({
        targets: '.login-form-container',
        opacity: [0, 1],
        translateX: [30, 0],
        easing: 'easeOutExpo',
        duration: 1000,
        delay: 1100
    });

    // Validação do formulário de login
    const loginForm = document.getElementById('loginForm');
    if (loginForm) {
        loginForm.addEventListener('submit', function (event) {
            if (!loginForm.checkValidity()) {
                event.preventDefault();
                event.stopPropagation();
            }

            loginForm.classList.add('was-validated');
        });
    }

    // Validação do formulário de recuperação
    const recoveryForm = document.getElementById('recoveryForm');
    const recoverPasswordBtn = document.getElementById('recoverPasswordBtn');

    if (recoveryForm && recoverPasswordBtn) {
        recoverPasswordBtn.addEventListener('click', function (event) {
            const newPassword = document.getElementById('newPassword').value;
            const confirmPassword = document.getElementById('confirmPassword').value;

            // Verificar se as senhas coincidem
            if (newPassword !== confirmPassword) {
                document.getElementById('confirmPassword').setCustomValidity('As senhas não coincidem');
            } else {
                document.getElementById('confirmPassword').setCustomValidity('');
            }

            if (!recoveryForm.checkValidity()) {
                event.preventDefault();
                event.stopPropagation();
            } else {
                // Aqui você enviaria o formulário via AJAX ou outra lógica
                // Por enquanto, apenas fechamos o modal

                // Check if bootstrap is defined
                if (typeof bootstrap !== 'undefined') {
                    const modal = bootstrap.Modal.getInstance(document.getElementById('forgotPasswordModal'));
                    modal.hide();

                    // Mostrar mensagem de sucesso (pode ser substituído por um modal)
                    alert('Solicitação de recuperação enviada com sucesso!');
                } else {
                    console.error('Bootstrap is not defined. Ensure Bootstrap is properly loaded.');
                    alert('Erro ao processar a solicitação. Bootstrap não carregado.');
                }
            }

            recoveryForm.classList.add('was-validated');
        });
    }

    // Mostrar/ocultar senha
    const togglePassword = document.querySelector('.toggle-password');
    if (togglePassword) {
        togglePassword.addEventListener('click', function () {
            const senhaInput = document.getElementById('senha');
            const type = senhaInput.getAttribute('type') === 'password' ? 'text' : 'password';
            senhaInput.setAttribute('type', type);

            // Alternar ícone
            const icon = this.querySelector('i');
            icon.classList.toggle('fa-eye');
            icon.classList.toggle('fa-eye-slash');
        });
    }

    // Animações para os modais
    const modals = document.querySelectorAll('.modal');
    modals.forEach(modal => {
        modal.addEventListener('show.bs.modal', function () {
            const modalContent = this.querySelector('.modal-content');

            anime({
                targets: modalContent,
                opacity: [0, 1],
                scale: [0.9, 1],
                easing: 'easeOutExpo',
                duration: 300
            });
        });
    });

    // Animações para botões
    const buttons = document.querySelectorAll('.btn');
    buttons.forEach(button => {
        button.addEventListener('mouseenter', function () {
            anime({
                targets: this,
                scale: 1.05,
                duration: 200,
                easing: 'easeOutQuad'
            });
        });

        button.addEventListener('mouseleave', function () {
            anime({
                targets: this,
                scale: 1,
                duration: 200,
                easing: 'easeOutQuad'
            });
        });
    });

    // Check if bootstrap is defined, if not, load it.
    if (typeof bootstrap === 'undefined') {
        var bootstrapLink = document.createElement('link');
        bootstrapLink.rel = 'stylesheet';
        bootstrapLink.href = 'https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css';
        document.head.appendChild(bootstrapLink);

        var bootstrapScript = document.createElement('script');
        bootstrapScript.src = 'https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js';
        document.body.appendChild(bootstrapScript);
    }
});