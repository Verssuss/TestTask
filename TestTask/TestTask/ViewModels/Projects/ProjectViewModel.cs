using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TestTask.Model;
using TestTask.ViewModels.Others;
using TestTask.Views;
using TestTask.Views.Projects;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TestTask.ViewModels.Projects
{
    class ProjectViewModel : BaseViewModel
    {
        public ProjectViewModel()
        {
            Title = "Projects";
            Projects = new ObservableCollection<Project>();
        }

        #region Fields
        #endregion

        #region Properties
        public ObservableCollection<Project> Projects { get; private set; }
        #endregion

        #region Methods
        public void OnAppearing()
        {
            IsBusy = true;
            //SelectedItem = null;
        }
        #endregion

        #region Commands
        public Command<Project> ItemTapped { get; }

        public ICommand SearchCommand
        {
            get => new Command<string>((text) =>
            {
                LoadProjects.Execute(null);
            });
        }
        private string searchText;
        public string SearchText { get => searchText; set { SetProperty(ref searchText, value); SearchCommand.Execute(value); } }
        public Command AddProject //Добавить проект
        {
            get => new Command(async () =>
            {
                await Shell.Current.GoToAsync(nameof(NewProjectPage));
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
        public Command LoadProjects //Загрузить проекты
        {
            get => new Command(async () =>
            {
                IsBusy = true;

                try
                {
                    Projects.Clear();
                    var items = await ProjectStore.GetItemsAsync(true);

                    IEnumerable<Project> enumFilter;
                    if (string.IsNullOrEmpty(SearchText)) enumFilter = items;
                    else
                    enumFilter = items.Where(x => string.Join("", x.Name, x.CompanyCustomer, x.CompanyExecutor, x.Employee, x.Leader).ToUpper().Contains(SearchText.ToUpper()));
                    
                    foreach (var item in enumFilter)
                    {
                        Projects.Add(item);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
                finally
                {
                    IsBusy = false;
                }
            });
        }
        #endregion
    }
}
