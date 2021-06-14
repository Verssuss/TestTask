using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace TestTask.Model
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<Employee> Employees { get; set; }
        readonly string stringConnect;

        public ApplicationDbContext() { }
        public ApplicationDbContext(string stringConnect)
        {
            this.stringConnect = stringConnect;
        }

        protected override async void OnModelCreating(ModelBuilder modelBuilder)
        {

            if (!this.GetService<IRelationalDatabaseCreator>().Exists())
            {
                 this.Database.ExecuteSqlRaw(@"
        CREATE TABLE IF NOT EXISTS ""__EFMigrationsHistory"" (
            ""MigrationId"" TEXT NOT NULL CONSTRAINT ""PK___EFMigrationsHistory"" PRIMARY KEY,
            ""ProductVersion"" TEXT NOT NULL
        );

        INSERT OR IGNORE INTO ""__EFMigrationsHistory"" (""MigrationId"", ""ProductVersion"")
        VALUES ('20210614121004_Initial', '5.0.7');        
    ");
            }

            //        if (this.GetService<IRelationalDatabaseCreator>().Exists())
            //        {
            //            await this.Database.ExecuteSqlRawAsync(@"
            //    CREATE TABLE IF NOT EXISTS '__EFMigrationsHistory' (
            //        'MigrationId' TEXT NOT NULL CONSTRAINT 'PK___EFMigrationsHistory' PRIMARY KEY,
            //        'ProductVersion' TEXT NOT NULL
            //    );

            //    INSERT OR IGNORE INTO '__EFMigrationsHistory' ('MigrationId', 'ProductVersion')
            //    VALUES ('20210611160746_Initial', '5.0.7');        
            //");
            //        }

            //modelBuilder.Entity<Project>()
            //      .HasMany(p => p.Executors)
            //      .WithMany(p => p.ExecutorProjects)
            //      .UsingEntity<EmployeeProject>(
            //          j => j
            //              .HasOne(pt => pt.Employee)
            //              .WithMany(t => t.EmpPrj)
            //              .HasForeignKey(pt => pt.EmployeeId),
            //          j => j
            //              .HasOne(pt => pt.Project)
            //              .WithMany(p => p.EmpPrj)
            //              .HasForeignKey(pt => pt.ProjectId),
            //          j =>
            //          {
            //              j.HasKey(t => new { t.EmployeeId, t.ProjectId });
            //          });

            modelBuilder
              .Entity<Employee>()
              .HasMany(e => e.EmployeeProjects)
              .WithOne(p => p.Employee);

            modelBuilder
              .Entity<Employee>()
              .HasMany(e => e.LeaderProjects)
              .WithOne(p => p.Leader);

            modelBuilder
              .Entity<Employee>()
              .HasMany(e => e.ExecutorProjects)
              .WithMany(p => p.Executors);

            var emp  = new Employee { Id = 1, Name = "Игорь", Surname = "Панфлов", Patronymic = "Платонович", Email = "igor.p@yandex.ru" };
            var emp2 = new Employee { Id = 2, Name = "Степан", Surname = "Князев", Patronymic = "Даниилович", Email = "stepan.knyazev@mail.ru" };
            var emp3 = new Employee { Id = 3, Name = "Аверьян", Surname = "Корнилов", Patronymic = "Демьянович", Email = "kornilov224@gmail.com" };

            var prj  = new Project { Id = 1, EmployeeId = emp.Id, LeaderId = emp.Id, CompanyCustomer = "Customer", CompanyExecutor = "Executor", Start = DateTime.Now, Finish = DateTime.Now.AddDays(10), Name = "SuperProject", Priority = Priority.High };
            var prj2 = new Project { Id = 2, EmployeeId = emp2.Id, LeaderId = emp2.Id, CompanyCustomer = "Customer2", CompanyExecutor = "Executor2", Start = DateTime.Now, Finish = DateTime.Now.AddDays(13), Name = "SuperProject2", Priority = Priority.Low };
            var prj3 = new Project { Id = 3, EmployeeId = emp3.Id, LeaderId = emp3.Id, CompanyCustomer = "Customer3", CompanyExecutor = "Executor3", Start = DateTime.Now, Finish = DateTime.Now.AddDays(16), Name = "SuperProject3", Priority = Priority.Middle };

            modelBuilder.Entity<Employee>()
            .HasData(emp, emp2, emp3);

            modelBuilder.Entity<Project>()
                .HasData(prj, prj2, prj3);


            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={stringConnect}");
        }
    }
}
