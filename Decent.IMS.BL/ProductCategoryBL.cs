using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Decent.IMS.Data;

namespace Decent.IMS.BL
{
    public class ProductCategoryBL
    {
        private DecentDbEntities _context=new DecentDbEntities();

        public List<ProductCategory> GetAll(string key="")
        {
            IEnumerable<ProductCategory> query = _context.ProductCategories;

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
            var productCategory = _context.ProductCategories.FirstOrDefault(u => u.ID == id);

            if (productCategory == null)
            {
                error = "Invalid Product Category Id...!!";
                return false;
            }

            try
            {
                _context.ProductCategories.Remove(productCategory);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
            return true;
        }

        public ProductCategory Save(ProductCategory value, out string error)
        {
            error = string.Empty;
            try
            {
               var productCategory= _context.ProductCategories.FirstOrDefault(u => u.ID == value.ID);

                if (productCategory == null)
                {
                    productCategory=new ProductCategory();
                    _context.ProductCategories.Add(productCategory);
                }

                productCategory.Name = value.Name;

                _context.SaveChanges();
                return productCategory;

            }
            catch (Exception e)
            {
                error = e.Message;
                return value;

            }
        }
    }
}
