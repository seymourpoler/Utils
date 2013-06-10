using System;
using System.Data;
using System.Linq;
using Demo.AnonymousTypes.Domain.Entities;

namespace Demo.AnonymousTypes.Domain.Persistence.Queries
{
    public abstract class UsersQuery : CommandQueryBase
    {
        public UsersQuery(string connectionString) : base(connectionString) { }

        protected User ParseUser(IDataRecord dataRecord)
        {
            return new User
            {
                Email = Parse<User, string>(dataRecord, x => x.Email),
                FirstName = Parse<User, string>(dataRecord, x => x.FirstName),
                LastName = Parse<User, string>(dataRecord, x => x.LastName),
                Id = Parse<User, int>(dataRecord, x => x.Id)
            };
        }
    }
}
