using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decent.IMS.Data
{
    public partial class Salesman
    {
        public double DueSalary
        {
            get
            {
                return this.Salary - this.PaySalary;
            }
        }
        
        
    }
}
