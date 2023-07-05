using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Decent.IMS.Data;

namespace Decent.IMS.BL
{
    public class ProductBL
    {
        private DecentDbEntities _context=new DecentDbEntities();

        public List<Product> GetAll(string key="")
        {
            IEnumerable<Product> query = _context.Products;

            if (!string.IsNullOrEmpty(key))
            {
                int x;
                if (Int32.TryParse(key, out x))
                {
                    query = query.Where(q => q.ID==x || q.PriceRate==x);
                }
                else
                {
                
                    query = query.Where(q => q.Name.Contains(key) || q.ProductCategoryName.Contains(key));
                }
                
            }
            return query.ToList();
        }

        public List<Product> GetAllProNameProCat(string key1 = "",string key2 ="")
        {
            IEnumerable<Product> query = _context.Products;

            query = query.Where(q => q.Name.Contains(key1) && q.ProductCategoryName.Contains(key2));

            return query.ToList();
        }

        public List<Product> GetAll1(string key = "")
        {
            IEnumerable<Product> query = _context.Products;

            if (!string.IsNullOrEmpty(key))
            {
                int x;
                if (Int32.TryParse(key, out x))
                {
                    query = query.Where(q => q.ID == x);
                }
                else
                {
                    query = query.Where(q => q.Name.Contains(key) || q.ProductCategoryName.Contains(key));
                }

                
               
            }
            return query.ToList();
        }

      public bool Delete(int id,out string error)
        {
            error = string.Empty;
            var product = _context.Products.FirstOrDefault(u => u.ID == id);

            if (product == null)
            {
                error = "Invalid Product Id...!!";
                return false;
            }

            try
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
            return true;
        }

        public Product Save(Product value, out string error)
        {
            error = string.Empty;
            try
            {
               var product= _context.Products.FirstOrDefault(u => u.ID == value.ID);

                if (product == null)
                {
                    product=new Product();
                    _context.Products.Add(product);
                }

                product.Name = value.Name;
                product.PriceRate = value.PriceRate;
                product.Availability = value.Availability;
                product.Benifit = value.Benifit;
                product.SalesmanId = value.SalesmanId;
                product.DaySellId = value.DaySellId;
                product.ProductCategoryId = value.ProductCategoryId;
                product.SellerId = value.SellerId;

              


                _context.SaveChanges();
                return product;

            }
            catch (Exception e)
            {
                error = e.Message;
                return value;

            }
        }

    }
}
