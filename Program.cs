using MongoDB.Driver;
using DotNetEnv;
using perla_metro_ticket_service.Models;
using perla_metro_ticket_service.Models.Enums;
using perla_metro_ticket_service.src.Interfaces;
using perla_metro_ticket_service.src.Repositories;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        // Esto hace que ASP.NET acepte y serialice enums como string
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

var pack = new ConventionPack
{
    new EnumRepresentationConvention(BsonType.String)
};
ConventionRegistry.Register("EnumAsString", pack, t => true);

string linkDb = Environment.GetEnvironmentVariable("DATABASE_URL") ?? "mongodb+srv://<UserName>:<Contraseña>@base-ticket-service.y9rcn0b.mongodb.net/?retryWrites=true&w=majority&appName=Base-Ticket-Service";
string nameDb = Environment.GetEnvironmentVariable("DATABASE_NAME") ?? "Data-base-ticket-service";

MongoClient mongoClient = new MongoClient(linkDb);

var database = mongoClient.GetDatabase(nameDb);



builder.Services.AddSingleton(database);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<ITicketRepository, TicketRepository>();






var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.MapControllers();


app.Run();
