using System.Linq;
using Microsoft.UI.Xaml.Controls;
using TatamiArena.Data;

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
        }

        private void LoadStats()
        {
            CompetitionsCount.Text = _context.Competitions.Count().ToString();
            CategoriesCount.Text = _context.Categories.Count().ToString();
            CompetitorsCount.Text = _context.Competitors.Count().ToString();
        }
    }
} 