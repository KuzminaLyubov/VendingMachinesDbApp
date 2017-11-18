using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachines.Models
{
    public partial class Product
    {
        public decimal GetProductProfit(DateTime date)
        {
            var p = ProductPrices
                    .Where(pp => pp.Date <= date)
                    .OrderBy(pp => pp.Date)
                    .LastOrDefault();
            return p.SellingPrice - p.PurchasePrice;

        }

        public ProductPrice GetProductPrice(DateTime date)
        {
            return ProductPrices
                    .Where(pp => pp.Date <= date)
                    .OrderBy(pp => pp.Date)
                    .LastOrDefault();
        }

        public ProductPrice CurrentProductPrice
        {
           get {
                return GetProductPrice(DateTime.Today);
            }
        }
    }
}
