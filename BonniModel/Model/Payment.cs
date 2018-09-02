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
        private double _amount;
        private string _details;
        private string _paymentType;
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


        public double Amount
        {
            get { return _amount; }

            set { _amount = value; }
        }



        public string Details
        {
            get { return _details; }

            set { _details = value; }
        }



        public string PaymentType
        {
            get { return _paymentType; }

            set { _paymentType = value; }
        }

        public int? ID
        {
            get { return _id; }

            set { _id = value; }
        }
    }
}
