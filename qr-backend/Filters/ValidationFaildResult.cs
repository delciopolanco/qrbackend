using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using qrbackend.Models.ViewModels;

namespace qr_backend.Filters
{
    public class ValidationFailedResult : ObjectResult
    {
        public ValidationFailedResult(ModelStateDictionary modelState)
            : base(new FrontStatusCode(modelState))
        {
            StatusCode = StatusCodes.Status400BadRequest;
        }
    }
}
