using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Decent.IMS.Data;

namespace Decent.IMS.BL
{
    public class DayCostBL
    {
        private DecentDbEntities _context=new DecentDbEntities();

        public List<DayCost> GetAll(string key="")
        {
            IEnumerable<DayCost> query = _context.DayCosts;

            if (!string.IsNullOrEmpty(key))
            {
                int x;
                if (Int32.TryParse(key, out x))
                {
                    query = query.Where(q => q.ID==x);
                }
                else
                {
                    query = query.Where(q => q.Name.Contains(key));

                }
                
            }
            return query.ToList();
        }

        public bool Delete(int id,out string error)
        {
            error = string.Empty;
            var dayCost = _context.DayCosts.FirstOrDefault(u => u.ID == id);

            if (dayCost == null)
            {
                error = "Invalid Day Cost Id...!!";
                return false;
            }

            try
            {
                _context.DayCosts.Remove(dayCost);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
            return true;
        }

        public DayCost Save(DayCost value, out string error)
        {
            error = string.Empty;
            try
            {
               var dayCost= _context.DayCosts.FirstOrDefault(u => u.ID == value.ID);

                if (dayCost == null)
                {
                    dayCost=new DayCost();
                    _context.DayCosts.Add(dayCost);
                }

                dayCost.Name = value.Name;
                dayCost.CostAmount = value.CostAmount;

              if (dayCost.Name == string.Empty)
                {
                    error = "Give your name please..!!!";
                    return value;
                    

                }
                


                _context.SaveChanges();
                return dayCost;

            }
            catch (Exception e)
            {
                error = e.Message;
                return value;

            }
        }
    }
}
