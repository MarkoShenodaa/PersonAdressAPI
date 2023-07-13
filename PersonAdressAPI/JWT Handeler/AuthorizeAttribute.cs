global using Microsoft.AspNetCore.Mvc.Filters;

namespace PersonAdressAPI.JWT_Handeler;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = (Person)context.HttpContext.Items["Person"];
        if (user == null)
        {
            // not logged in
            context.Result = new JsonResult(new { message = "No Login Data Found!" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}