using Microsoft.EntityFrameworkCore;
using ProjectEF.Models;

namespace ProjectEF.Connection;

public class TaskContext : DbContext
{
    public DbSet<Category> Category { get; set; }
    public DbSet<Models.Task> Task { get; set; }
    
    public TaskContext(DbContextOptions<TaskContext> options) : base(options) { }
}