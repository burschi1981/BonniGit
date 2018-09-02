using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BonnyUI.Model;
using BonniViewModel.ViewModel;

namespace BonnyUI.ViewModel
{
    public class PaymentViewModel : ViewModelBase //, IPayment
    {
        private IDBConnector _DBConnection;
        private CategoryAdapter _categoryAdapter;

        public PaymentViewModel(IPayment zahlung, IDBConnector dbConnection)
        {
            _DBConnection = dbConnection;
            this._zahlung = zahlung;
            this._amount = zahlung.Amount;
            this._details = zahlung.Details;
            this.PaymentType = zahlung.PaymentType;
            this._id = zahlung.ID;
            _categoryAdapter = new CategoryAdapter(zahlung.Categories, dbConnection);
            ChangedEverything();
        }

        public CategoryAdapter CategoryAdapter
        {
            get { return _categoryAdapter; }
            set { _categoryAdapter = value; }
        }

        public IPayment WrappedObject
        {
            get { return _zahlung; }
            set { _zahlung = value; }
        }

        public void ChangedEverything()
        {
            RaisePropertyChanged("Details");
            RaisePropertyChanged("Amount");
            RaisePropertyChanged("PaymentType");
            CategoryAdapter.ReloadCategories();
        }

        private double _amount = 0d;
        public double Amount
        {
            get
            {
                return _amount;
            }

            set
            {
                _amount = value;
                RaisePropertyChanged("Amount");
            }
        }

        protected override void RaisePropertyChanged(string propertyName)
        {
            base.RaisePropertyChanged(propertyName);

        }


        private string _details = "";

        public string Details
        {
            get
            {
                return _details;
            }

            set
            {
                _details = value;
                RaisePropertyChanged("Details");
            }
        }


        private string _paymentType = Constants.PaymentTypes.PaidForBoth;
        
        public string PaymentType
        {
            get
            {
                return _paymentType;
            }

            set
            {
                _paymentType = value;
                RaisePropertyChanged("PaymentType");
            }
        }

        private int? _id;

        private IPayment _zahlung;

        public int? ID
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }

        internal void Save()
        {
            _zahlung.Amount = Amount;
            _zahlung.Details = Details;
            _zahlung.PaymentType = PaymentType;
        }
    }
}
