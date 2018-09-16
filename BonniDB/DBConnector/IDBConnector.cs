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
        /// Fügt neues Geschäft in DB hinzu und gibt die ID zurück
        /// </summary>        
        int AddShop(string name);


        /// <summary>
        /// Ändert Eckdaten eines bestehenden Geschäfts
        /// </summary>
        /// <param name="shop"></param>
        void ChangeShop(IShop shop);

        /// <summary>
        /// Liefert alle Geschäfte
        /// </summary>
        /// <returns></returns>
        IList<IShop> GetAllShops();

        /// <summary>
        /// Liefert alle Kategorien
        /// </summary>
        /// <returns></returns>
        IList<ICategory> GetAllCategories();

        /// <summary>
        /// Ändert eine Kategorie
        /// </summary>
        /// <param name="category"></param>
        void ChangeCategory(ICategory category);

        /// <summary>
        /// Fügt eine neue Kategorie hinzu und gibt die ID zurück
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        int AddCategory(string name);

        /// <summary>
        /// Liefert die Liste aller Geschäfte
        /// </summary>
        /// <returns></returns>
        IList<IReceipt> GetAllBons();
        
        /// <summary>
        /// Speichert den Bon und gibt die Kategorie zurück
        /// </summary>
        /// <param name="bon"></param>
        /// <returns></returns>
        int SaveBon(IReceipt bon);

        /// <summary>
        /// Gleicht den Bon in der DB aus
        /// </summary>
        /// <param name="bon"></param>
        /// <returns></returns>
        int BalanceBon(IReceipt bon);
    }
}
