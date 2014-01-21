using System;
using CodeFirstSample.Entities;

namespace CodeFirstSample.Repositories
{
    public class GroupRepository : BaseRepository
    {
        public GroupRepository(DataBaseContext db) : base(db) { }

        public Guid Insert(Group group)
        {
            var guid = Guid.NewGuid();
            group.IdGroup = guid;
            _db.Groups.Add(group);
            _db.SaveChanges();
            return guid;
        }

        public void AddUserToGroup(Group group, User user)
        {
            var currentGroup = _db.Groups.Find(group.IdGroup);
            var currentUser = _db.Users.Find(user.IdUser);
            currentGroup.Users.Add(currentUser);
            _db.SaveChanges();
        }

        public Group GetById(Guid idGroup)
        {
            return _db.Groups.Find(idGroup);
        }

        public void Update(Group group)
        {
            _db.Entry<Group>(group).State = System.Data.EntityState.Modified;
            _db.SaveChanges();
        }

        public void Delete(Guid idGroup)
        {
            var user = GetById(idGroup);
            _db.Groups.Remove(user);
            _db.SaveChanges();
        }
    }
}
