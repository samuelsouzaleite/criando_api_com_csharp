using System;
using System.Collections.Generic;
using System.Text;

namespace Escola.Domain.Entities
{
    public class CodigoRecuperacaoSenha
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public byte[] CodigoHash { get; set; }
        public byte[] CodigoSalt { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime ExpiraEm { get; set; }
        public bool Utilizado { get; set; }
        public int Tentativas { get; set; }

        public Usuario Usuario { get; set; }
    }
}