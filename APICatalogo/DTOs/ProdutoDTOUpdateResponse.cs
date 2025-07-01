using APICatalogo.Models;

namespace APICatalogo.DTOs;

public class ProdutoDTOUpdateResponse
{
    public int ProdutoId { get; set; }

    public string? Nome { get; set; }

    public string? Descricao { get; set; }

    public decimal Preco { get; set; }

    public string? ImagemUrl { get; set; }

    public float Estoque { get; set; }

    public DateTime DataCadastro { get; set; }

    public int CategoriaId { get; set; } // mapeai coluna ID da categoria produts

    public Categoria? Categoria { get; set; } //define que produto está mapeado para a categoria// uma categoria para muitos produtos
}
