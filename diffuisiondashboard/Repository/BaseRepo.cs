using LiteDB;
using Microsoft.VisualBasic;
using System.Linq.Expressions;

namespace diffuisiondashboard.Repository
{
    public class BaseRepo<T> : IBaseRepo<T> where T : class
    {
        protected readonly LiteDatabase db;
        protected readonly string table;

        public BaseRepo(string table)
        {
            this.db = new LiteDatabase($@"Filename={APIConstants.DB_LOCATION};connection=shared");
            this.table = table;
        }

        public void Add(T entity)
        {
            using (db)
            {
                var col = db.GetCollection<T>(table);
                col.Insert(entity);
            }
        }

        public void Update(T entity)
        {
            using (db)
            {
                var col = db.GetCollection<T>(table);
                col.Update(entity);
            }
        }

        public void AddRange(IEnumerable<T> entities)
        {
            using (db)
            {
                var col = db.GetCollection<T>(table);
                foreach (var entity in entities)
                {
                    col.Insert(entity);
                }
            }
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            using (db)
            {
                var col = db.GetCollection<T>(table);
                return col.Find(predicate);
            }
        }

        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            using (db)
            {
                var col = db.GetCollection<T>(table);
                return col.Find(predicate).FirstOrDefault();
            }
        }

        public IEnumerable<T> GetAll()
        {
            using (db)
            {
                var col = db.GetCollection<T>(table);
                return col.FindAll().ToList();
            }
        }

        public void Remove(Expression<Func<T, bool>> predicate)
        {
            using (db)
            {
                var col = db.GetCollection<T>(table);
                col.DeleteMany(predicate);
            }
        }

        public void Truncate()
        {
            using (db)
            {
                var col = db.GetCollection<T>(table);
                col.DeleteAll();
            }
        }

        public void Upsert(T entity)
        {
            using (db)
            {
                var col = db.GetCollection<T>(table);
                col.Upsert(entity);
            }
        }
    }
}
