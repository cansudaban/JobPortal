using JobPortal.Common.Resources;
using System.Resources;

namespace JobPortal.Common.Helpers
{
    public static class ResourceHelper
    {
        private static readonly ResourceManager ResourceManager = new ResourceManager(typeof(CommonResource));

        public static string GetMessage(string key)
        {
            return ResourceManager.GetString(key);
        }

        public static string RequiredField => ResourceManager.GetString("RequiredField");
        public static string InvalidEmail => ResourceManager.GetString("InvalidEmail");
        public static string MaxLength => ResourceManager.GetString("MaxLength");
        public static string OperationSuccess => ResourceManager.GetString("OperationSuccess");
        public static string OperationFailed => ResourceManager.GetString("OperationFailed");

    }
}
