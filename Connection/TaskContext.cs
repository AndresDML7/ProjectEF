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
        modelBuilder.Entity<Category>(category => 
        {
            category.ToTable("Category");
            category.HasKey(p => p.CategoryId);

            category.Property(p => p.Name).IsRequired().HasMaxLength(150);
            category.Property(p => p.Description);
        });

        modelBuilder.Entity<Models.Task>(task => 
        {
            task.ToTable("Task");
            task.HasKey(p => p.TaskId);

            task.HasOne(p => p.Category).WithMany(p => p.Tasks).HasForeignKey(p => p.CategoryId);
            task.Property(p => p.Title).IsRequired().HasMaxLength(200);
            task.Property(p => p.Description);
            task.Property(p => p.TaskPriority);
            task.Property(p => p.CreationDate);
            task.Ignore(p => p.Summary);
        });
    }
}