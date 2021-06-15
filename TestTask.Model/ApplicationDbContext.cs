using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

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
        public ApplicationDbContext(DbContextOptions options) : base(options) { }
        protected override async void OnModelCreating(ModelBuilder modelBuilder)
        {

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

            var emp = new Employee { Id = 1, Name = "Игорь", Surname = "Панфлов", Patronymic = "Платонович", Email = "igor.p@yandex.ru" };
            var emp2 = new Employee { Id = 2, Name = "Степан", Surname = "Князев", Patronymic = "Даниилович", Email = "stepan.knyazev@mail.ru" };
            var emp3 = new Employee { Id = 3, Name = "Аверьян", Surname = "Корнилов", Patronymic = "Демьянович", Email = "kornilov224@gmail.com" };

            var prj = new Project { Id = 1, EmployeeId = emp.Id, LeaderId = emp.Id, CompanyCustomer = "Customer", CompanyExecutor = "Executor", Start = DateTime.Now, Finish = DateTime.Now.AddDays(10), Name = "SuperProject", Priority = Priority.High };
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

    public class DesignTimeApplicationContext : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<ApplicationDbContext> builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseSqlite("Data Source=database.db");
            //builder.UseSqlite("Data Source=db.db", x => x.MigrationsAssembly("TestTask.Model"));
            return new ApplicationDbContext(builder.Options);
        }
    }
}
