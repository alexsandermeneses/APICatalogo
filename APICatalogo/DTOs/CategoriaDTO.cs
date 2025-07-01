using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs;

public class CategoriaDTO
{
    public int CategoriaId { get; set; }

    [Required]
    [StringLength(80)]
    public string? Nome { get; set; } //{?} define a propriedade como nullable

    [Required]
    [StringLength(300)]
    public string? ImagemUrl { get; set; }
}