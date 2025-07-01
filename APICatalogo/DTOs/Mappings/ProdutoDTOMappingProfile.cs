using APICatalogo.Models;
using AutoMapper;

namespace APICatalogo.DTOs.Mappings;

public class ProdutoDTOMappingProfile : Profile
{
    public ProdutoDTOMappingProfile() 
    { 
        //CreateMap<ProdutoDTO, Produto>();// pode ser feito assim para usar o reverso do mapeamento
        CreateMap<Produto, ProdutoDTO>().ReverseMap();//O uso do reverse map serve para que possamos usar os dois mapeamentos(tanto para o dto quando sem ser o dto).
        CreateMap<Categoria, CategoriaDTO>().ReverseMap();
        CreateMap<Produto, ProdutoDTOUpdateRequest>().ReverseMap();
        CreateMap<Produto, ProdutoDTOUpdateResponse>().ReverseMap();
    }
}
