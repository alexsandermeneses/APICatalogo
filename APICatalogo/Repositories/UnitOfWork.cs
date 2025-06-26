using System;
using APICatalogo.Context;

namespace APICatalogo.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private IProdutoRepository? _produtoRepo;
    private ICategoriaRepository? _categoriaRepo;
    public AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    //garante que obtem a instancias apenas se não existirem e apenas se precisar
    public IProdutoRepository ProdutoRepository
    {
        get
        {
            return _produtoRepo = _produtoRepo ?? new ProdutoRepository(_context); // se _produtoRepo for null, cria uma nova instância de ProdutoRepository e armazena, caso contrário, retorna a instância já criada. Uso de lazy loading: adia a obtenção dos objetos até que eles sejam realmente necessários
           
            //mesma coisa que a linha acima 

            //if(_produtoRepo is null)
            //{
            //    _produtoRepo = new ProdutoRepository(_context);
            //}   
            //return _produtoRepo;    
        }
    }

    //garante que obtem a instancias apenas se não existirem e apenas se precisar
    public ICategoriaRepository CategoriaRepository
    {
        get 
        {
            return _categoriaRepo = _categoriaRepo ?? new CategoriaRepository(_context); //Uso de lazy loading: adia a obtenção dos objetos até que eles sejam realmente necessários
        }
    }

    public void Commit()
    {
        _context.SaveChanges();
    }

    //libera recursos associados ao context do banco de dados 
    public void Dispose() 
    { 
        _context.Dispose();
    }
}
