using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using System.Text.RegularExpressions;

namespace ToggleApi.Controllers.Attributes
{
    public class QueryStringConstraintAttribute : ActionMethodSelectorAttribute
    {
        public override bool IsValidForRequest(RouteContext routeContext, ActionDescriptor action)
        {
            var path = routeContext.HttpContext.Request.Path;
            return new Regex(@"/api/toggles/([0-9a-zA-Z_-]*)=(true|false)$").IsMatch(path);
        }
    }
}
