using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using TestTask.Model;
using TestTask.ViewModels.Others;
using TestTask.Views;
using TestTask.Views.Projects;
using Xamarin.Forms;

namespace TestTask.ViewModels.Projects
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    class ProjectDetailViewModel : BaseViewModel
    {
        public ProjectDetailViewModel()
        {
            Title = "Information";
        }

        #region Fields
        private int itemId;
        private string name;
        private string companyCustomer;
        private string companyExecutor;
        private Employee employee;
        private Employee leader;
        private ICollection<Employee> executors;
        private DateTime start;
        private DateTime finish;
        private Priority priority;
        #endregion

        #region Properies
        public int Id { get; set; }
        public int ItemId
        {
            get => itemId; set { itemId = value; LoadItemId(value); }
        }

        public string Name
        {
            get => name; set => SetProperty(ref name, value);
        }

        public string CompanyCustomer
        {
            get => companyCustomer; set => SetProperty(ref companyCustomer, value);
        }

        public string CompanyExecutor
        {
            get => companyExecutor; set => SetProperty(ref companyExecutor, value);
        }

        public Employee Employee
        {
            get => employee; set => SetProperty(ref employee, value);
        }

        public Employee Leader
        {
            get => leader; set => SetProperty(ref leader, value);
        }

        public ICollection<Employee> Executors
        {
            get => executors; set => SetProperty(ref executors, value);
        }

        public DateTime Start
        {
            get => start; set => SetProperty(ref start, value);
        }

        public DateTime Finish
        {
            get => finish; set => SetProperty(ref finish, value);
        }

        public Priority Priority
        {
            get => priority; set => SetProperty(ref priority, value);
        }

        #endregion

        #region Methods
        public async void LoadItemId(int id)
        {
            try
            {
                var item = await ProjectStore.GetItemAsync(id);
                Id = item.Id;
                Name = item.Name;
                CompanyCustomer = item.CompanyCustomer;
                CompanyExecutor = item.CompanyExecutor;
                Employee = item.Employee;
                Leader = item.Leader;
                Executors = item.Executors;
                Start = item.Start;
                Finish = item.Finish;
                Priority = item.Priority;
            }
            catch (Exception)
            {
                Debug.WriteLine("Не удалось добавить проект!");
            }
        }
        #endregion

        #region Commands
        public Command RemoveProject
        {
            get => new Command(async() =>
            {
                await ProjectStore.DeleteItemAsync(ItemId);
                await Shell.Current.GoToAsync("..");    
            });
        }
        public Command EditProject
        {
            get => new Command(async () =>
            {
                await Shell.Current.GoToAsync($"{nameof(EditProjectPage)}?{nameof(EditProjectViewModel.ItemId)}={ItemId}");
            });
        }
        #endregion
    }
}
