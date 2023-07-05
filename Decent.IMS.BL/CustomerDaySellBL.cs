using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Decent.IMS.Data;

namespace Decent.IMS.BL
{
    public class CustomerDaySellBL
    {
        private DecentDbEntities _context = new DecentDbEntities();

        public List<CustomerDaySell> GetAll(string key="")
        {
            IEnumerable<CustomerDaySell> query = _context.CustomerDaySells;

            if (!string.IsNullOrEmpty(key))
            {
                 DateTime a;
                if (DateTime.TryParse(key, out a))
                {
                    query = query.Where(q => q.Date == a);
                }
                else
                {


                    query = query.Where(q => q.Name.Contains(key) ||
                                             q.Phone.Contains(key));
                }

            }
            return query.ToList();
        }

        public bool Delete(int id,out string error)
        {
            error = string.Empty;
            var customerDaySell = _context.CustomerDaySells.FirstOrDefault(u => u.ID == id);

            if (customerDaySell == null)
            {
                error = "Invalid Customer Day Sell Id...!!";
                return false;
            }

            try
            {
                _context.CustomerDaySells.Remove(customerDaySell);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
            return true;
        }

        public CustomerDaySell Save(CustomerDaySell value, out string error)
        {
            error = string.Empty;
            try
            {
               var customerDaySell= _context.CustomerDaySells.FirstOrDefault(u => u.ID == value.ID);

                if (customerDaySell == null)
                {
                    customerDaySell=new CustomerDaySell();
                    _context.CustomerDaySells.Add(customerDaySell);
                }

                customerDaySell.Date = value.Date;
                customerDaySell.Time = value.Time;
                customerDaySell.Name = value.Name;
                customerDaySell.Phone = value.Phone;
                customerDaySell.TotalPrice = value.TotalPrice;
                customerDaySell.Payment = value.Payment;
                customerDaySell.Benifit = value.Benifit;

                _context.SaveChanges();
                return customerDaySell;

            }
            catch (Exception e)
            {
                error = e.Message;
                return value;

            }
        }
    }
}
