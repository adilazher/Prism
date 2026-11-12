using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace Prism.Core.Connections
{
    /// <summary>
    /// Wraps a <see cref="DbConnection"/> and manages its lifecycle for the duration of a <c>PrismContext</c>.
    /// Opens the connection lazily on the first call to <see cref="GetDbConnection"/> and reuses it
    /// while it remains open, avoiding redundant connection overhead across multiple operations.
    /// </summary>
    public class PrismDbConnection : IDisposable
    {
        private readonly string _ConnectionString;
        private DbConnection _DbConnection;

        public PrismDbConnection(string connectionString)
        {
            _ConnectionString = connectionString;
        }

        /// <summary>
        /// Returns the active <see cref="DbConnection"/>, opening a new one if the connection
        /// has not yet been created or has been closed. Throws if the connection string is missing.
        /// </summary>
        public DbConnection GetDbConnection()
        {
            if (string.IsNullOrEmpty(_ConnectionString))
            {
                throw new ArgumentNullException($"Connection string cannot be null or empty.");
            }

            if (_DbConnection == null || _DbConnection.State != ConnectionState.Open)
            {
                _DbConnection = new SqlConnection(_ConnectionString);
                _DbConnection.Open();
            }

            return _DbConnection;
        }

        /// <summary>
        /// Closes and releases the underlying connection. Should be called when the owning
        /// <c>PrismContext</c> is no longer needed.
        /// </summary>
        public void Dispose()
        {
            _DbConnection.Close();
            _DbConnection.Dispose();
        }
    }
}
