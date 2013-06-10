using System;
using System.Data;
using System.Collections.Generic;
using Demo.AnonymousTypes.Domain.Entities;
using Demo.AnonymousTypes.Domain.Persistence.Queries;
using Demo.AnonymousTypes.Domain.Persistence.Commands;

namespace Demo.AnonymousTypes.Domain.Persistence.Repositories
{
    public class UsersRepository : IRepository<User>
    {
        private string _connectionString = string.Empty;

        public UsersRepository()
        {
            _connectionString = Configuration.GetConnectionString();
        }

        public IEnumerable<User> GetAll()
        {
            var db = new GetAllUsersQuery(_connectionString);
            return db.Execute();
        }

        public User GetById(int idUser)
        {
            var db = new GetUserByIdQuery(_connectionString, idUser);
            return db.Execute();
        }

        public void Save(User user)
        {
            var db = new CreateUserCommand(_connectionString, user);
            db.Execute();
        }

        public void Update(User user)
        {
            var db = new UpdateUserCommand(_connectionString, user);
            db.Execute();
        }

        public void DeleteAll()
        {
            var db = new DeleteAllUsersCommand(_connectionString);
            db.Execute();
        }

        public void DeleteById(int idUser)
        {
            var db = new DeleteUserByIdCommand(_connectionString, idUser);
            db.Execute();
        }
    }
}
