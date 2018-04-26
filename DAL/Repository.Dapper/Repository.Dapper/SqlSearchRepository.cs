using Dapper;
using System.Collections.Generic;
using System.Linq;

namespace Repository.Dapper
{
    public interface ISqlSearchRepository
    {
        IReadOnlyCollection<TResult> Search<TResult>(string sqlQuery, object param = null);
    }

    public class SqlSearchRepository : ISqlSearchRepository
    {
        private readonly SqlConnectionFactory sqlConnectionFactory;

        public SqlSearchRepository(SqlConnectionFactory sqlConnectionFactory)
        {
            this.sqlConnectionFactory = sqlConnectionFactory;
        }

        public IReadOnlyCollection<TResult> Search<TResult>(string sqlQuery, object param = null)
        {
            using (var connection = sqlConnectionFactory.Create())
            {
                connection.Open();
                return connection
                    .Query<TResult>(sqlQuery, param)
                    .ToList()
                    .AsReadOnly();
            }
        }
    }
}
