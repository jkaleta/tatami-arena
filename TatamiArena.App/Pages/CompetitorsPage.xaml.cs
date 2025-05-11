using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using TatamiArena.Core.Models;
using TatamiArena.Data;

namespace TatamiArena.App.Pages
{
    public sealed partial class CompetitorsPage : Page
    {
        private readonly TatamiArenaContext _context;

        public CompetitorsPage()
        {
            this.InitializeComponent();
            _context = new TatamiArenaContext();
            LoadCompetitors();
        }

        private void LoadCompetitors()
        {
            CompetitorsListView.ItemsSource = _context.Competitors
                .Include(c => c.Participations)
                    .ThenInclude(p => p.Category)
                .Include(c => c.Participations)
                    .ThenInclude(p => p.Competition)
                .OrderBy(c => c.Name)
                .ToList();
        }

        private async void AddCompetitor_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog
            {
                Title = "Add Competitor",
                PrimaryButtonText = "Save",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                XamlRoot = this.XamlRoot
            };

            var panel = new StackPanel { Spacing = 8 };

            var nameBox = new TextBox { PlaceholderText = "Name" };
            var clubBox = new TextBox { PlaceholderText = "Club" };
            var ageBox = new NumberBox { PlaceholderText = "Age" };
            var weightBox = new NumberBox { PlaceholderText = "Weight" };
            var sexCombo = new ComboBox
            {
                PlaceholderText = "Select Sex",
                ItemsSource = Enum.GetValues(typeof(Sex))
            };

            var competitionCombo = new ComboBox
            {
                PlaceholderText = "Select Competition",
                ItemsSource = _context.Competitions.ToList(),
                DisplayMemberPath = "Name"
            };

            var suggestedCategoriesList = new ListView
            {
                SelectionMode = ListViewSelectionMode.Multiple,
                Height = 200
            };

            panel.Children.Add(new TextBlock { Text = "Name:" });
            panel.Children.Add(nameBox);
            panel.Children.Add(new TextBlock { Text = "Club:" });
            panel.Children.Add(clubBox);
            panel.Children.Add(new TextBlock { Text = "Age:" });
            panel.Children.Add(ageBox);
            panel.Children.Add(new TextBlock { Text = "Weight:" });
            panel.Children.Add(weightBox);
            panel.Children.Add(new TextBlock { Text = "Sex:" });
            panel.Children.Add(sexCombo);
            panel.Children.Add(new TextBlock { Text = "Competition:" });
            panel.Children.Add(competitionCombo);
            panel.Children.Add(new TextBlock { Text = "Suggested Categories:" });
            panel.Children.Add(suggestedCategoriesList);

            dialog.Content = panel;

            // Update suggested categories when age, weight, sex, or competition changes
            void UpdateSuggestedCategories()
            {
                if (ageBox.Value == null || weightBox.Value == null || 
                    sexCombo.SelectedItem == null || competitionCombo.SelectedItem == null)
                    return;

                var age = (int)ageBox.Value;
                var weight = (decimal)weightBox.Value;
                var sex = (Sex)sexCombo.SelectedItem;
                var competition = (Competition)competitionCombo.SelectedItem;

                var suggestedCategories = _context.Categories
                    .Where(c => c.Sex == sex &&
                           (!c.MinAge.HasValue || c.MinAge <= age) &&
                           (!c.MaxAge.HasValue || c.MaxAge >= age) &&
                           (!c.MinWeight.HasValue || c.MinWeight <= weight) &&
                           (!c.MaxWeight.HasValue || c.MaxWeight >= weight))
                    .ToList();

                suggestedCategoriesList.ItemsSource = suggestedCategories;
            }

            ageBox.ValueChanged += (s, args) => UpdateSuggestedCategories();
            weightBox.ValueChanged += (s, args) => UpdateSuggestedCategories();
            sexCombo.SelectionChanged += (s, args) => UpdateSuggestedCategories();
            competitionCombo.SelectionChanged += (s, args) => UpdateSuggestedCategories();

            dialog.PrimaryButtonClick += async (s, args) =>
            {
                if (string.IsNullOrWhiteSpace(nameBox.Text) ||
                    string.IsNullOrWhiteSpace(clubBox.Text) ||
                    ageBox.Value == null ||
                    weightBox.Value == null ||
                    sexCombo.SelectedItem == null ||
                    competitionCombo.SelectedItem == null)
                {
                    var errorDialog = new ContentDialog
                    {
                        Title = "Error",
                        Content = "Please fill in all required fields.",
                        CloseButtonText = "OK",
                        XamlRoot = this.XamlRoot
                    };
                    await errorDialog.ShowAsync();
                    args.Cancel = true;
                    return;
                }

                var competitor = new Competitor
                {
                    Name = nameBox.Text,
                    Club = clubBox.Text,
                    Age = (int)ageBox.Value,
                    Weight = (decimal)weightBox.Value,
                    Sex = (Sex)sexCombo.SelectedItem,
                    CreatedAt = DateTime.Now
                };

                if (!competitor.IsValidAge() || !competitor.IsValidWeight())
                {
                    var errorDialog = new ContentDialog
                    {
                        Title = "Error",
                        Content = "Invalid age or weight values.",
                        CloseButtonText = "OK",
                        XamlRoot = this.XamlRoot
                    };
                    await errorDialog.ShowAsync();
                    args.Cancel = true;
                    return;
                }

                _context.Competitors.Add(competitor);
                await _context.SaveChangesAsync();

                // Add selected categories as competition participants
                if (suggestedCategoriesList.SelectedItems.Count > 0)
                {
                    var competition = (Competition)competitionCombo.SelectedItem;
                    var participants = suggestedCategoriesList.SelectedItems
                        .Cast<Category>()
                        .Select(category => new CompetitionParticipant
                        {
                            CompetitionId = competition.Id,
                            CategoryId = category.Id,
                            CompetitorId = competitor.Id,
                            RegisteredAt = DateTime.Now
                        });

                    _context.CompetitionParticipants.AddRange(participants);
                    await _context.SaveChangesAsync();
                }

                LoadCompetitors();
            };

            await dialog.ShowAsync();
        }
    }
} 