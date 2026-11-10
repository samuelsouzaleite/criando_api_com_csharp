using System;
using System.Collections.Generic;
using System.Text;
using Escola.Application.DTOs.Usuario;
using Escola.Domain.Entities;

namespace Escola.Application.Interfaces
{
    public interface IAuthenticateService
    {
        Task<UsuarioGetDTO> AuthenticateAsync(string email, string senha);
        string GenerateToken(int id, string email, string role);
    }
}