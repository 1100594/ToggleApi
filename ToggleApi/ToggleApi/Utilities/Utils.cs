using System;

namespace ToggleApi.Utilities
{
    internal static class Utils
    {
        public static void ThrowOnNullArgument(object parameterValue, string parameterName)
        {
            if (parameterValue == null)
                throw new ArgumentNullException(parameterName);
        }
    }
}
