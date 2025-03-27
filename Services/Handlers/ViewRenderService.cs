using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.IO;
using System.Threading.Tasks;

namespace Intranet_NEW.Services.Handlers
{
    public interface IViewRenderService
    { 
        Task<string> RenderToStringAsync(Controller controller, string viewName, object model);
    }
    public class ViewRenderService : IViewRenderService
    {
        private readonly ICompositeViewEngine _viewEngine;
        private readonly IServiceProvider _serviceProvider;
        private readonly ITempDataProvider _tempDataProvider;

        public ViewRenderService(ICompositeViewEngine viewEngine, IServiceProvider serviceProvider, ITempDataProvider tempDataProvider)
        {
            _viewEngine = viewEngine;
            _serviceProvider = serviceProvider;
            _tempDataProvider = tempDataProvider;
        }


        public async Task<string> RenderToStringAsync(Controller controller,string viewName, object model)
        {
            controller.ViewData.Model = model;

            using (StringWriter writer = new())
            { 
            
                var viewResult = _viewEngine.FindView(controller.ControllerContext, viewName, false);

                if (!viewResult.Success)
                {
                    throw new InvalidOperationException($"A view {viewName} não foi encontrada");
                }

                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View,controller.ViewData, new TempDataDictionary(controller.HttpContext, _tempDataProvider), writer,new HtmlHelperOptions());

                await viewContext.View.RenderAsync(viewContext);

                return writer.ToString();

            }

            
        }



    }
}
