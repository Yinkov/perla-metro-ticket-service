using MongoDB.Driver;
using DotNetEnv;
using perla_metro_ticket_service.Models;
using perla_metro_ticket_service.Models.Enums;
var builder = WebApplication.CreateBuilder(args);

Env.Load();

string linkDb = Environment.GetEnvironmentVariable("DATABASE_URL") ?? "mongodb+srv://<UserName>:<Contraseña>@base-ticket-service.y9rcn0b.mongodb.net/?retryWrites=true&w=majority&appName=Base-Ticket-Service";
string nameDb = Environment.GetEnvironmentVariable("DATABASE_NAME") ?? "Data-base-ticket-service";
string nameColection = Environment.GetEnvironmentVariable("COLECTION_TICKET_NAME") ?? "ticket";
MongoClient mongoClient = new MongoClient(linkDb);

var database1 = mongoClient.GetDatabase(nameDb);
var collection = database1.GetCollection<Ticket>(nameColection);


await collection.InsertOneAsync(new Ticket {
    IdUser = "aaaaaa",
    issueDate = DateTime.Now,
    Type = TicketType.Ida,
    State = TicketState.Activo,
    Price = 1200.50m
});


List<String> databases = mongoClient.ListDatabaseNames().ToList();

foreach (string database in databases)
{
    
    Console.WriteLine(database);
    
}

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();




app.Run();
