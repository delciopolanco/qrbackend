using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using qr_backend.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qr_backend.Filters
{
    public class AuthenticationFilter: ActionFilterAttribute
    {
        private StringValues xyz;

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            var authHeader = actionContext.HttpContext.Request.Headers.TryGetValue("Authorization", out xyz);
            var jwt = (xyz.Count() > 0 ? xyz.ToString().Split(" ")[1] : "");

            actionContext.RouteData.Values.Add("jwtData", JwtManager.GetDataFromJWT(jwt));

            base.OnActionExecuting(actionContext);
        }
    }
}
