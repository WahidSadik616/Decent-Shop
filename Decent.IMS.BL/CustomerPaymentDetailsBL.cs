using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Decent.IMS.Data;

namespace Decent.IMS.BL
{
    public class CustomerPaymentDetailsBL
    {
        private DecentDbEntities _context = new DecentDbEntities();

       public List<CustomerPaymentDetail> GetAll(string key="")
        {
            IEnumerable<CustomerPaymentDetail> query = _context.CustomerPaymentDetails;

            if (!string.IsNullOrEmpty(key))
            {
                 DateTime a;
                if (DateTime.TryParse(key, out a))
                {
                    query = query.Where(q => q.Date == a);
                }
                else
                {
                    query = query.Where(q => q.CustomerName.Contains(key));
                }

            }
            return query.ToList();
        }

        public bool Delete(int id,out string error)
        {
            error = string.Empty;
            var customerPaymentDetails = _context.CustomerPaymentDetails.FirstOrDefault(u => u.ID == id);

            if (customerPaymentDetails == null)
            {
                error = "Invalid Customer Payment Details Id...!!";
                return false;
            }

            try
            {
                _context.CustomerPaymentDetails.Remove(customerPaymentDetails);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
            return true;
        }

        public CustomerPaymentDetail Save(CustomerPaymentDetail value, out string error)
        {
            error = string.Empty;
            try
            {
               var customerPaymentDetails= _context.CustomerPaymentDetails.FirstOrDefault(u => u.ID == value.ID);

                if (customerPaymentDetails == null)
                {
                    customerPaymentDetails=new CustomerPaymentDetail();
                    _context.CustomerPaymentDetails.Add(customerPaymentDetails);
                }

                customerPaymentDetails.Date = value.Date;
                customerPaymentDetails.CustomerName = value.CustomerName;
                customerPaymentDetails.Phone = value.Phone;
                customerPaymentDetails.TotalDue = value.TotalDue;
                customerPaymentDetails.Payment = value.Payment;
                


                _context.SaveChanges();
                return customerPaymentDetails;

            }
            catch (Exception e)
            {
                error = e.Message;
                return value;

            }
        }
    }
}
