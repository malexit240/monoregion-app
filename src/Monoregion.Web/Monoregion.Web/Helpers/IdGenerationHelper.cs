namespace Monoregion.Web.Helpers
{
    public static class IdGenerationHelper
    {
        public static string GetNext() => Guid.NewGuid().ToString("N");
    }
}
