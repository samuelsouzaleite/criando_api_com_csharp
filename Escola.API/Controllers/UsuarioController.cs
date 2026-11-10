using Microsoft.AspNetCore.Mvc;
using Escola.Application.Interfaces;
using Escola.Application.DTOs.Usuario;
using Escola.Domain.Account;
using Microsoft.AspNetCore.Authorization;
using Escola.API.Models;
using Escola.API.Extensions;

namespace Escola.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class UsuarioController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IAuthenticateService _authenticate;
        public UsuarioController(IUsuarioService usuarioService, IAuthenticateService authenticate)
        {
            _usuarioService = usuarioService;
            _authenticate = authenticate;
        }

        [HttpPost] //Implementar futuramente um serviço de verificação de e-mail para validar o cadastro do usuário
        public async Task<ActionResult> CreateUsuario(UsuarioPostDTO usuarioPostDTO)
        {
            var usuario = await _usuarioService.AddAsync(usuarioPostDTO);
            var token =  _authenticate.GenerateToken(usuario.Id, usuario.Email.ToLower(), usuario.Perfil);
            return Ok(new { Nome = usuario.Nome, Token = token });
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> GetAllUsuarios([FromQuery] PaginationParams paginationParams)
        {
            var usuarios = await _usuarioService.GetAllAsync(paginationParams.PageNumber, paginationParams.PageSize);
            Response.AddPaginationHeader(
                new PaginationHeader(paginationParams.PageNumber, paginationParams.PageSize, usuarios.TotalCount, usuarios.TotalPages));
            return Ok(usuarios);
        }

        [HttpPost("login")]
        public async Task<ActionResult> GetTokenUsuario(UserLogin userLogin)
        {   
            var usuario = await _authenticate.AuthenticateAsync(userLogin.Email, userLogin.Senha);
            
            var token =  _authenticate.GenerateToken(usuario.Id, usuario.Email.ToLower(), usuario.Perfil);
            return Ok(new { Nome = usuario.Nome, Token = token });
        }
    }
}