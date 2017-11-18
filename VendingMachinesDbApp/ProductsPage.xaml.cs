using System.Linq;
using System.Windows;
using System.Windows.Controls;
using VendingMachines.Models;

namespace VendingMachines
{
    /// <summary>
    /// Interaction logic for ProductsPage.xaml
    /// </summary>
    public partial class ProductsPage : Page
    {
        private VendingMachinesEntities _database;
        private Product _selectedProduct;
        private ProductPrice _currentProductPrice;

        public ProductsPage(VendingMachinesEntities database)
        {
            InitializeComponent();

            _database = database;

        }

        public void RefreshListBox()
        {
            var selectedItem = (Product)_productsListBox.SelectedItem;
            _productsListBox.ItemsSource = null;
            _productsListBox.ItemsSource = _database.Products.Local;

            _productsListBox.SelectedItem = null;
            _productsListBox.SelectedItem = selectedItem;

        }

        private void Page_ProductsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (_productsListBox.SelectedIndex == -1)
            {
                _selectedProduct = null;
                _currentProductPrice = null;
                _textBlockProductId.Text = "";
                _textBlockProductName.Text = "";

            }
            else
            {
                _selectedProduct = (Product)_productsListBox.SelectedItem;
                _textBlockProductId.Text = _selectedProduct.Id;
                _textBlockProductName.Text = _selectedProduct.Name;
            }

            if (_selectedProduct != null
                && _selectedProduct.TerminalsProducts != null
                && _selectedProduct.TerminalsProducts.Count() > 0)
            {
                _dataGridTerminals.ItemsSource = _selectedProduct
                    .TerminalsProducts
                    .OrderByDescending(p => p.Quantity);
            }
            else
            {
                _dataGridTerminals.ItemsSource = null;
            }

            if (_selectedProduct != null 
                && _selectedProduct.ProductPrices != null 
                && _selectedProduct.ProductPrices.Count() > 0)
            {
                _dataGridPrices.ItemsSource = 
                    _selectedProduct.ProductPrices
                    .OrderByDescending(d => d.Date)
                    .Select(p => new {
                        p.Id,
                        p.ProductId,
                        SellingPrice = p.SellingPrice.ToString("0.00p."),
                        PurchasePrice = p.PurchasePrice.ToString("0.00p."),
                        p.Date
                    });

                _currentProductPrice = _selectedProduct.CurrentProductPrice;
            }
            else
            {
                _dataGridPrices.ItemsSource = null;
                _currentProductPrice = null;
            }

            if (_currentProductPrice != null)
            {
                _textBlockProductSellingPrice.Text = _currentProductPrice.SellingPrice.ToString();
                _textBlockProductPurchasePrice.Text = _currentProductPrice.PurchasePrice.ToString();
                _textBlockProductPriceDate.Text = _currentProductPrice.Date.ToString();
            }
            else
            {
                _textBlockProductSellingPrice.Text = " ";
                _textBlockProductPurchasePrice.Text = " ";
                _textBlockProductPriceDate.Text = " ";
            }

        }

        private void Page_ButtonAddProduct_Click(object sender, RoutedEventArgs e)
        {
            Pages.ProductPage.Product = new Product();
            Pages.ProductPage.CurrentRegime = PageRegime.Add;
            NavigationService.Navigate(Pages.ProductPage);
        }

        private void Page_ButtonEditProduct_Click(object sender, RoutedEventArgs e)
        {
            if (_productsListBox.SelectedIndex == -1)
                return;

            Product product = (Product)_productsListBox.SelectedItem;
            Pages.ProductPage.Product = product;
            Pages.ProductPage.CurrentRegime = PageRegime.Edit;
            NavigationService.Navigate(Pages.ProductPage);
        }

        private void Page_ButtonDeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            if (_productsListBox.SelectedIndex != -1)
            {
                Product product = (Product)_productsListBox.SelectedItem;

                MessageBoxResult result = MessageBox.Show(
                    $"Do you want to delete \"{product.Name}\"?",
                    "Warning!",
                    MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    if (_database.TerminalsProducts.Local.Where(p => p.ProductId == product.Id).Count() > 0)
                    {
                        MessageBox.Show($"You can't delete \"{product.Name}\" because there are links to the terminals", "Warning!");
                        return;
                    }

                    _database.Products.Local.Remove(product);

                    RefreshListBox();
                }
            }
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Pages.MainWindow.Title = this.Title;

            RefreshListBox();

        }

        private void Page_ButtonAddPrice_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedProduct == null)
                return;

            Pages.ProductPricePage.ProductPrice = 
                new ProductPrice {
                    ProductId = _selectedProduct.Id,
                    Date = System.DateTime.Today.AddDays(1),
                    PurchasePrice = _currentProductPrice != null ? _currentProductPrice.PurchasePrice : 0,
                    SellingPrice = _currentProductPrice != null ? _currentProductPrice.SellingPrice : 0
            };
            Pages.ProductPricePage.CurrentRegime = PageRegime.Add;
            NavigationService.Navigate(Pages.ProductPricePage);
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

