using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using Demo.AnonymousTypes.Domain.Entities;

namespace Demo.AnonymousTypes.Domain.Persistence.Queries
{
    public class GetAllUsersQuery : UsersQuery, IQuery<IEnumerable<User>>
	{
        public GetAllUsersQuery(string connectionstring)
            : base(connectionstring) { }

		public IEnumerable<User> Execute()
		{
            return Query("SELECT * FROM [Users]", ParseUser);
		}
	}
}