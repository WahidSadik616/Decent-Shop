using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decent.IMS.Data
{
    public partial class DayIncome
    {
        public double Cash
        {
            get
            {
                return this.Sell - (this.Cost+this.Payment);
            }
        }

        public double NetIncome
        {
            get
            {
                return this.Benifit - this.Cost;
            }
        }
  
    }
}
