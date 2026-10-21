using System;
using System.Collections.Generic;
using System.Text;
using Escola.Application.DTOs.Curso;
using Escola.Domain.Pagination;

namespace Escola.Application.Interfaces
{
    public interface ICursoService
    {
        Task<CursoGetDTO?> GetByIdAsync(int id);
        Task<PagedList<CursoGetDTO>> GetAllAsync(int pageNumber, int pageSize);
        Task<CursoGetDTO> AddAsync(CursoPostDTO cursoPostDTO);
        Task<CursoGetDTO?> UpdateAsync(int id, CursoPutDTO cursoPutDTO);
        Task<CursoGetDTO?> DeleteAsync(int id);
    }
}