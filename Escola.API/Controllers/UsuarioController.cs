using Microsoft.AspNetCore.Mvc;
using Escola.Application.Interfaces;
using Escola.Application.DTOs.Usuario;

namespace Escola.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class UsuarioController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        
        public IActionResult index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateUsuario(UsuarioPostDTO usuarioPostDTO)
        {
            await _usuarioService.AddAsync(usuarioPostDTO);
            return Ok(new { message = "Cadastro realizado com sucesso!" });
        }
    }
}