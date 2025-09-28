using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using perla_metro_ticket_service.Models.Enums;

namespace perla_metro_ticket_service.src.Dtos.Ticket
{
    public class TicketDto
    {
        public string? Id { get; set; }
        public string?  NameUser { get; set; }
        public DateTime issueDate { get; set; } 
        public TicketType Type { get; set; }
        public TicketState State { get; set; } 
        public decimal Price { get; set; }
    }
}