using Escola.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Escola.Domain.Interfaces
{
    public interface ICodigoRecuperacaoSenhaRepository
    {
        Task AddAsync(CodigoRecuperacaoSenha codigoRecuperacaoSenha);
        Task<CodigoRecuperacaoSenha> GetByUsuarioIdAsync(int usuarioId);
        Task InvalidarAtivosAsync(int usuarioId);
        Task UpdateAsync(CodigoRecuperacaoSenha codigoRecuperacaoSenha);
    }
}