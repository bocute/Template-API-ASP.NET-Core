using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RBTemplate.Infra.CrossCutting.Identity.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace RBTemplate.Infra.CrossCutting.Identity.Mappings
{
    public class RefreshTokenMap : IEntityTypeConfiguration<RefreshTokenData>
    {
        public void Configure(EntityTypeBuilder<RefreshTokenData> builder)
        {
            builder.HasKey(r => r.UsuarioId);

            builder.Property(r => r.UsuarioId)
                .HasColumnName("UsuarioId")
                .HasColumnType("varchar(256)");

            builder.Property(r => r.RefreshToken)
                .IsRequired();

            builder.ToTable("RefreshToken");
        }
    }
}
