using Company_X.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

public class SuperAdminRequirement : IAuthorizationRequirement
{
}

public class SuperAdminHandler : AuthorizationHandler<SuperAdminRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SuperAdminHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SuperAdminRequirement requirement)
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext.Items["UserSession"] is UserSession session && session.Level == "Superadmin")
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
    