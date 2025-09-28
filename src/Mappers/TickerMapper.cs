using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using perla_metro_ticket_service.Models;
using perla_metro_ticket_service.src.Dtos.Ticket;

namespace perla_metro_ticket_service.src.Mappers
{
    public static class TickerMapper
    {
        public static Ticket ToModel(this AddTicketDto addTicketDto)
        {
            return new Ticket
            {
                IdUser = addTicketDto.IdUser,
                Type = addTicketDto.Type,
                Price = addTicketDto.Price
            };
        }
    }
}