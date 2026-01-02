using Api5_Kanban.Models;
using Microsoft.EntityFrameworkCore;

namespace Api5_Kanban.Data
{
    public class KanbanDbContext : DbContext
    {
        public KanbanDbContext(DbContextOptions<KanbanDbContext> options):base(options) { }
        public DbSet<Column> Columns => Set<Column>();
        public DbSet<TaskItem> Tasks=> Set<TaskItem>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskItem>()
                .HasIndex(t => new { t.ColId, t.Ord })
                .IsUnique();
        }
    }
}
