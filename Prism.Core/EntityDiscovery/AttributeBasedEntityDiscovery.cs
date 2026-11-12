using System.Reflection;
using Prism.Core.Attributes;

namespace Prism.Core.EntityDiscovery
{
    /// <summary>
    /// Scans an assembly at runtime and yields every type decorated with <see cref="PrismTableAttribute"/>
    /// that is not also decorated with <see cref="PrismIgnoreAttribute"/>.
    /// Uses a lazy iterator (<c>yield return</c>) so types are surfaced one at a time without
    /// materialising the full set upfront.
    /// </summary>
    public class AttributeBasedEntityDiscovery : IEntitySearcher
    {
        public IEnumerable<Type> GetMappableTypes(Assembly assemblyToSearch)
        {
            foreach (Type entity in assemblyToSearch.GetTypes())
            {
                if (entity.GetCustomAttributes<PrismTableAttribute>().Any() &&
                    entity.GetCustomAttribute<PrismIgnoreAttribute>() == null)
                {
                    yield return entity;
                }
            }
        }
    }
}
