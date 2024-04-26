using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectEF.Connection;

var builder = WebApplication.CreateBuilder(args);

//Base de datos en memoria
//builder.Services.AddDbContext<TaskContext>(p => p.UseInMemoryDatabase("TaskDB"));
builder.Services.AddSqlServer<TaskContext>("Data Source=DESKTOP-T7PGRPB; Initial Catalog=TaskDB; user id=sa; password=andresDML7; TrustServerCertificate=True");

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/dbconnection", async ([FromServices] TaskContext dbContext) => 
{
    dbContext.Database.EnsureCreated();
    return Results.Ok("Base de datos en memoria: " + dbContext.Database.IsInMemory());
});

app.Run();
