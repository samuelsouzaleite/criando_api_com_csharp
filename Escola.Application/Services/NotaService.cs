using Escola.Application.DTOs.Nota;
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
    public class NotaService : INotaService
    {
        private readonly INotaRepository _notaRepository;
        private readonly IMatriculaRepository _matriculaRepository;

        public NotaService(INotaRepository notaRepository, IMatriculaRepository matriculaRepository)
        {
            _notaRepository = notaRepository;
            _matriculaRepository = matriculaRepository;
        }
        public async Task<NotaGetDTO> AddAsync(NotaPostDTO notaPostDTO)
        {
            if (await _matriculaRepository.GetByIdAsync(notaPostDTO.MatriculaId) == null)
            {
                throw new NotFoundException("Matrícula não encontrada.");
            }

            var nota = new Nota
            {
                MatriculaId = notaPostDTO.MatriculaId,
                ValorNota = notaPostDTO.ValorNota,
                Aprovado = notaPostDTO.ValorNota >= 60,
                DataNota = DateTime.UtcNow
            };
            var createdNota = await _notaRepository.AddAsync(nota);
            return new NotaGetDTO
            {
                Id = createdNota.Id,
                MatriculaId = createdNota.MatriculaId,
                ValorNota = createdNota.ValorNota,
                Aprovado = createdNota.Aprovado,
                DataNota = createdNota.DataNota
            };
        }

        public async Task<NotaGetDTO> DeleteAsync(int id)
        {
            var deletedNota = await _notaRepository.DeleteAsync(id);
            if (deletedNota == null)
                throw new NotFoundException("Nota não encontrada.");
            return new NotaGetDTO
            {
                Id = deletedNota.Id,
                MatriculaId = deletedNota.MatriculaId,
                ValorNota = deletedNota.ValorNota,
                Aprovado = deletedNota.Aprovado,
                DataNota = deletedNota.DataNota
            };
        }

        public async Task<PagedList<NotaGetDTO>> GetAllAsync(int pageNumber, int pageSize)
        {
            var notas = await _notaRepository.GetAllAsync(pageNumber, pageSize);
            var notaDTOs = notas.Select(nota => new NotaGetDTO
            {
                Id = nota.Id,
                MatriculaId = nota.MatriculaId,
                ValorNota = nota.ValorNota,
                Aprovado = nota.Aprovado,
                DataNota = nota.DataNota
            }).ToList();

            return new PagedList<NotaGetDTO>(notaDTOs, notas.CurrentPage, notas.PageSize, notas.TotalCount);
        }

        public async Task<NotaGetDTO> GetByIdAsync(int id)
        {
            var nota = await _notaRepository.GetByIdAsync(id);
            if (nota == null)
                throw new NotFoundException("Nota não encontrada.");
            return new NotaGetDTO
            {
                Id = nota.Id,
                MatriculaId = nota.MatriculaId,
                ValorNota = nota.ValorNota,
                Aprovado = nota.Aprovado,
                DataNota = nota.DataNota
            };
        }

        public async Task<PagedList<NotaGetDTO>> GetNotasByTurmaUsuario(int idTurma, int idUsuario, int pageNumber, int pageSize)
        {
            var notas = await _notaRepository.GetNotasByTurmaUsuario(idTurma, idUsuario, pageNumber, pageSize);
            var notaDTOs = notas.Select(nota => new NotaGetDTO
            {
                Id = nota.Id,
                MatriculaId = nota.MatriculaId,
                ValorNota = nota.ValorNota,
                Aprovado = nota.Aprovado,
                DataNota = nota.DataNota
            }).ToList();

            return new PagedList<NotaGetDTO>(notaDTOs, notas.CurrentPage, notas.PageSize, notas.TotalCount);
        }


        public async Task<NotaGetDTO> UpdateAsync(NotaPutDTO notaPutDTO)
        {
            var existingNota = await _notaRepository.GetByIdAsync(notaPutDTO.Id);
            if (existingNota == null)
                throw new NotFoundException("Nota não encontrada.");
            if (notaPutDTO.MatriculaId != existingNota.MatriculaId)
            {
                if (await _matriculaRepository.GetByIdAsync(notaPutDTO.MatriculaId) == null)
                {
                    throw new NotFoundException("Matrícula não encontrada.");
                }
                existingNota.MatriculaId = notaPutDTO.MatriculaId;
            }

            existingNota.ValorNota = notaPutDTO.ValorNota;
            existingNota.Aprovado = notaPutDTO.ValorNota >= 60;
            var updatedNota = await _notaRepository.UpdateAsync(existingNota);
            return new NotaGetDTO
            {
                Id = updatedNota.Id,
                MatriculaId = updatedNota.MatriculaId,
                ValorNota = updatedNota.ValorNota,
                Aprovado = updatedNota.Aprovado,
                DataNota = updatedNota.DataNota
            };
        }
    }
}
