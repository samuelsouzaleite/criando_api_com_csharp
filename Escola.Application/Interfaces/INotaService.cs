using Escola.Application.DTOs.Nota;
using Escola.Domain.Entities;
using Escola.Domain.Pagination;
using System;
using System.Collections.Generic;
using System.Text;

namespace Escola.Application.Interfaces
{
    public interface INotaService
    {
        Task<NotaGetDTO> GetByIdAsync(int id);
        Task<PagedList<NotaGetDTO>> GetAllAsync(int pageNumber, int pageSize);
        Task<NotaGetDTO> AddAsync(NotaPostDTO notaPostDTO);
        Task<NotaGetDTO> UpdateAsync(NotaPutDTO notaPutDTO);
        Task<NotaGetDTO> DeleteAsync(int id);
        Task<PagedList<NotaGetDTO>> GetNotasByTurmaUsuario(int idTurma, int idUsuario, int pageNumber, int pageSize);
    }
}