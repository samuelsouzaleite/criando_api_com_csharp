using System;
using System.Collections.Generic;
using System.Text;

namespace Escola.Domain.Entities
{
    public class Nota : BaseEntity
    {
        public int Id { get; set; }
        public int MatriculaId { get; set; }
        public int ValorNota { get; set; }
        public bool Aprovado { get; set; }
        public Matricula Matricula { get; set; }
    }
}   