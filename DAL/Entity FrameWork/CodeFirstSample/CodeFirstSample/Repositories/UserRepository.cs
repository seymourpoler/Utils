using System;
using System.Linq;
using System.Collections.Generic;
using CodeFirstSample.Entities;

namespace CodeFirstSample.Repositories
{
    public class UserRepository : BaseRepository
    {
        
        public UserRepository(DataBaseContext db) : base(db)
        {
        }

        public Guid Insert(User user)
        {
            var guid = Guid.NewGuid();
            user.IdUser = guid;
            _db.Users.Add(user);
            _db.SaveChanges();
            return guid;
        }

        public User GetById(Guid idUser)
        {
            return _db.Users.Find(idUser);
        }

        public void Update(User user)
        {
            _db.Entry<User>(user).State = System.Data.EntityState.Modified;
            _db.SaveChanges();
        }

        public void Delete(Guid idUser)
        {
            var user = GetById(idUser);
            _db.Users.Remove(user);
            _db.SaveChanges();
        }
    }
}
