using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Decent.IMS.Data;

namespace Decent.IMS.BL
{
    public class DaySellBL
    {
        private DecentDbEntities _context=new DecentDbEntities();

        public List<DaySell> GetAll(string key="")
        {
            IEnumerable<DaySell> query = _context.DaySells;

            if (!string.IsNullOrEmpty(key))
            {
             
                query = query.Where(q => q.Name.Contains(key));
           
            }
            return query.ToList();
        }

        public bool Delete(int id,out string error)
        {
            error = string.Empty;
            var daySell = _context.DaySells.FirstOrDefault(u => u.ID == id);

            if (daySell == null)
            {
                error = "Invalid Sell Id...!!";
                return false;
            }

            try
            {
                _context.DaySells.Remove(daySell);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
            return true;
        }

       public DaySell Save(DaySell value, out string error)
        {
            error = string.Empty;
            try
            {
               var daySell= _context.DaySells.FirstOrDefault(u => u.ID == value.ID);

                if (daySell == null)
                {
                    daySell=new DaySell();
                    _context.DaySells.Add(daySell);
                }

                daySell.Name = value.Name;
                daySell.Time = value.Time;
                daySell.ProductCategory = value.ProductCategory;
                daySell.Sell = value.Sell;
                daySell.Amount = value.Amount;
                daySell.Salesman = value.Salesman;
                daySell.Benifit = value.Benifit;
               


                _context.SaveChanges();
                return daySell;

            }
            catch (Exception e)
            {
                error = e.Message;
                return value;

            }
        }

    }
}
