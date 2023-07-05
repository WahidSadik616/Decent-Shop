using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decent.IMS.Data
{
    public partial class Customer
    {
        public double Due
        {
            get
            {
                return this.TotalPrice - this.Payment;
            }
        }
        
        
    }
}
