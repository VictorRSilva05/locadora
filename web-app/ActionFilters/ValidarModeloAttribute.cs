using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Locadora.WebApp.ActionFilters;

public class ValidarModeloAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.Controller is not Controller controller)
            return;

        var modelstate = context.ModelState;

        var viewModel = context.ActionArguments.Values
            .FirstOrDefault(x => x?.GetType().Name.EndsWith("ViewModel") == true);

        if (!modelstate.IsValid && viewModel is not null)
            context.HttpContext.Items["ModelStateInvalid"] = true;
    }
}
