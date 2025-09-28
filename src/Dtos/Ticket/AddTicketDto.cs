using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using perla_metro_ticket_service.Models.Enums;

namespace perla_metro_ticket_service.src.Dtos.Ticket
{
    public class AddTicketDto
    {
        public required string  IdUser { get; set; }
        public TicketType Type { get; set; }
        public decimal Price { get; set; }
    }
}