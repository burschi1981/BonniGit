using BonniModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonnyUI
{
    /// <summary>
    /// Einzelne Zahlung des Projekts
    /// </summary>
    public interface IPayment : IHasCategories
    {
        /// <summary>
        /// Betrag der Zahlung
        /// </summary>
        double Amount { get; set; }

        /// <summary>
        /// Details der Zahlung (optional)
        /// </summary>
        string Details { get; set; }

        /// <summary>
        /// Zahlungstyp
        /// </summary>
        string PaymentType { get; set; }

        /// <summary>
        /// ID der Zahlung in der DB
        /// </summary>
        int? ID { get; set; }
    }
}
