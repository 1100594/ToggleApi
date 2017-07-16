namespace ToggleApi.Utilities
{
    public static class ObjectExtensions
    {
        internal static bool IsNull(this object originalObject)
        {
            return originalObject == null;
        }
    }
}
