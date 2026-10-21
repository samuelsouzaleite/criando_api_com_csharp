using System;
using System.Collections.Generic;
using System.Text;
using Escola.Domain.Entities;
using Escola.Domain.Interfaces;
using Escola.Domain.Pagination;
using Escola.Infra.Data.Context;
using Escola.Infra.Data.Helpers;
using Microsoft.EntityFrameworkCore;  

namespace Escola.Infra.Data.Repositories
{
    public class CursoRepository : ICursoRepository
    {
        private readonly ApplicationDbContext _context;
        public CursoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Curso> AddAsync(Curso curso)
        {
           _context.Curso.Add(curso);
           await _context.SaveChangesAsync();
           return curso;
        }

        public async Task<Curso> DeleteAsync(int id)
        {
            var curso = await _context.Curso.Where(x => x.Excluido == false && x.Id == id).FirstOrDefaultAsync();
            if (curso == null)
            {
                return null;
            }

            curso.Excluido = true;
            _context.Curso.Update(curso);
            await _context.SaveChangesAsync();
            return curso;
        }

        public async Task<PagedList<Curso>> GetAllAsync(int pageNumber, int pageSize)
        {
            var query = _context.Curso.Where(x => x.Excluido == false).AsNoTracking();
            return await PaginationHelper.CreateAsync(query, pageNumber, pageSize);
            //return await _context.Curso.Where( x => x.Excluido == false).ToListAsync();
        }

        public async Task<Curso> GetByIdAsync(int id)
        {
            return await _context.Curso.Where(x => x.Excluido == false && x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Curso> UpdateAsync(Curso curso)
        {
            _context.Curso.Update(curso);
            await _context.SaveChangesAsync();
            return curso;
        }
    }
}
