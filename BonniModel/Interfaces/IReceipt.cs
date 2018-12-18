using BonniModel.Interfaces;
using BonnyUI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BonnyUI
{
    /// <summary>
    /// Der gesamte Bon
    /// </summary>
    public interface IReceipt : IHasCategories
    {
        /// <summary>
        /// Benutzer, der den Bon bezahlt hat
        /// </summary>
        string User { get; set; }

        /// <summary>
        /// Betrag des Bons
        /// </summary>
        double Amount { get; set; }

        DateTime? PayDate { get; set; }

        string PaymentType { get; set; }

        IList<IPayment> Payments { get; set; }

        int? ID { get; set; }

        Shop Shop { get; set; }

        string Details { get; set; }

        bool Settled { get; set; }

        DateTime? SettlementDate { get; set; }
        
    }
}