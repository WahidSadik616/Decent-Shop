using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Decent.IMS.Data;

namespace Decent.IMS.BL
{
    public class DepartmentBL
    {
        private DecentDbEntities _context=new DecentDbEntities();

        public List<Department> GetAll(string key="")
        {
            IEnumerable<Department> query = _context.Departments;

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
            var department = _context.Departments.FirstOrDefault(u => u.ID == id);

            if (department == null)
            {
                error = "Invalid Department Id...!!";
                return false;
            }

            try
            {
                _context.Departments.Remove(department);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
            return true;
        }

        public Department Save(Department value, out string error)
        {
            error = string.Empty;
            try
            {
               var department= _context.Departments.FirstOrDefault(u => u.ID == value.ID);

                if (department == null)
                {
                    department=new Department();
                    _context.Departments.Add(department);
                }

                department.Name = value.Name;

                if (department.Name == string.Empty)
                {
                    error = "Give your name please..!!!";
                    return value;
                    

                }
                


                _context.SaveChanges();
                return department;

            }
            catch (Exception e)
            {
                error = e.Message;
                return value;

            }
        }
    }
}
