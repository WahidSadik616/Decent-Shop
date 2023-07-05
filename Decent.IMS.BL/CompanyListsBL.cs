using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Decent.IMS.Data;

namespace Decent.IMS.BL
{
    public class CompanyListsBL
    {
        private DecentDbEntities _context=new DecentDbEntities();

        public List<CompanyList> GetAll(string key="")
        {
            IEnumerable<CompanyList> query = _context.CompanyLists;

            if (!string.IsNullOrEmpty(key))
            {
                query = query.Where(q => q.CompanyName.Contains(key));
            }
            return query.ToList();
        }

        
        public bool Delete(int id,out string error)
        {
            error = string.Empty;
            var companyLists = _context.CompanyLists.FirstOrDefault(u => u.ID == id);

            if (companyLists == null)
            {
                error = "Invalid CompanyLists Id...!!";
                return false;
            }

            try
            {
                _context.CompanyLists.Remove(companyLists);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
            return true;
        }

        public CompanyList Save(CompanyList value, out string error)
        {
            error = string.Empty;
            try
            {
               var companyLists= _context.CompanyLists.FirstOrDefault(u => u.ID == value.ID);

                if (companyLists == null)
                {
                    companyLists=new CompanyList();
                    _context.CompanyLists.Add(companyLists);
                }

                companyLists.CompanyName = value.CompanyName;
                

                _context.SaveChanges();
                return companyLists;

            }
            catch (Exception e)
            {
                error = e.Message;
                return value;

            }
        }

    }
}
