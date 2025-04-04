using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TehnokratProject.Models;

namespace TehnokratProject.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Feedback> feedbacks { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Problem> problems { get; set; }
        public DbSet<Solution> solutions { get; set; }
        public DbSet<Product> products { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options)
        {
        }

        public ApplicationDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Category-Products
            modelBuilder.Entity<Product>()
                .HasOne(p => p.category)
                .WithMany(p => p.products)
                .HasForeignKey(c => c.category_id);

            // Category-Problems
            modelBuilder.Entity<Problem>()
                 .HasOne(p => p.category)
                 .WithMany(p => p.problems)
                 .HasForeignKey(c => c.category_id);

            // Promblem-solutions
            modelBuilder.Entity<Solution>()
              .HasOne(g => g.problem)
              .WithMany(p => p.solutions)
              .HasForeignKey(g => g.problem_id);


            base.OnModelCreating(modelBuilder);
        }
    }
}
