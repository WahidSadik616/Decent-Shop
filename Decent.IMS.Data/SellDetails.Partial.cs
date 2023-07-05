using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decent.IMS.Data
{
    public partial class SellDetail
    {
        
        public string Category
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
