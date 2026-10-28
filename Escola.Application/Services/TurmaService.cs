using Escola.Application.DTOs.Curso;
using Escola.Application.DTOs.Turma;
using Escola.Application.Exceptions;
using Escola.Application.Interfaces;
using Escola.Domain.Entities;
using Escola.Domain.Interfaces;
using Escola.Domain.Pagination;
using System;
using System.Collections.Generic;
using System.Text;

namespace Escola.Application.Services
{
    public class TurmaService : ITurmaService
    {
        private readonly ITurmaRepository _turmaRepository;
        private readonly ICursoRepository _cursoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        public TurmaService(ITurmaRepository turmaRepository, ICursoRepository cursoRepository, IUsuarioRepository usuarioRepository)
        {
            _turmaRepository = turmaRepository;
            _cursoRepository = cursoRepository;
            _usuarioRepository = usuarioRepository;
        }
        public async Task<TurmaGetDTO> AddAsync(TurmaPostDTO turmaPostDTO)
        {
            var curso = await _cursoRepository.GetByIdAsync(turmaPostDTO.CursoId);
            if (curso == null)
            {
                throw new NotFoundException("Curso não encontrado.");   
            };

            var turma = new Turma
            {
                Nome = turmaPostDTO.Nome,
                Descricao = turmaPostDTO.Descricao,
                CursoId = turmaPostDTO.CursoId
            };
            var createdTurma = await _turmaRepository.AddAsync(turma);
            return new TurmaGetDTO
            {
                Id = createdTurma.Id,
                Nome = createdTurma.Nome,
                Descricao = createdTurma.Descricao,
                CursoId = createdTurma.CursoId
            };
        }

        public async Task<TurmaGetDTO> DeleteAsync(int id)
        {
            var deletedTurma = await _turmaRepository.DeleteAsync(id);
            if (deletedTurma == null)
                throw new NotFoundException("Turma não encontrada.");
            return new TurmaGetDTO
            {
                Id = deletedTurma.Id,
                Nome = deletedTurma.Nome,
                Descricao = deletedTurma.Descricao,
                CursoId = deletedTurma.CursoId
            };
        }

        public async Task<PagedList<TurmaGetDetailDTO>> GetAllAsync(int pageNumber, int pageSize)
        {

            var turmas = await _turmaRepository.GetAllAsync(pageNumber, pageSize);
            var turmaGetDetailDTO = turmas.Select(turma => new TurmaGetDetailDTO
            {
                Id = turma.Id,
                Nome = turma.Nome,
                Descricao = turma.Descricao,
                Curso = new CursoGetDTO
                {
                    Id = turma.Curso.Id,
                    Nome = turma.Curso.Nome,
                    Descricao = turma.Curso.Descricao
                }
            }).ToList();

            return new PagedList<TurmaGetDetailDTO>(turmaGetDetailDTO, turmas.TotalCount, turmas.CurrentPage, turmas.PageSize);
        }

        public async Task<TurmaGetDetailDTO> GetByIdAsync(int id)
        {
            var turma = await _turmaRepository.GetByIdAsync(id);
            if (turma == null)
                throw new NotFoundException("Turma não encontrada.");
            return new TurmaGetDetailDTO
            {
                Id = turma.Id,
                Nome = turma.Nome,
                Descricao = turma.Descricao,
                Curso = new CursoGetDTO
                {
                    Id = turma.Curso.Id,
                    Nome = turma.Curso.Nome,
                    Descricao = turma.Curso.Descricao
                }
            };
        }

        public async Task<PagedList<TurmaGetDetailDTO>> GetTurmasByUsuario(int idUsuario, int pageNumber, int pageSize)
        {
            var turmas = await _turmaRepository.GetTurmasByUsuario(idUsuario, pageNumber, pageSize);
            var turmaGetDetailDTO = turmas.Select(turma => new TurmaGetDetailDTO
            {
                Id = turma.Id,
                Nome = turma.Nome,
                Descricao = turma.Descricao,
                Curso = new CursoGetDTO
                {
                    Id = turma.Curso.Id,
                    Nome = turma.Curso.Nome,
                    Descricao = turma.Curso.Descricao
                }
            }).ToList();

            return new PagedList<TurmaGetDetailDTO>(turmaGetDetailDTO, turmas.CurrentPage, turmas.PageSize, turmas.TotalCount);
        }


        public async Task<TurmaGetDTO> UpdateAsync(TurmaPutDTO turmaPutDTO)
        {
            var turma = await _turmaRepository.GetByIdAsync(turmaPutDTO.Id);
            if (turma == null)
            {
                throw new NotFoundException("Turma não encontrada.");
            };

            var curso = await _cursoRepository.GetByIdAsync(turmaPutDTO.CursoId);
            if (curso == null)
            {
                throw new NotFoundException("Curso não encontrado.");   
            };

            turma.Id = turmaPutDTO.Id;
            turma.Nome = turmaPutDTO.Nome;
            turma.Descricao = turmaPutDTO.Descricao;
            turma.CursoId = turmaPutDTO.CursoId;

            var updatedTurma = await _turmaRepository.UpdateAsync(turma);
            if (updatedTurma == null)
                return null;
            return new TurmaGetDTO
            {
                Id = updatedTurma.Id,
                Nome = updatedTurma.Nome,
                Descricao = updatedTurma.Descricao,
                CursoId = updatedTurma.CursoId
            };
        }
    }
}
