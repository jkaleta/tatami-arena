using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Linq;
using TatamiArena.Core.Models;
using TatamiArena.Data;

namespace TatamiArena.App.Pages
{
    public sealed partial class CompetitionsPage : Page
    {
        private readonly TatamiArenaContext _context;

        public CompetitionsPage()
        {
            this.InitializeComponent();
            _context = new TatamiArenaContext();
            LoadCompetitions();
        }

        private void LoadCompetitions()
        {
            CompetitionsListView.ItemsSource = _context.Competitions
                .OrderByDescending(c => c.CreatedAt)
                .ToList();
        }

        private async void AddCompetition_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog
            {
                Title = "Add Competition",
                PrimaryButtonText = "Save",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                XamlRoot = this.XamlRoot
            };

            var panel = new StackPanel { Spacing = 8 };
            
            var nameBox = new TextBox { PlaceholderText = "Name" };
            var descriptionBox = new TextBox 
            { 
                PlaceholderText = "Description",
                AcceptsReturn = true,
                TextWrapping = TextWrapping.Wrap,
                Height = 100
            };

            panel.Children.Add(new TextBlock { Text = "Name:" });
            panel.Children.Add(nameBox);
            panel.Children.Add(new TextBlock { Text = "Description:" });
            panel.Children.Add(descriptionBox);

            dialog.Content = panel;

            dialog.PrimaryButtonClick += async (s, args) =>
            {
                if (string.IsNullOrWhiteSpace(nameBox.Text))
                {
                    var errorDialog = new ContentDialog
                    {
                        Title = "Error",
                        Content = "Please enter a name for the competition.",
                        CloseButtonText = "OK",
                        XamlRoot = this.XamlRoot
                    };
                    await errorDialog.ShowAsync();
                    args.Cancel = true;
                    return;
                }

                var competition = new Competition
                {
                    Name = nameBox.Text,
                    Description = descriptionBox.Text,
                    CreatedAt = DateTime.Now
                };

                _context.Competitions.Add(competition);
                await _context.SaveChangesAsync();
                LoadCompetitions();
            };

            await dialog.ShowAsync();
        }
    }
} 