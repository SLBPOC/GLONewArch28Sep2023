using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Delfi.Glo.Api.Middleware
{
    [AttributeUsage(AttributeTargets.All)]
    public class ApiAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext authorizationFilterContext)
        {
            _ = authorizationFilterContext ?? throw new ArgumentNullException(nameof(authorizationFilterContext));
            bool isAuthenticationEnabled = false;
            string? strIsAuthenticationRequired = Environment.GetEnvironmentVariable("IsAuthenticationRequired");
            if (strIsAuthenticationRequired != null &&
                   bool.Parse(strIsAuthenticationRequired))
                bool.TryParse(strIsAuthenticationRequired, out isAuthenticationEnabled);

            if (isAuthenticationEnabled)
            {
                if (!authorizationFilterContext.HttpContext.User.Identity!.IsAuthenticated)
                {
                    authorizationFilterContext.Result = new UnauthorizedResult();
                }
            }
        }
    }
}
