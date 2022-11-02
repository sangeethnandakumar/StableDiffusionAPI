using System.Linq.Expressions;

namespace diffuisiondashboard.Repository
{
    public interface IBaseRepo<T> where T : class
    {
        //CREATE
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        //READ
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        //UPDATE
        void Update(T entity);
        void Upsert(T entity);
        //DELETE
        void Remove(Expression<Func<T, bool>> predicate);
        void Truncate();
    }
}
