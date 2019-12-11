using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RBTemplate.Domain.Entities.EventLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace RBTemplate.Infra.Data.Mappings
{
    public class EventLogMap : IEntityTypeConfiguration<EventLog>
    {
        public void Configure(EntityTypeBuilder<EventLog> builder)
        {
            builder.HasKey(l => l.Id);

            builder.ToTable("EventLog");
        }
    }
}
