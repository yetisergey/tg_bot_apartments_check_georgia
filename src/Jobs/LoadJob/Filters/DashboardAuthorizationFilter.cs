using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace LoadJob.Filters;

public class DashboardAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize([NotNull] DashboardContext context)
    {
        return true;
    }
}
