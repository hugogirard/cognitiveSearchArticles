using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Article.Api.Models
{
    public class ArticleContext : DbContext
    {
        public DbSet<Article> Articles { get; set; }

        public DbSet<Category> Categories { get; set; }

        public ArticleContext(DbContextOptions<ArticleContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasKey(c => c.Id);

            modelBuilder.Entity<Category>().Property(c => c.Id).ValueGeneratedNever();
        }
    }
}
