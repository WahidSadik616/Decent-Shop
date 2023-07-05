using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Decent.IMS.Data;

namespace Decent.IMS.BL
{
    public class SalesmanBL
    {
        private DecentDbEntities _context=new DecentDbEntities();

        public List<Salesman> GetAll(string key="")
        {
            IEnumerable<Salesman> query = _context.Salesmen;

            if (!string.IsNullOrEmpty(key))
            {
                int x;
                if (Int32.TryParse(key, out x))
                {
                    query = query.Where(q => q.ID==x);
                }
                else
                {
                
                    query = query.Where(q => q.Name.Contains(key) || q.Phone.Contains(key));
                }
                
            }
            return query.ToList();
         }

        public bool Delete(int id,out string error)
        {
            error = string.Empty;
            var salesman = _context.Salesmen.FirstOrDefault(u => u.ID == id);

            if (salesman == null)
            {
                error = "Invalid Salesman Id...!!";
                return false;
            }

            try
            {
                _context.Salesmen.Remove(salesman);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
            return true;
        }

        public Salesman Save(Salesman value, out string error)
        {
            error = string.Empty;
            try
            {
               var salesman= _context.Salesmen.FirstOrDefault(u => u.ID == value.ID);

                if (salesman == null)
                {
                    salesman=new Salesman();
                    _context.Salesmen.Add(salesman);
                }

                salesman.Name = value.Name;
                salesman.Salary = value.Salary;
                salesman.PaySalary = value.PaySalary;
                salesman.Phone = value.Phone;
                salesman.Email = value.Email;
                salesman.Address = value.Address;
                salesman.Benifit = value.Benifit;
                
                _context.SaveChanges();
                return salesman;

            }
            catch (Exception e)
            {
                error = e.Message;
                return value;

            }
        }
    }
}
