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
    public class TurmaRepository : ITurmaRepository
    {
       private readonly ApplicationDbContext _context;
        public TurmaRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Turma> AddAsync(Turma turma)
        {
           _context.Turma.Add(turma);
           await _context.SaveChangesAsync();
           return turma;
        }
       
        public async Task<Turma> DeleteAsync(int id)
        {
            var turma = await _context.Turma.FindAsync(id);
            if (turma == null)
            {
                return null;
            }

            turma.Excluido = true;
            _context.Turma.Update(turma);
            await _context.SaveChangesAsync();
            return turma;
        }

        public async Task<PagedList<Turma>> GetAllAsync(int pageNumber, int pageSize)
        {
            var query = _context.Turma.Include(x => x.Curso).Where(x => x.Excluido == false).AsNoTracking();
            return await PaginationHelper.CreateAsync(query, pageNumber, pageSize);
        }

        public async Task<Turma> GetByIdAsync(int id)
        {
            return await _context.Turma.Include(x => x.Curso).Where(x => x.Excluido == false && x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Turma>> GetTurmasByUsuario(int idUsuario)
        {
            return await _context.Turma
                .Include(t => t.Curso)
                .Where(t => t.Excluido == false && t.Matriculas.Any(m => m.UsuarioId == idUsuario))
                .ToListAsync();
        }

        public async Task<Turma> UpdateAsync(Turma turma)
        {
            _context.Turma.Update(turma);
            await _context.SaveChangesAsync();
            return turma;
        }
    
    }
}