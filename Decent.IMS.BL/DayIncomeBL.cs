using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Decent.IMS.Data;

namespace Decent.IMS.BL
{
    public class DayIncomeBL
    {
        private DecentDbEntities _context=new DecentDbEntities();

        public List<DayIncome> GetAll(string key="")
        {
            IEnumerable<DayIncome> query = _context.DayIncomes;

            if (!string.IsNullOrEmpty(key))
            { 
                DateTime a;
                if (DateTime.TryParse(key, out a))
                {
                    query = query.Where(q => q.Date == a);
                }
                
              
           }
            return query.ToList();
        }

        public bool Delete(int id,out string error)
        {
            error = string.Empty;
            var dayIncome = _context.DayIncomes.FirstOrDefault(u => u.ID == id);

            if (dayIncome == null)
            {
                error = "Invalid DayIncome Id...!!";
                return false;
            }

            try
            {
                _context.DayIncomes.Remove(dayIncome);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
            return true;
        }

       public DayIncome Save(DayIncome value, out string error)
        {
            error = string.Empty;
            try
            {
               var dayIncome= _context.DayIncomes.FirstOrDefault(u => u.ID == value.ID);

                if (dayIncome == null)
                {
                    dayIncome = new DayIncome()
                    {
                        Date = DateTime.Now
                    };
                    _context.DayIncomes.Add(dayIncome);
                }
                dayIncome.Date = value.Date;
                dayIncome.Sell = value.Sell;
                dayIncome.Cost = value.Cost;
                dayIncome.Payment = value.Payment;
                dayIncome.Benifit = value.Benifit;
                dayIncome.DayCostId = value.DayCostId;

                _context.SaveChanges();
                return dayIncome;

            }
            catch (Exception e)
            {
                error = e.Message;
                return value;

            }
        }
    }
}
