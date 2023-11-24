using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Models;

namespace API.Controllers;

[Route("api/tarefa")]
[ApiController]
public class TarefaController : ControllerBase
{
    private readonly AppDataContext _context;

    public TarefaController(AppDataContext context) =>
        _context = context;

    // GET: api/tarefa/listar
    [HttpGet]
    [Route("listar")]
    public IActionResult Listar()
    {
        try
        {
            List<Tarefa> tarefas = _context.Tarefas.Include(x => x.Categoria).ToList();
            return Ok(tarefas);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // GET: api/tarefa/naoconcluidas
    [HttpGet]
    [Route("naoconcluidas")]
    public IActionResult NaoConcluidas()
    {
        try
        {
            List<Tarefa> tarefasConcluidas = _context.Tarefas
                .Where(x => x.Status == "Nao iniciada" || x.Status == "Em andamento" )
                .Include(x => x.Categoria)
                .ToList();

            return tarefasConcluidas.Count == 0 ? NotFound() : Ok(tarefasConcluidas);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // GET: api/tarefa/concluidas
    [HttpGet]
    [Route("concluidas")]
    public IActionResult Concluidas()
    {
        try
        {
            List<Tarefa> tarefasConcluidas = _context.Tarefas
                .Where(x => x.Status == "Concluida")
                .Include(x => x.Categoria)
                .ToList();

            return tarefasConcluidas.Count == 0 ? NotFound() : Ok(tarefasConcluidas);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }


    // POST: api/tarefa/cadastrar
    [HttpPost]
    [Route("cadastrar")]
    public IActionResult Cadastrar([FromBody] Tarefa tarefa)
    {
        try
        {
            Categoria? categoria = _context.Categorias.Find(tarefa.CategoriaId);
            if (categoria == null)
            {
                return NotFound();
            }
            tarefa.Categoria = categoria;
            tarefa.Status = "Nao iniciada";
            _context.Tarefas.Add(tarefa);
            _context.SaveChanges();
            return Created("", tarefa);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // PATCH: api/tarefa/alterar
    [HttpPatch]
    [Route("alterar/{id}")]
    public IActionResult AlterarStatus([FromRoute] int id)
    {
        try
        {
            Tarefa? tarefaCadastrado = _context.Tarefas.FirstOrDefault(x => x.TarefaId == id);
            if (tarefaCadastrado != null)
            {
                if (tarefaCadastrado.Status == "Nao iniciada")
                {
                    tarefaCadastrado.Status = "Em andamento";
                }
                else if (tarefaCadastrado.Status == "Em andamento")
                {
                    tarefaCadastrado.Status = "Concluida";
                }
                else if (tarefaCadastrado.Status == null)
                {
                    tarefaCadastrado.Status = "Nao iniciada";
                }

                _context.Tarefas.Update(tarefaCadastrado);
                _context.SaveChanges();

                return Ok(tarefaCadastrado);
            }

            return NotFound();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

}
