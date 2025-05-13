using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using TextileCuttingOptimizer.Models;

namespace TextileCuttingOptimizer.Data
{
    public class DatabaseService : DbContext
    {
        public DbSet<Fabric> Fabrics { get; set; }
        public DbSet<Pattern> Patterns { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(@"Server=localhost;Database=TextileCutting;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Тестовые данные
            modelBuilder.Entity<Fabric>().HasData(
                new Fabric { Id = 1, Name = "Хлопок", Width = 150, Length = 50 },
                new Fabric { Id = 2, Name = "Сатин", Width = 220, Length = 100 }
            );

            modelBuilder.Entity<Pattern>().HasData(
                new Pattern
                {
                    Id = 1,
                    Name = "Пододеяльник 1.5сп",
                    Contour = new List<Point> { new Point(0, 0), new Point(150, 0), new Point(150, 200), new Point(0, 200) },
                    SeamAllowance = 2.0
                },
                new Pattern
                {
                    Id = 2,
                    Name = "Наволочка 50x70",
                    Contour = new List<Point> { new Point(0, 0), new Point(50, 0), new Point(50, 70), new Point(0, 70) },
                    SeamAllowance = 1.5
                }
            );
        }
    }
}
