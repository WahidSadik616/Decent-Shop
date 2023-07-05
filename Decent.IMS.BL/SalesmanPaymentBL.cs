using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Decent.IMS.Data;

namespace Decent.IMS.BL
{
    public class SalesmanPaymentBL
    {
        private DecentDbEntities _context = new DecentDbEntities();

        public List<SalesmanPayment> GetAll(string key="")
        {
            IEnumerable<SalesmanPayment> query = _context.SalesmanPayments;

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
            var salesmanPayment = _context.SalesmanPayments.FirstOrDefault(u => u.ID == id);

            if (salesmanPayment == null)
            {
                error = "Invalid Salesman Payment Id...!!";
                return false;
            }

            try
            {
                _context.SalesmanPayments.Remove(salesmanPayment);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
            return true;
        }

        public SalesmanPayment Save(SalesmanPayment value, out string error)
        {
            error = string.Empty;
            try
            {
               var salesmanPayment= _context.SalesmanPayments.FirstOrDefault(u => u.ID == value.ID);

                if (salesmanPayment == null)
                {
                    salesmanPayment = new SalesmanPayment()
                    {
                        Date = DateTime.Now
                    };
                    _context.SalesmanPayments.Add(salesmanPayment);
                }

                salesmanPayment.Date = value.Date;
                salesmanPayment.Name = value.Name;
                salesmanPayment.Phone = value.Phone;
                salesmanPayment.TotalDue = value.TotalDue;
                salesmanPayment.Payment = value.Payment;
                
                _context.SaveChanges();
                return salesmanPayment;

            }
            catch (Exception e)
            {
                error = e.Message;
                return value;

            }
        }
    }
}
