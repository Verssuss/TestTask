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

        private Employee selectedEmployee;
        public List<Employee> Employees { get; private set; }
        #endregion

        #region Properties
        public int ItemId
        {
            get => itemId; set => SetProperty(ref itemId, value);
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

        public Employee SelectedEmployee
        {
            get => selectedEmployee;
            set
            {
                SetProperty(ref selectedEmployee, value);
                AddItemById(value);
            }
        }

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
        public Command DebugCommand => new Command(() => ValidateSave());
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
                Name = Name,
                CompanyCustomer = CompanyCustomer,
                CompanyExecutor = CompanyExecutor,
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
