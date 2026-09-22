using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Escola.Domain.Entities;

namespace Escola.Application.DTOs.Curso
{
    public class CursoPostDTO
    {
        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [MaxLength(50, ErrorMessage = "O campo Nome deve ter no máximo 50 caracteres.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O campo Descricao é obrigatório.")]
        [MaxLength(150, ErrorMessage = "O campo Descricao deve ter no máximo 150 caracteres.")]
        public string Descricao { get; set; }
    }
}