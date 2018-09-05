using BonniModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonnyUI
{
    /// <summary>
    /// Enthält die Daten, wie sie in der Datenbank vorliegen.
    /// </summary>
    public interface IProject
    {
        /// <summary>
        /// Alle Bons des Projekts
        /// </summary>
        IList<IReceipt> Bons {get;set;}

        /// <summary>
        /// Alle Geschäfte des Projekts
        /// </summary>
        IList<IShop> Shops { get; set; }

        /// <summary>
        /// Alle Kategorien des Projekts
        /// </summary>
        IList<ICategory> Categories { get; set; }        
    }
}
