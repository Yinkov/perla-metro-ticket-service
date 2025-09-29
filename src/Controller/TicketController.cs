using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using perla_metro_ticket_service.Models.Enums;
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


        [HttpPost("/Add")]
        public async Task<IActionResult> AddTicket([FromForm] AddTicketDto addTicketDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                /* Preguntar al main api si existe el user
                var userId = 
                if (userId == null)
                {
                    return Unauthorized("Usuario no existe.");
                }
                */
                // Validar modelo inicial


                var newTicket = addTicketDto.ToModel();

                var ticket = await _ticketRepository.Add(newTicket);

                return Ok(new { message = "Ticket añadido exitosamente", ticket});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("/GetAllTickets")]
        public async Task<IActionResult> GetAllTicket([FromQuery] string? userId, [FromQuery] DateTime? fecha, [FromQuery] TicketState? state)
        {

            var tickets = await _ticketRepository.GetAll(userId,fecha,state);
            if (tickets.Count == 0)
            {
                return NotFound("No hay tickets actualmente");
            }

            return Ok(tickets);
        }

        [HttpGet("/Get/{id}")]
        public async Task<IActionResult> GetTicket(string id)
        {
            var ticket = await _ticketRepository.GetById(id);
            if (ticket == null)
            {
                return NotFound("El ticket no existe");
            }

            

            return Ok(ticket);
        }

        [HttpPut("/Update/{id}")]
        public async Task<IActionResult> UpdateTicket(string id,[FromForm] UpdateTicket updateTicket)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var ticket = await _ticketRepository.GetById(id);
            if (ticket != null)
            {
                if (ticket.State.Equals(TicketState.Caducado) && updateTicket.State.Equals(TicketState.Activo))
                {
                    return BadRequest("No se puede Activar un ticket Caducado");
                }
                var result = await _ticketRepository.Update(id, updateTicket);
                if (result)
                {
                    return Ok("El ticket se actulizo exitosamente");
                }
                return BadRequest("No se pudo actualizar el ticket");
            }
            return NotFound("El ticke no existe");
            

        }

        [HttpDelete("/Delete/{id}")]
        public async Task<IActionResult> DeleteTicket(string id)
        {
            var ticket = await _ticketRepository.GetById(id);
            if (ticket == null)
            {
                return NotFound("El ticket no existe");
            }
            
            var result = await _ticketRepository.DeleteSoft(id);
            if (result)
            {
                return Ok("El ticket se elimino exitosamente");
            }
            return BadRequest("No se pudo eliminar el ticket");

        }
    }
}