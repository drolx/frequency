namespace Proton.Frequency.Common.Util;

public static class FactoryLoader
{
    public static IEnumerable<T> LoadClassInstance<T>()
    {
        return typeof(T).Assembly
            .GetTypes()
            .Where(p => p.IsClass && p.IsAssignableTo(typeof(T)))
            .Select(Activator.CreateInstance)
            .Cast<T>();
    }

    public static object Type { get; set; }
}
