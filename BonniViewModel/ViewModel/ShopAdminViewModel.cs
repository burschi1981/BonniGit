using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BonnyUI.ViewModel
{
    /// <summary>
    /// Viewmodel der Shop-Admin
    /// Hinzufügen und Ändern von Shops
    /// </summary>
    public class ShopAdminViewModel:ViewModelBase
    {
        #region private fields
        private int _currentID;

        private ObservableCollection<ShopViewModel> _allShops;
        
        private string _currentShopName;

        private IDBConnector _DBConnection;

        private IList<IShop> _shops;

        private ShopViewModel _currentShop;
        #endregion

        #region Konstruktoren
        public ShopAdminViewModel(IList<IShop> shops, IDBConnector dbConnection)
        {
             _shops = shops;
            SaveShopDatesAsNewCommand = new ActionCommand(SaveShopDatesAsNew, CanUpdateShopDates);
            UpdateShopDatesCommand = new ActionCommand(UpdateShopDates, CanUpdateShopDates);
            _DBConnection = dbConnection;
            ReloadShopViewModels();
        }
        #endregion
        
        #region public Properties

        public ICommand SaveShopDatesAsNewCommand { get; private set; }
        public ICommand UpdateShopDatesCommand { get; private set; }





        public string CurrentShopName
        {
            get
            {
                return _currentShopName;
            }
            set
            {
                _currentShopName = value;
            }
        }

        public IList<IShop> Shops
        {
            get { return _shops; }
            set
            {
                _shops = value;
            }
        }

        public ObservableCollection<ShopViewModel> AllShops
        {
            get { return _allShops; }
        }

        public ShopViewModel CurrentShop
        {
            get { return _currentShop; }
            set
            {
                _currentShop = value;
                if (_currentShop != null)
                {
                    _currentShopName = _currentShop.Name;
                    _currentID = _currentShop.ID;
                }
                else
                {                 
                    _currentShopName = "";
                }
                RaisePropertyChanged("CurrentShopName");
                RaisePropertyChanged("CurrentShop");
            }
        }


        #endregion
        
        #region public voids

        public void ReloadShopViewModels()
        {
            // Geschäfte laden:
            ObservableCollection<ShopViewModel> obs = new ObservableCollection<ShopViewModel>();
            foreach (IShop shop in _shops)
                obs.Add(new ShopViewModel(shop));
            _allShops = obs;
            
            CurrentShop = GetCurrentShop();
            if (CurrentShop == null && AllShops.Count > 0)
                CurrentShop = AllShops[0];
            
            RaisePropertyChanged("CurrentShop");
            RaisePropertyChanged("AllShops");
        }

        #endregion

        #region private voids


        private ShopViewModel GetCurrentShop()
        {
            var retval = _allShops.Where(x => x.ID.Equals(_currentID)).FirstOrDefault();
            return retval;
        }

        private void UpdateShopDates(object obj)
        {
            try
            {
                _currentShop.WrappedObject.Name = _currentShopName;
                
                _currentShop.Name = _currentShopName;
                
                _DBConnection.ChangeShop(CurrentShop.WrappedObject);
                
                ReloadShopViewModels();
                
                RaisePropertyChanged("Shops");
            }
            catch (Exception ex)
            {
                throw new Exception("Fehler beim Ändern der Daten des Geschäfts", ex);
            }
        }

        private void SaveShopDatesAsNew(object obj)
        {
            try
            {
                _currentID = _DBConnection.AddShop(CurrentShopName);
                Shops = _DBConnection.GetAllShops();
                ReloadShopViewModels();
                RaisePropertyChanged("Shops");
            }
            catch (Exception ex)
            {
                throw new Exception("Fehler beim Anlegen des neuen Geschäfts", ex);
            }
        }



        private bool CanUpdateShopDates(object obj)
        {
            bool retval = true;
            if (_currentShopName == null || _currentShopName == "" || ShopAlreadyExists())
                retval = false;
            return retval;
        }

        






        private bool ShopAlreadyExists()
        {
            bool retval = false;
            if (_allShops != null)
            {   
                if (_allShops.Where(x => x.Name.Equals(CurrentShopName)).FirstOrDefault() != null)
                    retval = true;
            }
            return retval;
        }

        #endregion

        
    }
}
