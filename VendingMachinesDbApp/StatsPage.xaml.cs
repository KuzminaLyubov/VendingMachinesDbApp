using System.Linq;
using System.Windows;
using System.Windows.Controls;
using VendingMachines.Models;

namespace VendingMachines
{
    /// <summary>
    /// Interaction logic for StatsPage.xaml
    /// </summary>
    public partial class StatsPage : Page
    {
        private VendingMachinesEntities _database;

        public StatsPage(VendingMachinesEntities database)
        {
            InitializeComponent();

            _database = database;

        }
        
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Pages.MainWindow.Title = this.Title;

            _dataGridTerminals.ItemsSource = _database.TerminalsProducts.Local
                .Where(t => t.Quantity == 0)
                .Select(t => new {
                    t.Terminal.Location,
                    t.Product.Name,
                    t.Quantity
                });

            _month.ItemsSource = System.Enum.GetValues(typeof(MonthEnum)).Cast<MonthEnum>();
            _month.SelectedValue = MonthEnum.November;

            _year.ItemsSource = new[] { 2016, 2017 };
            _year.SelectedValue = 2017;

            Refresh();
        }

        private void Refresh()
        {
            if (_month.SelectedValue == null || _year.SelectedValue == null)
                return;

            var month = (int)_month.SelectedValue;
            var year = (int)_year.SelectedValue;

            _dataGridSoldProducts.ItemsSource = _database.SoldProducts.Local
                .Where(p => (month == (int)MonthEnum.NotSelected || p.Date.Month == month) && p.Date.Year == year)
                .GroupBy(p => p.Product.Name)
                .OrderBy(p => p.Count())
                .Take(5)
                .Select(p => new
                {
                    Name = p.Key,
                    Quantity = p.Count()
                });

            _dataGridTerminalProfit.ItemsSource = _database.SoldProducts.Local
                .Where(p => (month == (int)MonthEnum.NotSelected || p.Date.Month == month) && p.Date.Year == year)
                .Select(p => new {
                    p.Terminal.Location,
                    Profit = p.Product.GetProductProfit(p.Date)
                })
                .GroupBy(p => new { p.Location })
                .Select(p => new {
                    p.Key.Location,
                    Profit = p.Sum(pp => pp.Profit),
                    ProfitStr = p.Sum(pp => pp.Profit).ToString("0.00p."),
                    Month = month,
                    Year = year
                })
                .OrderByDescending(p => p.Profit);

        }

        private void Page_ButtonTerminals_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(Pages.TerminalsPage);
        }

        private void Page_ButtonProducts_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(Pages.ProductsPage);
        }

        private void Page_ButtonStats_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(Pages.StatsPage);
        }

        private void _month_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }

        private void _year_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }
    }

    public enum MonthEnum : int
    {
        NotSelected = 0, 
        January,
        February,
        March,
        April,
        May,
        June,
        July,
        August,
        September,
        October,
        November,
        December
    }
}

