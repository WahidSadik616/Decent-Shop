using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Decent.IMS.Data;

namespace Decent.IMS.BL
{
    public class SellerNewProductBL
    {
        private DecentDbEntities _context=new DecentDbEntities();

        public List<SellerNewProduct> GetAll(string key="")
        {
            IEnumerable<SellerNewProduct> query = _context.SellerNewProducts;

            if (!string.IsNullOrEmpty(key))
            {
                DateTime a;
                if (DateTime.TryParse(key,out a))
                {
                    query = query.Where(q => q.Date == a);
                }
                else
                {

                    query = query.Where(q => q.Name.Contains(key) ||
                                                 q.ProductCategory.Name.Contains(key));
               }
                
            }
            return query.ToList();
        }

        public List<SellerNewProduct> GetAllDateRange(string startDate = "",string endDate="")
        {
            IEnumerable<SellerNewProduct> query = _context.SellerNewProducts;

            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                DateTime a;
                DateTime b;
                if (DateTime.TryParse(startDate, out a) && DateTime.TryParse(endDate, out b))
                {
                    query = query.Where(q => q.Date >= a && q.Date<=b);
                }
                
            }
            return query.ToList();
        }




        public bool Delete(int id,out string error)
        {
            error = string.Empty;
            var sellerNewProduct = _context.SellerNewProducts.FirstOrDefault(u => u.ID == id);

            if (sellerNewProduct == null)
            {
                error = "Invalid Seller New Product Id...!!";
                return false;
            }

            try
            {
                _context.SellerNewProducts.Remove(sellerNewProduct);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
            return true;
        }

        public SellerNewProduct Save(SellerNewProduct value, out string error)
        {
            error = string.Empty;
            try
            {
               var sellerNewProduct= _context.SellerNewProducts.FirstOrDefault(u => u.ID == value.ID);

                if (sellerNewProduct == null)
                {
                    sellerNewProduct=new SellerNewProduct();
                    _context.SellerNewProducts.Add(sellerNewProduct);
                }
                sellerNewProduct.Date = value.Date;
                sellerNewProduct.Time = value.Time;
                sellerNewProduct.Name = value.Name;
                sellerNewProduct.ProductName = value.ProductName;
                sellerNewProduct.ProductCategoryId = value.ProductCategoryId;
                sellerNewProduct.ProductAmount = value.ProductAmount;
                sellerNewProduct.PriceRate = value.PriceRate;
                sellerNewProduct.TotalPrice = value.TotalPrice;
                sellerNewProduct.Discount = value.Discount;
                sellerNewProduct.AfterTotalPrice = value.AfterTotalPrice;
                sellerNewProduct.CarryingCost = value.CarryingCost;
                sellerNewProduct.AfterPriceRate = value.AfterPriceRate;
               

                _context.SaveChanges();
                return sellerNewProduct;

            }
            catch (Exception e)
            {
                error = e.Message;
                return value;

            }
        }
    }
}
