using System;
using System.IO;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ToggleApi.Utilities
{
    internal static class Utils
    {
        public static void ThrowOnNullArgument(object parameterValue, string parameterName)
        {
            if (parameterValue.IsNull())
                throw new ArgumentNullException(parameterName);
        }

        public static void ThrowOnNullArgument(string value)
        {
            if(value.IsNull())
                throw new ArgumentNullException(paramName: nameof(value));
        }

        public static void ThrowInvalidData(string message)
        {
            throw new InvalidDataException(message);
        }
            
    }
}
