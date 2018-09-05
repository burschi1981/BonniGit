using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonniModel.Interfaces
{
    /// <summary>
    /// Interface für alles, was eine Kategorie hat (Bons und Zahlungen)
    /// </summary>
    public interface IHasCategories
    {
        /// <summary>
        /// Kategorien des Bons
        /// </summary>
        IList<ICategory> Categories { get; set; }
    }
}
