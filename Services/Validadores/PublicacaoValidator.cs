using FluentValidation;
using Intranet_NEW.Models.WEB;

namespace Intranet_NEW.Services.Validadores
{
    public class PublicacaoValidator : AbstractValidator<PublicacaoModel>
    {

        public PublicacaoValidator() {
            RuleSet("ValidarInsercao", () => {
            
                 RuleFor(x => x.Titulo).MinimumLength(1).WithMessage("A publicação precisa de um Titulo");

            });
                
        
        
        }

    }
}
