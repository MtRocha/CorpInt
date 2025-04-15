using FluentValidation;
using Intranet_NEW.Models.WEB;

namespace Intranet_NEW.Services.Validadores
{
    public class PowerBiValidator : AbstractValidator<PowerBiModel>
    {

        public PowerBiValidator() {
        
            RuleSet("ValidarInsercao", () =>
            {

                RuleFor(x => x.Link).Matches(@"^https:\/\/app\.powerbi\.com\/view\?[a-zA-Z0-9=]{1,}").WithMessage("O link que você digitou não foi reconhecido como um link válido do Power BI.");

            });


        }

    }
}
