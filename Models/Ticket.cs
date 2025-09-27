using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using perla_metro_ticket_service.Models.Enums;

namespace perla_metro_ticket_service.Models
{
    public class Ticket
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public required string  IdUser { get; set; }
        public DateTime issueDate { get; set; }
        public TicketType Type { get; set; }
        public TicketState State { get; set; } = TicketState.Activo;
        public decimal Price { get; set; }
        public bool isActive { get; set; } = true;
 
    }
}