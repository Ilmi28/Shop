using Microsoft.AspNetCore.Authorization;
using Shop.Requirements;
using Shop.Models;
using System.Security.Claims;

namespace Shop.RequirementsHandlers
{
    public class ProductOwnerRequirementHandler : AuthorizationHandler<ProductOwnerRequirement, Product>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ProductOwnerRequirement requirement, Product product)
        {
            if (product.Creator == context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
