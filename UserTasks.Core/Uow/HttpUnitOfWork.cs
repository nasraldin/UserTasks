using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Http;
using UserTasks.Core.Data;

namespace UserTasks.Core.Uow
{
    public class HttpUnitOfWork : UnitOfWork
    {
        public HttpUnitOfWork(AppDbContext context, IHttpContextAccessor httpAccessor) : base(context)
        {
            context.CurrentUserId = httpAccessor.HttpContext.User.FindFirst(OpenIdConnectConstants.Claims.Subject)?.Value?.Trim();
        }
    }
}
