using NewsApp.Models;
using System.Windows.Input; // Required for ICommand

namespace NewsApp.Pages;

public partial class NewsDetailPage : ContentPage
{
    // Property to hold the selected article
    public Article SelectedArticle { get; set; }
    // Command to open the article URL
    public ICommand OpenUrlCommand { get; }

    // Constructor that accepts an Article object
    public NewsDetailPage(Article article)
    {
        InitializeComponent();

        // Set the selected article
        SelectedArticle = article;
        // Set the BindingContext of the page to the selected article
        BindingContext = SelectedArticle;

        // Initialize the command to open the URL
        OpenUrlCommand = new Command(async () => await OpenUrl(SelectedArticle.Url));
    }

    /// <summary>
    /// Opens the provided URL in the default web browser.
    /// </summary>
    /// <param name="url">The URL to open.</param>
    private async Task OpenUrl(string url)
    {
        try
        {
            if (!string.IsNullOrEmpty(url))
            {
                // Use Launcher to open the URL
                await Launcher.OpenAsync(new Uri(url));
            }
        }
        catch (Exception ex)
        {
            // Log or display an error if the URL cannot be opened
            Console.WriteLine($"Error opening URL: {ex.Message}");
            await DisplayAlert("Error", "Could not open the article link.", "OK");
        }
    }
}