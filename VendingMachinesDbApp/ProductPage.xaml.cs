using System.Windows;
using VendingMachines.Models;
using System.Windows.Controls;
using System.Data.Entity;

namespace VendingMachines
{
    /// <summary>
    /// Interaction logic for ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        private VendingMachinesEntities _database;
        private Product _product;

        public PageRegime CurrentRegime { get; set; }

        public ProductPage(VendingMachinesEntities database)
        {
            InitializeComponent();

            _database = database;

            _labelError.Visibility = Visibility.Hidden;
        }

        public Product Product
        {
            get
            {
                return _product;
            }
            set
            {
                _product = value;
            }
        }

        private void Page_ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_textBoxProductId.Text) || _textBoxProductId.Text.Length > 4)
            {
                _labelError.Visibility = Visibility.Visible;
                _labelError.Content = "Enter 4 char Product Id";
                _textBoxProductId.Focus();
                return;
            }

            _product.Id = _textBoxProductId.Text;

            if (string.IsNullOrWhiteSpace(_textBoxProductName.Text))
            {
                _labelError.Visibility = Visibility.Visible;
                _labelError.Content = "Enter Product Name";
                _textBoxProductName.Focus();
                return;
            }

            _product.Name = _textBoxProductName.Text;

            if (CurrentRegime == PageRegime.Add)
                _database.Products.Local.Add(_product);

            _database.SaveChanges();
            _database.Products.Load();

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
            _textBoxProductId.IsReadOnly = CurrentRegime == PageRegime.Edit;

            if (_product != null)
            {
                _textBoxProductId.Text = _product.Id;
                _textBoxProductName.Text = _product.Name;
            }
            else
            {
                _textBoxProductId.Text = "";
                _textBoxProductName.Text = "";
            }
        }

    }
}
