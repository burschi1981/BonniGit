using BonniModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonnyUI.Model
{
    public class Payment : HasCategories, IPayment
    {
        #region private fields
        private int? _id;
        #endregion

        #region ctor
        public Payment(int? id, double amount, string details, string paymentType) : base()
        {
            ID = id;
            Amount = amount;
            Details = details;
            PaymentType = paymentType;
        }

        public Payment() :base()
        {
            Amount = 0d;
            Details = "";
            PaymentType = Constants.PaymentTypes.PaidForSelf;
        }
        #endregion


        public double Amount { get; set; }



        public string Details { get; set; }



        public string PaymentType { get; set; }

        public int? ID
        {
            get { return _id; }

            set { _id = value; }
        }
    }
}
