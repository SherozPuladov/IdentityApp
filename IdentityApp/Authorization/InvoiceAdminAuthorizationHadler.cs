using IdentityApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Net.Mime;

namespace IdentityApp.Authorization
{
    public class InvoiceAdminAuthorizationHadler
        : AuthorizationHandler<OperationAuthorizationRequirement, Invoice>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            OperationAuthorizationRequirement requirement, 
            Invoice invoice)
        {
            if(context == null || invoice == null)
                return Task.CompletedTask;

            if (context.User.IsInRole(Constants.InvoiceAdminsRole))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
