using FluentValidation;
using Intranet_NEW.Models.WEB;

namespace Intranet_NEW.Services.Validadores
{
    public class ComentarioValidator : AbstractValidator<ComentarioModel>
    {
        public ComentarioValidator() {

            RuleFor(x => x.Conteudo)
                .NotEmpty().WithMessage("O campo conteúdo é obrigatório.")
                .MinimumLength(5).WithMessage("O Comentário deve ter pelo menos 5 caracteres.")
                .MaximumLength(500).WithMessage("O Comentário deve ter no máximo 500 caracteres.");

        }
    }
}
