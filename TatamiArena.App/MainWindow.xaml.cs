using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using TatamiArena.App.Pages;

namespace TatamiArena.App
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            
            // Set up the navigation event handler
            NavView.SelectionChanged += NavView_SelectionChanged;
            
            // Set the initial page to Home
            NavView.SelectedItem = NavView.MenuItems[0];
            ContentFrame.Navigate(typeof(HomePage));
        }

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItemContainer is NavigationViewItem item)
            {
                string tag = item.Tag?.ToString() ?? string.Empty;
                
                switch (tag)
                {
                    case "home":
                        ContentFrame.Navigate(typeof(HomePage));
                        break;
                    case "competitions":
                        ContentFrame.Navigate(typeof(CompetitionsPage));
                        break;
                    case "categories":
                        ContentFrame.Navigate(typeof(CategoriesPage));
                        break;
                    case "competitors":
                        ContentFrame.Navigate(typeof(CompetitorsPage));
                        break;
                }
            }
        }
    }
}