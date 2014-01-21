using System;
using System.Collections.Generic;

namespace CodeFirstSample.Repositories
{
    public  interface IRepository<T> where T: class
    {
        Guid Insert(T entity);
        IEnumerable<T> GetAll();
        T GetById(Guid id);
        void Delete(Guid id);
        void Update(T entity);
    }
}
