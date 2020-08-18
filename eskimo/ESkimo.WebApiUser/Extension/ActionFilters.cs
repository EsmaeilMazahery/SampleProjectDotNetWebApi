using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ESkimo.WebApiUser.Extension.ActionFilters
{
   public class ModelStateCheckActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
           if(!context.ModelState.IsValid){
               context.Result = new BadRequestObjectResult(context.ModelState);
           }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // do something after the action executes
        }
    }
}