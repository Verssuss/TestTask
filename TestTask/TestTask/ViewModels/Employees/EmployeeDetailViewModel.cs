using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using TestTask.Model;
using TestTask.ViewModels.Others;
using TestTask.ViewModels.Projects;
using TestTask.Views.Employees;
using TestTask.Views.Projects;
using Xamarin.Forms;

namespace TestTask.ViewModels.Employees
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    class EmployeeDetailViewModel : BaseViewModel
    {
        public EmployeeDetailViewModel()
        {
            Title = "Information";
        }

        #region Fields
        private int _itemId;
        private string _name;
        private string _surname;
        private string _patronymic;
        private string _email;
        private ObservableCollection<Project> _leaderProjects;
        private ObservableCollection<Project> _employeeProjects;
        private ObservableCollection<Project> _executorProjects;
        #endregion

        #region Properies
        public int Id { get; set; }
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
            get => _patronymic; set => SetProperty(ref _patronymic, value);
        }

        public string Email
        {
            get => _email; set => SetProperty(ref _email, value);
        }

        public ObservableCollection<Project> LeaderProjects
        {
            get => _leaderProjects; set => SetProperty(ref _leaderProjects, value);
        }

        public ObservableCollection<Project> EmployeeProjects
        {
            get => _employeeProjects; set => SetProperty(ref _employeeProjects, value);
        }

        public ObservableCollection<Project> ExecutorProjects
        {
            get => _executorProjects; set => SetProperty(ref _executorProjects, value);
        }

        #endregion

        #region Methods
        public void OnAppearing()
        {
            LoadItemId(ItemId);
        }
        public async void LoadItemId(int id)
        {
            try
            {
                var item = await EmployeeStore.GetItemAsync(id);
                Id = item.Id;
                Name = item.Name;
                Surname = item.Surname;
                Patronymic = item.Patronymic;
                Email = item.Email;
                LeaderProjects = new ObservableCollection<Project>(item.LeaderProjects);
                EmployeeProjects = new ObservableCollection<Project>(item.EmployeeProjects);
                ExecutorProjects = new ObservableCollection<Project>(item.ExecutorProjects);
            }
            catch (Exception)
            {
                Debug.WriteLine("Не удалось добавить проект!");
            }
        }
        #endregion

        #region Commands
        public Command RemoveEmployee
        {
            get => new Command(async () =>
            {
                await EmployeeStore.DeleteItemAsync(ItemId);
                await Shell.Current.GoToAsync("..");
            });
        }
        public Command EditEmployee
        {
            get => new Command(async () =>
            {
                await Shell.Current.GoToAsync($"{nameof(EditEmployeePage)}?{nameof(EditEmployeeViewModel.ItemId)}={ItemId}");
            });
        }
        public Command ShowProject //Просмотреть проект
        {
            get => new Command(async (item) =>
            {
                var project = item as Project;
                if (project == null)
                    return;

                await Shell.Current.GoToAsync($"{nameof(ProjectDetailPage)}?{nameof(ProjectDetailViewModel.ItemId)}={project.Id}");
            });
        }
        #endregion
    }
}
