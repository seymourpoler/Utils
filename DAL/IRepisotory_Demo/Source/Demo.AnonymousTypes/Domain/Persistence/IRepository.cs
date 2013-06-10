using System.Collections.Generic;

namespace Demo.AnonymousTypes.Domain.Persistence
{
    public interface IRepository<T> where T:class
    {
        void Save(T entity);
        void Update(T entity);
        IEnumerable<T> GetAll();
        T GetById(int id);
        void DeleteAll();
        void DeleteById(int id);
    }
}
