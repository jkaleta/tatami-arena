using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using TatamiArena.Data;
using TatamiArena.App.Services;

namespace TatamiArena.App.Pages
{
    public sealed partial class HomePage : Page
    {
        private readonly TatamiArenaContext _context;

        public HomePage()
        {
            this.InitializeComponent();
            _context = new TatamiArenaContext();
            LoadStats();
            
            // Subscribe to language changes
            LocalizationService.Instance.LanguageChanged += OnLanguageChanged;
        }

        private void LoadStats()
        {
            CompetitionsCount.Text = _context.Competitions.Count().ToString();
            CategoriesCount.Text = _context.Categories.Count().ToString();
            CompetitorsCount.Text = _context.Competitors.Count().ToString();
        }

        private void OnLanguageChanged(object sender, System.EventArgs e)
        {
            // Refresh the UI to reflect the new language
            this.Bindings.Update();
        }
    }
} 