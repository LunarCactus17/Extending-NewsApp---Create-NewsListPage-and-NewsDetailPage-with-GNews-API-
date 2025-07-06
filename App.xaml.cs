using NewsApp.Pages;
using Microsoft.Maui.Controls; // Ensure this is included for NavigationPage

namespace NewsApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new NewsHomePage());
        }
    }
}

