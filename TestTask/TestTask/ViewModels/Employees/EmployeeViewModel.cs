using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;
using TestTask.Model;
using TestTask.ViewModels.Others;
using TestTask.ViewModels.Projects;
using TestTask.Views.Employees;
using TestTask.Views.Projects;
using Xamarin.Forms;

namespace TestTask.ViewModels.Employees
{
    class EmployeeViewModel : BaseViewModel
    {
        public EmployeeViewModel()
        {
            Title = "Projects";
            Employees = new ObservableCollection<Employee>();
        }

        #region Fields
        #endregion

        #region Properties
        public ObservableCollection<Employee> Employees { get; private set; }
        #endregion

        #region Methods
        public void OnAppearing()
        {
            IsBusy = true;
        }
        #endregion

        #region Commands
        public Command<Project> ItemTapped { get; }

        public ICommand SearchCommand
        {
            get => new Command<string>((text) =>
            {
                LoadEmployees.Execute(null);
            });
        }
        private string searchText;
        public string SearchText { get => searchText; set { SetProperty(ref searchText, value); SearchCommand.Execute(value); } }
        public Command AddEmployee
        {
            get => new Command(async () =>
            {
                await Shell.Current.GoToAsync(nameof(NewEmployeePage));
            });
        }
        public Command ShowEmployee
        {
            get => new Command(async (item) =>
            {
                var employee = item as Employee;
                if (employee == null)
                    return;

                await Shell.Current.GoToAsync($"{nameof(EmployeeDetailPage)}?{nameof(EmployeeDetailViewModel.ItemId)}={employee.Id}");
            });
        }

        public Command LoadEmployees
        {
            get => new Command(async () =>
            {
                IsBusy = true;

                try
                {
                    Employees.Clear();
                    var items = await EmployeeStore.GetItemsAsync(true);

                    IEnumerable<Employee> enumFilter;
                    if (string.IsNullOrEmpty(SearchText)) enumFilter = items;
                    else
                        enumFilter = items.Where(x => string.Join("", x.Name, x.Surname, x.Patronymic, x.Email).ToUpper().Contains(SearchText.ToUpper()));

                    foreach (var item in enumFilter)
                    {
                        Employees.Add(item);
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
