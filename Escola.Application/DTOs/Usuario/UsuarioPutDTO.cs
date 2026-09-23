using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Escola.Application.DTOs.Usuario
{
    public class UsuarioPutDTO
    {
        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [MaxLength(250, ErrorMessage = "O nome deve ter, no máximo, 250 caracteres.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O E-mail é obrigatório.")]
        [MaxLength(250, ErrorMessage = "O E-mail deve ter, no máximo, 250 caracteres.")]
        [EmailAddress(ErrorMessage = "O E-mail é inválido.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "A senha é obrigatória.")]
        [MinLength(8, ErrorMessage = "A senha deve ter, no mínimo, 8 caracteres.")]
        [MaxLength(250, ErrorMessage = "A senha deve ter, no máximo, 250 caracteres.")]
        public string Senha { get; set; }
    }
}