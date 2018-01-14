using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Gambon.SqlServer
{
	public class SqlExecutor
	{
		private readonly SqlConnectionFactory sqlConnectionFactory;
		
		public SqlExecutor(SqlConnectionFactory sqlConnectionFactory)
		{
			this.sqlConnectionFactory = sqlConnectionFactory;
		}
		
		public IEnumerable<dynamic> Execute(string sql){
			using(var connection = sqlConnectionFactory.Create()){
				var command = new SqlCommand(sql, connection);
                var dataReader = command.ExecuteReader();
				 while (dataReader.Read())
                 {
				 	yield return dataReader.ToDynamic();
                 }
			}
		}
		
		public dynamic FirstOrDefault(string sql){
			return Execute(sql).FirstOrDefault();
		}
	}
}
