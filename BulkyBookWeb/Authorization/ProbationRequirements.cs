using Microsoft.AspNetCore.Authorization;

namespace BulkyBookWeb.Authorization
{
    public class ProbationRequirement : IAuthorizationRequirement
    {
        public ProbationRequirement(int probationMonths)
        {
            ProbationMonths = probationMonths;
        }
        public int ProbationMonths { get; }
    }

    public class ProbationRequirementHandler : AuthorizationHandler<ProbationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ProbationRequirement requirement)
        {
            if (!context.User.HasClaim(x => x.Type == "EmploymentDate"))
                return Task.CompletedTask;
            var empDate = DateTime.Parse(context.User.FindFirst(x => x.Type == "EmploymentDate").Value);
            var period = DateTime.Now - empDate;
            if (period.Days > 30 * requirement.ProbationMonths)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }

}
