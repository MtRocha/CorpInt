using FluentValidation;
using Intranet_NEW.Models.WEB;

namespace Intranet_NEW.Services.Validadores
{
    public class PublicacaoValidator : AbstractValidator<PublicacaoModel>
    {

        public PublicacaoValidator() {


            RuleSet("ValidarInsercao", () => {
            
                 RuleFor(x => x.Titulo).NotNull().WithMessage("A publicação precisa de um Titulo");
                 RuleFor(x => x.Titulo).MinimumLength(1).WithMessage("A publicação precisa de um Titulo");
                 RuleFor(x => x.Conteudo).NotNull().WithMessage("A publicação precisa de Conteúdo");
                 RuleFor(x => x.Conteudo).MinimumLength(1).WithMessage("A publicação precisa de Conteúdo");
            });
                
        
        
        }

    }
}
