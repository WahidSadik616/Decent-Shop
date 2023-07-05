using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decent.IMS.Data
{
    public partial class SellerNewProduct
    {

        public double UpdateTotalPrice
        {
            get
            {
                return this.AfterTotalPrice + this.CarryingCost;
            }
        }
        
        public string CategoryName
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
    }
}
