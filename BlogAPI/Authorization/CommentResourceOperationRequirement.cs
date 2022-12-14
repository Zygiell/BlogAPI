using BlogAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BlogAPI.Authorization
{
    public class CommentResourceOperationRequirement : AuthorizationHandler<ResourceOperationRequirement, Comment>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, Comment comment)
        {
            if (requirement.ResourceOperation == ResourceOperation.Read)
            {
                context.Succeed(requirement);
            }

            var userId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var userRole = context.User.FindFirst(c => c.Type == ClaimTypes.Role).Value;

            if (comment.CreatedByUserId == int.Parse(userId) || userRole == "Admin")
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}