using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs;

public class ProdutoDTO
{
    public int ProdutoId { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(80, ErrorMessage = "O nome deve ter no máximo {1} caracteres", MinimumLength = 5)]
    //[PrimeiraLetraMaiuscula] coloca quando é com a classe externa de validação
    public string? Nome { get; set; }

    [Required]
    [StringLength(10, ErrorMessage = "A descrição deve ter no máximo {1} caracteres")]
    public string? Descricao { get; set; }

    [Required]
    //[Range(1, 10000, ErrorMessage = "O preço deve estar entre {1} e {2}")] // A validação de faixa de valor já está na entidade. O DTO é apenas para transferência de dados, não é necessário repetir a anotação aqui.
    public decimal Preco { get; set; }

    [Required]
    [StringLength(300, MinimumLength = 10)]
    public string? ImagemUrl { get; set; }

    public int CategoriaId { get; set; } // mapeai coluna ID da categoria produts
}
