using Microsoft.AspNetCore.Mvc;
using Escola.Application.Interfaces;
using Escola.Application.DTOs.Usuario;
using Escola.Domain.Account;
using Microsoft.AspNetCore.Authorization;
using Escola.API.Models;

namespace Escola.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class UsuarioController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IAuthenticate _authenticate;
        
        public UsuarioController(IUsuarioService usuarioService, IAuthenticate authenticateService)
        {
            _usuarioService = usuarioService;
            _authenticate = authenticateService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateUsuario(UsuarioPostDTO usuarioPostDTO)
        {
            var usuarioExists = await _authenticate.UserExists(usuarioPostDTO.Email);
            if (usuarioExists)
                return BadRequest(new { message = "Usuário já existe" });
            var usuario = await _usuarioService.AddAsync(usuarioPostDTO);
            var token =  _authenticate.GenerateToken(usuario.Id, usuario.Email.ToLower(), usuario.Perfil);
            return Ok(new { Nome = usuario.Nome, Token = token });
        }

        [HttpPost("login")]
        public async Task<ActionResult> GetTokenUsuario(UserLogin userLogin)
        {
            var usuario = await _authenticate.GetUsuarioByEmail(userLogin.Email);
            if (usuario == null)
                return BadRequest(new { message = "Usuário ou senha inválidos" });
            
            var usuarioValido = await _authenticate.AuthenticateAsync(userLogin.Email, userLogin.Senha);
            if (!usuarioValido)
                return BadRequest(new { message = "Usuário ou senha inválidos" });

            var token =  _authenticate.GenerateToken(usuario.Id, usuario.Email.ToLower(), usuario.Perfil);
            return Ok(new { Nome = usuario.Nome, Token = token });
        }

        /*[HttpGet("rota-de-teste")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Teste()
        {
            return Ok(new { message = "passou aqui" });
        }*/
    }
}