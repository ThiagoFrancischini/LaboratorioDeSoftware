using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LaboratorioDeSoftware.Filters
{
    public class AuthorizationFilter : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {            
            var usuarioId = context.HttpContext.Session.GetString("UsuarioId");
            
            if (string.IsNullOrEmpty(usuarioId))
            {
                context.Result = new RedirectToActionResult("Index", "Login", null);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}