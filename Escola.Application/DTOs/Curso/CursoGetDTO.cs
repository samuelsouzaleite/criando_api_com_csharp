using System;
using System.Collections.Generic;
using System.Text;
using Escola.Domain.Entities;

namespace Escola.Application.DTOs.Curso
{
    public class CursoGetDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
    }
}