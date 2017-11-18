using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using VendingMachines.Models;
using System.Data.Entity;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Media;

namespace VendingMachines
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        VendingMachinesEntities _database = new VendingMachinesEntities();

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            _database.SaveChanges();
            _database.Dispose();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // LoadAsync - asynchronous call without blocking the window thread
            await _database.Bills.LoadAsync();
            await _database.Coins.LoadAsync();
            await _database.Terminals.LoadAsync();
            await _database.Products.LoadAsync();
            await _database.ProductPrices.LoadAsync();
            await _database.SoldProducts.LoadAsync();
            await _database.TerminalsBills.LoadAsync();
            await _database.TerminalsCoins.LoadAsync();
            await _database.TerminalsProducts.LoadAsync();

            Pages.MainWindow = this;
            Pages.TerminalsPage = new TerminalsPage(_database);
            Pages.ProductsPage = new ProductsPage(_database);
            Pages.TerminalPage = new TerminalPage(_database);
            Pages.ProductPage = new ProductPage(_database);
            Pages.ProductPricePage = new ProductPricePage(_database);
            Pages.StatsPage = new StatsPage(_database);

            frameMain.Navigate(Pages.StatsPage);

        }

    }
}

