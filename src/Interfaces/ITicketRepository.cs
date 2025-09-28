using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using perla_metro_ticket_service.Models;

namespace perla_metro_ticket_service.src.Interfaces
{
    public interface ITicketRepository
    {
        Task<Ticket> Add(Ticket ticket);
        Task<List<Ticket>> GetAll();
        Task<Ticket> GetById(string id);
        Task<List<Ticket>> GetByIdUser(string idUser);
        Task<bool> Update(string id,Ticket ticket);
        Task<bool> DeleteSoft(string id); 
    }
}