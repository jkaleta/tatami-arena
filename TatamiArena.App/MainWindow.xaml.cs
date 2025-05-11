using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using TatamiArena.App.Pages;
using TatamiArena.App.Services;

namespace TatamiArena.App
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            
            // Set up the navigation event handler
            NavView.SelectionChanged += NavView_SelectionChanged;
            
            // Set up language change handler
            LocalizationService.Instance.LanguageChanged += OnLanguageChanged;
            
            // Set the initial page to Home
            NavView.SelectedItem = NavView.MenuItems[0];
            ContentFrame.Navigate(typeof(HomePage));

            // Set initial language in selector
            var currentLanguage = LocalizationService.Instance.GetCurrentLanguage();
            foreach (ComboBoxItem item in LanguageSelector.Items)
            {
                if (item.Tag.ToString() == currentLanguage)
                {
                    LanguageSelector.SelectedItem = item;
                    break;
                }
            }
        }

        private void OnLanguageChanged(object sender, System.EventArgs e)
        {
            // Refresh the UI to reflect the new language
            this.Bindings.Update();
            
            //// Update navigation items
            //foreach (NavigationViewItem item in NavView.MenuItems)
            //{
            //    if (item is NavigationViewItem navItem)
            //    {
            //        navItem.SetBinding().Bindings.Update();
            //    }
            //}

            //// Update language selector
            //LanguageSelector.Bindings.Update();
            //foreach (ComboBoxItem item in LanguageSelector.Items)
            //{
            //    item.Bindings.Update();
            //}
        }

        private void LanguageSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LanguageSelector.SelectedItem is ComboBoxItem selectedItem)
            {
                LocalizationService.Instance.SetLanguage(selectedItem.Tag.ToString());
            }
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