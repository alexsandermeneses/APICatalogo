using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Context;

public class IProdutoRepositoryt : DbContext
{
    public IProdutoRepositoryt(DbContextOptions<IProdutoRepositoryt> options ) : base( options)
    {
    }

    public DbSet<Categoria>? Categorias { get; set; }
    public DbSet<Produto>? Produtos { get; set; }
}
