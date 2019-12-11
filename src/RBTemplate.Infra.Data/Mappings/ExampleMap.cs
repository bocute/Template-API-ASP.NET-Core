using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RBTemplate.Domain.Business.Example;
using System;
using System.Collections.Generic;
using System.Text;

namespace RBTemplate.Infra.Data.Mappings
{
    public class ExampleMap : IEntityTypeConfiguration<Example>
    {
        public void Configure(EntityTypeBuilder<Example> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Descricao)
                .HasColumnType("varchar(150)")
                .IsRequired();

            builder.Ignore(e => e.ValidationResult);
            builder.Ignore(e => e.CascadeMode);

            builder.ToTable("Example");
        }
    }
}
