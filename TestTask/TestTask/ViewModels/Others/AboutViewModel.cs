using System;
using System.Diagnostics;
using System.Windows.Input;
using TestTask.Model;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TestTask.ViewModels.Others
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamarin-quickstart"));
        }
        public ICommand OpenWebCommand { get; }
    }
}