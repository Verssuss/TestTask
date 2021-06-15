using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
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
            SelectDateStart = DateTime.Now;
            SelectDateFinish = DateTime.Now;
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

        private string _selectPriority = "All";
        private DateTime _selectDateStart;
        private DateTime _selectDateFinish;
        private DateTime _minDateTo;

        public string SelectPriority { get => _selectPriority; set { SetProperty(ref _selectPriority, value); LoadProjects.Execute(null); } }

        public string[] PriorityItems => new[] { "All", "Low", "Middle", "High" };

        public DateTime SelectDateStart 
        { get => _selectDateStart; 
            set 
            {
                SetProperty(ref _selectDateStart, value); 
                LoadProjects.Execute(null);
                MinDateTo = value;
            } 
        }
        public DateTime SelectDateFinish 
        { 
            get => _selectDateFinish; 
            set 
            { 
                SetProperty(ref _selectDateFinish, value); 
                LoadProjects.Execute(null);
            }
        }
        public DateTime MinDateTo { get => _minDateTo; set => SetProperty(ref _minDateTo, value); }
        public Command LoadProjects //Загрузить проекты
        {
            get => new Command(async () =>
            {
                IsBusy = true;

                try
                {
                    Projects.Clear();
                    var items = await ProjectStore.GetItemsAsync(true);

                    IEnumerable<Project> filterEnumeration;
                    if (SelectPriority == PriorityItems[0]) filterEnumeration = items;
                    else filterEnumeration = items.Where(x => x.Priority.ToString() == SelectPriority);

                    filterEnumeration = filterEnumeration.Where(x => x.Start.Date >= SelectDateStart.Date && x.Start.Date <= SelectDateFinish.Date);

                    if (!string.IsNullOrEmpty(SearchText)) filterEnumeration = filterEnumeration.Where(x =>string.Join("", x.Name, x.CompanyCustomer, x.CompanyExecutor, x.Employee, x.Leader).ToUpper().Contains(SearchText.ToUpper())); ;
                    
                    foreach (var item in filterEnumeration)
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
