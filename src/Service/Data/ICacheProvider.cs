using Frequency.Common.Entity;

namespace Frequency.Data;

public interface ICacheProvider {
    Task<IEnumerable<Category>?> GetCachedCategory();
}