using System.Linq;
using System.Windows;
using System.Windows.Controls;
using VendingMachines.Models;

namespace VendingMachines
{
    /// <summary>
    /// Interaction logic for TerminalsPage.xaml
    /// </summary>
    public partial class TerminalsPage : Page
    {
        private VendingMachinesEntities _database;
        Terminal _selectedTerminal;

        public TerminalsPage(VendingMachinesEntities database)
        {
            InitializeComponent();

            _database = database;

        }

        public void RefreshListBox()
        {
            var selectedItem = (Terminal)_terminalsListBox.SelectedItem;
            _terminalsListBox.ItemsSource = null;
            _terminalsListBox.ItemsSource = _database.Terminals.Local;

            _terminalsListBox.SelectedItem = null;
            _terminalsListBox.SelectedItem = selectedItem;

        }

        private void Page_TerminalsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_terminalsListBox.SelectedIndex == -1)
            {
                _selectedTerminal = null;
                _textBlockTerminalId.Text = "";
                _textBlockTerminalLocation.Text = "";
                _dataGridBills.ItemsSource = null;
                _dataGridCoins.ItemsSource = null;
                _dataGridProducts.ItemsSource = null;
            }
            else
            {
                _selectedTerminal = (Terminal)_terminalsListBox.SelectedItem;
                _textBlockTerminalId.Text = _selectedTerminal.Id.ToString();
                _textBlockTerminalLocation.Text = _selectedTerminal.Location.ToString();

                _dataGridBills.ItemsSource = _selectedTerminal.TerminalsBills
                    .OrderBy(b => b.Bill.Denomination)
                    .Select(b => new {
                        b.Bill.Name,
                        Denomination = b.Bill.Denomination.ToString("0.00p."),
                        b.InsertedQuantity
                    });
                _dataGridCoins.ItemsSource = _selectedTerminal.TerminalsCoins
                    .OrderBy(c => c.Coin.Denomination)
                    .Select(c => new {
                        c.Coin.Name,
                        Denomination=c.Coin.Denomination.ToString("0.00p."),
                        c.LoadedQuantity,
                        c.ReturnedQuantity
                    });
                _dataGridProducts.ItemsSource = _selectedTerminal
                    .TerminalsProducts
                    .OrderByDescending(p => p.Quantity)
                    .Select(p => new {
                        p.Product.Name,
                        p.Quantity,
                        SellingPrice = p.Product.CurrentProductPrice != null ? p.Product.CurrentProductPrice.SellingPrice.ToString("0.00p.") : "not activated" 
                    });
            }


        }
        private void Page_ButtonAddTerminal_Click(object sender, RoutedEventArgs e)
        {
            Pages.TerminalPage.Terminal = new Terminal();
            Pages.TerminalPage.CurrentRegime = PageRegime.Add;
            NavigationService.Navigate(Pages.TerminalPage);

        }
        private void Page_ButtonEditTerminal_Click(object sender, RoutedEventArgs e)
        {
            if (_terminalsListBox.SelectedIndex == -1)
                return;

            Terminal terminal = (Terminal)_terminalsListBox.SelectedItem;
            Pages.TerminalPage.Terminal = terminal;
            Pages.TerminalPage.CurrentRegime = PageRegime.Edit;
            NavigationService.Navigate(Pages.TerminalPage);

        }
       
        
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Pages.MainWindow.Title = this.Title;

            RefreshListBox();

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

    }
}

