using Escola.API.Errors;
using Escola.API.Extensions;
using Escola.API.Models;
using Escola.Application.DTOs.Turma;
using Escola.Application.Exceptions;
using Escola.Application.Interfaces;
using Escola.Infra.Ioc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Escola.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class TurmaController : Controller
    {
        private readonly ITurmaService _turmaService;
        public TurmaController(ITurmaService turmaService)
        {
            _turmaService = turmaService;
        }
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> CreateTurma(TurmaPostDTO turmaPostDTO)
        {
            var createdTurma = await _turmaService.AddAsync(turmaPostDTO);
            return Ok(new{ message = "Turma incluída com sucesso!" });
        }

        [HttpPut]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> UpdateTurma(TurmaPutDTO turmaPutDTO)
        {
            var updatedTurma = await _turmaService.UpdateAsync(turmaPutDTO);
            return Ok(new {message = "Turma Atualizada com sucesso!"});
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> DeleteTurma(int id)
        {
            var deletedTurma = await _turmaService.DeleteAsync(id);
            return Ok(new { message = "Turma excluída com sucesso!" });
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> GetTurmaById(int id)
        {
            var turma = await _turmaService.GetByIdAsync(id);
            if (turma == null)
            {
                throw new NotFoundException("Turma não encontrada.");
            }
            return Ok(turma);
        }
        
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> GetAllTurmas([FromQuery] PaginationParams paginationParams)
        {
            var turmas = await _turmaService.GetAllAsync(paginationParams.PageNumber, paginationParams.PageSize);

            Response.AddPaginationHeader(
                new PaginationHeader(paginationParams.PageNumber, paginationParams.PageSize, turmas.TotalCount, turmas.TotalPages));

            return Ok(turmas);
        }

        [HttpGet("user")]
        [Authorize(Roles = "Aluno, Administrador")]
        public async Task<ActionResult> GetAllTurmasByUsuario()
        {
            var userId = User.GetUserId();

            var turmas = await _turmaService.GetTurmasByUsuario(userId);
            return Ok(turmas);
        }
    }
}