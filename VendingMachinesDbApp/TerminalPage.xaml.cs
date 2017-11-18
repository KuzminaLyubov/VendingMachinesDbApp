using System.Windows;
using VendingMachines.Models;
using System.Windows.Controls;
using System.Data.Entity;

namespace VendingMachines
{
    /// <summary>
    /// Interaction logic for TerminalPage.xaml
    /// </summary>
    public partial class TerminalPage : Page
    {
        private VendingMachinesEntities _database;
        private Terminal _terminal;

        public PageRegime CurrentRegime { get; set; }

        public TerminalPage(VendingMachinesEntities database)
        {
            InitializeComponent();

            _database = database;

            _labelError.Visibility = Visibility.Hidden;
        }

        public Terminal Terminal
        {
            get
            {
                return _terminal;
            }
            set
            {
                _terminal = value;
            }
        }

        private void Page_ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_textBoxTerminalLocation.Text))
            {
                _labelError.Visibility = Visibility.Visible;
                _labelError.Content = "Enter terminal location";
                _textBoxTerminalLocation.Focus();
                return;
            }

            _terminal.Location = _textBoxTerminalLocation.Text;

            _terminal.BillsCapacity = 100;
            _terminal.CoinsLoadedQty = 1000;
            _terminal.ProductsLoadedQty = 120;

            _terminal.TerminalsBills.Clear();
            _terminal.TerminalsBills.Add(new TerminalsBill { TerminalId = _terminal.Id, BillId = 1, InsertedQuantity = 0 });
            _terminal.TerminalsBills.Add(new TerminalsBill { TerminalId = _terminal.Id, BillId = 2, InsertedQuantity = 0 });
            _terminal.TerminalsBills.Add(new TerminalsBill { TerminalId = _terminal.Id, BillId = 3, InsertedQuantity = 0 });
            _terminal.TerminalsBills.Add(new TerminalsBill { TerminalId = _terminal.Id, BillId = 4, InsertedQuantity = 0 });
            _terminal.TerminalsBills.Add(new TerminalsBill { TerminalId = _terminal.Id, BillId = 5, InsertedQuantity = 0 });

            _terminal.TerminalsCoins.Clear();
            _terminal.TerminalsCoins.Add(new TerminalsCoin { TerminalId = _terminal.Id, CoinId = 1, ReturnedQuantity = 0, LoadedQuantity = 400 });
            _terminal.TerminalsCoins.Add(new TerminalsCoin { TerminalId = _terminal.Id, CoinId = 2, ReturnedQuantity = 0, LoadedQuantity = 300 });
            _terminal.TerminalsCoins.Add(new TerminalsCoin { TerminalId = _terminal.Id, CoinId = 3, ReturnedQuantity = 0, LoadedQuantity = 200 });
            _terminal.TerminalsCoins.Add(new TerminalsCoin { TerminalId = _terminal.Id, CoinId = 4, ReturnedQuantity = 0, LoadedQuantity = 100 });

            _terminal.TerminalsProducts.Clear();
            foreach (var p in _database.Products.Local)
            {
                _terminal.TerminalsProducts.Add(new TerminalsProduct { TerminalId = _terminal.Id, ProductId = p.Id, Quantity = 10 });
            }

            if (CurrentRegime == PageRegime.Add)
                _database.Terminals.Local.Add(_terminal);

            _database.SaveChanges();
            _database.Terminals.Load();

            NavigationService.Navigate(Pages.TerminalsPage);
        }

        private void Page_ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            CurrentRegime = PageRegime.Cancel;
            NavigationService.Navigate(Pages.TerminalsPage);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Pages.MainWindow.Title = this.Title;

            if (_terminal != null)
            {
                _textBoxTerminalLocation.Text = _terminal.Location;
            }
        }

    }
}
