using NewsApp.Models;
using System.Windows.Input; 

namespace NewsApp.Pages;

public partial class NewsDetailPage : ContentPage
{
    
    public Article SelectedArticle { get; set; }
    
    public ICommand OpenUrlCommand { get; }

    
    public NewsDetailPage(Article article)
    {
        InitializeComponent();

        
        SelectedArticle = article;
        
        BindingContext = SelectedArticle;

        
        OpenUrlCommand = new Command(async () => await OpenUrl(SelectedArticle.Url));
    }


    private async Task OpenUrl(string url)
    {
        try
        {
            if (!string.IsNullOrEmpty(url))
            {
                
                await Launcher.OpenAsync(new Uri(url));
            }
        }
        catch (Exception ex)
        {
            
            Console.WriteLine($"Error opening URL: {ex.Message}");
            await DisplayAlert("Error", "Could not open the article link.", "OK");
        }
    }
}
