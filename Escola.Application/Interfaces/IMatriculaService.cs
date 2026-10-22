using Escola.Application.DTOs.Curso;
using Escola.Application.DTOs.Matricula;
using Escola.Domain.Entities;
using Escola.Domain.Pagination;
using System;
using System.Collections.Generic;
using System.Text;

namespace Escola.Application.Interfaces
{
    public interface IMatriculaService
    {
        Task<MatriculaGetDetailDTO> GetByIdAsync(int id);
        Task<PagedList<MatriculaGetDetailDTO>> GetAllAsync(int pageNumber, int pageSize);
        Task<MatriculaGetDTO> AddAsync(MatriculaPostDTO matriculaPostDTO);
        Task<MatriculaGetDTO> UpadateAsync(MatriculaPutDTO matriculaPutDTO);
        Task<MatriculaGetDTO> DeleteAsync(int id);
    }
}