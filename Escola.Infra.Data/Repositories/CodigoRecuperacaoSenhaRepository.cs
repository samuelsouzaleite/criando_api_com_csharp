using System;
using System.Collections.Generic;
using System.Text;
using Escola.Domain.Entities;
using Escola.Domain.Interfaces;
using Escola.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infra.Data.Repositories
{
    internal class CodigoRecuperacaoSenhaRepository : ICodigoRecuperacaoSenhaRepository
    {
        private readonly ApplicationDbContext _context;
        
        public CodigoRecuperacaoSenhaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(CodigoRecuperacaoSenha codigoRecuperacaoSenha)
        {
            _context.CodigoRecuperacaoSenha.Add(codigoRecuperacaoSenha);
            await _context.SaveChangesAsync();
        }

        public async Task<CodigoRecuperacaoSenha> GetByUsuarioIdAsync(int usuarioId)
        {
            var dataAgora = DateTime.UtcNow;
            return await _context.CodigoRecuperacaoSenha
                .Where(x => x.UsuarioId == usuarioId && x.ExpiraEm > dataAgora && !x.Utilizado)
                .FirstOrDefaultAsync();
        }

        public async Task InvalidarAtivosAsync(int usuarioId)
        {
            await _context.CodigoRecuperacaoSenha
                .Where(x => x.UsuarioId == usuarioId && x.Utilizado == false)
                .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.Utilizado, true));
        }

        public async Task UpdateAsync(CodigoRecuperacaoSenha codigoRecuperacaoSenha)
        {
            _context.CodigoRecuperacaoSenha.Update(codigoRecuperacaoSenha);
            await _context.SaveChangesAsync();
        }
    }
}