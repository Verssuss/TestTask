using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using TestTask.Model;
using TestTask.ViewModels.Others;
using Xamarin.Forms;

namespace TestTask.ViewModels.Employees
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    class EditEmployeeViewModel : BaseViewModel
    {
        public EditEmployeeViewModel()
        {
            Title = "Edit";
            LoadProjects();
            SaveCommand = new Command(OnSave, ValidateSave);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
            LeaderProjects = new ObservableCollection<Project>();
            EmployeeProjects = new ObservableCollection<Project>();
            ExecutorProjects = new ObservableCollection<Project>();
            InitEmployee();
        }

        #region Fields
        private int _itemId;
        private string _name;
        private string _surname;
        private string _patronymic;
        private string _email;
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
            get => _itemId; set { _itemId = value; InitEditingEmployee(value); }
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
            get => _patronymic; set => SetProperty(ref _patronymic, value);
        }

        public string Email
        {
            get => _email; set => SetProperty(ref _email, value);
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

        void InitEmployee()
        {
            Name = string.Empty;
            Surname = string.Empty;
            Patronymic = string.Empty;
            Email = string.Empty;
            LeaderProjects = new ObservableCollection<Project>();
            EmployeeProjects = new ObservableCollection<Project>();
            ExecutorProjects = new ObservableCollection<Project>();
        }

        async void InitEditingEmployee(int id)
        {
            var item = await EmployeeStore.GetItemAsync(id);
            Name = item.Name;
            Surname = item.Surname;
            Patronymic = item.Patronymic;
            Email = item.Email;

            foreach (var lead in item.LeaderProjects)
                LeaderProjects.Add(lead);

            foreach (var empl in item.EmployeeProjects)
                EmployeeProjects.Add(empl);

            foreach (var exec in item.ExecutorProjects)
                ExecutorProjects.Add(exec);
        }

        private async void LoadProjects()
        {
            var items = await ProjectStore.GetItemsAsync();
            Projects = items.ToList();
        }

        private bool ValidateSave()
        {
            return new string[]
            {
                Name,
                Surname,
                Patronymic,
                Email
            }
            .All(x => !string.IsNullOrEmpty(x));
        }

        async void OnSave()
        {
            var editItem = await EmployeeStore.GetItemAsync(ItemId);

            editItem.Name = Name;
            editItem.Surname = Surname;
            editItem.Patronymic = Patronymic;
            editItem.Email = Email;
            editItem.LeaderProjects = LeaderProjects;
            editItem.EmployeeProjects = EmployeeProjects;
            editItem.ExecutorProjects = ExecutorProjects;

            await EmployeeStore.UpdateItemAsync(editItem);

            await Shell.Current.GoToAsync("..");
        }
        #endregion

        #region Commands
        public Command SaveCommand { get; set; }

        public Command RemoveLeaderProjects
        {
            get => new Command((item) =>
            {
                LeaderProjects.Remove(item as Project);
                SaveCommand.ChangeCanExecute();
            });
        }
        
        public Command RemoveEmployeeProjects
        {
            get => new Command((item) =>
            {
                EmployeeProjects.Remove(item as Project);
                SaveCommand.ChangeCanExecute();
            });
        }

        public Command RemoveExecutorProjects
        {
            get => new Command((item) =>
            {
                ExecutorProjects.Remove(item as Project);
                SaveCommand.ChangeCanExecute();
            });
        }

        public Command CancelCommand => new Command(async () => await Shell.Current.GoToAsync(".."));
        #endregion
    }
}
