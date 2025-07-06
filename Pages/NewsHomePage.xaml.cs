using NewsApp.Models;
using NewsApp.Services;
using NewsApp.Pages; // Add this using directive for NewsListPage and NewsDetailPage
using System.Collections.Generic; // Ensure List is available

namespace NewsApp.Pages;

public partial class NewsHomePage : ContentPage
{
    // Using a List<Article> for ArticleList, as it's populated once and then bound.
    // If dynamic additions/removals were frequent *after* initial load, ObservableCollection would be better.
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
        // Initialize ArticleList BEFORE calling GetBreakingNews
        ArticleList = new List<Article>();
        CvCategories.ItemsSource = CategoryList;
        // Call GetBreakingNews asynchronously, suppressing the warning about awaiting void
        _ = GetBreakingNews();
    }

    /// <summary>
    /// Fetches breaking news articles from the GNews API.
    /// </summary>
    private async Task GetBreakingNews()
    {
        try
        {
            var apiService = new ApiService();
            // Fetch some default breaking news, e.g., "General" or "World"
            var newsResult = await apiService.GetNews("General");

            // Clear the list before adding new items to prevent duplicates on re-fetch
            ArticleList.Clear();

            // Check if newsResult and its Articles collection are not null before iterating
            if (newsResult != null && newsResult.Articles != null)
            {
                foreach (var item in newsResult.Articles)
                {
                    ArticleList.Add(item);
                }
            }
            // Set the ItemsSource of the Breaking News CollectionView
            CvNews.ItemsSource = ArticleList;
        }
        catch (Exception ex)
        {
            // Log the error for debugging purposes
            Console.WriteLine($"Error fetching breaking news: {ex.Message}");
            // Inform the user about the error
            await DisplayAlert("Error", "Failed to load breaking news. Please check your internet connection or try again later.", "OK");
        }
    }

    /// <summary>
    /// Handles the selection of a category in the CvCategories CollectionView.
    /// Navigates to the NewsListPage, passing the selected category name.
    /// </summary>
    /// <param name="sender">The sender of the event (CollectionView).</param>
    /// <param name="e">The event arguments containing information about the selection change.</param>
    private async void OnCategorySelected(object sender, SelectionChangedEventArgs e)
    {
        // Ensure an item was actually selected
        if (e.CurrentSelection != null && e.CurrentSelection.Any())
        {
            // Get the selected category
            var selectedCategory = e.CurrentSelection.FirstOrDefault() as Category;

            if (selectedCategory != null)
            {
                // Navigate to NewsListPage, passing the selected category name
                await Navigation.PushAsync(new NewsListPage(selectedCategory.Name));
                // Deselect the item to allow it to be selected again
                ((CollectionView)sender).SelectedItem = null;
            }
        }
    }

    /// <summary>
    /// Handles the selection of a breaking news article in the CvNews CollectionView.
    /// Navigates to the NewsDetailPage, passing the selected article.
    /// </summary>
    /// <param name="sender">The sender of the event (CollectionView).</param>
    /// <param name="e">The event arguments containing information about the selection change.</param>
    private async void OnBreakingNewsSelected(object sender, SelectionChangedEventArgs e)
    {
        // Ensure an item was actually selected
        if (e.CurrentSelection != null && e.CurrentSelection.Any())
        {
            // Get the selected article
            var selectedArticle = e.CurrentSelection.FirstOrDefault() as Article;

            if (selectedArticle != null)
            {
                // Navigate to NewsDetailPage, passing the selected article object
                await Navigation.PushAsync(new NewsDetailPage(selectedArticle));
                // Deselect the item to allow it to be selected again
                ((CollectionView)sender).SelectedItem = null;
            }
        }
    }
}