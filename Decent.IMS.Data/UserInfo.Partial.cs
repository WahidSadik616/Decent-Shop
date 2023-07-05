using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decent.IMS.Data
{
    public partial class UserInfo
    {
       public string FullName
        {
            get { return this.Name + " " + this.Password; }
        }
        
        public string DepartmentName
        {
            get
            {
                if (this.Department == null)
                    return "";
                else
                {
                    return this.Department.Name;
                }
            }
        }
    }
}
