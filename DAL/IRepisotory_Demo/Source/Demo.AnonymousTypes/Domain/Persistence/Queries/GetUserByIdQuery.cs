
using System;
using System.Data;
using System.Linq;
using Demo.AnonymousTypes.Domain.Entities;

namespace Demo.AnonymousTypes.Domain.Persistence.Queries
{
    public class GetUserByIdQuery : UsersQuery, IQuery<User>
    {
        private int Id;

        public GetUserByIdQuery(string connectionString, int id)
            : base(connectionString)
        {
            Id = id;
        }

        public User Execute() 
        {
            var data = Query("SELECT * FROM [Users] WHERE Id = @Id", ParseUser, new { Id });
            var dataList = data.ToList();
            if (dataList.Count > 0)
            { 
                return dataList[0]; 
            }
            throw new Exception(string.Format("User with Id: {0}, not found", Id));
        }
    }
}
