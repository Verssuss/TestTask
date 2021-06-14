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
        #endregion

        #region Properties
        public int ItemId
        {
            get => itemId; set { itemId = value; InitEditingProject(value); }
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

        private Priority _selectedPriority;
        public Priority SelectedPriority { get => _selectedPriority; set => SetProperty(ref _selectedPriority, value); }

        public ObservableCollection<Employee> EmployeeExecutors { get; private set; }
        public List<Employee> Employees { get; private set; }

        public List<string> Priorities => Enum.GetNames(typeof(Priority)).Select(x => x.ToString()).ToList();
        public Employee SelectedEmployee
        {
            get => selectedEmployee;
            set
            {
                SetProperty(ref selectedEmployee, value);
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

            editItem.Name = Name;
            editItem.CompanyCustomer = CompanyCustomer;
            editItem.CompanyExecutor = CompanyExecutor;
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
