using Microsoft.EntityFrameworkCore;
using SQLiteLibrary.Models;
using System;

namespace SQLiteLibrary
{
    class AppContext : DbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<Worker> Workers { get; set; }
        private readonly string _dbPath;
        public AppContext(string dbPath)
        {
            _dbPath = dbPath;

            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite($"Filename={_dbPath}");
        }
    }
}
