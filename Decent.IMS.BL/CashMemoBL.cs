using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Decent.IMS.Data;

namespace Decent.IMS.BL
{
    public class CashMemoBL
    {
        private DecentDbEntities _context = new DecentDbEntities();

        public List<CashMemo> GetAll(string key="")
        {
            IEnumerable<CashMemo> query = _context.CashMemoes;

            if (!string.IsNullOrEmpty(key))
            {
                int x;
                if (Int32.TryParse(key, out x))
                {
                    query = query.Where(q => q.ID==x);
                }
                else
                {
                
                    query = query.Where(q => q.CustomerName.Contains(key) ||
                                                 q.CustomerPhone.Contains(key));
                }
                
            }
            return query.ToList();
        }

        public bool Delete(int id,out string error)
        {
            error = string.Empty;
            var cashMemo = _context.CashMemoes.FirstOrDefault(u => u.ID == id);

            if (cashMemo == null)
            {
                error = "Invalid CashMemo Id...!!";
                return false;
            }

            try
            {
                _context.CashMemoes.Remove(cashMemo);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
            return true;
        }

        public CashMemo Save(CashMemo value, out string error)
        {
            error = string.Empty;
            try
            {
               var cashMemo= _context.CashMemoes.FirstOrDefault(u => u.ID == value.ID);

                if (cashMemo == null)
                {
                    cashMemo=new CashMemo();
                    _context.CashMemoes.Add(cashMemo);
                }

                cashMemo.CustomerName = value.CustomerName;
                cashMemo.CustomerPhone = value.CustomerPhone;
                cashMemo.Date = value.Date;
                cashMemo.Salesman = value.Salesman;
                cashMemo.Phone = value.Phone;
                cashMemo.Time = value.Time;

               

                _context.SaveChanges();
                return cashMemo;

            }
            catch (Exception e)
            {
                error = e.Message;
                return value;

            }
        }
    }
}
