using System.IO.Compression;
using Microsoft.EntityFrameworkCore;
using ProjectEF.Models;

namespace ProjectEF.Connection;

public class TaskContext : DbContext
{
    public DbSet<Category> Category { get; set; }
    public DbSet<Models.Task> Task { get; set; }
    
    public TaskContext(DbContextOptions<TaskContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) 
    {
        List<Category> initCategories = new List<Category>
        {
            new Category() { CategoryId = Guid.Parse("c00bf160-cd2a-4563-a552-37d70e46f81e"), Name = "Pendientes", Description = "Actividades pendientes por realizar", Weight = 20 },
            new Category() { CategoryId = Guid.Parse("c00bf160-cd2a-4563-a552-37d70e46f82f"), Name = "Personales", Description = "Actividades personales", Weight = 30 }
        };

        modelBuilder.Entity<Category>(category => 
        {
            category.ToTable("Category");
            category.HasKey(p => p.CategoryId);

            category.Property(p => p.Name).IsRequired().HasMaxLength(150);
            category.Property(p => p.Description).IsRequired(false);
            category.Property(p => p.Weight);

            category.HasData(initCategories);
        });

        List<Models.Task> initTasks = new List<Models.Task>
        {
            new Models.Task() { TaskId = Guid.Parse("c00bf160-cd2a-4563-a552-37d70e46f83c"), Title = "Lavar ropa", CategoryId = Guid.Parse("c00bf160-cd2a-4563-a552-37d70e46f81e"),  Description = "Lavar la ropa sucia", TaskPriority = Priority.Medium, CreationDate = DateTime.Now },
            new Models.Task() { TaskId = Guid.Parse("c00bf160-cd2a-4563-a552-37d70e46f84d"), Title = "Motilarme", CategoryId = Guid.Parse("c00bf160-cd2a-4563-a552-37d70e46f82f"),  Description = "Ir a la peluquería", TaskPriority = Priority.Medium, CreationDate = DateTime.Now },
        };

        modelBuilder.Entity<Models.Task>(task => 
        {
            task.ToTable("Task");
            task.HasKey(p => p.TaskId);

            task.HasOne(p => p.Category).WithMany(p => p.Tasks).HasForeignKey(p => p.CategoryId);
            task.Property(p => p.Title).IsRequired().HasMaxLength(200);
            task.Property(p => p.Description).IsRequired(false);
            task.Property(p => p.TaskPriority);
            task.Property(p => p.CreationDate);
            task.Ignore(p => p.Summary);
            
            task.HasData(initTasks);
        });
    }
}