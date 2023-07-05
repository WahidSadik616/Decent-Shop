using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decent.IMS.Framework
{
    public static class EnumCollection
    {
        public enum UserStatusEnum
        {
            Pending=1,
            Approve=2,
            Reject=4
        }

        public static List<EnumDetail> UserStatusList()
        {
            var list = new List<EnumDetail>();
            list.Add(new EnumDetail() {ID = 1, Name = "Pending"});
            list.Add(new EnumDetail(){ID = 2,Name = "Approve"});
            list.Add(new EnumDetail(){ID = 4,Name = "Reject"});

            return list;
        }

        public enum UserTypeEnum
        {
            Admin=1,
            Other=2
        }

        public static List<EnumDetail> UserTypeList()
        {
            var list = new List<EnumDetail>();
            list.Add(new EnumDetail(){ID = 1,Name = "Admin"});
            list.Add(new EnumDetail(){ID = 2,Name = "Other"});

            return list;
        }
    }

    public class EnumDetail
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
