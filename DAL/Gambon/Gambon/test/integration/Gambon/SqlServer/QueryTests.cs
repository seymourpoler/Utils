using System.Collections.Generic;
using Gambon.SqlServer;
using NUnit.Framework;
using System;

namespace GambonIntegrationTest.SqlServer
{
	[TestFixture]
	public class QueryTests
	{
		private Configuration configuration;
		private SqlConnectionFactory sqlConnectionFactory;
		private Query query;
		
		[SetUp]
		public void SetUp(){
			configuration = new Configuration();
			sqlConnectionFactory = new SqlConnectionFactory(configuration);
			query = new Query(sqlConnectionFactory);
		}
		
		[Test]
		public void ReturnsAllFromUsers(){
			var users = query.Execute("SELECT * FROM USERS");
			
			Assert.IsNotNull(users);
			Assert.IsInstanceOf(typeof(IEnumerable<dynamic>), users);
		}
	}
}
