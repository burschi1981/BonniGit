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
        
        private string _user;
        private double _amount;
        private DateTime? _payDate;
        private string _paymentType;
        private IList<IPayment> _payments;        
        private int? _id;
        private string _details;
        private IShop _shop;
        private bool _settled;


        #region Konstruktoren
        public Receipt(int id, string user, DateTime? payDate, string paymentType, string details, double amount, IList<IPayment> payments, IShop shop, bool settled, DateTime? settlementdate) : base()
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

        public string User
        {
            get { return _user; }

            set { _user = value; }
        }


        public double Amount
        {
            get { return _amount; }

            set { _amount = value; }
        }



        public DateTime? PayDate
        {
            get { return _payDate; }

            set { _payDate = value; }
        }


        public IList<IPayment> Payments
        {
            get { return _payments; }

            set { _payments = value; }
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

        public string Details
        {
            get { return _details; }

            set { _details = value; }
        }

        public IShop Shop
        {
            get { return _shop; }

            set { _shop = value; }
        }


        public bool Settled
        {
            get { return _settled; }

            set { _settled = value; }
        }

        



        public DateTime? SettlementDate
        {
            get { return _settlementDate; }
            set { _settlementDate = value; }
        }


        //protected IList<ICategory> _categories;

        //public IList<ICategory> Categories
        //{
        //    get
        //    {
        //        return _categories;
        //    }

        //    set
        //    {
        //        _categories = value;
        //    }
        //}

        #endregion
    }
}
