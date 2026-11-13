using Escola.Application.DTOs.Usuario;
using Escola.Application.Exceptions;
using Escola.Application.Interfaces;
using Escola.Domain.Entities;
using Escola.Domain.Interfaces;
using Escola.Domain.Pagination;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Escola.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<UsuarioGetDTO> AddAsync(UsuarioPostDTO usuarioPostDTO)
        {
            var usuarioExistente = await _usuarioRepository.UserExists(usuarioPostDTO.Email);
            if (usuarioExistente)
                throw new BadRequestException("Usuário com este e-mail já existe.");

            using var hmac = new HMACSHA512();
            byte[] passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(usuarioPostDTO.Senha));
            byte[] passwordSalt = hmac.Key;

            var existeUsuario = await _usuarioRepository.ExisteUsuarioAsync();

            var usuario = new Usuario
            {
                Nome = usuarioPostDTO.Nome,
                Email = usuarioPostDTO.Email,
                Excluido = false,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Perfil = existeUsuario ? "Aluno" : "Administrador"
            };

            var createdUsuario = await _usuarioRepository.AddAsync(usuario);
            return new UsuarioGetDTO
            {
                Id = createdUsuario.Id,
                Nome = createdUsuario.Nome,
                Email = createdUsuario.Email,
                Perfil = createdUsuario.Perfil
            };
        }

        public async Task AlterarSenhaAsync(int usuarioId, AlterarSenhaDTO alterarSenhaDTO)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
            if (usuario == null || usuario.Excluido)
            {
             throw new NotFoundException("Usuário não encontrado.");   
            }

            using var hmac = new HMACSHA512(usuario.PasswordSalt);
            var hashInformado = hmac.ComputeHash(Encoding.UTF8.GetBytes(alterarSenhaDTO.SenhaAtual));
            if (!hashInformado.SequenceEqual(usuario.PasswordHash))
            {
                throw new BadRequestException("Senha atual incorreta.");
            }

            // Gerar novo hash para a nova senha
            using var hmacNova = new HMACSHA512();
            usuario.PasswordHash = hmacNova.ComputeHash(Encoding.UTF8.GetBytes(alterarSenhaDTO.NovaSenha));
            usuario.PasswordSalt = hmacNova.Key;
            await _usuarioRepository.UpdateAsync(usuario);
        }


        public async Task<UsuarioGetDTO> DeleteAsync(int id)
        {
            var deletedUsuario = await _usuarioRepository.DeleteAsync(id);
            if (deletedUsuario == null)
                throw new NotFoundException("Usuário não encontrado.");
            return new UsuarioGetDTO
            {
                Id = deletedUsuario.Id,
                Nome = deletedUsuario.Nome,
                Email = deletedUsuario.Email,
                Perfil = deletedUsuario.Perfil
            };
        }

        public Task<bool> ExisteUsuarioAsync()
        {
            return _usuarioRepository.ExisteUsuarioAsync();
        }


        public async Task<PagedList<UsuarioGetDTO>> GetAllAsync(int pageNumber, int pageSize)
        {
            var usuarios = await _usuarioRepository.GetAllAsync(pageNumber, pageSize);
            var usuarioDTOs = usuarios.Select(u => new UsuarioGetDTO
            {
                Id = u.Id,
                Nome = u.Nome,
                Email = u.Email,
                Perfil = u.Perfil
            }).ToList();

            return new PagedList<UsuarioGetDTO>(usuarioDTOs, usuarios.CurrentPage, usuarios.PageSize, usuarios.TotalCount);
        }

        public async Task<UsuarioGetDTO> GetByIdAsync(int id)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            if (usuario == null)
                throw new NotFoundException("Usuário não encontrado.");
            return new UsuarioGetDTO
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Perfil = usuario.Perfil
            };
        }

        public async Task<UsuarioGetDTO> GetUsuarioByEmail(string email)
        {
            var usuario = await _usuarioRepository.GetUsuarioByEmail(email);
            if (usuario == null)
                throw new NotFoundException("Usuário não encontrado.");
            return new UsuarioGetDTO
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Perfil = usuario.Perfil
            };
        }


        public async Task<UsuarioGetDTO> UpdateAsync(int usuarioId, UsuarioPutDTO usuarioPutDTO)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
            if (usuario == null)
                throw new NotFoundException("Usuário não encontrado.");
            usuario.Nome = usuarioPutDTO.Nome;

            if (usuarioPutDTO.Email.ToLower() != usuario.Email.ToLower())
            {
                var usuarioExistente = await _usuarioRepository.UserExists(usuarioPutDTO.Email);
                if (usuarioExistente)
                    throw new BadRequestException("Usuário com este e-mail já existe.");

                usuario.Email = usuarioPutDTO.Email;
            }

            
            var updatedUsuario = await _usuarioRepository.UpdateAsync(usuario);
            return new UsuarioGetDTO
            {
                Id = updatedUsuario.Id,
                Nome = updatedUsuario.Nome,
                Email = updatedUsuario.Email,
                Perfil = updatedUsuario.Perfil
            };
        }

    }
}
