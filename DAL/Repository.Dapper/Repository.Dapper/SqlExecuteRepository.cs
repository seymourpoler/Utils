using Dapper;

namespace Repository.Dapper
{
    public interface ISqlExecuteRepository
    {
        void Execute(string sql);
    }

    public class SqlExecuteRepository : ISqlExecuteRepository
    {
        private readonly SqlConnectionFactory sqlConnectionFactory;

        public SqlExecuteRepository(SqlConnectionFactory sqlConnectionFactory)
        {
            this.sqlConnectionFactory = sqlConnectionFactory;
        }

        public void Execute(string sql)
        {
            using (var connection = sqlConnectionFactory.Create())
            {
                connection.Open();
                connection.Execute(sql);
            }
        }
    }
}
