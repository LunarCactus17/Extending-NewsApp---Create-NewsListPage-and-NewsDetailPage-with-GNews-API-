using NewsApp.Models;
using NewsApp.Services;
using System.Collections.ObjectModel; 
namespace NewsApp.Pages;

public partial class NewsListPage : ContentPage
{
    
    public ObservableCollection<Article> Articles { get; set; } = new ObservableCollection<Article>();
    public string PageTitle { get; set; } 
    
    public NewsListPage(string categoryName)
    {
        InitializeComponent();
        
        PageTitle = categoryName;
        BindingContext = this; 

        
        _ = LoadNews(categoryName);
    }

    
    private async Task LoadNews(string categoryName)
    {
        try
        {
            var apiService = new ApiService();
            var newsResult = await apiService.GetNews(categoryName);

            
            Articles.Clear();
            foreach (var article in newsResult.Articles)
            {
                Articles.Add(article);
            }
            
            CvNewsList.ItemsSource = Articles;
        }
        catch (Exception ex)
        {
            
            Console.WriteLine($"Error loading news: {ex.Message}");
            
            await DisplayAlert("Error", "Failed to load news. Please check your internet connection.", "OK");
        }
    }

   
    private async void OnNewsSelected(object sender, SelectionChangedEventArgs e)
    {
        
        if (e.CurrentSelection != null && e.CurrentSelection.Any())
        {
            
            var selectedArticle = e.CurrentSelection.FirstOrDefault() as Article;

            if (selectedArticle != null)
            {
                
                await Navigation.PushAsync(new NewsDetailPage(selectedArticle));
                
                ((CollectionView)sender).SelectedItem = null;
            }
        }
    }
}
