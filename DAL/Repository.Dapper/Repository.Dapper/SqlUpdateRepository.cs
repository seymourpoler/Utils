using Dapper;

namespace Repository.Dapper
{
    public interface ISqlUpdateRepository
    {
        void Update<T>(string sql, T entity);
    }

    public class SqlUpdateRepository : ISqlUpdateRepository
    {
        private readonly SqlConnectionFactory sqlConnectionFactory;

        public SqlUpdateRepository(SqlConnectionFactory sqlConnectionFactory)
        {
            this.sqlConnectionFactory = sqlConnectionFactory;
        }

        public void Update<T>(string sql, T entity)
        {
            using (var connection = sqlConnectionFactory.Create())
            {
                connection.Open();
                connection.Execute(
                        sql: sql,
                        param: entity);
            }
        }
    }
}
