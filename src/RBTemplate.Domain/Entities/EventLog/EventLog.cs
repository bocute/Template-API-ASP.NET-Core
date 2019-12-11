using System;
using System.Collections.Generic;
using System.Text;

namespace RBTemplate.Domain.Entities.EventLog
{
    public class EventLog
    {
        public Guid Id { get; private set; }
        public DateTime DateLog { get; set; }
        public string Data { get; set; }
        public EventLog()
        {
            Id = Guid.NewGuid();
        }
    }
}
