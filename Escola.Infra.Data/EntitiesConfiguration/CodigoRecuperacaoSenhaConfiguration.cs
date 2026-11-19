using System;
using System.Collections.Generic;
using System.Text;
using Escola.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Escola.Infra.Data.EntitiesConfiguration
{
    public class CodigoRecuperacaoSenhaConfiguration : IEntityTypeConfiguration<CodigoRecuperacaoSenha>
    {
        public void Configure(EntityTypeBuilder<CodigoRecuperacaoSenha> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.CodigoHash).IsRequired();
            builder.Property(x => x.CodigoSalt).IsRequired();
            builder.Property(x => x.CriadoEm).IsRequired();
            builder.Property(x => x.ExpiraEm).IsRequired();

            builder.HasOne(x => x.Usuario)
                .WithMany()
                .HasForeignKey(x => x.UsuarioId)
                .OnDelete(DeleteBehavior.NoAction);
            
            builder.HasIndex(x => x.UsuarioId).IsUnique();
        }
    }
}