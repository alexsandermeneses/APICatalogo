using System.Linq.Expressions;

namespace APICatalogo.Repositories;

public interface IRepository<T>
{
    //não viola o principio ISP do SOLID
    IEnumerable<T> GetAll();
    T? Get(Expression<Func<T, bool>> predicate);
    T Create(T entity);
    T Update(T entity);
    T Delete(T entity);
}
