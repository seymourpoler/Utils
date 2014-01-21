using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeFirstSample.Repositories
{
    public class ProjectRepository :BaseRepository
    {
        public ProjectRepository(DataBaseContext db)
            : base(db)
        { }
    }
}
