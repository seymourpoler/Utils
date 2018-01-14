using System.Collections.Generic;
using Gambon.SqlServer;
using NUnit.Framework;
using System;

namespace GambonIntegrationTest.SqlServer
{
	[TestFixture]
	public class SqlExecutorTests
	{
		private Configuration configuration;
		private SqlConnectionFactory sqlConnectionFactory;
		private SqlExecutor sqlExecutor;
		
		[SetUp]
		public void SetUp(){
			configuration = new Configuration();
			sqlConnectionFactory = new SqlConnectionFactory(configuration);
			sqlExecutor = new SqlExecutor(sqlConnectionFactory);
		}
		
		[Test]
		public void ReturnsAllFromUsers(){
			var users = sqlExecutor.ExecuteReader("SELECT * FROM USERS");
			
			Assert.IsNotNull(users);
			Assert.IsInstanceOf(typeof(IEnumerable<dynamic>), users);
		}
		
		[Test]
		public void ReturnsFirstOrDefaultFromUsers(){
			var result = sqlExecutor.ExecuteFirstOrDefault("SELECT * FROM USERS");
			
			Assert.IsNull(result);
		}
	}
}
