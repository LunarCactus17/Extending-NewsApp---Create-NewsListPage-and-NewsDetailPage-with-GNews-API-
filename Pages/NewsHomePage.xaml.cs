using NewsApp.Models;
using NewsApp.Services;
using NewsApp.Pages; // Add this using directive for NewsListPage and NewsDetailPage
using System.Collections.Generic; // Ensure List is available

namespace NewsApp.Pages;

public partial class NewsHomePage : ContentPage
{
    
    public List<Article> ArticleList { get; set; }
    public List<Category> CategoryList = new List<Category>()
    {
        new Category(){Name="World", ImageUrl = "world.png"},
        new Category(){Name = "Nation" , ImageUrl="nation.png"},
        new Category(){Name = "Business" , ImageUrl="business.png"},
        new Category(){Name = "Technology" , ImageUrl="technology.png"},
        new Category(){Name = "Entertainment", ImageUrl = "entertainment.png"},
        new Category(){Name = "Sports" , ImageUrl="sports.png"},
        new Category(){Name = "Science", ImageUrl= "science.png"},
        new Category(){Name = "Health", ImageUrl="health.png"},
    };

    public NewsHomePage()
    {
        InitializeComponent();
        
        ArticleList = new List<Article>();
        CvCategories.ItemsSource = CategoryList;
        
        _ = GetBreakingNews();
    }

    
    private async Task GetBreakingNews()
    {
        try
        {
            var apiService = new ApiService();
            
            var newsResult = await apiService.GetNews("General");

            
            ArticleList.Clear();

            
            if (newsResult != null && newsResult.Articles != null)
            {
                foreach (var item in newsResult.Articles)
                {
                    ArticleList.Add(item);
                }
            }
           
            CvNews.ItemsSource = ArticleList;
        }
        catch (Exception ex)
        {
            
            Console.WriteLine($"Error fetching breaking news: {ex.Message}");
            
            await DisplayAlert("Error", "Failed to load breaking news. Please check your internet connection or try again later.", "OK");
        }
    }

    
    private async void OnCategorySelected(object sender, SelectionChangedEventArgs e)
    {
        
        if (e.CurrentSelection != null && e.CurrentSelection.Any())
        {
            
            var selectedCategory = e.CurrentSelection.FirstOrDefault() as Category;

            if (selectedCategory != null)
            {
                
                await Navigation.PushAsync(new NewsListPage(selectedCategory.Name));
                
                ((CollectionView)sender).SelectedItem = null;
            }
        }
    }

    
    private async void OnBreakingNewsSelected(object sender, SelectionChangedEventArgs e)
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
