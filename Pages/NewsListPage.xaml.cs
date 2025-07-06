using NewsApp.Models;
using NewsApp.Services;
using System.Collections.ObjectModel; // Use ObservableCollection for dynamic updates

namespace NewsApp.Pages;

public partial class NewsListPage : ContentPage
{
    // ObservableCollection to hold the list of articles, allowing UI to update automatically
    public ObservableCollection<Article> Articles { get; set; } = new ObservableCollection<Article>();
    public string PageTitle { get; set; } // Property to bind to the page title

    // Constructor that accepts the category name
    public NewsListPage(string categoryName)
    {
        InitializeComponent();
        // Set the page title dynamically
        PageTitle = categoryName;
        BindingContext = this; // Set the binding context to this page to access PageTitle

        // Asynchronously load news articles for the given category
        _ = LoadNews(categoryName);
    }

    /// <summary>
    /// Loads news articles from the API based on the provided category name.
    /// </summary>
    /// <param name="categoryName">The category of news to fetch.</param>
    private async Task LoadNews(string categoryName)
    {
        try
        {
            var apiService = new ApiService();
            var newsResult = await apiService.GetNews(categoryName);

            // Clear existing articles and add new ones
            Articles.Clear();
            foreach (var article in newsResult.Articles)
            {
                Articles.Add(article);
            }
            // Set the ItemsSource of the CollectionView to the ObservableCollection
            CvNewsList.ItemsSource = Articles;
        }
        catch (Exception ex)
        {
            // Log or display an error message if news fetching fails
            Console.WriteLine($"Error loading news: {ex.Message}");
            // Optionally, show an alert to the user
            await DisplayAlert("Error", "Failed to load news. Please check your internet connection.", "OK");
        }
    }

    /// <summary>
    /// Handles the selection of a news article in the CollectionView.
    /// Navigates to the NewsDetailPage, passing the selected article.
    /// </summary>
    /// <param name="sender">The sender of the event (CollectionView).</param>
    /// <param name="e">The event arguments containing information about the selection change.</param>
    private async void OnNewsSelected(object sender, SelectionChangedEventArgs e)
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