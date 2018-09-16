using BonnyUI.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BonnyUI.Model;

namespace BonnyUI.ViewModel
{
    public class BonAdminViewModel : ViewModelBase
    {
        #region Fields
        private IList<string> _allTypes = new List<string>() { Constants.PaymentTypes.PaidForBoth, Constants.PaymentTypes.PaidForSelf, Constants.PaymentTypes.PaidForTheOther };

        private IList<string> _users = new List<string>() { Constants.Users.Nina, Constants.Users.Marc };

        private BonListViewModel _openBons;

        private BonListViewModel _settledBons;

        private IProject _project;

        IDBConnector _dbConnection;
        #endregion



        #region ctor

        public BonAdminViewModel(IProject project, ShopAdminViewModel shopAdmin, IDBConnector dbConnection)
        {
            _dbConnection = dbConnection;
            _project = project;

            // offene Bons laden
            IList<IReceipt> openBons = _project.Bons.Where(x => !x.Settled).ToList();
            _openBons = new BonListViewModel(openBons, shopAdmin, dbConnection);
            _openBons.PropertyChanged -= BonsWereBalanced;
            _openBons.PropertyChanged += BonsWereBalanced;

            // geschlossene Bons laden
            IList<IReceipt> settledBons = _project.Bons.Where(x => x.Settled).ToList();
            _settledBons = new BonListViewModel(settledBons, shopAdmin, dbConnection);
        }

        #endregion



        public BonListViewModel OpenBons
        {
            get { return _openBons; }
        }

        public BonListViewModel SettledBons
        {
            get { return _settledBons; }
        }



        public void ReloadShopsInBons()
        {
            OpenBons.ReloadShopsInBons();
        }


        private void ReloadBons()
        {   
            _project.Bons.Clear();
            _project.Bons = _dbConnection.GetAllBons();
            _openBons.SetBons(_project.Bons.Where(x => !x.Settled).ToList());
            _settledBons.SetBons(_project.Bons.Where(x => x.Settled).ToList());
        }

        private void BonsWereBalanced(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Bons gespeichert") || e.PropertyName.Equals("All Bons"))
                ReloadBons();
        }

    }
}
