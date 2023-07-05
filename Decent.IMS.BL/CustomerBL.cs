using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Decent.IMS.Data;

namespace Decent.IMS.BL
{
    public class CustomerBL
    {
        private DecentDbEntities _context = new DecentDbEntities();

        public List<Customer> GetAll(string key="")
        {
            IEnumerable<Customer> query = _context.Customers;

            if (!string.IsNullOrEmpty(key))
            {
               
                    query = query.Where(q => q.Name.Contains(key) || q.Phone.Contains(key));
              
                
            }
            return query.ToList();
        }

        public bool Delete(int id,out string error)
        {
            error = string.Empty;
            var customer = _context.Customers.FirstOrDefault(u => u.ID == id);

            if (customer == null)
            {
                error = "Invalid Customer Id...!!";
                return false;
            }

            try
            {
                _context.Customers.Remove(customer);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
            return true;
        }

        public Customer Save(Customer value, out string error)
        {
            error = string.Empty;
            try
            {
               var customer= _context.Customers.FirstOrDefault(u => u.ID == value.ID);

                if (customer == null)
                {
                    customer=new Customer();
                    _context.Customers.Add(customer);
                }

                customer.Name = value.Name;
                customer.Address = value.Address;
                customer.Phone = value.Phone;
                customer.Email = value.Email;
                customer.TotalPrice = value.TotalPrice;
                customer.Payment = value.Payment;
                customer.Benifit = value.Benifit;
               

                _context.SaveChanges();
                return customer;

            }
            catch (Exception e)
            {
                error = e.Message;
                return value;

            }
        }
    }
}
