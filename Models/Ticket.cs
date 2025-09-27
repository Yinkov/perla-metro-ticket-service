using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace perla_metro_ticket_service.Models
{
    public class Ticket
    {
        public ObjectId _id { get; set; }
        public int IdUser { get; set; }
        public DateTime issueDate { get; set; }
        public TicketType Type { get; set; }
        public TicketState State { get; set; } = TicketState.Activo;
        public float Price { get; set; }
        public bool isActive { get; set; } = true;
        public enum TicketState
        {
            Activo, Usado, Caducado
        }
        public enum TicketType
        {
            Ida,Vuelta
        }
    }
}