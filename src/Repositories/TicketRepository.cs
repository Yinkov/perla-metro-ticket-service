using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using perla_metro_ticket_service.Models;
using perla_metro_ticket_service.src.Interfaces;

namespace perla_metro_ticket_service.src.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly IMongoCollection<Ticket> _tickets;

        public TicketRepository(IMongoDatabase database)
        {
            _tickets = database.GetCollection<Ticket>("Tickets");

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
          
            var result =  await _tickets.Find(t => t.isActive).ToListAsync(); //poner t => t.isActive
            return result;
        }

        public async Task<Ticket> GetById(string id)
        {
            var filter = Builders<Ticket>.Filter.Eq(t => t.Id, id); //poner t => t.isActive
            var result =  await _tickets.Find(filter).FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<Ticket>> GetByIdUser(string idUser)
        {
            var filter = Builders<Ticket>.Filter.Eq(t => t.IdUser, idUser); //poner t => t.isActive
            var result =  await _tickets.Find(filter).ToListAsync();
            return result;
        }

        public async Task<bool> Update(string id, Ticket ticket)
        {
            var filter = Builders<Ticket>.Filter.Eq("_id", id); //poner t => t.isActive

            var combinedUpdate = Builders<Ticket>.Update.Combine(
                Builders<Ticket>.Update.Set("Type", ticket.Type),
                Builders<Ticket>.Update.Set("State", ticket.State),
                Builders<Ticket>.Update.Set("issueDate", ticket.issueDate),
                Builders<Ticket>.Update.Set("Price", ticket.Price)
            );
            var result = await _tickets.UpdateOneAsync(filter, combinedUpdate);

            return result.ModifiedCount > 0; 
        }
    }
}