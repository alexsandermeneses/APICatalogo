using System.Linq.Expressions;
using APICatalogo.Context;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly AppDbContext _context;

    public Repository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<T> GetAll()
    {
        return _context.Set<T>().AsNoTracking().ToList(); //AsNoTracking desabilito o gerenciamento do estado da entidades namemoria, ganha memória e desempenho. Seu uso é apenas para exibição(Get)
    }

    public T? Get(Expression<Func<T, bool>> predicate)
    {
       return _context.Set<T>().FirstOrDefault(predicate);
    }

    public T Create(T entity)
    {
        _context.Set<T>().Add(entity);
        //_context.SaveChanges(); o uso de savechanges não é necessário neste método já que está no método commit da classe UnitOfWork 
        return entity;
    }

    public T Update(T entity)
    {
        _context.Set<T>().Update(entity);
        //_context.Entry(entity).State= EntityState.Modified;
        //_context.SaveChanges();  o uso de savechanges não é necessário neste método já que está no método commit da classe UnitOfWork
        return entity;
    }

    public T Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
        //_context.SaveChanges();
        return entity;
    }
}
