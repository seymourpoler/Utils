using System;
using Demo.AnonymousTypes.Domain.Entities;

namespace Demo.AnonymousTypes.Domain.Persistence.Commands
{
    public class UpdateUserCommand : CommandQueryBase, ICommand
    {
        private User _user;

        public UpdateUserCommand(string connectionString, User user):base(connectionString)
        {
            _user = user;
        }

        public void Execute() 
        {
            const string sql = @"UPDATE [Users] 
                                    SET FirstName = @FirstName, 
                                        LastName = @LastName, 
                                        Email= @Email
                                WHERE Id = @Id";
            Execute(sql, new { _user.FirstName, _user.LastName, _user.Email, _user.Id});
        }
    }
}
