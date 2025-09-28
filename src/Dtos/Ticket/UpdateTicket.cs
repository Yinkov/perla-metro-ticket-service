using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using perla_metro_ticket_service.Models.Enums;

namespace perla_metro_ticket_service.src.Dtos.Ticket
{
    public class UpdateTicket
    {
        public DateTime? issueDate { get; set; } 
        [EnumDataType(typeof(TicketType))]
        public TicketType? Type { get; set; }
        [EnumDataType(typeof(TicketState))]
        public TicketState? State { get; set; } 
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")]
        public decimal? Price { get; set; }
    }
}