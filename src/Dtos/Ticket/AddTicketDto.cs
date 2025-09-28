using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using perla_metro_ticket_service.Models.Enums;

namespace perla_metro_ticket_service.src.Dtos.Ticket
{
    public class AddTicketDto
    {
        [Required]
        public required string IdUser { get; set; }
        [Required]
        [EnumDataType(typeof(TicketType))]
        public TicketType Type { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")]
        public decimal Price { get; set; }
    }
}