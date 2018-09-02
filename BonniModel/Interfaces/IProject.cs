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
        IList<IReceipt> Bons {get;set;}

        IList<IShop> Shops { get; set; }

        IList<ICategory> Categories { get; set; }

        //void ReloadBons();
    }
}
