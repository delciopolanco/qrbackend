using Microsoft.AspNetCore.Mvc.Filters;


namespace qr_backend.Filters
{
    public class ValidateModelFilter : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new ValidationFailedResult(context.ModelState);
            }
        }
    }
}
