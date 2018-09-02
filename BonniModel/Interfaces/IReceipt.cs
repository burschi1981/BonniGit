using BonniModel.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BonnyUI
{
    public interface IReceipt : IHasCategories
    {
        string User { get; set; }

        double Amount { get; set; }

        DateTime? PayDate { get; set; }

        string PaymentType { get; set; }

        IList<IPayment> Payments { get; set; }

        int? ID { get; set; }

        IShop Shop { get; set; }

        string Details { get; set; }

        bool Settled { get; set; }

        DateTime? SettlementDate { get; set; }
        
    }
}