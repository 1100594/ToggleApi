using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ToggleApi.Properties;

namespace ToggleApi.Utilities
{
    public static class ContollerBaseExtensions
    {
        internal static IActionResult InternalServerError(this ControllerBase objectResult, string message = null)
        {
            message = message ?? Resources.InternalErrorMessage;
            return objectResult.StatusCode((int)HttpStatusCode.InternalServerError, message);
        }

        internal static IActionResult NotAllowed(this ControllerBase objectResult, string message = null)
        {
            message = message ?? Resources.NotAllowedErrorMessage;
            return objectResult.StatusCode((int)HttpStatusCode.MethodNotAllowed, message);
        }
    }
}
