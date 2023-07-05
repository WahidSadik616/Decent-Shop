using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decent.IMS.Data
{
    public partial class Product
    {
        public double TotalPrice
        {
            get { return this.Availability * this.PriceRate; }
        }
        
        public string ProductCategoryName
        {
            get
            {
                if (this.ProductCategory == null)
                    return "";

                else
                {
                    return this.ProductCategory.Name;
                }
            }
        }

        public string SellerName
        {
            get
            {
                if (this.Seller == null)
                    return "";

                else
                {
                    return this.Seller.Name;
                }
            }
        }
    }
}
