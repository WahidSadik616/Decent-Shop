using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Decent.IMS.Data;

namespace Decent.IMS.BL
{
    public class CostDetailsBL
    {
        private DecentDbEntities _context = new DecentDbEntities();

        public List<CostDetail> GetAll(string key="")
        {
            IEnumerable<CostDetail> query = _context.CostDetails;

            if (!string.IsNullOrEmpty(key))
            {
                DateTime a;
                if (DateTime.TryParse(key, out a))
                {
                    query = query.Where(q => q.Date == a);
                }
                else
                {
                    query = query.Where(q => q.Name.Contains(key));
                }

            }
            return query.ToList();
        }


        public List<CostDetail> GetAllDateRange(string startDate = "", string endDate = "")
        {
            IEnumerable<CostDetail> query = _context.CostDetails;

            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                DateTime a;
                DateTime b;
                if (DateTime.TryParse(startDate, out a) && DateTime.TryParse(endDate, out b))
                {
                    query = query.Where(q => q.Date >= a && q.Date <= b);
                }

            }
            return query.ToList();
        }


        public bool Delete(int id,out string error)
        {
            error = string.Empty;
            var costDetails = _context.CostDetails.FirstOrDefault(u => u.ID == id);

            if (costDetails == null)
            {
                error = "Invalid Cost Details Id...!!";
                return false;
            }

            try
            {
                _context.CostDetails.Remove(costDetails);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
            return true;
        }

        public CostDetail Save(CostDetail value, out string error)
        {
            error = string.Empty;
            try
            {
               var costDetails= _context.CostDetails.FirstOrDefault(u => u.ID == value.ID);

                if (costDetails == null)
                {
                    costDetails = new CostDetail()
                    {
                        Date = DateTime.Now
                    };
                    _context.CostDetails.Add(costDetails);
                }

                costDetails.Date = value.Date;
                costDetails.Name = value.Name;
                costDetails.Amount = value.Amount;
                
               


                _context.SaveChanges();
                return costDetails;

            }
            catch (Exception e)
            {
                error = e.Message;
                return value;

            }
        }
    }
}
