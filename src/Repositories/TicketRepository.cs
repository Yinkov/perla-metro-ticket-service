using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetEnv;
using MongoDB.Driver;
using perla_metro_ticket_service.Models;
using perla_metro_ticket_service.src.Dtos.Ticket;
using perla_metro_ticket_service.src.Interfaces;
using perla_metro_ticket_service.Models.Enums;

namespace perla_metro_ticket_service.src.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly IMongoCollection<Ticket> _tickets;

        public TicketRepository(IMongoDatabase database)
        {
            
            string nameColection = Environment.GetEnvironmentVariable("COLECTION_TICKET_NAME") ?? "ticket";
            _tickets = database.GetCollection<Ticket>(nameColection);

            var indexKeys = Builders<Ticket>.IndexKeys
            .Ascending(t => t.IdUser)
            .Ascending(t => t.issueDate);

            var indexOptions = new CreateIndexOptions
            {
                Unique = true,
                Name = "ux_ticket_user_date"
            };

            var indexModel = new CreateIndexModel<Ticket>(indexKeys, indexOptions);

            _tickets.Indexes.CreateOne(indexModel);

        

        }

        public async Task<Ticket> Add(Ticket ticket)
        {
            try
            {
                await _tickets.InsertOneAsync(ticket);
                return ticket;
            }
            catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
            {
                throw new Exception("Error: El usuario ya tiene un ticket para esa fecha.");
            }
        }

        public async Task<bool> DeleteSoft(string id)
        {
            var filter = Builders<Ticket>.Filter.Eq(t => t.Id, id);

            var combinedUpdate = Builders<Ticket>.Update.Combine(
                Builders<Ticket>.Update.Set("isActive", false)
            );
            
            var result = await _tickets.UpdateOneAsync(filter, combinedUpdate);

            return result.ModifiedCount > 0;
        }

        public async Task<List<Ticket>> GetAll()
        {
          
            var result =  await _tickets.Find(t => t.isActive).ToListAsync(); 
            return result;
        }

        public async Task<Ticket> GetById(string id)
        {
            var filter = Builders<Ticket>.Filter.And(
                Builders<Ticket>.Filter.Eq(t => t.Id, id),
                Builders<Ticket>.Filter.Eq(t => t.isActive, true)
            );
            var result =  await _tickets.Find(filter).FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<Ticket>> GetByIdUser(string idUser)
        {
            var filter = Builders<Ticket>.Filter.And(
                Builders<Ticket>.Filter.Eq(t => t.IdUser, idUser),
                Builders<Ticket>.Filter.Eq(t => t.isActive, true)); 
            var result =  await _tickets.Find(filter).ToListAsync();
            return result;
        }

        public async Task<bool> Update(string id, UpdateTicket ticket)
        {
            var filter = Builders<Ticket>.Filter.And(
                Builders<Ticket>.Filter.Eq(t => t.Id, id),
                Builders<Ticket>.Filter.Eq(t => t.isActive, true)
            );

            var updates = new List<UpdateDefinition<Ticket>>();

            if (ticket.Type.HasValue)
                updates.Add(Builders<Ticket>.Update.Set(t => t.Type, ticket.Type.Value));

            if (ticket.State.HasValue)
                updates.Add(Builders<Ticket>.Update.Set(t => t.State, ticket.State.Value));

            if (ticket.issueDate.HasValue)
                updates.Add(Builders<Ticket>.Update.Set(t => t.issueDate, ticket.issueDate.Value));

            if (ticket.Price.HasValue)
                updates.Add(Builders<Ticket>.Update.Set(t => t.Price, ticket.Price.Value));

            if (!updates.Any())
                return false; // No hay nada que actualizar

            var combinedUpdate = Builders<Ticket>.Update.Combine(updates);

            try
            {
                var result = await _tickets.UpdateOneAsync(filter, combinedUpdate);
                return result.ModifiedCount > 0;
            }
            catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
            {
                throw new InvalidOperationException("Error: El usuario ya tiene un ticket en esa fecha.", ex);
            }
        }
    }
}