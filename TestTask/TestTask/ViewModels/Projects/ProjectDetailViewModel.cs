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
        private int _itemId;
        private string _name;
        private string _companyCustomer;
        private string _companyExecutor;
        private Employee _employee;
        private Employee _leader;
        private ICollection<Employee> _executors;
        private DateTime _start;
        private DateTime _finish;
        private Priority _priority;
        #endregion

        #region Properies
        public int Id { get; set; }
        public int ItemId
        {
            get => _itemId; set { _itemId = value; LoadItemId(value); }
        }

        public string Name
        {
            get => _name; set => SetProperty(ref _name, value);
        }

        public string CompanyCustomer
        {
            get => _companyCustomer; set => SetProperty(ref _companyCustomer, value);
        }

        public string CompanyExecutor
        {
            get => _companyExecutor; set => SetProperty(ref _companyExecutor, value);
        }

        public Employee Employee
        {
            get => _employee; set => SetProperty(ref _employee, value);
        }

        public Employee Leader
        {
            get => _leader; set => SetProperty(ref _leader, value);
        }

        public ICollection<Employee> Executors
        {
            get => _executors; set => SetProperty(ref _executors, value);
        }

        public DateTime Start
        {
            get => _start; set => SetProperty(ref _start, value);
        }

        public DateTime Finish
        {
            get => _finish; set => SetProperty(ref _finish, value);
        }

        public Priority Priority
        {
            get => _priority; set => SetProperty(ref _priority, value);
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
