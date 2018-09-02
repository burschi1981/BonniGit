using BonniModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonnyUI
{
    public interface IDBConnector
    {
        /// <summary>
        /// Fügt neues Geschäft in DB hinzu
        /// </summary>        
        int AddShop(string name);

        void ChangeShop(IShop shop);

        /// <summary>
        /// Liste aller Geschäfte
        /// </summary>
        /// <returns></returns>
        IList<IShop> GetAllShops();

        IList<ICategory> GetAllCategories();

        void ChangeCategory(ICategory category);


        int AddCategory(string name);

        /// <summary>
        /// Liefert die Liste aller Geschäfte
        /// </summary>
        /// <returns></returns>
        IList<IReceipt> GetAllBons();

        //void SaveBon(IBon bon);

        int SaveBon(IReceipt bon);

        int BalanceBon(IReceipt bon);
    }
}
