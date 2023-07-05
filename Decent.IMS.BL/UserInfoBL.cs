using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Decent.IMS.Data;

namespace Decent.IMS.BL
{
    public class UserInfoBL
    {
        private DecentDbEntities _context = new DecentDbEntities();

        public List<UserInfo> GetAll(string key="")
        {
            IEnumerable<UserInfo> query = _context.UserInfoes;

            if (!string.IsNullOrEmpty(key))
            {
                int x;
                if (Int32.TryParse(key, out x))
                {
                    query = query.Where(q => q.ID==x);
                }
                else
                {
                
                    query = query.Where(q => q.Name.Contains(key) ||
                                                 q.Email.Contains(key) ||
                                                 q.Phone.Contains(key));
                }
                
            }
            return query.ToList();
        }

        public bool Delete(int id,out string error)
        {
            error = string.Empty;
            var userInfo = _context.UserInfoes.FirstOrDefault(u => u.ID == id);

            if (userInfo == null)
            {
                error = "Invalid User Id...!!";
                return false;
            }

            try
            {
                _context.UserInfoes.Remove(userInfo);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
            return true;
        }

        public UserInfo Save(UserInfo value, out string error)
        {
            error = string.Empty;
            try
            {
               var userInfo= _context.UserInfoes.FirstOrDefault(u => u.ID == value.ID);

                if (userInfo == null)
                {
                    userInfo=new UserInfo();
                    _context.UserInfoes.Add(userInfo);
                }

                userInfo.Name = value.Name;
                userInfo.Password = value.Password;
                userInfo.Email = value.Email;
                userInfo.Phone = value.Phone;
                userInfo.OrgId = value.OrgId;
                userInfo.Gender = value.Gender;
                userInfo.TypeId = value.TypeId;
                userInfo.StatusId = value.StatusId;
                userInfo.DepartmentId = value.DepartmentId;

                if (userInfo.Name == string.Empty)
                {
                    error = "Give your name please..!!!";
                    return value;
                    

                }
                


                _context.SaveChanges();
                return userInfo;

            }
            catch (Exception e)
            {
                error = e.Message;
                return value;

            }
        }
    }
}
