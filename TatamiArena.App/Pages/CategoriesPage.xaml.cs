using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Linq;
using TatamiArena.Core.Models;
using TatamiArena.Data;

namespace TatamiArena.App.Pages
{
    public sealed partial class CategoriesPage : Page
    {
        private readonly TatamiArenaContext _context;

        public CategoriesPage()
        {
            this.InitializeComponent();
            _context = new TatamiArenaContext();
            LoadCategories();
        }

        private void LoadCategories()
        {
            CategoriesListView.ItemsSource = _context.Categories
                .Include(c => c.Participants)
                .OrderBy(c => c.Name)
                .ThenBy(c => c.Name)
                .ToList();
        }

        private async void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog
            {
                Title = "Add Category",
                PrimaryButtonText = "Save",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                XamlRoot = this.XamlRoot
            };

            var panel = new StackPanel { Spacing = 8 };

            var nameBox = new TextBox { PlaceholderText = "Name" };
            var typeCombo = new ComboBox
            {
                PlaceholderText = "Select Type",
                ItemsSource = Enum.GetValues(typeof(CategoryType))
            };
            var sexCombo = new ComboBox
            {
                PlaceholderText = "Select Sex",
                ItemsSource = Enum.GetValues(typeof(Sex))
            };
            var minAgeBox = new NumberBox { PlaceholderText = "Min Age" };
            var maxAgeBox = new NumberBox { PlaceholderText = "Max Age" };
            var minWeightBox = new NumberBox { PlaceholderText = "Min Weight" };
            var maxWeightBox = new NumberBox { PlaceholderText = "Max Weight" };
            var competitionCombo = new ComboBox
            {
                PlaceholderText = "Select Competition",
                ItemsSource = _context.Competitions.ToList(),
                DisplayMemberPath = "Name"
            };

            panel.Children.Add(new TextBlock { Text = "Name:" });
            panel.Children.Add(nameBox);
            panel.Children.Add(new TextBlock { Text = "Type:" });
            panel.Children.Add(typeCombo);
            panel.Children.Add(new TextBlock { Text = "Sex:" });
            panel.Children.Add(sexCombo);
            panel.Children.Add(new TextBlock { Text = "Age Range:" });
            panel.Children.Add(minAgeBox);
            panel.Children.Add(maxAgeBox);
            panel.Children.Add(new TextBlock { Text = "Weight Range:" });
            panel.Children.Add(minWeightBox);
            panel.Children.Add(maxWeightBox);
            panel.Children.Add(new TextBlock { Text = "Competition:" });
            panel.Children.Add(competitionCombo);

            dialog.Content = panel;

            dialog.PrimaryButtonClick += async (s, args) =>
            {
                if (string.IsNullOrWhiteSpace(nameBox.Text) ||
                    typeCombo.SelectedItem == null ||
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

                var category = new Category
                {
                    Name = nameBox.Text,
                    Type = (CategoryType)typeCombo.SelectedItem,
                    Sex = (Sex)sexCombo.SelectedItem,
                    MinAge = minAgeBox.Value != null ? (int)minAgeBox.Value : null,
                    MaxAge = maxAgeBox.Value != null ? (int)maxAgeBox.Value : null,
                    MinWeight = minWeightBox.Value != null ? (decimal)minWeightBox.Value : null,
                    MaxWeight = maxWeightBox.Value != null ? (decimal)maxWeightBox.Value : null
                };

                if (!category.IsValidAgeRange() || !category.IsValidWeightRange())
                {
                    var errorDialog = new ContentDialog
                    {
                        Title = "Error",
                        Content = "Invalid age or weight range. The minimum value must be less than or equal to the maximum value.",
                        CloseButtonText = "OK",
                        XamlRoot = this.XamlRoot
                    };
                    await errorDialog.ShowAsync();
                    args.Cancel = true;
                    return;
                }

                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                LoadCategories();
            };

            await dialog.ShowAsync();
        }
    }
} 