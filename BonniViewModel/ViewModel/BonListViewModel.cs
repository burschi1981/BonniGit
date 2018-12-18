using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BonnyUI.Model;
using BonnyUI.DBConnection;

namespace BonnyUI.ViewModel
{
    public class BonListViewModel : ViewModelBase
    {
        #region Fields
        private DBConnector _DBConnection;

        private IList<string> _allTypes = new List<string>() { Constants.PaymentTypes.PaidForBoth, Constants.PaymentTypes.PaidForSelf, Constants.PaymentTypes.PaidForTheOther };

        private IList<string> _users = new List<string>() { Constants.Users.Nina, Constants.Users.Marc };

        private IList<IReceipt> _bons;

        private ObservableCollection<BonViewModel> _allBons;

        private BonViewModel _currentBon;

        private ShopAdminViewModel _shopAdmin;

        private bool _isFiltered;
        #endregion



        #region ctor

        public BonListViewModel(IList<IReceipt> bons, ShopAdminViewModel shopAdmin, DBConnector dbConnection)
        {
            this._DBConnection = dbConnection;
            this._bons = bons;
            this._shopAdmin = shopAdmin;
            ReloadViewmodel();

            CreateBonCommand = new ActionCommand(CreateBon, null);
            BalanceBonsCommand = new ActionCommand(BalanceBons, CanBalanceBons);
            MarkAllBonsCommand = new ActionCommand(MarkAllBons, CanMarkAllBons);
        }

        public void SetBons(IList<IReceipt> bons)
        {
            this._bons = bons;
            ReloadViewmodel();
        }

        #endregion

        #region public voids
        public ICommand CreateBonCommand { get; private set; }

        
        public ICommand BalanceBonsCommand { get; private set; }
        public ICommand MarkAllBonsCommand { get; private set; }

        public bool IsFiltered
        {
            get
            {
                return _isFiltered;   
            }
            set
            {
                _isFiltered = value;
                RaisePropertyChanged("AllBons");
            }
        }


        public double SumMarc
        {
            get
            {
                double retval = 0;
                foreach (BonViewModel bvm in _allBons)
                {
                    if (bvm.User.Equals(Constants.Users.Marc) && bvm.Balance)
                        retval += bvm.SumToPay;
                }
                return retval;
            }
        }

        

        public double SumNina
        {
            get
            {
                double retval = 0;
                foreach (BonViewModel bvm in _allBons)
                {
                    if (bvm.User.Equals(Constants.Users.Nina) && bvm.Balance)
                        retval += bvm.SumToPay;
                }
                return retval;
            }
        }

        public double NinaZahlt
        {
            get
            {
                return Math.Max(0d, SumMarc - SumNina);
            }
        }

        public double MarcZahlt
        {
            get
            {
                return Math.Max(0d, SumNina - SumMarc);
            }
        }

        public IList<string> AllTypes
        {
            get { return _allTypes; }
        }





        public ObservableCollection<BonViewModel> AllBons
        {
            get { return _allBons; }
        }

        public IList<string> Users
        {
            get { return _users; }

        }

        public bool BonNichtNull
        {
            get { return CurrentBon != null; }
        }



        public ShopAdminViewModel ShopAdmin
        {
            get { return _shopAdmin; }
        }


        public BonViewModel CurrentBon
        {
            get { return _currentBon; }
            set
            {
                _currentBon = value;

                if (value != null)
                    CurrentBon.ChangedEverything();
                RaisePropertyChanged("CurrentBon");
                RaisePropertyChanged("BonNichtNull");
            }
        }
        #endregion

        #region private voids


        private void ReloadBonViewModels()
        {
            int? currentID = null;
            if (CurrentBon != null)
                currentID = CurrentBon.ID;


            //_allBons.Clear();
            ObservableCollection<BonViewModel> obs2 = new ObservableCollection<BonViewModel>();
            foreach (IReceipt bon in this._bons)
            {
                //BonViewModel bvm = new BonViewModel(bon, this._DBConnection);
                BonViewModel bvm = new BonViewModel(bon, _DBConnection);

                if (_allBons != null)
                {
                    BonViewModel bvm2 = _allBons.Where(x => x.ID.Equals(bvm.ID)).FirstOrDefault();
                    if (bvm2 != null)
                        bvm.Balance = bvm2.Balance;
                }

                bvm.PropertyChanged -= EventuellBetragGeändert;
                bvm.PropertyChanged += EventuellBetragGeändert;
                bvm.PropertyChanged -= CurrentBonSaved;
                bvm.PropertyChanged += CurrentBonSaved;
                obs2.Insert(0, bvm);
            }
             if(_allBons != null)
                foreach (BonViewModel bonvm in obs2)
                {
                    BonViewModel a = _allBons.Where(x => x.ID.Equals(bonvm.ID)).FirstOrDefault() as BonViewModel;
                    if (a != null)
                        bonvm.CanBeEdited = a.CanBeEdited;
                }
            _allBons = obs2;

            if (AllBons.Count == 0)
                CurrentBon = null;
            else
                if (currentID != null)
                CurrentBon = _allBons.Where(x => x.ID.Equals(currentID)).FirstOrDefault();
                    
                
            RaisePropertyChanged("AllBons");
        }

        public void ReloadShopsInBons()
        {
            if (_allBons != null)
                foreach (BonViewModel bvm in _allBons)
                {
                    if (bvm.Bon.Shop != null)
                        bvm.ShopViewModel = _shopAdmin.AllShops.Where(x => x.ID.Equals(bvm.Bon.Shop.ID)).FirstOrDefault();
                    else
                        bvm.ShopViewModel = _shopAdmin.AllShops[0];
                    bvm.ChangedEverything();
                    //bvm.Changed = false;
                }
        }

        private bool CanBalanceBons(object obj)
        {
            return _allBons.Where(x => x.Balance).ToList().Count > 0;
        }

        
        

        private void SaveBonsInDB()
        {
            foreach (BonViewModel bvm in _allBons)
                bvm.Save();
            foreach (IReceipt bon in _bons)
            {
                int id = _DBConnection.BalanceBon(bon);
            }
        }

        private void SaveCurrentBon()
        {
            //CurrentBon.Save();
            IReceipt bon = CurrentBon.Bon;
            
            int id = _DBConnection.SaveBon(bon);
            // CurrentBon = null;
        }

        private void BalanceBons(object obj)
        {
            foreach (BonViewModel bvm in _allBons)
                if (bvm.Balance)
                {
                    bvm.Settled = true;
                }
            SaveBonsInDB();
            // Hier Bons neu setzen
            RaisePropertyChanged("Bons gespeichert");
            _bons = _DBConnection.GetAllBons().Where(x => !x.Settled).ToList();
            ReloadViewmodel();
        }



        private void MarkAllBons(object obj)
        {

            foreach (BonViewModel bvm in this.AllBons)
                if (!bvm.Settled && !bvm.CanBeEdited)
                    bvm.Balance = true;
        }

        private bool CanMarkAllBons(object obj)
        {
            bool retval = true;
            
            return retval;
        }

        private void ReloadViewmodel()
        {
            ReloadBonViewModels();
            ReloadShopsInBons();
        }

        private void ReloadSums()
        {

            RaisePropertyChanged("SumMarc");
            RaisePropertyChanged("SumNina");
            RaisePropertyChanged("NinaZahlt");
            RaisePropertyChanged("MarcZahlt");
        }


        
        private void CreateBon(object obj)
        {
            IReceipt newBon = new BonnyUI.Model.Receipt();

            if (CurrentBon != null)
                newBon.User = CurrentBon.User;
            _bons.Add(newBon);
            BonViewModel bvm = new BonViewModel(newBon, _DBConnection);            

            bvm.ShopViewModel = _shopAdmin.AllShops[0];
            bvm.CanBeEdited = true;
            bvm.PropertyChanged -= EventuellBetragGeändert;
            bvm.PropertyChanged += EventuellBetragGeändert;
            bvm.PropertyChanged -= CurrentBonSaved;
            bvm.PropertyChanged += CurrentBonSaved;
            _allBons.Insert(0, bvm);
            RaisePropertyChanged("AllBons");
            CurrentBon = bvm;
            RaisePropertyChanged("CurrentBon");
        }


        private void EventuellBetragGeändert(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Balance"))
                ReloadSums();
        }

        private void CurrentBonSaved(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Saved"))
            {
                this.SaveCurrentBon();
            }
        }
        #endregion

    }
}
