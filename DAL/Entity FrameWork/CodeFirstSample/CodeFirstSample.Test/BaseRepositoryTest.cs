using System;
using System.Data.Entity;

namespace CodeFirstSample.Test
{
    public class BaseRepositoryTest
    {

        protected DataBaseContext _db;
        private string _connectionString = @"Data Source=PORTATIL\SQLEXPRESS;Initial Catalog=CodeFirstSample.DataBaseContext;Integrated Security=True";

        public BaseRepositoryTest()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DataBaseContext>());
            _db = new DataBaseContext(_connectionString);
        }
    }
}
