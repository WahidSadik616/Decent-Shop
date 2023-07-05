using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Decent.IMS.Data;

namespace Decent.IMS.BL
{
    public class DayPaymentBL
    {
        private DecentDbEntities _context=new DecentDbEntities();

        public List<DayPayment> GetAll(string key="")
        {
            IEnumerable<DayPayment> query = _context.DayPayments;

            if (!string.IsNullOrEmpty(key))
            {
               
                    query = query.Where(q => q.Name.Contains(key));
           
            }
            return query.ToList();
        }

        public bool Delete(int id,out string error)
        {
            error = string.Empty;
            var dayPayment = _context.DayPayments.FirstOrDefault(u => u.ID == id);

            if (dayPayment == null)
            {
                error = "Invalid Day Payment Id...!!";
                return false;
            }

            try
            {
                _context.DayPayments.Remove(dayPayment);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
            return true;
        }

        public DayPayment Save(DayPayment value, out string error)
        {
            error = string.Empty;
            try
            {
               var dayPayment= _context.DayPayments.FirstOrDefault(u => u.ID == value.ID);

                if (dayPayment == null)
                {
                    dayPayment=new DayPayment();
                    _context.DayPayments.Add(dayPayment);
                }

                dayPayment.Name = value.Name;
                dayPayment.Amount = value.Amount;
               

                _context.SaveChanges();
                return dayPayment;

            }
            catch (Exception e)
            {
                error = e.Message;
                return value;

            }
        }
    }
}
