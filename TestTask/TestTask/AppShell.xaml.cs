using System;
using System.Collections.Generic;
using TestTask.ViewModels;
using TestTask.Views;
using TestTask.Views.Employees;
using TestTask.Views.Projects;
using Xamarin.Forms;

namespace TestTask
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ProjectDetailPage), typeof(ProjectDetailPage));
            Routing.RegisterRoute(nameof(NewProjectPage), typeof(NewProjectPage));
            Routing.RegisterRoute(nameof(EditProjectPage), typeof(EditProjectPage));

            Routing.RegisterRoute(nameof(EmployeeDetailPage), typeof(EmployeeDetailPage));
            Routing.RegisterRoute(nameof(NewEmployeePage), typeof(NewEmployeePage));
            Routing.RegisterRoute(nameof(EditEmployeePage), typeof(EditEmployeePage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
