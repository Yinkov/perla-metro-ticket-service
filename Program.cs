using MongoDB.Driver;
using DotNetEnv;
var builder = WebApplication.CreateBuilder(args);

Env.Load();

string linkDb = Environment.GetEnvironmentVariable("DATABASE_URL") ?? "mongodb+srv://<UserName>:<Contraseña>@base-ticket-service.y9rcn0b.mongodb.net/?retryWrites=true&w=majority&appName=Base-Ticket-Service";

MongoClient mongoClient = new MongoClient(linkDb);

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
