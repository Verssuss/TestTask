using Autofac;
using Autofac.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using TestTask.Model;
using TestTask.Services;
using TestTask.Views;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace TestTask
{
    public partial class App : Application
    {
        static string pathDB = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "test.db");
        public App()
        {
            InitializeComponent();

            DependencyService.Register<ProjectDataStore>();
            DependencyService.Register<EmployeeDataStore>();

            DependencyResolver.ResolveUsing(type => container.IsRegistered(type) ? container.Resolve(type) : null);
            App.RegisterTypeWithParameters<ApplicationDbContext>(typeof(string), pathDB);
            App.BuildContainer();
            DbContext = DependencyService.Resolve<ApplicationDbContext>();

            //var services = new ServiceCollection();
            //services.AddDbContext<ApplicationDbContext>(o => o.UseSqlite(pathDB));
            //_provider = services.BuildServiceProvider();
            //var services = new ServiceCollection();
            //services.AddTransient<ApplicationDbContext>();

            //_provider = services.BuildServiceProvider();
            MainPage = new AppShell();
        }
        public ApplicationDbContext DbContext;
        //public static ApplicationDbContext DbContext;
        static readonly ContainerBuilder builder = new ContainerBuilder();
        static IContainer container;
        public static void BuildContainer()
        {
            container = builder.Build();
        }
        public static void RegisterType<T>() where T : class
        {
            builder.RegisterType<T>();
        }
        public static void RegisterTypeWithParameters<T>(Type param1Type, object param1Value) where T : class
        {
            builder.RegisterType<T>().InstancePerLifetimeScope()
                   .WithParameters(new List<Parameter>()
            {
            new TypedParameter(param1Type, param1Value)
            });
        }
        protected async override void OnStart()
        {
            //if (!DbContext.GetService<IRelationalDatabaseCreator>().Exists())
    //        {
    //            await DbContext.Database.ExecuteSqlRawAsync(@"
    //    CREATE TABLE IF NOT EXISTS '__EFMigrationsHistory' (
    //        'MigrationId' TEXT NOT NULL CONSTRAINT 'PK___EFMigrationsHistory' PRIMARY KEY,
    //        'ProductVersion' TEXT NOT NULL
    //    );

    //    INSERT OR IGNORE INTO '__EFMigrationsHistory' ('MigrationId', 'ProductVersion')
    //    VALUES ('20210611160746_Initial', '5.0.7');        
    //");
    //        }
            await DbContext.Database.MigrateAsync();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
