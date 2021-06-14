using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask.ViewModels;
using TestTask.ViewModels.Projects;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TestTask.Views.Projects
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProjectPage : ContentPage
    {
        public ProjectPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            (BindingContext as ProjectViewModel).OnAppearing();
        }
    }
}