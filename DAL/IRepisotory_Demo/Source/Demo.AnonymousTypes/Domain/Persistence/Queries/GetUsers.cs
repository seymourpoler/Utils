using System;
using System.Data;
using System.Linq;
using Demo.AnonymousTypes.Domain.Entities;

namespace Demo.AnonymousTypes.Domain.Persistence.Queries
{
    public class GetUsers : CommandQueryBase
    {
        public GetUsers(string connectionString) : base(connectionString) { }

        public static GetUserByIdQuery GetUserByIdQuery(string connectionString, int id) 
        {
            return new GetUserByIdQuery(connectionString, id);
        }

        public static GetAllUsersQuery GetAllUsersQuery(string connectionString)
        {
            return new GetAllUsersQuery(connectionString);
        }

        public static User ParseUser(IDataRecord dataRecord)
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
