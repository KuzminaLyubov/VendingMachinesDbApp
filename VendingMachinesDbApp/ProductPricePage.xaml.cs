using System.Windows;
using VendingMachines.Models;
using System.Windows.Controls;
using System.Data.Entity;

namespace VendingMachines
{
    /// <summary>
    /// Interaction logic for ProductPricePage.xaml
    /// </summary>
    public partial class ProductPricePage : Page
    {
        private VendingMachinesEntities _database;
        private ProductPrice _productprice;

        public PageRegime CurrentRegime { get; set; }

        public ProductPricePage(VendingMachinesEntities database)
        {
            InitializeComponent();

            _database = database;

            _labelError.Visibility = Visibility.Hidden;
        }

        public ProductPrice ProductPrice
        {
            get
            {
                return _productprice;
            }
            set
            {
                _productprice = value;
            }
        }

        private void Page_ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            decimal price;

            if (!decimal.TryParse(_textBoxProductSellingPrice.Text, out price))
            {
                _labelError.Visibility = Visibility.Visible;
                _labelError.Content = "Enter Product Selling Price";
                _textBoxProductSellingPrice.Focus();
                return;
            }

            _productprice.SellingPrice = price;
            if (!decimal.TryParse(_textBoxProductPurchasePrice.Text, out price))
            {
                _labelError.Visibility = Visibility.Visible;
                _labelError.Content = "Enter Product Purchase Price";
                _textBoxProductPurchasePrice.Focus();
                return;
            }

            _productprice.PurchasePrice = price;
            if (_productprice.PurchasePrice >= _productprice.SellingPrice)
            {
                _labelError.Visibility = Visibility.Visible;
                _labelError.Content = "Product Selling Price should be greater than Purchase Price";
                _textBoxProductSellingPrice.Focus();
                return;
            }


            if (!_datePickerProductDate.SelectedDate.HasValue || _datePickerProductDate.SelectedDate.Value.Date <= System.DateTime.Today)
            {
                _labelError.Visibility = Visibility.Visible;
                _labelError.Content = "Price activation date should be larger than current date";
                _datePickerProductDate.Focus();
                return;
            }

            _productprice.Date = _datePickerProductDate.SelectedDate.Value.Date;

            if (CurrentRegime == PageRegime.Add)
                _database.ProductPrices.Local.Add(_productprice);

            _database.SaveChanges();
            _database.ProductPrices.Load();

            NavigationService.Navigate(Pages.ProductsPage);
        }

        private void Page_ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            CurrentRegime = PageRegime.Cancel;
            NavigationService.Navigate(Pages.ProductsPage);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Pages.MainWindow.Title = this.Title;
            _textBoxProductPurchasePrice.Text = _productprice.PurchasePrice.ToString();
            _textBoxProductSellingPrice.Text = _productprice.SellingPrice.ToString();
            _datePickerProductDate.SelectedDate = _productprice.Date.Date;
        }

    }
}
