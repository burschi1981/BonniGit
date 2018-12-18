using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BonnyUI.Model;
using BonniModel.Interfaces;
using BonniViewModel.ViewModel;
using BonnyUI.DBConnection;

namespace BonnyUI.ViewModel
{
    public class BonViewModel: ViewModelBase
    {
        #region Fields
        private DBConnector _DBConnection;
        private bool _balance = false;
        private bool _editMode;
        private string _user = "";
        private bool _settled;
        private DateTime? _payDate = DateTime.Now;
        private DateTime? _settlementDate;
        


        IList<IPayment> _payments = new List<IPayment>();
        
        private string _paymentType;
        private int? _id;
        private string _details = "";

        private ShopViewModel _shopViewModel;
        private IReceipt _bon;
        private PaymentViewModel _currentPayment;


        private ObservableCollection<PaymentViewModel> _allPayments;
        
        private CategoryAdapter _categoryAdapter;
        #endregion


        #region Konstruktoren
        public BonViewModel(IReceipt bon, DBConnector dbConnection)        
        {
            _DBConnection = dbConnection;
            _bon = bon;
            _user = _bon.User;
            _payDate = _bon.PayDate;
            _details = _bon.Details;
            _amount = bon.Amount;
            _settlementDate = _bon.SettlementDate;
            _id = _bon.ID;
            _settled = _bon.Settled;
            _paymentType = _bon.PaymentType;
            _payments = _bon.Payments;
            _categoryAdapter = new CategoryAdapter(bon.Categories, dbConnection);
            
            Balance = false;
            LoadPaymentViewModels();
            
            NewPaymentCommand = new ActionCommand(NewPayment, null);
            SaveBonCommand = new ActionCommand(SaveBon, CanSaveBon);
            EditBonCommand = new ActionCommand(EditBon, CanEditBon);
            LiftPayDateCommand = new ActionCommand(LiftPayDate, null);
            ReducePayDateCommand = new ActionCommand(ReducePayDate, null);
            CanBeEdited = false;
        }

        private void ReducePayDate(object obj)
        {
            PayDate = PayDate.Value.AddDays(-1);
        }

        private void LiftPayDate(object obj)
        {
            PayDate = PayDate.Value.AddDays(1);
        }
        #endregion


        #region public Properties and Commands
        public ICommand NewPaymentCommand { get; private set; }
        public ICommand DeletePaymentCommand { get; private set; }

        public ICommand EditBonCommand { get; private set; }
        public ICommand SaveBonCommand { get; private set; }

        public ICommand ReducePayDateCommand { get; private set; }
        public ICommand LiftPayDateCommand { get; private set; }
        public ObservableCollection<PaymentViewModel> AllPayments
        {
            get
            {
                return _allPayments;
            }
        }

        
        public CategoryAdapter CategoryAdapter
        {
            get { return _categoryAdapter; }
            set { _categoryAdapter = value; }
        }

        public bool PaymentsExist
        {
            get
            {
                return AllPayments != null && AllPayments.Count > 0;
            }
        }





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
                RaisePropertyChanged("UserToPay");
                RaisePropertyChanged("SumToPay");
            }
        }


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


        public ShopViewModel ShopViewModel
        {
            get
            {
                return _shopViewModel;
            }

            set
            {
                _shopViewModel = value;
                RaisePropertyChanged("ShopViewModel");
            }
        }



        public IReceipt Bon
        {
            get
            {
                return _bon;
            }

            set
            {
                _bon = value;
                RaisePropertyChanged("Bon");
            }
        }



        public PaymentViewModel CurrentPayment
        {
            get { return _currentPayment; }
            set
            {
                _currentPayment = value;
                RaisePropertyChanged("CurrentPayment");
            }
        }

        public bool Settled
        {
            get { return _settled; }
            set { _settled = value; }
        }

       



        public DateTime? PayDate
        {
            get
            {
                return _payDate;
            }

            set
            {
                _payDate = value;
                RaisePropertyChanged("PayDate");
                RaisePropertyChanged("SumToPay");
            }
        }

        public DateTime? SettlementDate
        {
            get
            {
                return _settlementDate;
            }

            set
            {
                _settlementDate = value;
                RaisePropertyChanged("SettlementDate");
            }
        }




        public IList<IPayment> Payments
        {
            get
            {
                return _payments;
            }

            set
            {
                _payments = value;
                RaisePropertyChanged("Payments");
                RaisePropertyChanged("SumToPay");

            }
        }


        public bool Balance
        {
            get { return _balance; }
            set
            {
                _balance = value;
                RaisePropertyChanged("Balance");
            }
        }

        
       


        public bool CanBeEdited
        {
            get { return _editMode; }
            set
            {
                _editMode = value;
                RaisePropertyChanged("CanBeEdited");
            }
        }

        public bool BonIsSaved
        {
           
            get { return !CanBeEdited; }
        }



        



        public string User
        {
            get
            {
                return _user;
            }

            set
            {
                _user = value;
                RaisePropertyChanged("User");
                RaisePropertyChanged("UserToPay");
                RaisePropertyChanged("SumToPay");
                RaisePropertyChanged("MarcsBon");
                RaisePropertyChanged("NinasBon");
            }
        }

        public bool MarcsBon
        {
            get
            {
                return User.Equals(Constants.Users.Marc);
            }
            set
            {
                User = Constants.Users.Marc;
            }
        }

        public bool NinasBon
        {
            get
            {
                return User.Equals(Constants.Users.Nina);
            }
            set
            {
                User = Constants.Users.Nina;
            }
        }



        public string UserToPay
        {
            get
            {
                if (User.Equals(Constants.Users.Marc))
                    return Constants.Users.Nina;
                else
                    return Constants.Users.Marc;
            }
        }

        public double SumToPay
        {
            get
            {

                double retval = 0d;
                if (PaymentType.Equals(Constants.PaymentTypes.PaidForBoth))
                    retval = (Amount - SumOfPaymentsForSelf() - SumOfPaymentsForTheOther()) / 2 + SumOfPaymentsForTheOther();
                else if (PaymentType.Equals(Constants.PaymentTypes.PaidForSelf))
                    retval = SumOfPaymentsForTheOther() + SumOfPaymentsForBoth() / 2;
                else if (PaymentType.Equals(Constants.PaymentTypes.PaidForTheOther))
                    retval = Amount - SumOfPaymentsForSelf() - SumOfPaymentsForBoth() / 2;
                return retval;
            }
        }


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
                RaisePropertyChanged("SumToPay");
            }
        }

        #endregion

        #region public voids
        public void ChangedEverything()
        {
            RaisePropertyChanged("AllPayments");
            RaisePropertyChanged("User");
            RaisePropertyChanged("Amount");
            RaisePropertyChanged("Payments");
            RaisePropertyChanged("PayDate");
            RaisePropertyChanged("Details");
            RaisePropertyChanged("AllShops");
            RaisePropertyChanged("CurrentShop");
            RaisePropertyChanged("PaymentType");
            RaisePropertyChanged("SumToPay");
            RaisePropertyChanged("UserToPay");
            CategoryAdapter.ReloadCategories();
            foreach(PaymentViewModel pvm in AllPayments)
            {
                pvm.CategoryAdapter.ReloadCategories();
            }
        }
        #endregion

        #region private voids and delegates

        private PaymentViewModel GetCurrentPayment()
        {
             if (AllPayments.Count > 0)
                return AllPayments[0];
            else
                return null;
        }

        
        private void LoadPaymentViewModels()
        {
            // Geschäfte laden:
            ObservableCollection<PaymentViewModel> obs = new ObservableCollection<PaymentViewModel>();
            foreach (IPayment zahlung in this.Payments)
            {
                PaymentViewModel zvm = new PaymentViewModel(zahlung, _DBConnection);
                zvm.PropertyChanged -= dingsda;
                zvm.PropertyChanged += dingsda;
                obs.Add(zvm);
            }

            _allPayments = obs;

            CurrentPayment = GetCurrentPayment();
            if (CurrentPayment == null && AllPayments.Count > 0)
                CurrentPayment = AllPayments[0];
            
            RaisePropertyChanged("AllShops");
        }

        


        private bool CanSaveBon(object obj)
        {
            return !Settled && CanBeEdited;
        }

        private void SaveBon(object obj)
        {   
            Save();
            CanBeEdited = false;
            RaisePropertyChanged("Saved");
            RaisePropertyChanged("BonIsSaved");
        }


        private bool CanEditBon(object obj)
        {
            return !Settled && !CanBeEdited;
        }


        private void EditBon(object obj)
        {
            CanBeEdited = true;
            Balance = false;
            RaisePropertyChanged("BonIsSaved");
            RaisePropertyChanged("CanBeEdited");
            RaisePropertyChanged("Balance");
        }



        private void NewPayment(object obj)
        {
            IPayment newPayment = new Payment();
            _bon.Payments.Add(newPayment);
            PaymentViewModel zahlungWrapper = new PaymentViewModel(newPayment, _DBConnection);
            zahlungWrapper.PropertyChanged -= dingsda;
            zahlungWrapper.PropertyChanged += dingsda;

            AllPayments.Add(zahlungWrapper);
            CurrentPayment = zahlungWrapper;
            
            RaisePropertyChanged("CurrentPayment");
            RaisePropertyChanged("SumToPay");
            RaisePropertyChanged("PaymentsExist");
        }

        private double SumOfPaymentsForSelf()
        {
            double retval = 0d;
            foreach (PaymentViewModel zahlung in this.AllPayments)
                if (zahlung.PaymentType.Equals(Constants.PaymentTypes.PaidForSelf))
                    retval += zahlung.Amount;
            return retval;
        }

        private double SumOfPaymentsForTheOther()
        {
            double retval = 0d;
            foreach (PaymentViewModel zahlung in this.AllPayments)
                if (zahlung.PaymentType.Equals(Constants.PaymentTypes.PaidForTheOther))
                    retval += zahlung.Amount;
            return retval;
        }

        private double SumOfPaymentsForBoth()
        {
            double retval = 0d;
            foreach (PaymentViewModel zahlung in this.AllPayments)
                if (zahlung.PaymentType.Equals(Constants.PaymentTypes.PaidForBoth))
                    retval += zahlung.Amount;
            return retval;
        }


        private double _amount = 0d;

        internal void Save()
        {
            Bon.User = this.User;
            Bon.PayDate = this.PayDate;
            Bon.Details = this.Details;
            Bon.Amount = this.Amount;
            Bon.Settled = this.Settled;
            Bon.Shop = this._shopViewModel.WrappedObject;
           
            Bon.SettlementDate = this.SettlementDate;
            

            Bon.Categories.Clear();
            foreach (CategoryViewModel catVM in this.CategoryAdapter.AllCategories.Where(x => x.IsSelected).ToList())
                Bon.Categories.Add(catVM.WrappedObject);
            Bon.PaymentType = this.PaymentType;
            
            foreach (PaymentViewModel zvm in _allPayments)
            {
                zvm.Save();
                IPayment p = zvm.WrappedObject;
                p.Categories.Clear();
                foreach (CategoryViewModel catVM in zvm.CategoryAdapter.AllCategories.Where(x => x.IsSelected).ToList())
                    p.Categories.Add(catVM.WrappedObject);
            }


            this.CanBeEdited = false;
        }

        private void dingsda(object sender, PropertyChangedEventArgs e)
        {
           
            RaisePropertyChanged("SumToPay");
        }

        #endregion










        

        

        

        

        

        
    }
}
