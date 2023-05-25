using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;

namespace Gses.Services.Mail.ServiceLayer
{
	public interface IHtmlTemplateEngineerService
	{
		Task<string> RenderViewToStringAsync<TModel>(string templateName, TModel model);
	}

	public class HtmlTemplateEngineerService : IHtmlTemplateEngineerService
	{
		private readonly IRazorViewEngine _viewEngine;
		private readonly ITempDataProvider _tempDataProvider;
		private readonly IServiceProvider _serviceProvider;

		public HtmlTemplateEngineerService(
			IRazorViewEngine viewEngine,
			ITempDataProvider tempDataProvider,
			IServiceProvider serviceProvider)
		{
			_viewEngine = viewEngine;
			_tempDataProvider = tempDataProvider;
			_serviceProvider = serviceProvider;
		}

		public async Task<string> RenderViewToStringAsync<TModel>(string templateName, TModel model)
		{
			var actionContext = getActionContext();
			var viewName = $"~/views/emailtemplates/{templateName}.cshtml";
			var view = findView(actionContext, viewName);

			await using var output = new StringWriter();
			var viewContext = new ViewContext(actionContext, view,
				new ViewDataDictionary<TModel>(new EmptyModelMetadataProvider(), new ModelStateDictionary()) {
					Model = model
				},
				new TempDataDictionary(actionContext.HttpContext, _tempDataProvider), output,
				new HtmlHelperOptions());

			await view.RenderAsync(viewContext);
			return output.ToString();
		}

		private IView findView(ActionContext actionContext, string viewName)
		{
			var getViewResult = _viewEngine.GetView(null, viewName, true);
			if (getViewResult.Success)
			{
				return getViewResult.View;
			}

			var findViewResult = _viewEngine.FindView(actionContext, viewName, true);
			if (findViewResult.Success)
			{
				return findViewResult.View;
			}

			var searchedLocations = getViewResult.SearchedLocations.Concat(findViewResult.SearchedLocations);
			var errorMessage = string.Join(Environment.NewLine, new[] {
				$"Unable to find view '{viewName}'. The following locations were searched:"
			}.Concat(searchedLocations));

			throw new InvalidOperationException(errorMessage);
		}

		private ActionContext getActionContext()
		{
			var httpContext = new DefaultHttpContext { RequestServices = _serviceProvider };
			return new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
		}
	}
}
