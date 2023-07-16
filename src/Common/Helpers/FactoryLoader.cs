namespace Frequency.Common.Helpers;

public static class FactoryLoader {
    public static IEnumerable<T> LoadClassInstance<T>() {
        return typeof(T).Assembly
            .GetTypes()
            .Where(p => p.IsClass && p.IsAssignableTo(typeof(T)))
            .Select(Activator.CreateInstance)
            .Cast<T>();
    }
}