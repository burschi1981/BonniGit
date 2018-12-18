using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BonniModel.Interfaces;

namespace BonnyUI.Model
{
    /// <summary>
    /// Projekt: Modelobjekt, dass die Liste der Bons, die Liste der Kategorien und die Liste der Geschäfte enthält
    /// </summary>
    public class Project
    {
        #region private fields
        //private IDBConnector _dbConnection;
        private IList<IReceipt> _bons;
        private IList<Shop> _shops;
        private IList<ICategory> _categories;
        #endregion


        #region ctor
        
        public Project()
        { }
        #endregion

        #region public properties
        public IList<IReceipt> Bons
        {
            get { return _bons; }
            set { _bons = value; }
        }

        public IList<ICategory> Categories
        {
            get
            {
                return _categories;
            }

            set
            {
                _categories = value;
            }
        }

        public IList<Shop> Shops
        {
            get { return _shops; }
            set { _shops = value; }
        }

        #endregion
        
        
    }
}
