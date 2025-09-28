using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using perla_metro_ticket_service.src.Dtos.Ticket;
using perla_metro_ticket_service.src.Interfaces;
using perla_metro_ticket_service.src.Mappers;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace perla_metro_ticket_service.src.Controller
{
    [Route("Ticket-Service/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {

        private readonly ITicketRepository _ticketRepository;

        public TicketController(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }


        [HttpPost]
        public async Task<IActionResult> AddTicket([FromForm] AddTicketDto addTicketDto)
        {
            try
            {
                /* Preguntar al main api si existe el user
                var userId = 
                if (userId == null)
                {
                    return Unauthorized("Usuario no existe.");
                }
                */
                // Validar modelo inicial
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var newTicket = addTicketDto.ToModel();

                await _ticketRepository.Add(newTicket);

                return Ok("Ticker añadido exitosamente.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetAllTicket()
        {

            var tickets = await _ticketRepository.GetAll();

            return Ok(tickets);
        }

        [HttpGet]
        public async Task<IActionResult> GetTicket(string id)
        {

            var ticket = await _ticketRepository.GetById(id);

            return Ok(ticket);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTicket(string id, UpdateTicket updateTicket)
        {
            var ticket = await _ticketRepository.GetById(id);
            if (ticket != null)
            {
                var ticketToUpdate = updateTicket.ToModel();
                var result = await _ticketRepository.Update(id, ticketToUpdate);
                if (result)
                {
                    return Ok("El ticket se actulizo exitosamente");
                }
                return BadRequest("No se pudo actualizar el ticket");
            }
            return BadRequest("El ticke no existe");
            

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTicket(string id)
        {
            
            var result = await _ticketRepository.DeleteSoft(id);
            if (result)
            {
                return Ok("El ticket se elimino exitosamente");
            }
            return BadRequest("No se pudo eliminar el ticket");

        }
    }
}