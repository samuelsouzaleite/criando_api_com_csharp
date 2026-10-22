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
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApplicationDbContext _context;
        public UsuarioRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Usuario> AddAsync(Usuario usuario)
        {
           _context.Usuario.Add(usuario);
           await _context.SaveChangesAsync();
           return usuario;
        }
       
        public async Task<Usuario> DeleteAsync(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return null;
            }

            usuario.Excluido = true;
            _context.Usuario.Update(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<bool> ExisteUsuarioAsync()
        {
            return await _context.Usuario.AnyAsync(x => x.Excluido == false);
        }

        public async Task<PagedList<Usuario>> GetAllAsync(int pageNumber, int pageSize)
        {
            var query = _context.Usuario.Where(x => x.Excluido == false).AsNoTracking();
            return await PaginationHelper.CreateAsync(query, pageNumber, pageSize);
        }

        public async Task<Usuario> GetByIdAsync(int id)
        {
            return await _context.Usuario.Where(x => x.Excluido == false && x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Usuario> UpdateAsync(Usuario usuario)
        {
            _context.Usuario.Update(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }
    }
}