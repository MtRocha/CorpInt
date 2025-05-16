

async function AlterarSenhaUsuario()
{
    event.preventDefault();

    if ($('#senha').val() !== $('#confirmacaoSenha').val()) {
        $('#divError').show();
        $('#divErrorMsg').text('As senhas não coincidem.');
        $('#divMensagem').show();
        return false;
    }
    $('#divError').hide();
    $('#divMensagem').show();

    // Desabilitar o formulário após envio
    $('#formRecuperarSenha input, #formRecuperarSenha button').prop('disabled', true);


    const form = document.getElementById("formRecuperarSenha");
    let divMensagem = document.getElementById("divMensagem");
    let divErro = document.getElementById("divError");
    let divSucesso = document.getElementById("divSuccess");
    let divErroMsg = document.getElementById("divErrorMsg");
    const formData =
    {
        NR_CPF: document.getElementById("cpf").value,
        DT_NASCIMENTO : document.getElementById("dataNascimento").value,
        NM_SENHA : document.getElementById("senha").value,
        NM_CONFIRMACAO_SENHA : document.getElementById("confirmacaoSenha").value
    }

    if (formData.NM_SENHA != formData.NM_CONFIRMACAO_SENHA) {
        divErro.style.display = "block";
        divMensagem.style.display = "flex";
        divErroMsg.innerText = "As Senhas devem ser iguais para que a alteração possa ser realizada";
        return;
    }
    else {

        const response = await fetch("/Login/ResetarSenha",
            {
                method: "POST",
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(formData)
        })
        .then(response => response.json())
        .then(data => {
            if (data.erro) {
                divErroMsg.innerText = data.erro;
                divErro.style.display = "block";
                divMensagem.style.display = "flex";
            }
            else  {
                divMensagem.style.display = "flex";
                form.style.display = "none";
                divErro.style.display = "none";
                divSucesso.style.display = "block";
            }
        })
        .catch(error => console.error("Erro:", error));




    }

}

    async function LoginApi() {
        const formData = new FormData();

        const response = await fetch('/Login/LoginCopilot', {
            method: 'GET',
        });

        if (!response.ok) {
            alert("Erro ao logar");
            return;
        }

        const data = await response.json();

        localStorage.setItem("token", data.token);
        localStorage.setItem("usuario", data.nome);

        // Redireciona
        window.location.href = "http://copilotroveri.grupo.roveri/home";
    }