using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using TestTask.Model;
using TestTask.ViewModels.Others;
using Xamarin.Forms;

namespace TestTask.ViewModels.Employees
{
    class NewEmployeeViewModel : BaseViewModel
    {
        public NewEmployeeViewModel()
        {
            Title = "New employee";
            LoadEmployees();

            SaveCommand = new Command(OnSave, ValidateSave);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();

            InitNewEmployee();
        }

        #region Fields
        private int _itemId;
        private string _name;
        private string _surname;
        private string _patronumic;
        private string _email;

        private bool _isValidate;

        private ICollection<Project> _leaderProjects;
        private ICollection<Project> _employeeProjects;
        private ICollection<Project> _executorProjects;

        private Project _selectedProjectLeader;
        private Project _selectedProjectEmployee;
        private Project _selectedProjectExecutor;
        public List<Project> Projects { get; private set; }
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

        public string Surname
        {
            get => _surname; set => SetProperty(ref _surname, value);
        }

        public string Patronymic
        {
            get => _patronumic; set => SetProperty(ref _patronumic, value);
        }

        public string Email
        {
            get => _email; set => SetProperty(ref _email, value);
        }

        public bool IsValidate
        {
            get => _isValidate; set => SetProperty(ref _isValidate, value);
        }

        public ICollection<Project> LeaderProjects
        {
            get => _leaderProjects; set => SetProperty(ref _leaderProjects, value);
        }
        public ICollection<Project> EmployeeProjects
        {
            get => _employeeProjects; set => SetProperty(ref _employeeProjects, value);
        }
        public ICollection<Project> ExecutorProjects
        {
            get => _executorProjects; set => SetProperty(ref _executorProjects, value);
        }

        public Project SelectedProjectLeader
        {
            get => _selectedProjectLeader;
            set
            {
                SetProperty(ref _selectedProjectLeader, value);
                AddItemToCollection(value, LeaderProjects);
            }
        }

        public Project SelectedProjectEmployee
        {
            get => _selectedProjectEmployee;
            set
            {
                SetProperty(ref _selectedProjectEmployee, value);
                AddItemToCollection(value, EmployeeProjects);
            }
        }

        public Project SelectedProjectExecutor
        {
            get => _selectedProjectExecutor;
            set
            {
                SetProperty(ref _selectedProjectExecutor, value);
                AddItemToCollection(value, ExecutorProjects);
            }
        }

        #endregion

        #region Methods
        void InitNewEmployee()
        {
            Name = string.Empty;
            Surname = string.Empty;
            Patronymic = string.Empty;
            Email = string.Empty;
            LeaderProjects = new ObservableCollection<Project>();
            EmployeeProjects = new ObservableCollection<Project>();
            ExecutorProjects = new ObservableCollection<Project>();
        }

        public void AddItemToCollection(Project item, ICollection<Project> collection)
        {
            try
            {
                if (collection.Contains(item)) return;
                collection.Add(item);
                SaveCommand.ChangeCanExecute();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to Add Project: " + ex.Message);
            }
        }
        private async void LoadEmployees()
        {
            var items = await ProjectStore.GetItemsAsync();
            Projects = items.ToList();
        }
        public Command DebugCommand => new Command(() => ValidateSave());
        private bool ValidateSave()
        {
            return new string[]
            {
                Name,
                Surname,
                Patronymic,
                Email
            }
            .All(x => !string.IsNullOrEmpty(x.ToString())) && IsValidate;
        }
        #endregion

        #region Commands
        public Command RemoveExecutorProject
        {
            get => new Command((item) =>
            {
                ExecutorProjects.Remove(item as Project);
                SaveCommand.ChangeCanExecute();
            });
        }

        public Command RemoveEmployeeProject
        {
            get => new Command((item) =>
            {
                EmployeeProjects.Remove(item as Project);
                SaveCommand.ChangeCanExecute();
            });
        }

        public Command RemoveLeaderProject
        {
            get => new Command((item) =>
            {
                LeaderProjects.Remove(item as Project);
                SaveCommand.ChangeCanExecute();
            });
        }
        public Command SaveCommand { get; set; }
        public async void OnSave()
        {
            var newItem = new Employee
            {
                Name = Name,
                Surname = Surname,
                Patronymic = Patronymic,
                Email = Email,
                EmployeeProjects = EmployeeProjects,
                ExecutorProjects = ExecutorProjects,
                LeaderProjects = LeaderProjects
            };

            await EmployeeStore.AddItemAsync(newItem);

            await Shell.Current.GoToAsync("..");
        }
        public Command CancelCommand => new Command(async () => await Shell.Current.GoToAsync(".."));
        #endregion
    }
}
