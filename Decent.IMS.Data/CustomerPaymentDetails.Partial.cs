using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decent.IMS.Data
{
    public partial class CustomerPaymentDetail
    {
        public double Due
        {
            get
            {
                return this.TotalDue - this.Payment;
            }
        }
        
        
    }
}
