using Dapper;

namespace Repository.Dapper
{
    public interface ISqlInsertRepository
    {
        void Insert<T>(string sql, T entity);
    }

    public class SqlInsertRepository : ISqlInsertRepository
    {
        private readonly SqlConnectionFactory sqlConnectionFactory;

        public SqlInsertRepository(SqlConnectionFactory sqlConnectionFactory)
        {
            this.sqlConnectionFactory = sqlConnectionFactory;
        }

        public void Insert<T>(string sql, T entity)
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
