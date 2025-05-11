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
            NavView.SelectedItem = NavView.MenuItems[0];
            NavView_SelectionChanged(NavView, null);
        }

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItemContainer is NavigationViewItem item)
            {
                switch (item.Tag.ToString())
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