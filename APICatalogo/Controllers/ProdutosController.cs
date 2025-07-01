using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{    
    //usando o AutoMapper para mapear o DTO
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper; //usando automapper

    public ProdutosController(IUnitOfWork uof, IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }

    [HttpGet("produtos/{id}")]
    public  ActionResult<IEnumerable<ProdutoDTO>> GetProdutosCategoria(int id)
    {
        var produtos = _uof.ProdutoRepository.GetProdutoPorCategoria(id); //troca do _produtoRepository para o uso do _uof
    
        if(produtos is null)
            return NotFound();

        //var destino = _mapper.Map<Destino>(origem);
        var produtosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos); // Adicionado para retornar o dto

        return Ok(produtosDTO);
    }

    [HttpGet]
    public ActionResult<IEnumerable<ProdutoDTO>> Get()
    {
        var produtos = _uof.ProdutoRepository.GetAll();

        if (produtos is null)
        {
            return NotFound("Produtos não encontrados");
        }

        //var destino = _mapper.Map<Destino>(origem);
        var produtosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos); // Adicionado para retornar o dto

        return Ok(produtosDTO);    
    }

    [HttpGet("{id:int}", Name ="ObterProduto")]
    public  ActionResult<ProdutoDTO> Get(int id)
    {
        var produto = _uof.ProdutoRepository.Get(p => p.ProdutoId == id);

        if (produto is null)
            return NotFound($"Produto de código {id} não encontrado");

        var produtoDTO = _mapper.Map<ProdutoDTO>(produto);//retorno de um produto atraves do id

        return Ok(produtoDTO);
    }

    [HttpPost]
    public ActionResult<ProdutoDTO> Post(ProdutoDTO produtoDTO) 
    {
        if (!ModelState.IsValid)
            return BadRequest();

        //nesse caso o Post recebe em dto, então é necessário mapear de dto para produto para que possa salvar
        var produto = _mapper.Map<Produto>(produtoDTO);

        var novoProduto = _uof.ProdutoRepository.Create(produto); // vai criar o produto. Nos repositorios ao criar e editar, não se trabalha com DTO apenas com entidades
        _uof.Commit(); // vai dar o commit

        //mapear para dto 
        var novoProdutoDTO = _mapper.Map<ProdutoDTO>(novoProduto); 

        return new CreatedAtRouteResult("ObterProduto", 
            new { id=novoProdutoDTO.ProdutoId}, novoProdutoDTO);
    }

    [HttpPatch("{id}/UpdatePartial")]
    public ActionResult<ProdutoDTOUpdateResponse>Patch(int id, JsonPatchDocument<ProdutoDTOUpdateRequest> patchProdutoDTO)
    {
        if(patchProdutoDTO is null || id <= 0)
            return BadRequest("Dados inválidos");

        var produto = _uof.ProdutoRepository.Get(c => c.ProdutoId == id);

        if (produto is null)
            return NotFound();

        var produtoUpdateRequest = _mapper.Map<ProdutoDTOUpdateRequest>(produto);

        patchProdutoDTO.ApplyTo(produtoUpdateRequest, ModelState);

        if (!ModelState.IsValid || !TryValidateModel(produtoUpdateRequest))
            return BadRequest(ModelState);

        _mapper.Map(produtoUpdateRequest, produto);

        _uof.ProdutoRepository.Update(produto);
        _uof.Commit();

        return Ok(_mapper.Map<ProdutoDTOUpdateResponse>(produto));
    }

    [HttpPut("{id:int}")]
    public ActionResult<ProdutoDTO> Put(int id, ProdutoDTO produtoDTO)
    {
        if (id != produtoDTO.ProdutoId)
            return BadRequest();//400
        
        //nesse caso o put recebe em dto, então é necessário mapear de dto para produto para que possa salvar
        var produto = _mapper.Map<Produto>(produtoDTO);

        var produtoAtualizado = _uof.ProdutoRepository.Update(produto);
        _uof.Commit();

        //mapear para o retorno em dto
        var produtoAtualizadoDTO = _mapper.Map<ProdutoDTO>(produtoAtualizado);

        return Ok(produtoAtualizadoDTO);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<ProdutoDTO> Delete(int id)
    {
        var produto = _uof.ProdutoRepository.Get(p => p.ProdutoId == id);

        if (produto is null)
            return NotFound("Produto não encontrado");

        var produtoDeletado = _uof.ProdutoRepository.Delete(produto);
        _uof.Commit();

        var produtoDeletadoDTO = _mapper.Map<ProdutoDTO>(produtoDeletado);

        return Ok(produtoDeletadoDTO);
    }
}