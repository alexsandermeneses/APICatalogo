using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{

    //usando método de extensão(MANUAL) para fazer o mapeamento do DTO
    private readonly IUnitOfWork _uof;
    private readonly ILogger<CategoriasController> _logger;

    public CategoriasController(ICategoriaRepository repository, ILogger<CategoriasController> logger, IUnitOfWork uof)
    {
        _logger = logger;
        _uof = uof;
    }

    [HttpGet]
    public ActionResult<IEnumerable<CategoriaDTO>> Get() // trocado de entidade Categoria pela CategoriaDTO
    {
        //var categorias = _repository.GetAll();  //com a implementação do UOF, não usa o repository. O repository da entidade está na classe UnitOfWork 
        var categorias = _uof.CategoriaRepository.GetAll();

        if (categorias is null) 
            return NotFound("Não existem categorias");

        var categoriasDTO = categorias.ToCategoriaDTOList();//removido o codigo de conversão do DTO e inserido a extensão

        return Ok (categoriasDTO);
    }

    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public ActionResult<CategoriaDTO> Get(int id) // trocado de entidade Categoria pela CategoriaDTO
    {
        var categoria = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);

        if (categoria is null)
        {
            _logger.LogWarning($"Categoria com id= {id} não encontrada...");
            return NotFound($"Categoria com id= {id} não encontrada...");
        }

        //mapeamento do DTO 
        var categoriaDTO = categoria.ToCategoriaDTO();//removido o codigo de conversão do DTO e inserido a extensão

        return Ok(categoriaDTO); //retorna o mapeamento feito
    }

    [HttpPost]
    public ActionResult Post(CategoriaDTO categoriaDTO)//trocado categoriaDTO da requisição do corpo(entrada de dados) e adicionado categoriadto de forma explicita no retorno
    {
        if (categoriaDTO is null)
        {
            _logger.LogWarning($"Dados inválidos...");
            return BadRequest("Dados inválidos");
        }

        //converto o dto recebido. no repositório não se trata com DTO, mas sim com entidades, pois exige o objeto do tipo categoria
        var categoria = categoriaDTO.ToCategoria();

        var categoriaCriada =_uof.CategoriaRepository.Create(categoria);//troca do _repository para _uof
        _uof.Commit(); // com a remoção do savechanges do repositoy, se usa o commit do unitofwork garantindo a atomicidade da operação

        //mapeamento do DTO, converter para DTO, pois já foi criado e agora precisa printar para o user o DTO
        var retornoCategoriaDTO = categoria.ToCategoriaDTO();

        return new CreatedAtRouteResult("ObterCategoria", new { id = retornoCategoriaDTO.CategoriaId }, retornoCategoriaDTO);
    }

    [HttpPut("{id:int}")]
    public ActionResult<CategoriaDTO> Put(int id, CategoriaDTO categoriaDTO)//trocado categoriaDTO da requisição do corpo(entrada de dados) e adicionado categoriadto de forma explicita no retorno
    {
        if (id != categoriaDTO.CategoriaId)
        {
            _logger.LogWarning($"Dados inválidos...");
            return BadRequest("Dados inválidos");
        }

        var categoria = categoriaDTO.ToCategoria();

        var categoriaAtualizada = _uof.CategoriaRepository.Update(categoria); // change of _repository to _uof 
        _uof.Commit(); //com a remoção do savechange do repository, se usa o commit do unitofwork para garantir a atomicidade da operação

        var retornoCategoriaAtualizadaDTO = categoria.ToCategoriaDTO();

        return Ok(retornoCategoriaAtualizadaDTO);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<CategoriaDTO> Delete(int id)// adicionado categoriadto de forma explicita no retorno
    {
        var categoria = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);

        if (categoria is null)
        {
            _logger.LogWarning($"Categoria com id={id} não encontrada...");
            return NotFound($"Categoria com id={id} não encontrada...");
        }

        var categoriaExcluida = _uof.CategoriaRepository.Delete(categoria);
        _uof.Commit(); //persistencia com o método commit

        var categoriaExcluidaDTO = categoriaExcluida.ToCategoriaDTO();

        return Ok(categoriaExcluidaDTO);
    }
}