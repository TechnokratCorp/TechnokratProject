using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TehnokratProject.Models;

namespace TehnokratProject.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Feedback> feedbacks { get; set; }
        public DbSet<Category> categorys { get; set; }
        public DbSet<Problem> problems { get; set; }
        public DbSet<Solution> solutions { get; set; }
        public DbSet<Product> products { get; set; }

    }
}
