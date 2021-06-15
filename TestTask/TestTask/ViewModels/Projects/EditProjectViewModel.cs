using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
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
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    class EditProjectViewModel : BaseViewModel
    {
        public EditProjectViewModel()
        {
            Title = "Edit";
            LoadEmployees();
            SaveCommand = new Command(OnSave, ValidateSave);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
            EmployeeExecutors = new ObservableCollection<Employee>();
            InitProject();

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

        private DateTime _minDateTo;
        private Employee _selectedEmployee;
        #endregion

        #region Properties
        public int ItemId
        {
            get => _itemId; set { _itemId = value; InitEditingProject(value); }
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
        public DateTime MinDateTo { get => _minDateTo; set => SetProperty(ref _minDateTo, value); }

        private Priority _selectedPriority;

        public Priority SelectedPriority { get => _selectedPriority; set => SetProperty(ref _selectedPriority, value); }

        public ObservableCollection<Employee> EmployeeExecutors { get; private set; }
        public List<Employee> Employees { get; private set; }

        public List<string> Priorities => Enum.GetNames(typeof(Priority)).Select(x => x.ToString()).ToList();
        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                SetProperty(ref _selectedEmployee, value);
                AddItemToCollection(value);
            }
        }

        #endregion

        #region Methods
        public void AddItemToCollection(Employee item)
        {
            try
            {
                if (Executors.Contains(item)) return;
                Executors.Add(item);
                SaveCommand.ChangeCanExecute();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to Load Item: " + ex.Message);
            }
        }

        void InitProject()
        {
            Name = string.Empty;
            CompanyCustomer = string.Empty;
            CompanyExecutor = string.Empty;
            Start = DateTime.Now;
            Finish = DateTime.Now.AddDays(1);
            Priority = Priority.Low;
            Leader = Employees.FirstOrDefault();
            Employee = Employees.FirstOrDefault();
            Executors = new ObservableCollection<Employee>();
        }

        async void InitEditingProject(int id)
        {
            var item = await ProjectStore.GetItemAsync(id);
            Name = item.Name;
            CompanyCustomer = item.CompanyCustomer;
            CompanyExecutor = item.CompanyExecutor;
            Start = item.Start;
            Finish = item.Finish;
            Priority = item.Priority;
            Leader = item.Leader;
            Employee = item.Employee;
            foreach (var exec in item.Executors)
            {
                Executors.Add(exec);
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
                Priority
            }
            .All(x => !string.IsNullOrEmpty(x.ToString())) && Executors.Count > 0;
        }

        async void OnSave()
        {
            var editItem = await ProjectStore.GetItemAsync(ItemId);

            editItem.Name = Name.Trim();
            editItem.CompanyCustomer = CompanyCustomer.Trim();
            editItem.CompanyExecutor = CompanyExecutor.Trim();
            editItem.Employee = Employee;
            editItem.Leader = Leader;
            editItem.Start = Start;
            editItem.Finish = Finish;
            editItem.Priority = Priority;
            editItem.Executors = Executors;

            await ProjectStore.UpdateItemAsync(editItem);

            await Shell.Current.GoToAsync("..");
        }

        #endregion

        #region Commands
        public Command SaveCommand { get; set; }

        public Command RemoveEmployee
        {
            get => new Command((item) =>
            {
                Executors.Remove(item as Employee);
                SaveCommand.ChangeCanExecute();
            });
        }
        public Command CancelCommand => new Command(async () => await Shell.Current.GoToAsync(".."));
        #endregion
    }
}
