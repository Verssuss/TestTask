using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask.ViewModels.Employees;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TestTask.Views.Employees
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmployeeDetailPage : ContentPage
    {
        public EmployeeDetailPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            (BindingContext as EmployeeDetailViewModel).OnAppearing();
        }
    }
}