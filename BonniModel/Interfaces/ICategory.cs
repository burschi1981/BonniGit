using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonniModel.Interfaces
{
    /// <summary>
    /// Interface für die Kategorie der Bons
    /// </summary>
    public interface ICategory
    {
        /// <summary>
        /// ID in der Datenbank
        /// </summary>
        int ID { get; set; }

        /// <summary>
        /// Name der Kategorie
        /// </summary>
        string Name { get; set; }
    }
}