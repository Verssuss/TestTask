using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using TestTask.Model;
using TestTask.ViewModels.Others;
using Xamarin.Forms;

namespace TestTask.ViewModels.Projects
{
   
    class NewProjectViewModel : BaseViewModel
    {
        public NewProjectViewModel()
        {
            Title = "New project";
            LoadEmployees();

            SaveCommand = new Command(OnSave, ValidateSave);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();

            EmployeeExecutors = new ObservableCollection<Employee>();
            InitNewProject();
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

        private Employee _selectedEmployee;
        private DateTime _minDateTo;

        public List<Employee> Employees { get; private set; }
        #endregion

        #region Properties
        public int ItemId
        {
            get => _itemId; set => SetProperty(ref _itemId, value);
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
            get => _start; set { SetProperty(ref _start, value); MinDateTo = value; }
        }

        public DateTime Finish
        {
            get => _finish; set => SetProperty(ref _finish, value);
        }

        public Priority Priority
        {
            get => _priority; set => SetProperty(ref _priority, value);
        }

        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                SetProperty(ref _selectedEmployee, value);
                AddItemById(value);
            }
        }
        public DateTime MinDateTo { get => _minDateTo; set => SetProperty(ref _minDateTo, value); }
        public ObservableCollection<Employee> EmployeeExecutors { get; private set; }
     
        #endregion

        #region Methods
        void InitNewProject()
        {
            Name = string.Empty;
            CompanyCustomer = string.Empty;
            CompanyExecutor = string.Empty;
            Leader = Employees.FirstOrDefault();
            Employee = Employees.FirstOrDefault();
            Start = DateTime.Now;
            Finish = DateTime.Now.AddDays(1);
            Priority = Priority.Low;
            Executors = new ObservableCollection<Employee>();
        }

        public void AddItemById(Employee item)
        {
            try
            {
                if (Executors.Contains(item)) return;
                Executors.Add(item);
                SaveCommand.ChangeCanExecute();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to Add Project: " + ex.Message);
            }
        }

        private async void LoadEmployees()
        {
            var items = await EmployeeStore.GetItemsAsync();
            Employees = items.ToList();
        }
        private bool ValidateSave()
        {
            return new object[]
            {
                Name,
                CompanyCustomer,
                CompanyExecutor,
                Employee,
                Leader,
                Start,
                Finish,
            }
            .All(x => !string.IsNullOrEmpty(x.ToString())) && Executors.Count > 0;
        }
        #endregion

        #region Commands
        public Command RemoveEmployee
        {
            get => new Command((item) =>
            {
                Executors.Remove(item as Employee);
                SaveCommand.ChangeCanExecute();
            });
        }
        public Command SaveCommand { get; set; }
        public async void OnSave()
        {
            var newItem = new Project
            {
                Name = Name.Trim(),
                CompanyCustomer = CompanyCustomer.Trim(),
                CompanyExecutor = CompanyExecutor.Trim(),
                Employee = Employee,
                Leader = Leader,
                Start = Start,
                Finish = Finish,
                Priority = Priority,
                Executors = Executors
            };

            await ProjectStore.AddItemAsync(newItem);

            await Shell.Current.GoToAsync("..");
        }
        public Command CancelCommand => new Command(async () => await Shell.Current.GoToAsync(".."));
        #endregion
    }
}
