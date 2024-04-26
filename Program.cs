using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectEF.Connection;
using ProjectEF.Models;

var builder = WebApplication.CreateBuilder(args);

//Base de datos en memoria
//builder.Services.AddDbContext<TaskContext>(p => p.UseInMemoryDatabase("TaskDB"));
builder.Services.AddSqlServer<TaskContext>(builder.Configuration.GetConnectionString("cntask"));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/dbconnection", async ([FromServices] TaskContext dbContext) => 
{
    dbContext.Database.EnsureCreated();
    return Results.Ok("Base de datos en memoria: " + dbContext.Database.IsInMemory());
});

app.MapGet("/api/tasks", async ([FromServices] TaskContext dbContext) => 
{
     return Results.Ok(dbContext.Task.Include(p => p.Category));
});

app.MapPost("/api/createtask", async ([FromServices] TaskContext dbContext, [FromBody] ProjectEF.Models.Task task) => 
{
    task.TaskId = Guid.NewGuid();
    task.CreationDate = DateTime.Now;

    //await dbContext.AddAsync(task);
    await dbContext.Task.AddAsync(task);
    await dbContext.SaveChangesAsync();

    return Results.Ok("Registro creado exitosamente.");
});

app.MapPut("/api/updatetask/{id}", async ([FromServices] TaskContext dbContext, [FromBody] ProjectEF.Models.Task task, [FromRoute] Guid id) => 
{
    var taskToUpdate = dbContext.Task.Find(id);

    if(taskToUpdate != null) 
    {
        taskToUpdate.CategoryId = task.CategoryId;
        taskToUpdate.Title = task.Title;
        taskToUpdate.TaskPriority = task.TaskPriority;
        taskToUpdate.Description = task.Description;

        await dbContext.SaveChangesAsync();
        return Results.Ok("Registro actualizado satisfactoriamente.");
    }

    return Results.NotFound("El Id ingresado no coincide con ningún registro.");
});

app.MapDelete("/api/deletetask/{id}", async ([FromServices] TaskContext dbContext, [FromRoute] Guid id) => 
{
    var taskToUpdate = dbContext.Task.Find(id);

    if(taskToUpdate != null) 
    {
        dbContext.Remove(taskToUpdate);
        await dbContext.SaveChangesAsync();
        return Results.Ok("Registro eliminado satisfactoriamente.");
    }

    return Results.NotFound("El Id ingresado no coincide con ningún registro.");
});

app.Run();
