using System.Reflection;

namespace Prism.Core.EntityDiscovery
{
    /// <summary>
    /// Contract for locating Prism-managed entity types within an assembly.
    /// Decouples the discovery strategy from <c>PrismContext</c> so alternative
    /// scanning approaches (e.g. convention-based) can be swapped in.
    /// </summary>
    public interface IEntitySearcher
    {
        /// <summary>
        /// Returns all mappable entity types found in <paramref name="assemblyToSearch"/>.
        /// </summary>
        public IEnumerable<Type> GetMappableTypes(Assembly assemblyToSearch);
    }
}
