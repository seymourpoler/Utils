using System;
using System.Data.Entity;
using CodeFirstSample.Repositories;
using CodeFirstSample.Entities;

namespace CodeFirstSample.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DataBaseContext>());

            var simpleUser = new User() { Name = "Name" };
            using (var context = new DataBaseContext())
            {
                context.Users.Add(simpleUser);
                context.SaveChanges();

                context.Status.Add(new Status() { IdStatus = new Guid("00000000-0000-0000-0000-000000000001"), Name = "Stopped"});
                context.Status.Add(new Status() { IdStatus = new Guid("00000000-0000-0000-0000-000000000002"), Name = "Executing" });
                context.Status.Add(new Status() { IdStatus = new Guid("00000000-0000-0000-0000-000000000003"), Name = "Done" });
                context.SaveChanges();
            }
        }
    }
}
