using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        //[HttpGet("primeiro")]
        [HttpGet("/primeiro")]
        //[HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> Get2()
        {
            return await _context.Produtos.AsNoTracking().ToListAsync();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = _context.Produtos.AsNoTracking().ToList();

            if (produtos is null) { 
                return NotFound("Produtos não encontrados");
            }

            return produtos;    
        }

        [HttpGet("{id:int}", Name ="ObterProduto")]
        public async Task<ActionResult<Produto>> Get([FromQuery]int id)
        {

            var produto = await _context.Produtos.AsNoTracking().
                FirstOrDefaultAsync(p => p.ProdutoId == id);

            if (produto == null) { 
                return NotFound($"Produto de código {id} não encontrado");
            }

            return produto;
        }

        [HttpPost]
        public ActionResult Post(Produto produto) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            _context.Produtos.Add(produto); // salva em memória
            _context.SaveChanges(); //persiste os dados na tabela
            
            return new CreatedAtRouteResult("ObterProduto", 
                new { id=produto.ProdutoId}, produto);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if(id != produto.ProdutoId)
            {
                return BadRequest();
            }
            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id) 
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
            //var produto = _context.Produtos.Find(id);

            if (produto is null) 
            {
                return NotFound("Produto não encontrado");
            }

            _context.Produtos.Remove(produto);
            _context.SaveChanges();

            return Ok($"Produto deletado"); //ou apenas coloca o objeto "produto"
        }
    }
}
