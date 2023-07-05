using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Decent.IMS.Data;

namespace Decent.IMS.BL
{
    public class SellDetailsBL
    {
        private DecentDbEntities _context = new DecentDbEntities();

        public List<SellDetail> GetAll(string key="")
        {
            IEnumerable<SellDetail> query = _context.SellDetails;
            
            if (!string.IsNullOrEmpty(key))
            {
                string phone = key.Substring(0, 2);
                int x;
                DateTime b;
                if ((Int32.TryParse(key, out x) && phone!="01") || DateTime.TryParse(key, out b))
                {
                    if (Int32.TryParse(key, out x))
                    {
                        query = query.Where(q => q.CashMemoId == x);
                    }
                    DateTime a;
                    if (DateTime.TryParse(key, out a))
                    {
                        query = query.Where(q => q.Date == a);
                    }
                    
                }
                  
                else
                {

                    query = query.Where(q => q.Salesman.Contains(key) ||
                                             q.Customer.Contains(key)  ||
                                             q.CustomerPhone.Contains(key));
                    
                }

            }
            
            return query.ToList();
        }



        public List<SellDetail> GetAllDateRange(string startDate = "", string endDate = "")
        {
            IEnumerable<SellDetail> query = _context.SellDetails;

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
            var sellDetails = _context.SellDetails.FirstOrDefault(u => u.ID == id);

            if (sellDetails == null)
            {
                error = "Invalid Sell Details Id...!!";
                return false;
            }

            try
            {
                _context.SellDetails.Remove(sellDetails);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
            return true;
        }

        public SellDetail Save(SellDetail value, out string error)
        {
            error = string.Empty;
            try
            {
               var sellDetails= _context.SellDetails.FirstOrDefault(u => u.ID == value.ID);

                if (sellDetails == null)
                {
                    sellDetails=new SellDetail()
                    {
                        Date = DateTime.Now
                    };
                    _context.SellDetails.Add(sellDetails);
                }

                sellDetails.Date = value.Date;
                sellDetails.Salesman = value.Salesman;
                sellDetails.Customer = value.Customer;
                sellDetails.CustomerPhone = value.CustomerPhone;
                sellDetails.ProductName = value.ProductName;
                sellDetails.ProductCategoryId = value.ProductCategoryId;
                sellDetails.RealPriceRate = value.RealPriceRate;
                sellDetails.SellPriceRate = value.SellPriceRate;
                sellDetails.Amount = value.Amount;
                sellDetails.TotalPrice = value.TotalPrice;
                sellDetails.Benifit = value.Benifit;
                sellDetails.CashMemoId = value.CashMemoId;
                sellDetails.Time = value.Time;
               


                _context.SaveChanges();
                return sellDetails;

            }
            catch (Exception e)
            {
                error = e.Message;
                return value;

            }
        }

        public List<SellDetail> GetAllOrder(string key = "")
        {
            IEnumerable<SellDetail> query = _context.SellDetails;

            
            if (!string.IsNullOrEmpty(key))
            {
                int x;
                if (Int32.TryParse(key, out x))
                {
                    query = query.Where(q => q.CashMemoId == x);
                }
                
            }
             
            return query.ToList();
        }

        public List<SellDetail> GetAllOrderCategory(string key1 = "", string key2 = "")
        {
            IEnumerable<SellDetail> query = _context.SellDetails;


            if (!string.IsNullOrEmpty(key1))
            {
                int x;
                if (Int32.TryParse(key1, out x))
                {
                    query = query.Where(q => q.CashMemoId == x);
                }

                query = query.Where(q =>q.ProductCategory.Name.Contains(key2));

            }

            return query.ToList();
        }

       public List<SellDetail> GetAllPhone(string key = "")
        {
            IEnumerable<SellDetail> query = _context.SellDetails;


            if (!string.IsNullOrEmpty(key))
            {

                query = query.Where(q => q.CustomerPhone.Contains(key));

            }

            return query.ToList();
        }

        
        public List<SellDetail> GetAllPhoneCat(string key1 = "", string key2 = "")
        {
            IEnumerable<SellDetail> query = _context.SellDetails;


            if (!string.IsNullOrEmpty(key1) && !string.IsNullOrEmpty(key2))
            {

                query = query.Where(q => q.CustomerPhone.Contains(key1) && q.Category.Contains(key2));

            }

            return query.ToList();
        }

        public List<SellDetail> GetAllPhoneCatProName(string key1 = "", string key2 = "", string key3 = "")
        {
            IEnumerable<SellDetail> query = _context.SellDetails;


            if (!string.IsNullOrEmpty(key1) && !string.IsNullOrEmpty(key2) && !string.IsNullOrEmpty(key3))
            {

                query = query.Where(q => q.CustomerPhone.Contains(key1) && 
                                         q.Category.Contains(key2) &&
                                         q.ProductName.Contains(key3));

            }

            return query.ToList();
        }

        public List<SellDetail> GetAllPhoneCatProNamePriceRate(string key1 = "", string key2 = "", string key3 = "", string key4 = "")
        {
            IEnumerable<SellDetail> query = _context.SellDetails;


            if (!string.IsNullOrEmpty(key1) && 
                !string.IsNullOrEmpty(key2) && 
                !string.IsNullOrEmpty(key3) && 
                !string.IsNullOrEmpty(key4))
            {

                query = query.Where(q => q.CustomerPhone.Contains(key1) &&
                                         q.Category.Contains(key2) &&
                                         q.ProductName.Contains(key3));


                double x;
                if (Double.TryParse(key4, out x))
                {
                    query = query.Where(q => q.SellPriceRate == x);
                }

            }

            return query.ToList();
        }

        public List<SellDetail> GetAllPhoneCatProNamePriceRateDate(string key1 = "", string key2 = "", string key3 = "", string key4 = "", string key5 = "")
        {
            IEnumerable<SellDetail> query = _context.SellDetails;


            if (!string.IsNullOrEmpty(key1) &&
                !string.IsNullOrEmpty(key2) &&
                !string.IsNullOrEmpty(key3) &&
                !string.IsNullOrEmpty(key4) &&
                !string.IsNullOrEmpty(key5))
            {

                query = query.Where(q => q.CustomerPhone.Contains(key1) &&
                                         q.Category.Contains(key2) &&
                                         q.ProductName.Contains(key3));


                double x;
                if (Double.TryParse(key4, out x))
                {
                    query = query.Where(q => q.SellPriceRate == x);
                }

                DateTime a;
                if (DateTime.TryParse(key5, out a))
                {
                    query = query.Where(q => q.Date == a);
                }
            }

            return query.ToList();
        }

    }
}
