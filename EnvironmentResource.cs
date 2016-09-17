using System;
using System.Linq;
using System.Reflection;

namespace FrameworkInternalResource
{
    public static class EnvironmentResource
    {
        private const BindingFlags ReflectionFilter = BindingFlags.Static | BindingFlags.NonPublic;
        private static readonly Func<object, object[], object> GetResourceString = (from m in typeof(Environment).GetMethods(ReflectionFilter)
                                                                where m.Name == "GetResourceString"
                                                                select m).First().Invoke;

        public static string GetString(string key, string cultureName)
        {
            var ci = new System.Globalization.CultureInfo(cultureName);
            var thread = System.Threading.Thread.CurrentThread;
            var originalCulture = thread.CurrentCulture;
            var originalUICulture = thread.CurrentUICulture;
            thread.CurrentCulture = ci;
            thread.CurrentUICulture = ci;

            var value = (string)GetResourceString(null, new object[] { key });

            thread.CurrentUICulture = originalUICulture;
            thread.CurrentCulture = originalCulture;
            return value;
        }
    }
}
