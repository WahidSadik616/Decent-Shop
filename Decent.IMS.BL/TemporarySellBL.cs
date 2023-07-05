using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Decent.IMS.Data;

namespace Decent.IMS.BL
{
    public class TemporarySellBL
    {
        private DecentDbEntities _context = new DecentDbEntities();

        public List<TemporarySell> GetAll(string key="")
        {
            IEnumerable<TemporarySell> query = _context.TemporarySells;

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

        public bool Delete(int id,out string error)
        {
            error = string.Empty;
            var temporarySell = _context.TemporarySells.FirstOrDefault(u => u.ID == id);

            if (temporarySell == null)
            {
                error = "Invalid Temporary Sell Id...!!";
                return false;
            }

            try
            {
                _context.TemporarySells.Remove(temporarySell);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
            return true;
        }
        public bool DeleteTemporaryProduct(int count, out string error)
        {
            error = string.Empty;
            var temporarySell = _context.TemporarySells.FirstOrDefault(u => u.Count == count);

            if (temporarySell == null)
            {
                error = "Invalid Temporary Sell Id...!!";
                return false;
            }

            try
            {
                _context.TemporarySells.Remove(temporarySell);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
            return true;
        }

        public TemporarySell Save(TemporarySell value, out string error)
        {
            error = string.Empty;
            try
            {
               var temporarySell= _context.TemporarySells.FirstOrDefault(u => u.ID == value.ID);

                if (temporarySell == null)
                {
                    temporarySell=new TemporarySell();
                    _context.TemporarySells.Add(temporarySell);
                }

                temporarySell.Count = value.Count;
                temporarySell.Date = value.Date;
                temporarySell.Name = value.Name;
                temporarySell.ProductCategoryId = value.ProductCategoryId;
                temporarySell.Quantity = value.Quantity;
                temporarySell.RealPriceRate = value.RealPriceRate;
                temporarySell.PriceRate = value.PriceRate;
                temporarySell.TotalPrice = value.TotalPrice;
                temporarySell.Benifit = value.Benifit;
                temporarySell.ProductId = value.ProductId;


                _context.SaveChanges();
                return temporarySell;

            }
            catch (Exception e)
            {
                error = e.Message;
                return value;

            }
        }
    }
}
