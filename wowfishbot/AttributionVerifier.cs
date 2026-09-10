using System.Reflection;

namespace wowfishbot;

internal static class AttributionVerifier
{
    public static bool HasExpectedIdentity()
    {
        Assembly assembly = typeof(AttributionVerifier).Assembly;
        AssemblyCompanyAttribute? company = assembly.GetCustomAttribute<AssemblyCompanyAttribute>();
        AssemblyProductAttribute? product = assembly.GetCustomAttribute<AssemblyProductAttribute>();
        AssemblyDescriptionAttribute? description = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>();

        return string.Equals(company?.Company, ProjectIdentity.Publisher, StringComparison.Ordinal) &&
               string.Equals(product?.Product, ProjectIdentity.ProductName, StringComparison.Ordinal) &&
               string.Equals(description?.Description, ProjectIdentity.Attribution, StringComparison.Ordinal);
    }
}
