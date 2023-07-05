using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Decent.IMS.Data;

namespace Decent.IMS.BL
{
    public class SellerBL
    {
        private DecentDbEntities _context=new DecentDbEntities();

        public List<Seller> GetAll(string key="")
        {
            IEnumerable<Seller> query = _context.Sellers;

            if (!string.IsNullOrEmpty(key))
            {
              
               
                query = query.Where(q => q.Name.Contains(key) || q.Phone.Contains(key));
               
                
            }
            return query.ToList();
        }

        public bool Delete(int id,out string error)
        {
            error = string.Empty;
            var seller = _context.Sellers.FirstOrDefault(u => u.ID == id);

            if (seller == null)
            {
                error = "Invalid Seller Id...!!";
                return false;
            }

            try
            {
                _context.Sellers.Remove(seller);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
            return true;
        }

        public Seller Save(Seller value, out string error)
        {
            error = string.Empty;
            try
            {
               var seller= _context.Sellers.FirstOrDefault(u => u.ID == value.ID);

                if (seller == null)
                {
                    seller=new Seller();
                    _context.Sellers.Add(seller);
                }

                seller.Name = value.Name;
                seller.Address = value.Address;
                seller.Phone = value.Phone;
                seller.Email = value.Email;
                seller.TotalPrice = value.TotalPrice;
                seller.Payment = value.Payment;
                
                
             
                _context.SaveChanges();
                return seller;

            }
            catch (Exception e)
            {
                error = e.Message;
                return value;

            }
        }
    }
}
