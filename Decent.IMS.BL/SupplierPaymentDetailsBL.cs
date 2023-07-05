using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Decent.IMS.Data;

namespace Decent.IMS.BL
{
    public class SupplierPaymentDetailsBL
    {
        private DecentDbEntities _context=new DecentDbEntities();

        public List<PaymentDetail> GetAll(string key="")
        {
            IEnumerable<PaymentDetail> query = _context.PaymentDetails;

            if (!string.IsNullOrEmpty(key))
            {
                 DateTime a;
                if (DateTime.TryParse(key, out a))
                {
                    query = query.Where(q => q.Date == a);
                }
                else
                {


                    query = query.Where(q => q.SellerName.Contains(key) ||
                                            q.Phone.Contains(key));
                }
            }
            return query.ToList();
        }


        public List<PaymentDetail> GetAllDateRange(string startDate = "", string endDate = "")
        {
            IEnumerable<PaymentDetail> query = _context.PaymentDetails;

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
            var paymentDetails = _context.PaymentDetails.FirstOrDefault(u => u.ID == id);

            if (paymentDetails == null)
            {
                error = "Invalid Payment Details Id...!!";
                return false;
            }

            try
            {
                _context.PaymentDetails.Remove(paymentDetails);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
            return true;
        }

        public PaymentDetail Save(PaymentDetail value, out string error)
        {
            error = string.Empty;
            try
            {
               var paymentDetails= _context.PaymentDetails.FirstOrDefault(u => u.ID == value.ID);

                if (paymentDetails == null)
                {
                    paymentDetails=new PaymentDetail();
                    _context.PaymentDetails.Add(paymentDetails);
                }

                paymentDetails.Date = value.Date;
                paymentDetails.SellerName = value.SellerName;
                paymentDetails.Phone = value.Phone;
                paymentDetails.TotalDue = value.TotalDue;
                paymentDetails.Payment = value.Payment;
               

                _context.SaveChanges();
                return paymentDetails;

            }
            catch (Exception e)
            {
                error = e.Message;
                return value;

            }
        }
    }
}
