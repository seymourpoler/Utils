using System;
using System.Collections.Generic;
using CodeFirstSample.Entities;

namespace CodeFirstSample.Repositories
{
    public class StatusRepository: BaseRepository
    {
        public StatusRepository(DataBaseContext context)
            : base(context)
        { }

        public Guid Insert(Status status)
        {
            _db.Status.Add(status);
            return status.IdStatus;
        }

        public void Delete(Guid idStatus)
        {
            var status = _db.Status.Find(idStatus);
            if (status != null)
            { 
                _db.Status.Remove(status);
                _db.SaveChanges();
            }
        }
    }
}
