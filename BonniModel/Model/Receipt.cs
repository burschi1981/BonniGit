using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BonniModel.Interfaces;
using BonniModel.Model;

namespace BonnyUI.Model
{
    public class Receipt : HasCategories, IReceipt
    {
        private DateTime? _settlementDate;        
        private DateTime? _payDate;
        private int? _id;
        

        #region Konstruktoren
        public Receipt(int id, string user, DateTime? payDate, string paymentType, string details, double amount, IList<IPayment> payments, Shop shop, bool settled, DateTime? settlementdate) : base()
        {

            ID = id;

            User = user;
            PayDate = payDate;
            PaymentType = paymentType;

            Amount = amount;
            Details = details;
            Shop = shop;

            Payments = payments;

            Settled = settled;
            SettlementDate = settlementdate;

        }

        public Receipt() : base()
        {
            User = Constants.Users.Nina;
            PayDate = DateTime.Today;
            PaymentType = Constants.PaymentTypes.PaidForBoth;

            Amount = 0d;
            Details = "";
            
            Payments = new List<IPayment>();

            Settled = false;         
        }
        #endregion


        #region public properties

        public string User { get; set; }
        
        public double Amount { get; set; }
        
        public DateTime? PayDate
        {
            get { return _payDate; }

            set { _payDate = value; }
        }
        
        public IList<IPayment> Payments { get; set; }
        
        public string PaymentType { get; set; }
        
        public int? ID
        {
            get { return _id; }

            set { _id = value; }
        }

        public string Details { get; set; }

        public Shop Shop { get; set; }
        
        public bool Settled { get; set; }
        
        public DateTime? SettlementDate
        {
            get { return _settlementDate; }
            set { _settlementDate = value; }
        }

        #endregion
    }
}
