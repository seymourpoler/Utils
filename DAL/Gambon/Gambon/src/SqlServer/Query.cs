using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Gambon.SqlServer
{
	public class Query
	{
		private readonly SqlConnectionFactory sqlConnectionFactory;
		
		public Query(SqlConnectionFactory sqlConnectionFactory)
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
	}
}
