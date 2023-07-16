using Proton.Frequency.Common.Entity;

namespace Proton.Frequency.Data;

public interface ICacheProvider {
    Task<IEnumerable<Category>?> GetCachedCategory();
}